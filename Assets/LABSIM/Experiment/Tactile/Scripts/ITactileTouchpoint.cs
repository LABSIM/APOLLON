// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public interface ITactileTouchpoint
    {

        float X { get; }
        float Y { get; }
        string HostTimestamp { get; }
        float UnityTimestamp { get; }
        UnityEngine.GameObject Reference { get; }

    } /* interface ITactileTouchpoint */ 

} /* } Labsim.experiment.tactile */