using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{
    
    public sealed class TactileSubjectRHWristTracker
        : TactileAbstractTracker
    {

        //ctor
        public TactileSubjectRHWristTracker()
            : base()
        { }

        #region Tracker abstract implementation 

        // task members
        private System.Threading.CancellationTokenSource m_cancellationTokenSource = null;
        private System.Threading.Tasks.Task m_trackerTask = null;

        protected override UnityEngine.MonoBehaviour WrapBehaviour()
        {

            // retreive
            var behaviours = UnityEngine.Resources.FindObjectsOfTypeAll<TactileSubjectRHWristTrackerBehaviour>();
            if ((behaviours?.Length ?? 0) == 0)
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warning: </color> TactileSubjectRHWristTracker.WrapBehaviour() : could not find object of type behaviour.TactileSubjectRHWristTrackerBehaviour from Unity."
                );

                return null;

            } /* if() */

            // tail 
            foreach (var behaviour in behaviours)
            {
                behaviour.Tracker = this;
            }

            // finally 
            // TODO : implement the logic of multiple instante (prefab)
            return behaviours[0];

        } /* WrapBehaviour() */

        protected override TactileTrackerManager.IDType WrapID()
        {
            return TactileTrackerManager.IDType.TactileSubjectRHWrist;
        }
        
        protected override void SetActive(bool value)
        {

            // escape
            if (this.Behaviour == null) { return; }

            // switch
            if (value)
            {

                // escape
                if (this.Behaviour.isActiveAndEnabled) { return; }

                // activate 
                this.Behaviour.enabled = true;
                this.Behaviour.gameObject.SetActive(true);

                // lauch
                this.m_cancellationTokenSource = new System.Threading.CancellationTokenSource();
                this.m_trackerTask 
                    = System.Threading.Tasks.Task.Factory.StartNew(
                        this.RecordRowTaskCallback,
                        this.m_cancellationTokenSource.Token
                    );

            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                // stop until completion
                this.m_cancellationTokenSource.Cancel();
                this.m_trackerTask.Wait();
                if(!this.m_trackerTask.IsCompleted)
                {
                    
                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warn: </color> TactileSubjectRHWristTracker.SetActive() : tracking task callback hasn't completed correctly... strange"
                    );

                } /* if()*/

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;

            } /* if() */

        } /* SetActive() */

        #endregion

        #region Recording task callback

        private async void RecordRowTaskCallback()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileSubjectRHWristTracker.OnRecordRowCoroutine() : begin"
            );

            // loop until
            while(!this.m_cancellationTokenSource.Token.IsCancellationRequested) 
            {

                // get a high res timepoint & behaviour reference
                var timepoint = apollon.ApollonHighResolutionTime.Now;
                var behaviour = this.Behaviour as TactileSubjectRHWristTrackerBehaviour;

                // do record
                if(behaviour.Recording)
                {
                    
                    behaviour.RecordRow();

                } /* if() */

                // wait remaing time elapsed 
                var remaining = ((1.0 / behaviour.DataAcquisitionFrequencyHz) * 1000.0) - timepoint.ElapsedMilliseconds;
                if(remaining > 0.0)
                {
                    
                    await apollon.ApollonHighResolutionTime.DoSleep(remaining);

                }
                else
                {
                    
                    // log

                } /* if() */

            } /* while() */

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileSubjectRHWristTracker.OnRecordRowCoroutine() : end"
            );

        } /* OnPressed() */

        #endregion

    } /* class TactileSubjectRHWristTracker */

} /* } Labsim.experiment.tactile */