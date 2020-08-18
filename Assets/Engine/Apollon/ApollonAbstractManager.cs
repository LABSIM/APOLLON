
// avoid namespace pollution
namespace Labsim.apollon
{
    public abstract class ApollonAbstractManager
    {

        // Callback methods

        public virtual void onStart(object sender, ApollonEngine.EngineEventArgs arg)
        {
            /* nothing by default */
        }

        public virtual void onAwake(object sender, ApollonEngine.EngineEventArgs arg)
        {
            /* nothing by default */
        }

        public virtual void onUpdate(object sender, ApollonEngine.EngineEventArgs arg)
        {
            /* nothing by default */
        }

        public virtual void onFixedUpdate(object sender, ApollonEngine.EngineEventArgs arg)
        {
            /* nothing by default */
        }

        public virtual void onSceneLoaded(object sender, ApollonEngine.EngineSceneEventArgs arg)
        {
            /* nothing by default */
        }

        public virtual void onSceneUnloaded(object sender, ApollonEngine.EngineSceneEventArgs arg)
        {
            /* nothing by default */
        }

        public virtual void onExperimentSessionBegin(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {
            /* nothing by default */
        }

        public virtual void onExperimentSessionEnd(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {
            /* nothing by default */
        }

        public virtual void onExperimentTrialBegin(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {
            /* nothing by default */
        }

        public virtual void onExperimentTrialEnd(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {
            /* nothing by default */
        }

    } /* abstract ApollonAbstractManager */

} /* namespace Labsim.apollo */