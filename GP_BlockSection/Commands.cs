﻿using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;

[assembly: CommandClass(typeof(GP_BlockSection.Commands))]

namespace GP_BlockSection
{
   public class Commands
   {
      /// <summary>
      /// Создание блоков монтажных планов (создаются блоки с именем вида АКР_Монтажка_2).
      /// </summary>
      [CommandMethod("PIK", "GP-BlockSectionTable", CommandFlags.Modal | CommandFlags.NoPaperSpace | CommandFlags.NoBlockEditor)]
      public void BlockSectionTableCommand()
      {
         Document doc = Application.DocumentManager.MdiActiveDocument;
         if (doc == null) return;
         using (var DocLock = doc.LockDocument())
         {
            try
            {
               SectionService ss = new SectionService(doc);
               ss.CalcSections();
            }
            catch (System.Exception ex)
            {
               doc.Editor.WriteMessage("\n{0}", ex.Message);
            }
         }
      }
   }
}