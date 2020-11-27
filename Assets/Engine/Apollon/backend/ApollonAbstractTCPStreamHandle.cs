// using directives 
using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.backend
{

    public abstract class ApollonAbstractTCPStreamHandle
        : ApollonAbstractHandle
    {

        #region TCP/IP settings

        private static readonly System.Int32 _s_port = 8888;
        private static readonly System.Net.IPAddress _s_localAddr = System.Net.IPAddress.Loopback;
        private readonly System.Net.Sockets.TcpListener _server = new System.Net.Sockets.TcpListener(_s_localAddr, _s_port);

        private System.Net.Sockets.NetworkStream m_TCPStream = null;
        public System.Net.Sockets.NetworkStream TCPStream
        {
            get => m_TCPStream;
            private set => m_TCPStream = value;
        }

        private System.Diagnostics.Process m_TCPClientProcess = null;
        public System.Diagnostics.Process TCPClientProcess
        {
            get => m_TCPClientProcess;
            private set => m_TCPClientProcess = value;
        }

        private bool Initialize()
        {
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.Initialize() : on entry"
            );

            // encapsulate
            try
            {

                // Start the servere.
                _server.Start();

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.Initialize() : TCP server started"
                );

                // wait for client
                System.Net.Sockets.TcpClient client = null;
                using (var acceptTask = _server.AcceptTcpClientAsync())
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.Initialize() : server is async waiting for connection, launch client."
                    );

                    // Configure the client process. 
                    this.TCPClientProcess = new System.Diagnostics.Process();
                    this.TCPClientProcess.StartInfo.FileName
                        = System.IO.Path.Combine(
                            UnityEngine.Application.streamingAssetsPath,
                            "Apollon-feature-IxxatCAN/Apollon-feature-IxxatCAN-client.exe"
                        );
                    this.TCPClientProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    this.TCPClientProcess.StartInfo.UseShellExecute = false;
                    this.TCPClientProcess.StartInfo.RedirectStandardOutput = true;

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.Initialize() : TCPClientProcess instantiated & initialized."
                    );

                    // launch client process
                    this.TCPClientProcess.Start();

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.Initialize() : TCPClientProcess started"
                    );

                    // wait for client completion 
                    acceptTask.Wait();
                    client = acceptTask.Result;

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.Initialize() : accepted client ["
                        + client.Client.LocalEndPoint
                        + "]."
                    );

                } /* using() */

                // Get the stream object for reading and writing
                this.TCPStream = client.GetStream();

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.Initialize() : TCP stream ok, Initialization successfull"
                );

                // success
                return true;

            }
            catch (System.IO.IOException ex)
            {

                // Catch the IOException that is raised if the pipe is broken
                // or disconnected.
                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonAbstractTCPStreamHandle.Initialize() : "
                    + ex.Message
                );

            } /* try() */

            // whatever, fail
            UnityEngine.Debug.LogWarning(
                "<color=orange>Warning: </color> ApollonAbstractTCPStreamHandle.Initialize() : Initialization failed..."
            );
            return false;

        } /* Initialize() */

        private bool Close()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.Close() : entry"
            );

            // encapsulate
            try
            {

                // save output to file
                using (
                    System.IO.StreamWriter log_file
                        = new System.IO.StreamWriter(
                            System.IO.Path.Combine(
                                UnityEngine.Application.streamingAssetsPath,
                                "Apollon-feature-IxxatCAN/Apollon-feature-IxxatCAN-client.log"
                            )
                        )
                )
                {

                    log_file.Write(
                        this.TCPClientProcess.StandardOutput.ReadToEnd()
                    );

                } /* using */

                // close
                TCPStream.Close();
                _server.Stop();
                TCPClientProcess.Close();

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.Close() : close TCP system system OK !"
                );

                // success
                return true;

            }
            catch (System.IO.IOException ex)
            {

                // Catch the IOException that is raised if the pipe is broken
                // or disconnected.
                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonAbstractTCPStreamHandle.Close() : "
                    + ex.Message
                );

            } /* try() */

            // whatever, fail
            UnityEngine.Debug.LogWarning(
                "<color=orange>Warning: </color> ApollonAbstractTCPStreamHandle.Close() : Closure failed..."
            );
            return false;

        } /* Close() */
        
        #endregion

        // ctor     
        public ApollonAbstractTCPStreamHandle()
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
                    "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.onHandleActivationRequested() : initialize TCP communication system"
                );

                // init
                if (!this.Initialize())
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonAbstractTCPStreamHandle.onHandleActivationRequested() : failed to initialize connection, exit"
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
                    "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.onHandleDeactivationRequested() : close TCP communication system"
                );

                // close
                if (!this.Close())
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonAbstractTCPStreamHandle.onHandleDeactivationRequested() : failed to close connection, exit"
                    );

                    // abort
                    this.Dispose();

                } /* if() */

                // pull-up
                base.onHandleDeactivationRequested(sender, arg);

            } /* if() */

        } /* onHandleDeactivationRequested() */

        #endregion

    } /* class ApollonAbstractTCPStreamHandle */
    
} /* } namespace */