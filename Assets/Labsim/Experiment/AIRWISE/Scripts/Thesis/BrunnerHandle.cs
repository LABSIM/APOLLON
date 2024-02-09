using UnityEngine;
using System;
// extensions
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;
using System.Security;

public class BrunnerHandle : System.IDisposable
{
    #region disposable pattern

    //private static bool bHasBeenDisposed = false;

    public void Dispose()
    {
        // Early bail out if already loaded
        if (BrunnerHandle.libLoaded)
        {
            return;
        }
        this.Dispose(true);
        System.GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool bDisposing)
    {
        if (bDisposing)
        {
            this.InternalDispose();
        }
    }

    #endregion

    #region singleton pattern

    private static readonly System.Lazy<BrunnerHandle> _lazyInstance
        = new System.Lazy<BrunnerHandle>(
            () =>
            {

                var instance = new BrunnerHandle();
                instance.InternalInit();
                return instance;
            }
        );

    public static BrunnerHandle Instance => _lazyInstance.Value;

    private BrunnerHandle() 
    {
    }

    ~BrunnerHandle()
    {
        this.Dispose(false);
    }

    private void InternalInit()
    {

        if (!BrunnerHandle.libLoaded)
        {
            // Load native library
            if (!this.LoadNativeLibrary())
            {
                // log
                Debug.LogError(
                    "<color=Red>Error: </color> " + this.GetType() + ".InternalInit(): failed to load library. "
                    + Marshal.GetLastWin32Error()
                );

                // fail 
                return;

            } /* if() */
        }

        if (!BrunnerHandle.libBinded)
        {
            // bind delegate(s) 
            if (!this.BindNativeLibrary())
            {
                // log
                Debug.LogError(
                    "<color=Red>Error: </color> " + this.GetType() + ".InternalInit(): failed to bind library entries to delegates. "
                    + Marshal.GetLastWin32Error()
                );

                // fail 
                return;

            } /* if() */
        }

        if (!BrunnerHandle.libConstructed)
        {
            // construct internal
            if (!this.ConstructNativeLibrary())
            {
                // log
                Debug.LogError(
                    "<color=Red>Error: </color> " + this.GetType() + ".InternalInit(): failed to construct library (Error "
                    + Marshal.GetLastWin32Error() +")."
                );

                // fail 
                return;

            } /* if() */
        }

    }
    private void InternalDispose()
    {
        if (BrunnerHandle.libLoaded & Application.isEditor)
        {
            return;
        }
        // Reset native library
        if (!this.ResetNativeLibrary())
        {
            // log
            Debug.LogError(
                "<color=Red>Error: </color> " + this.GetType() + ".InternalDispose(): failed to reset library. "
                + Marshal.GetLastWin32Error()
            );

            // fail 
            return;

        } /* if() */

        // unload native library
        if (!this.UnloadNativeLibrary())
        {
            // log
            Debug.LogError(
                "<color=Red>Error: </color> " + this.GetType() + ".InternalDispose(): failed to unload library. "
                + Marshal.GetLastWin32Error()
            );

            // fail 
            return;

        } /* if() */

    } 

    #endregion 

    #region .dll system safe handle

    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
    protected class NativeDLLSafeHandle: SafeHandleZeroOrMinusOneIsInvalid
    {
        private NativeDLLSafeHandle(): base(true)
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
    // When lib found and loaded in memory
    private static bool libLoaded = false;
    // When every function from lib binded
    private static bool libBinded = false;
    // When internal components initialized (all previous steps successful)
    private static bool libConstructed = false;
    public static bool libOperational { get { return BrunnerHandle.libLoaded && BrunnerHandle.libBinded && BrunnerHandle.libConstructed; } }

    #endregion

    #region .dll system interop services (PInvoke)

    [SuppressUnmanagedCodeSecurity()]
    protected static class NativeDLLInteropServices
    {
        internal enum DirectoryFlags: uint
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

    #region .dll settings

    internal class DLLSettings
    {
        // static settings
        public static readonly string
            _dllPath = UnityEngine.Application.streamingAssetsPath + @"\AIRWISE\Plugins\Brunner\",
            //_dllBoostPath = @"C:\DevEnv\Boost-1.61.0\lib\",
            _dllSystemPath = @"C:\Windows\System32\",
            _dllName = "BrunnerAPI.dll",

            /* model method call api */
            _dllInitializeEntryPointName = "BrunnerAPI_Initialize",
            _dllExecuteOneStepEntryPointName = "BrunnerAPI_ExecuteOneStep",
            _dllResetEntryPointName = "BrunnerAPI_Reset",

            /* data member read/write api */
            _dllReadPositionsEntryPointName = "BrunnerAPI_ReadPositions",
            _dllReadForcesEntryPointName = "BrunnerAPI_ReadForces",
            _dllReadButtonsEntryPointName = "BrunnerAPI_ReadButtons",
            _dllReadVirtualForceEntryPointName = "BrunnerAPI_ReadVirtualForce",
            _dllReadAxisRangeLimitEntryPointName = "BrunnerAPI_ReadAxisRangeLimits",
            _dllWriteForceProfileEntryPointName = "BrunnerAPI_WriteForceProfile",
            _dllWriteForceProfileXYEntryPointName = "BrunnerAPI_WriteForceProfileXY",
            _dllWriteForceScaleFactorEntryPointName = "BrunnerAPI_WriteForceScaleFactor",
            _dllWriteForceScaleFactorXYEntryPointName = "BrunnerAPI_WriteForceScaleFactorXY",
            _dllWriteFrictionEntryPointName = "BrunnerAPI_WriteFriction",
            _dllWriteFrictionXYEntryPointName = "BrunnerAPI_WriteFrictionXY",
            _dllWriteTrimPositionEntryPointName = "BrunnerAPI_WriteTrimPosition",
            _dllWriteTrimPositionXYEntryPointName = "BrunnerAPI_WriteTrimPositionXY",
            _dllWriteFrictionByVelocityEntryPointName = "BrunnerAPI_WriteFrictionByVelocity",
            _dllWriteFrictionXYByVelocityEntryPointName = "BrunnerAPI_WriteFrictionXYByVelocity",
            _dllWriteAxisRangeLimitEntryPointName = "BrunnerAPI_WriteAxisRangeLimits",
            _dllWriteAxisXYRangeLimitEntryPointName = "BrunnerAPI_WriteAxisXYRangeLimits",

            /* data member getter/setter api */
            _dllGetReturnBrunnerEntryPointName = "BrunnerAPI_GetReturnBrunner",
            _dllGetDDLEntryPointName = "BrunnerAPI_GetDDL",
            _dllGetDDMEntryPointName = "BrunnerAPI_GetDDM",
            _dllGetXEntryPointName = "BrunnerAPI_GetX",
            _dllGetYEntryPointName = "BrunnerAPI_GetY",
            _dllGetForceLateralEntryPointName = "BrunnerAPI_GetForceLateral",
            _dllGetForceLongitudinalEntryPointName = "BrunnerAPI_GetForceLongitudinal",
            _dllGetButton1EntryPointName = "BrunnerAPI_GetButton1",
            _dllGetButton2EntryPointName = "BrunnerAPI_GetButton2",
            _dllGetButton3EntryPointName = "BrunnerAPI_GetButton3",
            _dllGetButton4EntryPointName = "BrunnerAPI_GetButton4",
            _dllGetButton5EntryPointName = "BrunnerAPI_GetButton5",
            _dllGetButton6EntryPointName = "BrunnerAPI_GetButton6",
            _dllGetButton7EntryPointName = "BrunnerAPI_GetButton7",
            _dllGetButton8EntryPointName = "BrunnerAPI_GetButton8",
            _dllGetButton9EntryPointName = "BrunnerAPI_GetButton9",
            _dllGetButton10EntryPointName = "BrunnerAPI_GetButton10",
            _dllGetButton11EntryPointName = "BrunnerAPI_GetButton11",
            _dllGetButton12EntryPointName = "BrunnerAPI_GetButton12",
            _dllGetButton13EntryPointName = "BrunnerAPI_GetButton13",
            _dllGetButton14EntryPointName = "BrunnerAPI_GetButton14",
            _dllGetButton15EntryPointName = "BrunnerAPI_GetButton15",
            _dllGetButton16EntryPointName = "BrunnerAPI_GetButton16",
            _dllGetButton17EntryPointName = "BrunnerAPI_GetButton17",
            _dllGetButton18EntryPointName = "BrunnerAPI_GetButton18",
            _dllGetButton19EntryPointName = "BrunnerAPI_GetButton19",
            _dllGetHatUpEntryPointName = "BrunnerAPI_GetHatUp",
            _dllGetHatRightEntryPointName = "BrunnerAPI_GetHatRight",
            _dllGetHatDownEntryPointName = "BrunnerAPI_GetHatDown",
            _dllGetHatLeftEntryPointName = "BrunnerAPI_GetHatLeft",
            _dllGetVirtualForceLateralEntryPointName = "BrunnerAPI_GetVirtualForceLateral",
            _dllGetVirtualForceLongitudinalEntryPointName = "BrunnerAPI_GetVirtualForceLongitudinal",
            _dllGetLowerLimitLateralEntryPointName = "BrunnerAPI_GetLowerLimitLateral",
            _dllGetUpperLimitLateralEntryPointName = "BrunnerAPI_GetUpperLimitLateral",
            _dllGetCurrentLowerLimitLateralEntryPointName = "BrunnerAPI_GetCurrentLowerLimitLateral",
            _dllGetCurrentUpperLimitLateralEntryPointName = "BrunnerAPI_GetCurrentUpperLimitLateral",
            _dllGetLowerLimitLongitudinalEntryPointName = "BrunnerAPI_GetLowerLimitLongitudinal",
            _dllGetUpperLimitLongitudinalEntryPointName = "BrunnerAPI_GetUpperLimitLongitudinal",
            _dllGetCurrentLowerLimitLongitudinalEntryPointName = "BrunnerAPI_GetCurrentLowerLimitLongitudinal",
            _dllGetCurrentUpperLimitLongitudinalEntryPointName = "BrunnerAPI_GetCurrentUpperLimitLongitudinal",

            _dllTestBindingEntryPointName = "BrunnerAPI_TestBinding";
    } /* protected class DLLSettings*/

    #endregion

    #region .dll delegate interop mechanism

    // delegate prototype

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool InitializeDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool ExecuteOneStepDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool ResetDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool ReadPositionsDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool ReadForcesDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool ReadButtonsDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool ReadVirtualForceDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool ReadAxisRangeLimitDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool WriteForceProfileDelegate(UInt16 Lat0, UInt16 Lat1, UInt16 Lat2, UInt16 Lat3, UInt16 Lat4, UInt16 Lat5, UInt16 Lat6, UInt16 Lat7, UInt16 Lat8,
        UInt16 Longi0, UInt16 Longi1, UInt16 Longi2, UInt16 Longi3, UInt16 Longi4, UInt16 Longi5, UInt16 Longi6, UInt16 Longi7, UInt16 Longi8);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool WriteForceProfileXYDelegate(UInt16 X0, UInt16 X1, UInt16 X2, UInt16 X3, UInt16 X4, UInt16 X5, UInt16 X6, UInt16 X7, UInt16 X8,
        UInt16 Y0, UInt16 Y1, UInt16 Y2, UInt16 Y3, UInt16 Y4, UInt16 Y5, UInt16 Y6, UInt16 Y7, UInt16 Y8);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool WriteForceScaleFactorDelegate(UInt16 latFactor, UInt16 longiFactor);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool WriteForceScaleFactorXYDelegate(UInt16 xFactor, UInt16 yFactor);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool WriteFrictionDelegate(float latFriction, float longiFriction);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool WriteFrictionXYDelegate(float xFriction, float yFriction);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool WriteTrimPositionDelegate(float latPos, float longiPos);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool WriteTrimPositionXYDelegate(float xPos, float yPos);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool WriteFrictionByVelocityDelegate(float latFriction, float longiFriction);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool WriteFrictionXYByVelocityDelegate(float xFriction, float yFriction);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool WriteAxisRangeLimitDelegate(Int32 lowerLat, Int32 upperLat, Int32 lowerLongi, Int32 upperLongi);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool WriteAxisXYRangeLimitDelegate(Int32 lowerX, Int32 upperX, Int32 lowerY, Int32 upperY);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetReturnBrunnerDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.R8)]
    public delegate float GetDDLDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.R8)]
    public delegate float GetDDMDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.R8)]
    public delegate float GetXDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.R8)]
    public delegate float GetYDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.R8)]
    public delegate float GetForceLateralDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.R8)]
    public delegate float GetForceLongitudinalDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetButton1Delegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetButton2Delegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetButton3Delegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetButton4Delegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetButton5Delegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetButton6Delegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetButton7Delegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetButton8Delegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetButton9Delegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetButton10Delegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetButtonDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetButton11Delegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetButton12Delegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetButton13Delegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetButton14Delegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetButton15Delegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetButton16Delegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetButton17Delegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetButton18Delegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetButton19Delegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetHatUpDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetHatRightDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetHatDownDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool GetHatLeftDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.R8)]
    public delegate float GetVirtualForceLateralDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.R8)]
    public delegate float GetVirtualForceLongitudinalDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.R8)]
    public delegate float GetLowerLimitLateralDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.R8)]
    public delegate float GetUpperLimitLateralDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.R8)]
    public delegate float GetCurrentLowerLimitLateralDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.R8)]
    public delegate float GetCurrentUpperLimitLateralDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.R8)]
    public delegate float GetLowerLimitLongitudinalDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.R8)]
    public delegate float GetUpperLimitLongitudinalDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.R8)]
    public delegate float GetCurrentLowerLimitLongitudinalDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.R8)]
    public delegate float GetCurrentUpperLimitLongitudinalDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.R8)]
    public delegate float TestBindingDelegate();

    private InitializeDelegate m_initializeDelegate = null;
    public InitializeDelegate Initialize
    {
        get
        {
            if (this.m_initializeDelegate != null)
            {
                return this.m_initializeDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_initializeDelegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "Brunner::Initialize: InitializeDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }

        private set
        {
            this.m_initializeDelegate = value;
        }

    } /* Initialize */

    private ExecuteOneStepDelegate m_executeOneStepDelegate = null;
    public ExecuteOneStepDelegate ExecuteOneStep
    {
        get
        {
            if (this.m_executeOneStepDelegate != null)
            {
                return this.m_executeOneStepDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_executeOneStepDelegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "Brunner::ExecuteOneStep: ExecuteOneStepDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }

        private set
        {
            this.m_executeOneStepDelegate = value;
        }

    } /* ExecuteOneStep */

    private ResetDelegate m_resetDelegate = null;
    public ResetDelegate Reset
    {
        get
        {
            if (this.m_resetDelegate != null)
            {
                return this.m_resetDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_resetDelegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "Brunner::Reset: ResetDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }

        private set
        {
            this.m_resetDelegate = value;
        }

    } /* Reset */

    private ReadPositionsDelegate m_readPositionsDelegate = null;
    public ReadPositionsDelegate ReadPositions
    {
        get
        {
            if (this.m_readPositionsDelegate != null)
            {
                return this.m_readPositionsDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_readPositionsDelegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::ReadPositions: ReadPositionsDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }

        private set
        {
            this.m_readPositionsDelegate = value;
        }

    } /* ReadPositions */

    private ReadForcesDelegate m_readForcesDelegate = null;
    public ReadForcesDelegate ReadForces
    {
        get
        {
            if (this.m_readForcesDelegate != null)
            {
                return this.m_readForcesDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_readForcesDelegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::ReadForces: ReadForcesDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }

        private set
        {
            this.m_readForcesDelegate = value;
        }

    } /* ReadForces */

    private ReadButtonsDelegate m_readButtonsDelegate = null;
    public ReadButtonsDelegate ReadButtons
    {
        get
        {
            if (this.m_readButtonsDelegate != null)
            {
                return this.m_readButtonsDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_readButtonsDelegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::ReadButtons: ReadButtonsDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }

        private set
        {
            this.m_readButtonsDelegate = value;
        }

    } /* ReadButtons */

    private ReadVirtualForceDelegate m_readVirtualForceDelegate = null;
    public ReadVirtualForceDelegate ReadVirtualForce
    {
        get
        {
            if (this.m_readVirtualForceDelegate != null)
            {
                return this.m_readVirtualForceDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_readVirtualForceDelegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::ReadVirtualForce: ReadVirtualForceDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }

        private set
        {
            this.m_readVirtualForceDelegate = value;
        }

    } /* ReadVirtualForce */

    private ReadAxisRangeLimitDelegate m_readAxisRangeLimitDelegate = null;
    public ReadAxisRangeLimitDelegate ReadAxisRangeLimit
    {
        get
        {
            if (this.m_readAxisRangeLimitDelegate != null)
            {
                return this.m_readAxisRangeLimitDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_readAxisRangeLimitDelegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::ReadAxisRangeLimit: ReadAxisRangeLimitDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }

        private set
        {
            this.m_readAxisRangeLimitDelegate = value;
        }

    } /* ReadAxisRangeLimit */


    private WriteForceProfileDelegate m_writeForceProfileDelegate = null;
    public WriteForceProfileDelegate WriteForceProfile
    {
        get
        {
            if (this.m_writeForceProfileDelegate != null)
            {
                return this.m_writeForceProfileDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_writeForceProfileDelegate = (UInt16 Lat0, UInt16 Lat1, UInt16 Lat2, UInt16 Lat3, UInt16 Lat4, UInt16 Lat5, UInt16 Lat6, UInt16 Lat7, UInt16 Lat8,
                        UInt16 Longi0, UInt16 Longi1, UInt16 Longi2, UInt16 Longi3, UInt16 Longi4, UInt16 Longi5, UInt16 Longi6, UInt16 Longi7, UInt16 Longi8) => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::WriteForceProfile: WriteForceProfileDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }

        private set
        {
            this.m_writeForceProfileDelegate = value;
        }

    } /* WriteForceProfile */

    private WriteForceProfileXYDelegate m_writeForceProfileXYDelegate = null;
    public WriteForceProfileXYDelegate WriteForceProfileXY
    {
        get
        {
            if (this.m_writeForceProfileXYDelegate != null)
            {
                return this.m_writeForceProfileXYDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_writeForceProfileXYDelegate = (UInt16 Lat0, UInt16 Lat1, UInt16 Lat2, UInt16 Lat3, UInt16 Lat4, UInt16 Lat5, UInt16 Lat6, UInt16 Lat7, UInt16 Lat8,
                        UInt16 Longi0, UInt16 Longi1, UInt16 Longi2, UInt16 Longi3, UInt16 Longi4, UInt16 Longi5, UInt16 Longi6, UInt16 Longi7, UInt16 Longi8) => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::WriteForceProfileXY: WriteForceProfileXYDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }

        private set
        {
            this.m_writeForceProfileXYDelegate = value;
        }

    } /* WriteForceProfileXY */

    private WriteForceScaleFactorDelegate m_writeForceScaleFactorDelegate = null;
    public WriteForceScaleFactorDelegate WriteForceScaleFactor
    {
        get
        {
            if (this.m_writeForceScaleFactorDelegate != null)
            {
                return this.m_writeForceScaleFactorDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_writeForceScaleFactorDelegate = (UInt16 latFactor, UInt16 longiFactor) => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::WriteForceScaleFactor: WriteForceScaleFactorDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }

        private set
        {
            this.m_writeForceScaleFactorDelegate = value;
        }

    } /* WriteForceScaleFactor */

    private WriteForceScaleFactorXYDelegate m_writeForceScaleFactorXYDelegate = null;
    public WriteForceScaleFactorXYDelegate WriteForceScaleFactorXY
    {
        get
        {
            if (this.m_writeForceScaleFactorXYDelegate != null)
            {
                return this.m_writeForceScaleFactorXYDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_writeForceScaleFactorXYDelegate = (UInt16 latFactor, UInt16 longiFactor) => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::WriteForceScaleFactorXY: WriteForceScaleFactorXYDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }

        private set
        {
            this.m_writeForceScaleFactorXYDelegate = value;
        }

    } /* WriteForceScaleFactorXY */

    private WriteFrictionDelegate m_writeFrictionDelegate = null;
    public WriteFrictionDelegate WriteFriction
    {
        get
        {
            if (this.m_writeFrictionDelegate != null)
            {
                return this.m_writeFrictionDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_writeFrictionDelegate = (float latFriction, float longiFriction) => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::WriteFriction: WriteFrictionDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }

        private set
        {
            this.m_writeFrictionDelegate = value;
        }

    } /* WriteFriction */

    private WriteFrictionXYDelegate m_WriteFrictionXYDelegate = null;
    public WriteFrictionXYDelegate WriteFrictionXY
    {
        get
        {
            if (this.m_WriteFrictionXYDelegate != null)
            {
                return this.m_WriteFrictionXYDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_WriteFrictionXYDelegate = (float latFriction, float longiFriction) => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::WriteFrictionXY: WriteFrictionXYDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }

        private set
        {
            this.m_WriteFrictionXYDelegate = value;
        }

    } /* WriteFrictionXY */

    private WriteTrimPositionDelegate m_writeTrimPositionDelegate = null;
    public WriteTrimPositionDelegate WriteTrimPosition
    {
        get
        {
            if (this.m_writeTrimPositionDelegate != null)
            {
                return this.m_writeTrimPositionDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_writeTrimPositionDelegate = (float latPos, float longiPos) => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::WriteTrimPosition: WriteTrimPositionDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }

        private set
        {
            this.m_writeTrimPositionDelegate = value;
        }

    } /* WriteTrimPosition */

    private WriteTrimPositionXYDelegate m_writeTrimPositionXYDelegate = null;
    public WriteTrimPositionXYDelegate WriteTrimPositionXY
    {
        get
        {
            if (this.m_writeTrimPositionXYDelegate != null)
            {
                return this.m_writeTrimPositionXYDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_writeTrimPositionXYDelegate = (float xPos, float yPos) => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::WriteTrimPositionXY: WriteTrimPositionXYDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }

        private set
        {
            this.m_writeTrimPositionXYDelegate = value;
        }

    } /* WriteTrimPositionXY */

    private WriteFrictionByVelocityDelegate m_writeFrictionByVelocityDelegate = null;
    public WriteFrictionByVelocityDelegate WriteFrictionByVelocity
    {
        get
        {
            if (this.m_writeFrictionByVelocityDelegate != null)
            {
                return this.m_writeFrictionByVelocityDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_writeFrictionByVelocityDelegate = (float latFrictionByVelocity, float longiFrictionByVelocity) => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::WriteFrictionByVelocity: WriteFrictionByVelocityDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }

        private set
        {
            this.m_writeFrictionByVelocityDelegate = value;
        }

    } /* WriteFrictionByVelocity */

    private WriteFrictionXYByVelocityDelegate m_writeFrictionXYByVelocityDelegate = null;
    public WriteFrictionXYByVelocityDelegate WriteFrictionXYByVelocity
    {
        get
        {
            if (this.m_writeFrictionXYByVelocityDelegate != null)
            {
                return this.m_writeFrictionXYByVelocityDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_writeFrictionXYByVelocityDelegate = (float latFrictionXYByVelocity, float longiFrictionXYByVelocity) => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::WriteFrictionXYByVelocity: WriteFrictionXYByVelocityDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }

        private set
        {
            this.m_writeFrictionXYByVelocityDelegate = value;
        }

    } /* WriteFrictionXYByVelocity */

    private WriteAxisRangeLimitDelegate m_writeAxisRangeLimitDelegate = null;
    public WriteAxisRangeLimitDelegate WriteAxisRangeLimit
    {
        get
        {
            if (this.m_writeAxisRangeLimitDelegate != null)
            {
                return this.m_writeAxisRangeLimitDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_writeAxisRangeLimitDelegate = (Int32 lowerLat, Int32 upperLat, Int32 lowerLongi, Int32 upperLongi) => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::WriteAxisRangeLimit: WriteAxisRangeLimitDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_writeAxisRangeLimitDelegate = value;
        }
    } /* WriteAxisRangeLimit */

    private WriteAxisXYRangeLimitDelegate m_writeAxisXYRangeLimitDelegate = null;
    public WriteAxisXYRangeLimitDelegate WriteAxisXYRangeLimit
    {
        get
        {
            if (this.m_writeAxisXYRangeLimitDelegate != null)
            {
                return this.m_writeAxisXYRangeLimitDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_writeAxisXYRangeLimitDelegate = (Int32 lowerLat, Int32 upperLat, Int32 lowerLongi, Int32 upperLongi) => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::WriteAxisXYRangeLimit: WriteAxisXYRangeLimitDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_writeAxisXYRangeLimitDelegate = value;
        }
    } /* WriteAxisXYRangeLimit */

    private GetReturnBrunnerDelegate m_getReturnBrunnerDelegate = null;
    public GetReturnBrunnerDelegate GetReturnBrunner
    {
        get
        {
            if (this.m_getReturnBrunnerDelegate != null)
            {
                return this.m_getReturnBrunnerDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getReturnBrunnerDelegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetReturnBrunner: GetReturnBrunnerDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getReturnBrunnerDelegate = value;
        }
    } /* GetReturnBrunner */

    private GetDDLDelegate m_getDDLDelegate = null;
    public GetDDLDelegate GetDDL
    {
        get
        {
            if (this.m_getDDLDelegate != null)
            {
                return this.m_getDDLDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getDDLDelegate = () => { return 0.0f; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetDDL: GetDDLDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getDDLDelegate = value;
        }
    } /* GetDDL */

    private GetDDMDelegate m_getDDMDelegate = null;
    public GetDDMDelegate GetDDM
    {

        get
        {
            if (this.m_getDDMDelegate != null)
            {
                return this.m_getDDMDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getDDMDelegate = () => { return 0.0f; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetDDM: GetDDMDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getDDMDelegate = value;
        }
    } /* GetDDM */

    private GetXDelegate m_getXDelegate = null;
    public GetXDelegate GetX
    {

        get
        {
            if (this.m_getXDelegate != null)
            {
                return this.m_getXDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getXDelegate = () => { return 0.0f; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetX: GetXDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getXDelegate = value;
        }
    } /* GetX */

    private GetYDelegate m_getYDelegate = null;
    public GetYDelegate GetY
    {

        get
        {
            if (this.m_getYDelegate != null)
            {
                return this.m_getYDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getYDelegate = () => { return 0.0f; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetY: GetYDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getYDelegate = value;
        }
    } /* GetY */

    private GetForceLateralDelegate m_getForceLateralDelegate = null;
    public GetForceLateralDelegate GetForceLateral
    {

        get
        {
            if (this.m_getForceLateralDelegate != null)
            {
                return this.m_getForceLateralDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getForceLateralDelegate = () => { return 0.0f; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetForceLateral: GetForceLateralDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getForceLateralDelegate = value;
        }
    } /* GetForceLateral */

    private GetForceLongitudinalDelegate m_getForceLongitudinalDelegate = null;
    public GetForceLongitudinalDelegate GetForceLongitudinal
    {

        get
        {
            if (this.m_getForceLongitudinalDelegate != null)
            {
                return this.m_getForceLongitudinalDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getForceLongitudinalDelegate = () => { return 0.0f; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetForceLongitudinal: GetForceLongitudinalDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getForceLongitudinalDelegate = value;
        }
    } /* GetForceLongitudinal */

    private GetButton1Delegate m_getButton1Delegate = null;
    public GetButton1Delegate GetButton1
    {
        get
        {
            if (this.m_getButton1Delegate != null)
            {
                return this.m_getButton1Delegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getButton1Delegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetButton1: GetButton1Delegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getButton1Delegate = value;
        }
    } /* GetButton1 */

    private GetButton2Delegate m_getButton2Delegate = null;
    public GetButton2Delegate GetButton2
    {

        get
        {
            if (this.m_getButton2Delegate != null)
            {
                return this.m_getButton2Delegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getButton2Delegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetButton2: GetButton2Delegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getButton2Delegate = value;
        }
    } /* GetButton2 */

    private GetButton3Delegate m_getButton3Delegate = null;
    public GetButton3Delegate GetButton3
    {

        get
        {
            if (this.m_getButton3Delegate != null)
            {
                return this.m_getButton3Delegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getButton3Delegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetButton3: GetButton3Delegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getButton3Delegate = value;
        }
    } /* GetButton3 */

    private GetButton4Delegate m_getButton4Delegate = null;
    public GetButton4Delegate GetButton4
    {

        get
        {
            if (this.m_getButton4Delegate != null)
            {
                return this.m_getButton4Delegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getButton4Delegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetButton4: GetButton4Delegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getButton4Delegate = value;
        }
    } /* GetButton4 */

    private GetButton5Delegate m_getButton5Delegate = null;
    public GetButton5Delegate GetButton5
    {

        get
        {
            if (this.m_getButton5Delegate != null)
            {
                return this.m_getButton5Delegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getButton5Delegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetButton5: GetButton5Delegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getButton5Delegate = value;
        }
    } /* GetButton5 */

    private GetButton6Delegate m_getButton6Delegate = null;
    public GetButton6Delegate GetButton6
    {

        get
        {
            if (this.m_getButton6Delegate != null)
            {
                return this.m_getButton6Delegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getButton6Delegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetButton6: GetButton6Delegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getButton6Delegate = value;
        }
    } /* GetButton6 */

    private GetButton7Delegate m_getButton7Delegate = null;
    public GetButton7Delegate GetButton7
    {

        get
        {
            if (this.m_getButton7Delegate != null)
            {
                return this.m_getButton7Delegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getButton7Delegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetButton7: GetButton7Delegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getButton7Delegate = value;
        }
    } /* GetButton7 */

    private GetButton8Delegate m_getButton8Delegate = null;
    public GetButton8Delegate GetButton8
    {

        get
        {
            if (this.m_getButton8Delegate != null)
            {
                return this.m_getButton8Delegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getButton8Delegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetButton8: GetButton8Delegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getButton8Delegate = value;
        }
    } /* GetButton8 */

    private GetButton9Delegate m_getButton9Delegate = null;
    public GetButton9Delegate GetButton9
    {

        get
        {
            if (this.m_getButton9Delegate != null)
            {
                return this.m_getButton9Delegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getButton9Delegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetButton9: GetButton9Delegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getButton9Delegate = value;
        }
    } /* GetButton9 */

    private GetButton10Delegate m_getButton10Delegate = null;
    public GetButton10Delegate GetButton10
    {

        get
        {
            if (this.m_getButton10Delegate != null)
            {
                return this.m_getButton10Delegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getButton10Delegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetButton10: GetButton10Delegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getButton10Delegate = value;
        }
    } /* GetButton10 */

    private GetButton11Delegate m_getButton11Delegate = null;
    public GetButton11Delegate GetButton11
    {

        get
        {
            if (this.m_getButton11Delegate != null)
            {
                return this.m_getButton11Delegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getButton11Delegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetButton11: GetButton11Delegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getButton11Delegate = value;
        }
    } /* GetButton11 */

    private GetButton12Delegate m_getButton12Delegate = null;
    public GetButton12Delegate GetButton12
    {

        get
        {
            if (this.m_getButton12Delegate != null)
            {
                return this.m_getButton12Delegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getButton12Delegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetButton12: GetButton12Delegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getButton12Delegate = value;
        }
    } /* GetButton12 */

    private GetButton13Delegate m_getButton13Delegate = null;
    public GetButton13Delegate GetButton13
    {

        get
        {
            if (this.m_getButton13Delegate != null)
            {
                return this.m_getButton13Delegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getButton13Delegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetButton13: GetButton13Delegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getButton13Delegate = value;
        }
    } /* GetButton13 */

    private GetButton14Delegate m_getButton14Delegate = null;
    public GetButton14Delegate GetButton14
    {

        get
        {
            if (this.m_getButton14Delegate != null)
            {
                return this.m_getButton14Delegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getButton14Delegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetButton14: GetButton14Delegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getButton14Delegate = value;
        }
    } /* GetButton14 */

    private GetButton15Delegate m_getButton15Delegate = null;
    public GetButton15Delegate GetButton15
    {

        get
        {
            if (this.m_getButton15Delegate != null)
            {
                return this.m_getButton15Delegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getButton15Delegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetButton15: GetButton15Delegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getButton15Delegate = value;
        }
    } /* GetButton15 */

    private GetButton16Delegate m_getButton16Delegate = null;
    public GetButton16Delegate GetButton16
    {

        get
        {
            if (this.m_getButton16Delegate != null)
            {
                return this.m_getButton16Delegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getButton16Delegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetButton16: GetButton16Delegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getButton16Delegate = value;
        }
    } /* GetButton16 */

    private GetButton17Delegate m_getButton17Delegate = null;
    public GetButton17Delegate GetButton17
    {

        get
        {
            if (this.m_getButton17Delegate != null)
            {
                return this.m_getButton17Delegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getButton17Delegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetButton17: GetButton17Delegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getButton17Delegate = value;
        }
    } /* GetButton17 */

    private GetButton18Delegate m_getButton18Delegate = null;
    public GetButton18Delegate GetButton18
    {

        get
        {
            if (this.m_getButton18Delegate != null)
            {
                return this.m_getButton18Delegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getButton18Delegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetButton18: GetButton18Delegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getButton18Delegate = value;
        }
    } /* GetButton18 */

    private GetButton19Delegate m_getButton19Delegate = null;
    public GetButton19Delegate GetButton19
    {

        get
        {
            if (this.m_getButton19Delegate != null)
            {
                return this.m_getButton19Delegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getButton19Delegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetButton19: GetButton19Delegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getButton19Delegate = value;
        }
    } /* GetButton19 */

    private GetHatUpDelegate m_getHatUpDelegate = null;
    public GetHatUpDelegate GetHatUp
    {

        get
        {
            if (this.m_getHatUpDelegate != null)
            {
                return this.m_getHatUpDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getHatUpDelegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetHatUp: GetHatUpDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getHatUpDelegate = value;
        }
    } /* GetHatUp */

    private GetHatRightDelegate m_getHatRightDelegate = null;
    public GetHatRightDelegate GetHatRight
    {

        get
        {
            if (this.m_getHatRightDelegate != null)
            {
                return this.m_getHatRightDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getHatRightDelegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetHatRight: GetHatRightDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getHatRightDelegate = value;
        }
    } /* GetHatRight */

    private GetHatDownDelegate m_getHatDownDelegate = null;
    public GetHatDownDelegate GetHatDown
    {

        get
        {
            if (this.m_getHatDownDelegate != null)
            {
                return this.m_getHatDownDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getHatDownDelegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetHatDown: GetHatDownDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getHatDownDelegate = value;
        }
    } /* GetHatDown */

    private GetHatLeftDelegate m_getHatLeftDelegate = null;
    public GetHatLeftDelegate GetHatLeft
    {

        get
        {
            if (this.m_getHatLeftDelegate != null)
            {
                return this.m_getHatLeftDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getHatLeftDelegate = () => { return false; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetHatLeft: GetHatLeftDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getHatLeftDelegate = value;
        }
    } /* GetHatLeft */

    private GetVirtualForceLateralDelegate m_getVirtualForceLateralDelegate = null;
    public GetVirtualForceLateralDelegate GetVirtualForceLateral
    {

        get
        {
            if (this.m_getVirtualForceLateralDelegate != null)
            {
                return this.m_getVirtualForceLateralDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getVirtualForceLateralDelegate = () => { return 0.0f; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetVirtualForceLateral: GetVirtualForceLateralDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getVirtualForceLateralDelegate = value;
        }
    } /* GetVirtualForceLateral */

    private GetVirtualForceLongitudinalDelegate m_getVirtualForceLongitudinalDelegate = null;
    public GetVirtualForceLongitudinalDelegate GetVirtualForceLongitudinal
    {

        get
        {
            if (this.m_getVirtualForceLongitudinalDelegate != null)
            {
                return this.m_getVirtualForceLongitudinalDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getVirtualForceLongitudinalDelegate = () => { return 0.0f; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetVirtualForceLongitudinal: GetVirtualForceLongitudinalDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getVirtualForceLongitudinalDelegate = value;
        }
    } /* GetVirtualForceLongitudinal */

    private GetLowerLimitLateralDelegate m_getLowerLimitLateralDelegate = null;
    public GetLowerLimitLateralDelegate GetLowerLimitLateral
    {

        get
        {
            if (this.m_getLowerLimitLateralDelegate != null)
            {
                return this.m_getLowerLimitLateralDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getLowerLimitLateralDelegate = () => { return 0.0f; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetLowerLimitLateral: GetLowerLimitLateralDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getLowerLimitLateralDelegate = value;
        }
    } /* GetLowerLimitLateral */

    private GetUpperLimitLateralDelegate m_getUpperLimitLateralDelegate = null;
    public GetUpperLimitLateralDelegate GetUpperLimitLateral
    {

        get
        {
            if (this.m_getUpperLimitLateralDelegate != null)
            {
                return this.m_getUpperLimitLateralDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getUpperLimitLateralDelegate = () => { return 0.0f; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetUpperLimitLateral: GetUpperLimitLateralDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getUpperLimitLateralDelegate = value;
        }
    } /* GetUpperLimitLateral */

    private GetCurrentLowerLimitLateralDelegate m_getCurrentLowerLimitLateralDelegate = null;
    public GetCurrentLowerLimitLateralDelegate GetCurrentLowerLimitLateral
    {

        get
        {
            if (this.m_getCurrentLowerLimitLateralDelegate != null)
            {
                return this.m_getCurrentLowerLimitLateralDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getCurrentLowerLimitLateralDelegate = () => { return 0.0f; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetCurrentLowerLimitLateral: GetCurrentLowerLimitLateralDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getCurrentLowerLimitLateralDelegate = value;
        }
    } /* GetCurrentLowerLimitLateral */

    private GetCurrentUpperLimitLateralDelegate m_getCurrentUpperLimitLateralDelegate = null;
    public GetCurrentUpperLimitLateralDelegate GetCurrentUpperLimitLateral
    {

        get
        {
            if (this.m_getCurrentUpperLimitLateralDelegate != null)
            {
                return this.m_getCurrentUpperLimitLateralDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getCurrentUpperLimitLateralDelegate = () => { return 0.0f; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetCurrentUpperLimitLateral: GetCurrentUpperLimitLateralDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getCurrentUpperLimitLateralDelegate = value;
        }
    } /* GetCurrentUpperLimitLateral */

    private GetLowerLimitLongitudinalDelegate m_getLowerLimitLongitudinalDelegate = null;
    public GetLowerLimitLongitudinalDelegate GetLowerLimitLongitudinal
    {

        get
        {
            if (this.m_getLowerLimitLongitudinalDelegate != null)
            {
                return this.m_getLowerLimitLongitudinalDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getLowerLimitLongitudinalDelegate = () => { return 0.0f; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetLowerLimitLongitudinal: GetLowerLimitLongitudinalDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getLowerLimitLongitudinalDelegate = value;
        }
    } /* GetLowerLimitLongitudinal */

    private GetUpperLimitLongitudinalDelegate m_getUpperLimitLongitudinalDelegate = null;
    public GetUpperLimitLongitudinalDelegate GetUpperLimitLongitudinal
    {

        get
        {
            if (this.m_getUpperLimitLongitudinalDelegate != null)
            {
                return this.m_getUpperLimitLongitudinalDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getUpperLimitLongitudinalDelegate = () => { return 0.0f; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetUpperLimitLongitudinal: GetUpperLimitLongitudinalDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getUpperLimitLongitudinalDelegate = value;
        }
    } /* GetUpperLimitLongitudinal */

    private GetCurrentLowerLimitLongitudinalDelegate m_getCurrentLowerLimitLongitudinalDelegate = null;
    public GetCurrentLowerLimitLongitudinalDelegate GetCurrentLowerLimitLongitudinal
    {

        get
        {
            if (this.m_getCurrentLowerLimitLongitudinalDelegate != null)
            {
                return this.m_getCurrentLowerLimitLongitudinalDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getCurrentLowerLimitLongitudinalDelegate = () => { return 0.0f; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetCurrentLowerLimitLongitudinal: GetCurrentLowerLimitLongitudinalDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getCurrentLowerLimitLongitudinalDelegate = value;
        }
    } /* GetCurrentLowerLimitLongitudinal */

    private GetCurrentUpperLimitLongitudinalDelegate m_getCurrentUpperLimitLongitudinalDelegate = null;
    public GetCurrentUpperLimitLongitudinalDelegate GetCurrentUpperLimitLongitudinal
    {

        get
        {
            if (this.m_getCurrentUpperLimitLongitudinalDelegate != null)
            {
                return this.m_getCurrentUpperLimitLongitudinalDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_getCurrentUpperLimitLongitudinalDelegate = () => { return 0.0f; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::GetCurrentUpperLimitLongitudinal: GetCurrentUpperLimitLongitudinalDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_getCurrentUpperLimitLongitudinalDelegate = value;
        }
    } /* GetCurrentUpperLimitLongitudinal */

    private TestBindingDelegate m_testBindingDelegate = null;
    public TestBindingDelegate TestBinding
    {
        get
        {
            if (this.m_testBindingDelegate != null)
            {
                return this.m_testBindingDelegate;
            }
            else
            {
                if (this.m_bEnableFailSafeMode)
                {
                    return this.m_testBindingDelegate = () => { return 14.5f; };
                }
                else
                {
                    throw new System.Exception(
                        "BrunnerHandle::TestBinding: TestBindingDelegate not loaded... maybe you should load library prior to this call."
                    );
                }
            }
        }
        private set
        {
            this.m_testBindingDelegate = value;
        }
    } /* TestBinding */

    #endregion

    #region .dll native library handling

    // fail safe mode. do not throw, use default implementation
    protected bool m_bEnableFailSafeMode = true;
    protected bool LoadNativeLibrary()
    {
        // set default loading policies
        if (!NativeDLLInteropServices.SetDefaultDllDirectories((uint)NativeDLLInteropServices.DirectoryFlags.LOAD_LIBRARY_SEARCH_DEFAULT_DIRS))
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".LoadNativeLibrary(): failed to set default dll search path policy: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".LoadNativeLibrary(): set current search path policy via SetDefaultDllDirectories() syscall ["
            + NativeDLLInteropServices.DirectoryFlags.LOAD_LIBRARY_SEARCH_DEFAULT_DIRS
            + "]."
        );

        // add path to system
        if( 
            NativeDLLInteropServices.AddDllDirectory(DLLSettings._dllPath) == 0
            || NativeDLLInteropServices.AddDllDirectory(DLLSettings._dllSystemPath) == 0 
        ) {

            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".LoadNativeLibrary(): failed to add dll directory search path: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".LoadNativeLibrary(): adding current search path to process via AddDllDirectory() syscall ["
            + DLLSettings._dllPath
            + "]."
        );

        // load lib
        if ((this.m_handle = NativeDLLInteropServices.LoadLibrary(DLLSettings._dllName)).IsInvalid)
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".LoadNativeLibrary(): failed to load dll: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".LoadNativeLibrary(): loaded library handle via LoadLibraryEx() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // success
        BrunnerHandle.libLoaded = true;
        return true;

    } /* LoadNativeLibrary() */

    protected bool BindNativeLibrary()
    {
        // bind DLLSettings._dllInitializeEntryPointName entry point to delegate callback
        if (
            (
            this.Initialize
                = Marshal.GetDelegateForFunctionPointer<InitializeDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllInitializeEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind InitializeDelegate to corresponding entry point ["
                + DLLSettings._dllInitializeEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded InitializeDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllExecuteOneStepEntryPointName entry point to delegate callback
        if (
            (
            this.ExecuteOneStep
                = Marshal.GetDelegateForFunctionPointer<ExecuteOneStepDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllExecuteOneStepEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind ExecuteOneStepDelegate to corresponding entry point ["
                + DLLSettings._dllExecuteOneStepEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded ExecuteOneStepDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllResetEntryPointName entry point to delegate callback
        if (
            (
            this.Reset
                = Marshal.GetDelegateForFunctionPointer<ResetDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllResetEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind ResetDelegate to corresponding entry point ["
                + DLLSettings._dllResetEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded ResetDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllReadPositionsEntryPointName entry point to delegate callback
        if (
            (
            this.ReadPositions
                = Marshal.GetDelegateForFunctionPointer<ReadPositionsDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllReadPositionsEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind ReadPositionsDelegate to corresponding entry point ["
                + DLLSettings._dllReadPositionsEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded ReadPositionsDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllReadForcesEntryPointName entry point to delegate callback
        if (
            (
            this.ReadForces
                = Marshal.GetDelegateForFunctionPointer<ReadForcesDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllReadForcesEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind ReadForcesDelegate to corresponding entry point ["
                + DLLSettings._dllReadForcesEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded ReadForcesDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllReadButtonsEntryPointName entry point to delegate callback
        if (
            (
            this.ReadButtons
                = Marshal.GetDelegateForFunctionPointer<ReadButtonsDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllReadButtonsEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind ReadButtonsDelegate to corresponding entry point ["
                + DLLSettings._dllReadButtonsEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded ReadButtonsDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllReadVirtualForceEntryPointName entry point to delegate callback
        if (
            (
            this.ReadVirtualForce
                = Marshal.GetDelegateForFunctionPointer<ReadVirtualForceDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllReadVirtualForceEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind ReadVirtualForceDelegate to corresponding entry point ["
                + DLLSettings._dllReadVirtualForceEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded ReadVirtualForceDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllAxisRangeLimitEntryPointName entry point to delegate callback
        if (
            (
            this.ReadAxisRangeLimit
                = Marshal.GetDelegateForFunctionPointer<ReadAxisRangeLimitDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllReadAxisRangeLimitEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind ReadAxisRangeLimitDelegate to corresponding entry point ["
                + DLLSettings._dllReadAxisRangeLimitEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded ReadAxisRangeLimitDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllWriteForceProfileEntryPointName entry point to delegate callback
        if (
            (
            this.WriteForceProfile
                = Marshal.GetDelegateForFunctionPointer<WriteForceProfileDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllWriteForceProfileEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind WriteForceProfileDelegate to corresponding entry point ["
                + DLLSettings._dllWriteForceProfileEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded WriteForceProfileDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllWriteForceProfileXYEntryPointName entry point to delegate callback
        if (
            (
            this.WriteForceProfileXY
                = Marshal.GetDelegateForFunctionPointer<WriteForceProfileXYDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllWriteForceProfileXYEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind WriteForceProfileXYDelegate to corresponding entry point ["
                + DLLSettings._dllWriteForceProfileXYEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded WriteForceProfileXYDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllWriteForceScaleFactorEntryPointName entry point to delegate callback
        if (
            (
            this.WriteForceScaleFactor
                = Marshal.GetDelegateForFunctionPointer<WriteForceScaleFactorDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllWriteForceScaleFactorEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind WriteForceScaleFactorDelegate to corresponding entry point ["
                + DLLSettings._dllWriteForceScaleFactorEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded WriteForceScaleFactorDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllWriteForceScaleFactorXYEntryPointName entry point to delegate callback
        if (
            (
            this.WriteForceScaleFactorXY
                = Marshal.GetDelegateForFunctionPointer<WriteForceScaleFactorXYDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllWriteForceScaleFactorXYEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind WriteForceScaleFactorXYDelegate to corresponding entry point ["
                + DLLSettings._dllWriteForceScaleFactorXYEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded WriteForceScaleFactorXYDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllWriteFrictionEntryPointName entry point to delegate callback
        if (
            (
            this.WriteFriction
                = Marshal.GetDelegateForFunctionPointer<WriteFrictionDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllWriteFrictionEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind WriteFrictionDelegate to corresponding entry point ["
                + DLLSettings._dllWriteFrictionEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded WriteFrictionDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllWriteFrictionXYEntryPointName entry point to delegate callback
        if (
            (
            this.WriteFrictionXY
                = Marshal.GetDelegateForFunctionPointer<WriteFrictionXYDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllWriteFrictionXYEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind WriteFrictionXYDelegate to corresponding entry point ["
                + DLLSettings._dllWriteFrictionXYEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded WriteFrictionXYDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllWriteTrimPositionEntryPointName entry point to delegate callback
        if (
            (
            this.WriteTrimPosition
                = Marshal.GetDelegateForFunctionPointer<WriteTrimPositionDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllWriteTrimPositionEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind WriteTrimPositionDelegate to corresponding entry point ["
                + DLLSettings._dllWriteTrimPositionEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded WriteTrimPositionDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllWriteTrimPositionXYEntryPointName entry point to delegate callback
        if (
            (
            this.WriteTrimPositionXY
                = Marshal.GetDelegateForFunctionPointer<WriteTrimPositionXYDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllWriteTrimPositionXYEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind WriteTrimPositionXYDelegate to corresponding entry point ["
                + DLLSettings._dllWriteTrimPositionXYEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded WriteTrimPositionXYDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllWriteFrictionByVelocityEntryPointName entry point to delegate callback
        if (
            (
            this.WriteFrictionByVelocity
                = Marshal.GetDelegateForFunctionPointer<WriteFrictionByVelocityDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllWriteFrictionByVelocityEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind WriteFrictionByVelocityDelegate to corresponding entry point ["
                + DLLSettings._dllWriteFrictionByVelocityEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded WriteFrictionByVelocityDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllWriteFrictionXYByVelocityEntryPointName entry point to delegate callback
        if (
            (
            this.WriteFrictionXYByVelocity
                = Marshal.GetDelegateForFunctionPointer<WriteFrictionXYByVelocityDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllWriteFrictionXYByVelocityEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind WriteFrictionXYByVelocityDelegate to corresponding entry point ["
                + DLLSettings._dllWriteFrictionXYByVelocityEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded WriteFrictionXYByVelocityDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllWriteAxisRangeLimitEntryPointName entry point to delegate callback
        if (
            (
            this.WriteAxisRangeLimit
                = Marshal.GetDelegateForFunctionPointer<WriteAxisRangeLimitDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllWriteAxisRangeLimitEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind WriteAxisRangeLimitDelegate to corresponding entry point ["
                + DLLSettings._dllWriteAxisRangeLimitEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded WriteAxisRangeLimitDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllWriteAxisXYRangeLimitEntryPointName entry point to delegate callback
        if (
            (
            this.WriteAxisXYRangeLimit
                = Marshal.GetDelegateForFunctionPointer<WriteAxisXYRangeLimitDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllWriteAxisXYRangeLimitEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind WriteAxisXYRangeLimitDelegate to corresponding entry point ["
                + DLLSettings._dllWriteAxisXYRangeLimitEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded WriteAxisXYRangeLimitDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetReturnBrunnerEntryPointName entry point to delegate callback
        if (
            (
            this.GetReturnBrunner
                = Marshal.GetDelegateForFunctionPointer<GetReturnBrunnerDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetReturnBrunnerEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetReturnBrunnerDelegate to corresponding entry point ["
                + DLLSettings._dllGetReturnBrunnerEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetReturnBrunnerDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetDDLEntryPointName entry point to delegate callback
        if (
            (
            this.GetDDL
                = Marshal.GetDelegateForFunctionPointer<GetDDLDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetDDLEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetDDLDelegate to corresponding entry point ["
                + DLLSettings._dllGetDDLEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetDDLDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetDDMEntryPointName entry point to delegate callback
        if (
            (
            this.GetDDM
                = Marshal.GetDelegateForFunctionPointer<GetDDMDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetDDMEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetDDMDelegate to corresponding entry point ["
                + DLLSettings._dllGetDDMEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetDDMDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetXEntryPointName entry point to delegate callback
        if (
            (
            this.GetX
                = Marshal.GetDelegateForFunctionPointer<GetXDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetXEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetXDelegate to corresponding entry point ["
                + DLLSettings._dllGetXEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetXDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetYEntryPointName entry point to delegate callback
        if (
            (
            this.GetY
                = Marshal.GetDelegateForFunctionPointer<GetYDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetYEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetYDelegate to corresponding entry point ["
                + DLLSettings._dllGetYEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetYDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetForceLateralEntryPointName entry point to delegate callback
        if (
            (
            this.GetForceLateral
                = Marshal.GetDelegateForFunctionPointer<GetForceLateralDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetForceLateralEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetForceLateralDelegate to corresponding entry point ["
                + DLLSettings._dllGetForceLateralEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetForceLateralDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetForceLongitudinalEntryPointName entry point to delegate callback
        if (
            (
            this.GetForceLongitudinal
                = Marshal.GetDelegateForFunctionPointer<GetForceLongitudinalDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetForceLongitudinalEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetForceLongitudinalDelegate to corresponding entry point ["
                + DLLSettings._dllGetForceLongitudinalEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetForceLongitudinalDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetButton1EntryPointName entry point to delegate callback
        if (
            (
            this.GetButton1
                = Marshal.GetDelegateForFunctionPointer<GetButton1Delegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetButton1EntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetButton1Delegate to corresponding entry point ["
                + DLLSettings._dllGetButton1EntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetButton1Delegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetButton2EntryPointName entry point to delegate callback
        if (
            (
            this.GetButton2
                = Marshal.GetDelegateForFunctionPointer<GetButton2Delegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetButton2EntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetButton2Delegate to corresponding entry point ["
                + DLLSettings._dllGetButton2EntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetButton2Delegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetButton3EntryPointName entry point to delegate callback
        if (
            (
            this.GetButton3
                = Marshal.GetDelegateForFunctionPointer<GetButton3Delegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetButton3EntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetButton3Delegate to corresponding entry point ["
                + DLLSettings._dllGetButton3EntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetButton3Delegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetEntryPointName entry point to delegate callback
        if (
            (
            this.GetButton4
                = Marshal.GetDelegateForFunctionPointer<GetButton4Delegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetButton4EntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetButton4Delegate to corresponding entry point ["
                + DLLSettings._dllGetButton4EntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetButton4Delegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetButton5EntryPointName entry point to delegate callback
        if (
            (
            this.GetButton5
                = Marshal.GetDelegateForFunctionPointer<GetButton5Delegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetButton5EntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetButton5Delegate to corresponding entry point ["
                + DLLSettings._dllGetButton5EntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetButton5Delegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetButton6EntryPointName entry point to delegate callback
        if (
            (
            this.GetButton6
                = Marshal.GetDelegateForFunctionPointer<GetButton6Delegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetButton6EntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetButton6Delegate to corresponding entry point ["
                + DLLSettings._dllGetButton6EntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetButton6Delegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetEntryPointName entry point to delegate callback
        if (
            (
            this.GetButton7
                = Marshal.GetDelegateForFunctionPointer<GetButton7Delegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetButton7EntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetButton7Delegate to corresponding entry point ["
                + DLLSettings._dllGetButton7EntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetButton7Delegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetButton8EntryPointName entry point to delegate callback
        if (
            (
            this.GetButton8
                = Marshal.GetDelegateForFunctionPointer<GetButton8Delegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetButton8EntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetButton8Delegate to corresponding entry point ["
                + DLLSettings._dllGetButton8EntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetButton8Delegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetButton9EntryPointName entry point to delegate callback
        if (
            (
            this.GetButton9
                = Marshal.GetDelegateForFunctionPointer<GetButton9Delegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetButton9EntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetButton9Delegate to corresponding entry point ["
                + DLLSettings._dllGetButton9EntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetButton9Delegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetButton10EntryPointName entry point to delegate callback
        if (
            (
            this.GetButton10
                = Marshal.GetDelegateForFunctionPointer<GetButton10Delegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetButton10EntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetButton10Delegate to corresponding entry point ["
                + DLLSettings._dllGetButton10EntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetButton10Delegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetButton11EntryPointName entry point to delegate callback
        if (
            (
            this.GetButton11
                = Marshal.GetDelegateForFunctionPointer<GetButton11Delegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetButton11EntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetButton11Delegate to corresponding entry point ["
                + DLLSettings._dllGetButton11EntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetButton11Delegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetButton12EntryPointName entry point to delegate callback
        if (
            (
            this.GetButton12
                = Marshal.GetDelegateForFunctionPointer<GetButton12Delegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetButton12EntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetButton12Delegate to corresponding entry point ["
                + DLLSettings._dllGetButton12EntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetButton12Delegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetButton13EntryPointName entry point to delegate callback
        if (
            (
            this.GetButton13
                = Marshal.GetDelegateForFunctionPointer<GetButton13Delegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetButton13EntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetButton13Delegate to corresponding entry point ["
                + DLLSettings._dllGetButton13EntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetButton13Delegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetEntryPointName entry point to delegate callback
        if (
            (
            this.GetButton14
                = Marshal.GetDelegateForFunctionPointer<GetButton14Delegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetButton14EntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetButton14Delegate to corresponding entry point ["
                + DLLSettings._dllGetButton14EntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetButton14Delegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetButton15EntryPointName entry point to delegate callback
        if (
            (
            this.GetButton15
                = Marshal.GetDelegateForFunctionPointer<GetButton15Delegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetButton15EntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetButton15Delegate to corresponding entry point ["
                + DLLSettings._dllGetButton15EntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetButton15Delegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetButton16EntryPointName entry point to delegate callback
        if (
            (
            this.GetButton16
                = Marshal.GetDelegateForFunctionPointer<GetButton16Delegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetButton16EntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetButton16Delegate to corresponding entry point ["
                + DLLSettings._dllGetButton16EntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetButton16Delegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetEntryPointName entry point to delegate callback
        if (
            (
            this.GetButton17
                = Marshal.GetDelegateForFunctionPointer<GetButton17Delegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetButton17EntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetButton17Delegate to corresponding entry point ["
                + DLLSettings._dllGetButton17EntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetButton17Delegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetButton18EntryPointName entry point to delegate callback
        if (
            (
            this.GetButton18
                = Marshal.GetDelegateForFunctionPointer<GetButton18Delegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetButton18EntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetButton18Delegate to corresponding entry point ["
                + DLLSettings._dllGetButton18EntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetButton18Delegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetButton19EntryPointName entry point to delegate callback
        if (
            (
            this.GetButton19
                = Marshal.GetDelegateForFunctionPointer<GetButton19Delegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetButton19EntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetButton19Delegate to corresponding entry point ["
                + DLLSettings._dllGetButton19EntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetButton19Delegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetHatUpEntryPointName entry point to delegate callback
        if (
            (
            this.GetHatUp
                = Marshal.GetDelegateForFunctionPointer<GetHatUpDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetHatUpEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetHatUpDelegate to corresponding entry point ["
                + DLLSettings._dllGetHatUpEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetHatUpDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetHatRightEntryPointName entry point to delegate callback
        if (
            (
            this.GetHatRight
                = Marshal.GetDelegateForFunctionPointer<GetHatRightDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetHatRightEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetHatRightDelegate to corresponding entry point ["
                + DLLSettings._dllGetHatRightEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetHatRightDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetHatDownEntryPointName entry point to delegate callback
        if (
            (
            this.GetHatDown
                = Marshal.GetDelegateForFunctionPointer<GetHatDownDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetHatDownEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetHatDownDelegate to corresponding entry point ["
                + DLLSettings._dllGetHatDownEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetHatDownDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetEntryPointName entry point to delegate callback
        if (
            (
            this.GetHatLeft
                = Marshal.GetDelegateForFunctionPointer<GetHatLeftDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetHatLeftEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetHatLeftDelegate to corresponding entry point ["
                + DLLSettings._dllGetHatLeftEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetHatLeftDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetVirtualForceLateralEntryPointName entry point to delegate callback
        if (
            (
            this.GetVirtualForceLateral
                = Marshal.GetDelegateForFunctionPointer<GetVirtualForceLateralDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetVirtualForceLateralEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetVirtualForceLateralDelegate to corresponding entry point ["
                + DLLSettings._dllGetVirtualForceLateralEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetVirtualForceLateralDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetVirtualForceLongitudinalEntryPointName entry point to delegate callback
        if (
            (
            this.GetVirtualForceLongitudinal
                = Marshal.GetDelegateForFunctionPointer<GetVirtualForceLongitudinalDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetVirtualForceLongitudinalEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetVirtualForceLongitudinalDelegate to corresponding entry point ["
                + DLLSettings._dllGetVirtualForceLongitudinalEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetVirtualForceLongitudinalDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetEntryPointName entry point to delegate callback
        if (
            (
            this.GetLowerLimitLateral
                = Marshal.GetDelegateForFunctionPointer<GetLowerLimitLateralDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetLowerLimitLateralEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetLowerLimitLateralDelegate to corresponding entry point ["
                + DLLSettings._dllGetLowerLimitLateralEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetLowerLimitLateralDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetUpperLimitLateralEntryPointName entry point to delegate callback
        if (
            (
            this.GetUpperLimitLateral
                = Marshal.GetDelegateForFunctionPointer<GetUpperLimitLateralDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetUpperLimitLateralEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetUpperLimitLateralDelegate to corresponding entry point ["
                + DLLSettings._dllGetUpperLimitLateralEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetUpperLimitLateralDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetCurrentLowerLimitLateralEntryPointName entry point to delegate callback
        if (
            (
            this.GetCurrentLowerLimitLateral
                = Marshal.GetDelegateForFunctionPointer<GetCurrentLowerLimitLateralDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetCurrentLowerLimitLateralEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetCurrentLowerLimitLateralDelegate to corresponding entry point ["
                + DLLSettings._dllGetCurrentLowerLimitLateralEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetCurrentLowerLimitLateralDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetCurrentUpperLimitLateralEntryPointName entry point to delegate callback
        if (
            (
            this.GetCurrentUpperLimitLateral
                = Marshal.GetDelegateForFunctionPointer<GetCurrentUpperLimitLateralDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetCurrentUpperLimitLateralEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetCurrentUpperLimitLateralDelegate to corresponding entry point ["
                + DLLSettings._dllGetCurrentUpperLimitLateralEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetCurrentUpperLimitLateralDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetLowerLimitLongitudinalEntryPointName entry point to delegate callback
        if (
            (
            this.GetLowerLimitLongitudinal
                = Marshal.GetDelegateForFunctionPointer<GetLowerLimitLongitudinalDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetLowerLimitLongitudinalEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetLowerLimitLongitudinalDelegate to corresponding entry point ["
                + DLLSettings._dllGetLowerLimitLongitudinalEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetLowerLimitLongitudinalDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetUpperLimitLongitudinalEntryPointName entry point to delegate callback
        if (
            (
            this.GetUpperLimitLongitudinal
                = Marshal.GetDelegateForFunctionPointer<GetUpperLimitLongitudinalDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetUpperLimitLongitudinalEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetUpperLimitLongitudinalDelegate to corresponding entry point ["
                + DLLSettings._dllGetUpperLimitLongitudinalEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetUpperLimitLongitudinalDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetEntryPointName entry point to delegate callback
        if (
            (
            this.GetCurrentLowerLimitLongitudinal
                = Marshal.GetDelegateForFunctionPointer<GetCurrentLowerLimitLongitudinalDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetCurrentLowerLimitLongitudinalEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetCurrentLowerLimitLongitudinalDelegate to corresponding entry point ["
                + DLLSettings._dllGetCurrentLowerLimitLongitudinalEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetCurrentLowerLimitLongitudinalDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllGetCurrentUpperLimitLongitudinalEntryPointName entry point to delegate callback
        if (
            (
            this.GetCurrentUpperLimitLongitudinal
                = Marshal.GetDelegateForFunctionPointer<GetCurrentUpperLimitLongitudinalDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllGetCurrentUpperLimitLongitudinalEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind GetCurrentUpperLimitLongitudinalDelegate to corresponding entry point ["
                + DLLSettings._dllGetCurrentUpperLimitLongitudinalEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded GetCurrentUpperLimitLongitudinalDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // bind DLLSettings._dllTestBindingEntryPointName entry point to delegate callback
        if (
            (
            this.TestBinding
                = Marshal.GetDelegateForFunctionPointer<TestBindingDelegate>(
                    NativeDLLInteropServices.GetProcAddress(this.m_handle, DLLSettings._dllTestBindingEntryPointName)
                )
            ) == null
        )
        {
            Debug.LogError(
                "<color=red>Error: </color> " + this.GetType() + ".BindNativeLibrary(): failed to bind TestBindingDelegate to corresponding entry point ["
                + DLLSettings._dllTestBindingEntryPointName
                + "]: "
                + Marshal.GetLastWin32Error()
            );

            // fail
            return false;

        } /* if() */

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".BindNativeLibrary(): binded TestBindingDelegate entry point callback via GetProcAddress() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // success
        BrunnerHandle.libBinded = true;
        return true;

    } /* BindNativeLibrary() */

    protected bool ConstructNativeLibrary()
    {
        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".ConstructNativeLibrary(): initialize internal components ["
            + DLLSettings._dllName
            + "]."
        );

        // call delegate
        BrunnerHandle.libLoaded = true;
        BrunnerHandle.libBinded = true;
        BrunnerHandle.libConstructed = this.Initialize();
        return BrunnerHandle.libConstructed;

    } /* ConstructNativeLibrary() */

    protected bool ResetNativeLibrary()
    {
        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".ResetNativeLibrary(): deleting internal components ["
            + DLLSettings._dllName
            + "]."
        );

        // call delegate
        return this.Reset();

    } /* ResetNativeLibrary() */

    protected bool UnloadNativeLibrary()
    {
        // free library internal
        this.m_handle.Close();
        BrunnerHandle.libLoaded = false;
        //System.GC.SuppressFinalize(this.m_handle);
        //this.m_handle = null;

        // log
        Debug.Log(
            "<color=blue>Info: </color> " + this.GetType() + ".UnloadNativeLibrary(): unloaded library handle via LoadLibraryEx() syscall ["
            + DLLSettings._dllName
            + "]."
        );

        // success
        return true;

    } /* UnloadNativeLibrary() */

    #endregion

} /* class BrunnerHandle */