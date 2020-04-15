using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace AStockViewer.UI.ViewModels
{
    public class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyName)
        {
            var bodyExpression = propertyName.Body as MemberExpression;
            if (bodyExpression?.Member?.Name != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(bodyExpression.Member.Name));
            }
        }
    }
}