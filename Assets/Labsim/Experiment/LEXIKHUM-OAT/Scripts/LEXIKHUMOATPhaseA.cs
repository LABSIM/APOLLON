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

using System.Linq;
using System.Threading.Tasks;

// avoid namespace pollution
namespace Labsim.experiment.LEXIKHUM_OAT
{

    //
    // Internal init - FSM state
    //
    public sealed class LEXIKHUMOATPhaseA 
        : apollon.experiment.ApollonAbstractExperimentState<LEXIKHUMOATProfile>
    {
        public LEXIKHUMOATPhaseA(LEXIKHUMOATProfile fsm)
            : base(fsm)
        {
        }
        
        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATPhaseA.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.PhaseA.timing_on_entry_host_timestamp  = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.PhaseA.timing_on_entry_varjo_timestamp = Varjo.XR.VarjoTime.GetVarjoTimestamp();
            this.FSM.CurrentResults.PhaseA.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // currrent timestamp
            System.Diagnostics.Stopwatch current_stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // // refs
            // var motion_platform
            //     = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
            //         apollon.gameplay.device.AppollonGenericMotionSystemBridge
            //     >(
            //         apollon.gameplay.ApollonGameplayManager.GameplayIDType.GenericMotionSystem
            //     );

            // setup UI frontend instructions
            this.FSM.CurrentInstruction = "Initialisation";

            // show grey cross & frame
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyFrameGUI);
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATPhaseA.OnEntry() : initializing LEXIKHUMOAT Motion platform impedence system"
            );

            // // raise init motion event
            // motion_platform.ConcreteDispatcher.RaiseInit();
            
            // get elapsed 
            var remaining = this.FSM.CurrentSettings.PhaseA.duration - current_stopwatch.ElapsedMilliseconds;
            if(remaining > 0.0f)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> LEXIKHUMOATPhaseA.OnEntry() : waiting [" 
                    + remaining 
                    + "ms] for end of phase"
                );

            }
            else
            {
                
                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Warn: </color> LEXIKHUMOATPhaseA.OnEntry() : strange... no remaing time to wait..."
                );

            } /* if() */

            // wait end of phase 
            await apollon.ApollonHighResolutionTime.DoSleep(remaining);

            // hide grey cross & frame
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyFrameGUI);
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATPhaseA.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATPhaseA.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.PhaseA.timing_on_exit_host_timestamp  = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.PhaseA.timing_on_exit_varjo_timestamp = Varjo.XR.VarjoTime.GetVarjoTimestamp();
            this.FSM.CurrentResults.PhaseA.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> LEXIKHUMOATPhaseA.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class LEXIKHUMOATPhaseA */

} /* } Labsim.experiment.LEXIKHUM_OAT */