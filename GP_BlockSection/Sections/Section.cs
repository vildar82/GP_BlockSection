using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcadLib.Errors;
using GP_BlockSection.Options;

namespace GP_BlockSection.Sections
{
   // Блок-секция
   public class Section
   {
      // Имя блок-секции
      public string Name { get; private set; } = string.Empty;
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
         AreaBKFN = getArea(textString, Settings.Default.AttrAreaBKFN);
      }      

      public void SetAreaApart(string textString)
      {
         AreaApart = getArea(textString, Settings.Default.AttrAreaApart);
      }

      public void SetAreaApartTotal(string textString)
      {
         AreaApartTotal = getArea(textString, Settings.Default.AttrAreaApartTotal);
      }

      public void SetNumberFloor(string textString)
      {
         NumberFloor = getNum(textString, Settings.Default.AttrNumberFloor);
      }

      public  void SetName(string textString)
      {
         Name = textString.Trim();
      }

      private double getArea(string textString, string areaName)
      {
         double val =0;
         if (!double.TryParse(textString, out val))
         {
            Inspector.AddError("Не определена площадь в атрибуте {0} - значение {1}", areaName, textString);
         }
         return val;                  
      }

      private int getNum(string textString, string attrNumberFloor)
      {
         int val = 0;
         if (!int.TryParse(textString, out val))
         {
            Inspector.AddError("Не определено кол этажей в атрибуте {0} - значение {1}", attrNumberFloor, textString);
         }
         return val;
      }
   }
}
