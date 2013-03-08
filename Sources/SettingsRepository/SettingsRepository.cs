using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SettingsRepository
{
    /// <summary>
    /// Class which can read and write settings
    /// </summary>
    public class SettingsRepository
    {
        string file;
        bool isCorrect = false;
        Settings current;

        public bool Correct { get { return isCorrect; } }


        public SettingsRepository(string file_)
        {
            file = file_;
            current = new Settings();
            if (current.LoadFromFile(file))
            {
                isCorrect = true;
            }
        }

        /// <summary>
        /// returns value associated to this record 
        /// </summary>
        /// <param name="name">name of record</param>
        /// <returns>only int for know</returns>
        public object Get(string name)
        {
            return current.Get(name);
        }

        /// <summary>
        /// works only for int for now.
        /// </summary>
        /// <param name="name">name of record</param>
        /// <param name="value">it's new value</param>
        public void Set(string name, object value)
        {
            if (value is int)
                current.Set(name, (int)value);
            //else if (value is string)
            //    current.Set(name, (string)value);
        }

        /// <summary>
        /// saves repo to file
        /// </summary>
        public void Save()
        {
            current.DumpToFile(file);
            isCorrect = true;
        }

        /// <summary>
        /// opens repo
        /// </summary>
        /// <param name="file_">path and file name</param>
        /// <returns>true if new settings are okey (loaded from xml)</returns>
        public bool Open(string file_)
        {
            Settings new_setting = new Settings();

            if (new_setting.LoadFromFile(file_))
            {
                current = new_setting;
                file = file_;
                isCorrect = true;
                
                return true;
            }

            return false;
        }

    }
}
