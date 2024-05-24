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
using System.Linq;
using System.Windows.Forms;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Labsim.experiment.LEXIKHUM_OAT
{

    public class LEXIKHUMOATCueManagerBehaviour
        : UnityEngine.MonoBehaviour
    {

        [UnityEngine.SerializeField]
        private System.Collections.Generic.Dictionary<LEXIKHUMOATSettings.SharedIntentionIDType, ILEXIKHUMOATCueBehaviour> m_cues = new();
        public System.Collections.Generic.Dictionary<LEXIKHUMOATSettings.SharedIntentionIDType, ILEXIKHUMOATCueBehaviour> Cues => this.m_cues;

        private void Awake()
        {
            
            // crawl ICue
            foreach(ILEXIKHUMOATCueBehaviour cue in this.GetComponentsInChildren<ILEXIKHUMOATCueBehaviour>())
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATCueManagerBehaviour.Awake() : found cue behaviour ["
                    + apollon.ApollonEngine.GetEnumDescription(cue.Kind)
                    + "]"
                );
                
                // refs
                this.m_cues.Add(cue.Kind, cue);

            } /* foreach() */

        } /* Awake() */


        public void OnCheckpointReached(string parent_name, LEXIKHUMOATResults.PhaseCResults.Checkpoint checkpoint)
        {

            // process cues 
            switch((checkpoint.kind, checkpoint.side))
            {

                case (LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Departure, LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Center):
                case (LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Arrival, LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Center):
                {

                    // skip
                    break;

                } /* case (Departure, Center) || (Arrival, Center)*/

                case (LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Success, LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Left):
                case (LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Success, LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Right):
                case (LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Fail, LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Left):
                case (LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Fail, LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Right):
                {

                    // play practice ?
                    if(apollon.experiment.ApollonExperimentManager.Instance.Session.CurrentBlock.settings.GetBool("is_practice_condition"))
                    {
                        
                        // has refs ?
                        if(this.Cues.ContainsKey(LEXIKHUMOATSettings.SharedIntentionIDType.Practice))
                        {
                            
                            // log
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> LEXIKHUMOATCueManagerBehaviour.OnCheckpointReached() : found cue behaviour ["
                                + apollon.ApollonEngine.GetEnumDescription(LEXIKHUMOATSettings.SharedIntentionIDType.Practice)
                                + "], starting cue"
                            );

                            // start
                            this.Cues[LEXIKHUMOATSettings.SharedIntentionIDType.Practice].StartCue(checkpoint);

                        }
                        else
                        {

                            UnityEngine.Debug.LogWarning(
                                "<color=Orange>Warn: </color> LEXIKHUMOATCueManagerBehaviour.OnCheckpointReached() : is_practice_condition but no cue behaviour ["
                                + apollon.ApollonEngine.GetEnumDescription(LEXIKHUMOATSettings.SharedIntentionIDType.Practice)
                                + "]... strange"
                            );

                        } /* if() */
                        
                    } /* if() */ 

                    break;

                } /* case (Success, _) || (Fail, _)*/

                case (LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Cue, LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Left):
                case (LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Cue, LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Right):
                {

                    // extract current shared intention
                    var current_mode 
                        = (apollon.experiment.ApollonExperimentManager.Instance.Profile as LEXIKHUMOATProfile)
                            .CurrentSettings
                            .PhaseC
                            .shared_intention_type;
                        
                    // has refs ?
                    if(this.Cues.ContainsKey(current_mode))
                    {
                        
                        // log
                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> LEXIKHUMOATCueManagerBehaviour.OnCheckpointReached() : found cue behaviour ["
                            + apollon.ApollonEngine.GetEnumDescription(current_mode)
                            + "], starting cue"
                        );

                        // start
                        this.Cues[LEXIKHUMOATSettings.SharedIntentionIDType.Practice].StartCue(checkpoint);

                    }
                    else
                    {

                        UnityEngine.Debug.LogWarning(
                            "<color=Orange>Warn: </color> LEXIKHUMOATCueManagerBehaviour.OnCheckpointReached() : Cue ["
                            + apollon.ApollonEngine.GetEnumDescription(current_mode)
                            + "] requested but no associated behaviour has been found ! strange..."
                        );

                    } /* if() */
                    
                    break;

                } /* case (Cue, _) */

                default:
                {
                    
                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> LEXIKHUMOATCueManagerBehaviour.OnCheckpointReached() : (" 
                        + checkpoint.kind
                        + ","
                        + checkpoint.side
                        + ")... this is impossible ! :)"
                    );

                    break;

                } /* default || case Undefined */ 

            } /* switch() */

        } /**/

    } /* public class LEXIKHUMOATCueManagerBehaviour */

} /* } Labsim.experiment.LEXIKHUM_OAT */