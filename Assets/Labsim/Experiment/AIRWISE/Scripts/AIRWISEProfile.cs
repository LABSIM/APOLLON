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

using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.AIRWISE
{

    public class AIRWISEProfile 
        : apollon.experiment.ApollonAbstractExperimentFiniteStateMachine< AIRWISEProfile >
    {

        // Ctor
        public AIRWISEProfile()
            : base()
        {
            // default profile
            this.m_profileID = apollon.experiment.ApollonExperimentManager.ProfileIDType.AIRWISE;
        }

        // properties
        public AIRWISESettings CurrentSettings { get; private set; } = null;
        public AIRWISEResults CurrentResults { get; set; } = null;

        // infos
        protected override System.String getCurrentStatusInfo()
        {

            return (
                "[" + apollon.ApollonEngine.GetEnumDescription(this.ID) + "]" 
                + "\n" 
                + this.CurrentSettings.Trial.pattern_type
                + "|"
                + apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.Trial.control_type)
                + "|"
                + apollon.ApollonEngine.GetEnumDescription(this.CurrentSettings.Trial.scene_type)
            );

        } /* getCurrentStatusInfo() */

        protected override System.String getCurrentCounterStatusInfo()
        {

            return (
                (this.CurrentSettings.Trial.bIsActive) 
                ? ( 
                    (
                        (UXF.Session.instance.blocks.Count > 1) 
                            ? (UXF.Session.instance.CurrentBlock.number + "/" + UXF.Session.instance.blocks.Count + " | ")
                            : ""
                    )
                )
                : ""
            );

        } /* getCurrentCounterStatusInfo() */

        public System.String CurrentInstruction { get; set; } = "";
        protected override System.String getCurrentInstructionStatusInfo()
        {

            // simply
            return this.CurrentInstruction;

        } /* getCurrentInstructionStatusInfo() */

    } /* class AIRWISEProfile */
    
} /* } Labsim.experiment.AIRWISE */