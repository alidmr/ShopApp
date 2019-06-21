using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ShopApp.UI.Models.Product;

namespace ShopApp.UI.TagHelpers
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        [HtmlAttributeName]
        public PageInfo PageModel { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";

            var sb = new StringBuilder();
            sb.Append("<ul class='pagination'>");
            for (int i = 1; i <= PageModel.TotalPages(); i++)
            {
                sb.AppendFormat("<li class='page-item {0}'>", i == PageModel.CurrentPage ? "active" : "");
                if (string.IsNullOrEmpty(PageModel.CurrentCategory))
                {
                    sb.AppendFormat("<a class='page-link' href='/products?page={0}'>{0}</a>", i);
                }
                else
                {
                    sb.AppendFormat("<a class='page-link' href='/products/{1}?page={0}'>{0}</a>", i, PageModel.CurrentCategory);
                }

                sb.Append("</li>");
            }

            sb.Append("</ul>");

            output.Content.SetHtmlContent(sb.ToString());
            base.Process(context, output);
        }
    }
}
