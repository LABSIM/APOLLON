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

// using directives
using UXF;
using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.experiment
{

    public abstract class ApollonAbstractExperimentProfile
    {

        // Experiment profile ID field
        protected ApollonExperimentManager.ProfileIDType m_profileID = ApollonExperimentManager.ProfileIDType.None;
        public ApollonExperimentManager.ProfileIDType ID
        {
            get
            {
                return this.m_profileID;
            }
        }

        // Experiment profile current status info
        public System.String Status
        {
            get
            {
                return this.getCurrentStatusInfo();
            }
        }
        protected virtual System.String getCurrentStatusInfo() { return ""; }

        public System.String CounterStatus
        {
            get
            {
                return this.getCurrentCounterStatusInfo();
            }
        }
        protected virtual System.String getCurrentCounterStatusInfo() { return ""; }

        public System.String InstructionStatus
        {
            get
            {
                return this.getCurrentInstructionStatusInfo();
            }
        }
        protected virtual System.String getCurrentInstructionStatusInfo() { return ""; }

        // constructor
        public ApollonAbstractExperimentProfile()
        {

        }

        #region trial transition handling

        // session inter-trial latency
        internal enum TrialType
        {
            Undefined = 0,
            First = 1,
            Next = 2,
            End = 3
        }
        internal TrialType 
            _trial_requested_type = TrialType.Undefined;
        internal bool 
            _trial_requested = false,
            _trial_chrono_is_running = false;
        internal float 
            _trial_sleep_duration = 2500.0f,
            _trial_fade_in_duration = 250.0f,
            _trial_fade_out_duration = 250.0f;
        internal readonly System.Diagnostics.Stopwatch _trial_chrono = new System.Diagnostics.Stopwatch();

        public bool SubjectHasFailed { get; set; } = false;

        private System.Collections.Generic.List< System.Collections.Generic.Dictionary<UXF.Trial, long> > m_draft_counter = null;

        #endregion

        #region subscription / unsubscription

        // registration
        public void OnExperimentProfileActivationRequested(object sender, ApollonExperimentManager.EngineProfileEventArgs arg)
        {

            // check
            if (this.ID == arg.ProfileID)
            {

                // plug it
                ApollonEngine.Instance.EngineUpdateEvent += this.OnUpdate;
                ApollonEngine.Instance.EngineExperimentSessionBeginEvent += this.OnExperimentSessionBegin;
                ApollonEngine.Instance.EngineExperimentSessionEndEvent += this.OnExperimentSessionEnd;
                ApollonEngine.Instance.EngineExperimentTrialBeginEvent += this.OnExperimentTrialBegin;
                ApollonEngine.Instance.EngineExperimentTrialEndEvent += this.OnExperimentTrialEnd;

                // register it
                ApollonExperimentManager.Instance.Profile = this;

            } /* if() */
  
        } /* onExperimentTrialBeginEvent() */

        // unregistration
        public void OnExperimentProfileDeactivationRequested(object sender, ApollonExperimentManager.EngineProfileEventArgs arg)
        {

            // check
            if (this.ID == arg.ProfileID)
            {

                // plug it
                ApollonEngine.Instance.EngineUpdateEvent -= this.OnUpdate;
                ApollonEngine.Instance.EngineExperimentSessionBeginEvent -= this.OnExperimentSessionBegin;
                ApollonEngine.Instance.EngineExperimentSessionEndEvent -= this.OnExperimentSessionEnd;
                ApollonEngine.Instance.EngineExperimentTrialBeginEvent -= this.OnExperimentTrialBegin;
                ApollonEngine.Instance.EngineExperimentTrialEndEvent -= this.OnExperimentTrialEnd;

                // register it
                ApollonExperimentManager.Instance.Profile = null;

            } /* if() */

        } /* onExperimentTrialBeginEvent() */

        #endregion

        #region actions
       
        protected async System.Threading.Tasks.Task DoRunProtocol(params System.Func<System.Threading.Tasks.Task>[] main_loop)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAbstractExperimentProfile.DoRunProtocol() : experimental protocol start now"
            );

            // simply
            await this.DoForeachLoop(main_loop);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAbstractExperimentProfile.DoRunProtocol() : experimental protocol end"
            );

            // end Trial
            ApollonExperimentManager.Instance.Trial.End();

        } /* DoRunProtocol() */
        
        protected async System.Threading.Tasks.Task DoWhileLoop(System.Func<bool> predicate, params System.Func<System.Threading.Tasks.Task>[] sub_loop)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAbstractExperimentProfile.DoWhileLoop() : begin"
            );

            // execute
            do 
            {
                
                await this.DoForeachLoop(sub_loop);

            } while(predicate());

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAbstractExperimentProfile.DoWhileLoop() : end"
            );

        } /* DoWhileLoop() */

        protected async System.Threading.Tasks.Task DoIfBranch(
            System.Func<bool> predicate, 
            params System.Func<System.Threading.Tasks.Task>[] if_branch)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAbstractExperimentProfile.DoIfBranch() : begin"
            );

            // execute
            if(predicate()) 
            {
                
                await this.DoForeachLoop(if_branch);

            };

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAbstractExperimentProfile.DoIfBranch() : end"
            );

        } /* DoIfBranch() */

        protected async System.Threading.Tasks.Task DoForeachLoop(params System.Func<System.Threading.Tasks.Task>[] sub_loop)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAbstractExperimentProfile.DoForeachLoop() : begin"
            );

            // execute
            foreach(var step in sub_loop) { await step(); }

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAbstractExperimentProfile.DoForeachLoop() : end"
            );

        } /* ForeachLoop() */

        public void DoBlockConfiguration(UXF.Block block, System.Collections.Generic.List<string> seq)
        {

            // iterate over seq
            UXF.Settings pattern_settings = new UXF.Settings(block.settings.GetDict("trial_pattern"));
            foreach (var item in seq.Select((pattern, index) => new { index, pattern }))
            {

                // extract pattern dictionary
                UXF.Settings current_pattern_settings = new UXF.Settings(pattern_settings.GetDict(item.pattern));

                // assign a pattern name (internal)
                block.trials[item.index].settings.SetValue(
                    "current_pattern",
                    item.pattern
                );

                // initialize our draft counter mecanism 
                this.m_draft_counter.Last().Add(block.trials[item.index], 0);

                // iterate over each found keys
                foreach (string key in current_pattern_settings.Keys)
                {

                    // extract specific per trial settings
                    block.trials[item.index].settings.SetValue(
                        key,
                        current_pattern_settings.GetObject(key)
                    );

                } /* foreach() */
                
            } /* foreach() */

        } /* DoBlockConfiguration() */

        public void DoSessionConfiguration(UXF.Session session)
        {
            // retrieve the block sequence, which was loaded from our .json file
            System.Collections.Generic.List<string> seq = session.settings.GetStringList("block_sequence");

            // increment our draft counter mechanism
            this.m_draft_counter = new(seq.Count);

            // iterate over seq
            UXF.Settings pattern_settings = new UXF.Settings(session.settings.GetDict("block_pattern"));
            foreach (var item in seq.Select((pattern, index) => new { index, pattern }))
            {

                // extract pattern dictionary
                UXF.Settings current_pattern_settings = new UXF.Settings(pattern_settings.GetDict(item.pattern));

                // create block
                UXF.Block block = session.CreateBlock(current_pattern_settings.GetStringList("trial_draft_bucket").Count);

                // update our draft counter mecanism 
                this.m_draft_counter.Add(new(block.trials.Count));

                // assign settings
                block.settings.SetValue("current_pattern", item.pattern);
                block.settings.SetValue("is_practice_condition", current_pattern_settings.GetBool("is_practice_condition"));
                block.settings.SetValue("trial_max_retry_per_draw", current_pattern_settings.GetLong("trial_max_retry_per_draw"));

                // call child configuration
                this.DoBlockConfiguration(block, current_pattern_settings.GetStringList("trial_draft_bucket"));

                // suffle it !
                if (current_pattern_settings.GetBool("is_draft_bucket_randomized"))
                {
                    block.trials.Shuffle();
                }

            } /* foreach() */

            // retrieve the inter trial setting, which was loaded from our .json file
            if (session.settings.Keys.ToList().Contains("trial_inter_sleep_duration_ms"))
            {

                this._trial_sleep_duration = session.settings.GetFloat("trial_inter_sleep_duration_ms");

            }
            else
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Info: </color> ApollonAbstractExperimentProfile.onExperimentTrialEnd() : could not find settings with key [trial_inter_sleep_duration_ms], set to default value :"
                    + this._trial_sleep_duration
                );

            }

            if (session.settings.Keys.ToList().Contains("trial_final_fade_in_duration_ms"))
            {

                this._trial_fade_in_duration = session.settings.GetFloat("trial_final_fade_in_duration_ms");

            }
            else
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Info: </color> ApollonAbstractExperimentProfile.onExperimentTrialEnd() : could not find settings with key [trial_final_fade_in_duration_ms], set to default value :"
                    + this._trial_fade_in_duration
                );

            } /* if() */

            if (session.settings.Keys.ToList().Contains("trial_initial_fade_out_duration_ms"))
            {

                this._trial_fade_out_duration = session.settings.GetFloat("trial_initial_fade_out_duration_ms");

            }
            else
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=Orange>Info: </color> ApollonAbstractExperimentProfile.onExperimentTrialEnd() : could not find settings with key [trial_initial_fade_out_duration_ms], set to default value :"
                    + this._trial_fade_out_duration
                );

            } /* if() */

        } /* DoSessionConfiguration() */
    
        public async System.Threading.Tasks.Task DoLightFadeIn(float duration_in_ms, bool bASync = true)
        {

            // set start color
            var light_component = UnityEngine.Component.FindObjectsOfType<ApollonLightFader>().ToList().Find( x=>x.name == "[Apollon_Rig]" );

            // set and start fade in
            light_component.RequestFadeIn(duration_in_ms);

            // synchronous
            if (!bASync)
            {

                // synchronous wait
                await ApollonHighResolutionTime.DoSleep(duration_in_ms);

            } /* if() */
            
        } /* DoLightFadeIn() */

        public async System.Threading.Tasks.Task DoLightFadeOut(float duration_in_ms, bool bASync = true)
        {

            // set start color
            var light_component = UnityEngine.Component.FindObjectsOfType<ApollonLightFader>().ToList().Find( x=>x.name == "[Apollon_Rig]" );

            // set and start fade out
            light_component.RequestFadeOut(duration_in_ms);

            // synchronous
            if (!bASync)
            {

                // synchronous wait
                await ApollonHighResolutionTime.DoSleep(duration_in_ms);

            } /* if() */

        } /* DoLightFadeOut() */

        public async System.Threading.Tasks.Task DoBlankFadeIn(float duration_in_ms, bool bASync = true)
        {

            // set start color
            var blank_component = UnityEngine.Component.FindObjectsOfType<ApollonBlankFader>().ToList().Find( x=>x.name == "[Apollon_Rig]" );

            // set and start fade in
            blank_component.RequestFadeIn(duration_in_ms);

            // synchronous
            if(!bASync)
            {

                // synchronous wait
                await ApollonHighResolutionTime.DoSleep(duration_in_ms);

            } /* if() */
            
        } /* DoBlankFadeIn() */

        public async System.Threading.Tasks.Task DoBlankFadeOut(float duration_in_ms, bool bASync = true)
        {

            // set start color
            var blank_component = UnityEngine.Component.FindObjectsOfType<ApollonLightFader>().ToList().Find( x=>x.name == "[Apollon_Rig]" );

            // set and start fade out
            blank_component.RequestFadeOut(duration_in_ms);

            // synchronous
            if(!bASync)
            {

                // synchronous wait
                await ApollonHighResolutionTime.DoSleep(duration_in_ms);

            } /* if() */

        } /* DoBlankFadeOut() */

        #endregion

        #region abstract method

        public virtual void OnUpdate(object sender, ApollonEngine.EngineEventArgs arg)
        {
            
            // check request
            if(_trial_requested)
            {

                // start chrono
                this._trial_chrono.Start();
                
                // inactivate everything first
                // frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.All);
                // gameplay.ApollonGameplayManager.Instance.setInactive(gameplay.ApollonGameplayManager.GameplayIDType.All);

                // reset request & raise running
                this._trial_requested = false;
                this._trial_chrono_is_running = true;

                // dbg
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAbstractExperimentProfile.onUpdate() : trial requested ["
                    + this._trial_sleep_duration
                    + "ms]"
                );

            } /* if () */

            // check chrono && start a certain amount of time after session begin exept for the first one
            if((this._trial_chrono_is_running)
                && (
                    (this._trial_chrono.ElapsedMilliseconds >= this._trial_sleep_duration)
                    || (this._trial_requested_type == TrialType.First)
                )
            ) {

                // stop & reset chrono then fall running
                this._trial_chrono.Stop();
                this._trial_chrono.Reset();
                this._trial_chrono_is_running = false;

                // raise corresponding signal
                switch (this._trial_requested_type)
                {
                    case TrialType.First:
                        {
                            // dbg
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> ApollonAbstractExperimentProfile.onUpdate() : first trial will begin now !"
                            );
                            // call
                            ApollonExperimentManager.Instance.Session.FirstTrial.Begin();
                            break;
                        } /* case TrialType.First */

                    case TrialType.Next:
                        {
                            // dbg
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> ApollonAbstractExperimentProfile.onUpdate() : next trial will begin now !"
                            );
                            // call
                            ApollonExperimentManager.Instance.Session.BeginNextTrial();
                            break;
                        } /* case TrialType.Next */

                    case TrialType.End:
                        {
                            // dbg
                            UnityEngine.Debug.Log(
                                "<color=Blue>Info: </color> ApollonAbstractExperimentProfile.onUpdate() : session will end now !"
                            );
                            // call
                            ApollonExperimentManager.Instance.Session.End();
                            break;
                        } /* case TrialType.End */

                    case TrialType.Undefined:
                    default:
                        {
                            // dbg
                            UnityEngine.Debug.LogError(
                                "<color=Red>Info: </color> ApollonAbstractExperimentProfile.onUpdate() : switch on Undefined value, this should never happen..."
                            );
                            break;
                        } /* case TrialType.Undefined || default */

                } /* switch() */

                // finally, reset
                this._trial_requested_type = TrialType.Undefined;

            } /* if () */

        } /* onUpdate() */

        public virtual void OnExperimentSessionBegin(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {

            // call config factories
            this.DoSessionConfiguration(arg.Session);
            
            // request
            this._trial_requested_type = TrialType.First;
            this._trial_requested = true;

            // inactivate everything first
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.All);
            gameplay.ApollonGameplayManager.Instance.setInactive(gameplay.ApollonGameplayManager.GameplayIDType.All);

        } /* onExperimentSessionBegin() */

        public virtual async void OnExperimentSessionEnd(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {

            // inactivate all
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.All);
            gameplay.ApollonGameplayManager.Instance.setInactive(gameplay.ApollonGameplayManager.GameplayIDType.All);
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAbstractExperimentProfile.onExperimentSessionEnd() : definitive end ok"
            );

            // stop
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            UnityEngine.Application.Quit();
#endif

        } /* onExperimentSessionEnd() */

        public virtual void OnExperimentTrialBegin(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {

            // reset fail condition
            this.SubjectHasFailed = false;

        } /* onExperimentTrialBegin() */

        public virtual void OnExperimentTrialEnd(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {
            
            // check failed condition
            if(this.SubjectHasFailed)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAbstractExperimentProfile.onExperimentTrialEnd() : Subject has failed to match performance criteria, check current draft counter"
                );

                // check current trial_max_retry_per_draw value
                if( ++(
                        this.m_draft_counter[
                            ApollonExperimentManager.Instance.Session.currentBlockNum
                        ][
                            ApollonExperimentManager.Instance.Session.CurrentTrial
                        ]
                    ) < ApollonExperimentManager.Instance.Session.CurrentBlock.settings.GetLong("trial_max_retry_per_draw")
                )
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonAbstractExperimentProfile.onExperimentTrialEnd() : re draw !"
                    );

                    // re draw
                    var re_draw_trial
                        = new UXF.Trial(ApollonExperimentManager.Instance.Session.CurrentBlock);
                    var re_draw_counter 
                        = this.m_draft_counter
                            [ApollonExperimentManager.Instance.Session.currentBlockNum]
                            [ApollonExperimentManager.Instance.Session.CurrentTrial];
                    var re_draw_pattern_name 
                        = ApollonExperimentManager.Instance.Session.CurrentTrial.settings.GetString("current_pattern");

                    // extract curtrent pattern dictionary
                    UXF.Settings re_draw_pattern_settings 
                        = new(
                            ApollonExperimentManager.Instance.Session.CurrentTrial.settings.GetDict(
                                re_draw_pattern_name
                            )
                        );

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonAbstractExperimentProfile.onExperimentTrialEnd() : trial with pattern["
                        + re_draw_pattern_name
                        + "] has been instanciated, re injecting each settings "
                    );

                    // iterate over each found keys
                    foreach (string key in re_draw_pattern_settings.Keys)
                    {

                        // extract specific per trial settings
                        re_draw_trial.settings.SetValue(
                            key,
                            re_draw_pattern_settings.GetObject(key)
                        );

                        // log
                        UnityEngine.Debug.Log(
                            "<color=Blue>Info: </color> ApollonAbstractExperimentProfile.onExperimentTrialEnd() : trial with pattern["
                            + re_draw_pattern_name
                            + "], injected key ["
                            + key
                            + "]"
                        );

                    } /* foreach() */

                    // update our draft counter mecanism by erasing previous ref & place new one
                    if(!(
                        this.m_draft_counter[
                            ApollonExperimentManager.Instance.Session.currentBlockNum
                        ].Remove(
                            ApollonExperimentManager.Instance.Session.CurrentTrial
                        )
                    )) 
                    {

                        // fail...
                        UnityEngine.Debug.LogError(
                            "<color=Red>Error: </color> ApollonAbstractExperimentProfile.onExperimentTrialEnd() : failed to remove previous ref from internal counter mecanism... stange"
                        );

                    }
                    else
                    {
    
                        // new counter 
                        this.m_draft_counter[
                            ApollonExperimentManager.Instance.Session.currentBlockNum
                        ].Add(
                            re_draw_trial,
                            re_draw_counter
                        );

                        // re insert pattern at random place 
                        ApollonExperimentManager.Instance.Session.CurrentBlock.trials.Insert(
                            new System.Random().Next(
                                ApollonExperimentManager.Instance.Session.CurrentTrial.numberInBlock + 1,
                                ApollonExperimentManager.Instance.Session.CurrentBlock.trials.Count
                            ),
                            re_draw_trial
                        );

                    } /* if() */

                }
                else 
                {
                    
                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonAbstractExperimentProfile.onExperimentTrialEnd() : no re draw available, subject is weak ;)"
                    );

                } /* if() */
            
            }
            else
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAbstractExperimentProfile.onExperimentTrialEnd() : subject is strong ! subject has succedeed "
                );
            
            } /* if() */

            // check if there is any trial left
            if (ApollonExperimentManager.Instance.Session.CurrentTrial == ApollonExperimentManager.Instance.Session.LastTrial)
            {

                // log
                UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonAbstractExperimentProfile.onExperimentTrialEnd() : final trial, session will exit ");

                // request end
                this._trial_requested_type = TrialType.End;
                this._trial_requested = true;

            }
            else
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAbstractExperimentProfile.onExperimentTrialEnd() : there is stil trial to run, current: [ block:"
                    + ApollonExperimentManager.Instance.Session.currentBlockNum
                    + " / trial: "
                    + ApollonExperimentManager.Instance.Session.currentTrialNum
                    + " ]"
                );

                // request next trial
                this._trial_requested_type = TrialType.Next;
                this._trial_requested = true;

            } /* if() */

        } /* onExperimentTrialEnd() */

        #endregion

    } /* abstract class ApollonAbstractExperimentProfile */

} /* namespace Labsim.apollon.experiment */
    
