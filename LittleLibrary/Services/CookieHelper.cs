using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleLibrary.Services
{
    public class CookieHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpResponse _response;
        private readonly HttpRequest _request;

        // The controller is set up to parse http requests so set up constructor to receive
        // these values.
        public CookieHelper(IHttpContextAccessor httpContextAccessor, HttpRequest request,
                            HttpResponse response)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._response = response;
            this._request = request;
        }

        public string Get(string key)
        {
            return _request.Cookies[key];
        }

        public void Set(string key, string value, int expireDays)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(expireDays);
            _response.Cookies.Append(key, value, option);
        }

        public void Remove(string key)
        {
            _response.Cookies.Delete(key);
        }
    }

}
