using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Calculation_of_penalties.Annotations;
using Calculation_of_penalties.Models;

namespace Calculation_of_penalties.Infrastructure
{
    class CreateDataBase:INotifyPropertyChanged
    {
        public CreateDataBase(DateTime Start,DateTime End, double AlimentTotal)
        {
            cal = new GregorianCalendar();
            this.AlimentTotal = AlimentTotal;
            this.Start = Start;
            this.End = End;
            
            TotalDays = GetDaysInPeriod();
            PenaltyCalculations = new ObservableCollection<PenaltyCalculation>();
            Penalties = new ObservableCollection<Penalty>();
            Create();

        }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public double AlimentTotal { get; }

        private int TotalDays;
        Calendar cal;

        private ObservableCollection<PenaltyCalculation> _penaltyCalculations;
        public ObservableCollection<PenaltyCalculation> PenaltyCalculations
        {
            get => _penaltyCalculations;
            set
            {
                _penaltyCalculations = value;
                OnPropertyChanged("PenaltyCalculations");
            }
        }
        
        public ObservableCollection<Penalty> Penalties { get; set; }

        //Метод для заповнення колекції з об'єктами PenaltyCalculation
        public void Create()
        {
            int i = 1;
            while (Start<=End)
            {
                PenaltyCalculations.Add(new PenaltyCalculation()
                {
                    Data = this,
                    Id = i,
                    Date = Start,
                    OverdueDays = TotalDays,
                    AlimentTotal = AlimentTotal,
                    AlimentPaid = 0,
                    PenaltyPersentage = 0.01
                });
                TotalDays -= cal.GetDaysInMonth(Start.Year, Start.Month);
                Start = Start.AddMonths(1);
                i++;
            }
            this.PenaltyCalculations[this.PenaltyCalculations.Count-1].UpdatePropertys();
        }
        //Метод для отримання копії колекції PenaltyCalculations з об'єктом Penalty, замість PenaltyCalculation
        public ObservableCollection<Penalty> GetDataCopy()
        {
            ObservableCollection<Penalty> penalties = new ObservableCollection<Penalty>();
            foreach (var j in PenaltyCalculations)
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
        //Метод для отримання та встановлення копії колекції Penalty з об'єктом PenaltyCalculations, замість PenaltyCalculation
        public void SetDataCopy(ObservableCollection<Penalty> list)
        {
            ObservableCollection<PenaltyCalculation> penalties = new ObservableCollection<PenaltyCalculation>();
            foreach (var j in list)
            {
                PenaltyCalculation penalty = new PenaltyCalculation()
                {
                    Data=this,
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

            PenaltyCalculations = penalties;
        }
        //Метод для отримання кількості днів в місяці
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
