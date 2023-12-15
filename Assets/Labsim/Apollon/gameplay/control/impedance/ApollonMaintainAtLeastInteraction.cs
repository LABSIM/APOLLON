// avoid namespace pollution
namespace Labsim.apollon.gameplay.control.impedance
{

#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    public class ApollonHoldInRangeWithThreshold
        : UnityEngine.InputSystem.IInputInteraction
    {

        // parameter
        [UnityEngine.SerializeField, UnityEngine.Tooltip("hold duration to validate event in ms")]
        public float m_duration_ms = 1500.0f;
        [UnityEngine.SerializeField]
        [UnityEngine.Tooltip("threshold value in percentage relative to [lower_bound;upper_bound] range in order to invalidate event")]
        [UnityEngine.Range(0.0f, 100.0f)]
        public float m_threshold_ratio_percentage = 10.00f;
        [UnityEngine.SerializeField, UnityEngine.Tooltip("minimal value to start the hold event")]
        public float m_lower_bound = -1.0f;
        [UnityEngine.SerializeField, UnityEngine.Tooltip("maximal value to start the hold event")]
        public float m_upper_bound = 1.0f;

        // internal
        internal float m_current_reference_point = 0.0f;
        internal float m_current_lower_bound = 0.0f;
        internal float m_current_upper_bound = 0.0f;

#if UNITY_EDITOR
        static ApollonHoldInRangeWithThreshold()
        {
            Initialize();
        }
#endif

        [UnityEngine.RuntimeInitializeOnLoadMethod]
        static void Initialize()
        {
            UnityEngine.InputSystem.InputSystem.RegisterInteraction<ApollonHoldInRangeWithThreshold>();
        }

        public void Process(ref UnityEngine.InputSystem.InputInteractionContext context)
        {
           
            if (context.timerHasExpired)
            {

                // Success
                context.Performed();
                return;
            }

            switch (context.phase)
            {

                case UnityEngine.InputSystem.InputActionPhase.Waiting:
                    // check bounds : [min <= val <= max]
                    if (
                        (this.m_lower_bound <= context.ReadValue<float>())
                        && (context.ReadValue<float>() <= this.m_upper_bound)
                    )
                    {
                        
                        // calculation
                        this.m_current_reference_point = context.ReadValue<float>();
                        this.m_current_lower_bound 
                            = UnityEngine.Mathf.Clamp(
                                this.m_current_reference_point - ((this.m_upper_bound - this.m_lower_bound) * this.m_threshold_ratio_percentage / 100.0f),
                                this.m_lower_bound,
                                this.m_upper_bound
                            );
                        this.m_current_upper_bound 
                            = UnityEngine.Mathf.Clamp(
                                this.m_current_reference_point + ((this.m_upper_bound - this.m_lower_bound) * this.m_threshold_ratio_percentage / 100.0f),
                                this.m_lower_bound,
                                this.m_upper_bound
                            );

                        // launch
                        context.Started();
                        context.SetTimeout(this.m_duration_ms / 1000.0f);

                    }
                    break;

                case UnityEngine.InputSystem.InputActionPhase.Started:
                    // check bounds
                    if (
                        (this.m_current_lower_bound > context.ReadValue<float>())
                        || (context.ReadValue<float>() > this.m_current_upper_bound)
                    )
                    {
                        context.Canceled();
                        return;
                    }
                    break;

            } /* switch() */

        } /* Process() */

        public void Reset()
        {
            this.m_current_reference_point 
                = this.m_current_lower_bound 
                = this.m_current_upper_bound 
                = 0.0f;
        }

    } /* class ApollonHoldInRangeWithThreshold */

} /* } Labsim.apollon.gameplay.control.sensor */
