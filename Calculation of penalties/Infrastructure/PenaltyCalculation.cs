using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Calculation_of_penalties.Annotations;

namespace Calculation_of_penalties.Infrastructure
{
    class PenaltyCalculation : INotifyPropertyChanged
    {
        public PenaltyCalculation()
        {
            calendar = new GregorianCalendar();
        }
        private Calendar calendar;

        private int id;
        private DateTime date;
        private int daysinmonth;
        private double _AlimentTotal;
        private double _AlimentPaid;
        private int overduedays;
        private double overpayment;
        private double penaltyforsum;
        private double penaltypersentage;
        private double penaltyvalue;
        private double eachdaypenalty;
        private double eachyearpenalty;

        //№ п/п
        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        //місяць/ рік
        public DateTime Date
        {
            get => date;
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }

        //кількість днів у місяці
        public int DaysInMonth
        {
            get
            {
                return calendar.GetDaysInMonth(Date.Year, Date.Month);
            }
            set
            {
                daysinmonth = value;
                OnPropertyChanged("DaysInMonth");
            }
        }

        //кількість прострочених днів
        public int OverdueDays
        {
            get => overduedays;
            set
            {
                overduedays = value;
                OnPropertyChanged("OverdueDays");
            }
        }

        // нарахована сума аліментів
        public double AlimentTotal
        {
            get => _AlimentTotal;
            set
            {
                _AlimentTotal = value;
                OnPropertyChanged("AlimentTotal");
                foreach (var i in Data.Penalty)
                    i.UpdatePropertys();
            }
        }

        // сплачена сума аліментів
        public double AlimentPaid
        {
            get => _AlimentPaid;
            set
            {
                _AlimentPaid = value;
                OnPropertyChanged("AlimentPaid");
                foreach (var i in Data.Penalty)
                    i.UpdatePropertys();
            }
        }

        //Розрахунок проплати
        public double Overpayment
        {
            get
            {
                return overpayment;
            }
            set
            {
                overpayment = value;
                OnPropertyChanged("Overpayment");
            }
        }

        //Сума, на яку нараховується пеня
        public double PenaltyForSum
        {
            get
            {
                return penaltyforsum;
            }
            set
            {
                penaltyforsum = value;
                OnPropertyChanged("PenaltyForSum");
            }
        }

        //пеня,%
        public double PenaltyPersentage
        {
            get => penaltypersentage;
            set
            {
                penaltypersentage = value;
                OnPropertyChanged("PenaltyPersentage");
            }
        }

        //пеня,грн.
        public double PenaltyValue
        {
            get => penaltyvalue;
            set
            {
                penaltyvalue = value;
                OnPropertyChanged("PenaltyValue");
            }
        }

        //сума пені за прострочені дні, грн.
        public double EachDayPenalty
        {
            get => eachdaypenalty;
            set
            {
                eachdaypenalty = value;
                OnPropertyChanged("EachDayPenalty");
            }
        }

        //Загальна сума пені за прострочені дні, грн.
        public double EachYearPenalty
        {
            get
            {
                return eachyearpenalty;
            }
            set
            {
                eachyearpenalty = value;
                OnPropertyChanged("EachYearPenalty");
            }
        }

        public CreateDataBase Data { get; set; }

        public void UpdatePropertys()
        {
           SetOverpayment();
           SetPenaltyForSum();
           SetPenaltyValue();
           SetEachdayPenalty();
           SetEachYearPenalty();
        }

        private void SetEachdayPenalty()
        {
            EachDayPenalty = Math.Round(OverdueDays * PenaltyValue, 2, MidpointRounding.ToEven);
        }
        private void SetEachYearPenalty()
        {
            double result = 0;
            foreach (var i in Data.Penalty)
            {
                result += i.EachDayPenalty;
                if (i.Date == Date)
                    break;
            }
            EachYearPenalty = Math.Round(result, 2, MidpointRounding.ToEven);
        }
        private void SetPenaltyValue()
        {
            PenaltyValue = Math.Round(PenaltyForSum * PenaltyPersentage, 2, MidpointRounding.ToEven);
        }
        private void SetPenaltyForSum()
        {
            if (Overpayment < 0)
                PenaltyForSum = 0;
            else
                PenaltyForSum = Overpayment;
        }
        private void SetOverpayment()
        {
            if (Data.Penalty.Count == 0)
            {
                Overpayment = AlimentTotal - AlimentPaid;

            }
            else
            {
                if (Data.Penalty[0].Date == this.Date)
                {
                    Overpayment = AlimentTotal - AlimentPaid;
                    return;
                }
                else
                {
                    if (Data.Penalty[GetNumInArray() - 1].Overpayment < 0)
                    {
                        Overpayment = AlimentTotal - AlimentPaid + Data.Penalty[GetNumInArray() - 1].Overpayment;
                    }
                    else
                    {
                        Overpayment = AlimentTotal - AlimentPaid;
                    }
                }
            }
        }

        private int GetNumInArray()
        {
            int i;
            for (i = 0; i < Data.Penalty.Count; i++)
            {
                if (Data.Penalty[i].Date == this.Date)
                    break;
            }
            return i;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
