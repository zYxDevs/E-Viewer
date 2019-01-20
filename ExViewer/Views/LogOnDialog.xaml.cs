﻿using ExClient;
using ExViewer.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Opportunity.MvvmUniverse;
using Opportunity.MvvmUniverse.Commands;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using static ExViewer.Helpers.HtmlHelper;

// “内容对话框”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上进行了说明

namespace ExViewer.Views
{
    public sealed partial class LogOnDialog : MyContentDialog
    {
        public LogOnDialog()
        {
            this.InitializeComponent();
        }

        private class VMData : ObservableObject
        {
            public VMData()
            {
                this.LogOn = AsyncCommand.Create(async s =>
                {
                    if (!long.TryParse(MemberId, out var uid))
                        return;
                    var hash = PassHash;

                    try
                    {
                        await Client.Current.LogOnAsync(uid, hash);
                    }
                    catch (Exception ex)
                    {
                        Reset();
                        this.ErrorMsg = ex.Message;
                        this.ShowErrorMsg = true;
                        Telemetry.LogException(ex);
                        return;
                    }
                    if (!string.IsNullOrEmpty(this.UserName) && !string.IsNullOrEmpty(this.Password))
                        AccountManager.CurrentCredential = AccountManager.CreateCredential(this.UserName, this.Password);

                    this.Succeed = true;
                }, s => CanLogOn);
                this.LogOn.PropertyChanged += (s, e) => OnPropertyChanged(nameof(IsPrimaryButtonEnabled));
            }

            public LogOnInfo LogOnInfoBackup { get; } = Client.Current.GetLogOnInfo();

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool _UseCookieLogOn;
            public bool UseCookieLogOn { get => this._UseCookieLogOn; set => Set(nameof(PrimaryButtonText), nameof(IsPrimaryButtonEnabled), ref this._UseCookieLogOn, value); }

            public AsyncCommand LogOn { get; }

            public string PrimaryButtonText => this._UseCookieLogOn ? "Log on" : "Reset";

            public bool IsPrimaryButtonEnabled => this._UseCookieLogOn ? CanLogOn && !LogOn.IsExecuting : !LogOn.IsExecuting;

            public bool CanLogOn => !Succeed && long.TryParse(MemberId, out _) && Regex.IsMatch(PassHash ?? "", @"^[0-9a-fA-F]{32}$");

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool _Succeed;
            public bool Succeed
            {
                get => this._Succeed;
                set => Set(nameof(CanLogOn), nameof(IsPrimaryButtonEnabled), ref this._Succeed, value);
            }

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string _UserName;
            public string UserName { get => this._UserName; set => Set(ref this._UserName, value); }

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string _Password;
            public string Password { get => this._Password; set => Set(ref this._Password, value); }

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string _MemberId;
            public string MemberId
            {
                get => this._MemberId;
                set => Set(nameof(CanLogOn), nameof(IsPrimaryButtonEnabled), ref this._MemberId, value);
            }

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string _PassHash;
            public string PassHash
            {
                get => this._PassHash;
                set => Set(nameof(CanLogOn), nameof(IsPrimaryButtonEnabled), ref this._PassHash, value);
            }

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string _ErrorMsg;
            public string ErrorMsg { get => this._ErrorMsg; private set => Set(ref this._ErrorMsg, value); }

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool _ShowErrorMsg;
            public bool ShowErrorMsg { get => this._ShowErrorMsg; set => Set(ref this._ShowErrorMsg, value); }


            public void Reset()
            {
                this.Succeed = false;

                this.UserName = null;
                this.Password = null;

                this.MemberId = "";
                this.PassHash = "";
            }
        }

        private readonly VMData VM = new VMData();

        public bool Succeed => this.VM.Succeed;

        private void btnUseCookie_Click(object sender, RoutedEventArgs e)
        {
            this.VM.UseCookieLogOn = true;
            reset();
        }

        private void btnUseWebpage_Click(object sender, RoutedEventArgs e)
        {
            this.VM.UseCookieLogOn = false;
            reset();
        }

        private async void reset()
        {
            this.VM.Reset();

            this.wv.NavigateToString("");
            await Dispatcher.YieldIdle();
            this.wv.Navigate(Client.LogOnUri);
        }

        private async Task injectLogOnPage()
        {
            var pass = AccountManager.CurrentCredential;
            var u = "";
            var p = "";
            if (pass != null)
            {
                pass.RetrievePassword();
                u = escape(pass.UserName);
                p = escape(pass.Password);
            }
            await this.wv.InvokeScriptAsync("eval", new[] { $@"
(function ()
{{
    var nL = document.LOGIN;
    if(!nL) return;
	var nU = nL.UserName;
    if(!nU) return;
    var nP = nL.PassWord;
    if(!nP) return;
    nU.value = '{u}';
    nP.value = '{p}';
    nL.onsubmit = function(ev)
    {{
        var ret = ValidateForm();
        if (ret)
        {{
            window.external.notify(nU.value + '\n' + nP.value);
        }}
        return ret;
    }}
}})();
" });
            string escape(string value)
            {
                return value.Replace(@"\", @"\\").Replace("'", @"\'");
            }
        }

        private async Task injectOtherPage()
        {
            if (this.VM.LogOn.IsExecuting)
                return;

            var r = await this.wv.InvokeScriptAsync("eval", new[] { @"
(function ()
{
    function getCookie(c_name)
    {
        if (document.cookie.length <= 0) return '';
        var c_start = document.cookie.indexOf(c_name + '=');
        if (c_start < 0) return '';
        c_start = c_start + c_name.length + 1;
        var c_end = document.cookie.indexOf(';', c_start);
        if (c_end == -1) c_end = document.cookie.length;
        return unescape(document.cookie.substring(c_start, c_end));
    }
    return (getCookie('ipb_member_id') + '\n' + getCookie('ipb_pass_hash'));
})();
" });
            var data = r.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (data.Length != 2)
            {
                return;
            }
            this.VM.MemberId = data[0];
            this.VM.PassHash = data[1];
            this.VM.LogOn.Execute();
        }

        private void cd_Loaded(object sender, RoutedEventArgs e)
        {
            if (Client.Current.NeedLogOn)
                this.CloseButtonText = Strings.Resources.General.Exit;
            else
                this.CloseButtonText = Strings.Resources.General.Cancel;
        }

        private void cd_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {
            reset();
        }

        private async void wv_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            Debug.WriteLine(args.Uri?.ToString() ?? "local string", "WebView");
            if (args.Uri is null)
                return;

            if (args.Uri == Client.LogOnUri)
            {
                await injectLogOnPage();
            }
            else if (args.Uri.ToString().StartsWith(Client.LogOnUri.ToString()))
            {
                await injectLogOnPage();
                await injectOtherPage();
            }
            else if (args.Uri.Host == Client.LogOnUri.Host)
            {
                await injectOtherPage();
            }
        }

        private void wv_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            this.wv.NavigateToString($@"
<html>
<head>
    <meta name='viewport' content='width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no' />
</head>
<body style='background:{Color((SolidColorBrush)this.Background)}; font-family: sans-serif;'>
    <div>
        <p style='color:red;'>
            {(int)e.WebErrorStatus} ({e.WebErrorStatus.ToString()})
        </p>
        <small style='color:{Color((SolidColorBrush)this.Foreground)}'>
            {e.Uri}
        </small>
    </div>
</body>
</html>");
        }

        private void wv_ScriptNotify(object sender, NotifyEventArgs e)
        {
            var data = e.Value.Split(new[] { '\n' }, StringSplitOptions.None);
            if (e.CallingUri.ToString().StartsWith(Client.LogOnUri.ToString()))
            {
                if (data.Length != 2)
                {
                    return;
                }

                this.VM.UserName = data[0];
                this.VM.Password = data[1];
                return;
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var ww = Window.Current.Bounds.Width;
            if (ww > 400)
                this.wv.MinWidth = Math.Min(availableSize.Width - 144, 700);
            else
                this.wv.MinWidth = 0;

            return base.MeasureOverride(availableSize);
        }

        private void cd_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;
            if (this.VM.UseCookieLogOn)
            {
                this.VM.LogOn.Execute();
            }
            else
            {
                reset();
            }
        }

        private void cd_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (this.VM.LogOn.IsExecuting)
            {
                args.Cancel = true;
                return;
            }
            if (this.VM.LogOnInfoBackup != null)
                Client.Current.RestoreLogOnInfo(this.VM.LogOnInfoBackup);
            if (Client.Current.NeedLogOn)
                Application.Current.Exit();
        }

        private void cookie_TextChanged(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text != "")
                this.VM.ShowErrorMsg = false;
        }
    }
}
