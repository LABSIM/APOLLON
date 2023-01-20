// using directive
using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon
{

    public sealed class ApollonEngine
    {

        #region task scheduler mechanism [async/await == multi-thread <=> UnityEngine.Coroutine == single-thread]

        // Use this as a global Coroutine tasker mechanism to handle internal Async/Await logic
        private readonly System.Lazy<ApollonEngineComponent> _lazyUnityComponent
            = new System.Lazy<ApollonEngineComponent>(
                () => UnityEngine.GameObject.FindObjectOfType<ApollonEngineComponent>()
            );
        private ApollonEngineComponent UnityComponent => this._lazyUnityComponent.Value;

        // facade
        public static void Schedule(System.Action task)
        {

            // schedule a new action into UnityEngine system
            ApollonEngine.Instance.UnityComponent.PendAction(task);

        } /* Schedule() */

        #endregion

        #region manager handling

        private readonly System.Collections.Generic.List<string> _managerList
            = new System.Collections.Generic.List<string>();
        
        private void RegisterAllAvailableManager()
        {
            
            // register all module
            foreach (System.Type type
                in System.Reflection.Assembly.GetAssembly(typeof(ApollonAbstractManager)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(ApollonAbstractManager)))
            )
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=blue>Info: </color> ApollonEngine.RegisterAllAvailableManager() : found module ["
                    + type.FullName
                    + "]."
                );

                // get "Instance" property
                System.Reflection.PropertyInfo prop = null;
                if ((prop = type.GetProperty("Instance")) != null)
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonEngine.RegisterAllAvailableManager() : found property Instance in module ["
                        + type.FullName
                        + "]."
                    );

                    // static class
                    ApollonAbstractManager mng = null;
                    if ((mng = prop.GetValue(null,null) as ApollonAbstractManager) != null)
                    {

                        // log
                        UnityEngine.Debug.Log(
                            "<color=blue>Info: </color> ApollonEngine.RegisterAllAvailableManager() : get Instance value for module ["
                            + type.FullName
                            + "], try to register it."
                        );

                        // register it

                        // check
                        if (this._managerList.Contains(type.FullName))
                        {

                            UnityEngine.Debug.LogError(
                                 "<color=red>Error: </color> ApollonEngine.RegisterAllAvailableManager("
                                 + type.FullName
                                 + ") : module already found ..."
                            );
                            return;

                        } /* if() */

                        // add to table
                        this._managerList.Add(type.FullName);

                        // plug module
                        this.EngineStartEvent += mng.onStart;
                        this.EngineAwakeEvent += mng.onAwake;
                        this.EngineUpdateEvent += mng.onUpdate;
                        this.EngineFixedUpdateEvent += mng.onFixedUpdate;
                        this.EngineSceneLoadedEvent += mng.onSceneLoaded;
                        this.EngineSceneUnloadedEvent += mng.onSceneUnloaded;
                        this.EngineExperimentSessionBeginEvent += mng.onExperimentSessionBegin;
                        this.EngineExperimentSessionEndEvent += mng.onExperimentSessionEnd;
                        this.EngineExperimentTrialBeginEvent += mng.onExperimentTrialBegin;
                        this.EngineExperimentTrialEndEvent += mng.onExperimentTrialEnd;

                        // log
                        UnityEngine.Debug.Log(
                            "<color=blue>Info: </color> ApollonEngine.RegisterAllAvailableManager() : module ["
                            + type.FullName
                            + "], registration ok !"
                        );

                    }
                    else
                    {

                        // log
                        UnityEngine.Debug.LogError(
                            "<color=red>Info: </color> ApollonEngine.RegisterAllAvailableManager() : could not get Instance value for module ["
                            + type.FullName
                            + "]. fail."
                        );

                    } /* if() */

                }
                else
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonEngine.RegisterAllAvailableManager() : could not found property Instance in module ["
                        + type.FullName
                        + "]. fail."
                    );

                } /* if() */

            } /* foreach() */

        } /* RegisterAllAvailableManager() */

        #endregion

        #region event handling

        // Dictionary & each list event

        private readonly System.Collections.Generic.Dictionary<string, System.Delegate> _eventTable;

        private readonly System.Collections.Generic.List<System.EventHandler<EngineEventArgs>>
            _eventEngineStartList       = new System.Collections.Generic.List<System.EventHandler<EngineEventArgs>>(),
            _eventEngineAwakeList       = new System.Collections.Generic.List<System.EventHandler<EngineEventArgs>>(),
            _eventEngineUpdateList      = new System.Collections.Generic.List<System.EventHandler<EngineEventArgs>>(),
            _eventEngineFixedUpdateList = new System.Collections.Generic.List<System.EventHandler<EngineEventArgs>>();
        
        private readonly System.Collections.Generic.List<System.EventHandler<EngineSceneEventArgs>>
            _eventEngineSceneLoadedList     = new System.Collections.Generic.List<System.EventHandler<EngineSceneEventArgs>>(),
            _eventEngineSceneUnloadedList   = new System.Collections.Generic.List<System.EventHandler<EngineSceneEventArgs>>();

        private readonly System.Collections.Generic.List<System.EventHandler<EngineExperimentEventArgs>>
            _eventEngineExperimentSessionBeginList  = new System.Collections.Generic.List<System.EventHandler<EngineExperimentEventArgs>>(),
            _eventEngineExperimentSessionEndList    = new System.Collections.Generic.List<System.EventHandler<EngineExperimentEventArgs>>(),
            _eventEngineExperimentTrialBeginList    = new System.Collections.Generic.List<System.EventHandler<EngineExperimentEventArgs>>(),
            _eventEngineExperimentTrialEndList      = new System.Collections.Generic.List<System.EventHandler<EngineExperimentEventArgs>>();

        // the actual event 

        public event System.EventHandler<EngineEventArgs> EngineStartEvent
        {
            add
            {
                this._eventEngineStartList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["EngineStart"] = (System.EventHandler<EngineEventArgs>)this._eventTable["EngineStart"] + value;
                }
            }

            remove
            {
                if (!this._eventEngineStartList.Contains(value))
                {
                    return;
                }
                this._eventEngineStartList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["EngineStart"] = null;
                    foreach (var eventEngineStart in this._eventEngineStartList)
                    {
                        this._eventTable["EngineStart"] = (System.EventHandler<EngineEventArgs>)this._eventTable["EngineStart"] + eventEngineStart;
                    }
                }
            }

        } /* EngineStartEvent */

        public event System.EventHandler<EngineEventArgs> EngineAwakeEvent
        {
            add
            {
                this._eventEngineAwakeList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["EngineAwake"] = (System.EventHandler<EngineEventArgs>)this._eventTable["EngineAwake"] + value;
                }
            }

            remove
            {
                if (!this._eventEngineAwakeList.Contains(value))
                {
                    return;
                }
                this._eventEngineAwakeList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["EngineAwake"] = null;
                    foreach (var eventEngineAwake in this._eventEngineAwakeList)
                    {
                        this._eventTable["EngineAwake"] = (System.EventHandler<EngineEventArgs>)this._eventTable["EngineAwake"] + eventEngineAwake;
                    }
                }
            }

        } /* EngineAwakeEvent */

        public event System.EventHandler<EngineEventArgs> EngineUpdateEvent
        {
            add
            {
                this._eventEngineUpdateList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["EngineUpdate"] = (System.EventHandler<EngineEventArgs>)this._eventTable["EngineUpdate"] + value;
                }
            }

            remove
            {
                if (!this._eventEngineUpdateList.Contains(value))
                {
                    return;
                }
                this._eventEngineUpdateList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["EngineUpdate"] = null;
                    foreach (var eventEngineUpdate in this._eventEngineUpdateList)
                    {
                        this._eventTable["EngineUpdate"] = (System.EventHandler<EngineEventArgs>)this._eventTable["EngineUpdate"] + eventEngineUpdate;
                    }
                }
            }

        } /* EngineUpdateEvent */

        public event System.EventHandler<EngineEventArgs> EngineFixedUpdateEvent
        {
            add
            {
                this._eventEngineFixedUpdateList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["EngineFixedUpdate"] = (System.EventHandler<EngineEventArgs>)this._eventTable["EngineFixedUpdate"] + value;
                }
            }

            remove
            {
                if (!this._eventEngineFixedUpdateList.Contains(value))
                {
                    return;
                }
                this._eventEngineFixedUpdateList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["EngineFixedUpdate"] = null;
                    foreach (var eventEngineFixedUpdate in this._eventEngineFixedUpdateList)
                    {
                        this._eventTable["EngineFixedUpdate"] = (System.EventHandler<EngineEventArgs>)this._eventTable["EngineFixedUpdate"] + eventEngineFixedUpdate;
                    }
                }
            }

        } /* EngineUpdateEvent */

        public event System.EventHandler<EngineSceneEventArgs> EngineSceneLoadedEvent
        {
            add
            {
                this._eventEngineSceneLoadedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["EngineSceneLoaded"] = (System.EventHandler<EngineSceneEventArgs>)this._eventTable["EngineSceneLoaded"] + value;
                }
            }

            remove
            {
                if (!this._eventEngineSceneLoadedList.Contains(value))
                {
                    return;
                }
                this._eventEngineSceneLoadedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["EngineSceneLoaded"] = null;
                    foreach (var eventEngineSceneLoaded in this._eventEngineSceneLoadedList)
                    {
                        this._eventTable["EngineSceneLoaded"] = (System.EventHandler<EngineSceneEventArgs>)this._eventTable["EngineSceneLoaded"] + eventEngineSceneLoaded;
                    }
                }
            }

        } /* EngineSceneLoadedEvent */

        public event System.EventHandler<EngineSceneEventArgs> EngineSceneUnloadedEvent
        {
            add
            {
                this._eventEngineSceneUnloadedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["EngineSceneUnloaded"] = (System.EventHandler<EngineSceneEventArgs>)this._eventTable["EngineSceneUnloaded"] + value;
                }
            }

            remove
            {
                if (!this._eventEngineSceneUnloadedList.Contains(value))
                {
                    return;
                }
                this._eventEngineSceneUnloadedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["EngineSceneUnloaded"] = null;
                    foreach (var eventEngineSceneUnloaded in this._eventEngineSceneUnloadedList)
                    {
                        this._eventTable["EngineSceneUnloaded"] = (System.EventHandler<EngineSceneEventArgs>)this._eventTable["EngineSceneUnloaded"] + eventEngineSceneUnloaded;
                    }
                }
            }

        } /* EngineSceneUnloadedEvent */

        public event System.EventHandler<EngineExperimentEventArgs> EngineExperimentSessionBeginEvent
        {
            add
            {
                this._eventEngineExperimentSessionBeginList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["EngineExperimentSessionBegin"] = (System.EventHandler<EngineExperimentEventArgs>)this._eventTable["EngineExperimentSessionBegin"] + value;
                }
            }

            remove
            {
                if (!this._eventEngineExperimentSessionBeginList.Contains(value))
                {
                    return;
                }
                this._eventEngineExperimentSessionBeginList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["EngineExperimentSessionBegin"] = null;
                    foreach (var eventEngineExperimentSessionBegin in this._eventEngineExperimentSessionBeginList)
                    {
                        this._eventTable["EngineExperimentSessionBegin"] = (System.EventHandler<EngineExperimentEventArgs>)this._eventTable["EngineExperimentSessionBegin"] + eventEngineExperimentSessionBegin;
                    }
                }
            }

        } /* EngineExperimentSessionBeginEvent */

        public event System.EventHandler<EngineExperimentEventArgs> EngineExperimentSessionEndEvent
        {
            add
            {
                this._eventEngineExperimentSessionEndList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["EngineExperimentSessionEnd"] = (System.EventHandler<EngineExperimentEventArgs>)this._eventTable["EngineExperimentSessionEnd"] + value;
                }
            }

            remove
            {
                if (!this._eventEngineExperimentSessionEndList.Contains(value))
                {
                    return;
                }
                this._eventEngineExperimentSessionEndList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["EngineExperimentSessionEnd"] = null;
                    foreach (var eventEngineExperimentSessionEnd in this._eventEngineExperimentSessionEndList)
                    {
                        this._eventTable["EngineExperimentSessionEnd"] = (System.EventHandler<EngineExperimentEventArgs>)this._eventTable["EngineExperimentSessionEnd"] + eventEngineExperimentSessionEnd;
                    }
                }
            }

        } /* EngineExperimentSessionEndEvent */

        public event System.EventHandler<EngineExperimentEventArgs> EngineExperimentTrialBeginEvent
        {
            add
            {
                this._eventEngineExperimentTrialBeginList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["EngineExperimentTrialBegin"] = (System.EventHandler<EngineExperimentEventArgs>)this._eventTable["EngineExperimentTrialBegin"] + value;
                }
            }

            remove
            {
                if (!this._eventEngineExperimentTrialBeginList.Contains(value))
                {
                    return;
                }
                this._eventEngineExperimentTrialBeginList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["EngineExperimentTrialBegin"] = null;
                    foreach (var eventEngineExperimentTrialBegin in this._eventEngineExperimentTrialBeginList)
                    {
                        this._eventTable["EngineExperimentTrialBegin"] = (System.EventHandler<EngineExperimentEventArgs>)this._eventTable["EngineExperimentTrialBegin"] + eventEngineExperimentTrialBegin;
                    }
                }
            }

        } /* EngineExperimentTrialBeginEvent */

        public event System.EventHandler<EngineExperimentEventArgs> EngineExperimentTrialEndEvent
        {
            add
            {
                this._eventEngineExperimentTrialEndList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["EngineExperimentTrialEnd"] = (System.EventHandler<EngineExperimentEventArgs>)this._eventTable["EngineExperimentTrialEnd"] + value;
                }
            }

            remove
            {
                if (!this._eventEngineExperimentTrialEndList.Contains(value))
                {
                    return;
                }
                this._eventEngineExperimentTrialEndList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["EngineExperimentTrialEnd"] = null;
                    foreach (var eventEngineExperimentTrialEnd in this._eventEngineExperimentTrialEndList)
                    {
                        this._eventTable["EngineExperimentTrialEnd"] = (System.EventHandler<EngineExperimentEventArgs>)this._eventTable["EngineExperimentTrialEnd"] + eventEngineExperimentTrialEnd;
                    }
                }
            }

        } /* EngineExperimentTrialEndEvent */

        internal void RaiseEngineStartEvent()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EngineEventArgs>)this._eventTable["EngineStart"];
                callback?.Invoke(this, new EngineEventArgs());
            }
        }

        internal void RaiseEngineAwakeEvent()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EngineEventArgs>)this._eventTable["EngineAwake"];
                callback?.Invoke(this, new EngineEventArgs());
            }
        }

        internal void RaiseEngineUpdateEvent()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EngineEventArgs>)this._eventTable["EngineUpdate"];
                callback?.Invoke(this, new EngineEventArgs());
            }
        }

        internal void RaiseEngineFixedUpdateEvent()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EngineEventArgs>)this._eventTable["EngineFixedUpdate"];
                callback?.Invoke(this, new EngineEventArgs());
            }
        }

        internal void RaiseEngineSceneLoadedEvent(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EngineSceneEventArgs>)this._eventTable["EngineSceneLoaded"];
                callback?.Invoke(this, new EngineSceneEventArgs(scene, mode));
            }
        }

        internal void RaiseEngineSceneUnloadedEvent(UnityEngine.SceneManagement.Scene scene)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EngineSceneEventArgs>)this._eventTable["EngineSceneUnloaded"];
                callback?.Invoke(this, new EngineSceneEventArgs(scene));
            }
        }

        internal void RaiseEngineExperimentSessionBeginEvent(UXF.Session experimentSession)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EngineExperimentEventArgs>)this._eventTable["EngineExperimentSessionBegin"];
                callback?.Invoke(this, new EngineExperimentEventArgs(session: experimentSession));
            }
        }
        
        internal void RaiseEngineExperimentSessionEndEvent()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EngineExperimentEventArgs>)this._eventTable["EngineExperimentSessionEnd"];
                callback?.Invoke(this, new EngineExperimentEventArgs());
            }
        }

        internal void RaiseEngineExperimentTrialBeginEvent(UXF.Trial experimentTrial)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EngineExperimentEventArgs>)this._eventTable["EngineExperimentTrialBegin"];
                callback?.Invoke(this, new EngineExperimentEventArgs(trial: experimentTrial));
            }
        }

        internal void RaiseEngineExperimentTrialEndEvent()
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EngineExperimentEventArgs>)this._eventTable["EngineExperimentTrialEnd"];
                callback?.Invoke(this, new EngineExperimentEventArgs());
            }
        }

        // the event args class
        public class EngineEventArgs
            : System.EventArgs
        {

            // ctor
            public EngineEventArgs()
                : base()
            {
                this.Time = ApollonHighResolutionTime.Now;
            }

            public EngineEventArgs(EngineEventArgs rhs)
            {
                this.Time = rhs.Time;
            }

            // property
            public ApollonHighResolutionTime.HighResolutionTimepoint Time { get; protected set; }

        } /* EngineEventArgs */

        public class EngineSceneEventArgs
            : EngineEventArgs
        {

            public EngineSceneEventArgs(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode = UnityEngine.SceneManagement.LoadSceneMode.Single)
                : base()
            {
                this.Scene = scene;
                this.Mode = mode;
            }

            public EngineSceneEventArgs(EngineSceneEventArgs rhs)
                : base(rhs)
            {
                this.Scene = rhs.Scene;
                this.Mode = rhs.Mode;
            }

            // property
            public UnityEngine.SceneManagement.Scene Scene { get; protected set; }
            public UnityEngine.SceneManagement.LoadSceneMode Mode { get; protected set; }

        } /* EngineEventArgs */

        public class EngineExperimentEventArgs
            : EngineEventArgs
        {
            // Session ctor
            public EngineExperimentEventArgs(UXF.Session session = null, UXF.Trial trial = null)
                : base()
            {
                this.Session = session;
                this.Trial = trial;
            }

            public EngineExperimentEventArgs(EngineExperimentEventArgs rhs)
                : base(rhs)
            {
                this.Session = rhs.Session;
                this.Trial = rhs.Trial;
            }

            // property
            public UXF.Session Session { get; protected set; }
            public UXF.Trial Trial { get; protected set; }

        } /* EngineExperimentEventArgs */

        #endregion

        #region lazy init singleton pattern

        // lazy init paradigm
        private static readonly System.Lazy<ApollonEngine> _lazyInstance
            = new System.Lazy<ApollonEngine>(() => new ApollonEngine());

        // Instance
        public static ApollonEngine Instance => ApollonEngine._lazyInstance.Value;

        // private ctor
        private ApollonEngine()
        {
            this._eventTable = new System.Collections.Generic.Dictionary<string, System.Delegate>
            {
                { "EngineStart", null },
                { "EngineUpdate", null },
                { "EngineFixedUpdate", null },
                { "EngineAwake", null },
                { "EngineSceneLoaded", null },
                { "EngineSceneUnloaded", null },
                { "EngineExperimentSessionBegin", null },
                { "EngineExperimentSessionEnd", null },
                { "EngineExperimentTrialBegin", null },
                { "EngineExperimentTrialEnd", null }
            };

            // check/init component
            if(this.UnityComponent == null)
            {
                
                // log
                UnityEngine.Debug.LogError(
                    "<color=Red>Error: </color> ApollonEngine.ApollonEngine() : could not find UnityComponent reference in scene !"
                );

            } /* if() */

            // register all managers
            this.RegisterAllAvailableManager();

        } /* ApollonEngine() */

        #endregion

        #region public method

        public void Start()
        {
            this.RaiseEngineStartEvent();
        }

        public void Awake()
        {
            this.RaiseEngineAwakeEvent();
        }

        public void Update()
        {
            this.RaiseEngineUpdateEvent();
        }

        public void FixedUpdate()
        {
            this.RaiseEngineFixedUpdateEvent();
        }

        public void SceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            this.RaiseEngineSceneLoadedEvent(scene, mode);
        }

        public void SceneUnloaded(UnityEngine.SceneManagement.Scene scene)
        {
            this.RaiseEngineSceneUnloadedEvent(scene);
        }

        public void ExperimentSessionBegin(UXF.Session experimentSession)
        {
            this.RaiseEngineExperimentSessionBeginEvent(experimentSession);
        }

        public void ExperimentSessionEnd()
        {
            this.RaiseEngineExperimentSessionEndEvent();
        }

        public void ExperimentTrialBegin(UXF.Trial experimentTrial)
        {
            this.RaiseEngineExperimentTrialBeginEvent(experimentTrial);
        }

        public void ExperimentTrialEnd()
        {
            this.RaiseEngineExperimentTrialEndEvent();
        }

        #endregion

        #region static useful method

        public static string GetEnumDescription<T>(T enumerationValue)
        {
            System.Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new System.ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            System.Reflection.MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((System.ComponentModel.DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }

        #endregion

    } /* class ApollonEngine */

} /* namespace Labsim.apollo */