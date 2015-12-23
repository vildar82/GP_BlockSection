using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcadLib.Errors;
using Autodesk.AutoCAD.DatabaseServices;
using GP_BlockSection.Options;

namespace GP_BlockSection.Sections
{
   public class ParserBlockSection
   {
      private List<ObjectId> _idsBlRefSections;
      private SectionService _service;

      public List<Section> Sections { get; private set; }

      public ParserBlockSection(SectionService sectionService, List<ObjectId> idsBlRefSections)
      {
         _service = sectionService;
         _idsBlRefSections = idsBlRefSections;
      }

      // Перебор блоков блок-секции и создание списка блок-секций
      public void Parse()
      {
         Sections = new List<Section>();
         foreach (var idBlRefSection in _idsBlRefSections)
         {
            Section section = new Section();
            string errMsg = string.Empty;
            using (var blRef = idBlRefSection.Open( OpenMode.ForRead, false, true) as BlockReference)
            {               
               // обработка атрибутов
               parseAttrs(blRef.AttributeCollection, section, ref errMsg);
               if (!string.IsNullOrEmpty(errMsg))
               {
                  Inspector.AddError(errMsg, blRef);
               }
            }            
            Sections.Add(section);
         }
      }     

      private void parseAttrs(AttributeCollection attrs, Section section, ref string errMsg)
      {
         if (attrs == null)
         {
            errMsg += "В блоке нет атрибутов.";
            return;
         }

         foreach (ObjectId idAtrRef in attrs)
         {
            using (var atrRef = idAtrRef.Open( OpenMode.ForRead, false, true)as AttributeReference)
            {
               // Наименование
               if (string.Equals(atrRef.Tag, Settings.Default.AttrName, StringComparison.OrdinalIgnoreCase))
               {
                  section.SetName(atrRef.TextString);
               }
               // Площадь БКФН
               if (string.Equals(atrRef.Tag, Settings.Default.AttrAreaBKFN, StringComparison.OrdinalIgnoreCase))
               {
                  section.SetAreaBKFN(atrRef.TextString);
               }
               // Площадь квартир на одном этаже
               else if (string.Equals(atrRef.Tag, Settings.Default.AttrAreaApart, StringComparison.OrdinalIgnoreCase))
               {
                  section.SetAreaApart(atrRef.TextString);                  
               }
               // Площадь квартир общая на секцию (по всем этажам кроме 1)
               else if (string.Equals(atrRef.Tag, Settings.Default.AttrAreaApartTotal, StringComparison.OrdinalIgnoreCase))
               {
                  section.SetAreaApartTotal(atrRef.TextString);
               }
               // Кол этажей
               else if (string.Equals(atrRef.Tag, Settings.Default.AttrNumberFloor, StringComparison.OrdinalIgnoreCase))
               {
                  section.SetNumberFloor(atrRef.TextString);
               }
            }
         }
         checkParam(section, ref errMsg);        
      }

      private void checkParam(Section section, ref string errMsg)
      {
         // Наименование
         if (string.IsNullOrEmpty(section.Name))
         {
            errMsg += "Наименование секции не определено.";
         }
      }
   }
}
