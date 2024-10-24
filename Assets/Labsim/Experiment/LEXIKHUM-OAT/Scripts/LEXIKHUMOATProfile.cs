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

// avoid namespace pollution
namespace Labsim.experiment.LEXIKHUM_OAT
{

    public class LEXIKHUMOATProfile 
        : apollon.experiment.ApollonAbstractExperimentFiniteStateMachine<LEXIKHUMOATProfile>
    {
        
        // Ctor
        public LEXIKHUMOATProfile()
            : base()
        {
            // default profile
            this.m_profileID = apollon.experiment.ApollonExperimentManager.ProfileIDType.LEXIKHUM_OAT;
        }

        // properties
        public LEXIKHUMOATSettings CurrentSettings { get; private set; } = null;
        public LEXIKHUMOATResults CurrentResults { get; private set; } = null;

        #region Abstract implementation

        // infos
        protected override System.String getCurrentStatusInfo()
        {

            return (
                "[" + apollon.ApollonEngine.GetEnumDescription(this.ID) + "]" 
                + "\n" 
                + this.CurrentSettings.Trial.pattern_type
                + "|"
                + apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.Trial.visual_type)
                + "|"
                + apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.Trial.scene_type)
            );

        } /* getCurrentStatusInfo() */

        protected override System.String getCurrentCounterStatusInfo()
        {

            return (
                apollon.experiment.ApollonExperimentManager.Instance.Session.CurrentBlock.number 
                + "/" 
                + apollon.experiment.ApollonExperimentManager.Instance.Session.blocks.Count 
                + " | "
                + (
                    (float)
                    (
                        apollon.experiment.ApollonExperimentManager.Instance.Session.CurrentTrial.number - 1
                    ) 
                    * 100.0f 
                    / apollon.experiment.ApollonExperimentManager.Instance.Session.Trials.Count()
                ).ToString("N1") 
                + " %"
            );

        } /* getCurrentCounterStatusInfo() */

        public System.String CurrentInstruction { get; set; } = "";
        protected override System.String getCurrentInstructionStatusInfo()
        {

            // simply
            return this.CurrentInstruction;

        } /* getCurrentInstructionStatusInfo() */


        public System.String CurrentQuestion { get; set; } = "";
        protected override System.String getCurrentQuestionStatusInfo()
        {

            // simply
            return this.CurrentQuestion;

        } /* getCurrentQuestionStatusInfo() */


        public System.String CurrentQuestionDetail { get; set; } = "";
        protected override System.String getCurrentQuestionDetailStatusInfo()
        {

            // simply
            return this.CurrentQuestionDetail;

        } /* getCurrentQuestionDetailStatusInfo() */


        public System.String CurrentQuestionTickLowerBound { get; set; } = "";
        protected override System.String getCurrentQuestionTickLowerBoundStatusInfo()
        {

            // simply
            return this.CurrentQuestionTickLowerBound;

        } /* getCurrentQuestionTickLowerBoundStatusInfo() */


        public System.String CurrentQuestionTickUpperBound { get; set; } = "";
        protected override System.String getCurrentQuestionTickUpperBoundStatusInfo()
        {

            // simply
            return this.CurrentQuestionTickUpperBound;

        } /* getCurrentQuestionTickUpperBoundStatusInfo() */


        public bool CurrentQuestionHasTick { get; set; } = true;
        protected override bool getCurrentQuestionHasTickStatusInfo()
        {

            // simply
            return this.CurrentQuestionHasTick;

        } /* getCurrentQuestionHasTickStatusInfo() */


        public bool CurrentQuestionHasTickText { get; set; } = true;
        protected override bool getCurrentQuestionHasTickTextStatusInfo()
        {

            // simply
            return this.CurrentQuestionHasTickText;

        } /* getCurrentQuestionHasTickStatusInfo() */

        #endregion

        #region Entry points callback override

        public override void OnUpdate(object sender, apollon.ApollonEngine.EngineEventArgs arg)
        {

            // base call
            base.OnUpdate(sender, arg);

        } /* onUpdate() */

        public async override void OnExperimentSessionBegin(object sender, apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATProfile.onExperimentSessionBegin() : begin"
            );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATProfile.onExperimentSessionBegin() : will async blank fade in 2500.0ms"
            );

            // async fade in
            this.DoBlankFadeIn(250.0f);
            this.DoLightFadeIn(250.0f);

            // synchronous wait loading scene
            await apollon.ApollonHighResolutionTime.DoSleep(250.0f);

            // deactivate default DB & Light
            var static_element
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.element.ApollonStaticElementBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.StaticElement
                ).ConcreteBehaviour;
            static_element.References["DBTag_BaseSetup"].SetActive(false);

            // activate gameplay element & backend
            apollon.gameplay.ApollonGameplayManager.Instance.setActive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.FogElement
            );
            apollon.gameplay.ApollonGameplayManager.Instance.setActive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.Generic3DoFHapticArm
            );
            apollon.backend.ApollonBackendManager.Instance.RaiseHandleActivationRequestedEvent(
                apollon.backend.ApollonBackendManager.HandleIDType.ApollonISIRForceDimensionOmega3Handle
            );

            // get our ISIR backend
            var backend 
                = apollon.backend.ApollonBackendManager.Instance.GetValidHandle(
                    apollon.backend.ApollonBackendManager.HandleIDType.ApollonISIRForceDimensionOmega3Handle
                ) as apollon.backend.handle.ApollonISIRForceDimensionOmega3Handle;

            // init session
            backend.NextGateKind          = "Initialize";
            backend.NextGateSide          = "Session";
            backend.SharedIntentionMode   = "";
            backend.NextGateWorldPosition = new(0.0f, 0.0f, 0.0f);
            backend.NextGateWidth         = 0.0f;

            // base call
            base.OnExperimentSessionBegin(sender, arg);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATProfile.onExperimentSessionBegin() : end"
            );
 
        } /* onExperimentSessionBegin() */

        public override async void OnExperimentSessionEnd(object sender, apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATProfile.onExperimentSessionEnd() : begin"
            );

            // base call
            base.OnExperimentSessionEnd(sender, arg);

            // get our ISIR backend
            var backend 
                = apollon.backend.ApollonBackendManager.Instance.GetValidHandle(
                    apollon.backend.ApollonBackendManager.HandleIDType.ApollonISIRForceDimensionOmega3Handle
                ) as apollon.backend.handle.ApollonISIRForceDimensionOmega3Handle;

            // end session
            backend.NextGateKind          = "End";
            backend.NextGateSide          = "Session";
            backend.SharedIntentionMode   = "";
            backend.NextGateWorldPosition = new(0.0f, 0.0f, 0.0f);
            backend.NextGateWidth         = 0.0f;

            // deactivate all entity
            apollon.gameplay.ApollonGameplayManager.Instance.setInactive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.All
            );
            apollon.backend.ApollonBackendManager.Instance.RaiseHandleDeactivationRequestedEvent(
                apollon.backend.ApollonBackendManager.HandleIDType.ApollonISIRForceDimensionOmega3Handle
            );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATProfile.onExperimentSessionEnd() : end"
            );

        } /* onExperimentSessionEnd() */
        
        public override async void OnExperimentTrialBegin(object sender, apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATProfile.onExperimentTrialBegin() : begin"
            );

            // instantiate Settings & Results classes 
            this.CurrentSettings = new(this);
            this.CurrentResults  = new(this);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATProfile.onExperimentTrialBegin() : begin"
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
            var fog_bridge
                = (
                    apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                        apollon.gameplay.element.ApollonFogElementBridge
                    >(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.FogElement
                    )
                );
            
            // first of all, activate current checkpoint & cue manager to dynamically handle scene loading
            dynamic_entity.References["EntityTag_Checkpoints"].SetActive(true);
            dynamic_entity.References["EntityTag_Cues"].SetActive(true);
            
             // current visual
            if(this.CurrentSettings.Trial.visual_type == LEXIKHUMOATSettings.VisualIDType.Undefined)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> LEXIKHUMOATProfile.onExperimentTrialBegin() : UNDEFINED visual for pattern["
                    + this.CurrentSettings.Trial.pattern_type
                    + "]... check configuration files !"
                );

            } 
            else if(this.CurrentSettings.Trial.visual_type == LEXIKHUMOATSettings.VisualIDType.None)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATProfile.onExperimentTrialBegin() : No visual, skipping loading mechanism"
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

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATProfile.onExperimentTrialBegin() : loading visual["
                    + apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.Trial.visual_type)
                    + "]"
                );

            } /* if() */

            // current scene
            if(this.CurrentSettings.Trial.scene_type == LEXIKHUMOATSettings.SceneIDType.Undefined)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> LEXIKHUMOATProfile.onExperimentTrialBegin() : UNDEFINED scene for pattern["
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

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATProfile.onExperimentTrialBegin() : loading scene["
                    + apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.Trial.scene_type)
                    + "]"
                );

            } /* if() */
            
            // activate entity
            apollon.gameplay.ApollonGameplayManager.Instance.setActive(apollon.gameplay.ApollonGameplayManager.GameplayIDType.LEXIKHUMOATEntity);

            // synchronous wait loading scene
            await apollon.ApollonHighResolutionTime.DoSleep(2000.0f);

            // base call
            base.OnExperimentTrialBegin(sender, arg);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATProfile.onExperimentTrialBegin() : fading out !"
            );

            // async fade out
            this.DoBlankFadeOut(250.0f);
            this.DoLightFadeOut(250.0f);

            // show fog
            fog_bridge.ConcreteDispatcher.RaiseSmoothLinearFogRequested(
                this.CurrentSettings.Trial.fog_start_distance,
                this.CurrentSettings.Trial.fog_end_distance,
                new(
                    this.CurrentSettings.Trial.fog_color[0], 
                    this.CurrentSettings.Trial.fog_color[1], 
                    this.CurrentSettings.Trial.fog_color[2]
                ),
                250.0f
            );

            // Set camera clear flag
            UnityEngine.Camera.main.clearFlags = UnityEngine.CameraClearFlags.Color;
            UnityEngine.Camera.main.backgroundColor 
                = new(
                    this.CurrentSettings.Trial.fog_color[0], 
                    this.CurrentSettings.Trial.fog_color[1], 
                    this.CurrentSettings.Trial.fog_color[2]
                );

            // activate all subject control
            apollon.gameplay.ApollonGameplayManager.Instance.setActive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.LEXIKHUMOATControl
            );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATProfile.onExperimentTrialBegin() : trial protocol will start"
            );

            // build protocol
            await this.DoRunProtocol(
                async () => {
                    await this.DoWhileLoop(
                        () => {

                            // increment current retry count & check (top priority)
                            if(++this.CurrentResults.Trial.user_performance_try_count >= this.CurrentSettings.Trial.performance_max_try)
                                return false;

                            // then check performance criteria
                            return !(this.CurrentResults.Trial.user_performance_value >= this.CurrentSettings.Trial.performance_criteria);
                            
                        },
                        async () => { await this.SetState( new LEXIKHUMOATPhaseA(this) ); },
                        async () => { await this.SetState( new LEXIKHUMOATPhaseB(this) ); },
                        async () => { await this.SetState( new LEXIKHUMOATPhaseC(this) ); },
                        async () => { await this.SetState( new LEXIKHUMOATPhaseD(this) ); }
                    );
                },
                async () => {
                    await this.DoIfBranch(
                        () => {

                            // then check performance criteria
                            return this.CurrentResults.Trial.user_performance_value >= this.CurrentSettings.Trial.performance_criteria;

                        },
                        async () => { await this.SetState( new LEXIKHUMOATPhaseE(this) ); },
                        async () => { await this.SetState( new LEXIKHUMOATPhaseF(this) ); },
                        async () => { await this.SetState( new LEXIKHUMOATPhaseG(this) ); },
                        async () => { await this.SetState( new LEXIKHUMOATPhaseH(this) ); }
                    );
                },
                async () => {
                    await this.DoIfBranch(
                        () => {

                            // then if block size == 2 (aka. control block)
                            return (
                                apollon.experiment.ApollonExperimentManager.Instance.Session.CurrentBlock.trials.Count == 2
                            );

                        },
                        async () => { await this.SetState( new LEXIKHUMOATPhaseI(this) ); },
                        async () => { await this.SetState( new LEXIKHUMOATPhaseJ(this) ); },
                        async () => { await this.SetState( new LEXIKHUMOATPhaseK(this) ); },
                        async () => { await this.SetState( new LEXIKHUMOATPhaseL(this) ); },
                        async () => { await this.SetState( new LEXIKHUMOATPhaseM(this) ); },
                        async () => { await this.SetState( new LEXIKHUMOATPhaseN(this) ); },
                        async () => { await this.SetState( new LEXIKHUMOATPhaseO(this) ); },
                        async () => { await this.SetState( new LEXIKHUMOATPhaseP(this) ); }
                    );
                },
                async () => { await this.SetState( null ); }
            );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATProfile.onExperimentTrialBegin() : end"
            );

        } /* onExperimentTrialBegin() */
 
        public override async void OnExperimentTrialEnd(object sender, apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {

            // log
            UnityEngine.Debug.Log( 
                "<color=Blue>Info: </color> LEXIKHUMOATProfile.onExperimentTrialEnd() : begin"
            );

            // get a ref
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
            var fog_bridge
                = (
                    apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                        apollon.gameplay.element.ApollonFogElementBridge
                    >(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.FogElement
                    )
                ); 

            // inactivate all subject control
            apollon.gameplay.ApollonGameplayManager.Instance.setInactive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.LEXIKHUMOATControl
            );

            // default fog
            fog_bridge.ConcreteDispatcher.RaiseSmoothLinearFogRequested(
                0.0f,
                300.0f,
                UnityEngine.Color.white,
                250.0f
            );

            // reset camera clear flag
            UnityEngine.Camera.main.clearFlags = UnityEngine.CameraClearFlags.Skybox;

            // async fade in
            this.DoBlankFadeIn(250.0f);
            this.DoLightFadeIn(250.0f);

            // current visual
            if(this.CurrentSettings.Trial.visual_type == LEXIKHUMOATSettings.VisualIDType.Undefined)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> LEXIKHUMOATProfile.OnExperimentTrialEnd() : UNDEFINED visual for pattern["
                    + this.CurrentSettings.Trial.pattern_type
                    + "]... check configuration files !"
                );

            } 
            else if(this.CurrentSettings.Trial.visual_type == LEXIKHUMOATSettings.VisualIDType.None)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATProfile.OnExperimentTrialEnd() : No visual, skipping unloading mechanism"
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
            if(this.CurrentSettings.Trial.scene_type == LEXIKHUMOATSettings.SceneIDType.Undefined)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> LEXIKHUMOATProfile.OnExperimentTrialEnd() : UNDEFINED scene for pattern["
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

            // inactivate checkpoint manager
            dynamic_entity.References["EntityTag_Checkpoints"].SetActive(false);

            // export current trial results & log on completion
            if(this.CurrentResults.ExportUXFResults(arg.Trial.result))
            {
                this.CurrentResults.LogUXFResults();
            }
            
            // re draft ? 
            this.SubjectHasFailed = this.CurrentResults.Trial.user_performance_value < this.CurrentSettings.Trial.performance_criteria;
            // if (this.SubjectHasFailed) { ++this.CurrentResults.Trial.user_performance_try_count; }

            // base call
            base.OnExperimentTrialEnd(sender, arg);
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATProfile.onExperimentTrialEnd() : end"
            );

        } /* onExperimentTrialEnd() */
        
        #endregion

    } /* class LEXIKHUMOATProfile */
    
} /* } Labsim.experiment.LEXIKHUM_OAT */