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

    public class ApollonDynamicProperty
    {
        private readonly System.Collections.Generic.List<object> m_metadata = new System.Collections.Generic.List<object>();
        public virtual System.Collections.Generic.List<object> Metadata
        {
            get
            {
                return this.m_metadata;
            }
        }

        private object m_data = null;
        public virtual object Data
        {
            get
            {
                return this.m_data;
            }
            set
            {
                this.m_data = value;
            }
        }

        private ApollonFieldRangeAttribute m_metadataRange = null;
        public ApollonFieldRangeAttribute Range
        {
            get
            {
                return this.m_metadataRange;
            }
            private set
            {
                this.m_metadataRange = value;
            }
        }

        public ApollonDynamicProperty(object data, System.Collections.Generic.List<object> metadata = null)
        {
            this.m_data = data;
            if (metadata != null)
            {
                this.m_metadata = metadata;

                // iterate
                foreach (object meta_property in this.Metadata)
                {
                    // check range
                    ApollonFieldRangeAttribute range = meta_property as ApollonFieldRangeAttribute;
                    if (range != null)
                    {
                        this.Range = range;
                    }

                } /* foreach() */

            } /* if() */

        } /* ApollonDynamicProperty() */

    } /* class ApollonDynamicProperty */

} /* } Labsim.apollon.common */