// avoid namespace pollution
namespace Labsim.apollon.backend.handle
{


    public class ApollonActiveSeatHandle
        : ApollonAbstractAnonymousPipeHandle
    {
        
        public void BeginSession()
        {

            this.PipeBuffer.WriteLine("BeginSession");
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatHandle.onHandleActivationRequested() : sended [BeginSession]."
            );
            this.PipeServer.WaitForPipeDrain();
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatHandle.onHandleActivationRequested() : pipe drained."
            );

        } /* BeginSession() */

        public void EndSession()
        {
            
            this.PipeBuffer.WriteLine("EndSession");
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatHandle.EndSession() : sended [EndSession]."
            );
            this.PipeServer.WaitForPipeDrain();
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatHandle.EndSession() : pipe drained."
            );

        } /* EndSession() */

        public void BeginTrial()
        {

            this.PipeBuffer.WriteLine("BeginTrial");
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatHandle.BeginTrial() : sended [BeginTrial]."
            );
            this.PipeServer.WaitForPipeDrain();
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatHandle.BeginTrial() : pipe drained."
            );

        } /* EndSession() */

        public void EndTrial()
        {

            this.PipeBuffer.WriteLine("EndTrial");
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatHandle.EndTrial() : sended [EndTrial]."
            );
            this.PipeServer.WaitForPipeDrain();
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatHandle.EndTrial() : pipe drained."
            );

        } /* EndSession() */

        public void Start(double AngularAcceleration, double AngularSpeedSaturation, double MaxStimDuration)
        {

            this.PipeBuffer.WriteLine("Start");
            this.PipeBuffer.WriteLine(AngularAcceleration);
            this.PipeBuffer.WriteLine(AngularSpeedSaturation);
            this.PipeBuffer.WriteLine(MaxStimDuration);
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatHandle.Start() : sended [Start] with args [dAngularAcceleration:"
                + AngularAcceleration
                + "], [dAngularSpeedSaturation:"
                + AngularSpeedSaturation
                + "], [dMaxStimDuration:"
                + MaxStimDuration
                + "] !"
            );
            this.PipeServer.WaitForPipeDrain();
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatHandle.Start() : pipe drained."
            );
            
        } /* EndSession() */

        public void Stop()
        {

            this.PipeBuffer.WriteLine("Stop");
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatHandle.Stop() : sended [Stop]."
            );
            this.PipeServer.WaitForPipeDrain();
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatHandle.Stop() : pipe drained."
            );

        } /* EndSession() */

        public void Reset()
        {

            this.PipeBuffer.WriteLine("Reset");
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatHandle.Reset() : sended [Reset]."
            );
            this.PipeServer.WaitForPipeDrain();
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonActiveSeatHandle.Reset() : pipe drained."
            );

        } /* EndSession() */
        
        // ctor
        public ApollonActiveSeatHandle()
            : base()
        {
            this.m_handleID = ApollonBackendManager.HandleIDType.ApollonActiveSeatHandle;
        }

    } /* class ApollonActiveSeatHandle */
    
} /* namespace Labsim.apollon.backend */
