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
        private static string temporaryFilesFolder = "~/Content/Temp/";
        private static int maxRangeForRangeAdding = 100;
        private static int progressProfileInterval = 500;
        private static string templateFolder = "~/Content/TemplateFiles/";
        private static string eventApplicationTemplateFile = "EventApplicationTemplate";
        private static string eventPosterFolder = "~/Content/Images/EventPosters/";
        private static string eventPlanFolder = "~/Content/EventPlans/";
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
                    xml.WriteElementString("TemporaryFilesFolder", temporaryFilesFolder.ToString());
                    xml.WriteElementString("MaxRangeForRangeAdding", maxRangeForRangeAdding.ToString());
                    xml.WriteElementString("ProgressProfileInterval", progressProfileInterval.ToString());
                    xml.WriteElementString("TemplateFolder", templateFolder.ToString());
                    xml.WriteElementString("EventApplicationTemplateFile", eventApplicationTemplateFile.ToString());
                    xml.WriteElementString("EventPosterFolder", eventPosterFolder.ToString());
                    xml.WriteElementString("EventPlanFolder", eventPlanFolder.ToString());

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
                if (reader.ReadToFollowing("TemporaryFilesFolder"))
                {
                    string str = reader.ReadElementContentAsString().ToLower();
                    temporaryFilesFolder = str;
                    string mapped_path = HttpContext.Current.Server.MapPath(str);

                    if (!Directory.Exists(mapped_path))
                    {
                        Directory.CreateDirectory(mapped_path);
                    }
                }
                if (reader.ReadToFollowing("MaxRangeForRangeAdding"))
                {
                    maxRangeForRangeAdding = reader.ReadElementContentAsInt();
                }
                if (reader.ReadToFollowing("ProgressProfileInterval"))
                {
                    progressProfileInterval = reader.ReadElementContentAsInt();
                }       
                if (reader.ReadToFollowing("TemplateFolder"))
                {
                    string str = reader.ReadElementContentAsString().ToLower();
                    templateFolder = str;
                    string mapped_path = HttpContext.Current.Server.MapPath(str);

                    if (!Directory.Exists(mapped_path))
                    {
                        Directory.CreateDirectory(mapped_path);
                    }
                }
                if (reader.ReadToFollowing("EventApplicationTemplateFile"))
                {
                    eventApplicationTemplateFile = reader.ReadElementContentAsString();
                }
                if (reader.ReadToFollowing("EventPosterFolder"))
                {
                    string str = reader.ReadElementContentAsString().ToLower();
                    eventPosterFolder = str;
                    string mapped_path = HttpContext.Current.Server.MapPath(str);

                    if (!Directory.Exists(mapped_path))
                    {
                        Directory.CreateDirectory(mapped_path);
                    }
                }
                if (reader.ReadToFollowing("EventPlanFolder"))
                {
                    string str = reader.ReadElementContentAsString().ToLower();
                    eventPlanFolder = str;
                    string mapped_path = HttpContext.Current.Server.MapPath(str);

                    if (!Directory.Exists(mapped_path))
                    {
                        Directory.CreateDirectory(mapped_path);
                    }
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

        public static int MaxRangeForRangeAdding
        {
            get
            {
                return maxRangeForRangeAdding;
            }
            set
            {
                if (maxRangeForRangeAdding != value)
                {
                    using (XmlWriter xml = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory + ConfigFile, settings))
                    {
                        xml.WriteElementString("MaxRangeForRangeAdding", value.ToString());

                        xml.Flush();
                        xml.Close();
                    }

                    maxRangeForRangeAdding = value;
                }
            }
        }

        public static string TemporaryFilesFolder
        {
            get
            {
                return temporaryFilesFolder;
            }
            set
            {
                if (temporaryFilesFolder != value)
                {
                    using (XmlWriter xml = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory + ConfigFile, settings))
                    {
                        xml.WriteElementString("TemporaryFilesFolder", value.ToString());

                        xml.Flush();
                        xml.Close();
                    }

                    temporaryFilesFolder = value;
                }
            }
        }

        public static int ProgressProfileInterval
        {
            get
            {
                return progressProfileInterval;
            }
            set
            {
                if (progressProfileInterval != value)
                {
                    using (XmlWriter xml = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory + ConfigFile, settings))
                    {
                        xml.WriteElementString("ProgressProfileInterval", value.ToString());

                        xml.Flush();
                        xml.Close();
                    }

                    progressProfileInterval = value;
                }
            }
        }

        public static string TemplateFolder
        {
            get
            {
                return templateFolder;
            }
            set
            {
                if (templateFolder != value)
                {
                    using (XmlWriter xml = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory + ConfigFile, settings))
                    {
                        xml.WriteElementString("TemplateFolder", value.ToString());

                        xml.Flush();
                        xml.Close();
                    }

                    templateFolder = value;
                }
            }
        }

        public static string EventApplicationTemplateFile
        {
            get
            {
                return eventApplicationTemplateFile;
            }
            set
            {
                if (eventApplicationTemplateFile != value)
                {
                    using (XmlWriter xml = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory + ConfigFile, settings))
                    {
                        xml.WriteElementString("EventApplicationTemplateFile", value.ToString());

                        xml.Flush();
                        xml.Close();
                    }

                    eventApplicationTemplateFile = value;
                }
            }
        }

        public static string EventPosterFolder
        {
            get
            {
                return eventPosterFolder;
            }
            set
            {
                if (eventPosterFolder != value)
                {
                    using (XmlWriter xml = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory + ConfigFile, settings))
                    {
                        xml.WriteElementString("EventPosterFolder", value.ToString());

                        xml.Flush();
                        xml.Close();
                    }

                    eventPosterFolder = value;
                }
            }
        }

        public static string EventPlanFolder
        {
            get
            {
                return eventPlanFolder;
            }
            set
            {
                if (eventPlanFolder != value)
                {
                    using (XmlWriter xml = XmlWriter.Create(AppDomain.CurrentDomain.BaseDirectory + ConfigFile, settings))
                    {
                        xml.WriteElementString("EventPlanFolder", value.ToString());

                        xml.Flush();
                        xml.Close();
                    }

                    eventPlanFolder = value;
                }
            }
        }
    }
}