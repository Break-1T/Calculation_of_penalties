using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using Calculation_of_penalties.Models;

namespace Calculation_of_penalties.Infrastructure
{
    class CreateDataBase
    {
        public CreateDataBase(DateTime Start,DateTime End)
        {
            this.Start = Start;
            this.End = End;

            Penalties = new BindingList<Penalty>();
            Create();

            Calendar cal;
        }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public BindingList<Penalty> Penalties { get; set; }

        public void Create()
        {
            int i = 1;
            while (Start<End)
            {
                Penalty penalty = new Penalty(Start)
                {
                    Id = i
                };
                Penalties.Add(penalty);
                Start = Start.AddMonths(1);
                i++;
            }
        }
    }
}
