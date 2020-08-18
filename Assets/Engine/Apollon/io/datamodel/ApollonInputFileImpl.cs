// System using derective 
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

// avoid namespace pollution
namespace Labsim.apollon.io.datamodel
{ 

    [XmlRoot("Apollon")]
    public class ApollonInputFileImpl
    {
        // general

        [XmlElement(ElementName = "mode_supervised")]
        public bool m_modeSupervised;

        [common.ApollonFieldRange(Min = 0.0f, Max = 100.0f)]
        [XmlElement(ElementName = "recording_frequency")]
        public uint m_recordingFrequency = 60;

        // stabilisation

        [common.ApollonFieldRange(Min = 100.0f, Max = 10000.0f)]
        [XmlElement(ElementName = "phase_stabilisation_timeout")]
        public double m_phaseStabilisationTimeout = 200.0;

        [common.ApollonFieldRange(Min = 100.0f, Max = 10000.0f)]
        [XmlElement(ElementName = "phase_stabilisation_target")]
        public double m_phaseStabilisationTarget = 200.0;

        [common.ApollonFieldRange(Min = 100.0f, Max = 10000.0f)]
        [XmlElement(ElementName = "phase_stabilisation_threshold")]
        public double m_phaseStabilisationThreshold = 200.0;

        // stabilisation event

        [common.ApollonFieldRange(Min = 100.0f, Max = 10000.0f)]
        [XmlElement(ElementName = "event_stabilisation_spawn")]
        public double m_eventStabilisationSpawn = 200.0;

        [common.ApollonFieldRange(Min = 100.0f, Max = 10000.0f)]
        [XmlElement(ElementName = "event_stabilisation_timeout")]
        public double m_eventStabilisationTimeout = 200.0;

        // test
        
        [common.ApollonFieldRange(Min = 100.0f, Max = 10000.0f)]
        [XmlElement(ElementName = "phase_test_timeout")]
        public double m_phaseTestTimeout = 200.0;

        // test event
        
        [common.ApollonFieldRange(Min = 100.0f, Max = 10000.0f)]
        [XmlElement(ElementName = "event_test_spawn")]
        public double m_eventTestSpawn = 200.0;
        
        [common.ApollonFieldRange(Min = 100.0f, Max = 10000.0f)]
        [XmlElement(ElementName = "event_test_timeout")]
        public double m_eventTestTimeout = 200.0;

        [common.ApollonFieldRange(Min = 100.0f, Max = 10000.0f)]
        [XmlElement(ElementName = "event_test_value")]
        public double m_eventTestValue = 200.0;

        // user input

        [common.ApollonFieldRange(Min = 0.0f, Max = 100.0f)]
        [XmlElement(ElementName = "user_input_threshold")]
        public uint m_userInputThreshold = 50;

        [common.ApollonFieldRange(Min = 0.0f, Max = 100.0f)]
        [XmlElement(ElementName = "user_input_max")]
        public uint m_userInputMax = 50;

        // user interface

        [common.ApollonFieldRange(Min = 0.0f, Max = 1000.0f)]
        [XmlElement(ElementName = "user_interface_frame_size_x")]
        public uint m_userInterfaceFrameSixeX = 200;

        [common.ApollonFieldRange(Min = 0.0f, Max = 1000.0f)]
        [XmlElement(ElementName = "user_interface_frame_size_y")]
        public uint m_userInterfaceFrameSixeY = 200;

        [common.ApollonFieldRange(Min = -1000.0f, Max = 1000.0f)]
        [XmlElement(ElementName = "user_interface_frame_position_x")]
        public int m_userInterfaceFramePositionX = 0;

        [common.ApollonFieldRange(Min = -1000.0f, Max = 1000.0f)]
        [XmlElement(ElementName = "user_interface_frame_position_y")]
        public int m_userInterfaceFramePositiionY = 0;

        [common.ApollonFieldRange(Min = 0.0f, Max = 1000.0f)]
        [XmlElement(ElementName = "user_interface_canvas_distance")]
        public uint m_userInterfaceCanvasDistance = 200;

    } /* public class ApollonInputFileImpl  */

} /* namespace Labsim.apollon.io.datamodel */ 
