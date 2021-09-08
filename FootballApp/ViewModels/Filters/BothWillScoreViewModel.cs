using FlashScore.Football;
using FlashScore.Football.Analiz;

namespace FootballApp.ViewModels.Filters
{
    class BothWillScoreViewModel : FilterViewModel<FBothWillScore>
    {
        public override string Name => "Обе забьют";
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
