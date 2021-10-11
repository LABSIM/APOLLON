// avoid namespace pollution
namespace Labsim.apollon.gameplay.device.impedance
{
    public class ApollonDualNormalizeProcessor : UnityEngine.InputSystem.InputProcessor<float>
    {

        public float positive_min = 0.5f;
        public float positive_max = 1.0f;
        public float negative_min = -0.5f;
        public float negative_max = -1.0f;

        public override float Process(float value, UnityEngine.InputSystem.InputControl control)
        {
            // normalize around -> continuous [-1.0, 1.0]
            return 
                (System.Math.Sign(value) > 0) 
                ? ((value - positive_min) / (positive_max - positive_min))
                : (-1.0f * ((value - negative_min) / (negative_max - negative_min)));

        } /* Process() */

    } /* class ApollonDualNormalizeProcessor */

} /* } Labsim.apollon.gameplay.device.sensor */
