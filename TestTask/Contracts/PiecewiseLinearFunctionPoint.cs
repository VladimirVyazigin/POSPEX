using TestTask.Tools;

namespace TestTask.Contracts
{
    public class PiecewiseLinearFunctionPoint : MvvmBase
    {
        #region X

        private double _x;

        public double X
        {
            get => _x;
            set => SetProperty(ref _x, value);
        }

        #endregion

        #region Y

        private double _y;

        public double Y
        {
            get => _y;
            set => SetProperty(ref _y, value);
        }

        #endregion
    }
}