using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using Calculation_of_penalties.Models;
using Newtonsoft.Json;

namespace Calculation_of_penalties.Services
{
    class FileIOService
    {
        public FileIOService(string path)
        {
            this._path = path;
        }

        private readonly string _path;

        public BindingList<Penalty> LoadData()
        {
            var fileExists = File.Exists(_path);
            if (!fileExists)
            {
                File.Create(_path).Dispose();
                return new BindingList<Penalty>();
            }

            using (StreamReader mReader = File.OpenText(_path))
            {
                var filetext = mReader.ReadToEnd();
                return JsonConvert.DeserializeObject<BindingList<Penalty>>(filetext);
            }
        }

        public void SaveData(BindingList<Penalty> myList)
        {
            using (StreamWriter myWriter = File.CreateText(_path))
            {
                string output = JsonConvert.SerializeObject(myList);
                myWriter.WriteLine(output);
            }
        }
    }
}
