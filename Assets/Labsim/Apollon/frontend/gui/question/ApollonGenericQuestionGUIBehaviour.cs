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

    public class ApollonGenericQuestionGUIBehaviour 
        : UnityEngine.MonoBehaviour
    {

        #region UI elements

        [UnityEngine.SerializeField]
        protected TMPro.TextMeshProUGUI m_questionText = null;
        public TMPro.TextMeshProUGUI QuestionText => this.m_questionText;

        [UnityEngine.SerializeField]
        protected UnityEngine.Color m_questionColor = UnityEngine.Color.black;
        public UnityEngine.Color QuestionColor => this.m_questionColor;

        [UnityEngine.SerializeField]
        protected TMPro.TextMeshProUGUI m_questionDetailText = null;
        public TMPro.TextMeshProUGUI QuestionDetailText => this.m_questionDetailText;

        [UnityEngine.SerializeField]
        protected UnityEngine.Color m_questionDetailColor = UnityEngine.Color.black;
        public UnityEngine.Color QuestionDetailColor => this.m_questionDetailColor;

        [UnityEngine.SerializeField]
        protected TMPro.TextMeshProUGUI m_questionTickLowerBoundText = null;
        public TMPro.TextMeshProUGUI QuestionTickLowerBoundText => this.m_questionTickLowerBoundText;

        [UnityEngine.SerializeField]
        protected TMPro.TextMeshProUGUI m_questionTickUpperBoundText = null;
        public TMPro.TextMeshProUGUI QuestionTickUpperBoundText => this.m_questionTickUpperBoundText;
        
        [UnityEngine.SerializeField]
        protected System.Collections.Generic.List<UnityEngine.GameObject> m_questionTick = new();
        public System.Collections.Generic.List<UnityEngine.GameObject> QuestionTick => this.m_questionTick;
        
        [UnityEngine.SerializeField]
        protected System.Collections.Generic.List<UnityEngine.GameObject> m_questionTickText = new();
        public System.Collections.Generic.List<UnityEngine.GameObject> QuestionTickText => this.m_questionTickText;

        #endregion

        void OnEnable()
        {

            if(this.QuestionText != null)
            {

                this.QuestionText.text  = experiment.ApollonExperimentManager.Instance.Profile.QuestionStatus;
                this.QuestionText.color = this.QuestionColor;

            } /* if() */
            
            if(this.QuestionDetailText != null)
            {

                this.QuestionDetailText.text  = experiment.ApollonExperimentManager.Instance.Profile.QuestionDetailStatus;
                this.QuestionDetailText.color = this.QuestionDetailColor;

            } /* if() */

            if(this.QuestionTickLowerBoundText != null)
            {

                this.QuestionTickLowerBoundText.text = experiment.ApollonExperimentManager.Instance.Profile.QuestionTickLowerBoundStatus;

            } /* if() */
        
            if(this.QuestionTickUpperBoundText != null)
            {

                this.QuestionTickUpperBoundText.text = experiment.ApollonExperimentManager.Instance.Profile.QuestionTickUpperBoundStatus;

            } /* if() */

            // hide/show all ticks & text
            this.QuestionTick.ForEach(
                (tickObj) => { 
                    tickObj.SetActive(experiment.ApollonExperimentManager.Instance.Profile.QuestionHasTickStatus); 
                }
            );
            this.QuestionTickText.ForEach(
                (tickText) => { 
                    tickText.SetActive(experiment.ApollonExperimentManager.Instance.Profile.QuestionHasTickTextStatus); 
                }
            );


        } /* OnEnable() */

        void OnDisable()
        {

            // hide all ticks
            this.QuestionTick.ForEach((tickObj) => tickObj.SetActive(false));
            this.QuestionTickText.ForEach((tickText) => tickText.SetActive(false));

        } /* OnDisable() */

    } /* public class ApollonGenericQuestionGUIBehaviour */

} /* } Labsim.apollon.frontend.gui */