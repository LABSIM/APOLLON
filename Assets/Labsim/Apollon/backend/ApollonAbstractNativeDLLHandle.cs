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

    public abstract class ApollonAbstractNativeDLLHandle
        : ApollonAbstractStandardHandle
    {

        #region .dll System safe handle

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, UnmanagedCode = true)]
        protected class NativeDLLSafeHandle
            :  Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
        {

            private NativeDLLSafeHandle()
                : base(true)
            {
            }

            [System.Runtime.ConstrainedExecution.ReliabilityContract(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
            protected override bool ReleaseHandle()
            {
                return NativeDLLInteropServices.FreeLibrary(this.handle);
            }

        } /* internal class NativeDLLSafeHandle */

        // handle
        protected NativeDLLSafeHandle m_handle = null;

        #endregion
        
        #region .dll System interop services (PInvoke)

        [System.Security.SuppressUnmanagedCodeSecurity()]
        protected static class NativeDLLInteropServices
        {

            internal enum DirectoryFlags : uint
            {

                LOAD_LIBRARY_SEARCH_APPLICATION_DIR = 0x00000200,
                LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000,
                LOAD_LIBRARY_SEARCH_SYSTEM32 = 0x00000800,
                LOAD_LIBRARY_SEARCH_USER_DIRS = 0x00000400

            };

            [System.Runtime.InteropServices.DllImport("kernel32", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
            [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
            internal extern static bool SetDefaultDllDirectories(uint directoryFlags);

            [System.Runtime.InteropServices.DllImport("kernel32", CharSet = System.Runtime.InteropServices.CharSet.Unicode, SetLastError = true)]
            internal extern static int AddDllDirectory(string lpPathName);

            [System.Runtime.InteropServices.DllImport("kernel32", CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
            internal extern static NativeDLLSafeHandle LoadLibrary([System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpLibFileName);

            [System.Runtime.InteropServices.DllImport("kernel32", CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
            internal extern static System.IntPtr GetProcAddress(NativeDLLSafeHandle hModule, string lpProcName);

            [System.Runtime.InteropServices.DllImport("kernel32", SetLastError = true)]
            [System.Runtime.ConstrainedExecution.ReliabilityContract(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
            [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
            internal extern static bool FreeLibrary(System.IntPtr hModule);

        } /* internal class NativeMethods */

        #endregion

        #region .dll Abstract native library handling decl.

        // fail safe mode. do not throw, use default implementation
        protected bool m_bEnableFailSafeMode = false;

        // Library loading
        protected abstract bool LoadNativeLibrary();

        protected abstract bool BindNativeLibrary();

        protected abstract bool ConstructNativeLibrary();

        protected abstract bool DisposeNativeLibrary();

        #endregion

        #region Dispose pattern impl.

        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, UnmanagedCode = true)]
        protected sealed override void Dispose(bool bDisposing = true)
        {

            // free lib
            if (!this.DisposeNativeLibrary())
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> ApollonAbstractNativeDLLHandle.Dispose(" + bDisposing + ") : failed to dispose library. "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

            } /* if() */

            // dispose safe handle
            if (this.m_handle != null && !this.m_handle.IsInvalid)
            {

                // free the library
                this.m_handle.Dispose();

            } /* if() */

        } /* Dispose(bool) */

        #endregion

        #region Standard HandleInit/HandleClose pattern impl.

        protected sealed override StatusIDType HandleInitialize()
        {

            // Load native library
            if (!this.LoadNativeLibrary())
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> ApollonAbstractNativeDLLHandle.HandleInitialize() : failed to load library. "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

                // fail 
                return StatusIDType.Status_ERROR;

            } /* if() */

            // bind delegate(s) 
            if (!this.BindNativeLibrary())
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> ApollonAbstractNativeDLLHandle.HandleInitialize() : failed to bind library entries to delegates. "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

                // fail 
                return StatusIDType.Status_ERROR;

            } /* if() */

            // construct internal
            if (!this.ConstructNativeLibrary())
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> ApollonAbstractNativeDLLHandle.HandleInitialize() : failed to construct library. "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

                // fail 
                return StatusIDType.Status_ERROR;

            } /* if() */

            // Success ! 
            return StatusIDType.Status_OK;

        } /* HandleInitialize() */

        protected sealed override StatusIDType HandleClose()
        {
            
            // whatever. Success ! do not unload DLL under windaube 
            return StatusIDType.Status_OK;

        } /* HandleClose() */

        #endregion

    } /* class ApollonAbstractNativeDLLHandle */

} /* } namespace */