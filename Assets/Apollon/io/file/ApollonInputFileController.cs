
// avoid namespace pollution
namespace Labsim.apollon.io.file
{ 

    static class ApollonInputFileController
    {

        static public ApollonInputFileFacade Create()
        {
            ApollonInputFileFacade facade = new ApollonInputFileFacade(new datamodel.ApollonInputFileImpl());
            facade.Checkout();
            return facade;
        }

        static public ApollonInputFileFacade Load(string filename)
        {

            try
            {

                datamodel.ApollonInputFileImpl impl = new datamodel.ApollonInputFileImpl();
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(impl.GetType());
                System.IO.StreamReader reader = new System.IO.StreamReader(filename);
                impl = (datamodel.ApollonInputFileImpl)serializer.Deserialize(reader);
                ApollonInputFileFacade facade = new ApollonInputFileFacade(impl);
                facade.Checkout();
                reader.Close();
                return facade;

            }
            catch(System.Exception ex)
            {

                UnityEngine.Debug.LogError("<color=red>Error: </color> ApollonInputFileController::Load(" + filename + ") [" + ex.Message + "]");
                return null;

            }

        } /* Load() */

        static public bool Save(string filename, ApollonInputFileFacade facade)
        {

            try
            {

                facade.Commit();
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(facade.Implementation.GetType());
                System.IO.StreamWriter writer = new System.IO.StreamWriter(filename, false, System.Text.Encoding.UTF8);
                serializer.Serialize(writer, facade.Implementation);
                writer.Close();
                return true;

            }
            catch(System.Exception ex)
            {
            
                UnityEngine.Debug.Log("<color=red>Error: </color> ApollonInputFileController::Save(" + filename + ") [" + ex.Message + "]");
                return false;

            }

        } /* Save() */

    } /* static class ApollonInputFileController */

} /* namespace Labsim.apollon.io.file */ 
