//
// APOLLON : immersive & interactive experimental protocol engine
// Copyright (C) 2021-2023  Nawfel KINANI 
// nawfel (dot) kinani at onera (dot) fr
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program; see the files COPYING and COPYING.LESSER.
// If not, see <http://www.gnu.org/licenses/>.
//

// avoid namespace pollution
namespace Labsim.apollon.backend
{

    public abstract class ApollonAbstractTCPStreamHandle
        : ApollonAbstractStandardHandle
    {

        #region TCP/IP settings

        public static System.Int32 _s_port = 8888;
        public static System.Net.IPAddress _s_localAddr = System.Net.IPAddress.Loopback;
        private readonly System.Net.Sockets.TcpListener _server = new System.Net.Sockets.TcpListener(_s_localAddr, _s_port);

        private System.Net.Sockets.NetworkStream m_TCPStream = null;
        public System.Net.Sockets.NetworkStream TCPStream
        {
            get => this.m_TCPStream;
            private set => this.m_TCPStream = value;
        }

        private System.Diagnostics.Process m_TCPClientProcess = null;
        public System.Diagnostics.Process TCPClientProcess
        {
            get => this.m_TCPClientProcess;
            private set => this.m_TCPClientProcess = value;
        }

        #endregion

        #region Dispose pattern impl.

        protected sealed override void Dispose(bool bDisposing = true)
        {

            // TODO ?

        } /* Dispose(bool) */

        #endregion

        #region Standard HandleInit/HandleClose pattern impl.

        protected sealed override StatusIDType HandleInitialize()
        {
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.HandleInitialize() : on entry"
            );

            // encapsulate
            try
            {

                // Start the servere.
                _server.Start();

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.HandleInitialize() : TCP server started"
                );

                // wait for client
                System.Net.Sockets.TcpClient client = null;
                using (var acceptTask = _server.AcceptTcpClientAsync())
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.HandleInitialize() : server is async waiting for connection, launch client."
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
                        "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.HandleInitialize() : TCPClientProcess configured."
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
                        "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.HandleInitialize() : TCPClientProcess instantiated & started"
                    );

                    // start redirection
                    this.TCPClientProcess.BeginOutputReadLine();
                    this.TCPClientProcess.BeginErrorReadLine();

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.HandleInitialize() : TCPClientProcess output/error redirected"
                    );

                    // wait for client completion 
                    acceptTask.Wait();
                    client = acceptTask.Result;

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.HandleInitialize() : accepted client ["
                        + client.Client.LocalEndPoint
                        + "]."
                    );

                } /* using() */

                // Get the stream object for reading and writing
                this.TCPStream = client.GetStream();

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.HandleInitialize() : TCP stream ok, Initialization successfull"
                );

                // success
                return StatusIDType.Status_OK;

            }
            catch (System.IO.IOException ex)
            {

                // Catch the IOException that is raised if the pipe is broken
                // or disconnected.
                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonAbstractTCPStreamHandle.HandleInitialize() : "
                    + ex.Message
                );

            } /* try() */

            // whatever, fail
            UnityEngine.Debug.LogWarning(
                "<color=orange>Warning: </color> ApollonAbstractTCPStreamHandle.HandleInitialize() : Initialization failed..."
            );
            return StatusIDType.Status_ERROR;

        } /* HandleInitialize() */

        protected sealed override StatusIDType HandleClose()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.HandleClose() : entry"
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
                    "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.HandleClose() : close client process OK !"
                );

                // end server
                this.TCPStream.Close();
                _server.Stop();

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAbstractTCPStreamHandle.HandleClose() : close TCP server OK !"
                );

                // success
                return StatusIDType.Status_OK;

            }
            catch (System.IO.IOException ex)
            {

                // Catch the IOException that is raised if the pipe is broken
                // or disconnected.
                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonAbstractTCPStreamHandle.HandleClose() : "
                    + ex.Message
                );

            } /* try() */

            // whatever, fail
            UnityEngine.Debug.LogWarning(
                "<color=orange>Warning: </color> ApollonAbstractTCPStreamHandle.HandleClose() : Closure failed..."
            );
            return StatusIDType.Status_ERROR;

        } /* HandleClose() */

        #endregion

    } /* class ApollonAbstractTCPStreamHandle */
    
} /* } namespace */