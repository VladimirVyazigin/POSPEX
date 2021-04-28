using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using TestTask.Tools;

namespace TestTask.Contracts
{
    public class PiecewiseLinearFunctionData : MvvmBase
    {
        #region Points

        private ObservableCollection<PiecewiseLinearFunctionPoint> _points;

        public ObservableCollection<PiecewiseLinearFunctionPoint> Points
        {
            get => _points;
            set
            {
                if (_points != null) _points.CollectionChanged -= PointsOnCollectionChanged;
                SetProperty(ref _points, value);
                if (_points != null) _points.CollectionChanged += PointsOnCollectionChanged;
            }
        }

        private void PointsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var items = e.NewItems.OfType<PiecewiseLinearFunctionPoint>().ToArray();
                foreach (var item in items) item.PropertyChanged += PiecewiseLinearFunctionPointOnPropertyChanged;
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var items = e.OldItems.OfType<PiecewiseLinearFunctionPoint>().ToArray();
                foreach (var item in items) item.PropertyChanged -= PiecewiseLinearFunctionPointOnPropertyChanged;
            }
        }

        private void PiecewiseLinearFunctionPointOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(PiecewiseLinearFunctionPoint.X) || Function == null) return;
            var point = (PiecewiseLinearFunctionPoint) sender;
            point.Y = Function(point.X);
        }

        #endregion

        public Func<double, double> Function { get; set; }


        public PiecewiseLinearFunctionData()
        {
            Points = new ObservableCollection<PiecewiseLinearFunctionPoint>();
        }
    }
}