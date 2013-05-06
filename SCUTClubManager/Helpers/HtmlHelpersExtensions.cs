using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace SCUTClubManager.Helpers
{
    public static class HtmlHelpersExtensions
    {
        public static string GenerateKeyPairFor(string url)
        {
            string key = Guid.NewGuid().ToString();
            HttpContext.Current.Session["PrivateKey" + url] = key;

            return key;
        }

        public static bool IsLegalAccessFrom(string url, string public_key)
        {
            string source = HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];
            string private_key = HttpContext.Current.Session["PrivateKey" + url] as string;

            return source != null && url != null && source.ToLower().Contains(url.ToLower()) 
                && private_key != null && public_key != null && private_key == public_key;
        }

        public static string RenameAndSaveFile(HttpPostedFileBase file, string folder, string file_name = null)
        {
            if (file != null && file.ContentLength > 0)
            {
                if (file_name == null)
                {
                    Guid guid = Guid.NewGuid();
                    file_name = guid.ToString();
                }

                string extension = "";
                if (Path.HasExtension(file.FileName))
                {
                    extension = Path.GetExtension(file.FileName);
                }

                file_name += extension;

                string path = Path.Combine(HttpContext.Current.Server.MapPath(folder), file_name);
                file.SaveAs(path);

                return file_name;
            }

            return null;
        }

        public static bool DeleteFileFrom(string folder, string file_name)
        {
            string path = Path.Combine(HttpContext.Current.Server.MapPath(folder), file_name);

            if (Directory.Exists(folder) && File.Exists(path))
            {
                File.Delete(path);

                return true;
            }

            return false;
        }

        public static MvcHtmlString ImageUploaderFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            TagBuilder container = new TagBuilder("div");
            TagBuilder sub_container1 = new TagBuilder("div");
            TagBuilder sub_container2 = new TagBuilder("div");
            TagBuilder image = new TagBuilder("img");
            TagBuilder input = new TagBuilder("input");

            MemberExpression exp = expression.Body as MemberExpression;
            string property_name = "";
            string property_value = "";

            if (exp != null)
            {
                var model = ModelMetadata.FromLambdaExpression(expression, html.ViewData).Model;

                property_name = exp.Member.Name;
                property_value = model == null ? "" : model.ToString();
            }

            input.Attributes.Add("type", "file");
            input.Attributes.Add("name", property_name);
            input.Attributes.Add("onchange", "imageUploader_valueChanged(this)");

            string path = Path.Combine(ConfigurationManager.EventPosterFolder, property_value);

            // 去掉'~'
            path = path.Substring(1);

            image.Attributes.Add("src", path);
            image.Attributes.Add("width", "320");
            image.Attributes.Add("height", "240");

            sub_container1.InnerHtml = image.ToString();
            sub_container2.InnerHtml = input.ToString();

            container.AddCssClass("ImageUploader");
            container.InnerHtml = sub_container1.ToString() + sub_container2.ToString();

            return MvcHtmlString.Create(container.ToString());
        }
    }
}