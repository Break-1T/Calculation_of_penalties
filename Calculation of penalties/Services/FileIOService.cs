using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<Penalty> LoadData()
        {
            var fileExists = File.Exists(_path);
            if (!fileExists)
            {
                File.Create(_path).Dispose();
                return new ObservableCollection<Penalty>();
            }

            using (StreamReader mReader = File.OpenText(_path))
            {
                var filetext = mReader.ReadToEnd();
                return JsonConvert.DeserializeObject<ObservableCollection<Penalty>>(filetext);
            }
        }

        public void SaveData(ObservableCollection<Penalty> myList)
        {
            using (StreamWriter myWriter = File.CreateText(_path))
            {
                string output = JsonConvert.SerializeObject(myList);
                myWriter.WriteLine(output);
            }
        }
    }
}
