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
    class Penalty :INotifyPropertyChanged
    {
        public Penalty()
        {
            calendar = new GregorianCalendar();
        }
        private Calendar calendar;
        
        private double _AlimentTotal;
        private double _AlimentPaid;

        public CreateDataBase DataBase { get; set; }

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
                OnPropertyChanged("Overpayment");
                OnPropertyChanged("PenaltyForSum");
                OnPropertyChanged("PenaltyValue");
                OnPropertyChanged("EachDayPenalty");
                OnPropertyChanged("EachYearPenalty");
                foreach (var i in DataBase.Penalties)
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
                OnPropertyChanged("Overpayment");
                OnPropertyChanged("PenaltyForSum");
                OnPropertyChanged("PenaltyValue");
                OnPropertyChanged("EachDayPenalty");
                OnPropertyChanged("EachYearPenalty");
                foreach (var i in DataBase.Penalties)
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
                double result=0;

                if (DataBase.Penalties[0].Date == this.Date)
                {
                    result = AlimentTotal - AlimentPaid;
                }
                else
                {
                    if (DataBase.Penalties[GetNumInArray() - 1].Overpayment < 0)
                    {
                        result = AlimentTotal - AlimentPaid + DataBase.Penalties[GetNumInArray() - 1].Overpayment;
                    }
                    else
                    {
                        result = AlimentTotal - AlimentPaid;
                    }
                }

                return result;
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
                foreach (var i in DataBase.Penalties)
                {
                    result += i.EachDayPenalty;
                    if (i.Date == Date)
                        break;
                }
                return Math.Round(result,2,MidpointRounding.ToEven);
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
            for (i=0; i < DataBase.Penalties.Count; i++)
            {
                if (DataBase.Penalties[i].Date == this.Date)
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
