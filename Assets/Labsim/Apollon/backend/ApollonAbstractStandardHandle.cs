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

namespace Labsim.apollon.backend
{

    public abstract class ApollonAbstractStandardHandle
        : ApollonAbstractHandle
    {

        #region Standard Init/Close pattern abstract decl.

        protected abstract StatusIDType HandleInitialize();

        protected abstract StatusIDType HandleClose();

        #endregion

        #region Event handling override impl.

        public sealed override void OnHandleActivationRequested(object sender, ApollonBackendManager.EngineHandleEventArgs arg)
        {

            // check
            if (this.ID == arg.HandleID)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAbstractStandardHandle.OnHandleActivationRequested() : Initializing handle"
                );

                // skip if necessary
                if(this.IsInitialized)
                {
                
                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warn: </color> ApollonAbstractStandardHandle.OnHandleActivationRequested() : handle is already initialized..."
                    );
                    
                } /* if() */

                // init proc
                StatusIDType 
                    status   = StatusIDType.Status_ERROR, 
                    overflow = StatusIDType.None;
                if ((status = this.HandleInitialize()) != StatusIDType.Status_OK)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonAbstractStandardHandle.OnHandleActivationRequested() : failed to initialize, exit"
                    );

                    // abort
                    this.Dispose();

                } /* if() */
                this.InternalStatus.Enqueue(StatusIDType.State_Run | status);
                while(this.InternalStatus.Count > s_status_history_depth && this.InternalStatus.TryDequeue(out overflow)) {};
                // pull-up
                base.OnHandleActivationRequested(sender, arg);

            } /* if() */

        } /* OnHandleActivationRequested() */

        // unregistration
        public sealed override void OnHandleDeactivationRequested(object sender, ApollonBackendManager.EngineHandleEventArgs arg)
        {

            // check
            if (this.ID == arg.HandleID)
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonAbstractStandardHandle.OnHandleDeactivationRequested() : Closing handle"
                );

                // skip if necessary
                if(this.IsClosed)
                {
                
                    // log
                    UnityEngine.Debug.LogWarning(
                        "<color=Orange>Warn: </color> ApollonAbstractStandardHandle.OnHandleActivationRequested() : handle is already closed..."
                    );
                    
                } /* if() */

                // close
                StatusIDType 
                    status   = StatusIDType.Status_ERROR, 
                    overflow = StatusIDType.None;
                if ((status = this.HandleClose()) != StatusIDType.Status_OK)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonAbstractStandardHandle.OnHandleDeactivationRequested() : failed to close, exit"
                    );

                    // abort
                    this.Dispose();

                } /* if() */
                this.InternalStatus.Enqueue(StatusIDType.State_Idle | status);
                while(this.InternalStatus.Count > s_status_history_depth && this.InternalStatus.TryDequeue(out overflow)) {};

                // pull-up
                base.OnHandleDeactivationRequested(sender, arg);

            } /* if() */

        } /* OnHandleDeactivationRequested() */

        #endregion

    } /* class ApollonAbstractStandardHandle */

    
} /* } namespace */