using System;
using IEADPC.Shell.Commen.DevExpressContracts.Interfaces.Services.ContainerService;

namespace IEADPC.Shell.Commen.DevExpress13140.Model
{
    public class ProgressInfo : IProgressInfo
    {
        private readonly Action _onEnd;
        private readonly Action _updateProgress;
        private double _progress;

        public ProgressInfo(Action updateProgress, Action onEnd)
        {
            _updateProgress = updateProgress;
            _onEnd = onEnd;
        }

        #region Implementation of IProgressInfo

        public Action UpdateProgress
        {
            get { return _updateProgress; }
        }

        public double Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                UpdateProgress();
            }
        }

        public string ProgressDescriptor { get; set; }

        public Action OnEnd
        {
            get { return _onEnd; }
        }

        #endregion
    }
}