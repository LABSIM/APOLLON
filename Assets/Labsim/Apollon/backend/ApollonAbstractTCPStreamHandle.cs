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

        #endregion

        #region TCP/IP init/close methods

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
                    var info = new System.Diagnostics.ProcessStartInfo();
                    info.FileName
                        = System.IO.Path.Combine(
                            UnityEngine.Application.streamingAssetsPath,
                            "Apollon-feature-IxxatCAN/Apollon-gateway-ActiveSeat.exe"
                        );
                    info.CreateNoWindow = true;
                    info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    info.UseShellExecute = false;
                    info.RedirectStandardOutput = true;
                    info.RedirectStandardError = true;

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.Initialize() : TCPClientProcess configured."
                    );

                    // launch client process
                    this.TCPClientProcess = new System.Diagnostics.Process();
                    this.TCPClientProcess.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(
                        (sender, e) => {

                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.TCPClientProcess : process output message {"
                                + e.Data
                                + "}"
                            );
                        }
                    );
                    this.TCPClientProcess.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(
                        (sender, e) => {

                            UnityEngine.Debug.LogError(
                                "<color=Red>Error: </color> ApollonAbstractTCPStreamHandle.TCPClientProcess : process error message {"
                                + e.Data
                                + "}"
                            );
                        }
                    );
                    this.TCPClientProcess.StartInfo = info;
                    this.TCPClientProcess.Start();

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.Initialize() : TCPClientProcess instantiated & started"
                    );

                    // start redirection
                    this.TCPClientProcess.BeginOutputReadLine();
                    this.TCPClientProcess.BeginErrorReadLine();

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.Initialize() : TCPClientProcess output/error redirected"
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
                
                // wait for process end
                if (!this.TCPClientProcess.HasExited)
                {

                    // discard cached information about the process.
                    this.TCPClientProcess.Refresh();
                    this.TCPClientProcess.WaitForExit();

                } /* if() */

                // cancel output/error redirection
                this.TCPClientProcess.CancelOutputRead();
                this.TCPClientProcess.CancelErrorRead();

                // close process by sending a close message to its main window.
                this.TCPClientProcess.CloseMainWindow();

                // free resources associated with process.
                this.TCPClientProcess.Close();

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.Close() : close client process OK !"
                );

                // end server
                this.TCPStream.Close();
                _server.Stop();

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.Close() : close TCP server OK !"
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

        public override void OnHandleActivationRequested(object sender, ApollonBackendManager.EngineHandleEventArgs arg)
        {

            // check
            if (this.ID == arg.HandleID)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.OnHandleActivationRequested() : initialize TCP communication system"
                );

                // init
                if (!this.Initialize())
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonAbstractTCPStreamHandle.OnHandleActivationRequested() : failed to initialize connection, exit"
                    );

                    // abort
                    this.Dispose();

                } /* if() */

                // pull-up
                base.OnHandleActivationRequested(sender, arg);

            } /* if() */

        } /* OnHandleActivationRequested() */

        // unregistration
        public override void OnHandleDeactivationRequested(object sender, ApollonBackendManager.EngineHandleEventArgs arg)
        {

            // check
            if (this.ID == arg.HandleID)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.OnHandleDeactivationRequested() : close TCP communication system"
                );

                // close
                if (!this.Close())
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonAbstractTCPStreamHandle.OnHandleDeactivationRequested() : failed to close connection, exit"
                    );

                    // abort
                    this.Dispose();

                } /* if() */

                // pull-up
                base.OnHandleDeactivationRequested(sender, arg);

            } /* if() */

        } /* OnHandleDeactivationRequested() */

        #endregion

    } /* class ApollonAbstractTCPStreamHandle */
    
} /* } namespace */