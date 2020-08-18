// avoid namespace pollution
namespace Labsim.apollon.gameplay.entity
{

    public class ApollonRealRobosoftEntityBridge : ApollonAbstractGameplayBridge
    {

        // ctor
        public ApollonRealRobosoftEntityBridge()
            : base()
        { }
        
        #region Bridge abstract implementation 
        
        protected override UnityEngine.MonoBehaviour WrapBehaviour()
        {
            
            // retreive
            var behaviours = UnityEngine.Resources.FindObjectsOfTypeAll<ApollonRealRobosoftEntityBehaviour>();
            if ((behaviours?.Length ?? 0) == 0)
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warning: </color> ApollonRealRobosoftEntityBridge.WrapBehaviour() : could not find object of type behaviour.ApollonRealRobosoftEntityBehaviour from Unity."
                );

                return null;

            } /* if() */            

            // finally 
            // TODO : implement the logic of multiple instante (prefab)
            return behaviours[0];

        } /* WrapBehaviour() */

        protected override ApollonGameplayManager.GameplayIDType WrapID()
        {
            return ApollonGameplayManager.GameplayIDType.RealRobosoftEntity;
        }

        protected override void SetActive(bool value)
        {

            // escape
            if (this.Behaviour == null) { return; }

            // switch
            if (value)
            {

                // escape
                if (this.Behaviour.isActiveAndEnabled) { return; }

                // activate
                this.Behaviour.gameObject.SetActive(true);
               
            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                // inactivate
                this.Behaviour.gameObject.SetActive(false);

            } /* if() */

        } /* SetActive() */
        
        #endregion

    }  /* class ApollonRealRobosoftEntityBridge */

} /* } Labsim.apollon.gameplay.entity */