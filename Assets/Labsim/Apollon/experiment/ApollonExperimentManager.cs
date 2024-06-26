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

// extensions
using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.experiment
{

    public sealed class ApollonExperimentManager : ApollonAbstractManager
    {

        #region experiment bridge handling 

        private readonly System.Collections.Generic.Dictionary<ProfileIDType, bool> _experimentState
            = new System.Collections.Generic.Dictionary<ProfileIDType, bool>();

        private readonly System.Collections.Generic.Dictionary<string, ApollonAbstractExperimentProfile> _experimentTable
            = new System.Collections.Generic.Dictionary<string, ApollonAbstractExperimentProfile>();

        private void RegisterAllAvailableExperimentProfile()
        {

            // register all profile
            foreach (System.Type type
                in System.Reflection.Assembly.GetAssembly(typeof(ApollonAbstractExperimentProfile)).GetTypes()
                .Where(
                    myType => myType.IsClass 
                    && !myType.IsAbstract
                    && !myType.IsGenericType
                    && myType.IsSubclassOf(typeof(ApollonAbstractExperimentProfile))
                )
            )
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=blue>Info: </color> ApollonExperimentManager.RegisterAllAvailableExperimentProfile() : found profile ["
                    + type.FullName
                    + "]."
                );


                // get "Instance" property
                ApollonAbstractExperimentProfile profile = null;
                if ((profile = System.Activator.CreateInstance(type) as ApollonAbstractExperimentProfile) != null)
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonExperimentManager.RegisterAllAvailableExperimentProfile() : success to create instance of profile ["
                        + type.FullName
                        + "]."
                    );

                    // register it

                    // check
                    if (this._experimentTable.ContainsKey(type.FullName))
                    {

                        UnityEngine.Debug.LogError(
                             "<color=red>Error: </color> ApollonExperimentManager.RegisterAllAvailableExperimentProfile("
                             + type.FullName
                             + ") : profile key already found ..."
                        );
                        return;

                    } /* if() */

                    // add to tables
                    this._experimentTable.Add(profile.ID.ToString(), profile);
                    this._experimentState.Add(profile.ID, false);

                    // bind event
                    this.ExperimentProfileActivationRequestedEvent += profile.OnExperimentProfileActivationRequested;
                    this.ExperimentProfileDeactivationRequestedEvent += profile.OnExperimentProfileDeactivationRequested;

                    // log
                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonExperimentManager.RegisterAllAvailableExperimentProfile() : profile ["
                        + type.FullName
                        + "], registration ok !"
                    );

                }
                else
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonExperimentManager.RegisterAllAvailableExperimentProfile() : could not create instance of gameobject ["
                        + type.FullName
                        + "]. fail."
                    );

                } /* if() */

            } /* foreach() */

        } /* RegisterAllAvailableExperimentProfile() */

        public ApollonAbstractExperimentProfile getBridge(ProfileIDType ID)
        {

            // check 
            if (this._experimentTable.ContainsKey(ID.ToString()))
            {
                return this._experimentTable[ID.ToString()];
            }

            // log
            UnityEngine.Debug.LogWarning(
                 "<color=orange>Warning: </color> ApollonExperimentManager.getBridge("
                 + ID.ToString()
                 + ") : requested ID not found ..."
            );

            // failed
            return null;

        } /* getBridge() */

        #endregion

        #region lazy init singleton pattern

        // lazy init paradigm
        private static readonly System.Lazy<ApollonExperimentManager> _lazyInstance
            = new System.Lazy<ApollonExperimentManager>(() => new ApollonExperimentManager());

        // Instance  property
        public static ApollonExperimentManager Instance { get { return _lazyInstance.Value; } }

        // private ctor
        private ApollonExperimentManager()
        {

            this._eventTable = new System.Collections.Generic.Dictionary<string, System.Delegate>
            {
                { "ExperimentProfileActivationRequested", null },
                { "ExperimentProfileDeactivationRequested", null }
            };

            // registering
            this.RegisterAllAvailableExperimentProfile();

        } /* ApollonExperimentManager() */

        #endregion

        #region event handling

        private readonly System.Collections.Generic.Dictionary<string, System.Delegate> _eventTable;

        private readonly System.Collections.Generic.List<System.EventHandler<EngineProfileEventArgs>>
            _eventExperimentProfileActivationRequestedList = new System.Collections.Generic.List<System.EventHandler<EngineProfileEventArgs>>(),
            _eventExperimentProfileDeactivationRequestedList = new System.Collections.Generic.List<System.EventHandler<EngineProfileEventArgs>>();

        public event System.EventHandler<EngineProfileEventArgs> ExperimentProfileActivationRequestedEvent
        {
            add
            {
                this._eventExperimentProfileActivationRequestedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["ExperimentProfileActivationRequested"] = (System.EventHandler<EngineProfileEventArgs>)this._eventTable["ExperimentProfileActivationRequested"] + value;
                }
            }

            remove
            {
                if (!this._eventExperimentProfileActivationRequestedList.Contains(value))
                {
                    return;
                }
                this._eventExperimentProfileActivationRequestedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["ExperimentProfileActivationRequested"] = null;
                    foreach (var eventExperimentProfileActivationRequested in this._eventExperimentProfileActivationRequestedList)
                    {
                        this._eventTable["ExperimentProfileActivationRequested"] = (System.EventHandler<EngineProfileEventArgs>)this._eventTable["ExperimentProfileActivationRequested"] + eventExperimentProfileActivationRequested;
                    }
                }
            }

        } /* ExperimentProfileActivationRequestedEvent */

        public event System.EventHandler<EngineProfileEventArgs> ExperimentProfileDeactivationRequestedEvent
        {
            add
            {
                this._eventExperimentProfileDeactivationRequestedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["ExperimentProfileDeactivationRequested"] = (System.EventHandler<EngineProfileEventArgs>)this._eventTable["ExperimentProfileDeactivationRequested"] + value;
                }
            }

            remove
            {
                if (!this._eventExperimentProfileDeactivationRequestedList.Contains(value))
                {
                    return;
                }
                this._eventExperimentProfileDeactivationRequestedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["ExperimentProfileDeactivationRequested"] = null;
                    foreach (var eventExperimentProfileDeactivationRequested in this._eventExperimentProfileDeactivationRequestedList)
                    {
                        this._eventTable["ExperimentProfileDeactivationRequested"] = (System.EventHandler<EngineProfileEventArgs>)this._eventTable["ExperimentProfileDeactivationRequested"] + eventExperimentProfileDeactivationRequested;
                    }
                }
            }

        } /* ExperimentProfileDeactivationRequestedEvent */

        internal void RaiseExperimentProfileActivationRequestedEvent(ProfileIDType profileID)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EngineProfileEventArgs>)this._eventTable["ExperimentProfileActivationRequested"];
                callback?.Invoke(this, new EngineProfileEventArgs(profileID));
            }
        }

        internal void RaiseExperimentProfileDeactivationRequestedEvent(ProfileIDType profileID)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EngineProfileEventArgs>)this._eventTable["ExperimentProfileDeactivationRequested"];
                callback?.Invoke(this, new EngineProfileEventArgs(profileID));
            }
        }
        public class EngineProfileEventArgs
            : ApollonEngine.EngineEventArgs
        {

            // ctor
            public EngineProfileEventArgs(ProfileIDType profileID)
                : base()
            {
                this.ProfileID = profileID;
            }

            // property
            public ProfileIDType ProfileID { get; private set; }

        } /* EngineProfileEventArgs */

        #endregion

        #region public members/method

        public enum ProfileIDType
        {
            [System.ComponentModel.Description("All")]
            None = 0,

            [System.ComponentModel.Description("AgencyAndTBW")]
            AgencyAndTBW,

            [System.ComponentModel.Description("AgencyAndThresholdPerception")]
            AgencyAndThresholdPerception,

            [System.ComponentModel.Description("AgencyAndThresholdPerceptionV2")]
            AgencyAndThresholdPerceptionV2,

            [System.ComponentModel.Description("AgencyAndThresholdPerceptionV3")]
            AgencyAndThresholdPerceptionV3,

            [System.ComponentModel.Description("AgencyAndThresholdPerceptionV4")]
            AgencyAndThresholdPerceptionV4,

            [System.ComponentModel.Description("CAVIAR")]
            CAVIAR,

            [System.ComponentModel.Description("AIRWISE")]
            AIRWISE,

            [System.ComponentModel.Description("LEXIKHUM-OAT")]
            LEXIKHUM_OAT

        } /* enum*/

        public bool IsActiveProfile(ProfileIDType profileID)
        {

            if (this._experimentState.ContainsKey(profileID))
            {
                return this._experimentState[profileID];
            }
            else
            {
                UnityEngine.Debug.LogWarning(
                    "<color=orange>Warning: </color> ApollonExperimentManager.IsActiveProfile("
                    + profileID
                    + ") : requested experimentID is not intantiated yet..."
                );
                return false;
            }

        } /* IsActiveProfile() */

        public ProfileIDType getActiveProfile()
        {

            return (
                this._experimentState
                    .Where(item => item.Value == true)
                    .Select(item => item.Key)
                    .First()
            );

        } /* getActiveProfile() */

        public void setActiveProfile(ProfileIDType profileID)
        {

            if (this._experimentState.ContainsKey(profileID))
            {
                
                if (this.IsActiveProfile(profileID))
                {
                    UnityEngine.Debug.LogWarning(
                        "<color=orange>Warning: </color> ApollonExperimentManager.setActiveProfile("
                        + profileID
                        + ") : requested profileID is already active, skip"
                    );
                    return;
                }

                if (this._experimentState.ContainsValue(true))
                {
                    UnityEngine.Debug.LogWarning(
                        "<color=orange>Warning: </color> ApollonExperimentManager.setActiveProfile("
                        + profileID
                        + ") : an other profile is already active, requesting deactivation first"
                    );

                    // raise unsubscripption
                    this.RaiseExperimentProfileDeactivationRequestedEvent(this.getActiveProfile());

                    // update state
                    this._experimentState[this.getActiveProfile()] = false; 

                } /* if() */

                // update state
                this._experimentState[profileID] = true; 

                // raise subscripption
                this.RaiseExperimentProfileActivationRequestedEvent(profileID);

            }
            else
            {
                UnityEngine.Debug.LogWarning(
                    "<color=orange>Warning: </color> ApollonExperimentManager.setActiveProfile("
                    + profileID
                    + ") : requested exprimentID is not intantiated yet..."
                );
                return;
            }

        } /* setActiveProfile() */

        #endregion

        #region properties

        private UXF.Session m_session = null;
        public UXF.Session Session 
        {
            get
            {
                return this.m_session;
            }
            private set
            {
                this.m_session = value;
            }
        }

        private UXF.Trial m_trial = null;
        public UXF.Trial Trial
        {
            get
            {
                return this.m_trial;
            }
            private set
            {
                this.m_trial = value;
            }
        }

        private ApollonAbstractExperimentProfile m_profile = null;
        public ApollonAbstractExperimentProfile Profile
        {
            get
            {
                return this.m_profile;
            }
            set
            {
                this.m_profile = value;
            }
        }

        #endregion

        #region abstract manager implementation

        public override void onStart(object sender, ApollonEngine.EngineEventArgs arg)
        {
        }
     
        public override void onExperimentSessionBegin(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {

            // keep session alive
            this.Session = arg.Session;

            // switch to active profile
            switch(this.Session.settings.GetString("APOLLON_profile"))
            {

                // Agency And TBW
                case string param when param.Equals(
                    ApollonEngine.GetEnumDescription(ProfileIDType.AgencyAndTBW), 
                    System.StringComparison.InvariantCultureIgnoreCase
                ) : {

                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonExperimentManager.onExperimentSessionBegin() : found APOLLON_profile setting value [AgencyAndTBW]"
                    );
                    this.setActiveProfile(ProfileIDType.AgencyAndTBW);
                    break;
                }

                // AgencyAndThresholdPerception
                case string param when param.Equals(
                    ApollonEngine.GetEnumDescription(ProfileIDType.AgencyAndThresholdPerception), 
                    System.StringComparison.InvariantCultureIgnoreCase
                ) : {
                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonExperimentManager.onExperimentSessionBegin() : found APOLLON_profile setting value [AgencyAndThresholdPerception]"
                    );
                    this.setActiveProfile(ProfileIDType.AgencyAndThresholdPerception);
                    break;
                }

                // AgencyAndThresholdPerception V2
                case string param when param.Equals(
                    ApollonEngine.GetEnumDescription(ProfileIDType.AgencyAndThresholdPerceptionV2), 
                    System.StringComparison.InvariantCultureIgnoreCase
                ) : {
                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonExperimentManager.onExperimentSessionBegin() : found APOLLON_profile setting value [AgencyAndThresholdPerceptionV2]"
                    );
                    this.setActiveProfile(ProfileIDType.AgencyAndThresholdPerceptionV2);
                    break;
                }

                // AgencyAndThresholdPerception V3
                case string param when param.Equals(
                    ApollonEngine.GetEnumDescription(ProfileIDType.AgencyAndThresholdPerceptionV3), 
                    System.StringComparison.InvariantCultureIgnoreCase
                ) : {
                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonExperimentManager.onExperimentSessionBegin() : found APOLLON_profile setting value [AgencyAndThresholdPerceptionV3]"
                    );
                    this.setActiveProfile(ProfileIDType.AgencyAndThresholdPerceptionV3);
                    break;
                }

                // AgencyAndThresholdPerception V4
                case string param when param.Equals(
                    ApollonEngine.GetEnumDescription(ProfileIDType.AgencyAndThresholdPerceptionV4), 
                    System.StringComparison.InvariantCultureIgnoreCase
                ) : {
                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonExperimentManager.onExperimentSessionBegin() : found APOLLON_profile setting value [AgencyAndThresholdPerceptionV4]"
                    );
                    this.setActiveProfile(ProfileIDType.AgencyAndThresholdPerceptionV4);
                    break;
                }

                // CAVIAR
                case string param when param.Equals(
                    ApollonEngine.GetEnumDescription(ProfileIDType.CAVIAR), 
                    System.StringComparison.InvariantCultureIgnoreCase
                ) : {

                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonExperimentManager.onExperimentSessionBegin() : found APOLLON_profile setting value [CAVIAR]"
                    );
                    this.setActiveProfile(ProfileIDType.CAVIAR);
                    break;
                }

                // AIRWISE
                case string param when param.Equals(
                    ApollonEngine.GetEnumDescription(ProfileIDType.AIRWISE), 
                    System.StringComparison.InvariantCultureIgnoreCase
                ) : {

                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonExperimentManager.onExperimentSessionBegin() : found APOLLON_profile setting value [AIRWISE]"
                    );
                    this.setActiveProfile(ProfileIDType.AIRWISE);
                    break;
                }

                // AIRWISE
                case string param when param.Equals(
                    ApollonEngine.GetEnumDescription(ProfileIDType.LEXIKHUM_OAT), 
                    System.StringComparison.InvariantCultureIgnoreCase
                ) : {

                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonExperimentManager.onExperimentSessionBegin() : found APOLLON_profile setting value [LEXIKHUM-OAT]"
                    );
                    this.setActiveProfile(ProfileIDType.LEXIKHUM_OAT);
                    break;
                }

                default:
                {
                    UnityEngine.Debug.LogError(
                        "<color=red>Error: </color> ApollonExperimentManager.onExperimentSessionBegin() : could not determine APOLLON_profile setting value... check configuration file"
                    );
                    return;
                }

            } /* switch() */

            // call
            this.Profile.OnExperimentSessionBegin(sender, arg);
            
        } /* onExperimentSessionBegin() */

        public override void onExperimentSessionEnd(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {

            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonExperimentManager.onExperimentSessionEnd() : call"
            );

            // remove ref
            //this.Session = null;

        }

        public override void onExperimentTrialBegin(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {

            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonExperimentManager.onExperimentTrialBegin() : call"
            );

            this.Trial = arg.Trial;

        } /* onExperimentTrialBegin() */

        public override void onExperimentTrialEnd(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {

            UnityEngine.Debug.Log(
                "<color=blue>Info: </color> ApollonExperimentManager.onExperimentTrialEnd() : call"
            );

            // remove ref
            //this.Trial = null;

        }

        #endregion

    } /* class ApollonExperimentManager */

} /* namespace Labsim.apollon.experiment */
