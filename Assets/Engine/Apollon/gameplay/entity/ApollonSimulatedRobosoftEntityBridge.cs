// avoid namespace pollution
namespace Labsim.apollon.gameplay.entity
{

    public class ApollonSimulatedRobosoftEntityBridge : ApollonAbstractGameplayBridge
    {

        // ctor
        public ApollonSimulatedRobosoftEntityBridge()
            : base()
        { }

        #region Bridge abstract implementation 

        protected override UnityEngine.MonoBehaviour WrapBehaviour()
        {

            // retreive
            var behaviours = UnityEngine.Resources.FindObjectsOfTypeAll<ApollonSimulatedRobosoftEntityBehaviour>();
            if ((behaviours?.Length ?? 0) == 0)
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warning: </color> ApollonSimulatedRobosoftEntityBridge.WrapBehaviour() : could not find object of type behaviour.ApollonSimulatedRobosoftEntityBehaviour from Unity."
                );

                return null;

            } /* if() */

            // finally 
            // TODO : implement the logic of multiple instante (prefab)
            return behaviours[0];

        } /* WrapBehaviour() */

        protected override ApollonGameplayManager.GameplayIDType WrapID()
        {
            return ApollonGameplayManager.GameplayIDType.SimulatedRobosoftEntity;
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

    }  /* class ApollonSimulatedRobosoftEntityBridge */

} /* } Labsim.apollon.gameplay.entity */