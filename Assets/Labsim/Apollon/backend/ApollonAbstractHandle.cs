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

// avoid namespace pollution
namespace Labsim.apollon.backend
{

    public abstract class ApollonAbstractHandle
        : System.IDisposable
    {

        // Handle ID field
        protected ApollonBackendManager.HandleIDType m_handleID = ApollonBackendManager.HandleIDType.None;
        public ApollonBackendManager.HandleIDType ID
        {
            get
            {
                return this.m_handleID;
            }
        }

        // ctor
        public ApollonAbstractHandle()
        { }
        
        // Dispose pattern
        public void Dispose()
        {

            this.Dispose(true);
            System.GC.SuppressFinalize(this);

        } /* Dispose() */

        protected abstract void Dispose(bool bDisposing = true);

        #region event handling 

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