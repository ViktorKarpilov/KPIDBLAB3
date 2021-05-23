using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace KPI.DB.Client
{
    public class PageBase : IPageBase
    {
        private IJSRuntime js;
        public PageBase(IJSRuntime js)
        {
            this.js = js;
        }
        public async Task WriteCookies(string name, string value, int time = 45)
        {
            await js.InvokeAsync<object>("WriteCookie.WriteCookie", name, value, DateTime.Now.AddMinutes(time));
        }

        public async Task<string> ReadCookies(string name)
        {
            return await js.InvokeAsync<string>("ReadCookie.ReadCookie", name);
        }
    }
}
