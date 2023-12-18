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
namespace Labsim.apollon.frontend.menu
{

    public class ApollonFileSelectionMenuBehaviour : UnityEngine.MonoBehaviour
    {

        // The game object content
        public UnityEngine.GameObject buttonPrefab;
        public UnityEngine.GameObject parentObject;

        // private member

        // because dictionnary does not garantee to be ordered by instertion
        private System.Collections.Generic.List<string> m_loadedOrder 
            = new System.Collections.Generic.List<string>();
        private System.Collections.Generic.Dictionary<string, UnityEngine.GameObject> m_loadedObject
            = new System.Collections.Generic.Dictionary<string, UnityEngine.GameObject>();

	    // Use this for initialization
        void Start() 
        {

	    } /* Start() */

        // Update is called once per frame
        void Update()
        {

        } /*  Update() */

        // OnEnable
        void OnEnable()
        {

            bool bOnce = true;

            // build up buttons
            foreach (string entry in io.ApollonIOManager.Instance.ListAvailableExperiment())
            {

                // is entry an already loaded button ?
                if (!this.m_loadedObject.ContainsKey(entry))
                {

                    // instantiate
                    UnityEngine.GameObject button = UnityEngine.GameObject.Instantiate(this.buttonPrefab);

                    // attach
                    button.transform.SetParent(this.parentObject.transform,false);

                    // add button click delegate
                    button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => this.OnListButtonClick(entry));

                    // add cancel trigger event delegate
                    UnityEngine.EventSystems.EventTrigger.Entry cancelEntry = new UnityEngine.EventSystems.EventTrigger.Entry();
                    cancelEntry.eventID = UnityEngine.EventSystems.EventTriggerType.Cancel;
                    cancelEntry.callback.AddListener( this.OnBackButtonClick );
                    button.GetComponent<UnityEngine.EventSystems.EventTrigger>().triggers.Add(cancelEntry);

                    // then configure
                    button.GetComponentInChildren<UnityEngine.UI.Text>().text = entry;
                    button.SetActive(true);

                    // add to the loaded element
                    this.m_loadedObject.Add(entry, button);
                    this.m_loadedOrder.Add(entry);
                
                } /* if() */

            } /* for() */

            // set selected to the first element if list are not empty
            if (bOnce
                && this.m_loadedOrder.Count != 0
                && this.m_loadedObject.Count !=0
            ) {
                this.gameObject.GetComponentInChildren<UnityEngine.EventSystems.EventSystem>().firstSelectedGameObject
                    = this.m_loadedObject[this.m_loadedOrder[0]];
                bOnce = false;
            }

        } /* OnEnable() */

        // load seleected content & advance
        public void OnListButtonClick(string filename)
        {
            io.ApollonIOManager.Instance.LoadInput(filename);
            ApollonFrontendManager.Instance.setActive(ApollonFrontendManager.FrontendIDType.OptionMenu);
            ApollonFrontendManager.Instance.setInactive(ApollonFrontendManager.FrontendIDType.FileSelectionMenu);
        }

        // load seleected content & rewind
        public void OnBackButtonClick()
        {
            ApollonFrontendManager.Instance.setActive(ApollonFrontendManager.FrontendIDType.MainMenu);
            ApollonFrontendManager.Instance.setInactive(ApollonFrontendManager.FrontendIDType.FileSelectionMenu);
        }

        public void OnBackButtonClick(UnityEngine.EventSystems.BaseEventData eventData)
        {
            this.OnBackButtonClick();
        }

    } /* class ApollonFileSelectionMenuBehaviour() */

} /* } Labsim.apollon.frontend.menu */

