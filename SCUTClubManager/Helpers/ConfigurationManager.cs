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
        private static string clubSplashPanelFolder = "~/Content/Images/ClubSplashPanels/";
        private static int inclinationsPerPerson = 3;
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
                    xml.WriteStartElement("SCUTClubManagerConfigFile");

                    xml.WriteElementString("IsRecruitEnabled", isRecruitEnabled.ToString());
                    xml.WriteElementString("ClubSplashPanelFolder", clubSplashPanelFolder.ToString());
                    xml.WriteElementString("InclinationsPerPerson", inclinationsPerPerson.ToString());

                    xml.WriteEndElement();

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
                if (reader.ReadToFollowing("ClubSplashPanelFolder"))
                {
                    string str = reader.ReadElementContentAsString().ToLower();
                    clubSplashPanelFolder = str;
                    string mapped_path = HttpContext.Current.Server.MapPath(str);

                    if (!Directory.Exists(mapped_path))
                    {
                        Directory.CreateDirectory(mapped_path);
                    }
                }
                if (reader.ReadToFollowing("InclinationsPerPerson"))
                {
                    inclinationsPerPerson = reader.ReadElementContentAsInt();
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

        public static int InclinationsPerPerson
        {
            get
            {
                return inclinationsPerPerson;
            }
            set
            {
                if (inclinationsPerPerson != value)
                {
                    using (XmlWriter xml = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory + ConfigFile, settings))
                    {
                        xml.WriteElementString("InclinationsPerPerson", value.ToString());

                        xml.Flush();
                        xml.Close();
                    }

                    inclinationsPerPerson = value;
                }
            }
        }

        public static string ClubSplashPanelFolder
        {
            get
            {
                return clubSplashPanelFolder;
            }
            set
            {
                if (clubSplashPanelFolder != value)
                {
                    using (XmlWriter xml = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory + ConfigFile, settings))
                    {
                        xml.WriteElementString("ClubSplashPanelFolder", value.ToString());

                        xml.Flush();
                        xml.Close();
                    }

                    clubSplashPanelFolder = value;
                }
            }
        }
    }
}