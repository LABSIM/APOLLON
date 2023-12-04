
// avoid namespace pollution
namespace Labsim.apollon.gameplay
{

    public class ApollonGameplayDispatcher
    {
        
        // root
        public ApollonAbstractGameplayBridge Bridge { get; set; }

        #region event args class

        public class GameplayEventArgs
            : ApollonEngine.EngineEventArgs
        {

            // ctor
            public GameplayEventArgs()
                : base()
            {

            }

            // ctor
            public GameplayEventArgs(GameplayEventArgs rhs)
                : base(rhs)
            {
            }

        } /* GameplayEventArgs() */

        #endregion

        #region Dictionary & each list of event

        protected readonly System.Collections.Generic.Dictionary<string, System.Delegate>
            _eventTable = new System.Collections.Generic.Dictionary<string, System.Delegate>();

        #endregion

    }  /* abstract ApollonGameplayDispatcher */

    public abstract class ApolloConcreteGameplayDispatcher<T> 
        : ApollonGameplayDispatcher
        where T : ApollonAbstractGameplayBridge
    {

        public T ConcreteBridge => this.Bridge as T;
        
    }  /* abstract generic ApollonConcreteGameplayDispatcher */

} /* } Labsim.apollon.gameplay */