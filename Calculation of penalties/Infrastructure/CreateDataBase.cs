using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
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
            Penalty = new ObservableCollection<PenaltyCalculation>();
            Penalties = new ObservableCollection<Penalty>();
            Create();

        }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        
        Calendar cal;
        
        private int TotalDays;

        private ObservableCollection<PenaltyCalculation> _Penalty;

        public ObservableCollection<PenaltyCalculation> Penalty
        {
            get => _Penalty;
            set
            {
                _Penalty = value;
                OnPropertyChanged("Penalty");
            }
        }
        public ObservableCollection<Penalty> Penalties { get; set; }

        public void Create()
        {
            int i = 1;
            while (Start<=End)
            {
                Penalty.Add(new PenaltyCalculation()
                {
                    Data = this,
                    Id = i,
                    Date = Start,
                    OverdueDays = TotalDays,
                    AlimentTotal = 0,
                    AlimentPaid = 0,
                    PenaltyPersentage = 0.01
                });
                TotalDays -= cal.GetDaysInMonth(Start.Year, Start.Month);
                Start = Start.AddMonths(1);
                i++;
            }
        }

        public ObservableCollection<Penalty> GetDataCopy()
        {
            ObservableCollection<Penalty> penalties = new ObservableCollection<Penalty>();
            foreach (var j in Penalty)
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

        public void SetDataCopy(ObservableCollection<Penalty> list)
        {
            ObservableCollection<PenaltyCalculation> penalties = new ObservableCollection<PenaltyCalculation>();
            foreach (var j in list)
            {
                PenaltyCalculation penalty = new PenaltyCalculation()
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

            Penalty = penalties;
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

        public void Update()
        {
            OnPropertyChanged("PenaltiesCalc");
        }
    }
}
