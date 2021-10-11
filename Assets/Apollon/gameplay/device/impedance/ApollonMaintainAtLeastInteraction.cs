// avoid namespace pollution
namespace Labsim.apollon.gameplay.device.impedance
{
    public class ApollonMaintainAtLeastInteraction
        : UnityEngine.InputSystem.IInputInteraction
    {

        // parameter
        public float duration = 1.0f;
        public float threshold = 0.05f;
        public float min = 0.0f;
        public float max = 1.0f;

        // internal
        internal float refPoint = 0.0f;
        
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
                        (this.min <= context.ReadValue<float>())
                        && (context.ReadValue<float>() <= this.max)
                    )
                    {
                        context.Started();
                        context.SetTimeout(this.duration);
                        this.refPoint = context.ReadValue<float>();
                    }
                    break;

                case UnityEngine.InputSystem.InputActionPhase.Started:
                    // check bounds : [ greater((ref - threshold), min) <= val <= lesser((ref + threshold), max) ]
                    if (
                        ((this.refPoint - this.threshold) > context.ReadValue<float>())
                        || (context.ReadValue<float>() > (this.refPoint + this.threshold))
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
            this.refPoint = 0.0f;
        }

    } /* class ApollonHOTASWarthogThrottleSensorInteraction */

} /* } Labsim.apollon.gameplay.device.sensor */
