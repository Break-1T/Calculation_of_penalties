using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Calculation_of_penalties.Annotations;
using Calculation_of_penalties.Infrastructure;

namespace Calculation_of_penalties.Models
{
    class Penalty
    {
        //№ п/п
        public int Id { get; set; }

        //місяць/ рік
        public DateTime Date { get; set; }

        //кількість днів у місяці
        public int DaysInMonth { get; set; }

        //кількість прострочених днів
        public int OverdueDays { get; set; }

        // нарахована сума аліментів
        public double AlimentTotal { get; set; }

        // сплачена сума аліментів
        public double AlimentPaid { get; set; }

        //Розрахунок проплати
        public double Overpayment { get; set; }


        //Сума, на яку нараховується пеня
        public double PenaltyForSum { get; set; }

        //пеня,%
        public double PenaltyPersentage { get; set; }

        //пеня,грн.
        public double PenaltyValue { get; set; }

        //сума пені за прострочені дні, грн.
        public double EachDayPenalty { get; set; }

        //Загальна сума пені за прострочені дні, грн.
        public double EachYearPenalty { get; set; }
    }
}
