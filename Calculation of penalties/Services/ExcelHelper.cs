using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using Calculation_of_penalties.Models;
using Calculation_of_penalties.Resources;
using OfficeOpenXml;

namespace Calculation_of_penalties.Services
{
    class ExcelHelper
    {
        public ExcelHelper(string Path)
        {
            this.Path = Path;
        }

        public string Path { get; }

        public bool SaveData(ObservableCollection<Penalty> list)
        {
            try
            {
                using (var helper = new ExcelPackage(new FileInfo(Path)))
                {
                    var firstsheet = helper.Workbook.Worksheets.Add("Нарахована пеня");
                    firstsheet.SetValue(1, 1, "№ п/п");
                    firstsheet.SetValue(1, 2, "Місяць/ рік");
                    firstsheet.SetValue(1, 3, "Кількість днів");
                    firstsheet.SetValue(1, 4, "Кількість прострочених днів");
                    firstsheet.SetValue(1, 5, "Нарахована сума аліментів ");
                    firstsheet.SetValue(1, 6, "Сплачена сума аліментів");
                    firstsheet.SetValue(1, 7, "Розрахунок проплати");
                    firstsheet.SetValue(1, 8, "Сума, на яку нараховується пеня");
                    firstsheet.SetValue(1, 9, "Пеня,%");
                    firstsheet.SetValue(1, 10, "Пеня,грн.");
                    firstsheet.SetValue(1, 11, "Сума пені за прострочені дні, грн.");
                    firstsheet.SetValue(1, 12, "Загальна сума");

                    int j = 2;
                    foreach (var i in list)
                    {
                        int k = 1;
                        firstsheet.SetValue(j, k++, i.Id);
                        firstsheet.SetValue(j, k++, i.Date.ToString("Y", new CultureInfo("uk-UA")));
                        firstsheet.SetValue(j, k++, i.DaysInMonth);
                        firstsheet.SetValue(j, k++, i.OverdueDays);
                        firstsheet.SetValue(j, k++, i.AlimentTotal);
                        firstsheet.SetValue(j, k++, i.AlimentPaid);
                        firstsheet.SetValue(j, k++, i.Overpayment);
                        firstsheet.SetValue(j, k++, i.PenaltyForSum);
                        firstsheet.SetValue(j, k++, i.PenaltyPersentage.ToString("P1"));
                        firstsheet.SetValue(j, k++, i.PenaltyValue);
                        firstsheet.SetValue(j, k++, i.EachDayPenalty);
                        firstsheet.SetValue(j, k++, i.EachYearPenalty);
                        j++;
                    }

                    helper.Save();
                    new Thread(Show).Start(MyResources.SaveExcelMessage);
                    return true;
                }
            }
            catch(Exception)
            {
                new Thread(Show).Start(MyResources.SaveExcelErrorMessage);
                return false;
            }
        }
        private void Show(object x)
        {
            MessageBox.Show((string)x);
        }
    }
}

