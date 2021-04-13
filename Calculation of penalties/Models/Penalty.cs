using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Calculation_of_penalties.Models
{
    class Penalty
    {
        public Penalty(DateTime Date)
        {
            this.Date = Date;
            calendar = new GregorianCalendar();
        }
        private Calendar calendar;
        //№ п/п
        public int Id { get; set; }
        
        //місяць/ рік
        public DateTime Date { get; set; }
        
        //кількість днів у місяці
        public int DaysInMonth
        {
            get
            {
                return calendar.GetDaysInMonth(Date.Year, Date.Month);
            }
        }
        
        //кількість прострочених днів
        public int OverdueDays { get; set; }
        
        // нарахована сума аліментів
        public double AlimentTotal { get; set; }
        
        // сплачена сума аліментів
        public double AlimentPaid { get; set; }
        
        public double PenaltyFor { get; set; }
        
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
