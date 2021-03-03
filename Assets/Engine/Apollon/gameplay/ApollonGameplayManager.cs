
// extensions
using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.gameplay
{

    public sealed class ApollonGameplayManager : ApollonAbstractManager
    {

        #region bridge handling

        private readonly System.Collections.Generic.Dictionary<GameplayIDType, ApollonAbstractGameplayBridge> _gameplayBridgeTable
            = new System.Collections.Generic.Dictionary<GameplayIDType, ApollonAbstractGameplayBridge>();

        private void RegisterAllAvailableGameplayBridge()
        {

            // register all module
            foreach (System.Type type
                in System.Reflection.Assembly.GetAssembly(typeof(ApollonAbstractGameplayBridge)).GetTypes()
                .Where(
                    myType => myType.IsClass 
                    && !myType.IsAbstract
                    && !myType.IsGenericType
                    && myType.IsSubclassOf(typeof(ApollonAbstractGameplayBridge))
                )
            )
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=blue>Info: </color> ApollonGameplayManager.RegisterAllAvailableGameplayBridge() : found bridge ["
                    + type.FullName
                    + "]."
                );

                // get "Instance" property
                ApollonAbstractGameplayBridge bridge = null;
                if ((bridge = System.Activator.CreateInstance(type) as ApollonAbstractGameplayBridge) != null)
                {

                    // check behaviour property, if null then object isn'nt in unity Scene
                    if(bridge.Behaviour == null)
                    {
                        
                        // mark as disposable
                        System.GC.SuppressFinalize(bridge);

                        // log
                        UnityEngine.Debug.Log(
                            "<color=blue>Info: </color> ApollonGameplayManager.RegisterAllAvailableGameplayBridge() : bridge ["
                            + type.FullName
                            + "] not in current Scene, mark as dispobale."
                        );

                        // next module 
                        continue;

                    } /* if() */
                    
                    // log
                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonGameplayManager.RegisterAllAvailableGameplayBridge() : success to create instance of bridge ["
                        + type.FullName
                        + "]."
                    );

                    // register it

                    // check
                    if (this._gameplayBridgeTable.ContainsKey(bridge.ID))
                    {

                        UnityEngine.Debug.LogError(
                             "<color=red>Error: </color> ApollonGameplayManager.RegisterAllAvailableGameplayBridge("
                             + bridge.ID
                             + ") : bridge key already found ..."
                        );
                        return;

                    } /* if() */

                    // add to table
                    this._gameplayBridgeTable.Add(bridge.ID, bridge);

                    // plug module
                    this.GameplayActivationRequestedEvent += bridge.onActivationRequested;
                    this.GameplayInactivationRequestedEvent += bridge.onInactivationRequested;

                    // log
                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonGameplayManager.RegisterAllAvailableGameplayBridge() : bridge ["
                        + bridge.ID
                        + "], registration ok !"
                    );

                }
                else
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonGameplayManager.RegisterAllAvailableGameplayBridge() : could not create instance of gameobject ["
                        + type.FullName
                        + "]. fail."
                    );

                } /* if() */

            } /* foreach() */

        } /* RegisterAllAvailableGameplayBridge() */

        public ApollonAbstractGameplayBridge getBridge(GameplayIDType ID)
        {

            // check 
            if(this._gameplayBridgeTable.ContainsKey(ID))
            {
                return this._gameplayBridgeTable[ID];
            }

            // log
            UnityEngine.Debug.LogWarning(
                 "<color=orange>Warning: </color> ApollonGameplayManager.getBridge("
                 + ID
                 + ") : requested ID not found ..."
            );

            // failed
            return null;

        } /* getBridge() */

        #endregion

        #region event handling

        // Dictionary & each list event

        private readonly System.Collections.Generic.Dictionary<string, System.Delegate> _eventTable;

        private readonly System.Collections.Generic.List<System.EventHandler<GameplayEventArgs>> _eventGameplayActivationRequestedList
            = new System.Collections.Generic.List<System.EventHandler<GameplayEventArgs>>();

        private readonly System.Collections.Generic.List<System.EventHandler<GameplayEventArgs>> _eventGameplayInactivationRequestedList
            = new System.Collections.Generic.List<System.EventHandler<GameplayEventArgs>>();

        // the actual event 

        public event System.EventHandler<GameplayEventArgs> GameplayActivationRequestedEvent
        {
            add
            {
                this._eventGameplayActivationRequestedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["GameplayActivationRequested"] = (System.EventHandler<GameplayEventArgs>)this._eventTable["GameplayActivationRequested"] + value;
                }
            }

            remove
            {
                if (!this._eventGameplayActivationRequestedList.Contains(value))
                {
                    return;
                }
                this._eventGameplayActivationRequestedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["GameplayActivationRequested"] = null;
                    foreach (var eventGameplayActivationRequested in this._eventGameplayActivationRequestedList)
                    {
                        this._eventTable["GameplayActivationRequested"] = (System.EventHandler<GameplayEventArgs>)this._eventTable["GameplayActivationRequested"] + eventGameplayActivationRequested;
                    }
                }
            }

        } /* GameplayLoadedEvent */

        public event System.EventHandler<GameplayEventArgs> GameplayInactivationRequestedEvent
        {
            add
            {
                this._eventGameplayInactivationRequestedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["GameplayInactivationRequested"] = (System.EventHandler<GameplayEventArgs>)this._eventTable["GameplayInactivationRequested"] + value;
                }
            }

            remove
            {
                if (!this._eventGameplayInactivationRequestedList.Contains(value))
                {
                    return;
                }
                this._eventGameplayInactivationRequestedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["GameplayInactivationRequested"] = null;
                    foreach (var eventGameplayInactivationRequested in this._eventGameplayInactivationRequestedList)
                    {
                        this._eventTable["GameplayInactivationRequested"] = (System.EventHandler<GameplayEventArgs>)this._eventTable["GameplayInactivationRequested"] + eventGameplayInactivationRequested;
                    }
                }
            }

        } /* GameplayUnloadedEvent */

        internal void RaiseGameplayActivationRequestedEvent(GameplayIDType gameplayID)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<GameplayEventArgs>)this._eventTable["GameplayActivationRequested"];
                callback?.Invoke(this, new GameplayEventArgs(gameplayID));
            }
        }

        internal void RaisGameplayInactivationRequestedEvent(GameplayIDType gameplayID)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<GameplayEventArgs>)this._eventTable["GameplayInactivationRequested"];
                callback?.Invoke(this, new GameplayEventArgs(gameplayID));
            }
        }

        // the event args class
        public class GameplayEventArgs
            : ApollonEngine.EngineEventArgs
        {

            // ctor
            public GameplayEventArgs(
                GameplayIDType gameplayID = GameplayIDType.None,
                bool bEnableBehaviour = false,
                bool bActivateObject = false
            )
                : base()
            {
                this.ID = gameplayID;
                this.EnableBehaviour = bEnableBehaviour;
                this.ActivateObject = bActivateObject;
            }

            // ctor
            public GameplayEventArgs(GameplayEventArgs rhs)
                : base(rhs)
            {
                this.ID = rhs.ID;
                this.EnableBehaviour = rhs.EnableBehaviour;
                this.ActivateObject = rhs.ActivateObject;
            }

            // property
            public GameplayIDType ID { get; protected set; } = GameplayIDType.None;
            public bool EnableBehaviour { get; protected set; } = false;
            public bool ActivateObject { get; protected set; } = false;
            
        } /* GameplayEventArgs() */

        #endregion

        #region lazy init singleton pattern

        // lazy init paradigm
        private static readonly System.Lazy<ApollonGameplayManager> _lazyInstance
            = new System.Lazy<ApollonGameplayManager>(() => new ApollonGameplayManager());

        // Instance  property
        public static ApollonGameplayManager Instance { get { return _lazyInstance.Value; } }

        // private ctor
        private ApollonGameplayManager()
        {
            // event table
            this._eventTable = new System.Collections.Generic.Dictionary<string, System.Delegate>
            {
                { "GameplayActivationRequested", null },
                { "GameplayInactivationRequested", null }
            };

            // set different menu state 
            this._gameplayState = new System.Collections.Generic.Dictionary<GameplayIDType, bool>
            {
                { GameplayIDType.None, false },
                { GameplayIDType.WorldElement, false },
                { GameplayIDType.SimulatedRobosoftEntity, false },
                { GameplayIDType.RealRobosoftEntity, false },
                { GameplayIDType.ActiveSeatEntity, false },
                { GameplayIDType.CAVIAREntity, false },
                { GameplayIDType.HOTASWarthogThrottleSensor, false },
                { GameplayIDType.RadioSondeSensor, false },
                { GameplayIDType.FogElement, false },
                { GameplayIDType.All, false }
            };

            // registering
            this.RegisterAllAvailableGameplayBridge();

        } /* ApollonGameplayManager() */

        #endregion

        #region private member/method

        private readonly System.Collections.Generic.Dictionary<GameplayIDType, bool> _gameplayState
            = new System.Collections.Generic.Dictionary<GameplayIDType, bool>();

        #endregion

        #region public member/method

        public enum GameplayIDType
        {

            [System.ComponentModel.Description("None")]
            None = 0,

            [System.ComponentModel.Description("WorldElement")]
            WorldElement,

            [System.ComponentModel.Description("FogElement")]
            FogElement,

            [System.ComponentModel.Description("SimulatedRobosoftEntity")]
            SimulatedRobosoftEntity,

            [System.ComponentModel.Description("RealRobosoftEntity")]
            RealRobosoftEntity,

            [System.ComponentModel.Description("ActiveSeatEntity")]
            ActiveSeatEntity,

            [System.ComponentModel.Description("CAVIAREntity")]
            CAVIAREntity,

            [System.ComponentModel.Description("HOTASWarthogThrottleSensor")]
            HOTASWarthogThrottleSensor,

            [System.ComponentModel.Description("RadioSondeSensor")]
            RadioSondeSensor,

            [System.ComponentModel.Description("All")]
            All

        } /* enum */

        public bool IsActive(GameplayIDType gameplayID)
        {

            lock (this._gameplayState)
            {

                if (this._gameplayState.ContainsKey(gameplayID))
                {

                    return this._gameplayState[gameplayID];

                }
                else
                {

                    UnityEngine.Debug.LogWarning(
                        "<color=orange>Warning: </color> ApollonGameplayManager.IsActive("
                        + gameplayID
                        + ") : requested gameplayID is not intantiated yet..."
                    );
                    return false;

                } /* if() */

            } /* lock() */

        } /* IsActive() */

        public void setActive(GameplayIDType gameplayID)
        {

            lock (this._gameplayState)
            {

                if (this._gameplayState.ContainsKey(gameplayID))
                {

                    if (this.IsActive(gameplayID))
                    {
                        UnityEngine.Debug.LogWarning(
                            "<color=orange>Warning: </color> ApollonGameplayManager.setActive("
                            + gameplayID
                            + ") : requested gameplayID is already active, skip"
                        );
                        return;
                    }

                    this._gameplayState[gameplayID] = true;
                    this.RaiseGameplayActivationRequestedEvent(gameplayID);

                }
                else
                {

                    UnityEngine.Debug.LogWarning(
                        "<color=orange>Warning: </color> ApollonGameplayManager.setActive("
                        + gameplayID
                        + ") : requested gameplayID is not intantiated yet..."
                    );
                    return;

                } /* if() */

            } /* lock() */

        } /* setActive() */

        public void setInactive(GameplayIDType gameplayID)
        {
            lock (this._gameplayState)
            {

                if (this._gameplayState.ContainsKey(gameplayID))
                {

                    if (!this.IsActive(gameplayID))
                    {
                        UnityEngine.Debug.LogWarning(
                            "<color=orange>Warning: </color> ApollonGameplayManager.setInactive("
                            + gameplayID
                            + ") : requested gameplayID is already inactive, skip"
                        );
                        return;
                    }

                    this._gameplayState[gameplayID] = false;
                    this.RaisGameplayInactivationRequestedEvent(gameplayID);

                }
                else
                {

                    UnityEngine.Debug.LogWarning(
                        "<color=orange>Warning: </color> ApollonGameplayManager.setInactive("
                        + gameplayID
                        + ") : requested menuID is not intantiated yet..."
                    );
                    return;

                } /* if() */

            } /* lock() */

        } /* setInactive() */

        public System.Collections.Generic.IEnumerable<GameplayIDType> getAllActive()
        {

            return (
                this._gameplayState
                    .Where(item => item.Value == true)
                    .Select(item => item.Key)
            );

        } /* getAllActive() */

        public System.Collections.Generic.IEnumerable<GameplayIDType> getAllInactive()
        {

            return (
                this._gameplayState
                    .Select(item => item.Key)
                    .Except(this.getAllActive())
            );

        } /* getAllInactive() */

        #endregion

        #region abstract manager implementation

        public override void onStart(object sender, ApollonEngine.EngineEventArgs arg)
        {

            // default 
            this.setActive(GameplayIDType.None);

        } /* onStart() */

        #endregion

    } /* class ApollonGameplayManager */

} /* namespace Labsim.apollon.ui */
