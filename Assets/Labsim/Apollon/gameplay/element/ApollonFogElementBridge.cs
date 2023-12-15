// avoid namespace pollution
namespace Labsim.apollon.gameplay.element
{

    public class ApollonFogElementBridge
        : ApollonGameplayBridge<ApollonFogElementBridge>
    {

        //ctor
        public ApollonFogElementBridge()
            : base()
        { }

        public ApollonFogElementBehaviour ConcreteBehaviour 
            => this.Behaviour as ApollonFogElementBehaviour;

        public ApollonFogElementDispatcher ConcreteDispatcher 
            => this.Dispatcher as ApollonFogElementDispatcher;

        #region Bridge abstract implementation
        
        protected override ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<ApollonFogElementBehaviour>(
                "ApollonFogElementBridge",
                "ApollonFogElementBehaviour"
            );

        } /* WrapBehaviour() */

        protected override ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<ApollonFogElementDispatcher>(
                "ApollonFogElementBridge",
                "ApollonFogElementDispatcher"
            );

        } /* WrapDispatcher() */

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
                this.ConcreteDispatcher.ParameterChangedEvent += this.OnParameterChanged;

            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                // unscribe
                this.ConcreteDispatcher.ParameterChangedEvent -= this.OnParameterChanged;

                // inactivate
                this.Behaviour.gameObject.SetActive(false);
                this.Behaviour.enabled = false;

            } /* if() */

        } /* SetActive() */
        
        #endregion

        #region event delegate

        public void OnParameterChanged(object sender, ApollonFogElementDispatcher.FogElementEventArgs args)
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