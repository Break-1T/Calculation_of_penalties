using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Calculation_of_penalties.Infrastructure;
using Calculation_of_penalties.Models;

namespace Calculation_of_penalties.ViewModel
{
    class DataBaseViewModel:Base.ViewModel
    {

        public DataBaseViewModel(DateTime start, DateTime end)
        {
            StartTime = start;
            EndTime = end;
            Data = new CreateDataBase(StartTime, EndTime);
        }

        public CreateDataBase Data { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
