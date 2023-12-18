
// avoid namespace pollution
namespace Labsim.apollon.gameplay
{

    public class ApollonGameplayBehaviour
        : UnityEngine.MonoBehaviour
    {

        // root
        public ApollonAbstractGameplayBridge Bridge { get; set; }
        
    }  /* abstract ApollonGameplayBehaviour */

    public abstract class ApollonConcreteGameplayBehaviour<T> 
        : ApollonGameplayBehaviour
        where T : ApollonAbstractGameplayBridge
    {

        public T ConcreteBridge => this.Bridge as T;
        
    }  /* abstract generic ApollonConcreteGameplayBehaviour */

} /* } Labsim.apollon.gameplay */