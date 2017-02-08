using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace MyHelper
{
    public class HelperImage
    {
        private static List<HelperImage_Item> _lst = new List<HelperImage_Item>();
        private static HelperImage_Item _itemChange = default(HelperImage_Item);

        public static HelperImage_Item GetItemChange()
        {
            return _itemChange;
        }

        public static void SetItemChange(HelperImage_Item item)
        {
            if (item != null)
            {
                _itemChange = item;
            }
        }

        public static void Load()
        {
            var setting = HelperSetting.GetSetting();
            if (setting != null && !string.IsNullOrEmpty(setting.FileData))
            {
                try
                {
                    if (File.Exists(setting.FileData))
                    {
                        using (var sr = new StreamReader(setting.FileData))
                        {
                            using (var jsonReader = new JsonTextReader(sr))
                            {
                                var serializer = new JsonSerializer();
                                _lst = serializer.Deserialize<List<HelperImage_Item>>(jsonReader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static List<HelperImage_Item> List()
        {
            return _lst;
        }

        public static HelperImage_Item GetByName(string name)
        {
            return _lst.FirstOrDefault(c => c.Name == name);
        }

        public static bool ExistsName(string name)
        {
            return _lst.Where(c => c.Name == name).Count() > 0;
        }

        public static void SaveInfo(string name, string ouput)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var item = _lst.FirstOrDefault(c => c.Name == name);
                if (item == null)
                {
                    item = new HelperImage_Item();
                    item.Name = name;
                    item.Pointers = new List<HelperImage_Pointer>();
                    _lst.Add(item);
                }
                item.PathOutput = ouput;

                Write();
            }
        }

        public static void Delete(string name)
        {
            var item = _lst.FirstOrDefault(c => c.Name == name);
            if (item != null)
            {
                _lst.Remove(item);
                Write();
            }
        }

        private static void Write()
        {
            var setting = HelperSetting.GetSetting();
            if (setting != null && !string.IsNullOrEmpty(setting.FileData))
            {
                using (var sw = new StreamWriter(setting.FileData, false, UTF8Encoding.Unicode))
                {
                    using (var jsonWriter = new JsonTextWriter(sw))
                    {
                        var serializer = new JsonSerializer();
                        serializer.Serialize(jsonWriter, _lst);
                    }
                }
            }            
        }
    }

    public class HelperImage_Item
    {
        public string Name { get; set; }
        public string PathOutput { get; set; }
        public string PathBackground { get; set; }
        public List<HelperImage_Pointer> Pointers { get; set; }
    }

    public class HelperImage_Pointer
    {
        public string PathPointer { get; set; }
        public double Top { get; set; }
        public double Left { get; set; }
    }
}
