using AcadLib.Jigs;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

namespace GP_BlockSection.Sections
{
   // Построение таблицы блок-секций
   public class TableSecton
   {
      private SectionService _service;

      public TableSecton(SectionService sectionService)
      {
         _service = sectionService;
      }

      public void CreateTable()
      {
         Database db = _service.Doc.Database;
         Table table = getTable(db);
         InsertTable(table);
      }

      private Table getTable(Database db)
      {
         Table table = new Table();
         table.SetDatabaseDefaults(db);
         table.TableStyle = db.GetTableStylePIK(); // если нет стиля ПИк в этом чертеже, то он скопируетс из шаблона, если он найдется

         var data = _service.DataSection;

         table.SetSize(8, data.SectionTypes.Count + 1);

         foreach (var column in table.Columns)
         {
            column.Width = 30;
            column.Alignment = CellAlignment.MiddleCenter;
         }

         table.Columns[0].Alignment = CellAlignment.MiddleLeft;
         table.Rows[1].Height = 15;

         table.Cells[0, 0].TextString = "Блок-секции";
         table.Cells[0, 0].Alignment = CellAlignment.MiddleCenter;

         table.Cells[1, 0].TextString = "Наименование блок-секции";
         table.Cells[1, 0].Alignment = CellAlignment.MiddleCenter;
         table.Cells[2, 0].TextString = "Площадь квартир, м.кв.";
         table.Cells[2, 0].Borders.Bottom.LineWeight = LineWeight.LineWeight030;
         table.Cells[3, 0].TextString = "Площадь БКФН, м.кв.";
         table.Cells[3, 0].Borders.Bottom.LineWeight = LineWeight.LineWeight030;
         table.Cells[4, 0].TextString = "Количество секций";
         table.Cells[4, 0].Borders.Bottom.LineWeight = LineWeight.LineWeight030;
         table.Cells[5, 0].TextString = "Средняя этажность";
         table.Cells[5, 0].Borders.Bottom.LineWeight = LineWeight.LineWeight030;
         table.Cells[6, 0].TextString = "ВСЕГО ПЛОЩАДЬ КВАРТИР, м.кв.";
         table.Cells[6, 0].Borders.Bottom.LineWeight = LineWeight.LineWeight030;
         table.Cells[7, 0].TextString = "ВСЕГО ПЛОЩАДЬ БКФН, м.кв.";
         table.Cells[7, 0].Borders.Bottom.LineWeight = LineWeight.LineWeight030;

         var titleCells = CellRange.Create(table, 1, 0, 1, table.Columns.Count - 1);
         titleCells.Borders.Bottom.LineWeight = LineWeight.LineWeight030;

         // Параметры по типам секций
         int col = 1;
         foreach (var sectType in data.SectionTypes)
         {
            table.Cells[1, col].TextString = sectType.Name; //Наименование
            table.Cells[2, col].TextString = sectType.AreaApartTotal.ToString("0.0");  //Площадь квартир
            table.Cells[3, col].TextString = sectType.AreaBKFN.ToString("0.0");  // Площадь БКФН
            table.Cells[4, col].TextString = sectType.Count.ToString();// Кол секций
            col++;
         }
         var mCells = CellRange.Create(table, 5, 1, 5, table.Columns.Count - 1);
         table.MergeCells(mCells);
         mCells = CellRange.Create(table, 6, 1, 6, table.Columns.Count - 1);
         table.MergeCells(mCells);
         mCells = CellRange.Create(table, 7, 1, 7, table.Columns.Count - 1);
         table.MergeCells(mCells);

         // Общие параметры по всем типам секций
         table.Cells[5, 1].TextString = data.AverageFloors.ToString("0.0"); // Средняя этажность
         table.Cells[5, 1].Borders.Bottom.LineWeight = LineWeight.LineWeight030;
         table.Cells[5, 1].Borders.Top.LineWeight = LineWeight.LineWeight030;
         table.Cells[6, 1].TextString = data.TotalAreaApart.ToString("0.0"); // ВСЕГО ПЛОЩАДЬ КВАРТИР
         table.Cells[6, 1].Borders.Bottom.LineWeight = LineWeight.LineWeight030;
         table.Cells[7, 1].TextString = data.TotalAreaBKFN.ToString("0.0"); // ВСЕГО ПЛОЩАДЬ БКФН
         table.Cells[7, 1].Borders.Bottom.LineWeight = LineWeight.LineWeight030;

         table.GenerateLayout();
         return table;
      }

      // вставка
      private void InsertTable(Table table)
      {
         Database db = _service.Doc.Database;
         Editor ed = _service.Doc.Editor;

         TableJig jigTable = new TableJig(table, 1 / db.Cannoscale.Scale, "Вставка таблицы блок-секций");
         if (ed.Drag(jigTable).Status == PromptStatus.OK)
         {
            using (var t = db.TransactionManager.StartTransaction())
            {
               //table.ScaleFactors = new Scale3d(100);
               var cs = db.CurrentSpaceId.GetObject(OpenMode.ForWrite) as BlockTableRecord;
               cs.AppendEntity(table);
               t.AddNewlyCreatedDBObject(table, true);
               t.Commit();
            }
         }
      }
   }
}