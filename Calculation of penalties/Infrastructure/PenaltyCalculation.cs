using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Calculation_of_penalties.Annotations;

namespace Calculation_of_penalties.Infrastructure
{
    class PenaltyCalculation : INotifyPropertyChanged
    {
        public PenaltyCalculation()
        {
            calendar = new GregorianCalendar();
        }

        #region Fields
        
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

        #endregion

        #region Properties

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
                foreach (var i in Data.PenaltyCalculations)
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
                foreach (var i in Data.PenaltyCalculations)
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

        #endregion

        //Метод для оновлення значень властивостей
        public void UpdatePropertys()
        {
           SetOverpayment();
           SetPenaltyForSum();
           SetPenaltyValue();
           SetEachdayPenalty();
           SetEachYearPenalty();
        }

        //Методи для розрахунку значень властивостей
        private void SetEachdayPenalty()
        {
            //Значення властивості EachDayPenalty є добутком кількості прострочених днів на грошове значення пені
            //Для більш корректного виду значення округляється до 2-х останніх знаків після коми
            EachDayPenalty = Math.Round(OverdueDays * PenaltyValue, 2, MidpointRounding.ToEven);
        }
        private void SetEachYearPenalty()
        {
            //Значення властивості EachYearPenalty є суммою властивостей суми пені за просрочені дні в загальній коллекції,
            //починаючи від початку стяглення аліментів і до поточної дати
            //Для більш корректного виду значення округляється до 2-х останніх знаків після коми
            double result = 0;
            foreach (var i in Data.PenaltyCalculations)
            {
                result += i.EachDayPenalty;
                if (i.Date == Date)
                    break;
            }
            EachYearPenalty = Math.Round(result, 2, MidpointRounding.ToEven);
        }
        private void SetPenaltyValue()
        {
            //Значення властивості PenaltyValue є добутком суми на яку нараховується пеня на відсоток пені.
            //Для більш корректного виду значення округляється до 2-х останніх знаків після коми
            PenaltyValue = Math.Round(PenaltyForSum * PenaltyPersentage, 2, MidpointRounding.ToEven);
        }
        private void SetPenaltyForSum()
        {
            if (Overpayment < 0)
                PenaltyForSum = 0;
            else
                PenaltyForSum = Overpayment;
            
            PenaltyForSum = Math.Round(PenaltyForSum, 2, MidpointRounding.ToEven);
        }
        private void SetOverpayment()
        {
            //Значення властивості Overpayment є переплатою, яка йде на наступні місяці.
            //Якщо сума сплачених аліментів більше ніж сума нарахованих, то в наступному місяці
            //платник сплатить суму, яка менша від нарахованих  аліментів.
            //Для більш корректного виду значення округляється до 2-х останніх знаків після коми

            if (Data.PenaltyCalculations.Count == 0)
            {
                Overpayment = AlimentTotal - AlimentPaid;
            }
            else
            {
                if (Data.PenaltyCalculations[0].Date == this.Date)
                {
                    Overpayment = AlimentTotal - AlimentPaid;
                    return;
                }
                else
                {
                    if (Data.PenaltyCalculations[GetNumInArray() - 1].Overpayment < 0)
                    {
                        Overpayment = AlimentTotal - AlimentPaid + Data.PenaltyCalculations[GetNumInArray() - 1].Overpayment;
                    }
                    else
                    {
                        Overpayment = AlimentTotal - AlimentPaid;
                    }
                }
            }
            Overpayment = Math.Round(Overpayment, 2, MidpointRounding.ToEven);
        }

        //Метод для виявлення індекса об'єкта в коллекції
        private int GetNumInArray()
        {
            int i;
            for (i = 0; i < Data.PenaltyCalculations.Count; i++)
            {
                if (Data.PenaltyCalculations[i].Date == this.Date)
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
