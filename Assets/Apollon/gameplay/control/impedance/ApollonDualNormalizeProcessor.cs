// avoid namespace pollution
namespace Labsim.apollon.gameplay.control.impedance
{

#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    public class ApollonDualNormalizeProcessor
        : UnityEngine.InputSystem.InputProcessor<float>
    {

        public float positive_min = 0.5f;
        public float positive_max = 1.0f;
        public float negative_min = -0.5f;
        public float negative_max = -1.0f;

#if UNITY_EDITOR
        static ApollonDualNormalizeProcessor()
        {
            Initialize();
        }
#endif

        [UnityEngine.RuntimeInitializeOnLoadMethod]
        static void Initialize()
        {
            UnityEngine.InputSystem.InputSystem.RegisterProcessor<ApollonDualNormalizeProcessor>();
        }

        public override float Process(float value, UnityEngine.InputSystem.InputControl control)
        {
            // normalize around -> continuous [-1.0, 1.0]
            return 
                (System.Math.Sign(value) > 0) 
                ? ((value - positive_min) / (positive_max - positive_min))
                : (-1.0f * ((value - negative_min) / (negative_max - negative_min)));

        } /* Process() */

    } /* class ApollonDualNormalizeProcessor */

} /* } Labsim.apollon.gameplay.control.impedance */
