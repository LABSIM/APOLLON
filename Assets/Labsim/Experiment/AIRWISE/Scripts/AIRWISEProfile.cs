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
using System.Windows.Forms;
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
        public AIRWISEResults CurrentResults { get; private set; } = null;

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

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEProfile.onExperimentSessionBegin() : will async blank fade in 2500.0ms"
            );

            // async fade in
            this.DoBlankFadeIn(2500.0f);
            this.DoLightFadeIn(2500.0f);

            // preconfigure root path
            foreach (var dh in apollon.experiment.ApollonExperimentManager.Instance.Session.ActiveDataHandlers)
            {
                if (dh is UXF.FileSaver)
                {

                    // assign session path
                    Logger.m_rootPath = (dh as UXF.FileSaver).GetSessionPath(apollon.experiment.ApollonExperimentManager.Instance.Session);
                    
                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> AIRWISEProfile.onExperimentSessionBegin() : configuring Yale's Logging system [Logger.m_rootPath="
                        + Logger.m_rootPath
                        + "]"
                    );

                    break;

                } /* if() */

            } /* foreach() */

            // configure VarjoXR plugin
            Varjo.XR.VarjoEyeTracking.SetGazeOutputFrequency(Varjo.XR.VarjoEyeTracking.GazeOutputFrequency.Frequency200Hz);
            Varjo.XR.VarjoEyeTracking.SetGazeOutputFilterType(Varjo.XR.VarjoEyeTracking.GazeOutputFilterType.Standard);
            Varjo.XR.VarjoHeadsetIPD.SetInterPupillaryDistanceParameters(Varjo.XR.VarjoHeadsetIPD.IPDAdjustmentMode.Automatic);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEProfile.onExperimentSessionBegin() : configured Varjo parameters to [200Hz, Standard filter, automatic IPD], requesting calibration"
            );

            // force recalibration
            Varjo.XR.VarjoEyeTracking.RequestGazeCalibration(
                Varjo.XR.VarjoEyeTracking.GazeCalibrationMode.Fast,
                Varjo.XR.VarjoEyeTracking.HeadsetAlignmentGuidanceMode.AutoContinueOnAcceptableHeadsetPosition
            );
 
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEProfile.onExperimentSessionBegin() : calibration requested"
            );

            // wait for eye tracking system init
            do
            {
                await apollon.ApollonHighResolutionTime.DoSleep(500.0f);
            } 
            while(Varjo.XR.VarjoEyeTracking.IsGazeCalibrating() && !Varjo.XR.VarjoEyeTracking.IsGazeCalibrated());

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEProfile.onExperimentSessionBegin() : calibration done !"
            );

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

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEProfile.onExperimentSessionBegin() : calibration results logged in participant details ["
                + "gaze_calibration_left_eye_quality:"
                    + apollon.experiment.ApollonExperimentManager.Instance.Session.participantDetails["gaze_calibration_left_eye_quality"]
                + "/gaze_calibration_right_eye_quality:"
                    + apollon.experiment.ApollonExperimentManager.Instance.Session.participantDetails["gaze_calibration_right_eye_quality"] 
                + "/inter_pupillary_distance:"
                    + apollon.experiment.ApollonExperimentManager.Instance.Session.participantDetails["inter_pupillary_distance"]
                +"]"
            );

            // activate all motion system command/sensor/impedence
            apollon.gameplay.ApollonGameplayManager.Instance.setActive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.AIRWISEEntity
            );

            // log 
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEProfile.onExperimentSessionBegin() : sync wait 2500.0ms for motion system init"
            );

            // wait for motion system init
            await apollon.ApollonHighResolutionTime.DoSleep(2500.0f);

            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEProfile.onExperimentSessionBegin() : async raise reset to initial position in 2500.0ms"
            );

            // Raise reset motion event
            apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                AIRWISEEntityBridge
            >(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.AIRWISEEntity
            ).ConcreteDispatcher.RaiseReset(2500.0f);     

            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEProfile.onExperimentSessionBegin() : switch scene setup"
            );

            // deactivate default DB & Light
            var static_element
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.element.ApollonStaticElementBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.StaticElement
                ).ConcreteBehaviour;
            static_element.References["DBTag_BaseSetup"].SetActive(false);

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

            // deactivate all entity
            apollon.gameplay.ApollonGameplayManager.Instance.setInactive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.All
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

            // instantiate Settings & Results classes 
            this.CurrentSettings = new(this);
            this.CurrentResults  = new(this);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEProfile.onExperimentTrialBegin() : begin"
            );
            
            // import current trial settings & log on completion
            if(this.CurrentSettings.ImportUXFSettings(arg.Trial.settings))
            {
                this.CurrentSettings.LogUXFSettings();
            }

            // get refs 
            var static_element
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.element.ApollonStaticElementBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.StaticElement
                ).ConcreteBehaviour;
            var dynamic_entity
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.entity.ApollonDynamicEntityBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.DynamicEntity
                ).ConcreteBehaviour;

            // current visual
            if(this.CurrentSettings.Trial.visual_type == AIRWISESettings.VisualIDType.Undefined)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> AIRWISEProfile.onExperimentTrialBegin() : UNDEFINED visual for pattern["
                    + this.CurrentSettings.Trial.pattern_type
                    + "]... check configuration files !"
                );

            } 
            else if(this.CurrentSettings.Trial.visual_type == AIRWISESettings.VisualIDType.None)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEProfile.onExperimentTrialBegin() : No visual, skipping loading mechanism"
                );

            }
            else
            {

                // finally load the required visual
                static_element
                    .References[
                        "DBTag_Visual" 
                        + apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.Trial.visual_type)
                    ]
                    .SetActive(true);

            } /* if() */

            // current scene
            if(this.CurrentSettings.Trial.scene_type == AIRWISESettings.SceneIDType.Undefined)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> AIRWISEProfile.onExperimentTrialBegin() : UNDEFINED scene for pattern["
                    + this.CurrentSettings.Trial.pattern_type
                    + "]... check configuration files !"
                );

            }
            else
            {

                // finally load the required scene
                static_element
                    .References[
                        "DBTag_Scene" 
                        + apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.Trial.scene_type)
                    ]
                    .SetActive(true);

            } /* if() */

            // base call
            base.OnExperimentTrialBegin(sender, arg);

            // async fade out
            this.DoBlankFadeOut(250.0f);
            this.DoLightFadeOut(250.0f);

            // activate subject control
            apollon.gameplay.ApollonGameplayManager.Instance.setActive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.AIRWISEControl
            );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AIRWISEProfile.onExperimentTrialBegin() : trial protocol will start"
            );

            // build protocol
            await this.DoRunProtocol(
                async () => {
                    await this.DoWhileLoop(
                        () => { 
                            return !(this.CurrentResults.Trial.user_performance_value < this.CurrentSettings.Trial.performance_criteria);
                        },
                        async () => { await this.SetState( new AIRWISEPhaseA(this) ); },
                        async () => { await this.SetState( new AIRWISEPhaseB(this) ); },
                        async () => { await this.SetState( new AIRWISEPhaseC(this) ); },
                        async () => { await this.SetState( new AIRWISEPhaseD(this) ); }
                    );
                },
                async () => { await this.SetState( new AIRWISEPhaseE(this) ); },
                async () => { await this.SetState( new AIRWISEPhaseF(this) ); },
                async () => { await this.SetState( new AIRWISEPhaseG(this) ); },
                async () => { await this.SetState( new AIRWISEPhaseH(this) ); },
                async () => { await this.SetState( new AIRWISEPhaseI(this) ); },
                async () => { await this.SetState( new AIRWISEPhaseJ(this) ); },
                async () => { await this.SetState( null ); }
            );
            
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

            // get a ref
            var static_element
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.element.ApollonStaticElementBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.StaticElement
                ).ConcreteBehaviour;

            // inactivate all subject control
            apollon.gameplay.ApollonGameplayManager.Instance.setInactive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.AIRWISEControl
            );

            // async fade in
            this.DoBlankFadeIn(250.0f);
            this.DoLightFadeIn(250.0f);

            // current visual
            if(this.CurrentSettings.Trial.visual_type == AIRWISESettings.VisualIDType.Undefined)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> AIRWISEProfile.OnExperimentTrialEnd() : UNDEFINED visual for pattern["
                    + this.CurrentSettings.Trial.pattern_type
                    + "]... check configuration files !"
                );

            } 
            else if(this.CurrentSettings.Trial.visual_type == AIRWISESettings.VisualIDType.None)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AIRWISEProfile.OnExperimentTrialEnd() : No visual, skipping unloading mechanism"
                );

            }
            else
            {

                // finally unload the required visual
                static_element
                    .References[
                        "DBTag_Visual" 
                        + apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.Trial.visual_type)
                    ]
                    .SetActive(false);

            } /* if() */

            // current scene
            if(this.CurrentSettings.Trial.scene_type == AIRWISESettings.SceneIDType.Undefined)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> AIRWISEProfile.OnExperimentTrialEnd() : UNDEFINED scene for pattern["
                    + this.CurrentSettings.Trial.pattern_type
                    + "]... check configuration files !"
                );

            }
            else
            {

                // finally load the required scene
                static_element
                    .References[
                        "DBTag_Scene" 
                        + apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.Trial.scene_type)
                    ]
                    .SetActive(false);

            } /* if() */

            // switch on model control config & dump current trial configs files
            switch(this.CurrentSettings.Trial.control_type)
            {

                case AIRWISESettings.ControlIDType.Familiarisation:
                {

                    foreach(
                        var configFilename in new string[]{
                            Constants.ConfigFile,
                            Constants.ForcingFunctionConfigFile,
                            Constants.MappingConfigFile,
                            Constants.ControlConfigFile,
                            Constants.ActuationConfigFile,
                            Constants.HapticConfigFile,
                            Constants.ErrorDisplayConfigFile
                        } 
                    )
                    { 

                        // build new paths
                        string
                            input       = System.IO.Path.Combine(Constants.streamingAssetsPath, configFilename),
                            filename    = System.IO.Path.GetFileNameWithoutExtension(input),
                            fileext     = System.IO.Path.GetExtension(input),
                            output      = System.IO.Path.Combine(
                                            Logger.m_rootPath, 
                                            "configs", 
                                            string.Concat(
                                                filename,
                                                string.Format("_T{1:000}", arg.Trial.number),
                                                fileext
                                            )
                                        );

                        // log
                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> AIRWISEProfile.onExperimentTrialBegin() : PositionControl mode, config:" + configFilename + " found["
                            + "input:"      + input
                            + "/filemame: " + filename
                            + "/fileext: "  + fileext
                            + "/output: "   + output
                            + "]"
                        );
                        
                        // // copy config files to correct location
                        // System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(output));
                        // System.IO.File.Copy(input, output); 

                    } /* foreach() */
                   
                    break;

                } /* case Familiarisation */

                case AIRWISESettings.ControlIDType.PositionControl:
                {

                    foreach(
                        var configFilename in new string[]{
                            Constants.ConfigFile,
                            Constants.ForcingFunctionConfigFile,
                            Constants.MappingConfigFile,
                            Constants.ControlConfigFile,
                            Constants.ActuationConfigFile,
                            Constants.HapticConfigFile,
                            Constants.ErrorDisplayConfigFile
                        } 
                    )
                    { 

                        // build new paths
                        string
                            input       = System.IO.Path.Combine(Constants.streamingAssetsPath, configFilename),
                            filename    = System.IO.Path.GetFileNameWithoutExtension(input),
                            fileext     = System.IO.Path.GetExtension(input),
                            output      = System.IO.Path.Combine(
                                            Logger.m_rootPath, 
                                            "configs", 
                                            string.Concat(
                                                filename,
                                                string.Format("_T{1:000}", arg.Trial.number),
                                                fileext
                                            )
                                        );

                        // log
                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> AIRWISEProfile.onExperimentTrialBegin() : PositionControl mode, config:" + configFilename + " found["
                            + "input:"      + input
                            + "/filemame: " + filename
                            + "/fileext: "  + fileext
                            + "/output: "   + output
                            + "]"
                        );
                        
                        // // copy config files to correct location
                        // System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(output));
                        // System.IO.File.Copy(input, output); 

                    } /* foreach() */
                   
                    break;

                } /* case PositionControl */

                case AIRWISESettings.ControlIDType.SpeedControl:
                {

                    // TODO config file ? see Yale
                    break;

                }

                case AIRWISESettings.ControlIDType.AccelerationControl:
                {

                    // TODO config file ? see Yale
                    break;
                    
                }

                case AIRWISESettings.ControlIDType.Undefined:
                default:
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> AIRWISEProfile.onExperimentTrialBegin() : UNDEFINED control for pattern["
                        + this.CurrentSettings.Trial.pattern_type
                        + "]... check configuration files !"
                    );

                    break;

                }

            } /* switch() */

            // export current trial results & log on completion
            if(this.CurrentResults.ExportUXFResults(arg.Trial.result))
            {
                this.CurrentResults.LogUXFResults();
            }
            
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