// avoid namespace pollution
namespace Labsim.apollon.gameplay.element
{

    public class ApollonFogElementBridge : ApollonAbstractGameplayBridge
    {

        //ctor
        public ApollonFogElementBridge()
            : base()
        { }

        public ApollonFogElementDispatcher Dispatcher { private set; get; } = null;

        #region Bridge abstract implementation 

        protected override UnityEngine.MonoBehaviour WrapBehaviour()
        {
            
            // retreive
            var behaviours = UnityEngine.Resources.FindObjectsOfTypeAll<ApollonFogElementBehaviour>();
            if ((behaviours?.Length ?? 0) == 0)
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warning: </color> ApollonFogElementBridge.WrapBehaviour() : could not find object of type behaviour.ApollonFogElementBehaviour from Unity."
                );

                return null;

            } /* if() */

            // instantiate
            this.Dispatcher = new ApollonFogElementDispatcher();

            // finally 
            // TODO : implement the logic of multiple instante (prefab)
            return behaviours[0];

        } /* WrapBehaviour() */

        protected override ApollonGameplayManager.GameplayIDType WrapID()
        {
            return ApollonGameplayManager.GameplayIDType.FogElement;
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

                // subscribe
                this.Dispatcher.ParameterChangedEvent += this.OnParameterChanged;

            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                // unscribe
                this.Dispatcher.ParameterChangedEvent -= this.OnParameterChanged;

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;

            } /* if() */

        } /* SetActive() */
        
        #endregion

        #region event delegate

        public void OnParameterChanged(object sender, ApollonFogElementDispatcher.EventArgs args)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonFogElementBridge.OnParameterChanged() : notifying Behaviour with args["
                + "fog_mode:"
                    + args.FogMode
                + ",fog_start_distance:"
                    + args.FogStartDistance
                + ",fog_end_distance:"
                    + args.FogEndDistance
                + ",fog_color:"
                    + args.FogColor
                + ",smoothing_duration:"
                    + args.SmoothingDuration
                + "]"
            );

            // Notify behaviour
            (this.Behaviour as ApollonFogElementBehaviour).SetFogParameter(
                fog_mode: 
                    args.FogMode,
                fog_start_distance:
                    args.FogStartDistance,
                fog_end_distance:
                    args.FogEndDistance,
                fog_color:
                    args.FogColor,
                smoothing_duration:
                    args.SmoothingDuration
            );

        } /* OnParameterChanged() */

        #endregion

    }  /* class ApollonFogElementBridge */

} /* } Labsim.apollon.gameplay.element */