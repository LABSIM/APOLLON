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
namespace Labsim.apollon
{
    public abstract class ApollonAbstractManager
    {

        // Callback methods

        public virtual void onStart(object sender, ApollonEngine.EngineEventArgs arg)
        {
            /* nothing by default */
        }

        public virtual void onAwake(object sender, ApollonEngine.EngineEventArgs arg)
        {
            /* nothing by default */
        }

        public virtual void onUpdate(object sender, ApollonEngine.EngineEventArgs arg)
        {
            /* nothing by default */
        }

        public virtual void onFixedUpdate(object sender, ApollonEngine.EngineEventArgs arg)
        {
            /* nothing by default */
        }

        public virtual void onSceneLoaded(object sender, ApollonEngine.EngineSceneEventArgs arg)
        {
            /* nothing by default */
        }

        public virtual void onSceneUnloaded(object sender, ApollonEngine.EngineSceneEventArgs arg)
        {
            /* nothing by default */
        }

        public virtual void onExperimentSessionBegin(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {
            /* nothing by default */
        }

        public virtual void onExperimentSessionEnd(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {
            /* nothing by default */
        }

        public virtual void onExperimentTrialBegin(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {
            /* nothing by default */
        }

        public virtual void onExperimentTrialEnd(object sender, ApollonEngine.EngineExperimentEventArgs arg)
        {
            /* nothing by default */
        }

    } /* abstract ApollonAbstractManager */

} /* namespace Labsim.apollo */