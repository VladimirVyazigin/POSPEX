using TestTask.Tools;

namespace TestTask.Models
{
    public abstract class Model<TData> : MvvmBase where TData : new()
    {
        #region Data

        private TData _data;

        public TData Data
        {
            get => _data;
            private set => SetProperty(ref _data, value);
        }

        #endregion

        protected Model()
        {
            Data = new TData();
        }
    }
}