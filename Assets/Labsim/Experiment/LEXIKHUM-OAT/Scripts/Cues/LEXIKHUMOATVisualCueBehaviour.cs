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
using Labsim.apollon;

namespace Labsim.experiment.LEXIKHUM_OAT
{

    public class LEXIKHUMOATVisualCueBehaviour 
        : UnityEngine.MonoBehaviour
        , ILEXIKHUMOATCueBehaviour
    {

        [UnityEngine.SerializeField]
        private LEXIKHUMOATSettings.SharedIntentionIDType m_kind = LEXIKHUMOATSettings.SharedIntentionIDType.Visual;
        public LEXIKHUMOATSettings.SharedIntentionIDType Kind { get => this.m_kind; private set => this.m_kind = value; }

        public void StartCue(LEXIKHUMOATResults.PhaseCResults.Checkpoint checkpoint)
        {

            // check side 
            string entityTag 
                = "EntityTag_" + ApollonEngine.GetEnumDescription(checkpoint.side) + "Cue";
            var dynamicEntityBehaviour 
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.entity.ApollonDynamicEntityBridge
                >(apollon.gameplay.ApollonGameplayManager.GameplayIDType.DynamicEntity)
                .ConcreteBehaviour;

            // check
            if(!dynamicEntityBehaviour.References.ContainsKey(entityTag))
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> LEXIKHUMOATVisualCueBehaviour.StartCue() : visual cue is required to start but checkpoint.side is inconsistent ["
                    + apollon.ApollonEngine.GetEnumDescription(checkpoint.side)
                    + "]... strange"
                );

                // exit
                return;

            }
            else
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATVisualCueBehaviour.StartCue() : required checkpoint.side is ["
                    + apollon.ApollonEngine.GetEnumDescription(checkpoint.side)
                    + "]... will activate cue"
                );

            } /* if()*/

            // schedule
            apollon.ApollonEngine.Schedule(
                async () => {

                    // offset ?
                    await apollon.ApollonHighResolutionTime.DoSleep( 
                        (apollon.experiment.ApollonExperimentManager.Instance.Profile as LEXIKHUMOATProfile).CurrentSettings.PhaseC.shared_intention_offset
                    );

                    // show cue !
                    dynamicEntityBehaviour.References[entityTag].SetActive(true);

                    // tieout
                    await apollon.ApollonHighResolutionTime.DoSleep( 
                        (apollon.experiment.ApollonExperimentManager.Instance.Profile as LEXIKHUMOATProfile).CurrentSettings.PhaseC.shared_intention_duration
                    );

                    // finally, hide
                    dynamicEntityBehaviour.References[entityTag].SetActive(false);

                }
            ); /* Schedule() */ 

        } /* StartCue() */

    } /* public class LEXIKHUMOATVisualCueBehaviour */

} /* } Labsim.experiment.LEXIKHUM_OAT */