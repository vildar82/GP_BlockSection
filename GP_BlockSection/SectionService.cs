﻿using System;
using System.Collections.Generic;
using AcadLib.Errors;
using Autodesk.AutoCAD.ApplicationServices;
using GP_BlockSection.Sections;

namespace GP_BlockSection
{
   public class SectionService
   {
      public SectionService(Document doc)
      {
         Doc = doc;
      }

      public DataSection DataSection { get; private set; }
      public Document Doc { get; private set; }
      public List<Section> Sections { get; private set; }

      // Подсчет секций
      public void CalcSections()
      {
         Inspector.Clear();
         // Выбор блоков
         Select.SelectSection select = new Select.SelectSection(this);
         select.Select();
         if (select.IdsBlRefSections.Count == 0)
         {
            throw new Exception("Не найдены блоки блок-секций");
         }
         else
         {
            Doc.Editor.WriteMessage("\nВыбрано {0} блоков блок-секций.", select.IdsBlRefSections.Count);
         }

         // Обработка выбранных блоков
         ParserBlockSection parser = new ParserBlockSection(this, select.IdsBlRefSections);
         parser.Parse();
         Sections = parser.Sections;

         // Подсчет площадей и типов блок-секций
         DataSection = new DataSection(this);
         DataSection.Calc();

         // Построение таблицы
         TableSecton tableSection = new TableSecton(this);
         tableSection.CreateTable();

         if (Inspector.HasErrors)
         {
            Inspector.Show();
            Inspector.Clear();
         }
      }
   }
}