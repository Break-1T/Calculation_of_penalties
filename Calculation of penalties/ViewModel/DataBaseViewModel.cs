using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Calculation_of_penalties.Infrastructure;

namespace Calculation_of_penalties.ViewModel
{
    class DataBaseViewModel:Base.ViewModel
    {
        public DataBaseViewModel()
        {
            Data = new CreateDataBase(new DateTime(2010,1,1), new DateTime(2021,1,1));
        }
        public CreateDataBase Data { get; set; }
        
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }
}
