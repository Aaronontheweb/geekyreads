using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace geekwall.Helpers
{
    public static class CacheExtensions
    {
        public static bool IsEmpty(this Cache cache, string key)
        {
            var result = cache[key];
            return (result == null);
        }


        public static T GetOrStore<T>(this Cache cache, string key, Func<T> generator)
        {
            var result = cache[key]; 
            if (result == null)
            {
                result = generator(); 
                cache[key] = result;
            } return (T)result;
        }
    }
}