// extensions
using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.backend
{

    public sealed class ApollonBackendManager 
        : ApollonAbstractManager
    {

        #region handle management
        
        public enum HandleIDType
        {
            
            [System.ComponentModel.Description("All")]
            None = 0,

            [System.ComponentModel.Description("Robulab Robot")]
            ApollonRobulabHandle = 1,

            [System.ComponentModel.Description("ISM - Active Seat")]
            ApollonISMActiveSeatHandle = 2,

            [System.ComponentModel.Description("Motion System PS-6TM-550")]
            ApollonMotionSystemPS6TM550Handle = 3

        } /* enum*/

        private readonly System.Collections.Generic.Dictionary<HandleIDType, (bool, ApollonAbstractHandle)> _handleState
            = new System.Collections.Generic.Dictionary<HandleIDType, (bool, ApollonAbstractHandle)>();
        
        private void RegisterAllAvailableHandle<T>()
        {
            
            // TODO handle multiple loading system as other Manager but be carefull with T

            // register all native backend handle
            foreach (System.Type type
                in System.Reflection.Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(
                    myType => myType.IsClass
                    && !myType.IsAbstract
                    && !myType.IsGenericType
                    && myType.IsSubclassOf(typeof(T))
                )
            )
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=blue>Info: </color> ApollonBackendManager.RegisterAllAvailableHandle<" + typeof(T) + ">() : found handle ["
                    + type.FullName
                    + "]."
                );

                // get "Instance" property
                ApollonAbstractHandle handle = null;
                if ((handle = System.Activator.CreateInstance(type) as ApollonAbstractHandle) != null)
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonBackendManager.RegisterAllAvailableHandle<" + typeof(T) + ">() : success to create instance of handle ["
                        + type.FullName
                        + "]."
                    );

                    // check
                    System.Enum.TryParse(type.Name, out HandleIDType handleID);
                    if (this._handleState.ContainsKey(handleID))
                    {

                        UnityEngine.Debug.LogError(
                             "<color=red>Error: </color> ApollonBackendManager.RegisterAllAvailableHandle<" + typeof(T) + ">("
                             + type.FullName
                             + ") : profile key already found ..."
                        );
                        return;

                    } /* if() */

                    // initialize
                    this._handleState[handleID] = (false, handle);

                    // bind event
                    this.HandleActivationRequestedEvent += handle.OnHandleActivationRequested;
                    this.HandleDeactivationRequestedEvent += handle.OnHandleDeactivationRequested;

                    // log
                    UnityEngine.Debug.Log(
                        "<color=blue>Info: </color> ApollonBackendManager.RegisterAllAvailableHandle<" + typeof(T) + ">() : handle ["
                        + type.FullName
                        + "], registration ok !"
                    );

                }
                else
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonBackendManager.RegisterAllAvailableHandle<" + typeof(T) + ">() : could not create instance ["
                        + type.FullName
                        + "]. fail."
                    );

                } /* if() */

            } /* foreach() */

        } /* RegisterAllAvailableHandle<T>() */

        private void RegisterAllAvailableHandle()
        {

            this.RegisterAllAvailableHandle<ApollonAbstractNativeHandle>();
            this.RegisterAllAvailableHandle<ApollonAbstractTCPStreamHandle>();
            this.RegisterAllAvailableHandle<ApollonAbstractDefaultHandle>();
            
        } /* RegisterAllAvailableHandle() */

        public ApollonAbstractHandle GetValidHandle(HandleIDType ID)
        {

            // check 
            if (this._handleState.ContainsKey(ID))
            {

                if (this._handleState[ID].Item1 && this._handleState[ID].Item2 != null)
                {

                    // valid !
                    return this._handleState[ID].Item2;

                }
                else
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                         "<color=orange>Warning: </color> ApollonBackendManager.GetValidHandle("
                         + ApollonEngine.GetEnumDescription(ID)
                         + ") : requested ID is not valid..."
                    );

                } /* if() */

            } 
            else
            {
                
                // log
                UnityEngine.Debug.LogWarning(
                     "<color=orange>Warning: </color> ApollonBackendManager.GetValidHandle("
                     + ApollonEngine.GetEnumDescription(ID)
                     + ") : requested ID not found ..."
                );

            } /* if() */

            // failed
            return null;

        } /* GetHandle() */

        public void ValidateHandle(HandleIDType ID, ApollonAbstractHandle handle)
        {

            // check 
            if (this._handleState.ContainsKey(ID))
            {

                // if inactive && same ref
                if (!this._handleState[ID].Item1 && this._handleState[ID].Item2 == handle)
                {

                    // then, validate
                    this._handleState[ID] = (true, handle);

                }
                else
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                         "<color=orange>Warning: </color> ApollonBackendManager.ValidateHandle("
                         + ApollonEngine.GetEnumDescription(ID)
                         + ") : requested ID is already registered..."
                    );

                } /* if() */

            }
            else
            {

                // log
                UnityEngine.Debug.LogWarning(
                     "<color=orange>Warning: </color> ApollonBackendManager.ValidateHandle("
                     + ApollonEngine.GetEnumDescription(ID)
                     + ") : requested ID not found ..."
                );

            } /* if() */

            // failed
            return;

        } /* ValidateHandle() */

        public void InvalidateHandle(HandleIDType ID, ApollonAbstractHandle handle)
        {

            // check 
            if (this._handleState.ContainsKey(ID))
            {

                if (this._handleState[ID].Item1 && this._handleState[ID].Item2 == handle)
                {

                    // unregister
                    this._handleState[ID] = (false, handle);

                }
                else
                {

                    // log
                    UnityEngine.Debug.LogWarning(
                         "<color=orange>Warning: </color> ApollonBackendManager.InvalidateHandle("
                         + ApollonEngine.GetEnumDescription(ID)
                         + ") : requested args aren't corresponding to internal settings..."
                    );

                } /* if() */

            }
            else
            {

                // log
                UnityEngine.Debug.LogWarning(
                     "<color=orange>Warning: </color> ApollonBackendManager.InvalidateHandle("
                     + ApollonEngine.GetEnumDescription(ID)
                     + ") : requested ID not found ..."
                );

            } /* if() */
            
            // failed
            return;

        } /* InvalidateHandle() */

        #endregion

        #region lazy init singleton pattern

        // lazy init paradigm
        private static readonly System.Lazy<ApollonBackendManager> _lazyInstance
            = new System.Lazy<ApollonBackendManager>(() => new ApollonBackendManager());

        // Instance  property
        public static ApollonBackendManager Instance { get { return _lazyInstance.Value; } }

        // private ctor
        private ApollonBackendManager()
        {

            this._eventTable = new System.Collections.Generic.Dictionary<string, System.Delegate>
            {
                { "HandleActivationRequested", null },
                { "HandleDeactivationRequested", null }
            };

            this._handleState = new System.Collections.Generic.Dictionary<HandleIDType, (bool, ApollonAbstractHandle)>
            {
                //{ HandleIDType.ApollonRobulabHandle, (false, null) },
                //{ HandleIDType.ApollonActiveSeatHandle, (false, null) }
            };

            // registering
            this.RegisterAllAvailableHandle();

        } /* ApollonBackendManager() */

        #endregion
        
        #region event handling

        private readonly System.Collections.Generic.Dictionary<string, System.Delegate> _eventTable;

        private readonly System.Collections.Generic.List<System.EventHandler<EngineHandleEventArgs>>
            _eventHandleActivationRequestedList = new System.Collections.Generic.List<System.EventHandler<EngineHandleEventArgs>>(),
            _eventHandleDeactivationRequestedList = new System.Collections.Generic.List<System.EventHandler<EngineHandleEventArgs>>();

        public event System.EventHandler<EngineHandleEventArgs> HandleActivationRequestedEvent
        {
            add
            {
                this._eventHandleActivationRequestedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["HandleActivationRequested"] = (System.EventHandler<EngineHandleEventArgs>)this._eventTable["HandleActivationRequested"] + value;
                }
            }

            remove
            {
                if (!this._eventHandleActivationRequestedList.Contains(value))
                {
                    return;
                }
                this._eventHandleActivationRequestedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["HandleActivationRequested"] = null;
                    foreach (var eventHandleActivationRequested in this._eventHandleActivationRequestedList)
                    {
                        this._eventTable["HandleActivationRequested"] = (System.EventHandler<EngineHandleEventArgs>)this._eventTable["HandleActivationRequested"] + eventHandleActivationRequested;
                    }
                }
            }

        } /* HandleActivationRequestedEvent */

        public event System.EventHandler<EngineHandleEventArgs> HandleDeactivationRequestedEvent
        {
            add
            {
                this._eventHandleDeactivationRequestedList.Add(value);
                lock (this._eventTable)
                {
                    this._eventTable["HandleDeactivationRequested"] = (System.EventHandler<EngineHandleEventArgs>)this._eventTable["HandleDeactivationRequested"] + value;
                }
            }

            remove
            {
                if (!this._eventHandleDeactivationRequestedList.Contains(value))
                {
                    return;
                }
                this._eventHandleDeactivationRequestedList.Remove(value);
                lock (this._eventTable)
                {
                    this._eventTable["HandleDeactivationRequested"] = null;
                    foreach (var eventHandleDeactivationRequested in this._eventHandleDeactivationRequestedList)
                    {
                        this._eventTable["HandleDeactivationRequested"] = (System.EventHandler<EngineHandleEventArgs>)this._eventTable["HandleDeactivationRequested"] + eventHandleDeactivationRequested;
                    }
                }
            }

        } /* HandleDeactivationRequestedEvent */

        internal void RaiseHandleActivationRequestedEvent(HandleIDType handleID)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EngineHandleEventArgs>)this._eventTable["HandleActivationRequested"];
                callback?.Invoke(this, new EngineHandleEventArgs(handleID));
            }
        }

        internal void RaiseHandleDeactivationRequestedEvent(HandleIDType handleID)
        {
            lock (this._eventTable)
            {
                var callback = (System.EventHandler<EngineHandleEventArgs>)this._eventTable["HandleDeactivationRequested"];
                callback?.Invoke(this, new EngineHandleEventArgs(handleID));
            }
        }
        public class EngineHandleEventArgs
            : ApollonEngine.EngineEventArgs
        {

            // ctor
            public EngineHandleEventArgs(HandleIDType handleID)
                : base()
            {
                this.HandleID = handleID;
            }

            // property
            public HandleIDType HandleID { get; private set; } = HandleIDType.None;

        } /* EngineHandleEventArgs */

        #endregion
        
    } /* class ApollonBackendManager */

} /* namespace Labsim.apollon.backend */
