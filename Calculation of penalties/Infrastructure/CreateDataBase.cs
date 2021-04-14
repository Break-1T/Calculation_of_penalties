using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using Calculation_of_penalties.Annotations;
using Calculation_of_penalties.Models;

namespace Calculation_of_penalties.Infrastructure
{
    class CreateDataBase:INotifyPropertyChanged
    {
        public CreateDataBase(DateTime Start,DateTime End)
        {
            cal = new GregorianCalendar();

            this.Start = Start;
            this.End = End;
            
            TotalDays = GetDaysInPeriod();

            PenaltiesCalc = new BindingList<PenaltyCalculation>();
            Penalties = new BindingList<Penalty>();
            Create();

        }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        
        Calendar cal;
        
        private int TotalDays;

        public BindingList<PenaltyCalculation> PenaltiesCalc { get; set; }
        public BindingList<Penalty> Penalties { get; set; }

        public void Create()
        {
            int i = 1;
            while (Start<=End)
            {
                PenaltyCalculation penalty = new PenaltyCalculation()
                {
                    DataBase = this,
                    Id = i,
                    Date = Start,
                    OverdueDays = TotalDays,
                    AlimentTotal = 0,
                    AlimentPaid = 0,
                    PenaltyPersentage = 0.01,
                };
                TotalDays -= cal.GetDaysInMonth(Start.Year, Start.Month);
                PenaltiesCalc.Add(penalty);
                Start = Start.AddMonths(1);
                i++;
            }
        }

        public BindingList<Penalty> GetDataCopy()
        {
            BindingList<Penalty> penalties = new BindingList<Penalty>();
            foreach (var j in PenaltiesCalc)
            {
                Penalty penalty = new Penalty()
                {
                    Id = j.Id,
                    AlimentPaid = j.AlimentPaid,
                    AlimentTotal = j.AlimentTotal,
                    Date = j.Date,
                    DaysInMonth = j.DaysInMonth,
                    EachDayPenalty = j.EachDayPenalty,
                    EachYearPenalty = j.EachYearPenalty,
                    OverdueDays = j.OverdueDays,
                    Overpayment = j.Overpayment,
                    PenaltyForSum = j.PenaltyForSum,
                    PenaltyPersentage = j.PenaltyPersentage,
                    PenaltyValue = j.PenaltyValue
                };
                penalties.Add(penalty);
            }

            return penalties;
        }

        public void SetDataCopy(BindingList<Penalty> list)
        {
            BindingList<PenaltyCalculation> penalties = new BindingList<PenaltyCalculation>();
            foreach (var j in list)
            {
                PenaltyCalculation penalty = new PenaltyCalculation()
                {
                    DataBase = this,
                    Id = j.Id,
                    AlimentPaid = j.AlimentPaid,
                    AlimentTotal = j.AlimentTotal,
                    Date = j.Date,
                    DaysInMonth = j.DaysInMonth,
                    EachDayPenalty = j.EachDayPenalty,
                    EachYearPenalty = j.EachYearPenalty,
                    OverdueDays = j.OverdueDays,
                    Overpayment = j.Overpayment,
                    PenaltyForSum = j.PenaltyForSum,
                    PenaltyPersentage = j.PenaltyPersentage,
                    PenaltyValue = j.PenaltyValue
                };
                penalties.Add(penalty);
            }

            PenaltiesCalc = penalties;
            OnPropertyChanged("PenaltiesCalc");
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
