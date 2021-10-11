// System using derective 
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

// avoid namespace pollution
namespace Labsim.apollon.io.datamodel
{

    [XmlRoot("Apollon")]
    public class ApollonOutputFileImpl
    {

        [XmlElement(ElementName = "configuration_full_filename")]
        public string m_configurationFullFilename = "";

        [XmlElement(ElementName = "application_start_time")]
        public string m_applicationStartTime = "";

        [XmlElement(ElementName = "application_stop_time")]
        public string m_applicationStopTime = "";

    } /* class ApollonOutputFileImpl */

} /* namespace Labsim.apollon.io.datamodel */
