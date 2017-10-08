using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace MyHelper
{
    public static class HelperSetting
    {
        public const string FileName = "setting.json";
        private static string _filePath = string.Empty;
        private static HelperSetting_Item _setting = default(HelperSetting_Item);

        public static void SetFilePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new Exception("Fail path setting");
            else
            {
                if (!File.Exists(path))
                {
                    SetSetting(new HelperSetting_Item(), path);
                }
                try
                {
                    using (var sr = new StreamReader(path))
                    {
                        using (var jsonReader = new JsonTextReader(sr))
                        {
                            var serializer = new JsonSerializer();
                            _setting = serializer.Deserialize<HelperSetting_Item>(jsonReader);
                        }
                    }
                    _filePath = path;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (string.IsNullOrEmpty(_filePath) && File.Exists(path))
                        File.Delete(path);
                }
            }
        }

        public static HelperSetting_Item GetSetting()
        {
            return _setting;
        }

        public static void SetSetting(HelperSetting_Item item)
        {
            if (item != null)
            {
                SetSetting(item, _filePath);
            }
        }

        private static void SetSetting(HelperSetting_Item item, string path)
        {
            if (item != null && !string.IsNullOrEmpty(path))
            {
                _setting = item;
                using (var sw = new StreamWriter(path, false, UTF8Encoding.Unicode))
                {
                    using (var jsonWriter = new JsonTextWriter(sw))
                    {
                        var serializer = new JsonSerializer();
                        serializer.Serialize(jsonWriter, _setting);
                    }
                }
            }
        }
    }

    public class HelperSetting_Item
    {
        public string FileData { get; set; }
        public string FolderBackground { get; set; }
        public string FolderPointer { get; set; }
    }
}
