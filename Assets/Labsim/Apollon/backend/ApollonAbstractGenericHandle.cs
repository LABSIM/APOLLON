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

    // default
    public abstract class ApollonAbstractGenericHandle
        : ApollonAbstractStandardHandle
    { 

        #region Concrete HandleInit/HandleClose pattern def.

        protected abstract StatusIDType ConcreteHandleInitialize();
        protected abstract StatusIDType ConcreteHandleClose();

        #endregion

        #region Standard HandleInit/HandleClose pattern impl.

        protected sealed override StatusIDType HandleInitialize()
        {            
            
            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAbstractGenericHandle.HandleInitialize() : call"
            );

            // encapsulate
            try
            {

                // return concrete result
                return this.ConcreteHandleInitialize();

            }
            catch (System.Exception ex)
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonAbstractGenericHandle.HandleInitialize() : caugh Exception => "
                    + ex.Message
                );

            } /* try() */

            // whatever, fail
            UnityEngine.Debug.LogWarning(
                "<color=orange>Warning: </color> ApollonAbstractGenericHandle.HandleInitialize() : Initialization failed..."
            );
            return StatusIDType.Status_ERROR;

        } /* HandleInitialize() */

        protected sealed override StatusIDType HandleClose()
        {

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonAbstractGenericHandle.HandleClose() : call"
            );

            // encapsulate
            try
            {

                // return concrete result
                return this.ConcreteHandleClose();

            }
            catch (System.Exception ex)
            {

                UnityEngine.Debug.LogError(
                    "<color=red>Error: </color> ApollonAbstractGenericHandle.HandleClose() : caugh Exception => "
                    + ex.Message
                );

            } /* try() */

            // whatever, fail
            UnityEngine.Debug.LogWarning(
                "<color=orange>Warning: </color> ApollonAbstractGenericHandle.HandleClose() : Closure failed..."
            );
            return StatusIDType.Status_ERROR;

        } /* HandleClose() */

        #endregion

    } /* ApollonAbstractGenericHandle */ 
    
} /* } namespace */