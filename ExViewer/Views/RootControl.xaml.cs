﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Data.Json;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;
using ExClient;
using Windows.ApplicationModel.Activation;
using Windows.UI.ViewManagement;
using ExViewer.ViewModels;
using System.Diagnostics;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace ExViewer.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class RootControl : Page
    {
        public RootControl()
        {
            this.InitializeComponent();
            tabs = new Dictionary<Controls.SplitViewTab, Type>()
            {
                [this.svt_Cache] = typeof(CachePage),
                [this.svt_Search] = typeof(SearchPage),
                [this.svt_Settings] = typeof(SettingsPage),
                [this.svt_About] = typeof(AboutPage)
            };

            pages = new Dictionary<Type, Controls.SplitViewTab>()
            {
                [typeof(CachePage)] = this.svt_Cache,
                [typeof(SearchPage)] = this.svt_Search,
                [typeof(SettingsPage)] = this.svt_Settings,
                [typeof(AboutPage)] = this.svt_About
            };
            sv_root.IsPaneOpen = false;
#if DEBUG
            this.GotFocus += OnGotFocus;
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(e.OriginalSource, "Focus state");
#endif
        }

        public Type HomePageType
        {
            get; set;
        }

        public ApplicationExecutionState PreviousState
        {
            get; set;
        }

        private readonly Dictionary<Controls.SplitViewTab, Type> tabs;
        private readonly Dictionary<Type, Controls.SplitViewTab> pages;

        public UserInfo UserInfo
        {
            get { return (UserInfo)GetValue(UserInfoProperty); }
            set { SetValue(UserInfoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UserInfo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserInfoProperty =
            DependencyProperty.Register("UserInfo", typeof(UserInfo), typeof(RootControl), new PropertyMetadata(null));

        SystemNavigationManager manager;

        private void btn_pane_Click(object sender, RoutedEventArgs e)
        {
            sv_root.IsPaneOpen = !sv_root.IsPaneOpen;
        }

        private void Control_Loading(FrameworkElement sender, object args)
        {
            RootController.SetRoot(this);
            manager = SystemNavigationManager.GetForCurrentView();
            manager.BackRequested += Manager_BackRequested;
            fm_inner.Navigate(HomePageType ?? typeof(SearchPage));
        }

        private async void Control_Loaded(object sender, RoutedEventArgs e)
        {
            UserInfo = await UserInfo.LoadFromCache();
            RootController.UpdateUserInfo(false);
        }

        private void Control_Unloaded(object sender, RoutedEventArgs e)
        {
            manager.BackRequested -= Manager_BackRequested;
        }

        private void Manager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if(fm_inner.CanGoBack && !RootController.ViewDisabled)
            {
                fm_inner.GoBack();
                e.Handled = true;
            }
        }

        private void fm_inner_Navigated(object sender, NavigationEventArgs e)
        {
            if(fm_inner.CanGoBack)
                manager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            else
                manager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            Controls.SplitViewTab tab;
            var pageType = fm_inner.Content.GetType();
            JYAnalyticsUniversal.JYAnalytics.TrackPageStart(pageType.ToString());
            if(this.pages.TryGetValue(pageType, out tab))
            {
                tab.IsChecked = true;
            }
        }

        private void fm_inner_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            var content = fm_inner.Content;
            if(content == null)
                return;
            var pageType = content.GetType();
            JYAnalyticsUniversal.JYAnalytics.TrackPageEnd(pageType.ToString());
            Controls.SplitViewTab tab;
            if(this.pages.TryGetValue(pageType, out tab))
            {
                tab.IsChecked = false;
            }
        }

        private void svt_Click(object sender, RoutedEventArgs e)
        {
            var s = (Controls.SplitViewTab)sender;
            if(s.IsChecked)
                return;
            fm_inner.Navigate(tabs[s]);
            sv_root.IsPaneOpen = !sv_root.IsPaneOpen;
        }

        private async void btn_ChangeUser_Click(object sender, RoutedEventArgs e)
        {
            await RootController.RequestLogOn();
        }
    }
}
