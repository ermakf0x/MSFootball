using FlashScore.Football;
using FlashScore.Football.Analiz;

namespace FootballApp.ViewModels.Filters
{
    class DrawMatchViewModel : FilterViewModel<FDrawMatch>
    {
        public override string Name => "Ничья";
        public Half Half
        {
            get => filter.Half.Value;
            set
            {
                filter.Half.Value = value;
                OnPropertyChanged();
            }
        }
    }
}
