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

// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// avoid namespace pollution
namespace Labsim.experiment.AIRWISE
{
    public class AIRWISEEntityTracker
        : UXF.Tracker
    {

        public override string MeasurementDescriptor => "EntityTracker";

        public override System.Collections.Generic.IEnumerable<string> CustomHeader 
            => new string[] { 
                ""
            };

        protected override UXF.UXFDataRow GetCurrentValues()
        {
           
            // return it
            return new UXF.UXFDataRow()
            {
                ("","")
            };

        } /* GetCurrentValues() */

    } /* public sealed class AIRWISEEntityTracker */

} /* } Labsim.experiment.AIRWISE */