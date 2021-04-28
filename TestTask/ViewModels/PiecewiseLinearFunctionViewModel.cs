using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TestTask.Contracts;
using TestTask.Models;
using TestTask.Tools;

namespace TestTask.ViewModels
{
    public class PiecewiseLinearFunctionViewModel : ViewModel<PiecewiseLinearFunctionModel>
    {
        #region Commands

        private void InitializeCommands()
        {
            CopySelectedFunctionPointsCommand = new RelayCommand(CopySelectedFunctionPoints);
            CopyAllFunctionPointsCommand = new RelayCommand(CopyAllFunctionPoints);
            PasteFunctionPointsCommand = new RelayCommand(PasteFunctionPoints);
            DeleteSelectedFunctionPointsCommand = new RelayCommand(DeleteSelectedFunctionPoints);
        }

        #region CopySelectedFunctionPointsCommand

        public ICommand CopySelectedFunctionPointsCommand { get; private set; }

        private void CopySelectedFunctionPoints(object obj)
        {
            var collection = ((IList)obj).OfType<PiecewiseLinearFunctionPoint>().ToArray();
            Clipboard.SetText(string.Join("\n", collection.Select(x => $"{x.X}\t{x.Y}")));
        }

        #endregion

        #region CopyAllFunctionPointsCommand

        public ICommand CopyAllFunctionPointsCommand { get; private set; }

        private void CopyAllFunctionPoints(object obj)
        {
            Clipboard.SetText(string.Join("\n", Model.Data.Points.Select(x => $"{x.X}\t{x.Y}")));
        }

        #endregion

        #region PasteFunctionPointsCommand

        public ICommand PasteFunctionPointsCommand { get; private set; }

        private void PasteFunctionPoints(object obj)
        {
            var str = Clipboard.GetText();
            var arrays = str.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).
                Select(x => x.Split(new[] { "\t" }, StringSplitOptions.RemoveEmptyEntries)).
                Where(x => x.Length >= 2);
            foreach (var array in arrays)
            {
                if (!double.TryParse(array[0], out var x) || !double.TryParse(array[1], out var y)) continue;
                Model.Data.Points.Add(new PiecewiseLinearFunctionPoint() { X = x, Y = y });
            }
        }

        #endregion

        #region DeleteSelectedFunctionPointsCommand

        public ICommand DeleteSelectedFunctionPointsCommand { get; private set; }

        private void DeleteSelectedFunctionPoints(object obj)
        {
            var collection = ((IList)obj).OfType<PiecewiseLinearFunctionPoint>().ToArray();
            foreach (var item in collection) Model.Data.Points.Remove(item);
        }

        #endregion

        #endregion

        public PiecewiseLinearFunctionViewModel()
        {
            InitializeCommands();
        }
    }
}
