using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Calculation_of_penalties.Models;
using Calculation_of_penalties.Resources;
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
            try
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
                    ObservableCollection<Penalty> temp =
                        JsonConvert.DeserializeObject<ObservableCollection<Penalty>>(filetext);
                    new Thread(Show).Start(MyResources.LoadMessage);
                    return temp;
                }
            }
            catch (Exception)
            {
                new Thread(Show).Start(MyResources.LoadErrorMessage);
                return new ObservableCollection<Penalty>();
            }
        }

        public void SaveData(ObservableCollection<Penalty> myList)
        {
            try
            {
                using (StreamWriter myWriter = File.CreateText(_path))
                {
                    string output = JsonConvert.SerializeObject(myList);
                    myWriter.WriteLine(output);
                }

                new Thread(Show).Start(MessageBox.Show(MyResources.SaveMessage));
            }
            catch (Exception)
            {
                new Thread(Show).Start(MyResources.SaveErrorMessage);
            }
        }
        private void Show(object x)
        {
            MessageBox.Show((string)x);
        }
    }
}
