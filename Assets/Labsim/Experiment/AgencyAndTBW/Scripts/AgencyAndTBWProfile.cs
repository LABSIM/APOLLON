
// avoid namespace pollution
namespace Labsim.experiment.AgencyAndTBW
{
    
    public class AgencyAndTBWProfile 
        : apollon.experiment.ApollonAbstractExperimentProfile
    {
        // Ctor
        public AgencyAndTBWProfile()
            : base()
        {
            // default profile
            this.m_profileID = apollon.experiment.ApollonExperimentManager.ProfileIDType.AgencyAndTBW;
        }

        #region internal settings/result

        // active camera settings color 
        internal readonly UnityEngine.CameraClearFlags _cameraFlags = UnityEngine.CameraClearFlags.Skybox;
        internal readonly UnityEngine.Color _cameraBackgroundColor = new UnityEngine.Color(49.0f, 77.0f, 121.0f, 0.0f);

        // blank camera settings color 
        internal readonly UnityEngine.CameraClearFlags _blankCameraFlags = UnityEngine.CameraClearFlags.SolidColor;
        internal readonly UnityEngine.Color _blankCameraBackgroundColor = UnityEngine.Color.grey;

        internal class Settings
        {
          
            public bool bIsActive;

            public float
                upstream_latency_offset,
                acceleration_offset,
                speed_initial,
                speed_saturation,
                acceleration_pattern_very_low,
                acceleration_pattern_low,
                acceleration_pattern_high,
                acceleration_pattern_very_high,
                phase_timeout_A_latency,
                phase_timeout_B0_latency,
                phase_timeout_B1_latency,
                phase_timeout_B2_latency,
                phase_timeout_C_SOA,
                phase_timeout_D_latency,
                phase_timeout_E0_latency,
                phase_timeout_E1_latency,
                phase_timeout_E2_latency;

            public string acceleration_pattern_sequence;

        } /* class Settings */

        internal class Results
        {

            public string
                user_acceleration_pattern,
                user_response;

            public UnityEngine.AudioClip
                user_clip;

        } /* class Results */

        // internal
        internal Settings _settings = new Settings();
        internal Results _results = new Results();

        #endregion

        #region experimentation actions

        public void DoPromptUserSpeedSelection()
        {
          
            // prompt
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.SpeedSelectionGUI);
       
        } /* DoPromptUserSpeedSelection() */

        public async void DoRespondToUserSpeedSelection(string user_response)
        {

            // log result
            this._results.user_acceleration_pattern = user_response;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndTBWProfile.DoRespondToUserSpeedSelection() : user_acceleration_pattern ["
                + this._results.user_acceleration_pattern
                + "]"
            );

            // go inactive
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.SpeedSelectionGUI);

            // wait a certain amout of time
            System.Diagnostics.Stopwatch chrono = new System.Diagnostics.Stopwatch();
            chrono.Start();
            while (chrono.ElapsedMilliseconds < this._settings.phase_timeout_B2_latency)
            {
                await System.Threading.Tasks.Task.Delay(5);
            }
            chrono.Stop();

            // finally Do phase C
            this.DoPhaseC();

        } /* DoRespondToUserSpeedSelection() */

        public void DoPromptUserResponse()
        {

            // prompt
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.ResponseGUI);
       
        } /* PromptUserResponse() */

        public async void DoRespondToUserResponse(string user_response)
        {

            // log result
            this._results.user_response = user_response;

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndTBWProfile.DoRespondToUserResponse() : user_response ["
                + this._results.user_response
                + "]"
            );

            // go inactive
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.ResponseGUI);

            // wait a certain amout of time
            System.Diagnostics.Stopwatch chrono = new System.Diagnostics.Stopwatch();
            chrono.Start();
            while (chrono.ElapsedMilliseconds < this._settings.phase_timeout_E2_latency)
            {
                await System.Threading.Tasks.Task.Delay(5);
            }
            chrono.Stop();

             // log responses & next trial
             this.DoLogResultToSessionAndEndTrial();

        } /* DoRespondToUserResponse() */

        public void DoLogResultToSessionAndEndTrial()
        {

            // log result
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_acceleration_pattern"] = this._results.user_acceleration_pattern;
            apollon.experiment.ApollonExperimentManager.Instance.Trial.result["user_response"] = this._results.user_response;

            // Raise currennt end trial
            apollon.experiment.ApollonExperimentManager.Instance.Trial.End();

        } /* DoLogResultToSessionAndEndTrial() */

        public async void DoPhaseA()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndTBWProfile.DoPhaseA() : begin"
            );

            // activate gameplay element & configure camera
            apollon.gameplay.ApollonGameplayManager.Instance.setActive(apollon.gameplay.ApollonGameplayManager.GameplayIDType.WorldElement);
            UnityEngine.Camera.main.clearFlags = this._cameraFlags;
            UnityEngine.Camera.main.backgroundColor = this._cameraBackgroundColor;

            // // activate audio recording 
            // this._results.user_clip
            //     = UnityEngine.Microphone.Start(
            //         UnityEngine.Microphone.devices[0],
            //         true,
            //         45,
            //         44100
            //     );

            // activate moving gameplay element
            apollon.gameplay.ApollonGameplayManager.Instance.setActive(apollon.gameplay.ApollonGameplayManager.GameplayIDType.RealRobosoftEntity);
            apollon.gameplay.ApollonGameplayManager.Instance.setActive(apollon.gameplay.ApollonGameplayManager.GameplayIDType.SimulatedRobosoftEntity);

            // Speed up - initial settings

            (
                apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.SimulatedRobosoftEntity
                ).Behaviour
                as apollon.gameplay.entity.ApollonSimulatedRobosoftEntityBehaviour
            ).Start(this._settings.speed_initial);

            (
                apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.RealRobosoftEntity
                ).Behaviour
                as apollon.gameplay.entity.ApollonRealRobosoftEntityBehaviour
            ).Start(this._settings.speed_initial);

            // wait a certain amout of time
            System.Diagnostics.Stopwatch chrono = new System.Diagnostics.Stopwatch();
            chrono.Start();
            while (chrono.ElapsedMilliseconds < this._settings.phase_timeout_A_latency)
            {
                await System.Threading.Tasks.Task.Delay(5);
            }
            chrono.Stop();

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndTBWProfile.DoPhaseA() : end"
            );

            // then, phase B
            this.DoPhaseB();

        } /* DoPhaseA() */

        public async void DoPhaseB()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndTBWProfile.DoPhaseB() : begin"
            );

            // show green cross
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);

            // wait a certain amout of time
            System.Diagnostics.Stopwatch chrono_0 = new System.Diagnostics.Stopwatch();
            chrono_0.Start();
            while (chrono_0.ElapsedMilliseconds < this._settings.phase_timeout_B0_latency)
            {
                await System.Threading.Tasks.Task.Delay(5);
            }
            chrono_0.Stop();

            // hide green cross
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.GreenCrossGUI);

            // wait a certain amout of time
            System.Diagnostics.Stopwatch chrono_1 = new System.Diagnostics.Stopwatch();
            chrono_1.Start();
            while (chrono_1.ElapsedMilliseconds < this._settings.phase_timeout_B1_latency)
            {
                await System.Threading.Tasks.Task.Delay(5);
            }
            chrono_1.Stop();

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndTBWProfile.doPhaseB() : end"
            );

            // finally, prompt user if active & wait or assign current acceleration pattern & continue directly to phase A
            if (this._settings.bIsActive)
            {

                // prompt & wait for response
                this.DoPromptUserSpeedSelection();

            }
            else
            {

                // assign results
                this._results.user_acceleration_pattern = this._settings.acceleration_pattern_sequence;

                // & action !
                this.DoPhaseC();

            } /* if() */

        } /* DoPhaseB() */

        public async void DoPhaseC()
        {
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndTBWProfile.DoPhaseC() : begin"
            );

            // calculate the real world SOA
            float RW_SOA = this._settings.phase_timeout_C_SOA + this._settings.upstream_latency_offset;
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndTBWProfile.DoPhaseC() : calculated RealWorld SOA : [" 
                + RW_SOA
                + "]"
            );

            // get acceleration pattern
            
            string pattern = "";
            if(this._settings.bIsActive)
            {
                pattern = this._results.user_acceleration_pattern;
            }
            else
            {
                pattern = this._settings.acceleration_pattern_sequence;
            }

            // get acceleration value 

            float RW_acc = 0.0f;
            if(pattern.Equals("very_low"))
            {
                RW_acc = this._settings.acceleration_pattern_very_low;
            }
            else if (pattern.Equals("low")) 
            {
                RW_acc = this._settings.acceleration_pattern_low;
            }
            else if (pattern.Equals("high")) 
            {
                RW_acc = this._settings.acceleration_pattern_high;
            }
            else if (pattern.Equals("very_high")) 
            {
                RW_acc = this._settings.acceleration_pattern_very_high;
            }
            
            // add acc offset
            RW_acc += this._settings.acceleration_offset;

            // print
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndTBWProfile.DoPhaseC() : calculated RealWorld Acceleration : ["
                + RW_acc
                + "]"
            );

            // extract current time
            System.Diagnostics.Stopwatch start_stim_chrono = new System.Diagnostics.Stopwatch();
            start_stim_chrono.Start();

            // switch on
            int offset_sign = System.Math.Sign(RW_SOA);
            RW_SOA = System.Math.Abs(RW_SOA);
            if (offset_sign == 0)
            {

                // congruant soooo... fuck.

                // Accelerate Simulated & Real "together"

                (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.SimulatedRobosoftEntity
                    ).Behaviour
                    as apollon.gameplay.entity.ApollonSimulatedRobosoftEntityBehaviour
                ).Accelerate(this._settings.speed_saturation, RW_acc);

                (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.RealRobosoftEntity
                    ).Behaviour
                    as apollon.gameplay.entity.ApollonRealRobosoftEntityBehaviour
                ).Accelerate(this._settings.speed_saturation, RW_acc);

            }
            else if (offset_sign < 0)
            {

                // less so visual -> delta -> vestibulaire

                // Accelerate Simulated first

                (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.SimulatedRobosoftEntity
                    ).Behaviour
                    as apollon.gameplay.entity.ApollonSimulatedRobosoftEntityBehaviour
                ).Accelerate(this._settings.speed_saturation, RW_acc);

                // wait a certain amout of time
                System.Diagnostics.Stopwatch SOA_chrono = new System.Diagnostics.Stopwatch();
                SOA_chrono.Start();
                while (SOA_chrono.ElapsedMilliseconds < RW_SOA)
                {
                    await System.Threading.Tasks.Task.Delay(10);
                }
                SOA_chrono.Stop();

                // then accelerate robot

                (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.RealRobosoftEntity
                    ).Behaviour
                    as apollon.gameplay.entity.ApollonRealRobosoftEntityBehaviour
                ).Accelerate(this._settings.speed_saturation, RW_acc);

            }
            else
            {

                // greater so vestibulaire -> delta -> visual 

                // first, real robot
                (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.RealRobosoftEntity
                    ).Behaviour
                    as apollon.gameplay.entity.ApollonRealRobosoftEntityBehaviour
                ).Accelerate(this._settings.speed_saturation, RW_acc);

                // wait a certain amout of time
                System.Diagnostics.Stopwatch SOA_chrono = new System.Diagnostics.Stopwatch();
                SOA_chrono.Start();
                while (SOA_chrono.ElapsedMilliseconds < RW_SOA)
                {
                    await System.Threading.Tasks.Task.Delay(10);
                }
                SOA_chrono.Stop();

                // Accelerate Simulated then

                (
                    apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                        apollon.gameplay.ApollonGameplayManager.GameplayIDType.SimulatedRobosoftEntity
                    ).Behaviour
                    as apollon.gameplay.entity.ApollonSimulatedRobosoftEntityBehaviour
                ).Accelerate(this._settings.speed_saturation, RW_acc);

            } /* if() */

            // get elapsed stim time & stop
            float elapsed_stim_time = start_stim_chrono.ElapsedMilliseconds;
            start_stim_chrono.Stop();

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndTBWProfile.DoPhaseC() : end"
            );

            // then, phase C
            this.DoPhaseD(elapsed_stim_time);

        } /* DoPhaseC() */

        public async void DoPhaseD(float elapsed_stim_time)
        {
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndTBWProfile.DoPhaseD() : begin"
            );


            // get remaining stim time & stop
            float remaining_stim_time = (this._settings.phase_timeout_D_latency - elapsed_stim_time);

            // wait a certain amout of time
            System.Diagnostics.Stopwatch remaining_stim_chrono = new System.Diagnostics.Stopwatch();
            remaining_stim_chrono.Start();
            while (remaining_stim_chrono.ElapsedMilliseconds < remaining_stim_time)
            {
                await System.Threading.Tasks.Task.Delay(5);
            }
            remaining_stim_chrono.Stop();

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndTBWProfile.DoPhaseD() : end"
            );

            // then, phase E
            this.DoPhaseE();

        } /* DoPhaseD() */

        public async void DoPhaseE()
        {
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndTBWProfile.DoPhaseE() : begin"
            );

            // Stop entities

            (
                apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.SimulatedRobosoftEntity
                ).Behaviour
                as apollon.gameplay.entity.ApollonSimulatedRobosoftEntityBehaviour
            ).Stop();

            (
                apollon.gameplay.ApollonGameplayManager.Instance.getBridge(
                    apollon.gameplay.ApollonGameplayManager.GameplayIDType.RealRobosoftEntity
                ).Behaviour
                as apollon.gameplay.entity.ApollonRealRobosoftEntityBehaviour
            ).Stop();

            // hide static & blank camer
            apollon.gameplay.ApollonGameplayManager.Instance.setInactive(apollon.gameplay.ApollonGameplayManager.GameplayIDType.WorldElement);
            UnityEngine.Camera.main.clearFlags = this._blankCameraFlags;
            UnityEngine.Camera.main.backgroundColor = this._blankCameraBackgroundColor;

            // show red cross
            apollon.frontend.ApollonFrontendManager.Instance.setActive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

            // wait a certain amout of time
            System.Diagnostics.Stopwatch chrono_0 = new System.Diagnostics.Stopwatch();
            chrono_0.Start();
            while (chrono_0.ElapsedMilliseconds < this._settings.phase_timeout_E0_latency)
            {
                await System.Threading.Tasks.Task.Delay(5);
            }
            chrono_0.Stop();

            // hide green cross
            apollon.frontend.ApollonFrontendManager.Instance.setInactive(apollon.frontend.ApollonFrontendManager.FrontendIDType.RedCrossGUI);

            // inactivate moving gameplay element
            apollon.gameplay.ApollonGameplayManager.Instance.setInactive(apollon.gameplay.ApollonGameplayManager.GameplayIDType.SimulatedRobosoftEntity);
            apollon.gameplay.ApollonGameplayManager.Instance.setInactive(apollon.gameplay.ApollonGameplayManager.GameplayIDType.RealRobosoftEntity);

            // wait a certain amout of time
            System.Diagnostics.Stopwatch chrono_1 = new System.Diagnostics.Stopwatch();
            chrono_1.Start();
            while (chrono_0.ElapsedMilliseconds < this._settings.phase_timeout_E1_latency)
            {
                await System.Threading.Tasks.Task.Delay(5);
            }
            chrono_1.Stop();

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndTBWProfile.DoPhaseE() : end"
            );

            // then ask response
            this.DoPromptUserResponse();

        } /* DoPhaseE() */

        #endregion

        #region abstract implementation

        protected override System.String getCurrentStatusInfo()
        {

            return "[" + apollon.ApollonEngine.GetEnumDescription(this.ID) + "] : no active status";

        } /* getCurrentStatusInfo() */

        protected override System.String getCurrentCounterStatusInfo()
        {

            return "";

        } /* getCurrentCounterStatusInfo() */

        public override void onUpdate(object sender, apollon.ApollonEngine.EngineEventArgs arg)
        {

            // base call
            base.onUpdate(sender, arg);

        } /* onUpdate() */

        public override void onExperimentSessionBegin(object sender, apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {

            // base call
            base.onExperimentSessionBegin(sender, arg);

        } /* onExperimentSessionBegin() */

        public override void onExperimentSessionEnd(object sender, apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {

            // base call
            base.onExperimentSessionEnd(sender, arg);

        } /* onExperimentSessionEnd() */

        public override void onExperimentTrialBegin(object sender, apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {
            // local
            int currentIdx = apollon.experiment.ApollonExperimentManager.Instance.Session.currentTrialNum - 1;

            // extract settings
            this._settings.bIsActive                        = apollon.experiment.ApollonExperimentManager.Instance.Session.settings.GetBoolList("active_mode")[currentIdx];
            this._settings.upstream_latency_offset          = apollon.experiment.ApollonExperimentManager.Instance.Session.settings.GetFloat("upstream_latency_offset");
            this._settings.acceleration_offset              = apollon.experiment.ApollonExperimentManager.Instance.Session.settings.GetFloat("acceleration_offset");
            this._settings.speed_initial                    = apollon.experiment.ApollonExperimentManager.Instance.Session.settings.GetFloat("speed_initial");
            this._settings.speed_saturation                 = apollon.experiment.ApollonExperimentManager.Instance.Session.settings.GetFloat("speed_saturation");
            this._settings.acceleration_pattern_very_low    = apollon.experiment.ApollonExperimentManager.Instance.Session.settings.GetFloatList("acceleration_pattern_very_low")[currentIdx];
            this._settings.acceleration_pattern_low         = apollon.experiment.ApollonExperimentManager.Instance.Session.settings.GetFloatList("acceleration_pattern_low")[currentIdx];
            this._settings.acceleration_pattern_high        = apollon.experiment.ApollonExperimentManager.Instance.Session.settings.GetFloatList("acceleration_pattern_high")[currentIdx];
            this._settings.acceleration_pattern_very_high   = apollon.experiment.ApollonExperimentManager.Instance.Session.settings.GetFloatList("acceleration_pattern_very_high")[currentIdx];
            this._settings.phase_timeout_A_latency          = apollon.experiment.ApollonExperimentManager.Instance.Session.settings.GetFloatList("phase_timeout_A_latency")[currentIdx];
            this._settings.phase_timeout_B0_latency         = apollon.experiment.ApollonExperimentManager.Instance.Session.settings.GetFloatList("phase_timeout_B0_latency")[currentIdx];
            this._settings.phase_timeout_B1_latency         = apollon.experiment.ApollonExperimentManager.Instance.Session.settings.GetFloatList("phase_timeout_B1_latency")[currentIdx];
            this._settings.phase_timeout_B2_latency         = apollon.experiment.ApollonExperimentManager.Instance.Session.settings.GetFloatList("phase_timeout_B2_latency")[currentIdx];
            this._settings.phase_timeout_C_SOA              = apollon.experiment.ApollonExperimentManager.Instance.Session.settings.GetFloatList("phase_timeout_C_SOA")[currentIdx];
            this._settings.phase_timeout_D_latency          = apollon.experiment.ApollonExperimentManager.Instance.Session.settings.GetFloatList("phase_timeout_D_latency")[currentIdx];
            this._settings.phase_timeout_E0_latency         = apollon.experiment.ApollonExperimentManager.Instance.Session.settings.GetFloatList("phase_timeout_E0_latency")[currentIdx];
            this._settings.phase_timeout_E1_latency         = apollon.experiment.ApollonExperimentManager.Instance.Session.settings.GetFloatList("phase_timeout_E1_latency")[currentIdx];
            this._settings.phase_timeout_E2_latency         = apollon.experiment.ApollonExperimentManager.Instance.Session.settings.GetFloatList("phase_timeout_E2_latency")[currentIdx];
            this._settings.acceleration_pattern_sequence    = apollon.experiment.ApollonExperimentManager.Instance.Session.settings.GetStringList("acceleration_pattern_sequence")[currentIdx];

            // log the
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> AgencyAndTBWProfile.onExperimentTrialBegin() : found current settings trial..."
                + "\n - bIsActive : "                       + this._settings.bIsActive                 
                + "\n - upstream_latency_offset : "         + this._settings.upstream_latency_offset.ToString()
                + "\n - acceleration_offset : "             + this._settings.acceleration_offset
                + "\n - speed_initial : "                   + this._settings.speed_initial
                + "\n - speed_saturation : "                + this._settings.speed_saturation           
                + "\n - acceleration_pattern_very_low : "   + this._settings.acceleration_pattern_very_low
                + "\n - acceleration_pattern_low : "        + this._settings.acceleration_pattern_low      
                + "\n - acceleration_pattern_high : "       + this._settings.acceleration_pattern_high
                + "\n - acceleration_pattern_very_high : "  + this._settings.acceleration_pattern_very_high
                + "\n - phase_timeout_A_latency : "         + this._settings.phase_timeout_A_latency
                + "\n - phase_timeout_B0_latency : "        + this._settings.phase_timeout_B0_latency
                + "\n - phase_timeout_B1_latency : "        + this._settings.phase_timeout_B1_latency
                + "\n - phase_timeout_B2_latency : "        + this._settings.phase_timeout_B2_latency
                + "\n - phase_timeout_C_SOA : "             + this._settings.phase_timeout_C_SOA
                + "\n - phase_timeout_D_latency : "         + this._settings.phase_timeout_D_latency
                + "\n - phase_timeout_E0_latency : "        + this._settings.phase_timeout_E0_latency
                + "\n - phase_timeout_E1_latency : "        + this._settings.phase_timeout_E1_latency
                + "\n - phase_timeout_E2_latency : "        + this._settings.phase_timeout_E2_latency        
                + "\n - acceleration_pattern_sequence : "   + this._settings.acceleration_pattern_sequence 
            );

            // base call
            base.onExperimentTrialBegin(sender, arg);

            // initial
            this.DoPhaseA();

        } /* onExperimentTrialBegin() */

        public override void onExperimentTrialEnd(object sender, apollon.ApollonEngine.EngineExperimentEventArgs arg)
        {
            // // stop audio recording & save it
            // UnityEngine.Microphone.End(UnityEngine.Microphone.devices[0]);
            // common.ApollonWavRecorder recorder = new common.ApollonWavRecorder();
            // recorder.Save(
            //     ApollonExperimentManager.Instance.Session.FullPath
            //     + string.Format(
            //         "/{0}_{1}_T{2:000}.wav", 
            //         "audioClip", 
            //         "HTCViveMic", 
            //         ApollonExperimentManager.Instance.Session.currentTrialNum
            //     ),
            //     this._results.user_clip
            // );

            // base call
            base.onExperimentTrialEnd(sender, arg);

        } /* onExperimentTrialEnd() */

        #endregion

    } /* class AgencyAndTBWProfile */

} /* } Labsim.experiment.AgencyAndTBW */
