using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using Calculation_of_penalties.Models;

namespace Calculation_of_penalties.Infrastructure
{
    class CreateDataBase
    {
        public CreateDataBase(DateTime Start,DateTime End)
        {
            cal = new GregorianCalendar();

            this.Start = Start;
            this.End = End;
            
            TotalDays = GetDaysInPeriod();
            
            Penalties = new BindingList<Penalty>();
            Create();

        }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        
        Calendar cal;
        private int TotalDays;
        public BindingList<Penalty> Penalties { get; set; }

        public void Create()
        {
            int i = 1;
            while (Start<=End)
            {
                Penalty penalty = new Penalty()
                {
                    Id = i,
                    Date = Start,
                    OverdueDays = TotalDays,
                    AlimentTotal = 0,
                    AlimentPaid = 0,
                    PenaltyPersentage = 0.01,
                    EachYearPenalty = 0
                };
                TotalDays -= cal.GetDaysInMonth(Start.Year,Start.Month);
                Penalties.Add(penalty);
                Start = Start.AddMonths(1);
                i++;
            }
        }

        private int GetDaysInPeriod()
        {
            DateTime starttime = new DateTime(Start.Year, Start.Month, Start.Day);
            int days = 0;
            while (starttime <= End)
            {
                days += cal.GetDaysInMonth(starttime.Year, starttime.Month);
                starttime = starttime.AddMonths(1);
            }
            return days;
        }
    }
}
