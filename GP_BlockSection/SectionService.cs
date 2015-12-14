using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using GP_BlockSection.Sections;

namespace GP_BlockSection
{
   public class SectionService
   {
      public Document Doc { get; private set; }

      public SectionService (Document doc)
      {
         Doc = doc;
      }

      // Подсчет секций
      public void CalcSections()
      {
         // Выбор блоков
         Select.SelectSection select = new Select.SelectSection(this);
         select.Select();
         if (select.IdsBlRefSections.Count==0)
         {
            throw new Exception("Не найденв блоки блок-секций");
         }

         // Обработка выбранных блоков
         ParserBlockSection parser = new ParserBlockSection(this, select.IdsBlRefSections);
         parser.Parse();

      }
   }
}
