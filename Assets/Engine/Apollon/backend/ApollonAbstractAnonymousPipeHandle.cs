// using directives 
using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.backend
{

    public abstract class ApollonAbstractAnonymousPipeHandle
        : ApollonAbstractHandle
    {

        #region Anonymous pipe implementation details

        protected System.Diagnostics.Process PipeClient { get; private set; } = null;
        protected System.IO.Pipes.AnonymousPipeServerStream PipeServer { get; private set; } = null;
        protected System.IO.StreamWriter PipeBuffer { get; private set; } = null;

        private bool InitializePipeSystem()
        {

            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAbstractAnonymousPipeHandle.InitializePipeSystem() : on entry"
            );

            // our child client process
            this.PipeClient = new System.Diagnostics.Process();
            
            using (
                this.PipeServer
                    = new System.IO.Pipes.AnonymousPipeServerStream(
                        System.IO.Pipes.PipeDirection.Out,
                        System.IO.HandleInheritability.Inheritable
                    )
            )
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAbstractAnonymousPipeHandle.InitializePipeSystem() : Pipe server instantiated ! Current TransmissionMode: ["
                    + this.PipeServer.TransmissionMode
                    + "]."
                );
                
                // Pass the client process a handle to the server.
                this.PipeClient.StartInfo.FileName = "Apollon-feature-IxxatCAN-client.exe";
                this.PipeClient.StartInfo.Arguments = this.PipeServer.GetClientHandleAsString();
                this.PipeClient.StartInfo.UseShellExecute = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAbstractAnonymousPipeHandle.InitializePipeSystem() : Pipe client instantiated & initialized ! starting it."
                );

                // start client process & dispose local server copy
                this.PipeClient.Start();
                this.PipeServer.DisposeLocalCopyOfClientHandle();

                // simulate the "classical experiment scenario"

                // encapsulate
                try
                {
                    // Read user input and send that to the client process.
                    using (this.PipeBuffer = new System.IO.StreamWriter(this.PipeServer))
                    {
                        // mark as autoFlush
                        this.PipeBuffer.AutoFlush = true;

                        // log
                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> ApollonAbstractAnonymousPipeHandle.InitializePipeSystem() : initialize pipe system OK !"
                        );

                        // success
                        return true;

                    } /* using */

                }
                catch (System.IO.IOException ex)
                {

                    // Catch the IOException that is raised if the pipe is broken
                    // or disconnected.
                    UnityEngine.Debug.LogError(
                        "<color=red>Error: </color> ApollonAbstractAnonymousPipeHandle.InitializePipeSystem() : "
                        + ex.Message
                    );
                    
                } /* try() */

            } /* using */

            // whatever, fail
            return false;

        } /* InitializePipeSystem() */

        private bool ClosePipeSystem()
        {
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAbstractAnonymousPipeHandle.ClosePipeSystem() : entry"
            );

            // encapsulate
            try
            {

                // close
                this.PipeClient.WaitForExit();
                this.PipeClient.Close();

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAbstractAnonymousPipeHandle.ClosePipeSystem() : close pipe system OK !"
                );

                // success
                return true;

            }
            catch (System.IO.IOException ex)
            {

                // Catch the IOException that is raised if the pipe is broken
                // or disconnected.
                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonAbstractAnonymousPipeHandle.ClosePipeSystem() : "
                    + ex.Message
                );

            } /* try() */

            // whatever, fail
            return false;

        } /* ClosePipeSystem() */

        #endregion

        // ctor     
        public ApollonAbstractAnonymousPipeHandle()
            : base()
        { }

        protected override void Dispose(bool bDisposing = true)
        {


        } /* Dispose(bool) */

        #region event handling 

        public override void onHandleActivationRequested(object sender, ApollonBackendManager.EngineHandleEventArgs arg)
        {

            // check
            if (this.ID == arg.HandleID)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAbstractAnonymousPipeHandle.onHandleActivationRequested() : initialize anonymous pipe communication system"
                );

                // init
                if (!this.InitializePipeSystem())
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonAbstractAnonymousPipeHandle.onHandleActivationRequested() : failed to initialize connection, exit"
                    );

                    // abort
                    this.Dispose();

                } /* if() */

                // pull-up
                base.onHandleActivationRequested(sender, arg);

            } /* if() */

        } /* onHandleActivationRequested() */

        // unregistration
        public override void onHandleDeactivationRequested(object sender, ApollonBackendManager.EngineHandleEventArgs arg)
        {

            // check
            if (this.ID == arg.HandleID)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAbstractAnonymousPipeHandle.onHandleDeactivationRequested() : close anonymous pipe communication system"
                );

                // close
                if (!this.ClosePipeSystem())
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonAbstractAnonymousPipeHandle.onHandleDeactivationRequested() : failed to close connection, exit"
                    );

                    // abort
                    this.Dispose();

                } /* if() */

                // pull-up
                base.onHandleDeactivationRequested(sender, arg);

            } /* if() */

        } /* onHandleDeactivationRequested() */

        #endregion

    } /* class ApollonAbstractAnonymousPipeHandle */
    
} /* } namespace */