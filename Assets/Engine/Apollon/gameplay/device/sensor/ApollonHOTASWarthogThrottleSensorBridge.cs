
// using
using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.gameplay.device.sensor
{

    public class ApollonHOTASWarthogThrottleSensorBridge 
        : ApollonAbstractGameplayBridge
    {

        //ctor
        public ApollonHOTASWarthogThrottleSensorBridge()
            : base()
        { }

        public ApollonHOTASWarthogThrottleSensorDispatcher Dispatcher { private set; get; } = null;
        
        #region Bridge abstract implementation 

        protected override UnityEngine.MonoBehaviour WrapBehaviour()
        {

            // retreive
            var behaviours = UnityEngine.Resources.FindObjectsOfTypeAll<ApollonHOTASWarthogThrottleSensorBehaviour>();
            if ((behaviours?.Length ?? 0) == 0)
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warning: </color> ApollonHOTASWarthogThrottleSensorBridge.WrapBehaviour() : could not find object of type behaviour.ApollonHOTASWarthogThrottleSensorBehaviour from Unity."
                );

                return null;

            } /* if() */

            // tail 
            foreach (var behaviour in behaviours)
            {
                behaviour.Bridge = this;
            }

            // instantiate
            this.Dispatcher = new ApollonHOTASWarthogThrottleSensorDispatcher();

            // finally 
            // TODO : implement the logic of multiple instante (prefab)
            return behaviours[0];

        } /* WrapBehaviour() */

        protected override ApollonGameplayManager.GameplayIDType WrapID()
        {
            return ApollonGameplayManager.GameplayIDType.HOTASWarthogThrottleSensor;
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
                this.Behaviour.enabled = true;
                this.Behaviour.gameObject.SetActive(true);
                //await System.Threading.Tasks.Task.Run(
                //    async () =>
                //    {
                //        while (!this.Behaviour.isActiveAndEnabled) await System.Threading.Tasks.Task.Delay(10);
                //    }
                //);

                (this.Behaviour as ApollonHOTASWarthogThrottleSensorBehaviour).OnEnable();

                // add them a bridge delegate
                (this.Behaviour as ApollonHOTASWarthogThrottleSensorBehaviour).ActionMap["ValueChanged"].performed += this.OnAxisZValueChanged;
                (this.Behaviour as ApollonHOTASWarthogThrottleSensorBehaviour).ActionMap["NeutralCommandTriggered"].performed += this.OnUserNeutralCommandTriggered;
                (this.Behaviour as ApollonHOTASWarthogThrottleSensorBehaviour).ActionMap["PositiveCommandTriggered"].performed += this.OnUserPositiveCommandTriggered;
                (this.Behaviour as ApollonHOTASWarthogThrottleSensorBehaviour).ActionMap["NegativeCommandTriggered"].performed += this.OnUserNegativeCommandTriggered;
                (this.Behaviour as ApollonHOTASWarthogThrottleSensorBehaviour).ActionMap["UserResponse"].performed += this.OnUserResponseTriggered;

            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                // remove them from bridge delegate
                (this.Behaviour as ApollonHOTASWarthogThrottleSensorBehaviour).ActionMap["ValueChanged"].performed -= this.OnAxisZValueChanged;
                (this.Behaviour as ApollonHOTASWarthogThrottleSensorBehaviour).ActionMap["NeutralCommandTriggered"].performed -= this.OnUserNeutralCommandTriggered;
                (this.Behaviour as ApollonHOTASWarthogThrottleSensorBehaviour).ActionMap["PositiveCommandTriggered"].performed -= this.OnUserPositiveCommandTriggered;
                (this.Behaviour as ApollonHOTASWarthogThrottleSensorBehaviour).ActionMap["NegativeCommandTriggered"].performed -= this.OnUserNegativeCommandTriggered;
                (this.Behaviour as ApollonHOTASWarthogThrottleSensorBehaviour).ActionMap["UserResponse"].performed -= this.OnUserResponseTriggered;

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;
                //await System.Threading.Tasks.Task.Run(
                //    async () =>
                //    {
                //        while (this.Behaviour.isActiveAndEnabled) await System.Threading.Tasks.Task.Delay(10);
                //    }
                //);

            } /* if() */

        } /* SetActive() */

        #endregion

        #region Event delegate

        private void OnAxisZValueChanged(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
           
            // dispatch event
            this.Dispatcher.RaiseAxisZValueChanged(context.ReadValue<float>());

            // update filtered tracker pos:rot
            var behaviour = this.Behaviour as ApollonHOTASWarthogThrottleSensorBehaviour;
            if(behaviour && behaviour.FilteredZAxisTracker) 
            {
                behaviour.FilteredZAxisTracker.transform.SetPositionAndRotation(
                    /* default */ 
                    behaviour.FilteredZAxisTracker.transform.position,
                    /* invert Z axe */
                    UnityEngine.Quaternion.Euler( 
                        UnityEngine.Vector3.right * context.ReadValue<float>()
                    )
                );
            }

        } /* OnAxisZValueChanged() */

        private void OnUserNeutralCommandTriggered(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            
            // check
            if (context.performed)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonHOTASWarthogThrottleSensorBridge.OnUserNeutralCommandTriggered() : event triggered !"
                );

                // dispatch event
                this.Dispatcher.RaiseUserNeutralCommandTriggered();

            } /* if() */

        } /* OnUserNeutralCommandTriggered() */

        private void OnUserPositiveCommandTriggered(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {

            // check
            if (context.performed)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonHOTASWarthogThrottleSensorBridge.OnUserPositiveCommandTriggered() : event triggered !"
                );

                // dispatch event
                this.Dispatcher.RaiseUserPositiveCommandTriggered();

            } /* if() */

        } /* OnUserPositiveCommandTriggered() */

        private void OnUserNegativeCommandTriggered(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
           
            // check
            if (context.performed)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonHOTASWarthogThrottleSensorBridge.OnUserNegativeCommandTriggered() : event triggered !"
                );

                // dispatch event
                this.Dispatcher.RaiseUserNegativeCommandTriggered();

            } /* if() */

        } /* OnUserNegativeCommandTriggered() */

        private void OnUserResponseTriggered(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {

            // check
            if (context.performed)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonHOTASWarthogThrottleSensorBridge.OnUserResponseTriggered() : event triggered !"
                );

                // dispatch event
                this.Dispatcher.RaiseUserResponseTriggered();

            } /* if() */

        } /* OnUserResponseTriggered() */

        #endregion

    }  /* class ApollonHOTASWarthogThrottleSensorBridge */

} /* } Labsim.apollon.gameplay.device.sensor */
