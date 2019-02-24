using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ES_WEBKYSO.Common
{
    public static class Utility
    {
        public static string getXMLPath()
        {
            string strXMLPath = System.Configuration.ConfigurationManager.AppSettings["PATH_FILEXML"].ToString();
            return strXMLPath;
        }

        public static string getAPI_IMG()
        {
            string strXMLPath = System.Configuration.ConfigurationManager.AppSettings["API_IMG"].ToString();
            return strXMLPath;
        }

        public static string getAPI_PATH()
        {
            string strXMLPath = System.Configuration.ConfigurationManager.AppSettings["API_PATH"].ToString();
            return strXMLPath;
        }

        public static List<ImportIncludeAttribute> AllPropertieInclude<T>() where T : class
        {
            var lst = new List<ImportIncludeAttribute>();

            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                var importIncAttr =
                    prop
                    .GetCustomAttributes(typeof(ImportIncludeAttribute), true)
                    .FirstOrDefault() as ImportIncludeAttribute;
                if (importIncAttr != null)
                {
                    var getDisplayAttr =
                        prop
                        .GetCustomAttributes(typeof(DisplayAttribute), true)
                        .FirstOrDefault() as DisplayAttribute;

                    importIncAttr.DisplayName = getDisplayAttr == null ? prop.Name : getDisplayAttr.Name;
                    importIncAttr.Name = prop.Name;
                    importIncAttr.AllowNull = Nullable.GetUnderlyingType(prop.PropertyType) == null;

                    if (importIncAttr.Type == null)
                    {
                        importIncAttr.Type = prop.PropertyType.GetGenericArguments().FirstOrDefault() ?? prop.PropertyType;
                    }

                    if (importIncAttr.CssName == null)
                    {
                        importIncAttr.CssName = "dt-left";

                        if (importIncAttr.Type == typeof(int) ||
                            importIncAttr.Type == typeof(Single) ||
                            importIncAttr.Type == typeof(float) ||
                            importIncAttr.Type == typeof(decimal) ||
                            importIncAttr.Type == typeof(double))
                        {
                            importIncAttr.CssName = "dt-right";
                        }

                        if (importIncAttr.Type == typeof(DateTime))
                        {
                            importIncAttr.CssName = "center";
                        }
                    }

                    lst.Add(importIncAttr);
                }
            }

            return lst.OrderBy(x => x.Order).ToList();
        }

        /// <summary>
        /// Trộn 2 thực thể cùng kiểu T
        /// </summary>
        /// <param name="newModel"></param>
        /// <param name="curModel"></param>
        /// <param name="allowNotMapped">Cho phép trộn các trường notmap</param>
        /// <returns></returns>
        public static T ApplyChange<T>(T newModel, T curModel, bool allowNotMapped = false) where T : class
        {
            var objects = (T)newModel;
            var properties = objects.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var keyAttr = prop
                    .GetCustomAttributes(typeof(KeyAttribute), true)
                    .Cast<KeyAttribute>()
                    .FirstOrDefault();
                var mapAttr = prop
                    .GetCustomAttributes(typeof(NotMappedAttribute), true)
                    .Cast<NotMappedAttribute>()
                    .FirstOrDefault();
                if (keyAttr == null && !prop.GetGetMethod().IsVirtual && (allowNotMapped || mapAttr == null))
                {
                    var curValue = curModel.GetType().GetProperty(prop.Name).GetValue(curModel, null);
                    prop.SetValue(
                        newModel,
                        curValue,
                        null
                    );
                }
            }

            return newModel;
        }
    }
}