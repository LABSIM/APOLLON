// extensions
using System.Linq;
using Labsim.apollon;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public sealed class TactileTrackerManager 
        : apollon.ApollonAbstractManager
    {

        #region tracker handling

        private readonly System.Collections.Generic.Dictionary<IDType, TactileAbstractTracker> _trackerTable
            = new System.Collections.Generic.Dictionary<IDType, TactileAbstractTracker>();

        private void RegisterAllAvailableTracker()
        {

            // register all module
            foreach (System.Type type
                in System.Reflection.Assembly.GetAssembly(typeof(TactileAbstractTracker)).GetTypes()
                .Where(
                    myType => myType.IsClass 
                    && !myType.IsAbstract
                    && !myType.IsGenericType
                    && myType.IsSubclassOf(typeof(TactileAbstractTracker))
                )
            )
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=blue>Info: </color> TactileManager.RegisterAllAvailableTracker() : found tracker ["
                    + type.FullName
                    + "]."
                );

                // get "Instance" property
                TactileAbstractTracker tracker = null;
                if ((tracker = System.Activator.CreateInstance(type) as TactileAbstractTracker) != null)
                {

                    // check behaviour property, if null then object isn'nt in unity Scene
                    if(tracker.Behaviour == null)
                    {
                        
                        // mark as disposable
                        System.GC.SuppressFinalize(tracker);

                        // log
                        UnityEngine.Debug.Log(
                            "<color=blue>Info: </color> TactileManager.RegisterAllAvailableTracker() : tracker ["
                            + type.FullName
                            + "] not in current Scene, mark as dispobale."
                        );

                        // next module 
                        continue;

                    } /* if() */
                    
                    // log
                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> TactileManager.RegisterAllAvailableTracker() : success to create instance of tracker ["
                        + type.FullName
                        + "]."
                    );

                    // register it

                    // check
                    if (this._trackerTable.ContainsKey(tracker.ID))
                    {

                        UnityEngine.Debug.LogError(
                             "<color=red>Error: </color> TactileManager.RegisterAllAvailableTracker("
                             + tracker.ID
                             + ") : tracker key already found ..."
                        );
                        return;

                    } /* if() */

                    // add to table
                    this._trackerTable.Add(tracker.ID, tracker);

                    // plug module
                    this.ActivationRequestedEvent += tracker.onActivationRequested;
                    this.InactivationRequestedEvent += tracker.onInactivationRequested;

                    // log
                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> TactileManager.RegisterAllAvailableTracker() : tracker ["
                        + tracker.ID
                        + "], registration ok !"
                    );

                }
                else
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> TactileManager.RegisterAllAvailableTracker() : could not create instance of gameobject ["
                        + type.FullName
                        + "]. fail."
                    );

                } /* if() */

            } /* foreach() */

        } /* RegisterAllAvailableTracker() */

        public TactileAbstractTracker getTracker(IDType ID)
        {

            // check 
            if(this._trackerTable.ContainsKey(ID))
            {
                return this._trackerTable[ID];
            }

            // log
            UnityEngine.Debug.LogWarning(
                 "<color=orange>Warning: </color> TactileManager.getTracker("
                 + ID
                 + ") : requested ID not found ..."
            );

            // failed
            return null;

        } /* getTracker() */

        #endregion

        #region event handling

        // Dictionary & each list event

        private readonly System.Collections.Generic.Dictionary<string, System.Delegate> _eventTable;

        private readonly System.Collections.Generic.List<System.EventHandler<EventArgs>> _eventActivationRequestedList
            = new System.Collections.Generic.List<System.EventHandler<EventArgs>>();

        private readonly System.Collections.Generic.List<System.EventHandler<EventArgs>> _eventInactivationRequestedList
            = new System.Collections.Generic.List<System.EventHandler<EventArgs>>();

        // the actual event 

        public event System.EventHandler<EventArgs> ActivationRequestedEvent
        {
            add
            {
                this._eventActivationRequestedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["ActivationRequested"] = (System.EventHandler<EventArgs>)this._eventTable["ActivationRequested"] + value;
                }
            }

            remove
            {
                if (!this._eventActivationRequestedList.Contains(value))
                {
                    return;
                }
                this._eventActivationRequestedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["ActivationRequested"] = null;
                    foreach (var eventActivationRequested in this._eventActivationRequestedList)
                    {
                        this._eventTable["ActivationRequested"] = (System.EventHandler<EventArgs>)this._eventTable["ActivationRequested"] + eventActivationRequested;
                    }
                }
            }

        } /* LoadedEvent */

        public event System.EventHandler<EventArgs> InactivationRequestedEvent
        {
            add
            {
                this._eventInactivationRequestedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["InactivationRequested"] = (System.EventHandler<EventArgs>)this._eventTable["InactivationRequested"] + value;
                }
            }

            remove
            {
                if (!this._eventInactivationRequestedList.Contains(value))
                {
                    return;
                }
                this._eventInactivationRequestedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["InactivationRequested"] = null;
                    foreach (var eventInactivationRequested in this._eventInactivationRequestedList)
                    {
                        this._eventTable["InactivationRequested"] = (System.EventHandler<EventArgs>)this._eventTable["InactivationRequested"] + eventInactivationRequested;
                    }
                }
            }

        } /* UnloadedEvent */

        internal void RaiseActivationRequestedEvent(IDType ID)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["ActivationRequested"];
                callback?.Invoke(this, new EventArgs(ID));
            }
        }

        internal void RaisInactivationRequestedEvent(IDType ID)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EventArgs>)this._eventTable["InactivationRequested"];
                callback?.Invoke(this, new EventArgs(ID));
            }
        }

        // the event args class
        public class EventArgs
            : apollon.ApollonEngine.EngineEventArgs
        {

            // ctor
            public EventArgs(
                IDType ID = IDType.None,
                bool bEnableBehaviour = false,
                bool bActivateObject = false
            )
                : base()
            {
                this.ID = ID;
                this.EnableBehaviour = bEnableBehaviour;
                this.ActivateObject = bActivateObject;
            }

            // ctor
            public EventArgs(EventArgs rhs)
                : base(rhs)
            {
                this.ID = rhs.ID;
                this.EnableBehaviour = rhs.EnableBehaviour;
                this.ActivateObject = rhs.ActivateObject;
            }

            // property
            public IDType ID { get; protected set; } = IDType.None;
            public bool EnableBehaviour { get; protected set; } = false;
            public bool ActivateObject { get; protected set; } = false;
            
        } /* EventArgs() */

        #endregion

        #region lazy init singleton pattern

        // lazy init paradigm
        private static readonly System.Lazy<TactileTrackerManager> _lazyInstance
            = new System.Lazy<TactileTrackerManager>(() => new TactileTrackerManager());

        // Instance  property
        public static TactileTrackerManager Instance => TactileTrackerManager._lazyInstance.Value;

        // private ctor
        private TactileTrackerManager()
        {

            // event table
            this._eventTable = new System.Collections.Generic.Dictionary<string, System.Delegate>
            {
                { "ActivationRequested", null },
                { "InactivationRequested", null }
            };

            // set different menu state 
            this._State = new System.Collections.Generic.Dictionary<IDType, bool>
            {
                { IDType.None, false },
                { IDType.TactileResponseArea, false },
                { IDType.TactileSubjectHMD, false },
                { IDType.TactileSubjectRHWrist, false },
                { IDType.TactileSubjectRHIndexProximalJoint, false },
                { IDType.TactileSubjectRHIndexMiddleJoint, false },
                { IDType.TactileSubjectRHIndexDistalJoint, false },
                { IDType.TactileSubjectRHIndexTip, false },
                { IDType.All, false }
            };

            // registering
            this.RegisterAllAvailableTracker();

        } /* TactileTrackerManager() */

        #endregion

        #region member/method

        private readonly System.Collections.Generic.Dictionary<IDType, bool> _State
            = new System.Collections.Generic.Dictionary<IDType, bool>();

        public enum IDType
        {

            [System.ComponentModel.Description("None")]
            None = 0,

            [System.ComponentModel.Description("ResponseArea")]
            TactileResponseArea,

            [System.ComponentModel.Description("SubjectHMD")]
            TactileSubjectHMD,

            [System.ComponentModel.Description("SubjectRHWrist")]
            TactileSubjectRHWrist,

            [System.ComponentModel.Description("SubjectRHIndexProximalJoint")]
            TactileSubjectRHIndexProximalJoint,

            [System.ComponentModel.Description("SubjectRHIndexMiddleJoint")]
            TactileSubjectRHIndexMiddleJoint,

            [System.ComponentModel.Description("SubjectRHIndexDistalJoint")]
            TactileSubjectRHIndexDistalJoint,

            [System.ComponentModel.Description("SubjectRHIndexTip")]
            TactileSubjectRHIndexTip,

            [System.ComponentModel.Description("All")]
            All

        } /* enum */

        public bool IsActive(IDType ID)
        {

            lock (this._State)
            {

                if (this._State.ContainsKey(ID))
                {

                    return this._State[ID];

                }
                else
                {

                    UnityEngine.Debug.LogWarning(
                        "<color=orange>Warning: </color> TactileManager.IsActive("
                        + ID
                        + ") : requested ID is not intantiated yet..."
                    );
                    return false;

                } /* if() */

            } /* lock() */

        } /* IsActive() */

        public void setActive(IDType ID)
        {

            lock (this._State)
            {

                if (this._State.ContainsKey(ID))
                {

                    if (this.IsActive(ID))
                    {
                        UnityEngine.Debug.LogWarning(
                            "<color=orange>Warning: </color> TactileManager.setActive("
                            + ID
                            + ") : requested ID is already active, skip"
                        );
                        return;
                    }

                    this._State[ID] = true;
                    this.RaiseActivationRequestedEvent(ID);

                }
                else
                {

                    UnityEngine.Debug.LogWarning(
                        "<color=orange>Warning: </color> TactileManager.setActive("
                        + ID
                        + ") : requested ID is not intantiated yet..."
                    );
                    return;

                } /* if() */

            } /* lock() */

        } /* setActive() */

        public void setInactive(IDType ID)
        {
            lock (this._State)
            {

                if (this._State.ContainsKey(ID))
                {

                    if (!this.IsActive(ID))
                    {
                        UnityEngine.Debug.LogWarning(
                            "<color=orange>Warning: </color> TactileManager.setInactive("
                            + ID
                            + ") : requested ID is already inactive, skip"
                        );
                        return;
                    }

                    this._State[ID] = false;
                    this.RaisInactivationRequestedEvent(ID);

                }
                else
                {

                    UnityEngine.Debug.LogWarning(
                        "<color=orange>Warning: </color> TactileManager.setInactive("
                        + ID
                        + ") : requested menuID is not intantiated yet..."
                    );
                    return;

                } /* if() */

            } /* lock() */

        } /* setInactive() */

        public System.Collections.Generic.IEnumerable<IDType> getAllActive()
        {

            return (
                this._State
                    .Where(item => item.Value == true)
                    .Select(item => item.Key)
            );

        } /* getAllActive() */

        public System.Collections.Generic.IEnumerable<IDType> getAllInactive()
        {

            return (
                this._State
                    .Select(item => item.Key)
                    .Except(this.getAllActive())
            );

        } /* getAllInactive() */

        #endregion

        #region abstract manager implementation

        public override void onStart(object sender, apollon.ApollonEngine.EngineEventArgs arg)
        {

            // default 
            this.setActive(IDType.None);

        } /* onStart() */

        public override void onExperimentTrialBegin(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {
            
            // activate all tracker
            this.RaiseActivationRequestedEvent(IDType.All);

        } /* onExperimentTrialBegin() */

        public override void onExperimentTrialEnd(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {
            
            // activate all tracker
            this.RaisInactivationRequestedEvent(IDType.All);

        } /* onExperimentTrialEnd() */

        #endregion

    } /* class TactileTrackerManager */

} /* namespace Labsim.experiment.tactile */