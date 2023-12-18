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
namespace Labsim.apollon.gameplay
{

    public class ApollonGameplayDispatcher
    {
        
        // root
        public ApollonAbstractGameplayBridge Bridge { get; set; }

        #region event args class

        public class GameplayEventArgs
            : ApollonEngine.EngineEventArgs
        {

            // ctor
            public GameplayEventArgs()
                : base()
            {

            }

            // ctor
            public GameplayEventArgs(GameplayEventArgs rhs)
                : base(rhs)
            {
            }

        } /* GameplayEventArgs() */

        #endregion

        #region Dictionary & each list of event

        protected readonly System.Collections.Generic.Dictionary<string, System.Delegate>
            _eventTable = new System.Collections.Generic.Dictionary<string, System.Delegate>();

        #endregion

    }  /* abstract ApollonGameplayDispatcher */

    public abstract class ApollonConcreteGameplayDispatcher<T> 
        : ApollonGameplayDispatcher
        where T : ApollonAbstractGameplayBridge
    {

        public T ConcreteBridge => this.Bridge as T;
        
    }  /* abstract generic ApollonConcreteGameplayDispatcher */

} /* } Labsim.apollon.gameplay */