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
            await this.FSM.DoSleep(this.FSM.CurrentSettings.phase_A_duration / 2.0f);

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
                            var future_settings_array_value 
                                = UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.GetFloatList(
                                    "phase_C_linear_acceleration_target_m_per_s2"
                                ).ToArray();
                            var future_settings_pattern
                                = UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.GetString(
                                    "current_pattern"
                                );

                            // check if future settings && command are congruant
                            if(UnityEngine.Mathf.Sign(this.FSM.CurrentResults.user_command) == UnityEngine.Mathf.Sign(future_settings_array_value[2]))
                            {

                                // perfect ! switch values of acceleration & pattern name
                                UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.SetValue(
                                    "phase_C_linear_acceleration_target_m_per_s2",
                                    this.FSM.CurrentSettings.phase_C_linear_acceleration_target.ToList()
                                );
                                future_settings_array_value.CopyTo(this.FSM.CurrentSettings.phase_C_linear_acceleration_target,0);
                                UXF.Session.instance.CurrentBlock.GetRelativeTrial(future_index).settings.SetValue(
                                    "current_pattern",
                                    UXF.Session.instance.CurrentTrial.settings.GetString("current_pattern")
                                );
                                UXF.Session.instance.CurrentTrial.settings.SetValue("current_pattern",future_settings_pattern);

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
                        await this.FSM.DoSleep(this.FSM.CurrentSettings.phase_A_duration / 2.0f);

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
            await this.FSM.DoSleep(this.FSM.CurrentSettings.phase_A_duration / 2.0f);

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
