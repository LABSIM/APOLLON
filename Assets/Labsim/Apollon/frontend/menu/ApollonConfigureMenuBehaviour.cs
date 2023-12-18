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

    public class ApollonConfigureMenuBehaviour : UnityEngine.MonoBehaviour
    {

        // The game object content
        public UnityEngine.GameObject parentObject;
        public UnityEngine.GameObject buttonPrefab;
        public UnityEngine.GameObject sliderPrefab;
        public UnityEngine.GameObject togglePrefab;

        // because dictionnary does not garantee to be ordered by instertion
        private System.Collections.Generic.List<string> m_loadedOrder
            = new System.Collections.Generic.List<string>();
        private System.Collections.Generic.Dictionary<string, UnityEngine.GameObject> m_loadedObject
            = new System.Collections.Generic.Dictionary<string, UnityEngine.GameObject>();

	    // Use this for initialization
        void Start()
        {
		
	    }

        // Update is called once per frame
        void Update()
        {

        }

        // OnEnable
        void OnEnable()
        {

            bool bOnce = true;

            // build up buttons
            foreach (string key in io.ApollonIOManager.Instance.Input.GetKeys())
            {
                // is entry an already loaded item ?
                if (!this.m_loadedObject.ContainsKey(key))
                {

                    // Switch on type
                    common.ApollonTypeSwitch.Do(
                        
                        // get data
                        io.ApollonIOManager.Instance.Input.GetData(key),

                        // case : boolean
                        common.ApollonTypeSwitch.Case<bool>(() => {

                            // instantiate
                            UnityEngine.GameObject toggle = UnityEngine.GameObject.Instantiate(this.togglePrefab);

                            // attach
                            toggle.transform.SetParent(this.parentObject.transform,false);

                            // initalize
                            toggle.GetComponentInChildren<UnityEngine.UI.Toggle>().isOn = (bool)io.ApollonIOManager.Instance.Input.GetData(key);

                            // add button click delegate
                            toggle.GetComponentInChildren<UnityEngine.UI.Toggle>().onValueChanged.AddListener(
                                (value) => { io.ApollonIOManager.Instance.Input.SetData(key, value); }
                            );

                            // add cancel trigger event delegate
                            UnityEngine.EventSystems.EventTrigger.Entry cancelEntry = new UnityEngine.EventSystems.EventTrigger.Entry();
                            cancelEntry.eventID = UnityEngine.EventSystems.EventTriggerType.Cancel;
                            cancelEntry.callback.AddListener(this.OnBackButtonClick);
                            toggle.GetComponent<UnityEngine.EventSystems.EventTrigger>().triggers.Add(cancelEntry);

                            // then configure
                            toggle.GetComponentInChildren<UnityEngine.UI.Text>().text = key;
                            toggle.SetActive(true);

                            // add to the loaded element
                            this.m_loadedObject.Add(key, toggle);
                            this.m_loadedOrder.Add(key);

                        }),
                        common.ApollonTypeSwitch.Case<int>(() => {

                            // instantiate
                            UnityEngine.GameObject slider = UnityEngine.GameObject.Instantiate(this.sliderPrefab);

                            // attach
                            slider.transform.SetParent(this.parentObject.transform, false);

                            // configure 

                            slider.GetComponentInChildren<UnityEngine.UI.Slider>().value =
                                System.Convert.ToInt32(io.ApollonIOManager.Instance.Input.GetData(key));

                            if (io.ApollonIOManager.Instance.Input.GetRangeMetadata(key) != null)
                            {
                                slider.GetComponentInChildren<UnityEngine.UI.Slider>().minValue =
                                    io.ApollonIOManager.Instance.Input.GetRangeMetadata(key).Min;
                                slider.GetComponentInChildren<UnityEngine.UI.Slider>().maxValue =
                                     io.ApollonIOManager.Instance.Input.GetRangeMetadata(key).Max;
                            }

                            slider.GetComponentInChildren<UnityEngine.UI.InputField>().text = 
                                System.Convert.ToString(io.ApollonIOManager.Instance.Input.GetData(key));

                            // add delegate

                            // slider -> data
                            slider.GetComponentInChildren<UnityEngine.UI.Slider>().onValueChanged.AddListener(
                                (value) => { io.ApollonIOManager.Instance.Input.SetData(key, System.Convert.ToInt32(value)); }
                            );
                            // slider -> input field 
                            slider.GetComponentInChildren<UnityEngine.UI.Slider>().onValueChanged.AddListener(
                                (value) => { slider.GetComponentInChildren<UnityEngine.UI.InputField>().text = System.Convert.ToString(System.Convert.ToInt32(value)); }
                            );
                            // input field -> data
                            slider.GetComponentInChildren<UnityEngine.UI.InputField>().onValueChanged.AddListener(
                                (value) => { io.ApollonIOManager.Instance.Input.SetData(key, System.Convert.ToInt32(value)); }
                            );
                            // input field -> slider
                            slider.GetComponentInChildren<UnityEngine.UI.InputField>().onValueChanged.AddListener(
                                (value) => { slider.GetComponentInChildren<UnityEngine.UI.Slider>().value = System.Convert.ToSingle(value); }
                            );

                            // add cancel trigger event delegate
                            UnityEngine.EventSystems.EventTrigger.Entry cancelEntry = new UnityEngine.EventSystems.EventTrigger.Entry();
                            cancelEntry.eventID = UnityEngine.EventSystems.EventTriggerType.Cancel;
                            cancelEntry.callback.AddListener(this.OnBackButtonClick);
                            slider.GetComponent<UnityEngine.EventSystems.EventTrigger>().triggers.Add(cancelEntry);

                            // then configure
                            slider.GetComponentInChildren<UnityEngine.UI.Text>().text = key;
                            slider.SetActive(true);

                            // add to the loaded element
                            this.m_loadedObject.Add(key, slider);
                            this.m_loadedOrder.Add(key);

                        }),
                        common.ApollonTypeSwitch.Case<uint>(() => {

                            // instantiate
                            UnityEngine.GameObject slider = UnityEngine.GameObject.Instantiate(this.sliderPrefab);

                            // attach
                            slider.transform.SetParent(this.parentObject.transform, false);
                            
                            // configure 

                            slider.GetComponentInChildren<UnityEngine.UI.Slider>().value =
                                System.Convert.ToUInt32(io.ApollonIOManager.Instance.Input.GetData(key));

                            if (io.ApollonIOManager.Instance.Input.GetRangeMetadata(key) != null)
                            {
                                slider.GetComponentInChildren<UnityEngine.UI.Slider>().minValue =
                                    io.ApollonIOManager.Instance.Input.GetRangeMetadata(key).Min;
                                slider.GetComponentInChildren<UnityEngine.UI.Slider>().maxValue =
                                     io.ApollonIOManager.Instance.Input.GetRangeMetadata(key).Max;
                            }

                            slider.GetComponentInChildren<UnityEngine.UI.InputField>().text = 
                                System.Convert.ToString(io.ApollonIOManager.Instance.Input.GetData(key));

                            // add delegate

                            // slider -> data
                            slider.GetComponentInChildren<UnityEngine.UI.Slider>().onValueChanged.AddListener(
                                (value) => { io.ApollonIOManager.Instance.Input.SetData(key, System.Convert.ToUInt32(value)); }
                            );
                            // slider -> input field 
                            slider.GetComponentInChildren<UnityEngine.UI.Slider>().onValueChanged.AddListener(
                                (value) => { slider.GetComponentInChildren<UnityEngine.UI.InputField>().text = System.Convert.ToString(System.Convert.ToUInt32(value)); }
                            );
                            // input field -> data
                            slider.GetComponentInChildren<UnityEngine.UI.InputField>().onValueChanged.AddListener(
                                (value) => { io.ApollonIOManager.Instance.Input.SetData(key, System.Convert.ToUInt32(value)); }
                            );
                            // input field -> slider
                            slider.GetComponentInChildren<UnityEngine.UI.InputField>().onValueChanged.AddListener(
                                (value) => { slider.GetComponentInChildren<UnityEngine.UI.Slider>().value = System.Convert.ToSingle(value); }
                            );

                            // add cancel trigger event delegate
                            UnityEngine.EventSystems.EventTrigger.Entry cancelEntry = new UnityEngine.EventSystems.EventTrigger.Entry();
                            cancelEntry.eventID = UnityEngine.EventSystems.EventTriggerType.Cancel;
                            cancelEntry.callback.AddListener(this.OnBackButtonClick);
                            slider.GetComponent<UnityEngine.EventSystems.EventTrigger>().triggers.Add(cancelEntry);

                            // then configure
                            slider.GetComponentInChildren<UnityEngine.UI.Text>().text = key;
                            slider.SetActive(true);

                            // add to the loaded element
                            this.m_loadedObject.Add(key, slider);
                            this.m_loadedOrder.Add(key);

                        }),
                        common.ApollonTypeSwitch.Case<double>(() => {
                         
                            // instantiate
                            UnityEngine.GameObject slider = UnityEngine.GameObject.Instantiate(this.sliderPrefab);

                            // attach
                            slider.transform.SetParent(this.parentObject.transform, false);
                            
                            // configure 

                            slider.GetComponentInChildren<UnityEngine.UI.Slider>().value =
                                System.Convert.ToSingle(io.ApollonIOManager.Instance.Input.GetData(key));

                            if (io.ApollonIOManager.Instance.Input.GetRangeMetadata(key) != null)
                            {
                                slider.GetComponentInChildren<UnityEngine.UI.Slider>().minValue =
                                    io.ApollonIOManager.Instance.Input.GetRangeMetadata(key).Min;
                                slider.GetComponentInChildren<UnityEngine.UI.Slider>().maxValue =
                                     io.ApollonIOManager.Instance.Input.GetRangeMetadata(key).Max;
                            }

                            slider.GetComponentInChildren<UnityEngine.UI.InputField>().text = 
                                System.Convert.ToString(io.ApollonIOManager.Instance.Input.GetData(key));

                            // add delegate

                            // slider -> data
                            slider.GetComponentInChildren<UnityEngine.UI.Slider>().onValueChanged.AddListener(
                                (value) => { io.ApollonIOManager.Instance.Input.SetData(key, System.Convert.ToDouble(value)); }
                            );
                            // slider -> input field 
                            slider.GetComponentInChildren<UnityEngine.UI.Slider>().onValueChanged.AddListener(
                                (value) => { slider.GetComponentInChildren<UnityEngine.UI.InputField>().text = System.Convert.ToString(System.Convert.ToDouble(value)); }
                            );
                            // input field -> data
                            slider.GetComponentInChildren<UnityEngine.UI.InputField>().onValueChanged.AddListener(
                                (value) => { io.ApollonIOManager.Instance.Input.SetData(key, System.Convert.ToDouble(value)); }
                            );
                            // input field -> slider
                            slider.GetComponentInChildren<UnityEngine.UI.InputField>().onValueChanged.AddListener(
                                (value) => { slider.GetComponentInChildren<UnityEngine.UI.Slider>().value = System.Convert.ToSingle(value); }
                            );

                            // add cancel trigger event delegate
                            UnityEngine.EventSystems.EventTrigger.Entry cancelEntry = new UnityEngine.EventSystems.EventTrigger.Entry();
                            cancelEntry.eventID = UnityEngine.EventSystems.EventTriggerType.Cancel;
                            cancelEntry.callback.AddListener(this.OnBackButtonClick);
                            slider.GetComponent<UnityEngine.EventSystems.EventTrigger>().triggers.Add(cancelEntry);

                            // then configure
                            slider.GetComponentInChildren<UnityEngine.UI.Text>().text = key;
                            slider.SetActive(true);

                            // add to the loaded element
                            this.m_loadedObject.Add(key, slider);
                            this.m_loadedOrder.Add(key);
                        
                        })

                    ); /* TypeSwitch */

                // if it is already there, then we should update values
                } else {

                    // Switch on type
                    common.ApollonTypeSwitch.Do(
                        
                        // get data
                        io.ApollonIOManager.Instance.Input.GetData(key),

                        // case : boolean
                        common.ApollonTypeSwitch.Case<bool>(() =>
                        {

                            // update
                            this.m_loadedObject[key].GetComponentInChildren<UnityEngine.UI.Toggle>().isOn = (bool)io.ApollonIOManager.Instance.Input.GetData(key);

                        }),
                        common.ApollonTypeSwitch.Case<int>(() =>
                        {

                            // update
                            this.m_loadedObject[key].GetComponentInChildren<UnityEngine.UI.Slider>().value =
                                System.Convert.ToInt32(io.ApollonIOManager.Instance.Input.GetData(key));

                        }),
                        common.ApollonTypeSwitch.Case<uint>(() =>
                        {

                            // update
                            this.m_loadedObject[key].GetComponentInChildren<UnityEngine.UI.Slider>().value =
                                System.Convert.ToUInt32(io.ApollonIOManager.Instance.Input.GetData(key));

                        }),
                        common.ApollonTypeSwitch.Case<double>(() =>
                        {

                            // update
                            this.m_loadedObject[key].GetComponentInChildren<UnityEngine.UI.Slider>().value =
                                System.Convert.ToSingle(io.ApollonIOManager.Instance.Input.GetData(key));

                        })

                    ); /* TypeSwitch */

                } /* if() */

            } /* for() */

            // set selected to the first element if list are not empty
            if (bOnce
                && this.m_loadedOrder.Count != 0
                && this.m_loadedObject.Count != 0
            )
            {
                this.gameObject.GetComponentInChildren<UnityEngine.EventSystems.EventSystem>().firstSelectedGameObject
                    = this.m_loadedObject[this.m_loadedOrder[0]];
                bOnce = false;
            }

        } /* OnEnable() */

        public void OnSaveButtonClicked()
        {
            io.ApollonIOManager.Instance.SaveInput();
            ApollonFrontendManager.Instance.setActive(ApollonFrontendManager.FrontendIDType.FileSelectionMenu);
            ApollonFrontendManager.Instance.setInactive(ApollonFrontendManager.FrontendIDType.ConfigureMenu);
        }

        // load seleected content & rewind
        public void OnBackButtonClick()
        {
            ApollonFrontendManager.Instance.setActive(ApollonFrontendManager.FrontendIDType.OptionMenu);
            ApollonFrontendManager.Instance.setInactive(ApollonFrontendManager.FrontendIDType.ConfigureMenu);
        }
        public void OnBackButtonClick(UnityEngine.EventSystems.BaseEventData eventData)
        {
            this.OnBackButtonClick();
        }

    } /* public class ApollonConfigureMenuBehaviour */

} /* } Labsim.apollon.frontend.menu */