﻿//
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
namespace Labsim.experiment.AgencyAndThresholdPerceptionV4
{

    //
    // Reset/init condition phase - FSM state
    //
    public sealed class AgencyAndThresholdPerceptionV4PhaseF 
        : apollon.experiment.ApollonAbstractExperimentState<AgencyAndThresholdPerceptionV4Profile>
    {
        public AgencyAndThresholdPerceptionV4PhaseF(AgencyAndThresholdPerceptionV4Profile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseF.OnEntry() : begin"
            );

            // save timestamps
            this.FSM.CurrentResults.phase_F_results.timing_on_entry_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_F_results.timing_on_entry_unity_timestamp = UnityEngine.Time.time;

            // check visual degradation
            if(this.FSM.CurrentSettings.bWithVisualDegradation)
            {
                
                // enable fog
                UnityEngine.RenderSettings.fog = true;

                // setup fog type
                switch(this.FSM.CurrentSettings.fog_type)
                {
                    case AgencyAndThresholdPerceptionV4Profile.Settings.FogIDType.Linear:
                    {
                        UnityEngine.RenderSettings.fogMode = UnityEngine.FogMode.Linear;
                        break;
                    }

                    default:
                    case AgencyAndThresholdPerceptionV4Profile.Settings.FogIDType.Exponential:
                    {
                        UnityEngine.RenderSettings.fogMode = UnityEngine.FogMode.Exponential;
                        break;
                    }

                    case AgencyAndThresholdPerceptionV4Profile.Settings.FogIDType.ExponentialSquared:
                    {
                        UnityEngine.RenderSettings.fogMode = UnityEngine.FogMode.ExponentialSquared;
                        break;
                    }

                } /* switch() */

                // setup fog parameter
                UnityEngine.RenderSettings.fogColor   
                    = new UnityEngine.Color(
                        /* R */ this.FSM.CurrentSettings.fog_color[0], 
                        /* G */ this.FSM.CurrentSettings.fog_color[1], 
                        /* B */ this.FSM.CurrentSettings.fog_color[2]
                    );
                UnityEngine.RenderSettings.fogDensity       = this.FSM.CurrentSettings.fog_density;
                UnityEngine.RenderSettings.fogStartDistance = this.FSM.CurrentSettings.fog_start_distance;
                UnityEngine.RenderSettings.fogEndDistance   = this.FSM.CurrentSettings.fog_end_distance;

                // solid background
                UnityEngine.Camera.main.backgroundColor = UnityEngine.RenderSettings.fogColor;
                UnityEngine.Camera.main.clearFlags      = UnityEngine.CameraClearFlags.SolidColor;

            } /* if() */

            // fade in 
            await this.FSM.DoLightFadeIn(this.FSM._trial_fade_in_duration, false);

            // get bridge
            var motion_system_bridge
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.device.command.ApollonMotionSystemCommandBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.MotionSystemCommand
                );

            var virtual_motion_system_bridge
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.device.command.ApollonVirtualMotionSystemCommandBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.VirtualMotionSystemCommand
                );

            var seat_bridge
                = apollon.gameplay.ApollonGameplayManager.Instance.getConcreteBridge<
                    apollon.gameplay.entity.ApollonActiveSeatEntityBridge
                >(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.ActiveSeatEntity
                );

            // raise reset event
            switch (this.FSM.CurrentSettings.scenario_type)
            {

                default:
                case AgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VisualOnly:
                {   
                    virtual_motion_system_bridge.ConcreteDispatcher.RaiseReset(this.FSM.CurrentSettings.phase_F_settings.duration);
                    break;
                }
                case AgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VestibularOnly:
                case AgencyAndThresholdPerceptionV4Profile.Settings.ScenarioIDType.VisuoVestibular:
                {
                    virtual_motion_system_bridge.ConcreteDispatcher.RaiseReset(this.FSM.CurrentSettings.phase_F_settings.duration);
                    motion_system_bridge.ConcreteDispatcher.RaiseReset(this.FSM.CurrentSettings.phase_F_settings.duration);
                    break;
                }

            } /* switch() */

            // check
            if (seat_bridge == null)
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> AgencyAndThresholdPerceptionV4Profile.onExperimentTrialEnd() : Could not find corresponding gameplay bridge !"
                );

                // fail
                return;

            } /* if() */

            // get back to idle state
            seat_bridge.ConcreteDispatcher.RaiseIdle();

            // wait
            await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_F_settings.duration);

            // inhibit visual degradation
            if(this.FSM.CurrentSettings.bWithVisualDegradation)
            {

                // reset fog
                UnityEngine.RenderSettings.fog              = true;
                UnityEngine.RenderSettings.fogMode          = UnityEngine.FogMode.ExponentialSquared;
                UnityEngine.RenderSettings.fogColor         = UnityEngine.Color.white;
                UnityEngine.RenderSettings.fogDensity       = 0.005f;
                UnityEngine.RenderSettings.fogStartDistance = 0.0f;
                UnityEngine.RenderSettings.fogEndDistance   = 10000.0f;

                // skybox background
                UnityEngine.Camera.main.backgroundColor = UnityEngine.Color.white;
                UnityEngine.Camera.main.clearFlags      = UnityEngine.CameraClearFlags.Skybox;

            } /* if() */

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseF.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseF.OnExit() : begin"
            );
            
            // save timestamps
            this.FSM.CurrentResults.phase_F_results.timing_on_exit_host_timestamp = apollon.ApollonHighResolutionTime.Now.ToString();
            this.FSM.CurrentResults.phase_F_results.timing_on_exit_unity_timestamp = UnityEngine.Time.time;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionV4PhaseF.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class AgencyAndThresholdPerceptionV4PhaseF */

} /* } Labsim.experiment.AgencyAndThresholdPerceptionV4 */
