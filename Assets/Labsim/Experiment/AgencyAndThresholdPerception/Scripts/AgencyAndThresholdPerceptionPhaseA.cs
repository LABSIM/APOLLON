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

// avoid namespace pollution
namespace Labsim.experiment.AgencyAndThresholdPerception
{

    //
    // User command input phase - FSM state
    //
    public sealed class AgencyAndThresholdPerceptionPhaseA
        : apollon.experiment.ApollonAbstractExperimentState<AgencyAndThresholdPerceptionProfile>
    {
        public AgencyAndThresholdPerceptionPhaseA(AgencyAndThresholdPerceptionProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionPhaseA.OnEntry() : begin"
            );
           
            // show grey(active)/green(passive) cross, green frame & counter if active
            if (this.FSM.CurrentSettings.bIsActive) 
            {
                apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);
            } 
            else
            {
                apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);
            }
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI);

            // // wait a certain amout of time
            await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_A_duration / 2.0f);

            // hide green frame first
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI);

            // if active condition 
            if (this.FSM.CurrentSettings.bIsActive)
            {

                bool bConditionIsOk = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> AgencyAndThresholdPerceptionPhaseA.OnEntry() : active condition"
                );

                // loop until condition is fullfilled 
                do
                {

                    // synchronisation mechanism (TCS + local function)
                    var sync_point = new System.Threading.Tasks.TaskCompletionSource<int>();
                    void sync_positive_local_function(object sender, AgencyAndThresholdPerceptionControlDispatcher.AgencyAndThresholdPerceptionControlEventArgs e)
                        => sync_point?.TrySetResult(1);
                    void sync_negative_local_function(object sender, AgencyAndThresholdPerceptionControlDispatcher.AgencyAndThresholdPerceptionControlEventArgs e)
                        => sync_point?.TrySetResult(-1);

                    // register our synchronisation function
                    (
                        apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                            apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionControl
                        ) as AgencyAndThresholdPerceptionControlBridge
                    ).ConcreteDispatcher.UserPositiveCommandTriggeredEvent += sync_positive_local_function;
                    (
                        apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                            apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionControl
                        ) as AgencyAndThresholdPerceptionControlBridge
                    ).ConcreteDispatcher.UserNegativeCommandTriggeredEvent += sync_negative_local_function;

                    // wait synchronisation point indefinitely & reset it once hit
                    this.FSM.CurrentResults.user_command = await sync_point.Task;

                    // found corresponding settings
                    bool bFoundTrialSettings = false;

                    // check if actual settings && command are congruant
                    if(UnityEngine.Mathf.Sign(this.FSM.CurrentResults.user_command) == UnityEngine.Mathf.Sign(this.FSM.CurrentSettings.phase_C_linear_acceleration_target[2]))
                    {

                        // no need to search, it's a simple match
                        bFoundTrialSettings = true;

                    }
                    // check if we are not in the last trial of the actual experiement
                    // nor the last of the actual block
                    else if(
                        (UXF.Session.instance.LastTrial != UXF.Session.instance.CurrentTrial)
                        && (UXF.Session.instance.NextTrial.numberInBlock != 1)
                    )
                    {

                        // then finally, search in current block if a trial will match requested condition & make it current if found   
                        for(int future_index = UXF.Session.instance.NextTrial.numberInBlock; 
                            future_index <= UXF.Session.instance.CurrentBlock.lastTrial.numberInBlock;
                            ++future_index
                        )
                        {     

                            // get future settings of acceleration & pattern name 
                            var future_settings_pattern
                                = UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.GetString(
                                    "current_pattern"
                                );
                            var future_settings_catch_try
                                = UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.GetBool(
                                    "is_catch_try_condition"
                                );
                            var future_settings_phase_C_angular_displacement_limiter_deg
                                = UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.GetFloatList(
                                    "phase_C_angular_displacement_limiter_deg"
                                ).ToArray();
                            var future_settings_phase_C_angular_acceleration_target_deg_per_s2
                                = UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.GetFloatList(
                                    "phase_C_angular_acceleration_target_deg_per_s2"
                                ).ToArray();
                            var future_settings_phase_C_angular_velocity_saturation_threshold_deg_per_s
                                = UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.GetFloatList(
                                    "phase_C_angular_velocity_saturation_threshold_deg_per_s"
                                ).ToArray();
                            var future_settings_phase_C_angular_mandatory_axis
                                = UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.GetBoolList(
                                    "phase_C_angular_mandatory_axis"
                                ).ToArray();
                            var future_settings_phase_C_linear_displacement_limiter_m
                                = UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.GetFloatList(
                                    "phase_C_linear_displacement_limiter_m"
                                ).ToArray();
                            var future_settings_phase_C_linear_acceleration_target_m_per_s2
                                = UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.GetFloatList(
                                    "phase_C_linear_acceleration_target_m_per_s2"
                                ).ToArray();
                            var future_settings_phase_C_linear_velocity_saturation_threshold_m_per_s
                                = UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.GetFloatList(
                                    "phase_C_linear_velocity_saturation_threshold_m_per_s"
                                ).ToArray();
                            var future_settings_phase_C_linear_mandatory_axis
                                = UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.GetBoolList(
                                    "phase_C_linear_mandatory_axis"
                                ).ToArray();

                            // check if future settings && command are congruant
                            if(UnityEngine.Mathf.Sign(this.FSM.CurrentResults.user_command) == UnityEngine.Mathf.Sign(future_settings_phase_C_linear_acceleration_target_m_per_s2[2]))
                            {

                                // perfect ! switch values of every settings & pattern name

                                UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.SetValue(
                                    "current_pattern",
                                    UXF.Session.instance.CurrentTrial.settings.GetString("current_pattern")
                                );
                                UXF.Session.instance.CurrentTrial.settings.SetValue("current_pattern",future_settings_pattern);

                                UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.SetValue(
                                    "is_catch_try_condition",
                                    this.FSM.CurrentSettings.bIsTryCatch
                                );
                                this.FSM.CurrentSettings.bIsTryCatch = future_settings_catch_try;

                                UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.SetValue(
                                    "phase_C_angular_displacement_limiter_deg",
                                    this.FSM.CurrentSettings.phase_C_angular_displacement_limiter.ToList()
                                );
                                future_settings_phase_C_angular_displacement_limiter_deg.CopyTo(this.FSM.CurrentSettings.phase_C_angular_displacement_limiter,0);
                                UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.SetValue(
                                    "phase_C_angular_acceleration_target_deg_per_s2",
                                    this.FSM.CurrentSettings.phase_C_angular_acceleration_target.ToList()
                                );
                                future_settings_phase_C_angular_acceleration_target_deg_per_s2.CopyTo(this.FSM.CurrentSettings.phase_C_angular_acceleration_target,0);
                                UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.SetValue(
                                    "phase_C_angular_velocity_saturation_threshold_deg_per_s",
                                    this.FSM.CurrentSettings.phase_C_angular_velocity_saturation_threshold.ToList()
                                );
                                future_settings_phase_C_angular_velocity_saturation_threshold_deg_per_s.CopyTo(this.FSM.CurrentSettings.phase_C_angular_velocity_saturation_threshold,0);
                                UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.SetValue(
                                    "phase_C_angular_mandatory_axis",
                                    this.FSM.CurrentSettings.phase_C_angular_mandatory_axis.ToList()
                                );
                                future_settings_phase_C_angular_mandatory_axis.CopyTo(this.FSM.CurrentSettings.phase_C_angular_mandatory_axis,0);

                                UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.SetValue(
                                    "phase_C_linear_displacement_limiter_m",
                                    this.FSM.CurrentSettings.phase_C_linear_displacement_limiter.ToList()
                                );
                                future_settings_phase_C_linear_displacement_limiter_m.CopyTo(this.FSM.CurrentSettings.phase_C_linear_displacement_limiter,0);
                                UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.SetValue(
                                    "phase_C_linear_acceleration_target_m_per_s2",
                                    this.FSM.CurrentSettings.phase_C_linear_acceleration_target.ToList()
                                );
                                future_settings_phase_C_linear_acceleration_target_m_per_s2.CopyTo(this.FSM.CurrentSettings.phase_C_linear_acceleration_target,0);
                                UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.SetValue(
                                    "phase_C_linear_velocity_saturation_threshold_m_per_s",
                                    this.FSM.CurrentSettings.phase_C_linear_velocity_saturation_threshold.ToList()
                                );
                                future_settings_phase_C_linear_velocity_saturation_threshold_m_per_s.CopyTo(this.FSM.CurrentSettings.phase_C_linear_velocity_saturation_threshold,0);
                                UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.SetValue(
                                    "phase_C_linear_mandatory_axis",
                                    this.FSM.CurrentSettings.phase_C_linear_mandatory_axis.ToList()
                                );
                                future_settings_phase_C_linear_mandatory_axis.CopyTo(this.FSM.CurrentSettings.phase_C_linear_mandatory_axis,0);

                                // mark as found
                                bFoundTrialSettings = true;

                                // exit loop
                                break;

                            } /* if() */

                        } /* for() */

                    } /* if() */

                    if(bFoundTrialSettings) 
                    {

                        if((UXF.Session.instance.LastTrial != UXF.Session.instance.CurrentTrial)
                            && (UXF.Session.instance.NextTrial.numberInBlock != 1)
                        ) 
                        {

                            // hack
                            if (this.FSM.CurrentResults.user_command > 0)
                            {

                                this.FSM.positiveConditionCount++;

                            }
                            else
                            {
                                
                                this.FSM.negativeConditionCount++;

                            } /* if() */

                        }
                        else
                        {
                            
                            this.FSM.positiveConditionCount = this.FSM.negativeConditionCount = 0;
                            
                        } /* if() */

                        // log
                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> AgencyAndThresholdPerceptionPhaseA.OnEntry() : user command request ["
                            + this.FSM.CurrentResults.user_command
                            + "]"
                        );

                        // mark condition as fullfilled
                        bConditionIsOk = true;

                    } 
                    // if not, notify failure to user & loop
                    else 
                    {

                        // invalidate user command
                        apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);
                        apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

                        // wait a certain amout of time
                        await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_A_duration / 2.0f);

                        // then turn back to default cross
                        apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);
                        apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

                    } /* if() */

                    // unregister our synchronisation function
                    (
                        apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                            apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionControl
                        ) as AgencyAndThresholdPerceptionControlBridge
                    ).ConcreteDispatcher.UserPositiveCommandTriggeredEvent -= sync_positive_local_function;
                    (
                        apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                            apollon.gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionControl
                        ) as AgencyAndThresholdPerceptionControlBridge
                    ).ConcreteDispatcher.UserNegativeCommandTriggeredEvent -= sync_negative_local_function;

                } while(!bConditionIsOk);

                // finally, validate user command
                apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);
                apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);

            }
            else
            {

                // passive condition == null command 
                this.FSM.CurrentResults.user_command = 0;
                    
            } /* if() */

            // fade in to black for vestibular-only scenario
            if (this.FSM.CurrentSettings.scenario_type == AgencyAndThresholdPerceptionProfile.Settings.ScenarioIDType.VestibularOnly)
            {

                // run it asynchronously
                this.FSM.DoFadeIn(this.FSM._trial_fade_in_duration);

            } /* if() */

            // wait a certain amout of time
            await apollon.ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_A_duration / 2.0f);

            // then hide green cross
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionPhaseA.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionPhaseA.OnExit() : begin"
            );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndThresholdPerceptionPhaseA.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class AgencyAndThresholdPerceptionPhaseA */

} /* } Labsim.experiment.AgencyAndThresholdPerception */
