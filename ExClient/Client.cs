﻿using ExClient.Api;
using ExClient.Settings;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using static System.Runtime.InteropServices.WindowsRuntime.AsyncInfo;
using ExClient.Internal;

namespace ExClient
{
    public partial class Client : IDisposable
    {
        public static Client Current
        {
            get;
        } = new Client();

        internal UriProvieder Uris => Host == HostType.Exhentai && HasPermittionForEx ? UriProvieder.Ex : UriProvieder.Eh;

        public HostType Host { get; set; } = HostType.Exhentai;

        public bool HasPermittionForEx
        {
            get
            {
                var ck = this.CookieManager.GetCookies(UriProvieder.Ex.RootUri).FirstOrDefault(c => c.Name == "igneous");
                if(ck == null)
                    return false;
                return ck.Value != "mystery";
            }
        }

        private Client()
        {
            var httpFilter = new HttpBaseProtocolFilter { AllowAutoRedirect = false };
            this.CookieManager = httpFilter.CookieManager;
            this.CookieManager.SetCookie(new HttpCookie("nw", "e-hentai.org", "/") { Value = "1" });
            this.HttpClient = new MyHttpClient(this, new HttpClient(new RedirectFilter(httpFilter)));
            this.Settings = new SettingCollection(this);
            this.Favorites = new FavoriteCollection(this);
        }

        internal HttpCookieManager CookieManager
        {
            get;
        }

        internal Internal.MyHttpClient HttpClient
        {
            get;
        }

        public bool NeedLogOn
            => this.CookieManager.GetCookies(UriProvieder.Eh.RootUri).Count < 2
            && this.CookieManager.GetCookies(UriProvieder.Ex.RootUri).Count < 2;

        public IAsyncOperation<Client> LogOnAsync(string userName, string password, ReCaptcha reCaptcha)
        {
            if(string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException(nameof(userName));
            if(string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));
            return Run(async token =>
            {
                if(this.disposedValue)
                    throw new InvalidOperationException(LocalizedStrings.Resources.ClientDisposed);
                var cookieBackUp = getLogOnInfo();
                ClearLogOnInfo();
                IEnumerable<KeyValuePair<string, string>> getParams()
                {
                    yield return new KeyValuePair<string, string>("CookieDate", "1");
                    yield return new KeyValuePair<string, string>("UserName", userName);
                    yield return new KeyValuePair<string, string>("PassWord", password);
                    if(reCaptcha?.Answer != null)
                    {
                        yield return new KeyValuePair<string, string>("recaptcha_challenge_field", reCaptcha.Answer);
                        yield return new KeyValuePair<string, string>("recaptcha_response_field", "manual_challenge");
                    }
                }
                try
                {
                    var log = await this.HttpClient.PostAsync(logOnUri, new HttpFormUrlEncodedContent(getParams()));
                    var html = new HtmlDocument();
                    using(var stream = await log.Content.ReadAsInputStreamAsync())
                    {
                        html.Load(stream.AsStreamForRead());
                    }
                    var errorNode = html.DocumentNode.Descendants("span").Where(node => node.GetAttributeValue("class", "") == "postcolor").FirstOrDefault();
                    if(errorNode != null)
                    {
                        var errorText = errorNode.InnerText;
                        switch(errorText)
                        {
                        case "Username or password incorrect":
                            errorText = LocalizedStrings.Resources.WrongAccountInfo;
                            break;
                        case "The captcha was not entered correctly. Please try again.":
                            errorText = LocalizedStrings.Resources.WrongCaptcha;
                            break;
                        }
                        throw new InvalidOperationException(errorText);
                    }
                    var init = await HttpClient.GetAsync(UriProvieder.Ex.RootUri, HttpCompletionOption.ResponseHeadersRead);
                    return this;
                }
                catch(Exception)
                {
                    ClearLogOnInfo();
                    foreach(var item in cookieBackUp)
                    {
                        CookieManager.SetCookie(item);
                    }
                    throw;
                }
            });
        }

        private List<HttpCookie> getLogOnInfo()
        {
            return CookieManager.GetCookies(UriProvieder.Eh.RootUri).Concat(CookieManager.GetCookies(UriProvieder.Ex.RootUri)).ToList();
        }

        public void ClearLogOnInfo()
        {
            foreach(var item in CookieManager.GetCookies(UriProvieder.Eh.RootUri))
            {
                CookieManager.DeleteCookie(item);
            }
            foreach(var item in CookieManager.GetCookies(UriProvieder.Ex.RootUri))
            {
                CookieManager.DeleteCookie(item);
            }
        }

        public int UserID
        {
            get
            {
                var cookie = CookieManager.GetCookies(UriProvieder.Eh.RootUri).FirstOrDefault(c => c.Name == "ipb_member_id");
                if(cookie == null)
                    return -1;
                return int.Parse(cookie.Value);
            }
        }

        public SettingCollection Settings { get; }

        public FavoriteCollection Favorites { get; }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if(!disposedValue)
            {
                if(disposing)
                {
                    // 释放托管状态(托管对象)。
                }

                // 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // 将大型字段设置为 null。
                HttpClient.Dispose();

                disposedValue = true;
            }
        }

        // 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~Client() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
