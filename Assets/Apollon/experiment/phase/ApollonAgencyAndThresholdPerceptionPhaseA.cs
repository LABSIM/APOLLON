using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.experiment.phase
{

    //
    // User command input phase - FSM state
    //
    public sealed class ApollonAgencyAndThresholdPerceptionPhaseA
        : ApollonAbstractExperimentState<profile.ApollonAgencyAndThresholdPerceptionProfile>
    {
        public ApollonAgencyAndThresholdPerceptionPhaseA(profile.ApollonAgencyAndThresholdPerceptionProfile fsm)
            : base(fsm)
        {
        }

        public async override System.Threading.Tasks.Task OnEntry()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseA.OnEntry() : begin"
            );
           
            // show grey(active)/green(passive) cross, green frame & counter if active
            if (this.FSM.CurrentSettings.bIsActive) 
            {
                frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);
            } 
            else
            {
                frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);
            }
            frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI);

            // // wait a certain amout of time
            await ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_A_duration / 2.0f);

            // hide green frame first
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.GreenFrameGUI);

            // if active condition 
            if (this.FSM.CurrentSettings.bIsActive)
            {

                bool bConditionIsOk = false;

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseA.OnEntry() : active condition"
                );

                // loop until condition is fullfilled 
                do
                {

                    // synchronisation mechanism (TCS + local function)
                    var sync_point = new System.Threading.Tasks.TaskCompletionSource<int>();
                    void sync_positive_local_function(object sender, gameplay.control.ApollonAgencyAndThresholdPerceptionControlDispatcher.EventArgs e)
                        => sync_point?.TrySetResult(1);
                    void sync_negative_local_function(object sender, gameplay.control.ApollonAgencyAndThresholdPerceptionControlDispatcher.EventArgs e)
                        => sync_point?.TrySetResult(-1);

                    // register our synchronisation function
                    (
                        gameplay.ApollonGameplayManager.Instance.getBridge(
                            gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionControl
                        ) as gameplay.control.ApollonAgencyAndThresholdPerceptionControlBridge
                    ).Dispatcher.UserPositiveCommandTriggeredEvent += sync_positive_local_function;
                    (
                        gameplay.ApollonGameplayManager.Instance.getBridge(
                            gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionControl
                        ) as gameplay.control.ApollonAgencyAndThresholdPerceptionControlBridge
                    ).Dispatcher.UserNegativeCommandTriggeredEvent += sync_negative_local_function;

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
                            "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseA.OnEntry() : user command request ["
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
                        frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);
                        frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

                        // wait a certain amout of time
                        await ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_A_duration / 2.0f);

                        // then turn back to default cross
                        frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);
                        frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

                    } /* if() */

                    // unregister our synchronisation function
                    (
                        gameplay.ApollonGameplayManager.Instance.getBridge(
                            gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionControl
                        ) as gameplay.control.ApollonAgencyAndThresholdPerceptionControlBridge
                    ).Dispatcher.UserPositiveCommandTriggeredEvent -= sync_positive_local_function;
                    (
                        gameplay.ApollonGameplayManager.Instance.getBridge(
                            gameplay.ApollonGameplayManager.GameplayIDType.AgencyAndThresholdPerceptionControl
                        ) as gameplay.control.ApollonAgencyAndThresholdPerceptionControlBridge
                    ).Dispatcher.UserNegativeCommandTriggeredEvent -= sync_negative_local_function;

                } while(!bConditionIsOk);

                // finally, validate user command
                frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.GreyCrossGUI);
                frontend.ApollonFrontendManager.Instance.setActive(frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);

            }
            else
            {

                // passive condition == null command 
                this.FSM.CurrentResults.user_command = 0;
                    
            } /* if() */

            // fade in to black for vestibular-only scenario
            if (this.FSM.CurrentSettings.scenario_type == profile.ApollonAgencyAndThresholdPerceptionProfile.Settings.ScenarioIDType.VestibularOnly)
            {

                // run it asynchronously
                this.FSM.DoFadeIn(this.FSM._trial_fade_in_duration);

            } /* if() */

            // wait a certain amout of time
            await ApollonHighResolutionTime.DoSleep(this.FSM.CurrentSettings.phase_A_duration / 2.0f);

            // then hide green cross
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseA.OnEntry() : end"
            );

        } /* OnEntry() */

        public async override System.Threading.Tasks.Task OnExit()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseA.OnExit() : begin"
            );

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAgencyAndThresholdPerceptionPhaseA.OnExit() : end"
            );

        } /* OnExit() */

    } /* public sealed class ApollonAgencyAndThresholdPerceptionPhaseA */

} /* } Labsim.apollon.experiment.phase */
