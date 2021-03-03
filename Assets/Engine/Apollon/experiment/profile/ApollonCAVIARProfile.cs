using UXF;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

// avoid namespace pollution
namespace Labsim.apollon.experiment.profile
{

    public class ApollonCAVIARProfile 
        : ApollonAbstractExperimentFiniteStateMachine< ApollonCAVIARProfile >
    {

        // Ctor
        public ApollonCAVIARProfile()
            : base()
        {
            // default profile
            this.m_profileID = ApollonExperimentManager.ProfileIDType.CAVIAR;
        }

        #region settings/result
        
        public class Settings
        {
            public enum VisualCueIDType
            {

                [System.ComponentModel.Description("Undefined")]
                Undefined = 0,

                [System.ComponentModel.Description("Controle")]
                Controle,

                [System.ComponentModel.Description("Grille")]
                Grille,

                [System.ComponentModel.Description("Objet3D")]
                Objet3D,

                [System.ComponentModel.Description("Objet2D")]
                Objet2D,

            } /* enum */
                
            public class PhaseCSettings 
            {
                
                public VisualCueIDType 
                    // le type d'aide visuelle {string}
                    visual_cue_type;

                public float 
                    // la distance totale de la phase {meter}
                    total_distance,
                    // la vitesse consigne en entree de phase {m.s-1}
                    target_velocity,
                    // la distance d'apparition du stim relativement au la distance de debut de la phase {meter}
                    stim_begin_distance,
                    // l'acceleration du stim {m.s-2}
                    stim_acceleration,
                    // la vitesse cible du stim (plateau) {m.s-1}
                    stim_velocity,
                    // la distance de debut du brouillard {meter}
                    fog_start_distance,
                    // la distance de fin du brouillard (opacite totale) {meter}
                    fog_end_distance;

            } /* class PhaseCSettings */

            public float
                // distance de la phase B pour la mise en mouvement de l'apareil {meter}
                phase_B_distance,
                // dstance de la phase D pour la transition entre les sous-phases C {meter}
                phase_D_distance,
                // distance de la phase E pour l'arret complet de l'apareil {meter}
                phase_E_distance,
                // duree de repos avec la croix de fixation noire {millisecond}
                phase_F_duration;

            public System.Collections.Generic.List<PhaseCSettings> phase_C_settings = new System.Collections.Generic.List<PhaseCSettings>();

        } /* class Settings */

        public class Results
        {
            public class PhaseCResults 
            {
                public bool
                    user_response;

                public float
                    // la distance de debut du stim relativement au debut de de phase
                    user_stim_distance,
                    // le temps de debut de stim (ref. Unity) {millisecond}
                    user_stim_unity_timestamp,
                    // la distance de detection du stim relativement au debut de de phase
                    user_perception_distance,
                    // le temps de perception (ref. Unity) {millisecond}
                    user_perception_unity_timestamp;

                public string
                    // le temps de debut de stim (ref. Host) {string}
                    user_stim_host_timestamp,
                    // le temps de perception (ref. Host) {string}
                    user_perception_host_timestamp;

                public UnityEngine.AudioClip
                    user_clip;

            } /* class PhaseCResults */

            public System.Collections.Generic.List<PhaseCResults> phase_C_results = new System.Collections.Generic.List<PhaseCResults>();

        } /* class Results */

        // properties
        public Settings CurrentSettings { get; } = new Settings();
        public Results CurrentResults { get; set; } = new Results();
        private static readonly ushort InternalPhaseLoopCount = 4;

        #endregion

        #region abstract implementation

        public override void onExperimentSessionBegin(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARProfile.onExperimentSessionBegin() : begin"
            );

            // activate current database
            var db_str = arg.Session.settings.GetString("database_name");
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARProfile.onExperimentSessionBegin() : found settings database name ["
                    + db_str
                + "], try finding the only associated game object tag"
            );
            
            var we_behaviour
                 = gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.WorldElement
                ).Behaviour as gameplay.element.ApollonWorldElementBehaviour;
            if(we_behaviour.References[db_str] != null) 
            {
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIARProfile.onExperimentSessionBegin() : found game object, activating"
                );

                we_behaviour.References[db_str].SetActive(true);

            } 
            else 
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> ApollonCAVIARProfile.onExperimentSessionBegin() : could not find requested game object by name, error..."
                );

            } /* if() */

            // base call
            base.onExperimentSessionBegin(sender, arg);

            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARProfile.onExperimentSessionBegin() : end"
            );

        } /* onExperimentSessionBegin() */

        public override async void onExperimentTrialBegin(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARProfile.onExperimentTrialBegin() : begin"
            );
            
            //// activate audio recording if any available
            //if (UnityEngine.Microphone.devices.Length != 0)
            //{
            //    // use default device
            //    this.CurrentResults.user_clip
            //    = UnityEngine.Microphone.Start(
            //        null,
            //        true,
            //        45,
            //        44100
            //    );
            //}

            //// log
            //UnityEngine.Debug.Log(
            //    "<color=Blue>Info: </color> ApollonCAVIARProfile.onExperimentTrialBegin() : audio recording started"
            //);

            // temporary string
            string log = "";

            // extract current settings
            this.CurrentSettings.phase_B_distance = arg.Trial.settings.GetFloat("phase_B_distance_meter");
            this.CurrentSettings.phase_D_distance = arg.Trial.settings.GetFloat("phase_D_distance_meter");
            this.CurrentSettings.phase_E_distance = arg.Trial.settings.GetFloat("phase_E_distance_meter");
            this.CurrentSettings.phase_F_duration = arg.Trial.settings.GetFloat("phase_F_duration_ms");

            // log
            log +="\n - phase_B_distance : " + this.CurrentSettings.phase_B_distance 
                + "\n - phase_D_distance : " + this.CurrentSettings.phase_D_distance 
                + "\n - phase_E_distance : " + this.CurrentSettings.phase_E_distance 
                + "\n - phase_F_duration : " + this.CurrentSettings.phase_F_duration;

            // instantiate loopable phase ([C -> D] -> [C -> D] -> ...) 
            for(ushort idx = 0; idx < ApollonCAVIARProfile.InternalPhaseLoopCount; ++idx) 
            {

                // get current visual cue identifier
                var current_cue = Settings.VisualCueIDType.Undefined;
                switch (arg.Trial.settings.GetString("phase_C" + idx + "_visual_cue_type_string"))
                {

                    // projected grid
                    case string param when param.Equals(
                        ApollonEngine.GetEnumDescription(Settings.VisualCueIDType.Grille), 
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        current_cue = Settings.VisualCueIDType.Grille;
                        break;
                    }

                    // 3D object
                    case string param when param.Equals(
                        ApollonEngine.GetEnumDescription(Settings.VisualCueIDType.Objet3D), 
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        current_cue = Settings.VisualCueIDType.Objet3D;
                        break;
                    }

                    // 2D object
                    case string param when param.Equals(
                        ApollonEngine.GetEnumDescription(Settings.VisualCueIDType.Objet2D), 
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        current_cue = Settings.VisualCueIDType.Objet2D;
                        break;
                    }

                    // Controle
                    case string param when param.Equals(
                        ApollonEngine.GetEnumDescription(Settings.VisualCueIDType.Controle), 
                        System.StringComparison.InvariantCultureIgnoreCase
                    ) : {
                        current_cue = Settings.VisualCueIDType.Controle;
                        break;
                    }

                    default:
                    {
                        // log error
                        UnityEngine.Debug.LogError(
                            "<color=Red>Error: </color> ApollonCAVIARProfile.onExperimentTrialBegin() : found invalid string value["
                            + arg.Trial.settings.GetString("phase_C" + idx + "_visual_cue_type_string")
                            + "] for setting["
                            + "phase_C" + idx + "_visual_cue_type_string"
                            + "]"
                        );
                        break;
                    }

                } /* switch() */

                // instantiate settings & result
                this.CurrentSettings.phase_C_settings.Add(
                    new Settings.PhaseCSettings() {
                        visual_cue_type     = current_cue,
                        total_distance      = arg.Trial.settings.GetFloat("phase_C" + idx + "_total_distance_meter"),
                        target_velocity     = arg.Trial.settings.GetFloat("phase_C" + idx + "_target_velocity_meter_per_s"),
                        stim_begin_distance = arg.Trial.settings.GetFloat("phase_C" + idx + "_stim_begin_distance_meter"),
                        stim_acceleration   = arg.Trial.settings.GetFloat("phase_C" + idx + "_stim_acceleration_meter_per_s2"),
                        stim_velocity       = arg.Trial.settings.GetFloat("phase_C" + idx + "_stim_velocity_meter_per_s"),
                        fog_start_distance  = arg.Trial.settings.GetFloat("phase_C" + idx + "_fog_start_distance_meter"),
                        fog_end_distance    = arg.Trial.settings.GetFloat("phase_C" + idx + "_fog_end_distance_meter")
                    }
                );
                this.CurrentResults.phase_C_results.Add(new Results.PhaseCResults());

                // log
                log += "\n - [C" + idx + "] visual_cue_type : "     + ApollonEngine.GetEnumDescription(this.CurrentSettings.phase_C_settings[idx].visual_cue_type)
                    + "\n - [C" + idx + "] total_distance : "       + this.CurrentSettings.phase_C_settings[idx].total_distance 
                    + "\n - [C" + idx + "] target_velocity : "      + this.CurrentSettings.phase_C_settings[idx].target_velocity 
                    + "\n - [C" + idx + "] stim_begin_distance : "  + this.CurrentSettings.phase_C_settings[idx].stim_begin_distance 
                    + "\n - [C" + idx + "] stim_acceleration : "    + this.CurrentSettings.phase_C_settings[idx].stim_acceleration 
                    + "\n - [C" + idx + "] stim_velocity : "        + this.CurrentSettings.phase_C_settings[idx].stim_velocity
                    + "\n - [C" + idx + "] fog_start_distance : "   + this.CurrentSettings.phase_C_settings[idx].fog_start_distance
                    + "\n - [C" + idx + "] fog_end_distance : "     + this.CurrentSettings.phase_C_settings[idx].fog_end_distance;

            } /* for() */
            
            // log the final result
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARProfile.onExperimentTrialBegin() : found current settings "
                + log
            );

            // write the randomized pattern as result for convenience
            arg.Trial.result["pattern"] = arg.Trial.settings.GetString("current_pattern");

            // inactivate gameplay & frontend
            gameplay.ApollonGameplayManager.Instance.setInactive(gameplay.ApollonGameplayManager.GameplayIDType.All);
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.All);
           
            // activate world, CAVIAR entity, Radiosonde sensor, HOTAS Trotthle
            gameplay.ApollonGameplayManager.Instance.setActive(gameplay.ApollonGameplayManager.GameplayIDType.WorldElement);
            gameplay.ApollonGameplayManager.Instance.setActive(gameplay.ApollonGameplayManager.GameplayIDType.CAVIAREntity);
            gameplay.ApollonGameplayManager.Instance.setActive(gameplay.ApollonGameplayManager.GameplayIDType.RadioSondeSensor);
            gameplay.ApollonGameplayManager.Instance.setActive(gameplay.ApollonGameplayManager.GameplayIDType.HOTASWarthogThrottleSensor);
            gameplay.ApollonGameplayManager.Instance.setActive(gameplay.ApollonGameplayManager.GameplayIDType.FogElement);
            
            // base call
            base.onExperimentTrialBegin(sender, arg);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARProfile.onExperimentTrialBegin() : end"
            );

            // build protocol
            await this.DoRunProtocol(
                async () => { await this.SetState( new phase.ApollonCAVIARPhaseA(this       ) ); },
                async () => { await this.SetState( new phase.ApollonCAVIARPhaseB(this       ) ); },
                async () => { await this.SetState( new phase.ApollonCAVIARPhaseC(this, 0    ) ); },
                async () => { await this.SetState( new phase.ApollonCAVIARPhaseD(this, 0, 1 ) ); },
                async () => { await this.SetState( new phase.ApollonCAVIARPhaseC(this, 1    ) ); },
                async () => { await this.SetState( new phase.ApollonCAVIARPhaseD(this, 1, 2 ) ); },
                async () => { await this.SetState( new phase.ApollonCAVIARPhaseC(this, 2    ) ); },
                async () => { await this.SetState( new phase.ApollonCAVIARPhaseD(this, 2, 3 ) ); },
                async () => { await this.SetState( new phase.ApollonCAVIARPhaseC(this, 3    ) ); },
                async () => { await this.SetState( new phase.ApollonCAVIARPhaseE(this       ) ); },
                async () => { await this.SetState( new phase.ApollonCAVIARPhaseF(this       ) ); }                
            );
            
        } /* onExperimentTrialBegin() */

        public override void onExperimentTrialEnd(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARProfile.onExperimentTrialEnd() : begin"
            );

            //// stop audio recording & save it, if any available...
            //if (UnityEngine.Microphone.devices.Length != 0)
            //{

            //    UnityEngine.Microphone.End(null);
            //    common.ApollonWavRecorder recorder = new common.ApollonWavRecorder();
            //    recorder.Save(
            //        ApollonExperimentManager.Instance.Session.FullPath
            //        + string.Format(
            //            "/{0}_{1}_T{2:000}.wav",
            //            "audioClip",
            //            "DefaultMicrophone",
            //            ApollonExperimentManager.Instance.Session.currentTrialNum
            //        ),
            //        this.CurrentResults.user_clip
            //    );

            //} /* if() */

            //// log
            //UnityEngine.Debug.Log(
            //    "<color=Blue>Info: </color> ApollonCAVIARProfile.onExperimentTrialEnd() : audio recording stopped, writing result"
            //);

            // write result
            for(ushort idx = 0; idx < ApollonCAVIARProfile.InternalPhaseLoopCount; ++idx) 
            {

                ApollonExperimentManager.Instance.Trial.result["C" + idx + "_user_response"] 
                    = this.CurrentResults.phase_C_results[idx].user_response;
                ApollonExperimentManager.Instance.Trial.result["C" + idx + "_user_stim_distance"] 
                    = this.CurrentResults.phase_C_results[idx].user_stim_distance;
                ApollonExperimentManager.Instance.Trial.result["C" + idx + "_user_stim_host_timestamp"] 
                    = this.CurrentResults.phase_C_results[idx].user_stim_host_timestamp;
                ApollonExperimentManager.Instance.Trial.result["C" + idx + "_user_stim_unity_timestamp"] 
                    = this.CurrentResults.phase_C_results[idx].user_stim_unity_timestamp;
                ApollonExperimentManager.Instance.Trial.result["C" + idx + "_user_perception_distance"] 
                    = this.CurrentResults.phase_C_results[idx].user_perception_distance;
                ApollonExperimentManager.Instance.Trial.result["C" + idx + "_user_perception_host_timestamp"] 
                    = this.CurrentResults.phase_C_results[idx].user_perception_host_timestamp;
                ApollonExperimentManager.Instance.Trial.result["C" + idx + "_user_perception_unity_timestamp"]
                    = this.CurrentResults.phase_C_results[idx].user_perception_unity_timestamp;

            } /* for() */

            // base call
            base.onExperimentTrialEnd(sender, arg);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARProfile.onExperimentTrialEnd() : end"
            );

        } /* onExperimentTrialEnd() */

        #endregion

    } /* class ApollonAgencyAndTBWExperimentProfile */

} /* } Labsim.apollon.experiment.profile */
