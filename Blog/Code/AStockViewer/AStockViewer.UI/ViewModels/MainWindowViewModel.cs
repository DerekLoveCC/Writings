using AStockViewer.UI.Helpers;
using AStockViewer.UI.Views;
using System.Windows.Input;

namespace AStockViewer.UI.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            CloseWindowSampleCommand = new DelegateCommand(OnCloseWindowSample);
        }

        private void OnCloseWindowSample(object obj)
        {
            var window = new DialogCloserSample();
            window.ShowDialog();
        }

        public ICommand CloseWindowSampleCommand { get; }
    }
}