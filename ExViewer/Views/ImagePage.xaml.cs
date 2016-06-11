﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Color = Windows.UI.Color;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using ExViewer.Settings;
using ExViewer.ViewModels;
using ImageLib;
using Windows.Storage.Streams;
using ImageLib.Gif;
using ExClient;
using Windows.UI.Xaml.Media.Imaging;
using ExViewer.Controls;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace ExViewer.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ImagePage : Page
    {
        public ImagePage()
        {
            this.InitializeComponent();
            var backColor = ((SolidColorBrush)Resources["ApplicationPageBackgroundThemeBrush"]).Color;
            var needColor = (Color)Resources["SystemChromeMediumColor"];
            var toColor = Color.FromArgb(74,
                (byte)(backColor.R - 2 * (backColor.R - needColor.R)),
                (byte)(backColor.G - 2 * (backColor.G - needColor.G)),
                (byte)(backColor.B - 2 * (backColor.B - needColor.B)));

            cb_top.Background = new SolidColorBrush(toColor);
            cb_top_OpenAnimation.To = needColor;
            cb_top_CloseAnimation.To = toColor;
        }

        public GalleryVM VM
        {
            get
            {
                return (GalleryVM)GetValue(VMProperty);
            }
            set
            {
                SetValue(VMProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for VM.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VMProperty =
            DependencyProperty.Register("VM", typeof(GalleryVM), typeof(ImagePage), new PropertyMetadata(null));

        ApplicationView av = ApplicationView.GetForCurrentView();

        private void btn_pane_Click(object sender, RoutedEventArgs e)
        {
            cb_top.IsOpen = false;
            RootControl.RootController.SwitchSplitView();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            cb_top.Visibility = Visibility.Visible;
            VM = await GalleryVM.GetVMAsync((long)e.Parameter);
            av.VisibleBoundsChanged += Av_VisibleBoundsChanged;
            Av_VisibleBoundsChanged(av, null);
            if(VM.CurrentIndex == -1 && Frame.CanGoBack)
                Frame.GoBack();
            await Task.Yield();
            fv.Focus(FocusState.Pointer);
            fv.SelectedIndex = VM.CurrentIndex;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            VM.CurrentIndex = fv.SelectedIndex;
            av.VisibleBoundsChanged -= Av_VisibleBoundsChanged;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            VM = null;
            isFullScreen = null;
        }

        bool? isFullScreen;

        private void Av_VisibleBoundsChanged(ApplicationView sender, object args)
        {
            switch(isFullScreen)
            {
            case true:
                if(av.IsFullScreenMode)
                    return;
                break;
            case false:
                if(!av.IsFullScreenMode)
                    return;
                break;
            }
            if(av.IsFullScreenMode)
            {
                abb_fullScreen.Icon = new SymbolIcon(Symbol.BackToWindow);
                abb_fullScreen.Label = "Back to window";
            }
            else
            {
                abb_fullScreen.Icon = new SymbolIcon(Symbol.FullScreen);
                abb_fullScreen.Label = "Full screen";
            }
            isFullScreen = av.IsFullScreenMode;
        }

        private void setScale()
        {
            int lb = fv.SelectedIndex - 1;
            int ub = fv.SelectedIndex + 2;
            lb = lb < 0 ? 0 : lb;
            ub = ub > VM.Gallery.Count ? VM.Gallery.Count : ub;
            for(int i = lb; i < ub; i++)
            {
                if(i == fv.SelectedIndex)
                    continue;
                var selected = (FlipViewItem)fv.ContainerFromIndex(i);
                if(selected == null)
                    continue;
                var inner = (Grid)selected.ContentTemplateRoot;
                if(inner == null)
                    continue;
                var ip = (ImagePresenter)inner.FindName("ip");
                ip.ResetScale();
            }
        }

        IAsyncOperation<LoadMoreItemsResult> loadItems;

        private void fv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(VM?.Gallery == null)
                return;
            var start = fv.SelectedIndex;
            if(start < 0)
                return;
            var end = start + 5;
            if(end > VM.Gallery.Count)
            {
                end = VM.Gallery.Count;
            }
            if(end + 10 > VM.Gallery.Count && VM.Gallery.HasMoreItems)
            {
                if(loadItems == null || loadItems.Status != AsyncStatus.Started)
                    loadItems = VM.Gallery.LoadMoreItemsAsync(5);
            }
            for(int i = start; i < end; i++)
            {
                var ignore = VM.Gallery[i].LoadImageAsync(false, SettingCollection.Current.GetStrategy(), false);
            }
            setScale();
        }

        private System.Threading.CancellationTokenSource changeCbVisibility;

        private async void fvi_Tapped(object sender, TappedRoutedEventArgs e)
        {
            changeCbVisibility = new System.Threading.CancellationTokenSource();
            await Task.Delay(SettingCollection.Current.ChangeCommandBarDelay, this.changeCbVisibility.Token).ContinueWith(async t =>
            {
                if(t.IsCanceled)
                    return;
                await this.cb_top.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    this.changeCbVisibility.Cancel();
                    switch(this.cb_top.Visibility)
                    {
                    case Visibility.Visible:
                        this.cb_top.Visibility = Visibility.Collapsed;
                        break;
                    case Visibility.Collapsed:
                        this.cb_top.Visibility = Visibility.Visible;
                        break;
                    }
                });
            });
        }

        private void fvi_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if(e.Handled)
                return;
            if(changeCbVisibility != null)
            {
                if(changeCbVisibility.IsCancellationRequested)
                    switch(cb_top.Visibility)
                    {
                    case Visibility.Visible:
                        cb_top.Visibility = Visibility.Collapsed;
                        break;
                    case Visibility.Collapsed:
                        cb_top.Visibility = Visibility.Visible;
                        break;
                    }
                else
                    changeCbVisibility.Cancel();
            }
            var fvi = (FlipViewItem)fv.ContainerFromIndex(fv.SelectedIndex);
            var gd = (Grid)fvi?.ContentTemplateRoot;
            var ip = (ImagePresenter)gd?.FindName("ip");
            ip?.ZoomTo(e);
            e.Handled = true;
        }

        private async void Flyout_Opening(object sender, object e)
        {
            VM.CurrentIndex = fv.SelectedIndex;
            await VM.RefreshInfoAsync();
        }

        private void cb_top_Opening(object sender, object e)
        {
            cb_top_Open.Begin();
        }

        private void cb_top_Closing(object sender, object e)
        {
            cb_top_Close.Begin();
        }

        private void abb_fullScreen_Click(object sender, RoutedEventArgs e)
        {
            if(av.IsFullScreenMode)
            {
                av.ExitFullScreenMode();
            }
            else
            {
                av.TryEnterFullScreenMode();
            }
        }

    }
}
