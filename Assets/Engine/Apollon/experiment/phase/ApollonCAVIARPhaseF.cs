using UXF;
using System.Linq;
using System.Threading.Tasks;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // Wait for input neutral - FSM State
    //
    public sealed class ApollonCAVIARPhaseF
        : ApollonAbstractExperimentState<profile.ApollonCAVIARProfile>
    {
        public ApollonCAVIARPhaseF(profile.ApollonCAVIARProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseF.OnEntry() : begin"
            );

            // inactivate entity
            gameplay.ApollonGameplayManager.Instance.setInactive(
                gameplay.ApollonGameplayManager.GameplayIDType.CAVIAREntity
            );

            // show grey cross & frame
            frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);
            frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.GreyFrameGUI);

            // wait a certain amout of time
            await this.FSM.DoSleep(this.FSM.CurrentSettings.phase_F_duration);

            // hide grey cross & frame
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.GreyFrameGUI);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseF.OnEntry() : end"
            );

        } /* OnEntry() */
        
        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseF.OnExit() : begin"
            );
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARPhaseF.OnExit() : end"
            );

        } /* OnExit() */

    } /* class ApollonCAVIARPhaseF */
    
} /* } Labsim.apollon.experiment.phase */