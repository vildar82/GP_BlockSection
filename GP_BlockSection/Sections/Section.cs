using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP_BlockSection.Sections
{
   // Блок-секция
   public class Section
   {
      // Имя блок-секции
      public string Name { get; private set; }
      // Площадь квартир
      public double AreaApart { get; private set; }
      // Общая площадь квартир
      public double AreaApartTotal { get; private set; }
      // Площадь БКФН
      public double AreaBKFN { get; private set; }
      // Кол этажей
      public int NumberFloor { get; private set; }


      public void SetAreaBKFN(string textString)
      {
         double val = getArea(textString);
      }      

      public void SetAreaApart(string textString)
      {
         throw new NotImplementedException();
      }

      public void SetAreaApartTotal(string textString)
      {
         throw new NotImplementedException();
      }

      public void SetNumberFloor(string textString)
      {
         throw new NotImplementedException();
      }

      public void CheckSection()
      {
         throw new NotImplementedException();
      }

      public  void SetName(string textString)
      {
         throw new NotImplementedException();
      }

      private double getArea(string textString)
      {
         double val =0;
         if (double.TryParse(textString, out val))
         {

         }
         return val;                  
      }
   }
}
