using System;
using System.Web.Mvc;

namespace EMS.Web.Website.Models
{
    public static class HtmlExtensions
    {
        public static string Image(this HtmlHelper html, byte[] image)
        {
            return $"data:image/jpg;base64,{Convert.ToBase64String(image)}";
        }
    }
}