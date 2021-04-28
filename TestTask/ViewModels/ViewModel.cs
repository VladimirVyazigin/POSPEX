using TestTask.Tools;

namespace TestTask.ViewModels
{
    public abstract class ViewModel<TModel> : MvvmBase where TModel : new()
    {
        #region Model

        private TModel _model;

        public TModel Model
        {
            get => _model;
            protected set => SetProperty(ref _model, value);
        }

        #endregion

        protected ViewModel()
        {
            Model = new TModel();
        }
    }
}