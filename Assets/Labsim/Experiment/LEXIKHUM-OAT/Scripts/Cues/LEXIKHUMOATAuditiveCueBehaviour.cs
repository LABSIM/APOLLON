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
namespace Labsim.experiment.LEXIKHUM_OAT
{

    public class LEXIKHUMOATAuditiveCueBehaviour 
        : UnityEngine.MonoBehaviour
        , ILEXIKHUMOATCueBehaviour
    {

        [UnityEngine.SerializeField]
        private LEXIKHUMOATSettings.SharedIntentionIDType m_kind = LEXIKHUMOATSettings.SharedIntentionIDType.Auditive;
        public LEXIKHUMOATSettings.SharedIntentionIDType Kind { get => this.m_kind; private set => this.m_kind = value; }

        [UnityEngine.SerializeField]
        private UnityEngine.AudioClip m_leftCueClip;
        public UnityEngine.AudioClip LeftCueClip => this.m_leftCueClip;

        [UnityEngine.SerializeField]
        private UnityEngine.AudioClip m_rightCueClip;
        public UnityEngine.AudioClip RightCueClip => this.m_rightCueClip;

        [UnityEngine.SerializeField]
        private UnityEngine.AudioClip m_leftStrongCueClip;
        public UnityEngine.AudioClip LeftStrongCueClip => this.m_leftStrongCueClip;

        [UnityEngine.SerializeField]
        private UnityEngine.AudioClip m_rightStrongCueClip;
        public UnityEngine.AudioClip RightStrongCueClip => this.m_rightStrongCueClip;

        [UnityEngine.SerializeField]
        private UnityEngine.AudioSource m_leftSpeakerSource;
        public UnityEngine.AudioSource LeftSpeakerSource => this.m_leftSpeakerSource;

        [UnityEngine.SerializeField]
        private UnityEngine.AudioSource m_rightSpeakerSource;
        public UnityEngine.AudioSource RightSpeakerSource => this.m_rightSpeakerSource;

        public void StartCue(LEXIKHUMOATResults.PhaseCResults.Checkpoint checkpoint)
        {

            // schedule
            apollon.ApollonEngine.Schedule(
                async () => {

                    // iff. neg. offset ?
                    var offset 
                        = (apollon.experiment.ApollonExperimentManager.Instance.Profile as LEXIKHUMOATProfile)
                            .CurrentSettings
                            .PhaseC
                            .shared_intention_offset;
                    if(UnityEngine.Mathf.Sign(offset) < .0f)
                    {

                        await apollon.ApollonHighResolutionTime.DoSleep(
                            UnityEngine.Mathf.Abs(offset)
                        );

                    } /* if() */

                    // play sound !
                    this.LoadClip(checkpoint.kind);
                    this.PlayClip(checkpoint.side);

                }
            );

        } /* StartCue() */

        private void LoadClip(LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType kind)
        {

            // load clip
            switch(kind)
            {

                case LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.StrongCue: 
                { 

                    this.LeftSpeakerSource.clip  = this.LeftStrongCueClip;
                    this.RightSpeakerSource.clip = this.RightStrongCueClip;

                    break; 
                }
                
                case LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Cue: 
                default:
                { 

                    this.LeftSpeakerSource.clip  = this.LeftCueClip;
                    this.RightSpeakerSource.clip = this.RightCueClip;

                    break; 
                }

            } /* switch() */

        } /* LoadClip() */

        private void PlayClip(LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType side)
        {

            switch(side)
            {

                case LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Right: 
                { 

                    this.RightSpeakerSource.Play(); 

                    break; 
                }

                case LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Left: 
                { 

                    this.LeftSpeakerSource.Play(); 

                    break; 
                }

                case LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Center: 
                default:
                { 

                    this.RightSpeakerSource.Play(); 
                    this.LeftSpeakerSource.Play(); 

                    break;
                }

            } /* switch() */

        } /* PlayClip() */

    } /* public class LEXIKHUMOATAuditiveCueBehaviour */

} /* } Labsim.experiment.LEXIKHUM_OAT */
