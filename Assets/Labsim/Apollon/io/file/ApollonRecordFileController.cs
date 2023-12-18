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
    static class ApollonRecordFileController
    {

        static public ApollonRecordFileFacade Create()
        {
            ApollonRecordFileFacade facade = new ApollonRecordFileFacade(new datamodel.ApollonRecordFileImpl());
            facade.Checkout();
            return facade;
        }

        static public ApollonRecordFileFacade Load(string filename)
        {

            try
            {

                datamodel.ApollonRecordFileImpl impl = new datamodel.ApollonRecordFileImpl();
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(impl.GetType());
                System.IO.StreamReader reader = new System.IO.StreamReader(filename);
                impl = (datamodel.ApollonRecordFileImpl)serializer.Deserialize(reader);
                ApollonRecordFileFacade facade = new ApollonRecordFileFacade(impl);
                facade.Checkout();
                reader.Close();
                return facade;

            }
            catch (System.Exception ex)
            {

                UnityEngine.Debug.LogError("<color=red>Error: </color> ApollonRecordFileController::Load(" + filename + ") [" + ex.Message + "]");
                return null;

            }

        } /* Load() */

        static public bool Save(string filename, ApollonRecordFileFacade facade)
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

                UnityEngine.Debug.Log("<color=red>Error: </color> ApollonRecordFileController::Save(" + filename + ") [" + ex.Message + "]");
                return false;

            }

        } /* Save() */

    } /* static class ApollonRecordFileController */

} /* namespace Labsim.apollon.io.file */
