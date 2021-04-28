using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Contracts;
using TestTask.Tools;

namespace TestTask.ViewModels.Windows
{
    public class MainWindowViewModel : MvvmBase
    {
        #region PiecewiseLinearFunctionViewModel

        private PiecewiseLinearFunctionViewModel _piecewiseLinearFunctionViewModel;

        public PiecewiseLinearFunctionViewModel PiecewiseLinearFunctionViewModel
        {
            get => _piecewiseLinearFunctionViewModel;
            set => SetProperty(ref _piecewiseLinearFunctionViewModel, value);
        }

        #endregion

        public MainWindowViewModel()
        {
            PiecewiseLinearFunctionViewModel = new PiecewiseLinearFunctionViewModel();
            PiecewiseLinearFunctionViewModel.Model.Data.Function = d => d * 3 + 2;
        }
    }
}
