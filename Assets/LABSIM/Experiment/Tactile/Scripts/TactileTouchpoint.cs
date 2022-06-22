// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public class TactileTouchpoint 
        : ITactileTouchpoint 
    {
        
        // Ctor
        public TactileTouchpoint(float x, float y, string timestamp, UnityEngine.GameObject obj)
        {
            
            this.X = x;
            this.Y = y;
            this.Timestamp = timestamp;
            this.Reference = obj;

        } /* Touchpoint() */

        // Property impl
        public float X { get; private set; } = -1.0f;
        public float Y { get; private set; } = -1.0f;
        public string Timestamp { get; private set; } = "-1.0f";
        public UnityEngine.GameObject Reference { get; private set; } = null;

    } /* class TactileTouchpoint */ 

} /* } Labsim.experiment.tactile */