using System;
using System.Collections.Generic;
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

        private double _AlimentTotal;
        private double _AlimentPaid;

        private int id;
        private DateTime date;
        private int daysinmonth;
        private int overduedays;
        private double alimenttotal;
        private double alimentpaid;
        private double overpayment;
        private double penaltyforsum;
        private double penaltypersentage;
        private double penaltyvalue;
        private double eachdaypenalty;
        private double eachyearpenalty;


        public CreateDataBase DataBase { get; set; }

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

                foreach (var i in DataBase.PenaltiesCalc)
                {
                    i.UpdatePropertys();
                }
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

                foreach (var i in DataBase.PenaltiesCalc)
                {
                    i.UpdatePropertys();
                }
            }
        }

        //Розрахунок проплати
        public double Overpayment
        {
            get
            {
                double result = 0;

                if (DataBase.PenaltiesCalc[0].Date == this.Date)
                {
                    result = AlimentTotal - AlimentPaid;
                }
                else
                {
                    if (DataBase.PenaltiesCalc[GetNumInArray() - 1].Overpayment < 0)
                    {
                        result = AlimentTotal - AlimentPaid + DataBase.PenaltiesCalc[GetNumInArray() - 1].Overpayment;
                    }
                    else
                    {
                        result = AlimentTotal - AlimentPaid;
                    }
                }

                return result;
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
                if (Overpayment < 0)
                {
                    return 0;
                }
                else
                {
                    return Overpayment;
                }
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
            get => Math.Round(PenaltyForSum * PenaltyPersentage, 2, MidpointRounding.ToEven);
            set
            {
                penaltyvalue = value;
                OnPropertyChanged("PenaltyValue");
            }
        }

        //сума пені за прострочені дні, грн.
        public double EachDayPenalty
        {
            get => Math.Round(OverdueDays * PenaltyValue, 2, MidpointRounding.ToEven);
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
                double result = 0;
                foreach (var i in DataBase.PenaltiesCalc)
                {
                    result += i.EachDayPenalty;
                    if (i.Date == Date)
                        break;
                }
                return Math.Round(result, 2, MidpointRounding.ToEven);
            }
            set
            {
                eachdaypenalty = value;
                OnPropertyChanged("EachYearPenalty");
            }
        }

        public void UpdatePropertys()
        {
            OnPropertyChanged("Overpayment");
            OnPropertyChanged("PenaltyForSum");
            OnPropertyChanged("PenaltyValue");
            OnPropertyChanged("EachDayPenalty");
            OnPropertyChanged("EachYearPenalty");
        }

        private int GetNumInArray()
        {
            int i;
            for (i = 0; i < DataBase.PenaltiesCalc.Count; i++)
            {
                if (DataBase.PenaltiesCalc[i].Date == this.Date)
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
