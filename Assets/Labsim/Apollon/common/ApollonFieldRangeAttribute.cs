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
namespace Labsim.apollon.common
{

    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class ApollonFieldRangeAttribute
        : System.Attribute
    {

        private float m_min = 0.0f;
        private float m_max = 0.0f;

        public virtual float Min
        {
            get 
            {
                return this.m_min;
            }
            set
            {
                this.m_min = value;
            }
        }

        public virtual float Max
        {
            get 
            {
                return this.m_max;
            }
            set
            {
                this.m_max = value;
            }
        }

    } /* class ApollonFieldRangeAttribute */

} /* namespace Labsim.apollon.common */ 
