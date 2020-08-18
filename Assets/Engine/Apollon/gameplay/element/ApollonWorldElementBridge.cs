// avoid namespace pollution
namespace Labsim.apollon.gameplay.element
{

    public class ApollonWorldElementBridge : ApollonAbstractGameplayBridge
    {

        //ctor
        public ApollonWorldElementBridge()
            : base()
        { }

        #region Bridge abstract implementation 

        protected override UnityEngine.MonoBehaviour WrapBehaviour()
        {
            
            // retreive
            var behaviours = UnityEngine.Resources.FindObjectsOfTypeAll<ApollonWorldElementBehaviour>();
            if ((behaviours?.Length ?? 0) == 0)
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warning: </color> ApollonWorldElementBridge.WrapBehaviour() : could not find object of type behaviour.ApollonWorldElementBehaviour from Unity."
                );

                return null;

            } /* if() */

            // finally 
            // TODO : implement the logic of multiple instante (prefab)
            return behaviours[0];

        } /* WrapBehaviour() */

        protected override ApollonGameplayManager.GameplayIDType WrapID()
        {
            return ApollonGameplayManager.GameplayIDType.WorldElement;
        }

        protected override void SetActive(bool value)
        {

            // escape
            if (this.Behaviour == null) { return; }

            // switch
            if (value)
            {

                // escape
                if (this.Behaviour.enabled) { return; }

                // activate
                this.Behaviour.enabled = true;
                this.Behaviour.gameObject.SetActive(true);

            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;

            } /* if() */

        } /* SetActive() */
        
        #endregion

    }  /* class ApollonWorldElementBridge */

} /* } Labsim.apollon.gameplay.element */