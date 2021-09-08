using FlashScore;
using FlashScore.Football;
using FlashScore.Football.Analiz;

namespace FootballApp.ViewModels.Filters
{
    class TotalCloggedViewModel : FilterViewModel<FTotalClogged>
    {
        public override string Name => "Тотал забитых";
        public Half Half
        {
            get => filter.Half.Value;
            set
            {
                filter.Half.Value = value;
                OnPropertyChanged();
            }
        }
        public LessMoreEqual LMQ
        {
            get => filter.LMQ.Value;
            set
            {
                filter.LMQ.Value = value;
                OnPropertyChanged();
            }
        }
        public byte ValueTotal
        {
            get => filter.ValueTotal.Value;
            set
            {
                filter.ValueTotal.Value = value;
                OnPropertyChanged();
            }
        }
    }
}
