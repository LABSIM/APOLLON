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

    public class LEXIKHUMOATPracticeCueBehaviour 
        : UnityEngine.MonoBehaviour
        , ILEXIKHUMOATCueBehaviour
    {

        [UnityEngine.SerializeField]
        private LEXIKHUMOATSettings.SharedIntentionIDType m_kind = LEXIKHUMOATSettings.SharedIntentionIDType.Practice;
        public LEXIKHUMOATSettings.SharedIntentionIDType Kind 
        { 
            get => this.m_kind; 
            private set => this.m_kind = value; 
        }

        [UnityEngine.SerializeField]
        private UnityEngine.AudioClip m_successClip;
        public UnityEngine.AudioClip SuccessClip => this.m_successClip;

        [UnityEngine.SerializeField]
        private UnityEngine.AudioClip m_failureClip;
        public UnityEngine.AudioClip FailureClip => this.m_failureClip;

        [UnityEngine.SerializeField]
        private UnityEngine.AudioSource m_leftSpeakerSource;
        public UnityEngine.AudioSource LeftSpeakerSource => this.m_leftSpeakerSource;

        [UnityEngine.SerializeField]
        private UnityEngine.AudioSource m_rightSpeakerSource;
        public UnityEngine.AudioSource RightSpeakerSource => this.m_rightSpeakerSource;

        public void StartCue(LEXIKHUMOATResults.PhaseCResults.Checkpoint checkpoint)
        {

            // play sound !
            this.LoadClip(checkpoint.kind);
            this.PlayClip(checkpoint.side);

        } /* StartCue() */

        private void LoadClip(LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType kind)
        {

            // load clip
            if(kind == LEXIKHUMOATResults.PhaseCResults.Checkpoint.KindIDType.Success)
            {
                
                this.LeftSpeakerSource.clip  
                    = this.RightSpeakerSource.clip 
                    = this.SuccessClip;

            }
            else
            {

                this.LeftSpeakerSource.clip  
                    = this.RightSpeakerSource.clip 
                    = this.FailureClip;

            } /* if() */

        } /* LoadClip() */

        private void PlayClip(LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType side)
        {

            switch(side)
            {

                case LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Center: 
                { 

                    this.RightSpeakerSource.Play(); 
                    this.LeftSpeakerSource.Play(); 

                    break;
                }

                case LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Right: 
                { 

                    this.RightSpeakerSource.Play(); 

                    break; 
                }

                default:
                case LEXIKHUMOATResults.PhaseCResults.Checkpoint.SideIDType.Left: 
                { 

                    this.LeftSpeakerSource.Play(); 

                    break; 
                }

            } /* switch() */

        } /* PlayClip() */

    } /* public class LEXIKHUMOATPracticeCueBehaviour */

} /* } Labsim.experiment.LEXIKHUM_OAT */
