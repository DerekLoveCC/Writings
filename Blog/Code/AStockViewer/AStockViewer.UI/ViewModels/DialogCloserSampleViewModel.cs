using AStockViewer.UI.Helpers;
using System.Windows.Input;

namespace AStockViewer.UI.ViewModels
{
    public class DialogCloserSampleViewModel : BindableBase
    {
        public DialogCloserSampleViewModel()
        {
            OkCommand = new DelegateCommand(OnOk);
            CancelCommand = new DelegateCommand(OnCancel);
        }

        private void OnCancel(object obj)
        {
            DialogResult = false;
        }

        private void OnOk(object obj)
        {
            DialogResult = true;
        }

        private bool? _dialogResul;

        public bool? DialogResult
        {
            get
            {
                return _dialogResul;
            }
            set
            {
                if (_dialogResul != value)
                {
                    _dialogResul = value;
                    RaisePropertyChanged(nameof(DialogResult));
                }
            }
        }

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }
    }
}