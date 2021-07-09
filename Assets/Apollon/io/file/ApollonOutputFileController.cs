
// avoid namespace pollution
namespace Labsim.apollon.io.file
{ 

    static class ApollonOutputFileController
    {

        static public ApollonOutputFileFacade Create()
        {
            ApollonOutputFileFacade facade = new ApollonOutputFileFacade(new datamodel.ApollonOutputFileImpl());
            facade.Checkout();
            return facade;
        }

        static public ApollonOutputFileFacade Load(string filename)
        {

            try
            {

                datamodel.ApollonOutputFileImpl impl = new datamodel.ApollonOutputFileImpl();
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(impl.GetType());
                System.IO.StreamReader reader = new System.IO.StreamReader(filename);
                impl = (datamodel.ApollonOutputFileImpl)serializer.Deserialize(reader);
                ApollonOutputFileFacade facade = new ApollonOutputFileFacade(impl);
                facade.Checkout();
                reader.Close();
                return facade;

            }
            catch (System.Exception ex)
            {

                UnityEngine.Debug.LogError("<color=red>Error: </color> ApollonOutputFileController::Load(" + filename + ") [" + ex.Message + "]");
                return null;

            }

        } /* Load() */

        static public bool Save(string filename, ApollonOutputFileFacade facade)
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
            catch (System.Exception ex)
            {

                UnityEngine.Debug.Log("<color=red>Error: </color> ApollonOutputFileController::Save(" + filename + ") [" + ex.Message + "]");
                return false;

            }

        } /* Save() */

    } /* static class ApollonOutputFileController */

} /* namespace Labsim.apollon.io.file */ 
