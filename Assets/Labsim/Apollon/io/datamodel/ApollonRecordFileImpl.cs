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

// System using derective 
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

// avoid namespace pollution
namespace Labsim.apollon.io.datamodel
{

    [XmlRoot("Apollon")]
    public class ApollonRecordFileImpl
    {

        public class Sample
        {

            [XmlElement(ElementName = "software_elapsed_time")]
            public double m_softwareElapsedTime = 0.0;

            [XmlElement(ElementName = "software_phase")]
            public string m_softwarePhase = "";

            [XmlElement(ElementName = "user_raw_input_right_trigger")]
            public uint m_userRawInputRightTrigger = 0;

            [XmlElement(ElementName = "user_raw_input_left_trigger")]
            public uint m_userRawInputLeftTrigger = 0;

            [XmlElement(ElementName = "system_localisation_x")]
            public double m_systemLocalisationX = 0.0;

            [XmlElement(ElementName = "system_localisation_Y")]
            public double m_systemLocalisationY = 0.0;

        } /* class Sample */

        // sample list 
        [XmlArray("Record")]
        [XmlArrayItem("Sample")]
        public System.Collections.Generic.List<Sample> m_record;

        // Ctor
        public ApollonRecordFileImpl()
        {
            this.m_record = new System.Collections.Generic.List<Sample>();
        }

    } /* public class ApollonRecordFileImpl  */

} /* namespace Labsim.apollon.io.datamodel */