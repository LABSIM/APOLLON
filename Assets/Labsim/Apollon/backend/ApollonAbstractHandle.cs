//
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

// using
using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon.backend
{

    public abstract class ApollonAbstractHandle
        : System.IDisposable
    {

        #region StatusID section

        public enum StatusIDType
        {
            
            [System.ComponentModel.Description("None")]
            None = 0,

            State_Idle     = 1 << 0,
            State_Run      = 1 << 1,
            
            Status_OK      = 1 << 2,
            Status_WARN    = 1 << 3,
            Status_ERROR   = 1 << 4,

            Idling         = State_Idle | Status_OK,
            Running        = State_Run  | Status_OK,

        } /* enum */

        protected static readonly int s_status_history_depth = 2; 
        protected System.Collections.Generic.Queue<StatusIDType> InternalStatus { get; set; } = new(new[]{ StatusIDType.Idling, StatusIDType.Idling});

        public StatusIDType CurrentStatus  => this.InternalStatus.Last();
        public StatusIDType PreviousStatus => this.InternalStatus.First();

        public bool IsInitialized => this.CurrentStatus == StatusIDType.Running && this.PreviousStatus == StatusIDType.Idling;
        public bool IsClosed      => this.CurrentStatus == StatusIDType.Idling  && this.PreviousStatus == StatusIDType.Running;
        public bool IsOK          => (this.CurrentStatus & StatusIDType.Status_OK)    == StatusIDType.Status_OK;
        public bool HasWarn       => (this.CurrentStatus & StatusIDType.Status_WARN)  == StatusIDType.Status_WARN;
        public bool HasError      => (this.CurrentStatus & StatusIDType.Status_ERROR) == StatusIDType.Status_ERROR;

        #endregion

        #region HandleID section
        
        protected abstract ApollonBackendManager.HandleIDType WrapID();
        private ApollonBackendManager.HandleIDType m_ID = ApollonBackendManager.HandleIDType.None;
        public ApollonBackendManager.HandleIDType ID
        {
            get
            {
                if (this.m_ID == ApollonBackendManager.HandleIDType.None)
                {

                    // log
                    UnityEngine.Debug.Log(
                        "<color=Blue>Info: </color> ApollonAbstractHandle.ID : property is uninitilized, trying to wrap ID."
                    );

                    this.m_ID = this.WrapID();
                }
                return this.m_ID;
            }
            private set
            {
                this.m_ID = value;
            }
        }
        
        #endregion

        #region Dispose pattern decl.

        public void Dispose()
        {

            this.Dispose(true);
            System.GC.SuppressFinalize(this);

        } /* Dispose() */

        protected abstract void Dispose(bool bDisposing = true);
        
        #endregion

        #region Event handling decl.

        public virtual void OnHandleActivationRequested(object sender, ApollonBackendManager.EngineHandleEventArgs arg)
        {

            // check
            if (this.ID == arg.HandleID)
            {

                // validate
                ApollonBackendManager.Instance.ValidateHandle(this.ID, this);

            } /* if() */

        } /* OnHandleActivationRequested() */

        // unregistration
        public virtual void OnHandleDeactivationRequested(object sender, ApollonBackendManager.EngineHandleEventArgs arg)
        {

            // check
            if (this.ID == arg.HandleID)
            {

                // unplug - comment: it let the system doing it's job
                //this.Dispose();

                // unvalidate
                ApollonBackendManager.Instance.InvalidateHandle(this.ID, this);

            } /* if() */

        } /* OnHandleDeactivationRequested() */

        #endregion

    } /* class ApollonAbstractInteroperabilityHandle */

} /* } namespace */