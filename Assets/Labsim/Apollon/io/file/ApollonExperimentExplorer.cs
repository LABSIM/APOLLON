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
