
namespace Labsim.apollon.experiment
{

    //
    // FSM CRTP
    //  -- Finite State Machine Curiously Recurring Template Pattern
    //
    public abstract class ApollonAbstractExperimentFiniteStateMachine<T> : ApollonAbstractExperimentProfile 
        where T : ApollonAbstractExperimentFiniteStateMachine<T>
    {

        // properties
        public ApollonAbstractExperimentState<T> State { get; protected set; } = null;

        protected async System.Threading.Tasks.Task SetState(ApollonAbstractExperimentState<T> next_state)
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

            // then, OnEntry fresh
            if (this.State != null)
            {
                await this.State.OnEntry();
            }
            
        } /* SetState() */

    } /* actract class ApollonAbstractExperimentFiniteStateMachine */

} /* namespace Labsim.apollon.experiment */
