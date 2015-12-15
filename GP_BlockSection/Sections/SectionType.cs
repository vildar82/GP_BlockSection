using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP_BlockSection.Sections
{
   // Тип секции - по Наименованию и по кол этажей
   public class SectionType
   {
      public string Name { get; private set; }
      public int NumberFloor { get; private set; }
      public double AreaApartTotal { get; private set; }
      public double AreaBKFN { get; private set; }
      public int Count { get; private set; }
      public List<Section> Sections { get; private set; }

      public SectionType(string name, int numberFloor)
      {
         Name = name;
         NumberFloor = numberFloor;
         Sections = new List<Section>();
      }

      public void AddSection (Section section)
      {
         // ? проверять соответствие добавляемой секции этому типу секций - Имя, кол этажей
         AreaApartTotal += section.AreaApartTotal;
         AreaBKFN += section.AreaBKFN;
         Count++;
         Sections.Add(section);
      }
   }
}
