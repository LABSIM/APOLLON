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

namespace Labsim.apollon.gameplay
{

    //
    // FSM CRTP
    //  -- Finite State Machine Curiously Recurring Template Pattern
    //
    public abstract class ApollonGameplayBridge<T> 
        : ApollonAbstractGameplayBridge 
        where T : ApollonGameplayBridge<T>
    {

        // properties
        public ApollonAbstractGameplayState<T> State { get; protected set; } = null;

        protected async System.Threading.Tasks.Task SetState(ApollonAbstractGameplayState<T> next_state)
        {

            // escape iff. same
            if (next_state == this.State) return;

            // first, OnExit on previous
            if (this.State != null)
            {
                await this.State.OnExit();
            }

            // assign
            this.State = next_state;

            // then, OnEntry on fresh
            if (this.State != null)
            {
                await this.State.OnEntry();
            }
            
        } /* SetState() */

    } /* abstract class ApollonGameplayBridge */

} /* namespace Labsim.apollon.gameplay */
