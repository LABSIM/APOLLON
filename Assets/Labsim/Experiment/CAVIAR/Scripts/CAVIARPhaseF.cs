using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.CAVIAR
{

    //
    // Wait for input neutral - FSM State
    //
    public sealed class CAVIARPhaseF
        : apollon.experiment.ApollonAbstractExperimentState<CAVIARProfile>
    {
        public CAVIARPhaseF(CAVIARProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseF.OnEntry() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_F_results.timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_F_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // inactivate subject control
            apollon.gameplay.ApollonGameplayManager.Instance.setInactive(apollon.gameplay.ApollonGameplayManager.GameplayIDType.CAVIARControl);

            // show grey cross & frame
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyFrameGUI);

            // wait a certain amout of time
            await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_F_duration);

            // hide grey cross & frame
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyFrameGUI);

            // inactivate 
            apollon.gameplay.ApollonGameplayManager.Instance.setInactive(apollon.gameplay.ApollonGameplayManager.GameplayIDType.CAVIAREntity);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseF.OnEntry() : end"
            );

        } /* OnEntry() */
        
        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseF.OnExit() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_F_results.timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_F_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> CAVIARPhaseF.OnExit() : end"
            );

        } /* OnExit() */

    } /* class CAVIARPhaseF */
    
} /* } Labsim.apollon.experiment.phase */