
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

    public class ApollonRedCrossGUIBehaviour : UnityEngine.MonoBehaviour
    {

        
        [UnityEngine.SerializeField]
        TMPro.TextMeshPro counterUI = null;
        
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

            if(counterUI != null)
            {

                counterUI.text  = experiment.ApollonExperimentManager.Instance.Profile.CounterStatus;
                counterUI.color = /* strong red */ new UnityEngine.Color(0.8f, 0.02f, 0.02f, 1.0f);

            } /*if() */

            if(instructionUI != null)
            {

                instructionUI.text  = experiment.ApollonExperimentManager.Instance.Profile.InstructionStatus;
                instructionUI.color = /* strong red */ new UnityEngine.Color(0.8f, 0.02f, 0.02f, 1.0f);

            } /*if() */

        } /* OnEnable() */

    } /* public class ApollonRedCrossElementBehaviour */

} /* } Labsim.apollon.frontend.gui */