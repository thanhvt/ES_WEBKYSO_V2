// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Html.LabelExtensions
// Assembly: System.Web.Mvc, Version=   , Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: CC73190B-AB9D-435C-8315-10FF295C572A
// Assembly location: E:\Public\BCSX\bin\System.Web.Mvc.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace ES_WEBKYSO.HtmlHelpers
{
    public static class LabelExtensionsNb
    {
        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return html.LabelFor<TModel, TValue>(expression, (string)null, htmlAttributes, (ModelMetadataProvider)null);
        }

        internal static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, object htmlAttributes, ModelMetadataProvider metadataProvider)
        {
            return html.LabelFor<TModel, TValue>(expression, labelText, (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), metadataProvider);
        }

        internal static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, IDictionary<string, object> htmlAttributes, ModelMetadataProvider metadataProvider)
        {
            return LabelExtensionsNb.LabelHelper((HtmlHelper)html, ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData), ExpressionHelper.GetExpressionText((LambdaExpression)expression), labelText, htmlAttributes);
        }

        internal static MvcHtmlString LabelHelper(HtmlHelper html, ModelMetadata metadata, string htmlFieldName, string labelText = null, IDictionary<string, object> htmlAttributes = null)
        {
            string str = labelText;
            if (str == null)
            {
                string displayName = metadata.DisplayName;
                if (displayName == null)
                {
                    string propertyName = metadata.PropertyName;
                    if (propertyName == null)
                        str = ((IEnumerable<string>)htmlFieldName.Split('.')).Last<string>();
                    else
                        str = propertyName;
                }
                else
                    str = displayName;
            }
            string innerText = str;
            if (string.IsNullOrEmpty(innerText))
                return MvcHtmlString.Empty;

            if (metadata.IsRequired)
            {
                if (htmlAttributes == null)
                {
                    htmlAttributes = new Dictionary<string, object>()
                    {
                        {"class", "label-required"}
                    };
                }
                else
                {
                    if (htmlAttributes.ContainsKey("class"))
                    {
                        htmlAttributes["class"] += " label-required";
                    }
                    else
                    {
                        htmlAttributes.Add("class", "label-required");
                    }
                }
            }

            TagBuilder tagBuilder = new TagBuilder("label");
            tagBuilder.Attributes.Add("for", TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
            tagBuilder.SetInnerText(innerText);
            tagBuilder.MergeAttributes<string, object>(htmlAttributes, true);

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));

        }
    }
}