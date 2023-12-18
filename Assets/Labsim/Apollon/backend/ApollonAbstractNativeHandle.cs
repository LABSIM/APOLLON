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

// using directives 
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;
using System.Security;
using Microsoft.Win32.SafeHandles;

// avoid namespace pollution
namespace Labsim.apollon.backend 
{

    public abstract class ApollonAbstractNativeHandle
        : ApollonAbstractHandle
    {

        #region .dll system safe handle

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        protected class NativeDLLSafeHandle
            : SafeHandleZeroOrMinusOneIsInvalid
        {

            private NativeDLLSafeHandle()
                : base(true)
            {
            }

            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
            protected override bool ReleaseHandle()
            {
                return NativeDLLInteropServices.FreeLibrary(this.handle);
            }

        } /* internal class NativeDLLSafeHandle */

        // handle
        protected NativeDLLSafeHandle m_handle = null;

        #endregion
        
        #region .dll system interop services (PInvoke)

        [SuppressUnmanagedCodeSecurity()]
        protected static class NativeDLLInteropServices
        {

            internal enum DirectoryFlags : uint
            {

                LOAD_LIBRARY_SEARCH_APPLICATION_DIR = 0x00000200,
                LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000,
                LOAD_LIBRARY_SEARCH_SYSTEM32 = 0x00000800,
                LOAD_LIBRARY_SEARCH_USER_DIRS = 0x00000400

            };

            [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal extern static bool SetDefaultDllDirectories(uint directoryFlags);

            [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
            internal extern static int AddDllDirectory(string lpPathName);

            [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true)]
            internal extern static NativeDLLSafeHandle LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpLibFileName);

            [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
            internal extern static System.IntPtr GetProcAddress(NativeDLLSafeHandle hModule, string lpProcName);

            [DllImport("kernel32", SetLastError = true)]
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal extern static bool FreeLibrary(System.IntPtr hModule);

        } /* internal class NativeMethods */

        #endregion

        #region .dll native library handling

        // fail safe mode. do not throw, use default implementation
        protected bool m_bEnableFailSafeMode = false;

        // Library loading
        protected abstract bool LoadNativeLibrary();

        protected abstract bool BindNativeLibrary();

        protected abstract bool ConstructNativeLibrary();

        protected abstract bool DisposeNativeLibrary();

        #endregion

        // ctor
        public ApollonAbstractNativeHandle()
            : base()
        { }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        protected override void Dispose(bool bDisposing = true)
        {

            // free lib
            if (!this.DisposeNativeLibrary())
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> ApollonAbstractNativeHandle.Dispose(" + bDisposing + ") : failed to dispose library. "
                    + Marshal.GetLastWin32Error()
                );

            } /* if() */

            // dispose safe handle
            if (this.m_handle != null && !this.m_handle.IsInvalid)
            {

                // free the library
                this.m_handle.Dispose();

            } /* if() */

        } /* Dispose(bool) */

        #region event handling 

        public override void OnHandleActivationRequested(object sender, ApollonBackendManager.EngineHandleEventArgs arg)
        {

            // check
            if (this.ID == arg.HandleID)
            {
                
                // Load native library
                if (!this.LoadNativeLibrary())
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonAbstractNativeHandle.OnHandleActivationRequested() : failed to load library. "
                        + Marshal.GetLastWin32Error()
                    );

                    // fail 
                    return;

                } /* if() */

                // bind delegate(s) 
                if (!this.BindNativeLibrary())
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonAbstractNativeHandle.OnHandleActivationRequested() : failed to bind library entries to delegates. "
                        + Marshal.GetLastWin32Error()
                    );

                    // fail 
                    return;

                } /* if() */

                // construct internal
                if (!this.ConstructNativeLibrary())
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonAbstractNativeHandle.OnHandleActivationRequested() : failed to construct library. "
                        + Marshal.GetLastWin32Error()
                    );

                    // fail 
                    return;

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

                // pull-up
                base.OnHandleDeactivationRequested(sender, arg);

            } /* if() */

        } /* OnHandleDeactivationRequested() */

        #endregion

    } /* class ApollonAbstractNativeHandle */

} /* } namespace */