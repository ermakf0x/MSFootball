using DevExpress.Mvvm;
using MSFootball.Models;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace MSFootball.ViewModels
{
    class LoaderCurrentMatchesVM : BindableBase
    {
        private string text;

        /// <summary>
        /// Событие происходит при успешной загрузке данных с сервера
        /// </summary>
        public event Action LoadSucsess;

        public string Text
        {
            get => text;
            private set
            {
                text = value;
                RaisePropertyChanged();
            }
        }

        public LoaderCurrentMatchesVM()
        {
            Text = "Loaded...";
            Debug.WriteLine("Ctor start");
            _ = GetMatchesAsync();
            Debug.WriteLine("Ctor end");
        }

        async Task GetMatchesAsync()
        {
            Debug.WriteLine("Async start");
            await FootballManager.LoadTempMatchesAsync();
            try
            {
                await FootballManager.GetMatchesAsync();
                LoadSucsess?.Invoke();
            }
            catch (HttpRequestException ex)
            {
                Text = "Error: " + ex.Message;
            }
            catch (Exception ex)
            {
                Text = "Error: " + ex.Message;
            }
            Debug.WriteLine("Async end");
        }
    }
}
