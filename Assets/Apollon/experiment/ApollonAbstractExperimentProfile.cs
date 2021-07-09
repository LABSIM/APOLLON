// using directives
using UXF;
using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.experiment
{

    public abstract class ApollonAbstractExperimentProfile
    {

        // Experiment ID field
        protected ApollonExperimentManager.ProfileIDType m_profileID = ApollonExperimentManager.ProfileIDType.None;
        public ApollonExperimentManager.ProfileIDType ID
        {
            get
            {
                return this.m_profileID;
            }
        }

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

        #endregion

        #region subscription / unsubscription

        // registration
        public void onExperimentProfileActivationRequested(object sender, ApollonExperimentManager.EngineProfileEventArgs arg)
        {

            // check
            if (this.ID == arg.ProfileID)
            {

                // plug it
                ApollonEngine.Instance.EngineUpdateEvent += this.onUpdate;
                ApollonEngine.Instance.EngineExperimentSessionBeginEvent += this.onExperimentSessionBegin;
                ApollonEngine.Instance.EngineExperimentSessionEndEvent += this.onExperimentSessionEnd;
                ApollonEngine.Instance.EngineExperimentTrialBeginEvent += this.onExperimentTrialBegin;
                ApollonEngine.Instance.EngineExperimentTrialEndEvent += this.onExperimentTrialEnd;

                // register it
                ApollonExperimentManager.Instance.Profile = this;

            } /* if() */
  
        } /* onExperimentTrialBeginEvent() */

        // unregistration
        public void onExperimentProfileDeactivationRequested(object sender, ApollonExperimentManager.EngineProfileEventArgs arg)
        {

            // check
            if (this.ID == arg.ProfileID)
            {

                // plug it
                ApollonEngine.Instance.EngineUpdateEvent -= this.onUpdate;
                ApollonEngine.Instance.EngineExperimentSessionBeginEvent -= this.onExperimentSessionBegin;
                ApollonEngine.Instance.EngineExperimentSessionEndEvent -= this.onExperimentSessionEnd;
                ApollonEngine.Instance.EngineExperimentTrialBeginEvent -= this.onExperimentTrialBegin;
                ApollonEngine.Instance.EngineExperimentTrialEndEvent -= this.onExperimentTrialEnd;

                // register it
                ApollonExperimentManager.Instance.Profile = null;

            } /* if() */

        } /* onExperimentTrialBeginEvent() */

        #endregion

        #region actions
       
        public async System.Threading.Tasks.Task DoRunProtocol(params System.Func<System.Threading.Tasks.Task>[] protocol)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAbstractExperimentProfile.DoRunProtocol() : experimental protocol start now"
            );

            // simply
            foreach(var step in protocol) { await step(); }

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAbstractExperimentProfile.DoRunProtocol() : experimental protocol end"
            );

            // end Trial
            ApollonExperimentManager.Instance.Trial.End();

        } /* DoRunProtocol() */
        
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

            // iterate over seq
            UXF.Settings pattern_settings = new UXF.Settings(session.settings.GetDict("block_pattern"));
            foreach (var item in seq.Select((pattern, index) => new { index, pattern }))
            {

                // extract pattern dictionary
                UXF.Settings current_pattern_settings = new UXF.Settings(pattern_settings.GetDict(item.pattern));

                // create block
                UXF.Block block = session.CreateBlock(current_pattern_settings.GetStringList("trial_sequence").Count);

                // assign settings
                block.settings.SetValue("current_pattern", item.pattern);
                block.settings.SetValue("is_practice_condition", current_pattern_settings.GetBool("is_practice_condition"));

                // call child configuration
                this.DoBlockConfiguration(block, current_pattern_settings.GetStringList("trial_sequence"));

                // suffle it !
                if (current_pattern_settings.GetBool("is_sequence_randomized"))
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
    
        public async System.Threading.Tasks.Task DoFadeIn(float duration_in_ms, bool bASync = true)
        {

            // set start color
            var light_component = UnityEngine.Component.FindObjectsOfType<ApollonLightFader>().ToList().Find( x=>x.name == "[Apollon_Rig]" );

            // set and start fade in
            light_component.RequestFadeIn(duration_in_ms);

            // synchronous
            if (!bASync)
            {

                // synchronous wait
                await this.DoSleep(duration_in_ms);

            } /* if() */
            
        } /* DoFadeIn() */

        public async System.Threading.Tasks.Task DoFadeOut(float duration_in_ms, bool bASync = true)
        {

            // set start color
            var light_component = UnityEngine.Component.FindObjectsOfType<ApollonLightFader>().ToList().Find( x=>x.name == "[Apollon_Rig]" );

            // set and start fade out
            light_component.RequestFadeOut(duration_in_ms);

            // synchronous
            if (!bASync)
            {

                // synchronous wait
                await this.DoSleep(duration_in_ms);

            } /* if() */

        } /* DoFadeOut() */

        public async System.Threading.Tasks.Task DoSleep(float duration_in_ms)
        {
        
            // wait a certain amout of time
            var chrono = System.Diagnostics.Stopwatch.StartNew();
            while (chrono.ElapsedMilliseconds < duration_in_ms)
            {
                await System.Threading.Tasks.Task.Delay(10);
            }

        } /* DoSleep() */

        #endregion

        #region abstract method

        public virtual void onUpdate(object sender, ApollonEngine.EngineEventArgs arg)
        {
            
            // check request
            if (_trial_requested)
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
            if ((this._trial_chrono_is_running)
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

        public virtual void onExperimentSessionBegin(object sender, ApollonEngine.EngineExperimentEventArgs arg)
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

        public virtual async void onExperimentSessionEnd(object sender, ApollonEngine.EngineExperimentEventArgs arg)
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

        public virtual void onExperimentTrialBegin(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {

        } /* onExperimentTrialBegin() */

        public virtual void onExperimentTrialEnd(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {
            
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
    
