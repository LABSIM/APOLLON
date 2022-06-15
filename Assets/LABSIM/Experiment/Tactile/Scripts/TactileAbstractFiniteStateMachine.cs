// avoid namespace pollution
namespace Labsim.experiment.tactile
{
    //
    // FSM CRTP
    //  -- Finite State Machine Curiously Recurring Template Pattern
    //
    public abstract class TactileAbstractFiniteStateMachine<T> 
        : TactileAbstractBridge 
            where T : TactileAbstractFiniteStateMachine<T>
    {

        // properties
        public TactileAbstractFiniteStateMachineState<T> State { get; protected set; } = null;

        protected async System.Threading.Tasks.Task SetState(TactileAbstractFiniteStateMachineState<T> next_state)
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

    } /* abstract class TactileAbstractFiniteStateMachine */

} /* namespace Labsim.experiment.tactile */
