// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public class TactileTouchpoint 
        : ITactileTouchpoint 
    {
        
        // Ctor
        public TactileTouchpoint(float x, float y, string host_timestamp, float unity_timestamp, UnityEngine.GameObject obj)
        {
            
            this.X = x;
            this.Y = y;
            this.HostTimestamp = host_timestamp;
            this.UnityTimestamp = unity_timestamp;
            this.Reference = obj;

        } /* Touchpoint() */

        // Property impl
        public float X { get; private set; } = -1.0f;
        public float Y { get; private set; } = -1.0f;
        public string HostTimestamp { get; private set; } = "NULL";
        public float UnityTimestamp { get; private set; } = -1.0f;
        public UnityEngine.GameObject Reference { get; private set; } = null;

    } /* class TactileTouchpoint */ 

} /* } Labsim.experiment.tactile */