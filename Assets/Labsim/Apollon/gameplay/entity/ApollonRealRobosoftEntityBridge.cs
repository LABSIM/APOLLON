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
namespace Labsim.apollon.gameplay.entity
{

    public class ApollonRealRobosoftEntityBridge 
        : ApollonGameplayBridge<ApollonRealRobosoftEntityBridge>
    {

        // ctor
        public ApollonRealRobosoftEntityBridge()
            : base()
        { }
        
        public ApollonRealRobosoftEntityBehaviour ConcreteBehaviour 
            => this.Behaviour as ApollonRealRobosoftEntityBehaviour;

        #region Bridge abstract implementation 
        
        protected override ApollonGameplayBehaviour WrapBehaviour()
        {

            return this.WrapBehaviour<ApollonRealRobosoftEntityBehaviour>(
                "ApollonRealRobosoftEntityBridge",
                "ApollonRealRobosoftEntityBehaviour"
            );

        } /* WrapBehaviour() */

        protected override ApollonGameplayDispatcher WrapDispatcher()
        {

            return this.WrapDispatcher<ApollonGameplayDispatcher>(
                "ApollonRealRobosoftEntityBridge",
                "ApollonGameplayDispatcher"
            );

        } /* WrapDispatcher() */

        protected override ApollonGameplayManager.GameplayIDType WrapID()
        {
            return ApollonGameplayManager.GameplayIDType.RealRobosoftEntity;
        }

        protected override void SetActive(bool value)
        {

            // escape
            if (this.Behaviour == null) { return; }

            // switch
            if (value)
            {

                // escape
                if (this.Behaviour.isActiveAndEnabled) { return; }

                // activate
                this.Behaviour.gameObject.SetActive(true);
               
            }
            else
            {

                // escape
                if (!this.Behaviour.isActiveAndEnabled) { return; }

                // inactivate
                this.Behaviour.gameObject.SetActive(false);

            } /* if() */

        } /* SetActive() */
        
        #endregion

    }  /* class ApollonRealRobosoftEntityBridge */

} /* } Labsim.apollon.gameplay.entity */