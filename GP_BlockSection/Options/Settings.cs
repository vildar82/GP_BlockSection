using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AcadLib.Files;

namespace GP_BlockSection.Options
{
   public class Settings
   {
      public string BlockSectionPrefix { get; set; }
      public string AttrAreaBKFN { get; set; }
      public string AttrAreaApart { get; set; }
      public string AttrAreaApartTotal { get; set; }
      public string AttrNumberFloor { get; set; }
      public string AttrName { get; set; }
      

      private static Settings _settings = Load();      
      private static string _curDllDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      public static string FileSettings { get { return Path.Combine(_curDllDir, "GP_BlockSection.xml"); } }
      
      public static Settings Default { get { return _settings; }}

      private static Settings Load()
      {
         Settings res;
         if (File.Exists(FileSettings))
         {
            try
            {
               SerializerXml ser = new SerializerXml(FileSettings);
               res = ser.DeserializeXmlFile<Settings>();
            }
            catch
            {
               res = new Settings();
               res.SetDefault();
            }
         }
         else
         {
            res = new Settings();
            res.SetDefault();
            res.Save();
         }
         return res;
      }

      private void Save()
      {
         try
         {
            SerializerXml ser = new SerializerXml(FileSettings);
            ser.SerializeList(this);
         }
         catch
         {            
         }         
      }

      private void SetDefault()
      {
         BlockSectionPrefix = "ГП_Блок-секция";
         AttrName = "НАИМЕНОВАНИЕ";
         AttrAreaBKFN = "БКФН";
         AttrAreaApart = "КВЭТ";
         AttrAreaApartTotal = "КВОБЩ";
         AttrNumberFloor = "ЭТАЖЕЙ";
      }
   }
}
