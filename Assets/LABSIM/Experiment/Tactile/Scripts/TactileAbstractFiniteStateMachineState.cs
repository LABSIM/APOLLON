// avoid namespace polluti
namespace Labsim.experiment.tactile
{

    public abstract class TactileAbstractFiniteStateMachineState<T> 
        where T : TactileAbstractBridge
    {
        public T FSM { get; protected set; } = null;

        public TactileAbstractFiniteStateMachineState(T fsm)
        {
            this.FSM = fsm;
        }

        public abstract System.Threading.Tasks.Task OnEntry();

        public abstract System.Threading.Tasks.Task OnExit();

    } /* abstract class TactileAbstractFiniteStateMachineState */

} /* namespace Labsim.experiment.tactile */
