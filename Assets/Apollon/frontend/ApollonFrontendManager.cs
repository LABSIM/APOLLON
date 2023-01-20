
// extensions
using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.frontend
{
  
    public sealed class ApollonFrontendManager : ApollonAbstractManager
    {

        #region frontend bridge handling

        private readonly System.Collections.Generic.Dictionary<FrontendIDType, ApollonAbstractFrontendBridge> _frontendBridgeTable
            = new System.Collections.Generic.Dictionary<FrontendIDType, ApollonAbstractFrontendBridge>();

        private void RegisterAllAvailableFrontendBridge()
        {

            // clear if it's already filled
            if(this._frontendBridgeTable.Count > 0)
            {

                foreach(ApollonAbstractFrontendBridge entry in this._frontendBridgeTable.Values)
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonFrontendManager.RegisterAllAvailableGameplayBridge() : unbinding entry ["
                            + Labsim.apollon.ApollonEngine.GetEnumDescription(entry.ID)
                        + "]"
                    );
                    
                    // unmap
                    this.FrontendActivationRequestedEvent   -= entry.onActivationRequested;
                    this.FrontendInactivationRequestedEvent -= entry.onInactivationRequested;       

                } /* foreach() */

                // log
                UnityEngine.Debug.Log(
                    "<color=blue>Info: </color> ApollonFrontendManager.RegisterAllAvailableGameplayBridge() : clear all table entries"
                );

                // clear all
                this._frontendBridgeTable.Clear();

            } /* if() */

            // register all module
            foreach (System.Type type
                in System.Reflection.Assembly.GetAssembly(typeof(ApollonAbstractFrontendBridge)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(ApollonAbstractFrontendBridge)))
            )
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=blue>Info: </color> ApollonFrontendManager.RegisterAllAvailableFrontendBridge() : found bridge ["
                    + type.FullName
                    + "]."
                );

                // get "Instance" property
                ApollonAbstractFrontendBridge bridge = null;
                if ((bridge = System.Activator.CreateInstance(type) as ApollonAbstractFrontendBridge) != null)
                {

                    // is it a prefab ? is it attached to a GO ? is it on current scene ?

                    bool 
                        isPrefabInstance             = false,
                        isPrefabOriginal             = false, 
                        isDisconnectedPrefabInstance = false,
                        isAttached                   = bridge.Behaviour != null && bridge.Behaviour.transform != null,
                        isInLoadedScene
                            = isAttached 
                            ? (
                                bridge.Behaviour.gameObject.scene != null 
                                ? UnityEngine.SceneManagement.SceneManager.GetSceneByName(bridge.Behaviour.gameObject.scene.name).IsValid()
                                : false
                            )
                            : false;
                    
                    #if UNITY_EDITOR
                    if (UnityEngine.Application.isEditor && isAttached) 
                    {

                        isPrefabInstance 
                            = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(bridge.Behaviour.gameObject)   != null 
                            && UnityEditor.PrefabUtility.GetPrefabInstanceHandle(bridge.Behaviour.transform) != null;

                        isPrefabOriginal 
                            = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(bridge.Behaviour.gameObject)   == null 
                            && UnityEditor.PrefabUtility.GetPrefabInstanceHandle(bridge.Behaviour.transform) != null;

                        isDisconnectedPrefabInstance 
                            = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(bridge.Behaviour.gameObject)   != null 
                            && UnityEditor.PrefabUtility.GetPrefabInstanceHandle(bridge.Behaviour.transform) == null;

                    } /* if() */
                    #endif

                    // log
                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonFrontendManager.RegisterAllAvailableGameplayBridge() : bridge ["
                        + type.FullName
                        + "] isPrefabInstance{"
                        + isPrefabInstance
                        + "}, isPrefabOriginal{"
                        + isPrefabOriginal
                        + "}, isDisconnectedPrefabInstance{"
                        + isDisconnectedPrefabInstance
                        + "}, isAttached{"
                        + isAttached
                        + "}, isInLoadedScene{"
                        + isInLoadedScene + "/" + ((isAttached && (bridge.Behaviour.gameObject.scene != null)) ? bridge.Behaviour.gameObject.scene.name : "null")
                        + "}"
                    );

                    // check behaviour property, if null or if there is no attached scene then object... is not in current unity Scene, it's a prefab
                    if(!isAttached
                    #if UNITY_EDITOR
                    || !isInLoadedScene
                    #endif
                    ) {
                        
                        // mark as disposable
                        System.GC.SuppressFinalize(bridge);

                        // log
                        UnityEngine.Debug.Log(
                            "<color=blue>Info: </color> ApollonFrontendManager.RegisterAllAvailableGameplayBridge() : bridge ["
                            + type.FullName
                            + "] not in current Scene, mark as disposable."
                        );

                        // next module 
                        continue;

                    } /* if() */

                    // log
                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonFrontendManager.RegisterAllAvailableFrontendBridge() : success to create instance of bridge ["
                        + type.FullName
                        + "]."
                    );

                    // // check behaviour property, if null then object isn'nt in unity Scene
                    // if (bridge.Behaviour == null)
                    // {

                    //     // mark as disposable
                    //     System.GC.SuppressFinalize(bridge);

                    //     // log
                    //     UnityEngine.Debug.Log(
                    //         "<color=blue>Info: </color> ApollonFrontendManager.RegisterAllAvailableFrontendBridge() : bridge ["
                    //         + type.FullName
                    //         + "] not in current Scene, mark as dispobale."
                    //     );

                    //     // next module 
                    //     continue;

                    // } /* if() */

                    // register it

                    // check
                    if (this._frontendBridgeTable.ContainsKey(bridge.ID))
                    {

                        UnityEngine.Debug.LogError(
                             "<color=red>Error: </color> ApollonFrontendManager.RegisterAllAvailableFrontendBridge("
                             + bridge.ID
                             + ") : bridge key already found ..."
                        );
                        return;

                    } /* if() */

                    // add to table
                    this._frontendBridgeTable.Add(bridge.ID, bridge);

                    // plug module
                    this.FrontendActivationRequestedEvent += bridge.onActivationRequested;
                    this.FrontendInactivationRequestedEvent += bridge.onInactivationRequested;

                    // log
                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonFrontendManager.RegisterAllAvailableFrontendBridge() : bridge ["
                        + bridge.ID
                        + "], registration ok !"
                    );

                }
                else
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonFrontendManager.RegisterAllAvailableFrontendBridge() : could not create instance of gameobject ["
                        + type.FullName
                        + "]. fail."
                    );

                } /* if() */

            } /* foreach() */

        } /* RegisterAllAvailableFrontendBridge() */

        public ApollonAbstractFrontendBridge getBridge(FrontendIDType ID)
        {

            // check 
            if (this._frontendBridgeTable.ContainsKey(ID))
            {
                return this._frontendBridgeTable[ID];
            }

            // log
            UnityEngine.Debug.LogWarning(
                 "<color=orange>Warning: </color> ApollonFrontendManager.getBridge("
                 + ID
                 + ") : requested ID not found ..."
            );

            // failed
            return null;

        } /* getBridge() */

        public T getConcreteBridge<T>(FrontendIDType ID)
            where T : ApollonAbstractFrontendBridge
        {

            // check 
            if(this._frontendBridgeTable.ContainsKey(ID))
            {

                if(this._frontendBridgeTable[ID] is T)
                {
                    return this._frontendBridgeTable[ID] as T;
                }
                else
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=orange>Warning: </color> ApollonFrontendManager.getConcreteBridge<"
                        + typeof(T).ToString()
                        + ">("
                        + ID
                        + ") : bridge isn't the requested generic argument type..."
                    );

                } /* if() */

            }
            else
            {

                // log
                UnityEngine.Debug.LogWarning(
                    "<color=orange>Warning: </color> ApollonFrontendManager.getConcreteBridge<"
                    + typeof(T).ToString()
                    + ">("
                    + ID
                    + ") : requested ID not found ..."
                );

            } /* if() */

            // failed
            return null;

        } /* getConcreteBridge() */

        #endregion

        #region event handling

        // Dictionary & each list event

        private readonly System.Collections.Generic.Dictionary<string, System.Delegate> _eventTable;

        private readonly System.Collections.Generic.List<System.EventHandler<FrontendEventArgs>> _eventFrontendActivationRequestedList
            = new System.Collections.Generic.List<System.EventHandler<FrontendEventArgs>>();

        private readonly System.Collections.Generic.List<System.EventHandler<FrontendEventArgs>> _eventFrontendInactivationRequestedList
            = new System.Collections.Generic.List<System.EventHandler<FrontendEventArgs>>();
        
        // the actual event 

        public event System.EventHandler<FrontendEventArgs> FrontendActivationRequestedEvent
        {
            add
            {
                this._eventFrontendActivationRequestedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["FrontendActivationRequested"] = (System.EventHandler<FrontendEventArgs>)this._eventTable["FrontendActivationRequested"] + value;
                }
            }

            remove
            {
                if (!this._eventFrontendActivationRequestedList.Contains(value))
                {
                    return;
                }
                this._eventFrontendActivationRequestedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["FrontendActivationRequested"] = null;
                    foreach (var eventFrontendActivationRequested in this._eventFrontendActivationRequestedList)
                    {
                        this._eventTable["FrontendActivationRequested"] = (System.EventHandler<FrontendEventArgs>)this._eventTable["FrontendActivationRequested"] + eventFrontendActivationRequested;
                    }
                }
            }

        } /* FrontendLoadedEvent */

        public event System.EventHandler<FrontendEventArgs> FrontendInactivationRequestedEvent
        {
            add
            {
                this._eventFrontendInactivationRequestedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["FrontendInactivationRequested"] = (System.EventHandler<FrontendEventArgs>)this._eventTable["FrontendInactivationRequested"] + value;
                }
            }

            remove
            {
                if (!this._eventFrontendInactivationRequestedList.Contains(value))
                {
                    return;
                }
                this._eventFrontendInactivationRequestedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["FrontendInactivationRequested"] = null;
                    foreach (var eventFrontendInactivationRequested in this._eventFrontendInactivationRequestedList)
                    {
                        this._eventTable["FrontendInactivationRequested"] = (System.EventHandler<FrontendEventArgs>)this._eventTable["FrontendInactivationRequested"] + eventFrontendInactivationRequested;
                    }
                }
            }

        } /* FrontendUnloadedEvent */

        internal void RaiseFrontendActivationRequestedEvent(FrontendIDType frontendID)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<FrontendEventArgs>)this._eventTable["FrontendActivationRequested"];
                callback?.Invoke(this, new FrontendEventArgs(frontendID));
            }
        }

        internal void RaisFrontendInactivationRequestedEvent(FrontendIDType frontendID)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<FrontendEventArgs>)this._eventTable["FrontendInactivationRequested"];
                callback?.Invoke(this, new FrontendEventArgs(frontendID));
            }
        }

        // the event args class
        public class FrontendEventArgs
            : ApollonEngine.EngineEventArgs
        {

            // ctor
            public FrontendEventArgs(FrontendIDType categoryID = FrontendIDType.None, string groupID = "", int instanceID = -1)
                : base()
            {
                this.ID = categoryID;
            }

            // ctor
            public FrontendEventArgs(FrontendEventArgs rhs)
                : base(rhs)
            {
                this.ID = rhs.ID;
            }

            // property
            public FrontendIDType ID { get; protected set; }
            public string Group { get; protected set; }

        } /* FrontendEventArgs() */

        #endregion

        #region lazy init singleton pattern

        // lazy init paradigm
        private static readonly System.Lazy<ApollonFrontendManager> _lazyInstance
            = new System.Lazy<ApollonFrontendManager>(() => new ApollonFrontendManager());

        // Instance  property
        public static ApollonFrontendManager Instance { get { return _lazyInstance.Value; } }

        // private ctor
        private ApollonFrontendManager()
        {
            // event table
            this._eventTable = new System.Collections.Generic.Dictionary<string, System.Delegate>
            {
                { "FrontendActivationRequested", null },
                { "FrontendInactivationRequested", null }
            };

            // set different menu state 
            this._frontendState = new System.Collections.Generic.Dictionary<FrontendIDType, bool>
            {
                { FrontendIDType.None, false }, 
                { FrontendIDType.MainMenu, false },
                { FrontendIDType.OptionMenu, false },
                { FrontendIDType.FileSelectionMenu, false },
                { FrontendIDType.ConfigureMenu, false },
                { FrontendIDType.SpeedSelectionGUI, false },
                { FrontendIDType.ResponseGUI, false },
                { FrontendIDType.GreenCrossGUI, false },
                { FrontendIDType.RedCrossGUI, false },
                { FrontendIDType.GreyCrossGUI, false },
                { FrontendIDType.SimpleCrossGUI, false },
                { FrontendIDType.GreenFrameGUI, false },
                { FrontendIDType.RedFrameGUI, false },
                { FrontendIDType.GreyFrameGUI, false },
                { FrontendIDType.RemainingTrialPoolCounterGUI, false },
                { FrontendIDType.SimpleFrameGUI, false },
                { FrontendIDType.VerticalAnchorableDock, false },
                { FrontendIDType.ResponseSliderGUI, false },
                { FrontendIDType.IntensitySliderGUI, false },
                { FrontendIDType.ConfidenceSliderGUI, false },
                { FrontendIDType.All, false }
            };

            // registering
            this.RegisterAllAvailableFrontendBridge();

        } /* ApollonFrontendManager() */

        #endregion

        #region private member/method

        private readonly System.Collections.Generic.Dictionary<FrontendIDType, bool> _frontendState
            = new System.Collections.Generic.Dictionary<FrontendIDType, bool>();

        #endregion

        #region public member/method

        public enum FrontendIDType
        {

            [System.ComponentModel.Description("None")]
            None = 0,

            [System.ComponentModel.Description("MainMenu")]
            MainMenu,

            [System.ComponentModel.Description("OptionMenu")]
            OptionMenu,

            [System.ComponentModel.Description("FileSelectionMenu")]
            FileSelectionMenu,

            [System.ComponentModel.Description("ConfigureMenu")]
            ConfigureMenu,

            [System.ComponentModel.Description("SpeedSelectionGUI")]
            SpeedSelectionGUI,

            [System.ComponentModel.Description("ResponseGUI")]
            ResponseGUI,

            [System.ComponentModel.Description("GreenCrossGUI")]
            GreenCrossGUI,

            [System.ComponentModel.Description("RedCrossGUI")]
            RedCrossGUI,

            [System.ComponentModel.Description("GreyCrossGUI")]
            GreyCrossGUI,
            
            [System.ComponentModel.Description("SimpleCrossGUI")]
            SimpleCrossGUI,

            [System.ComponentModel.Description("GreenFrameGUI")]
            GreenFrameGUI,

            [System.ComponentModel.Description("RedFrameGUI")]
            RedFrameGUI,

            [System.ComponentModel.Description("GreyFrameGUI")]
            GreyFrameGUI,  
            
            [System.ComponentModel.Description("RemainingTrialPoolCounterGUI")]
            RemainingTrialPoolCounterGUI,
            
            [System.ComponentModel.Description("SimpleFrameGUI")]
            SimpleFrameGUI,

            [System.ComponentModel.Description("VerticalAnchorableDock")]
            VerticalAnchorableDock,
                        
            [System.ComponentModel.Description("ResponseSliderGUI")]
            ResponseSliderGUI,

            [System.ComponentModel.Description("IntensitySliderGUI")]
            IntensitySliderGUI,

            [System.ComponentModel.Description("ConfidenceSliderGUI")]
            ConfidenceSliderGUI,

            [System.ComponentModel.Description("All")]
            All

        } /* enum */

        public enum FrontendAnchorIDType
        {
            None = 0,
            EgoCentric = 1,
            ExoCentric = 2
        }

        public bool IsActive(FrontendIDType frontendID)
        {

            lock (this._frontendState)
            {

                if (this._frontendState.ContainsKey(frontendID))
                {

                    return this._frontendState[frontendID];
                
                }
                else
                {

                    UnityEngine.Debug.LogWarning(
                        "<color=orange>Warning: </color> ApollonFrontendManager.IsActive("
                        + frontendID
                        + ") : requested frontendID is not intantiated yet..."
                    );
                    return false;

                } /* if() */

            } /* lock() */

        } /* IsActive() */

        public void setActive(FrontendIDType frontendID)
        {

            lock (this._frontendState)
            {

                if (this._frontendState.ContainsKey(frontendID))
                {

                    if (this.IsActive(frontendID))
                    {
                        UnityEngine.Debug.LogWarning(
                            "<color=orange>Warning: </color> ApollonFrontendManager.setActive("
                            + frontendID
                            + ") : requested frontendID is already active, skip"
                        );
                        return;
                    }

                    this._frontendState[frontendID] = true;
                    this.RaiseFrontendActivationRequestedEvent(frontendID);

                }
                else
                {

                    UnityEngine.Debug.LogWarning(
                        "<color=orange>Warning: </color> ApollonFrontendManager.setActive("
                        + frontendID
                        + ") : requested frontendID is not intantiated yet..."
                    );
                    return;

                } /* if() */
            
            } /* lock() */

        } /* setActive() */

        public void setInactive(FrontendIDType frontendID)
        {
            lock (this._frontendState)
            {

                if (this._frontendState.ContainsKey(frontendID))
                {

                    if (!this.IsActive(frontendID))
                    {
                        UnityEngine.Debug.LogWarning(
                            "<color=orange>Warning: </color> ApollonFrontendManager.setInactive("
                            + frontendID
                            + ") : requested frontendID is already inactive, skip"
                        );
                        return;
                    }

                    this._frontendState[frontendID] = false;
                    this.RaisFrontendInactivationRequestedEvent(frontendID);

                }
                else
                {

                    UnityEngine.Debug.LogWarning(
                        "<color=orange>Warning: </color> ApollonFrontendManager.setInactive("
                        + frontendID
                        + ") : requested menuID is not intantiated yet..."
                    );
                    return;

                } /* if() */

            } /* lock() */

        } /* setInactive() */

        public System.Collections.Generic.IEnumerable<FrontendIDType> getAllActive()
        {

            return (
                this._frontendState
                    .Where(item => item.Value == true)
                    .Select(item => item.Key)
            );

        } /* getAllActive() */

        public System.Collections.Generic.IEnumerable<FrontendIDType> getAllInactive()
        {

            return (
                this._frontendState
                    .Select(item => item.Key)
                    .Except(this.getAllActive())
            );

        } /* getAllInactive() */

        #endregion

        #region abstract manager implementation

        public override void onStart(object sender, ApollonEngine.EngineEventArgs arg)
        {

            // default 
            this.setActive(FrontendIDType.None);

        } /* onStart() */

        #endregion

    } /* class ApollonFrontendManager */

} /* namespace Labsim.apollon.ui */
