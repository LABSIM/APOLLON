
namespace Labsim.apollon.gameplay
{

    public abstract class ApollonAbstractGameplayState<T> 
        where T : ApollonAbstractGameplayBridge
    {
        public T FSM { get; protected set; } = null;

        public ApollonAbstractGameplayState(T fsm)
        {
            this.FSM = fsm;
        }

        public abstract System.Threading.Tasks.Task OnEntry();

        public abstract System.Threading.Tasks.Task OnExit();

    } /* abstract class ApollonAbstractGameplayState */

} /* namespace Labsim.apollon.gameplay */
