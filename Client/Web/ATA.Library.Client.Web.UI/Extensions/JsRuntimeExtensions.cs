﻿using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace ATA.Library.Client.Web.UI.Extensions
{
    public static class JsRuntimeExtensions
    {
        public static async Task AlertAsync(this IJSRuntime jsRuntime, string alertMessage)
        {
            await jsRuntime.InvokeVoidAsync("alert", alertMessage);
        }

        public static async Task SetCookieAsync(this IJSRuntime jsRuntime, string cookieName, string cookieValue, int expireDays = 365)
        {
            await jsRuntime.InvokeVoidAsync("setCookie", cookieName, cookieValue, expireDays);
        }

        public static async Task<string> GetCookieAsync(this IJSRuntime jsRuntime, string cookieName)
        {
            return await jsRuntime.InvokeAsync<string>("getCookie", cookieName);
        }

        public static async Task NavigateToUrlInNewTab(this IJSRuntime jsRuntime, string url)
        {
            await jsRuntime.InvokeVoidAsync("open", url, "_blank");
        }

        public static async Task SetLayoutTitle(this IJSRuntime jsRuntime, string title)
        {
            await jsRuntime.InvokeVoidAsync("setLayoutTitle", title);
        }
    }
}