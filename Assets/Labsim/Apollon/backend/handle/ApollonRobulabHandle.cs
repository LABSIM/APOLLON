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

// extensions
// using System.Runtime.InteropServices;

// avoid namespace pollution
namespace Labsim.apollon.backend.handle
{

    public class ApollonRobulabHandle
        : ApollonAbstractNativeHandle
    {

        // ctor
        public ApollonRobulabHandle() 
            : base()
        {
            this.m_handleID = ApollonBackendManager.HandleIDType.ApollonRobulabHandle;
        }

        #region .dll settings

        internal class RobulabSpecificDLLSettings
        {

            // static settings
            public static readonly string
                _dllPath = @"C:\Users\LABSIM\dev\Robosoft\lib\Debug\",
                _dllBoostPath = @"C:\DevEnv\Boost-1.61.0\lib\",
                _dllName = "RoboSoft.dll",

                /* model method call api */
                _dllInstantiateEntryPointName = "RobulabInterop_Model_Instantiate",
                _dllDeleteEntryPointName = "RobulabInterop_Model_Delete",
                _dllInitializeEntryPointName = "RobulabInterop_Model_Initialize",
                _dllExecuteOneStepEntryPointName = "RobulabInterop_Model_ExecuteOneStep",

                /* data member getter/setter api */
                _dllGetIMUEntryPointName = "RobulabInterop_Sensor_GetIMU",
                _dllGetLocalizationEntryPointName = "RobulabInterop_Sensor_GetLocalization",
                _dllGetOdometryEntryPointName = "RobulabInterop_Sensor_GetOdometry",
                _dllGetBatteryLevelEntryPointName = "RobulabInterop_Sensor_GetBatteryLevel",
                _dllGetEmergencyStopStateEntryPointName = "RobulabInterop_Sensor_GetEmergencyStopState",
                _dllResetEmergencyStopStateEntryPointName = "RobulabInterop_Command_ResetEmergencyStopState",
                _dllResetOdometryEntryPointName = "RobulabInterop_Command_ResetOdometry",
                _dllResetLocalizationEntryPointName = "RobulabInterop_Command_ResetLocalization",
                _dllResetMissionEntryPointName = "RobulabInterop_Command_ResetMission",
                _dllSetLinearVelocityTargetEntryPointName = "RobulabInterop_Command_SetLinearVelocityTarget",
                _dllSetAngularVelocityTargetEntryPointName = "RobulabInterop_Command_SetAngularVelocityTarget",
                _dllStartMissionEntryPointName = "RobulabInterop_Command_StartMission",
                _dllStopMissionEntryPointName = "RobulabInterop_Command_StopMission";

        } /* protected class RobulabSpecificDLLSettings*/
        
        #endregion

        #region .dll delegate interop mechanism

        // delegate prototype

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public delegate bool InstantiateDelegate();

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public delegate bool DeleteDelegate();

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public delegate bool InitializeDelegate();

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public delegate bool ExecuteOneStepDelegate();

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public delegate bool GetIMUDelegate(
            ref System.Single Phi,
            ref System.Single Theta,
            ref System.Single Psi,
            ref System.Single AccX,
            ref System.Single AccY,
            ref System.Single AccZ
        );

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public delegate bool GetOdometryDelegate(
            ref System.Single PosX,
            ref System.Single PosY,
            ref System.Single Orientation
        );

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public delegate bool GetLocalizationDelegate(
            ref System.Single PosX,
            ref System.Single PosY,
            ref System.Single Orientation
        );

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public delegate bool GetBatteryLevelDelegate(
            ref System.Single BatteryLevel
        );

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public delegate bool GetEmergencyStopStateDelegate(
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)] ref System.Boolean State
        );

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public delegate bool ResetEmergencyStopStateDelegate();

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public delegate bool ResetOdometryDelegate();

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public delegate bool ResetLocalizationDelegate();

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public delegate bool ResetMissionDelegate();

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public delegate bool SetLinearVelocityTargetDelegate(
            System.Single MeterPerSecond
        );

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public delegate bool SetAngularVelocityTargetDelegate(
            System.Single RadianPerSecond
        );

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public delegate bool StartMissionDelegate();

        [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public delegate bool StopMissionDelegate();

        private InstantiateDelegate m_instantiateDelegate = null;
        public InstantiateDelegate Instantiate
        {

            get
            {
                if (this.m_instantiateDelegate != null)
                {
                    return this.m_instantiateDelegate;
                }
                else
                {
                    if (this.m_bEnableFailSafeMode)
                    {
                        return this.m_instantiateDelegate = () => { return true; };
                    }
                    else
                    {
                        throw new System.Exception(
                            "ApollonRobulabHandle::Instantiate : InstantiateDelegate not loaded... maybe you should load library prior to this call."
                        );
                    }
                }
            }

            private set
            {
                this.m_instantiateDelegate = value;
            }

        } /* Instantiate */

        private DeleteDelegate m_deleteDelegate = null;
        public DeleteDelegate Delete
        {

            get
            {
                if (this.m_deleteDelegate != null)
                {
                    return this.m_deleteDelegate;
                }
                else
                {
                    if (this.m_bEnableFailSafeMode)
                    {
                        return this.m_deleteDelegate = () => { return true; };
                    }
                    else
                    {
                        throw new System.Exception(
                            "ApollonRobulabHandle::Delete : DeleteDelegate not loaded... maybe you should load library prior to this call."
                        );
                    }
                }
            }

            private set
            {
                this.m_deleteDelegate = value;
            }

        } /* Delete */

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
                        return this.m_initializeDelegate = () => { return true; };
                    }
                    else
                    {
                        throw new System.Exception(
                            "ApollonRobulabHandle::Initialize : InitializeDelegate not loaded... maybe you should load library prior to this call."
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
                        return this.m_executeOneStepDelegate = () => { return true; };
                    }
                    else
                    {
                        throw new System.Exception(
                            "ApollonRobulabHandle::ExecuteOneStep : ExecuteOneStepDelegate not loaded... maybe you should load library prior to this call."
                        );
                    }
                }
            }

            private set
            {
                this.m_executeOneStepDelegate = value;
            }

        } /* ExecuteOneStep */

        private GetIMUDelegate m_getIMUDelegate = null;
        public GetIMUDelegate GetIMU
        {

            get
            {
                if (this.m_getIMUDelegate != null)
                {
                    return this.m_getIMUDelegate;
                }
                else
                {
                    if (this.m_bEnableFailSafeMode)
                    {
                        return this.m_getIMUDelegate = (
                            ref System.Single Phi,
                            ref System.Single Theta,
                            ref System.Single Psi,
                            ref System.Single AccX,
                            ref System.Single AccY,
                            ref System.Single AccZ
                        ) => {
                            Phi = Theta = Psi = AccX = AccY = AccZ = 0.0f;
                            return true;
                        };
                    }
                    else
                    {
                        throw new System.Exception(
                            "ApollonRobulabHandle::GetIMU : GetIMUDelegate not loaded... maybe you should load library prior to this call."
                        );
                    }
                }
            }

            private set
            {
                this.m_getIMUDelegate = value;
            }

        } /* GetIMU */

        private GetLocalizationDelegate m_getLocalizationDelegate = null;
        public GetLocalizationDelegate GetLocalization
        {

            get
            {
                if (this.m_getLocalizationDelegate != null)
                {
                    return this.m_getLocalizationDelegate;
                }
                else
                {
                    if (this.m_bEnableFailSafeMode)
                    {
                        return this.m_getLocalizationDelegate = (
                            ref System.Single PosX,
                            ref System.Single PosY,
                            ref System.Single Orientation
                        ) => {
                            PosX = PosY = Orientation = 0.0f;
                            return true;
                        };
                    }
                    else
                    {
                        throw new System.Exception(
                            "ApollonRobulabHandle::GetLocalization : GetLocalizationDelegate not loaded... maybe you should load library prior to this call."
                        );
                    }
                }
            }

            private set
            {
                this.m_getLocalizationDelegate = value;
            }

        } /* GetLocalization */

        private GetOdometryDelegate m_getOdometryDelegate = null;
        public GetOdometryDelegate GetOdometry
        {

            get
            {
                if (this.m_getOdometryDelegate != null)
                {
                    return this.m_getOdometryDelegate;
                }
                else
                {
                    if (this.m_bEnableFailSafeMode)
                    {
                        return this.m_getOdometryDelegate = (
                            ref System.Single PosX,
                            ref System.Single PosY,
                            ref System.Single Orientation
                        ) => {
                            PosX = PosY = Orientation = 0.0f;
                            return true;
                        };
                    }
                    else
                    {
                        throw new System.Exception(
                            "ApollonRobulabHandle::GetOdometry : GetOdometryDelegate not loaded... maybe you should load library prior to this call."
                        );
                    }
                }
            }

            private set
            {
                this.m_getOdometryDelegate = value;
            }

        } /* GetOdometry */

        private GetBatteryLevelDelegate m_getBatteryLevelDelegate = null;
        public GetBatteryLevelDelegate GetBatteryLevel
        {

            get
            {
                if (this.m_getBatteryLevelDelegate != null)
                {
                    return this.m_getBatteryLevelDelegate;
                }
                else
                {
                    if (this.m_bEnableFailSafeMode)
                    {
                        return this.m_getBatteryLevelDelegate = (
                            ref System.Single BatteryLevel
                        ) => {
                            BatteryLevel = 0.0f;
                            return true;
                        };
                    }
                    else
                    {
                        throw new System.Exception(
                            "ApollonRobulabHandle::GetBatteryLevel : GetBatteryLevelDelegate not loaded... maybe you should load library prior to this call."
                        );
                    }
                }
            }

            private set
            {
                this.m_getBatteryLevelDelegate = value;
            }

        } /* GetBatteryLevel */

        private GetEmergencyStopStateDelegate m_getEmergencyStopStateDelegate = null;
        public GetEmergencyStopStateDelegate GetEmergencyStopState
        {

            get
            {
                if (this.m_getEmergencyStopStateDelegate != null)
                {
                    return this.m_getEmergencyStopStateDelegate;
                }
                else
                {
                    if (this.m_bEnableFailSafeMode)
                    {
                        return this.m_getEmergencyStopStateDelegate = (
                            ref System.Boolean State
                        ) => {
                            State = false;
                            return true;
                        };
                    }
                    else
                    {
                        throw new System.Exception(
                            "ApollonRobulabHandle::GetEmergencyStopState : GetEmergencyStopStateDelegate not loaded... maybe you should load library prior to this call."
                        );
                    }
                }
            }

            private set
            {
                this.m_getEmergencyStopStateDelegate = value;
            }

        } /* GetEmergencyStopState */

        private ResetEmergencyStopStateDelegate m_resetEmergencyStopStateDelegate = null;
        public ResetEmergencyStopStateDelegate ResetEmergencyStopState
        {

            get
            {
                if (this.m_resetEmergencyStopStateDelegate != null)
                {
                    return this.m_resetEmergencyStopStateDelegate;
                }
                else
                {
                    if (this.m_bEnableFailSafeMode)
                    {
                        return this.m_resetEmergencyStopStateDelegate = () => { return true; };
                    }
                    else
                    {
                        throw new System.Exception(
                            "ApollonRobulabHandle::ResetEmergencyStopState : ResetEmergencyStopStateDelegate not loaded... maybe you should load library prior to this call."
                        );
                    }
                }
            }

            private set
            {
                this.m_resetEmergencyStopStateDelegate = value;
            }

        } /* ResetEmergencyStopState */

        private ResetOdometryDelegate m_resetOdometryDelegate = null;
        public ResetOdometryDelegate ResetOdometry
        {

            get
            {
                if (this.m_resetOdometryDelegate != null)
                {
                    return this.m_resetOdometryDelegate;
                }
                else
                {
                    if (this.m_bEnableFailSafeMode)
                    {
                        return this.m_resetOdometryDelegate = () => { return true; };
                    }
                    else
                    {
                        throw new System.Exception(
                            "ApollonRobulabHandle::ResetOdometry : ResetOdometryDelegate not loaded... maybe you should load library prior to this call."
                        );
                    }
                }
            }

            private set
            {
                this.m_resetOdometryDelegate = value;
            }

        } /* ResetOdometry */

        private ResetLocalizationDelegate m_resetLocalizationDelegate = null;
        public ResetLocalizationDelegate ResetLocalization
        {

            get
            {
                if (this.m_resetLocalizationDelegate != null)
                {
                    return this.m_resetLocalizationDelegate;
                }
                else
                {
                    if (this.m_bEnableFailSafeMode)
                    {
                        return this.m_resetLocalizationDelegate = () => { return true; };
                    }
                    else
                    {
                        throw new System.Exception(
                            "ApollonRobulabHandle::ResetLocalization : ResetLocalizationDelegate not loaded... maybe you should load library prior to this call."
                        );
                    }
                }
            }

            private set
            {
                this.m_resetLocalizationDelegate = value;
            }

        } /* ResetLocalization */

        private ResetMissionDelegate m_resetMissionDelegate = null;
        public ResetMissionDelegate ResetMission
        {

            get
            {
                if (this.m_resetMissionDelegate != null)
                {
                    return this.m_resetMissionDelegate;
                }
                else
                {
                    if (this.m_bEnableFailSafeMode)
                    {
                        return this.m_resetMissionDelegate = () => { return true; };
                    }
                    else
                    {
                        throw new System.Exception(
                            "ApollonRobulabHandle::ResetMission : ResetMissionDelegate not loaded... maybe you should load library prior to this call."
                        );
                    }
                }
            }

            private set
            {
                this.m_resetMissionDelegate = value;
            }

        } /* ResetMission */

        private SetLinearVelocityTargetDelegate m_setLinearVelocityTargetDelegate = null;
        public SetLinearVelocityTargetDelegate SetLinearVelocityTarget
        {

            get
            {
                if (this.m_setLinearVelocityTargetDelegate != null)
                {
                    return this.m_setLinearVelocityTargetDelegate;
                }
                else
                {
                    if (this.m_bEnableFailSafeMode)
                    {
                        return this.m_setLinearVelocityTargetDelegate = (
                             System.Single MeterPerSecond
                        ) => { return true; };
                    }
                    else
                    {
                        throw new System.Exception(
                            "ApollonRobulabHandle::SetLinearVelocityTarget : SetLinearVelocityTargetDelegate not loaded... maybe you should load library prior to this call."
                        );
                    }
                }
            }

            private set
            {
                this.m_setLinearVelocityTargetDelegate = value;
            }

        } /* SetLinearVelocityTarget */

        private SetAngularVelocityTargetDelegate m_setAngularVelocityTargetDelegate = null;
        public SetAngularVelocityTargetDelegate SetAngularVelocityTarget
        {

            get
            {
                if (this.m_setAngularVelocityTargetDelegate != null)
                {
                    return this.m_setAngularVelocityTargetDelegate;
                }
                else
                {
                    if (this.m_bEnableFailSafeMode)
                    {
                        return this.m_setAngularVelocityTargetDelegate = (
                            System.Single RadianPerSecond
                        ) => { return true; };
                    }
                    else
                    {
                        throw new System.Exception(
                            "ApollonRobulabHandle::SetAngularVelocityTarget : SetAngularVelocityTargetDelegate not loaded... maybe you should load library prior to this call."
                        );
                    }
                }
            }

            private set
            {
                this.m_setAngularVelocityTargetDelegate = value;
            }

        } /* SetAngularVelocityTarget */

        private StartMissionDelegate m_startMissionDelegate = null;
        public StartMissionDelegate StartMission
        {

            get
            {
                if (this.m_startMissionDelegate != null)
                {
                    return this.m_startMissionDelegate;
                }
                else
                {
                    if (this.m_bEnableFailSafeMode)
                    {
                        return this.m_startMissionDelegate = () => { return true; };
                    }
                    else
                    {
                        throw new System.Exception(
                            "ApollonRobulabHandle::StartMission : StartMissionDelegate not loaded... maybe you should load library prior to this call."
                        );
                    }
                }
            }

            private set
            {
                this.m_startMissionDelegate = value;
            }

        } /* StartMission */

        private StopMissionDelegate m_stopMissionDelegate = null;
        public StopMissionDelegate StopMission
        {

            get
            {
                if (this.m_stopMissionDelegate != null)
                {
                    return this.m_stopMissionDelegate;
                }
                else
                {
                    if (this.m_bEnableFailSafeMode)
                    {
                        return this.m_stopMissionDelegate = () => { return true; };
                    }
                    else
                    {
                        throw new System.Exception(
                            "ApollonRobulabHandle::StopMission : StopMissionDelegate not loaded... maybe you should load library prior to this call."
                        );
                    }
                }
            }

            private set
            {
                this.m_stopMissionDelegate = value;
            }

        } /* StopMission */

        #endregion

        #region .dll native library handling

        protected override bool LoadNativeLibrary()
        {

            // set default loading policies
            if (!NativeDLLInteropServices.SetDefaultDllDirectories((uint)NativeDLLInteropServices.DirectoryFlags.LOAD_LIBRARY_SEARCH_DEFAULT_DIRS))
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonRobulabHandle.LoadNativeLibrary() : failed to set default dll search path policy : "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

                // fail
                return false;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonRobulabHandle.LoadNativeLibrary() : set current search path policy via SetDefaultDllDirectories() syscall ["
                + NativeDLLInteropServices.DirectoryFlags.LOAD_LIBRARY_SEARCH_DEFAULT_DIRS
                + "]."
            );

            // add path to system
            if (
                (NativeDLLInteropServices.AddDllDirectory(RobulabSpecificDLLSettings._dllPath) == 0)
                || (NativeDLLInteropServices.AddDllDirectory(RobulabSpecificDLLSettings._dllBoostPath) == 0)
            )
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonRobulabHandle.LoadNativeLibrary() : failed to add dll directory search path : "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

                // fail
                return false;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonRobulabHandle.LoadNativeLibrary() : adding current search path to process via AddDllDirectory() syscall ["
                + RobulabSpecificDLLSettings._dllPath
                + " | "
                + RobulabSpecificDLLSettings._dllBoostPath
                + "]."
            );

            // load lib
            if ((this.m_handle = NativeDLLInteropServices.LoadLibrary(RobulabSpecificDLLSettings._dllName)).IsInvalid)
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonRobulabHandle.LoadNativeLibrary() : failed to load dll : "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

                // fail
                return false;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonRobulabHandle.LoadNativeLibrary() : loaded library handle via LoadLibraryEx() syscall ["
                + RobulabSpecificDLLSettings._dllName
                + "]."
            );

            // success
            return true;

        } /* LoadNativeLibrary() */

        protected override bool BindNativeLibrary()
        {

            // bind RobulabSpecificDLLSettings._dllInstantiateEntryPointName entry point to delegate callback
            if (
                (
                this.Instantiate
                    = System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<InstantiateDelegate>(
                        NativeDLLInteropServices.GetProcAddress(this.m_handle, RobulabSpecificDLLSettings._dllInstantiateEntryPointName)
                    )
                ) == null
            )
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonRobulabHandle.BindNativeLibrary() : failed to bind InstantiateDelegate to corresponding entry point ["
                    + RobulabSpecificDLLSettings._dllInstantiateEntryPointName
                    + "] : "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

                // fail
                return false;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonRobulabHandle.BindNativeLibrary() : binded InstantiateDelegate entry point callback via GetProcAddress() syscall ["
                + RobulabSpecificDLLSettings._dllName
                + "]."
            );

            // bind RobulabSpecificDLLSettings._dllDeleteEntryPointName entry point to delegate callback
            if (
                (
                this.Delete
                    = System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<DeleteDelegate>(
                        NativeDLLInteropServices.GetProcAddress(this.m_handle, RobulabSpecificDLLSettings._dllDeleteEntryPointName)
                    )
                ) == null
            )
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonRobulabHandle.BindNativeLibrary() : failed to bind DeleteDelegate to corresponding entry point ["
                    + RobulabSpecificDLLSettings._dllDeleteEntryPointName
                    + "] : "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

                // fail
                return false;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonRobulabHandle.BindNativeLibrary() : binded DeleteDelegate entry point callback via GetProcAddress() syscall ["
                + RobulabSpecificDLLSettings._dllName
                + "]."
            );

            // bind RobulabSpecificDLLSettings._dllInitializeEntryPointName entry point to delegate callback
            if (
                (
                this.Initialize
                    = System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<InitializeDelegate>(
                        NativeDLLInteropServices.GetProcAddress(this.m_handle, RobulabSpecificDLLSettings._dllInitializeEntryPointName)
                    )
                ) == null
            )
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonRobulabHandle.BindNativeLibrary() : failed to bind InitializeDelegate to corresponding entry point ["
                    + RobulabSpecificDLLSettings._dllInitializeEntryPointName
                    + "] : "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

                // fail
                return false;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonRobulabHandle.BindNativeLibrary() : binded InitializeDelegate entry point callback via GetProcAddress() syscall ["
                + RobulabSpecificDLLSettings._dllName
                + "]."
            );

            // bind RobulabSpecificDLLSettings._dllExecuteOneStepEntryPointName entry point to delegate callback
            if (
                (
                this.ExecuteOneStep
                    = System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<ExecuteOneStepDelegate>(
                        NativeDLLInteropServices.GetProcAddress(this.m_handle, RobulabSpecificDLLSettings._dllExecuteOneStepEntryPointName)
                    )
                ) == null
            )
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonRobulabHandle.BindNativeLibrary() : failed to bind ExecuteOneStepDelegate to corresponding entry point ["
                    + RobulabSpecificDLLSettings._dllExecuteOneStepEntryPointName
                    + "] : "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

                // fail
                return false;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonRobulabHandle.BindNativeLibrary() : binded ExecuteOneStepDelegate entry point callback via GetProcAddress() syscall ["
                + RobulabSpecificDLLSettings._dllName
                + "]."
            );

            // bind RobulabSpecificDLLSettings._dllGetIC4EntryPointName entry point to delegate callback
            if (
                (
                this.GetIMU
                    = System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<GetIMUDelegate>(
                        NativeDLLInteropServices.GetProcAddress(this.m_handle, RobulabSpecificDLLSettings._dllGetIMUEntryPointName)
                    )
                ) == null
            )
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonRobulabHandle.BindNativeLibrary() : failed to bind GetIMUDelegate to corresponding entry point ["
                    + RobulabSpecificDLLSettings._dllGetIMUEntryPointName
                    + "] : "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

                // fail
                return false;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonRobulabHandle.BindNativeLibrary() : binded GetIMUDelegate entry point callback via GetProcAddress() syscall ["
                + RobulabSpecificDLLSettings._dllName
                + "]."
            );

            // bind RobulabSpecificDLLSettings._dllGetLocalizationEntryPointName entry point to delegate callback
            if (
                (
                this.GetLocalization
                    = System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<GetLocalizationDelegate>(
                        NativeDLLInteropServices.GetProcAddress(this.m_handle, RobulabSpecificDLLSettings._dllGetLocalizationEntryPointName)
                    )
                ) == null
            )
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonRobulabHandle.BindNativeLibrary() : failed to bind GetLocalizationDelegate to corresponding entry point ["
                    + RobulabSpecificDLLSettings._dllGetLocalizationEntryPointName
                    + "] : "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

                // fail
                return false;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonRobulabHandle.BindNativeLibrary() : binded GetLocalizationDelegate entry point callback via GetProcAddress() syscall ["
                + RobulabSpecificDLLSettings._dllName
                + "]."
            );

            // bind RobulabSpecificDLLSettings._dllGetOdometryEntryPointName entry point to delegate callback
            if (
                (
                this.GetOdometry
                    = System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<GetOdometryDelegate>(
                        NativeDLLInteropServices.GetProcAddress(this.m_handle, RobulabSpecificDLLSettings._dllGetOdometryEntryPointName)
                    )
                ) == null
            )
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonRobulabHandle.BindNativeLibrary() : failed to bind GetOdometryDelegate to corresponding entry point ["
                    + RobulabSpecificDLLSettings._dllGetOdometryEntryPointName
                    + "] : "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

                // fail
                return false;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonRobulabHandle.BindNativeLibrary() : binded GetOdometryDelegate entry point callback via GetProcAddress() syscall ["
                + RobulabSpecificDLLSettings._dllName
                + "]."
            );

            // bind RobulabSpecificDLLSettings._dllGetBatteryLevelEntryPointName entry point to delegate callback
            if (
                (
                this.GetBatteryLevel
                    = System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<GetBatteryLevelDelegate>(
                        NativeDLLInteropServices.GetProcAddress(this.m_handle, RobulabSpecificDLLSettings._dllGetBatteryLevelEntryPointName)
                    )
                ) == null
            )
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonRobulabHandle.BindNativeLibrary() : failed to bind GetBatteryLevelDelegate to corresponding entry point ["
                    + RobulabSpecificDLLSettings._dllGetBatteryLevelEntryPointName
                    + "] : "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

                // fail
                return false;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonRobulabHandle.BindNativeLibrary() : binded GetBatteryLevelDelegate entry point callback via GetProcAddress() syscall ["
                + RobulabSpecificDLLSettings._dllName
                + "]."
            );

            // bind RobulabSpecificDLLSettings._dllGetEmergencyStopStateEntryPointName entry point to delegate callback
            if (
                (
                this.GetEmergencyStopState
                    = System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<GetEmergencyStopStateDelegate>(
                        NativeDLLInteropServices.GetProcAddress(this.m_handle, RobulabSpecificDLLSettings._dllGetEmergencyStopStateEntryPointName)
                    )
                ) == null
            )
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonRobulabHandle.BindNativeLibrary() : failed to bind GetEmergencyStopStateDelegate to corresponding entry point ["
                    + RobulabSpecificDLLSettings._dllGetEmergencyStopStateEntryPointName
                    + "] : "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

                // fail
                return false;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonRobulabHandle.BindNativeLibrary() : binded GetEmergencyStopStateDelegate entry point callback via GetProcAddress() syscall ["
                + RobulabSpecificDLLSettings._dllName
                + "]."
            );

            // bind RobulabSpecificDLLSettings._dllResetEmergencyStopStateEntryPointName entry point to delegate callback
            if (
                (
                this.ResetEmergencyStopState
                    = System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<ResetEmergencyStopStateDelegate>(
                        NativeDLLInteropServices.GetProcAddress(this.m_handle, RobulabSpecificDLLSettings._dllResetEmergencyStopStateEntryPointName)
                    )
                ) == null
            )
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonRobulabHandle.BindNativeLibrary() : failed to bind ResetEmergencyStopStateDelegate to corresponding entry point ["
                    + RobulabSpecificDLLSettings._dllResetEmergencyStopStateEntryPointName
                    + "] : "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

                // fail
                return false;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonRobulabHandle.BindNativeLibrary() : binded ResetEmergencyStopStateDelegate entry point callback via GetProcAddress() syscall ["
                + RobulabSpecificDLLSettings._dllName
                + "]."
            );

            // bind RobulabSpecificDLLSettings._dllResetOdometryEntryPointName entry point to delegate callback
            if (
                (
                this.ResetOdometry
                    = System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<ResetOdometryDelegate>(
                        NativeDLLInteropServices.GetProcAddress(this.m_handle, RobulabSpecificDLLSettings._dllResetOdometryEntryPointName)
                    )
                ) == null
            )
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonRobulabHandle.BindNativeLibrary() : failed to bind ResetOdometryDelegate to corresponding entry point ["
                    + RobulabSpecificDLLSettings._dllResetOdometryEntryPointName
                    + "] : "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

                // fail
                return false;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonRobulabHandle.BindNativeLibrary() : binded ResetOdometryDelegate entry point callback via GetProcAddress() syscall ["
                + RobulabSpecificDLLSettings._dllName
                + "]."
            );

            // bind RobulabSpecificDLLSettings._dllResetLocalizationEntryPointName entry point to delegate callback
            if (
                (
                this.ResetLocalization
                    = System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<ResetLocalizationDelegate>(
                        NativeDLLInteropServices.GetProcAddress(this.m_handle, RobulabSpecificDLLSettings._dllResetLocalizationEntryPointName)
                    )
                ) == null
            )
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonRobulabHandle.BindNativeLibrary() : failed to bind ResetLocalizationDelegate to corresponding entry point ["
                    + RobulabSpecificDLLSettings._dllResetLocalizationEntryPointName
                    + "] : "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

                // fail
                return false;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonRobulabHandle.BindNativeLibrary() : binded ResetLocalizationDelegate entry point callback via GetProcAddress() syscall ["
                + RobulabSpecificDLLSettings._dllName
                + "]."
            );

            // bind RobulabSpecificDLLSettings._dllResetMissionEntryPointName entry point to delegate callback
            if (
                (
                this.ResetMission
                    = System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<ResetMissionDelegate>(
                        NativeDLLInteropServices.GetProcAddress(this.m_handle, RobulabSpecificDLLSettings._dllResetMissionEntryPointName)
                    )
                ) == null
            )
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonRobulabHandle.BindNativeLibrary() : failed to bind ResetMissionDelegate to corresponding entry point ["
                    + RobulabSpecificDLLSettings._dllResetMissionEntryPointName
                    + "] : "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

                // fail
                return false;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonRobulabHandle.BindNativeLibrary() : binded ResetMissionDelegate entry point callback via GetProcAddress() syscall ["
                + RobulabSpecificDLLSettings._dllName
                + "]."
            );

            // bind RobulabSpecificDLLSettings._dllSetLinearVelocityTargetEntryPointName entry point to delegate callback
            if (
                (
                this.SetLinearVelocityTarget
                    = System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<SetLinearVelocityTargetDelegate>(
                        NativeDLLInteropServices.GetProcAddress(this.m_handle, RobulabSpecificDLLSettings._dllSetLinearVelocityTargetEntryPointName)
                    )
                ) == null
            )
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonRobulabHandle.BindNativeLibrary() : failed to bind SetLinearVelocityTargetDelegate to corresponding entry point ["
                    + RobulabSpecificDLLSettings._dllSetLinearVelocityTargetEntryPointName
                    + "] : "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

                // fail
                return false;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonRobulabHandle.BindNativeLibrary() : binded SetLinearVelocityTargetDelegate entry point callback via GetProcAddress() syscall ["
                + RobulabSpecificDLLSettings._dllName
                + "]."
            );

            // bind RobulabSpecificDLLSettings._dllSetAngularVelocityTargetEntryPointName entry point to delegate callback
            if (
                (
                this.SetAngularVelocityTarget
                    = System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<SetAngularVelocityTargetDelegate>(
                        NativeDLLInteropServices.GetProcAddress(this.m_handle, RobulabSpecificDLLSettings._dllSetAngularVelocityTargetEntryPointName)
                    )
                ) == null
            )
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonRobulabHandle.BindNativeLibrary() : failed to bind SetAngularVelocityTargetDelegate to corresponding entry point ["
                    + RobulabSpecificDLLSettings._dllSetAngularVelocityTargetEntryPointName
                    + "] : "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

                // fail
                return false;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonRobulabHandle.BindNativeLibrary() : binded SetAngularVelocityTargetDelegate entry point callback via GetProcAddress() syscall ["
                + RobulabSpecificDLLSettings._dllName
                + "]."
            );

            // bind RobulabSpecificDLLSettings._dllStartMissionEntryPointName entry point to delegate callback
            if (
                (
                this.StartMission
                    = System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<StartMissionDelegate>(
                        NativeDLLInteropServices.GetProcAddress(this.m_handle, RobulabSpecificDLLSettings._dllStartMissionEntryPointName)
                    )
                ) == null
            )
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonRobulabHandle.BindNativeLibrary() : failed to bind StartMissionDelegate to corresponding entry point ["
                    + RobulabSpecificDLLSettings._dllStartMissionEntryPointName
                    + "] : "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

                // fail
                return false;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonRobulabHandle.BindNativeLibrary() : binded StartMissionDelegate entry point callback via GetProcAddress() syscall ["
                + RobulabSpecificDLLSettings._dllName
                + "]."
            );

            // bind RobulabSpecificDLLSettings._dllStopMissionEntryPointName entry point to delegate callback
            if (
                (
                this.StopMission
                    = System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<StopMissionDelegate>(
                        NativeDLLInteropServices.GetProcAddress(this.m_handle, RobulabSpecificDLLSettings._dllStopMissionEntryPointName)
                    )
                ) == null
            )
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonRobulabHandle.BindNativeLibrary() : failed to bind StopMissionDelegate to corresponding entry point ["
                    + RobulabSpecificDLLSettings._dllStopMissionEntryPointName
                    + "] : "
                    + System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                );

                // fail
                return false;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonRobulabHandle.BindNativeLibrary() : binded StopMissionDelegate entry point callback via GetProcAddress() syscall ["
                + RobulabSpecificDLLSettings._dllName
                + "]."
            );

            // success 
            return true;

        } /* BindNativeLibrary() */
        
        protected override bool ConstructNativeLibrary()
        {
            
            // log
            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonRobulabHandle.ConstructNativeLibrary() : instante & initialize internal components ["
                + RobulabSpecificDLLSettings._dllName
                + "]."
            );

            // call delegate 
            return (this.Instantiate() && this.Initialize());

        } /* ConstructNativeLibrary() */

        protected override bool DisposeNativeLibrary()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonRobulabHandle.DisposeNativeLibrary() : deleting internal components ["
                + RobulabSpecificDLLSettings._dllName
                + "]."
            );

            // call delegate 
            return this.Delete();

        } /* DisposeNativeLibrary() */

        #endregion

    } /* class ApollonRobulabHandle */
    
} /* namespace Labsim.apollon.backend */