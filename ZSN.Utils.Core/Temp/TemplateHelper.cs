using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Collections;
using System.IO;


namespace Yannyo.Common
{
    public  class TemplateHelper
    {
        /// <summary>
        /// 私有构造方法，不允许创建实例
        /// </summary>
        private TemplateHelper()
        {
            // TODO: Add constructor logic here
        }

        /// <summary>
        /// Template File Helper 
        /// </summary>
        /// <param name="templatePath">Templet Path</param>
        /// <param name="values">NameValueCollection</param>
        /// <returns>string</returns>
        public static string BulidByFile(string templatePath, NameValueCollection values)
        {
            return BulidByFile(templatePath, values, "[$", "]");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="values">NameValueCollection obj</param>
        /// <param name="prefix"></param>
        /// <param name="postfix"></param>
        /// <returns></returns>
        public static string Build(string template, NameValueCollection values, string prefix, string postfix)
        {
            if (values != null)
            {
                foreach (DictionaryEntry entry in values)
                {
                    template = template.Replace(string.Format("{0}{1}{2}", prefix, entry.Key, postfix), entry.Value.ToString());
                }
            }
            return template;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templatePath"></param>
        /// <param name="values"></param>
        /// <param name="prefix"></param>
        /// <param name="postfix"></param>
        /// <returns></returns>
        public static string BulidByFile(string templatePath, NameValueCollection values, string prefix, string postfix)
        {
            StreamReader reader = null;
            string template = string.Empty;
            try
            {
                reader = new StreamReader(templatePath);
                template = reader.ReadToEnd();
                reader.Close();
                if (values != null)
                {
                    foreach (string key in values.AllKeys)
                    {
                        template = template.Replace(string.Format("{0}{1}{2}", prefix, key, postfix), values[key]);
                    }
                }
            }
            catch
            {

            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return template;
        }


    }
}
