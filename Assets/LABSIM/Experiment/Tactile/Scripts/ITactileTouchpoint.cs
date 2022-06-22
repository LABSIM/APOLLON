// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public interface ITactileTouchpoint
    {

        float X { get; }
        float Y { get; }
        string Timestamp { get; }
        UnityEngine.GameObject Reference { get; }

    } /* interface ITactileTouchpoint */ 

} /* } Labsim.experiment.tactile */