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