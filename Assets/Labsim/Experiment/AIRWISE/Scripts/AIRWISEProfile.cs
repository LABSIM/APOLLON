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

using System.Linq;
using UnityEngine.UIElements;

// avoid namespace pollution
namespace Labsim.experiment.AIRWISE
{

    public class AIRWISEProfile 
        : apollon.experiment.ApollonAbstractExperimentFiniteStateMachine< AIRWISEProfile >
    {

        // Ctor
        public AIRWISEProfile()
            : base()
        {
            // default profile
            this.m_profileID = apollon.experiment.ApollonExperimentManager.ProfileIDType.AIRWISE;
        }

        // properties
        public AIRWISESettings CurrentSettings { get; private set; } = null;
        public AIRWISEResults CurrentResults { get; set; } = null;

        #region abstract implementation

        // infos
        protected override System.String getCurrentStatusInfo()
        {

            return (
                "[" + apollon.ApollonEngine.GetEnumDescription(this.ID) + "]" 
                + "\n" 
                + this.CurrentSettings.Trial.pattern_type
                + "|"
                + apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.Trial.control_type)
                + "|"
                + apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.Trial.scene_type)
            );

        } /* getCurrentStatusInfo() */

        protected override System.String getCurrentCounterStatusInfo()
        {

            return (
                (this.CurrentSettings.Trial.bIsActive) 
                ? ( 
                    (
                        (UXF.Session.instance.blocks.Count > 1) 
                            ? (UXF.Session.instance.CurrentBlock.number + "/" + UXF.Session.instance.blocks.Count + " | ")
                            : ""
                    )
                )
                : ""
            );

        } /* getCurrentCounterStatusInfo() */

        public System.String CurrentInstruction { get; set; } = "";
        protected override System.String getCurrentInstructionStatusInfo()
        {

            // simply
            return this.CurrentInstruction;

        } /* getCurrentInstructionStatusInfo() */

        public override void OnUpdate(object sender, apollon.ApollonEngine.EngineEventArgs arg)
        {

            // base call
            base.OnUpdate(sender, arg);

        } /* onUpdate() */

        public async override void OnExperimentSessionBegin(object sender, apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEProfile.onExperimentSessionBegin() : begin"
            );

            // configure VarjoXR plugin
            Varjo.XR.VarjoEyeTracking.SetGazeOutputFrequency(Varjo.XR.VarjoEyeTracking.GazeOutputFrequency.Frequency200Hz);
            Varjo.XR.VarjoEyeTracking.SetGazeOutputFilterType(Varjo.XR.VarjoEyeTracking.GazeOutputFilterType.Standard);
            Varjo.XR.VarjoHeadsetIPD.SetInterPupillaryDistanceParameters(Varjo.XR.VarjoHeadsetIPD.IPDAdjustmentMode.Automatic);

            // force recalibration
            Varjo.XR.VarjoEyeTracking.RequestGazeCalibration(
                Varjo.XR.VarjoEyeTracking.GazeCalibrationMode.Fast,
                Varjo.XR.VarjoEyeTracking.HeadsetAlignmentGuidanceMode.AutoContinueOnAcceptableHeadsetPosition
            );

            // wait for eye tracking system init
            do
            {
                await apollon.ApollonHighResolutionTime.DoSleep(500.0f);
            } 
            while(Varjo.XR.VarjoEyeTracking.IsGazeCalibrating() && !Varjo.XR.VarjoEyeTracking.IsGazeCalibrated());

            // extract calibration results
            var calibration_results = Varjo.XR.VarjoEyeTracking.GetGazeCalibrationQuality();
            string left_result, right_result;

            switch(calibration_results.left)
            {
                case Varjo.XR.VarjoEyeTracking.GazeEyeCalibrationQuality.Low:
                {
                    left_result
                        = "Low";
                    break;
                }
                case Varjo.XR.VarjoEyeTracking.GazeEyeCalibrationQuality.Medium:
                {
                    left_result
                        = "Medium";
                    break;
                }
                case Varjo.XR.VarjoEyeTracking.GazeEyeCalibrationQuality.High:
                {
                    left_result
                        = "High";
                    break;
                }
                default:
                case Varjo.XR.VarjoEyeTracking.GazeEyeCalibrationQuality.Invalid:
                {
                    left_result
                        = "Invalid";
                    break;
                }
            } /* switch() */

            switch(calibration_results.right)
            {
                case Varjo.XR.VarjoEyeTracking.GazeEyeCalibrationQuality.Low:
                {
                    right_result
                        = "Low";
                    break;
                }
                case Varjo.XR.VarjoEyeTracking.GazeEyeCalibrationQuality.Medium:
                {
                    right_result
                        = "Medium";
                    break;
                }
                case Varjo.XR.VarjoEyeTracking.GazeEyeCalibrationQuality.High:
                {
                    right_result
                        = "High";
                    break;
                }
                default:
                case Varjo.XR.VarjoEyeTracking.GazeEyeCalibrationQuality.Invalid:
                {
                    right_result
                        = "Invalid";
                    break;
                }
            } /* switch() */
            
            apollon.experiment.ApollonExperimentManager.Instance.Session.participantDetails["gaze_calibration_left_eye_quality"] 
                = left_result;
            apollon.experiment.ApollonExperimentManager.Instance.Session.participantDetails["gaze_calibration_right_eye_quality"] 
                = right_result;
            apollon.experiment.ApollonExperimentManager.Instance.Session.participantDetails["inter_pupillary_distance"] 
                = Varjo.XR.VarjoHeadsetIPD.GetDistance().ToString();

            // activate all motion system command/sensor
            apollon.gameplay.ApollonGameplayManager.Instance.setActive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemCommand
            );
            apollon.gameplay.ApollonGameplayManager.Instance.setActive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemSensor
            );
            apollon.gameplay.ApollonGameplayManager.Instance.setActive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.VirtualMotionSystemCommand
            );      


            // log 
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEProfile.onExperimentSessionBegin() : sync wait 5000.0ms for motion system init"
            );

            // wait for motion system init
            await apollon.ApollonHighResolutionTime.DoSleep(5000.0f);
            
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEProfile.onExperimentSessionBegin() : will async fade in 1000.0ms"
            );

            // fade in
            this.DoFadeIn(1000.0f);

            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEProfile.onExperimentSessionBegin() : async raise reset to initial position in 5000.0ms"
            );

            // Raise reset motion event
            apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                apollon.gameplay.device.command.ApollonMotionSystemCommandBridge
            >(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemCommand
            ).ConcreteDispatcher.RaiseReset(5000.0f);     

            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEProfile.onExperimentSessionBegin() : switch scene setup"
            );

            // deactivate default DB & activate room setup
            var we_behaviour
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.element.ApollonStaticElementBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.StaticElement
                ).ConcreteBehaviour;
            we_behaviour.References["DBTag_Default"].SetActive(false);
            we_behaviour.References["DBTag_Room"].SetActive(true);
            we_behaviour.References["DBTag_ExoFrontend"].SetActive(true);

            // log 
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEProfile.onExperimentSessionBegin() : sync wait 5000.0ms init pause"
            );

            // wait for motion system init
            await apollon.ApollonHighResolutionTime.DoSleep(5000.0f);

            // base call
            base.OnExperimentSessionBegin(sender, arg);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEProfile.onExperimentSessionBegin() : end"
            );

        } /* onExperimentSessionBegin() */

        public override async void OnExperimentSessionEnd(object sender, apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEProfile.onExperimentSessionEnd() : begin"
            );

            // base call
            base.OnExperimentSessionEnd(sender, arg);

            // deactivate all motion system command/sensor
            apollon.gameplay.ApollonGameplayManager.Instance.setInactive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemSensor
            );
            apollon.gameplay.ApollonGameplayManager.Instance.setInactive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemCommand
            );
            apollon.gameplay.ApollonGameplayManager.Instance.setInactive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.VirtualMotionSystemCommand
            );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEProfile.onExperimentSessionEnd() : end"
            );


        } /* onExperimentSessionEnd() */

        public override async void OnExperimentTrialBegin(object sender, apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEProfile.onExperimentTrialBegin() : begin"
            );

            // // send event to motion system backend
            // (
            //     backend.ApollonBackendManager.Instance.GetValidHandle(
            //         backend.ApollonBackendManager.HandleIDType.ApollonMotionSystemPS6TM550Handle
            //     ) as backend.handle.ApollonMotionSystemPS6TM550Handle
            // ).BeginTrial();
        
            // local
            // int currentIdx = apollon.experiment.ApollonExperimentManager.Instance.Session.currentTrialNum - 1;

            // // activate the active seat entity
            // apollon.gameplay.ApollonGameplayManager.Instance.setActive(apollon.gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity);

            // // inactivate all visual cues through LINQ request
            // var we_behaviour
            //     = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
            //         apollon.gameplay.element.ApollonStaticElementBridge
            //     >(
            //         apollon.gameplay.ApollonGameplayManager.GameplayIDType.StaticElement
            //     ).ConcreteBehaviour;
            // foreach (var vc_ref in we_behaviour.References.Where(kvp => kvp.Key.Contains("VCTag_")).Select(kvp => kvp.Value))
            // {
            //     vc_ref.SetActive(false);
            // }

            // // current scenario
            // switch (arg.Trial.settings.GetString("scenario_name"))
            // {

            //     case "visual-only":
            //     {
            //         we_behaviour.References["VCTag_Fan"].SetActive(true);
            //         this.CurrentSettings.scenario_type = Settings.ScenarioIDType.VisualOnly;
            //         // transit to corresponding entity state
            //         (
            //             apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
            //                 apollon.gameplay.entity.ApollonActiveSeatEntityBridge
            //             >(
            //                 apollon.gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity
            //             )
            //         ).ConcreteDispatcher.RaiseVisualOnly();
            //         break;
            //     }
            //     case "vestibular-only":
            //     {
            //         we_behaviour.References["VCTag_Spot"].SetActive(true);
            //         this.CurrentSettings.scenario_type = Settings.ScenarioIDType.VestibularOnly;
            //         // transit to corresponding entity state
            //         (
            //             apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
            //                 apollon.gameplay.entity.ApollonActiveSeatEntityBridge
            //             >(
            //                 apollon.gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity
            //             )
            //         ).ConcreteDispatcher.RaiseVestibularOnly();
            //         break;
            //     }
            //     case "visuo-vestibular":
            //     {
            //         we_behaviour.References["VCTag_Fan"].SetActive(true);
            //         this.CurrentSettings.scenario_type = Settings.ScenarioIDType.VisuoVestibular;
            //         // transit to corresponding entity state
            //         (
            //             apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
            //                 apollon.gameplay.entity.ApollonActiveSeatEntityBridge
            //             >(
            //                 apollon.gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity
            //             )
            //         ).ConcreteDispatcher.RaiseVisuoVestibular();
            //         break;
            //     }
            //     default:
            //     {
            //         this.CurrentSettings.scenario_type = Settings.ScenarioIDType.Undefined;
            //         break;
            //     }

            // } /* switch() */

            // // import current trial settings & log on completion
            // if(this.CurrentSettings.ImportUXFSettings(arg.Trial.settings))
            // {
            //     this.CurrentSettings.LogUXFSettings();
            // }
           
            // // activate world element & contriol system
            // apollon.gameplay.ApollonGameplayManager.Instance.setActive(apollon.gameplay.ApollonGameplayManager.GameplayIDType.StaticElement);
            // apollon.gameplay.ApollonGameplayManager.Instance.setActive(apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionV4Control);

            // // base call
            // base.OnExperimentTrialBegin(sender, arg);

            // // fade out
            // await this.DoFadeOut(this._trial_fade_out_duration, false);

            // // initialize to position on first trial - wait 5s
            // if(apollon.experiment.ApollonExperimentManager.Instance.Session.FirstTrial == apollon.experiment.ApollonExperimentManager.Instance.Trial)
            // {
            //     await apollon.ApollonHighResolutionTime.DoSleep(5000.0f);
            // }

            // // log
            // UnityEngine.Debug.Log(
            //     "<color=Blue>Info: </color> AIRWISEProfile.onExperimentTrialBegin() : trial protocol will start"
            // );

            // // build protocol
            // await this.DoRunProtocol(
            //     async () => {
            //         await this.DoWhileLoop(
            //             () => { 
            //                 return !(this.CurrentResults.phase_A_results.step_is_valid);
            //             },
            //             async () => { await this.SetState( new AgencyAndThresholdPerceptionV4Phase0(this) ); },
            //             async () => { await this.SetState( new AgencyAndThresholdPerceptionV4PhaseA(this) ); }
            //         );
            //     },
            //     async () => { await this.SetState( new AgencyAndThresholdPerceptionV4PhaseB(this) ); },
            //     async () => { await this.SetState( new AgencyAndThresholdPerceptionV4PhaseC(this) ); },
            //     async () => { await this.SetState( new AgencyAndThresholdPerceptionV4PhaseD(this) ); },
            //     async () => { await this.SetState( new AgencyAndThresholdPerceptionV4PhaseE(this) ); },
            //     async () => { await this.SetState( new AgencyAndThresholdPerceptionV4PhaseF(this) ); },
            //     async () => { await this.SetState( null ); }
            // );
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEProfile.onExperimentTrialBegin() : end"
            );

        } /* onExperimentTrialBegin() */

        public override async void OnExperimentTrialEnd(object sender, apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEProfile.onExperimentTrialEnd() : begin"
            );

            // // reset internal
            // this.CurrentResults.phase_A_results.step_is_valid = false;
            
            // // write the randomized scenario/pattern/conditions(s) as result for convenience
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["scenario"] = apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.scenario_type);
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["pattern"] = this.CurrentSettings.pattern_type;
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["active_condition"] = this.CurrentSettings.bIsActive.ToString();
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["catch_try_condition"] = this.CurrentSettings.bIsTryCatch.ToString();
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["current_latency_bucket"] = "[" + System.String.Join(";",this.CurrentLatencyBucket) + "]";

            // // phase 0 - RAZ input to neutral position
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["0_timing_on_entry_unity_timestamp"]
            //     = this.CurrentResults.phase_0_results.timing_on_entry_unity_timestamp.ToString();
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["0_timing_on_exit_unity_timestamp"]
            //     = this.CurrentResults.phase_0_results.timing_on_exit_unity_timestamp.ToString();
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["0_timing_on_entry_host_timestamp"]
            //     = this.CurrentResults.phase_0_results.timing_on_entry_host_timestamp;
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["0_timing_on_exit_host_timestamp"]
            //     = this.CurrentResults.phase_0_results.timing_on_exit_host_timestamp;

            // // phase A - user input selection + UI notification / validation
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["A_timing_on_entry_unity_timestamp"]
            //     = this.CurrentResults.phase_A_results.timing_on_entry_unity_timestamp.ToString();
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["A_timing_on_exit_unity_timestamp"]
            //     = this.CurrentResults.phase_A_results.timing_on_exit_unity_timestamp.ToString();
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["A_timing_on_entry_host_timestamp"]
            //     = this.CurrentResults.phase_A_results.timing_on_entry_host_timestamp;
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["A_timing_on_exit_host_timestamp"]
            //     = this.CurrentResults.phase_A_results.timing_on_exit_host_timestamp;
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_command"] 
            //     = this.CurrentResults.phase_A_results.user_command;
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_latency_unity_timestamp"] 
            //     = this.CurrentResults.phase_A_results.user_latency_unity_timestamp.ToString();
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_latency_host_timestamp"] 
            //     = this.CurrentResults.phase_A_results.user_latency_host_timestamp.ToString();
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_measured_latency"] 
            //     = this.CurrentResults.phase_A_results.user_measured_latency.ToString();
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_randomized_stim_latency"] 
            //     = this.CurrentResults.phase_A_results.user_randomized_stim_latency.ToString();
            
            // // phase B - primary stim -> SOA -> secondary stim
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["B_timing_on_entry_unity_timestamp"]
            //     = this.CurrentResults.phase_B_results.timing_on_entry_unity_timestamp.ToString();
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["B_timing_on_exit_unity_timestamp"]
            //     = this.CurrentResults.phase_B_results.timing_on_exit_unity_timestamp.ToString();
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["B_timing_on_entry_host_timestamp"]
            //     = this.CurrentResults.phase_B_results.timing_on_entry_host_timestamp;
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["B_timing_on_exit_host_timestamp"]
            //     = this.CurrentResults.phase_B_results.timing_on_exit_host_timestamp;
            
            // // phase C - UI end notification
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["C_timing_on_entry_unity_timestamp"]
            //     = this.CurrentResults.phase_C_results.timing_on_entry_unity_timestamp.ToString();
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["C_timing_on_exit_unity_timestamp"]
            //     = this.CurrentResults.phase_C_results.timing_on_exit_unity_timestamp.ToString();
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["C_timing_on_entry_host_timestamp"]
            //     = this.CurrentResults.phase_C_results.timing_on_entry_host_timestamp;
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["C_timing_on_exit_host_timestamp"]
            //     = this.CurrentResults.phase_C_results.timing_on_exit_host_timestamp;
                
            // // phase D - user response
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["D_timing_on_entry_unity_timestamp"]
            //     = this.CurrentResults.phase_D_results.timing_on_entry_unity_timestamp.ToString();
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["D_timing_on_exit_unity_timestamp"]
            //     = this.CurrentResults.phase_D_results.timing_on_exit_unity_timestamp.ToString();
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["D_timing_on_entry_host_timestamp"]
            //     = this.CurrentResults.phase_D_results.timing_on_entry_host_timestamp;
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["D_timing_on_exit_host_timestamp"]
            //     = this.CurrentResults.phase_D_results.timing_on_exit_host_timestamp;
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_response"]
            //     = this.CurrentResults.phase_D_results.user_response;
                
            // // phase E - user confidence in response
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["E_timing_on_entry_unity_timestamp"]
            //     = this.CurrentResults.phase_E_results.timing_on_entry_unity_timestamp.ToString();
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["E_timing_on_exit_unity_timestamp"]
            //     = this.CurrentResults.phase_E_results.timing_on_exit_unity_timestamp.ToString();
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["E_timing_on_entry_host_timestamp"]
            //     = this.CurrentResults.phase_E_results.timing_on_entry_host_timestamp;
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["E_timing_on_exit_host_timestamp"]
            //     = this.CurrentResults.phase_E_results.timing_on_exit_host_timestamp;
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_confidence"]
            //     = this.CurrentResults.phase_E_results.user_confidence;

            // // phase F - back to origin
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["F_timing_on_entry_unity_timestamp"]
            //     = this.CurrentResults.phase_F_results.timing_on_entry_unity_timestamp.ToString();
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["F_timing_on_exit_unity_timestamp"]
            //     = this.CurrentResults.phase_F_results.timing_on_exit_unity_timestamp.ToString();
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["F_timing_on_entry_host_timestamp"]
            //     = this.CurrentResults.phase_F_results.timing_on_entry_host_timestamp;
            // apollon.experiment.ApollonExperimentManager.Instance.Trial.result["F_timing_on_exit_host_timestamp"]
            //     = this.CurrentResults.phase_F_results.timing_on_exit_host_timestamp;

            // base call
            base.OnExperimentTrialEnd(sender, arg);
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEProfile.onExperimentTrialEnd() : end"
            );

        } /* onExperimentTrialEnd() */

        #endregion

    } /* class AIRWISEProfile */
    
} /* } Labsim.experiment.AIRWISE */