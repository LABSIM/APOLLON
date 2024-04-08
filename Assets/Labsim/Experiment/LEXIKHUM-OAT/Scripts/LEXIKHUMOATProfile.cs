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

        #region abstract implementation

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

            // deactivate all entity
            apollon.gameplay.ApollonGameplayManager.Instance.setInactive(
                apollon.gameplay.ApollonGameplayManager.GameplayIDType.All
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

            // export current trial results & log on completion
            if(this.CurrentResults.ExportUXFResults(arg.Trial.result))
            {
                this.CurrentResults.LogUXFResults();
            }
            
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