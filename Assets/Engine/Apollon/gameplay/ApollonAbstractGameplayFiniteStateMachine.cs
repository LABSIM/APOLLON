
namespace Labsim.apollon.gameplay
{

    //
    // FSM CRTP
    //  -- Finite State Machine Curiously Recurring Template Pattern
    //
    public abstract class ApollonAbstractGameplayFiniteStateMachine<T> : ApollonAbstractGameplayBridge 
        where T : ApollonAbstractGameplayFiniteStateMachine<T>
    {

        // properties
        public ApollonAbstractGameplayState<T> State { get; protected set; } = null;

        protected async System.Threading.Tasks.Task SetState(ApollonAbstractGameplayState<T> next_state)
        {

            // escape iff. same
            if (next_state == this.State) return;

            // first, OnExit on previous
            if (this.State != null)
            {
                await this.State.OnExit();
            }

            // assign
            this.State = next_state;

            // then, OnEntry on fresh
            if (this.State != null)
            {
                await this.State.OnEntry();
            }
            
        } /* SetState() */

    } /* abstract class ApollonAbstractGameplayFiniteStateMachine */

} /* namespace Labsim.apollon.gameplay */
