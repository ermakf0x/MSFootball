using DevExpress.Mvvm;
using System.Windows.Controls;

namespace MSFootball.ViewModels
{
    class MainVM : BindableBase
    {
        private Page currentPage;

        public AnalizOfMatchesVM AnalizVM { get; private set; }
        public Page CurrentPage
        {
            get => currentPage;
            private set
            {
                currentPage = value;
                RaisePropertyChanged();
            }
        }

        public MainVM()
        {
            var loaderCurrentMatchesVM = new LoaderCurrentMatchesVM();
            loaderCurrentMatchesVM.LoadSucsess += () => CurrentPage = new TableCurrentMatchesPage() { DataContext = new TableCurrentMatchesVM() };
            CurrentPage = new LoaderCurrentMatchesPage() { DataContext = loaderCurrentMatchesVM };

            AnalizVM = new AnalizOfMatchesVM();
        }
    }
}
