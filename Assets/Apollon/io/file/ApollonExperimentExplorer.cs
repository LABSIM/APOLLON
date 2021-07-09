
// avoid namespace pollution
namespace Labsim.apollon.io.file
{ 

    public class ApollonExperimentExplorer
    {

        public static System.Collections.Generic.List<string> ListAvailable(string basepath)
        {
            // instantiate
            System.Collections.Generic.List<string> availableConfigurationList = new System.Collections.Generic.List<string>();

            foreach (var cfgFile in System.IO.Directory.GetFiles(basepath, "ApollonInput.xml", System.IO.SearchOption.AllDirectories))
            {

                // get info
                System.IO.FileInfo cfgFileInfo = new System.IO.FileInfo(cfgFile);

                // build relative path

                System.Uri 
                    from = new System.Uri(cfgFileInfo.DirectoryName),
                    to   = new System.Uri(System.IO.Path.GetFullPath(basepath));

                availableConfigurationList.Add(
                    System.Uri.UnescapeDataString(
                        to.MakeRelativeUri(from).ToString()
                    )
                );

                // log
                UnityEngine.Debug.Log(
                    "<color=blue>Info: </color> ApollonExperimentExplorer.ListAvailable() : found ["
                    + System.Linq.Enumerable.Last(availableConfigurationList)
                    + "]"
                );

            } /* foreach() */

          
            // return 
            return availableConfigurationList;

        } /* ListAvailable */

    } /* class ApollonExperimentExplorer */

} /* namespace Labsim.apollon.io.file */ 
