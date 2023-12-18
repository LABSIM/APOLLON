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
// using System.Xml;
// using System.Xml.Serialization;
// using System.Xml.Schema;

// avoid namespace pollution
namespace Labsim.apollon.io.datamodel
{ 

    [System.Xml.Serialization.XmlRoot("Apollon")]
    public class ApollonInputFileImpl
    {
        // general

        [System.Xml.Serialization.XmlElement(ElementName = "mode_supervised")]
        public bool m_modeSupervised;

        [common.ApollonFieldRange(Min = 0.0f, Max = 100.0f)]
        [System.Xml.Serialization.XmlElement(ElementName = "recording_frequency")]
        public uint m_recordingFrequency = 60;

        // stabilisation

        [common.ApollonFieldRange(Min = 100.0f, Max = 10000.0f)]
        [System.Xml.Serialization.XmlElement(ElementName = "phase_stabilisation_timeout")]
        public double m_phaseStabilisationTimeout = 200.0;

        [common.ApollonFieldRange(Min = 100.0f, Max = 10000.0f)]
        [System.Xml.Serialization.XmlElement(ElementName = "phase_stabilisation_target")]
        public double m_phaseStabilisationTarget = 200.0;

        [common.ApollonFieldRange(Min = 100.0f, Max = 10000.0f)]
        [System.Xml.Serialization.XmlElement(ElementName = "phase_stabilisation_threshold")]
        public double m_phaseStabilisationThreshold = 200.0;

        // stabilisation event

        [common.ApollonFieldRange(Min = 100.0f, Max = 10000.0f)]
        [System.Xml.Serialization.XmlElement(ElementName = "event_stabilisation_spawn")]
        public double m_eventStabilisationSpawn = 200.0;

        [common.ApollonFieldRange(Min = 100.0f, Max = 10000.0f)]
        [System.Xml.Serialization.XmlElement(ElementName = "event_stabilisation_timeout")]
        public double m_eventStabilisationTimeout = 200.0;

        // test
        
        [common.ApollonFieldRange(Min = 100.0f, Max = 10000.0f)]
        [System.Xml.Serialization.XmlElement(ElementName = "phase_test_timeout")]
        public double m_phaseTestTimeout = 200.0;

        // test event
        
        [common.ApollonFieldRange(Min = 100.0f, Max = 10000.0f)]
        [System.Xml.Serialization.XmlElement(ElementName = "event_test_spawn")]
        public double m_eventTestSpawn = 200.0;
        
        [common.ApollonFieldRange(Min = 100.0f, Max = 10000.0f)]
        [System.Xml.Serialization.XmlElement(ElementName = "event_test_timeout")]
        public double m_eventTestTimeout = 200.0;

        [common.ApollonFieldRange(Min = 100.0f, Max = 10000.0f)]
        [System.Xml.Serialization.XmlElement(ElementName = "event_test_value")]
        public double m_eventTestValue = 200.0;

        // user input

        [common.ApollonFieldRange(Min = 0.0f, Max = 100.0f)]
        [System.Xml.Serialization.XmlElement(ElementName = "user_input_threshold")]
        public uint m_userInputThreshold = 50;

        [common.ApollonFieldRange(Min = 0.0f, Max = 100.0f)]
        [System.Xml.Serialization.XmlElement(ElementName = "user_input_max")]
        public uint m_userInputMax = 50;

        // user interface

        [common.ApollonFieldRange(Min = 0.0f, Max = 1000.0f)]
        [System.Xml.Serialization.XmlElement(ElementName = "user_interface_frame_size_x")]
        public uint m_userInterfaceFrameSixeX = 200;

        [common.ApollonFieldRange(Min = 0.0f, Max = 1000.0f)]
        [System.Xml.Serialization.XmlElement(ElementName = "user_interface_frame_size_y")]
        public uint m_userInterfaceFrameSixeY = 200;

        [common.ApollonFieldRange(Min = -1000.0f, Max = 1000.0f)]
        [System.Xml.Serialization.XmlElement(ElementName = "user_interface_frame_position_x")]
        public int m_userInterfaceFramePositionX = 0;

        [common.ApollonFieldRange(Min = -1000.0f, Max = 1000.0f)]
        [System.Xml.Serialization.XmlElement(ElementName = "user_interface_frame_position_y")]
        public int m_userInterfaceFramePositiionY = 0;

        [common.ApollonFieldRange(Min = 0.0f, Max = 1000.0f)]
        [System.Xml.Serialization.XmlElement(ElementName = "user_interface_canvas_distance")]
        public uint m_userInterfaceCanvasDistance = 200;

    } /* public class ApollonInputFileImpl  */

} /* namespace Labsim.apollon.io.datamodel */ 
