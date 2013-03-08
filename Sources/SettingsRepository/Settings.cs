using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SettingsRepository
{
    class Settings
    {
        Dictionary<string, int> numeric_settings;

        public Settings()
        {
            numeric_settings = new Dictionary<string, int>();
        }

        public int Get(string name)
        {
            return numeric_settings[name];
        }

        public void Set(string name, int value)
        {
            name.ToLower();
            if (numeric_settings.ContainsKey(name))
                numeric_settings[name] = value;
            else
                numeric_settings.Add(name, value);
        }

        public void DumpToFile(string file)
        {
            var writer = XmlTextWriter.Create(file);

            writer.WriteStartDocument(true);
            writer.WriteStartElement("settings");
            SaveAllRecords(ref writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Close();
        }

        private void SaveAllRecords(ref XmlWriter writer)
        {
            foreach (var record in numeric_settings)
            {   
                writer.WriteStartElement(record.Key);
                writer.WriteAttributeString("val", XmlConvert.ToString(record.Value));
                writer.WriteEndElement();
            }
        }
        
        public bool LoadFromFile(string file)
        {
            try
            {
                XmlTextReader reader = new XmlTextReader(file);
                reader.Read();
                while (reader.Read())
                {
                    if (reader.Name == "settings") continue;
                    numeric_settings.Add(reader.Name, XmlConvert.ToInt32(reader.GetAttribute(0)));
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
