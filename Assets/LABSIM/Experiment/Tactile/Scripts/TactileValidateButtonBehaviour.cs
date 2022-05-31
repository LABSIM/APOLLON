using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public class TactileValidateButtonBehaviour
        : UnityEngine.MonoBehaviour
    {

        // members
        private Leap.Unity.Interaction.InteractionButton m_button = null;

        #region MonoBehaviour Impl         
        
        [UnityEngine.SerializeField]
        private TactileTouchpointListBehaviour TouchpointListBehaviour = null;

        private void Start()
        {

            if((this.m_button = this.GetComponent<Leap.Unity.Interaction.InteractionButton>()) == null)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> TactileValidateButtonBehaviour.Start() : could not find component of type Leap.Unity.Interaction.InteractionButton from Unity..."
                );

            } /* if() */

        } /* Start() */

        private void Update()
        {
            
            // skip
            if(!this.m_button)
            {
                return;
            }

            // handle button activation
            if((this.TouchpointListBehaviour.Touchpoints.Count >= TactileTouchpointListBehaviour.s_touchpointMaxCount) && !this.m_button.controlEnabled)
            {
            
                this.m_button.controlEnabled = true;
            
            }
            else if((this.TouchpointListBehaviour.Touchpoints.Count < TactileTouchpointListBehaviour.s_touchpointMaxCount) && this.m_button.controlEnabled)
            {

                this.m_button.controlEnabled = false;

            } /* if() */ 

        } /* Update() */

        #endregion

        public void OnButtonPressed()
        {
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactileValidateButtonBehaviour.OnButtonPressed() : call"
            );


        } /* OnButtonPressed() */

    } /* class TactileValidateButtonBehaviour */

} /* } Labsim.experiment.tactile */