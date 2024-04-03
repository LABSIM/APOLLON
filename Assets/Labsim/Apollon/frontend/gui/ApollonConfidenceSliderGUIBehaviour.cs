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
namespace Labsim.apollon.frontend.gui
{

    public class ApollonConfidenceSliderGUIBehaviour 
        : UnityEngine.MonoBehaviour
    {

        [UnityEngine.SerializeField]
        TMPro.TextMeshPro instructionUI = null;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        private void OnEnable() 
        {

            // if(instructionUI != null)
            // {

            //     instructionUI.text  = experiment.ApollonExperimentManager.Instance.Profile.InstructionStatus;
            //     instructionUI.color = /* black */ UnityEngine.Color.black;

            // } /*if() */

        } /* OnEnable() */

    } /* public class ApollonConfidenceSliderGUIBehaviour */

} /* } Labsim.apollon.frontend.gui */