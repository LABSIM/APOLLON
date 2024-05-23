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

    public class LEXIKHUMOATHapticCueBehaviour 
        : UnityEngine.MonoBehaviour
        , ILEXIKHUMOATCueBehaviour
    {

        [UnityEngine.SerializeField]
        private LEXIKHUMOATSettings.SharedIntentionIDType m_kind = LEXIKHUMOATSettings.SharedIntentionIDType.Haptic;
        public LEXIKHUMOATSettings.SharedIntentionIDType Kind { get => this.m_kind; private set => this.m_kind = value; }

        public void StartCue(LEXIKHUMOATResults.PhaseCResults.Checkpoint checkpoint)
        {

            // TODO 
            // send order to ISIR here...

        } /* StartCue() */

    } /* public class LEXIKHUMOATHapticCueBehaviour */

} /* } Labsim.experiment.LEXIKHUM_OAT */
