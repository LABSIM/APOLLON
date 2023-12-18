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
namespace Labsim.apollon.frontend.gui
{

    public class ApollonResponseGUIBehaviour : UnityEngine.MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        void OnEnable()
        {
            //gameObject.GetComponent<Hover.InterfaceModules.Panel.HoverpanelRowTransitioner>().OnRowSwitched(Hover.InterfaceModules.Panel.HoverpanelRowSwitchingInfo.RowEntryType.SlideFromBottom);
        }

        void OnDisable()
        {
            //gameObject.GetComponent<Hover.InterfaceModules.Panel.HoverpanelRowTransitioner>().TransitionProgress = 0.0f;
        }

        //public void OnItemSelected(Hover.Core.Items.Types.IItemDataSelectable item)
        //{

        //    // follow call !
        //    (experiment.ApollonExperimentManager.Instance.Profile as experiment.profile.ApollonAgencyAndTBWExperimentProfile).DoRespondToUserResponse(item.Id);

        //} /* OnItemSelected() */

    } /* public class ApollonResponseGUIBehaviour */

} /* } Labsim.apollon.frontend.gui */