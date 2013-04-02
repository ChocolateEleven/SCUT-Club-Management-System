using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;

namespace SCUTClubManager.Helpers
{
    public static class ConfigurationManager
    {
        private static bool isRecruitEnabled = false;
        private static XmlWriterSettings settings = null;

        public static string ConfigFile
        {
            get
            {
                return "ScmConfig.xml";
            }
        }

        public static void Initialize()
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + ConfigFile))
            {
                settings = new XmlWriterSettings();
                settings.Indent = true;

                using (XmlWriter xml = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory + ConfigFile, settings))
                {
                    xml.WriteElementString("IsRecruitEnabled", "false");

                    xml.Flush();
                    xml.Close();
                }
            }

            using (XmlReader reader = XmlReader.Create(AppDomain.CurrentDomain.BaseDirectory + ConfigFile))
            {
                if (reader.ReadToFollowing("IsRecruitEnabled"))
                {
                    string str = reader.ReadElementContentAsString().ToLower();
                    isRecruitEnabled = Boolean.Parse(str);
                }
            }
        }

        public static bool IsRecruitEnabled
        {
            get
            {
                return isRecruitEnabled;
            }
            set
            {
                if (isRecruitEnabled != value)
                {
                    using (XmlWriter xml = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory + ConfigFile, settings))
                    {
                        xml.WriteElementString("IsRecruitEnabled", value.ToString());

                        xml.Flush();
                        xml.Close();
                    }

                    isRecruitEnabled = value;
                }
            }
        }
    }
}