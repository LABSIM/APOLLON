using System.Linq;

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
                Control,

                [System.ComponentModel.Description("HUD_Radiosonde")]
                VCHUDRadiosonde,

                [System.ComponentModel.Description("Objet3D")]
                VC3D,

                [System.ComponentModel.Description("Objet3D_Tetrahedre")]
                VC3DTetrahedre,

                [System.ComponentModel.Description("Objet3D_Cube")]
                VC3DCube,

                [System.ComponentModel.Description("Objet2D")]
                VC2D,

                [System.ComponentModel.Description("Objet2D_Grille")]
                VC2DGrid,

                [System.ComponentModel.Description("Objet2D_Cercle")]
                VC2DCircle,

                [System.ComponentModel.Description("Objet2D_Carre")]
                VC2DSquare

            } /* enum */
                
            public class PhaseCSettings 
            {

                public System.Collections.Generic.List<VisualCueIDType>
                    // le type d'aide visuelle [string]
                    visual_cue_type = new System.Collections.Generic.List<VisualCueIDType>();

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
            public class PhaseAResults
            {

                #region timing_*

                public float 
                    timing_on_entry_unity_timestamp,
                    timing_on_exit_unity_timestamp;

                public string
                    timing_on_entry_host_timestamp,
                    timing_on_exit_host_timestamp;
                
                #endregion

            } /* class PhaseAResults */

            public class PhaseBResults
            {

                #region timing_*

                public float
                    timing_on_entry_unity_timestamp,
                    timing_on_exit_unity_timestamp;

                public string
                    timing_on_entry_host_timestamp,
                    timing_on_exit_host_timestamp;

                #endregion

            } /* class PhaseBResults */

            public class PhaseCResults 
            {

                #region timing_*

                public float
                    timing_on_entry_unity_timestamp,
                    timing_on_exit_unity_timestamp;

                public string
                    timing_on_entry_host_timestamp,
                    timing_on_exit_host_timestamp;

                #endregion

                #region user_*

                public bool
                    user_response;

                public float
                    // la distance de debut du stim relativement au debut de l'essai
                    user_stim_distance,
                    // le temps de debut de stim (ref. Unity) {millisecond}
                    user_stim_unity_timestamp;
                
                public System.Collections.Generic.List<float>
                    // la distance de detection du stim relativement au debut de l'essai
                    user_perception_distance = new System.Collections.Generic.List<float>(),
                    // le temps de perception (ref. Unity) {millisecond}
                    user_perception_unity_timestamp = new System.Collections.Generic.List<float>();

                public string
                    // le temps de debut de stim (ref. Host) {string}
                    user_stim_host_timestamp;
                
                public System.Collections.Generic.List<string>
                    // le temps de perception (ref. Host) {string}
                    user_perception_host_timestamp = new System.Collections.Generic.List<string>();

                public UnityEngine.AudioClip
                    user_clip;

                #endregion

            } /* class PhaseCResults */

            public class PhaseDResults
            {

                #region timing_*

                public float
                    timing_on_entry_unity_timestamp,
                    timing_on_exit_unity_timestamp;

                public string
                    timing_on_entry_host_timestamp,
                    timing_on_exit_host_timestamp;

                #endregion

                #region user_* 
                
                public bool
                    user_response;

                public System.Collections.Generic.List<float>
                    // la distance de detection du stim relativement au debut de l'essai
                    user_perception_distance = new System.Collections.Generic.List<float>(),
                    // le temps de perception (ref. Unity) {millisecond}
                    user_perception_unity_timestamp = new System.Collections.Generic.List<float>();
                
                public System.Collections.Generic.List<string>
                    // le temps de perception (ref. Host) {string}
                    user_perception_host_timestamp = new System.Collections.Generic.List<string>();

                #endregion

            } /* class PhaseDResults */

            public class PhaseEResults
            {

                #region timing_*

                public float
                    timing_on_entry_unity_timestamp,
                    timing_on_exit_unity_timestamp;

                public string
                    timing_on_entry_host_timestamp,
                    timing_on_exit_host_timestamp;

                #endregion

            } /* class PhaseEResults */

            public class PhaseFResults
            {

                #region timing_*

                public float
                    timing_on_entry_unity_timestamp,
                    timing_on_exit_unity_timestamp;

                public string
                    timing_on_entry_host_timestamp,
                    timing_on_exit_host_timestamp;

                #endregion

            } /* class PhaseFResults */

            public PhaseAResults phase_A_results = new PhaseAResults();
            public PhaseBResults phase_B_results = new PhaseBResults();
            public System.Collections.Generic.List<PhaseCResults> phase_C_results = new System.Collections.Generic.List<PhaseCResults>();
            public System.Collections.Generic.List<PhaseDResults> phase_D_results = new System.Collections.Generic.List<PhaseDResults>();
            public PhaseEResults phase_E_results = new PhaseEResults();
            public PhaseFResults phase_F_results = new PhaseFResults();

        } /* class Results */

        // properties
        public Settings CurrentSettings { get; } = new Settings();
        public Results CurrentResults { get; set; } = new Results();
        private static readonly ushort InternalPhaseLoopCount = 4;

        #endregion

        #region abstract implementation

        protected override System.String getCurrentStatusInfo()
        {

            return "[" + ApollonEngine.GetEnumDescription(this.ID) + "] : no active status";

        } /* getCurrentStatusInfo() */

        public async override void onExperimentSessionBegin(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARProfile.onExperimentSessionBegin() : begin"
            );

            // fade in
            await this.DoFadeIn(2500.0f, false);

            // inactivate all gameplay & frontend
            gameplay.ApollonGameplayManager.Instance.setInactive(gameplay.ApollonGameplayManager.GameplayIDType.All);
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.All);

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

            // clean arrays
            this.CurrentSettings.phase_C_settings.Clear();
            this.CurrentResults.phase_C_results.Clear();
            this.CurrentResults.phase_D_results.Clear();

            // instantiate loopable phase ([C -> D] -> [C -> D] -> ...) 
            for (ushort idx = 0; idx < ApollonCAVIARProfile.InternalPhaseLoopCount; ++idx) 
            {

                // get current visual cue identifier
                var cue_list = new System.Collections.Generic.List<Settings.VisualCueIDType>();
                foreach (var cue in arg.Trial.settings.GetStringList("phase_C" + idx + "_visual_cue_type_string"))
                {

                    switch (cue)
                    {

                        // 3D object - cube
                        case string param when param.Equals(
                            ApollonEngine.GetEnumDescription(Settings.VisualCueIDType.VC3DCube),
                            System.StringComparison.InvariantCultureIgnoreCase
                        ) : {
                            cue_list.Add(Settings.VisualCueIDType.VC3DCube);
                            break;
                        }

                        // 3D object - tetrahedre
                        case string param when param.Equals(
                            ApollonEngine.GetEnumDescription(Settings.VisualCueIDType.VC3DTetrahedre),
                            System.StringComparison.InvariantCultureIgnoreCase
                        ) : {
                            cue_list.Add(Settings.VisualCueIDType.VC3DTetrahedre);
                            break;
                        }

                        // 3D object - default
                        case string param when param.Equals(
                            ApollonEngine.GetEnumDescription(Settings.VisualCueIDType.VC3D),
                            System.StringComparison.InvariantCultureIgnoreCase
                        ) : {
                            cue_list.Add(Settings.VisualCueIDType.VC3D);
                            break;
                        }

                        // 2D object - grid
                        case string param when param.Equals(
                            ApollonEngine.GetEnumDescription(Settings.VisualCueIDType.VC2DGrid),
                            System.StringComparison.InvariantCultureIgnoreCase
                        ) : {
                            cue_list.Add(Settings.VisualCueIDType.VC2DGrid);
                            break;
                        }

                        // 2D object - circle
                        case string param when param.Equals(
                            ApollonEngine.GetEnumDescription(Settings.VisualCueIDType.VC2DCircle),
                            System.StringComparison.InvariantCultureIgnoreCase
                        ) : {
                            cue_list.Add(Settings.VisualCueIDType.VC2DCircle);
                            break;
                        }


                        // 2D object - square
                        case string param when param.Equals(
                            ApollonEngine.GetEnumDescription(Settings.VisualCueIDType.VC2DSquare),
                            System.StringComparison.InvariantCultureIgnoreCase
                        ) : {
                            cue_list.Add(Settings.VisualCueIDType.VC2DSquare);
                            break;
                        }

                        // 2D object - default
                        case string param when param.Equals(
                            ApollonEngine.GetEnumDescription(Settings.VisualCueIDType.VC2D),
                            System.StringComparison.InvariantCultureIgnoreCase
                        ) : {
                            cue_list.Add(Settings.VisualCueIDType.VC2D);
                            break;
                        }

                        // Controle
                        case string param when param.Equals(
                            ApollonEngine.GetEnumDescription(Settings.VisualCueIDType.Control),
                            System.StringComparison.InvariantCultureIgnoreCase
                        ) : {
                            cue_list.Add(Settings.VisualCueIDType.Control);
                            break;
                        }

                        // Controle
                        case string param when param.Equals(
                            ApollonEngine.GetEnumDescription(Settings.VisualCueIDType.VCHUDRadiosonde),
                            System.StringComparison.InvariantCultureIgnoreCase
                        ) : {
                            cue_list.Add(Settings.VisualCueIDType.VCHUDRadiosonde);
                            break;
                        }

                        default:
                            {
                                // log error
                                UnityEngine.Debug.LogError(
                                    "<color=Red>Error: </color> ApollonCAVIARProfile.onExperimentTrialBegin() : found invalid string value["
                                    + cue
                                    + "] for setting["
                                    + "phase_C" + idx + "_visual_cue_type_string"
                                    + "]"
                                );
                                break;
                            }

                    } /* switch() */

                } /* foreach() */

                // instantiate settings & result
                this.CurrentSettings.phase_C_settings.Add(
                    new Settings.PhaseCSettings() {
                        visual_cue_type     = cue_list,
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
                this.CurrentResults.phase_D_results.Add(new Results.PhaseDResults());

                // log
                log += "\n - [C" + idx + "] visual_cue_type : "     + string.Join(
                                                                        ",",  
                                                                        this.CurrentSettings.phase_C_settings[idx].visual_cue_type.ConvertAll( 
                                                                            new System.Converter<Settings.VisualCueIDType,string>(ApollonEngine.GetEnumDescription) 
                                                                        )
                                                                    )
                    + "\n - [C" + idx + "] total_distance : "       + this.CurrentSettings.phase_C_settings[idx].total_distance 
                    + "\n - [C" + idx + "] target_velocity : "      + this.CurrentSettings.phase_C_settings[idx].target_velocity 
                    + "\n - [C" + idx + "] stim_begin_distance : "  + this.CurrentSettings.phase_C_settings[idx].stim_begin_distance 
                    + "\n - [C" + idx + "] stim_acceleration : "    + this.CurrentSettings.phase_C_settings[idx].stim_acceleration 
                    + "\n - [C" + idx + "] stim_velocity : "        + this.CurrentSettings.phase_C_settings[idx].stim_velocity
                    + "\n - [C" + idx + "] fog_start_distance : "   + this.CurrentSettings.phase_C_settings[idx].fog_start_distance
                    + "\n - [C" + idx + "] fog_end_distance : "     + this.CurrentSettings.phase_C_settings[idx].fog_end_distance;

            } /* for() */

            // pop last phase_D_results item
            this.CurrentResults.phase_D_results.RemoveAt(this.CurrentResults.phase_D_results.Count - 1);
            
            // log the final result
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARProfile.onExperimentTrialBegin() : found current settings "
                + log
            );

            // write the randomized pattern as result for convenience
            arg.Trial.result["pattern"] = arg.Trial.settings.GetString("current_pattern");

            // activate world, CAVIAR entity, Radiosonde sensor, HOTAS Throttle
            gameplay.ApollonGameplayManager.Instance.setActive(gameplay.ApollonGameplayManager.GameplayIDType.WorldElement);
            gameplay.ApollonGameplayManager.Instance.setActive(gameplay.ApollonGameplayManager.GameplayIDType.FogElement);

            // activate current database
            var db_str = arg.Trial.settings.GetString("database_name");
            var db_origin_position = arg.Trial.settings.GetFloatList("database_origin_position");
            var db_origin_orientation = arg.Trial.settings.GetFloatList("database_origin_orientation");

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARProfile.onExperimentTrialBegin() : found settings database name ["
                    + db_str
                + "], try finding the only associated game object tag"
            );

            // get bridge
            var we_behaviour
                 = gameplay.ApollonGameplayManager.Instance.getBridge(
                    gameplay.ApollonGameplayManager.GameplayIDType.WorldElement
                ).Behaviour as gameplay.element.ApollonWorldElementBehaviour;

            // LINQ         
            foreach (var db_ref in we_behaviour.References.Where(kvp => kvp.Key.Contains("DBTag_")).Select(kvp => kvp.Value))
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIARProfile.onExperimentTrialBegin() : referenced gameObject[" + db_ref.name + "], inactivating"
                );

                // inactivate all first
                db_ref.SetActive(false);
            }

            // then activate only requested
            if (we_behaviour.References[db_str] != null)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonCAVIARProfile.onExperimentTrialBegin() : found game object, activating"
                );

                // mark as active
                we_behaviour.References[db_str].SetActive(true);

                // translate to our new origin
                we_behaviour.References[db_str].transform.Translate(
                    -1.0f * new UnityEngine.Vector3(
                        db_origin_position[0],
                        db_origin_position[1],
                        db_origin_position[2]
                    )
                );

                // apply rotation from our new world space origin
                we_behaviour.References[db_str].transform.Rotate(
                    new UnityEngine.Vector3(
                        db_origin_orientation[0],
                        db_origin_orientation[1],
                        db_origin_orientation[2]
                    ),
                    UnityEngine.Space.World
                );

            }
            else
            {

                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> ApollonCAVIARProfile.onExperimentTrialBegin() : could not find requested game object by name, error..."
                );

            } /* if() */

            // finally activate raycasting element
            gameplay.ApollonGameplayManager.Instance.setActive(gameplay.ApollonGameplayManager.GameplayIDType.CAVIAREntity);
            gameplay.ApollonGameplayManager.Instance.setActive(gameplay.ApollonGameplayManager.GameplayIDType.RadioSondeSensor);
            gameplay.ApollonGameplayManager.Instance.setActive(gameplay.ApollonGameplayManager.GameplayIDType.CAVIARControl);

            // base call
            base.onExperimentTrialBegin(sender, arg);

            // fade out
            await this.DoFadeOut(this._trial_fade_out_duration, false);

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
                async () => { await this.SetState( new phase.ApollonCAVIARPhaseF(this       ) ); },
                async () => { await this.SetState( null ); }
            );
            
        } /* onExperimentTrialBegin() */

        public override async void onExperimentTrialEnd(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonCAVIARProfile.onExperimentTrialEnd() : begin"
            );

            // write result

            // phase A
            ApollonExperimentManager.Instance.Trial.result["A_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_A_results.timing_on_entry_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["A_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_A_results.timing_on_exit_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["A_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_A_results.timing_on_entry_host_timestamp;
            ApollonExperimentManager.Instance.Trial.result["A_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_A_results.timing_on_exit_host_timestamp;

            // phase B
            ApollonExperimentManager.Instance.Trial.result["B_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_B_results.timing_on_entry_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["B_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_B_results.timing_on_exit_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["B_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_B_results.timing_on_entry_host_timestamp;
            ApollonExperimentManager.Instance.Trial.result["B_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_B_results.timing_on_exit_host_timestamp;

            // phase C
            for (ushort idx = 0; idx < ApollonCAVIARProfile.InternalPhaseLoopCount; ++idx) 
            {

                ApollonExperimentManager.Instance.Trial.result["C" + idx + "_timing_on_entry_unity_timestamp"]
                    = this.CurrentResults.phase_C_results[idx].timing_on_entry_unity_timestamp.ToString();
                ApollonExperimentManager.Instance.Trial.result["C" + idx + "_timing_on_exit_unity_timestamp"]
                    = this.CurrentResults.phase_C_results[idx].timing_on_exit_unity_timestamp.ToString();
                ApollonExperimentManager.Instance.Trial.result["C" + idx + "_timing_on_entry_host_timestamp"]
                    = this.CurrentResults.phase_C_results[idx].timing_on_entry_host_timestamp;
                ApollonExperimentManager.Instance.Trial.result["C" + idx + "_timing_on_exit_host_timestamp"]
                    = this.CurrentResults.phase_C_results[idx].timing_on_exit_host_timestamp;
                ApollonExperimentManager.Instance.Trial.result["C" + idx + "_user_response"] 
                    = this.CurrentResults.phase_C_results[idx].user_response.ToString();
                ApollonExperimentManager.Instance.Trial.result["C" + idx + "_user_stim_distance"] 
                    = this.CurrentResults.phase_C_results[idx].user_stim_distance.ToString();
                ApollonExperimentManager.Instance.Trial.result["C" + idx + "_user_stim_host_timestamp"] 
                    = this.CurrentResults.phase_C_results[idx].user_stim_host_timestamp;
                ApollonExperimentManager.Instance.Trial.result["C" + idx + "_user_stim_unity_timestamp"] 
                    = this.CurrentResults.phase_C_results[idx].user_stim_unity_timestamp.ToString();
                ApollonExperimentManager.Instance.Trial.result["C" + idx + "_user_perception_distance"] 
                    = string.Join(
                        ";",  
                        this.CurrentResults.phase_C_results[idx].user_perception_distance
                    );
                ApollonExperimentManager.Instance.Trial.result["C" + idx + "_user_perception_host_timestamp"] 
                    = string.Join(
                        ";",  
                        this.CurrentResults.phase_C_results[idx].user_perception_host_timestamp
                    );
                ApollonExperimentManager.Instance.Trial.result["C" + idx + "_user_perception_unity_timestamp"]
                    = string.Join(
                        ";",  
                        this.CurrentResults.phase_C_results[idx].user_perception_unity_timestamp
                    );

            } /* for() */

            // phase D
            for (ushort idx = 0; idx < ApollonCAVIARProfile.InternalPhaseLoopCount -1; ++idx)
            {

                ApollonExperimentManager.Instance.Trial.result["D" + idx + (idx + 1) + "_timing_on_entry_unity_timestamp"]
                    = this.CurrentResults.phase_D_results[idx].timing_on_entry_unity_timestamp.ToString();
                ApollonExperimentManager.Instance.Trial.result["D" + idx + (idx + 1) + "_timing_on_exit_unity_timestamp"]
                    = this.CurrentResults.phase_D_results[idx].timing_on_exit_unity_timestamp.ToString();
                ApollonExperimentManager.Instance.Trial.result["D" + idx + (idx + 1) + "_timing_on_entry_host_timestamp"]
                    = this.CurrentResults.phase_D_results[idx].timing_on_entry_host_timestamp;
                ApollonExperimentManager.Instance.Trial.result["D" + idx + (idx + 1) + "_timing_on_exit_host_timestamp"]
                    = this.CurrentResults.phase_D_results[idx].timing_on_exit_host_timestamp;
                ApollonExperimentManager.Instance.Trial.result["D" + idx + (idx + 1) + "_user_response"] 
                    = this.CurrentResults.phase_D_results[idx].user_response.ToString();
                ApollonExperimentManager.Instance.Trial.result["D" + idx + (idx + 1) + "_user_perception_distance"] 
                    = string.Join(
                        ";",  
                        this.CurrentResults.phase_D_results[idx].user_perception_distance
                    );
                ApollonExperimentManager.Instance.Trial.result["D" + idx + (idx + 1) + "_user_perception_host_timestamp"] 
                    = string.Join(
                        ";",  
                        this.CurrentResults.phase_D_results[idx].user_perception_host_timestamp
                    );
                ApollonExperimentManager.Instance.Trial.result["D" + idx + (idx + 1) + "_user_perception_unity_timestamp"]
                    = string.Join(
                        ";",  
                        this.CurrentResults.phase_D_results[idx].user_perception_unity_timestamp
                    );

            } /* for() */

            // phase E
            ApollonExperimentManager.Instance.Trial.result["E_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_E_results.timing_on_entry_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["E_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_E_results.timing_on_exit_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["E_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_E_results.timing_on_entry_host_timestamp;
            ApollonExperimentManager.Instance.Trial.result["E_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_E_results.timing_on_exit_host_timestamp;

            // phase F
            ApollonExperimentManager.Instance.Trial.result["F_timing_on_entry_unity_timestamp"]
                = this.CurrentResults.phase_F_results.timing_on_entry_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["F_timing_on_exit_unity_timestamp"]
                = this.CurrentResults.phase_F_results.timing_on_exit_unity_timestamp.ToString();
            ApollonExperimentManager.Instance.Trial.result["F_timing_on_entry_host_timestamp"]
                = this.CurrentResults.phase_F_results.timing_on_entry_host_timestamp;
            ApollonExperimentManager.Instance.Trial.result["F_timing_on_exit_host_timestamp"]
                = this.CurrentResults.phase_F_results.timing_on_exit_host_timestamp;

            // fade in
            await this.DoFadeIn(this._trial_fade_in_duration, false);

            // inactivate gameplay & frontend
            gameplay.ApollonGameplayManager.Instance.setInactive(gameplay.ApollonGameplayManager.GameplayIDType.All);
            frontend.ApollonFrontendManager.Instance.setInactive(frontend.ApollonFrontendManager.FrontendIDType.All);
           
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
