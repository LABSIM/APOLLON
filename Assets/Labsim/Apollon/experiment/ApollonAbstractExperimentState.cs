
namespace Labsim.apollon.experiment
{

    public abstract class ApollonAbstractExperimentState<T> where T : ApollonAbstractExperimentProfile
    {
        public T FSM { get; protected set; } = null;

        public ApollonAbstractExperimentState(T fsm)
        {
            this.FSM = fsm;
        }

        public abstract System.Threading.Tasks.Task OnEntry();

        public abstract System.Threading.Tasks.Task OnExit();

    } /* abstract class ApollonAbstractExperimentState */

} /* namespace Labsim.apollon.experiment */
