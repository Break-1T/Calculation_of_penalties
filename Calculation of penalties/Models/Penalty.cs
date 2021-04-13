using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Calculation_of_penalties.Annotations;
using Calculation_of_penalties.Infrastructure;

namespace Calculation_of_penalties.Models
{
    class Penalty :INotifyPropertyChanged
    {
        public Penalty()
        {
            calendar = new GregorianCalendar();
        }
        private Calendar calendar;
        
        private double _AlimentTotal;
        private double _AlimentPaid;

        public BindingList<Penalty> Penalties { get; set; }
        public CreateDataBase DataBase { get; internal set; }

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
        public double AlimentTotal
        {
            get => _AlimentTotal;
            set
            {
                _AlimentTotal = value;
                OnPropertyChanged("AlimentTotal");
                OnPropertyChanged("PenaltyForSum");
                OnPropertyChanged("PenaltyValue");
                OnPropertyChanged("EachDayPenalty");
                OnPropertyChanged("EachYearPenalty");
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
                OnPropertyChanged("PenaltyForSum");
                OnPropertyChanged("PenaltyValue");
                OnPropertyChanged("EachDayPenalty");
                OnPropertyChanged("EachYearPenalty");
            }
        }

        //Сума, на яку нараховується пеня
        public double PenaltyForSum
        {
            get
            {
                if (AlimentPaid == 0d)
                {
                    return AlimentTotal;
                }
                else if (AlimentPaid == AlimentTotal)
                {
                    return 0d;
                }
                else
                {
                    return 0d;
                }
            }
        }

        //пеня,%
        public double PenaltyPersentage { get; set; }
        
        //пеня,грн.
        public double PenaltyValue
        {
            get => Math.Round(PenaltyForSum * PenaltyPersentage,2,MidpointRounding.ToEven);
        }
        
        //сума пені за прострочені дні, грн.
        public double EachDayPenalty
        {
            get => Math.Round(OverdueDays * PenaltyValue,2,MidpointRounding.ToEven);
        }
        
        //Загальна сума пені за прострочені дні, грн.
        public double EachYearPenalty
        {
            get
            {
                double result = 0;
                foreach (var i in Penalties)
                {
                    result += i.EachDayPenalty;
                    if (i.Date == Date)
                        break;
                }

                DataBase.Penalties = Penalties;
                return Math.Round(result,2,MidpointRounding.ToEven);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
