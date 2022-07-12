using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    //
    // FSM state
    //
    public sealed class TactilePhaseC
        : Labsim.apollon.experiment.ApollonAbstractExperimentState<TactileProfile>
    {
        public TactilePhaseC(TactileProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseC.OnEntry() : begin"
            );
 
            // save timestamps
            this.FSM.CurrentResults.phase_C_results.timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_C_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // bridge
            var haptic_bridge = TactileManager.Instance.getBridge(TactileManager.IDType.TactileHapticEntity) as TactileHapticEntityBridge;

            // switch on stim
            switch(this.FSM.CurrentSettings.phase_C_settings.stim_pattern)
            {
                
                case TactileProfile.Settings.PatternIDType.CC:
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> TactilePhaseC.OnEntry() : found CC settings, activate"
                    );

                    // activate
                    haptic_bridge.Dispatcher.RaiseStimCCRequested();
                    
                    break;
                
                } /* TactileProfile.Settings.PatternIDType.CC */

                case TactileProfile.Settings.PatternIDType.CV:
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> TactilePhaseC.OnEntry() : found CC settings, activate"
                    );

                    // activate
                    haptic_bridge.Dispatcher.RaiseStimCVRequested();
                    
                    break;
                
                } /* TactileProfile.Settings.PatternIDType.CV */

                case TactileProfile.Settings.PatternIDType.VC:
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> TactilePhaseC.OnEntry() : found CC settings, activate"
                    );

                    // activate
                    haptic_bridge.Dispatcher.RaiseStimVCRequested();
                    
                    break;
                
                } /* TactileProfile.Settings.PatternIDType.VC */

                case TactileProfile.Settings.PatternIDType.VV:
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> TactilePhaseC.OnEntry() : found CC settings, activate"
                    );

                    // activate
                    haptic_bridge.Dispatcher.RaiseStimVVRequested();
                    
                    break;
                
                } /* TactileProfile.Settings.PatternIDType.VV */

                default:
                case TactileProfile.Settings.PatternIDType.Undefined:
                {
                    
                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Info: </color> TactilePhaseC.OnEntry() : found default settings, skipping..."
                    );
                    
                    break;
                
                } /* default */ 

            } /* switch() */

            // wait a certain amout of time
            await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_C_settings.total_duration);

            // inactivate
            TactileManager.Instance.setInactive(TactileManager.IDType.TactileHapticEntity);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseC.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseC.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_C_results.timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_C_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> TactilePhaseC.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class TactilePhaseC */

} /* } Labsim.experiment.tactile */