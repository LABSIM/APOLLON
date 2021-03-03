
// avoid namespace pollution
namespace Labsim.apollon.gameplay.element
{

    public class ApollonFogElementBehaviour 
        : UnityEngine.MonoBehaviour
    {

        #region Properties/Members

        // property
        private int SmoothingRemainingIncrementCounter { get; set; } = -1;

        public UnityEngine.FogMode CurrentFogMode { get; private set; }
        public float CurrentFogStartDistance { get; private set; }
        public float CurrentFogEndDistance { get; private set; }
        public UnityEngine.Color CurrentFogColor { get; private set; }

        private float FogStartDistanceIncrement { get; set; } = 0.0f;
        private float FogEndDistanceIncrement { get; set; } = 0.0f;
        private UnityEngine.Color FogColorIncrement { get; set; } = new UnityEngine.Color();

        #endregion

        #region Unity MonoBehaviour entry point

        private void Start()
        {

            // update values
            this.CurrentFogMode = UnityEngine.RenderSettings.fogMode;
            this.CurrentFogStartDistance = UnityEngine.RenderSettings.fogStartDistance;
            this.CurrentFogEndDistance = UnityEngine.RenderSettings.fogEndDistance;
            this.CurrentFogColor = UnityEngine.RenderSettings.fogColor;
            
        } /* Start() */

        private void FixedUpdate()
        {

            // check if there is remaining increment
            if(this.SmoothingRemainingIncrementCounter > 0) 
            {
                
                // switch on mode
                switch(this.CurrentFogMode)
                {
                    
                    case UnityEngine.FogMode.Linear: 
                    default:
                    {

                        // increment & update properties
                        this.CurrentFogStartDistance = UnityEngine.RenderSettings.fogStartDistance += this.FogStartDistanceIncrement;
                        this.CurrentFogEndDistance = UnityEngine.RenderSettings.fogEndDistance += this.FogEndDistanceIncrement;
                        this.CurrentFogColor = UnityEngine.RenderSettings.fogColor += this.FogColorIncrement;

                        break;

                    } /* -default- Linear */
                
                } /* switch() */

                // decrement smoothing counter
                --this.SmoothingRemainingIncrementCounter;

            } /* if() */

        } /* FixedUpdate() */

        private void Awake()
        {

            // inactive by default
            this.enabled = false;
            this.gameObject.SetActive(false);

        } /* Awake() */

        #endregion

        public void SetFogParameter(
            UnityEngine.FogMode fog_mode,
            float fog_start_distance,
            float fog_end_distance,
            UnityEngine.Color fog_color,
            float smoothing_duration = -1.0f
        ) {

            // switch on requested
            switch(fog_mode)
            {

                case UnityEngine.FogMode.Linear: 
                default:
                {

                     // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonFogElementBehaviour.SetFogParameter() : call with following parameter ["
                        + "mode:" 
                            + UnityEngine.FogMode.Linear 
                        + ",fog_start_distance:" 
                            + fog_start_distance
                        + ",fog_end_distance:" 
                            + fog_end_distance 
                        + ",fog_color:"
                            + fog_color
                        + ",smoothing_duration:"
                            + smoothing_duration
                        + "]"
                    );

                    // default
                    int smoothing_factor = 1;

                    // get smoothing factor
                    if(smoothing_duration != -1.0f) {
                        smoothing_factor 
                            = UnityEngine.Mathf.CeilToInt( 
                                smoothing_duration / ( UnityEngine.Time.fixedDeltaTime * 1000.0f ) 
                            );
                    }

                    // get increment from current
                    this.FogStartDistanceIncrement = ( fog_start_distance - this.CurrentFogStartDistance ) / smoothing_factor;
                    this.FogEndDistanceIncrement = ( fog_end_distance - this.CurrentFogEndDistance ) / smoothing_factor;
                    this.FogColorIncrement = ( fog_color - this.CurrentFogColor ) / smoothing_factor;

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonFogElementBehaviour.SetFogParameter() : calculated parameter ["
                        + "smoothing_factor:" 
                            + smoothing_factor
                        + ",FogStartDistanceIncrement:"
                            + this.FogStartDistanceIncrement
                        + ",FogEndDistanceIncrement:"
                            + this.FogEndDistanceIncrement
                        + ",FogColorIncrement:"
                            + this.FogColorIncrement
                        + "]"
                    );

                    // assign new mode & remaining counter
                    this.CurrentFogMode = fog_mode;
                    this.SmoothingRemainingIncrementCounter = smoothing_factor;

                    // end
                    break;

                } /* -default- Linear */

            } /* switch() */

        } /* SetFogParameter() */

    } /* public class ApollonFogElementBehaviour */

} /* } Labsim.apollon.gameplay.element */