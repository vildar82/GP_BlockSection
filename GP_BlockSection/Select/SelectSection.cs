using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using GP_BlockSection.Options;

namespace GP_BlockSection.Select
{
   public class SelectSection
   {
      private SectionService _ss;

      public List<ObjectId> IdsBlRefSections { get; private set; }

      public SelectSection (SectionService ss)
      {
         _ss = ss;
      }

      public void Select()
      {
         var prOpt = new PromptSelectionOptions();
         prOpt.MessageForAdding = "Выбор блоков блок-секций";
         var res = _ss.Doc.Editor.GetSelection(prOpt);
         if (res.Status != PromptStatus.OK)
         {
            throw new Exception("Отменено пользователем.");
         }
         IdsBlRefSections = new List<ObjectId>();
         foreach (ObjectId idEnt in res.Value.GetObjectIds())
         {
            if (idEnt.ObjectClass.Name == "AcDbBlockReference")
            {
               using (var blRef = idEnt.Open( OpenMode.ForRead, false, true)as BlockReference)
               {
                  var name = blRef.GetEffectiveName();
                  if (name.StartsWith(Settings.Default.BlockSectionPrefix, StringComparison.OrdinalIgnoreCase))
                  {
                     IdsBlRefSections.Add(idEnt);
                  }
               }
            }            
         }
      }
   }
}
