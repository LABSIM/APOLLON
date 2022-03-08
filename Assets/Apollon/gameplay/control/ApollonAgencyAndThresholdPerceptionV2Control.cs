// GENERATED AUTOMATICALLY FROM 'Assets/Inputs/AgencyAndThresholdPerceptionV2Control.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Labsim.apollon.gameplay.control
{
    public class @ApollonAgencyAndThresholdPerceptionV2Control : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @ApollonAgencyAndThresholdPerceptionV2Control()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""AgencyAndThresholdPerceptionV2Control"",
    ""maps"": [
        {
            ""name"": ""Subject"",
            ""id"": ""4b856211-621f-4569-b880-49158515e745"",
            ""actions"": [
                {
                    ""name"": ""ValueChanged"",
                    ""type"": ""Value"",
                    ""id"": ""8349b3c3-5840-473e-a94e-21afc2a5d166"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""NeutralCommandTriggered"",
                    ""type"": ""Value"",
                    ""id"": ""a0fb8fb5-b63e-4610-8ff2-7b61ec34192c"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PositiveCommandTriggered"",
                    ""type"": ""Value"",
                    ""id"": ""26c2e13a-4ba6-4901-ab60-e46e55102ffa"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""NegativeCommandTriggered"",
                    ""type"": ""Value"",
                    ""id"": ""05cdefeb-c664-4af6-91a8-2639dc415c9f"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""UserResponse"",
                    ""type"": ""Button"",
                    ""id"": ""b6a56a5f-6e53-4488-af02-9fe9552581b8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b726fa74-623c-4f2e-bf1e-f4280a175af2"",
                    ""path"": ""<HID::Thrustmaster Throttle - HOTAS Warthog>/z"",
                    ""interactions"": """",
                    ""processors"": ""Invert"",
                    ""groups"": ""Cockpit"",
                    ""action"": ""ValueChanged"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bd9481ea-7ec0-4f2f-a9d5-4c61721ada75"",
                    ""path"": ""<HID::Thrustmaster Throttle - HOTAS Warthog>/z"",
                    ""interactions"": ""ApollonHoldInRangeWithThreshold(m_duration_ms=250,m_threshold_ratio_percentage=15,m_lower_bound=-0.05,m_upper_bound=0.23)"",
                    ""processors"": ""AxisDeadzone,Invert"",
                    ""groups"": ""Cockpit"",
                    ""action"": ""NeutralCommandTriggered"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""be316a56-89e1-4275-bb06-4acc3f710952"",
                    ""path"": ""<HID::Thrustmaster Throttle - HOTAS Warthog>/z"",
                    ""interactions"": ""ApollonHoldInRangeWithThreshold(m_duration_ms=50,m_lower_bound=0.75)"",
                    ""processors"": ""AxisDeadzone,Invert"",
                    ""groups"": ""Cockpit"",
                    ""action"": ""PositiveCommandTriggered"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""69a04864-5eed-49d4-8b5c-9f3347917bd9"",
                    ""path"": ""<HID::Thrustmaster Throttle - HOTAS Warthog>/z"",
                    ""interactions"": ""ApollonHoldInRangeWithThreshold(m_duration_ms=50,m_upper_bound=-0.75)"",
                    ""processors"": ""AxisDeadzone,Invert"",
                    ""groups"": ""Cockpit"",
                    ""action"": ""NegativeCommandTriggered"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cdeaec63-bb67-45a6-a091-4a0ad19a5615"",
                    ""path"": ""<HID::Thrustmaster Throttle - HOTAS Warthog>/button15"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Cockpit"",
                    ""action"": ""UserResponse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Supervisor"",
            ""id"": ""b230bea6-a7e6-4ec8-b769-9fdc768c0d6e"",
            ""actions"": [
                {
                    ""name"": ""Navigate"",
                    ""type"": ""Value"",
                    ""id"": ""7825ed6f-e94a-4605-9549-20a4c60c3856"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""f6ccb703-1a88-49ba-9f6e-ddf06b65d0f8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""04b43704-4e97-4c25-9126-a5d2b3b3f300"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""ebf46f1c-8db4-4cb3-a652-52eeb5a5d70c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""PassThrough"",
                    ""id"": ""47801199-125d-4634-b86a-df187804700b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ScrollWheel"",
                    ""type"": ""PassThrough"",
                    ""id"": ""190c8772-9216-44b7-b7a5-bbb3b36373d5"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MiddleClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""0957501b-9994-4117-8872-a7c3615f72ac"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""db595ea6-9f70-429b-b71d-fef5217d3135"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TrackedDevicePosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b31def38-1941-432b-8171-c3a8b3bc6da1"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TrackedDeviceOrientation"",
                    ""type"": ""PassThrough"",
                    ""id"": ""280f997d-9979-421d-a65c-b9a9e8999922"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""52fe3735-e5e3-4a66-9206-8680652afc16"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3a97e45f-bbbd-4c04-a264-350e192dd970"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d2b45d2c-6f0e-4a27-956c-b6f4b523c8ba"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""42f52a4f-4239-4a28-a1a6-bc020a4a916d"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""494fac0f-4742-443b-97e5-adc0a38591e2"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3031086f-e011-4875-aa56-5c5761f7d602"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""dfa88965-2782-480f-9b86-df9b8eb63c51"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2c26661b-439b-46d3-8a06-d0729eda43a9"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d5549dce-62aa-418a-9e54-aaaf2c8f1c82"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""322b5c01-d838-48a0-88c8-7ff91fbd5896"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Joystick"",
                    ""id"": ""87c47269-fd6c-410e-82db-ef6a4bf8e088"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""53bdb09e-6f2d-4f38-b1f9-a8ad70db04dd"",
                    ""path"": ""<Joystick>/stick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""53a72287-5af5-42d2-a864-329a5437a714"",
                    ""path"": ""<Joystick>/stick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b6f93f8e-dc8f-4c67-afdc-90da39f60d2e"",
                    ""path"": ""<Joystick>/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""092c120e-ab01-4fb2-8f00-0ac65f45a2ab"",
                    ""path"": ""<Joystick>/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""b7799ebf-a253-4d9a-8355-e15cb7017115"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b2531b01-343d-4ee8-9c2f-02dca7c22b49"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3b4503e3-57fc-4e49-b674-cbf03bc1e632"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""bbfa2b0d-113d-4c59-b552-93af3d9887ee"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""553cb807-5c95-4fcd-99c1-0c133b26eb5d"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a7e286e4-9127-4995-ae7e-db465df5cf40"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e774f905-2248-45ff-b121-66f71aee8f5c"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""bec570e2-3cb0-454a-9b56-a31d8dc66354"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b114580b-ac2a-4e90-bdf0-3118af328ee1"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""24e000d9-4a66-44aa-83e9-ef7c8200d965"",
                    ""path"": ""*/{Submit}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5bd88c7e-245c-4dd4-b9d5-483972b93acf"",
                    ""path"": ""*/{Cancel}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""27e52e9b-f043-4600-a32d-aaac5f3149cd"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7ef896fe-4c0f-43c7-a022-ee585520b899"",
                    ""path"": ""<Pen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1cb9307f-3c17-4428-9b04-b5b7c3f8c37f"",
                    ""path"": ""<Touchscreen>/touch*/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""855eabd4-0efb-45a9-a04b-87029d8fc4a1"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bb5a08e2-d390-4758-9304-0da5ce0e4f7c"",
                    ""path"": ""<Pen>/tip"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c0b7cb0f-1d75-4123-977a-62f5efcb0194"",
                    ""path"": ""<Touchscreen>/touch*/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e1231808-2a69-4561-b705-0627a9846f99"",
                    ""path"": ""<XRController>/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""918d4c03-1d89-43ac-b1ab-83f500441bb9"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""ScrollWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""480c7f67-c831-42e6-8c6c-24d4303c6687"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""MiddleClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e79ed5c8-395c-4483-aed3-3b86c7fc2882"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fd0423df-9ac2-4aa4-b778-a5820e1563f1"",
                    ""path"": ""<XRController>/devicePosition"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""TrackedDevicePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1a08f4dd-8686-4575-b30b-a569d6f43265"",
                    ""path"": ""<XRController>/deviceRotation"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XR"",
                    ""action"": ""TrackedDeviceOrientation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Cockpit"",
            ""bindingGroup"": ""Cockpit"",
            ""devices"": [
                {
                    ""devicePath"": ""<HID::Thustmaster Joystick - HOTAS Warthog>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<HID::Thrustmaster Throttle - HOTAS Warthog>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Subject
            m_Subject = asset.FindActionMap("Subject", throwIfNotFound: true);
            m_Subject_ValueChanged = m_Subject.FindAction("ValueChanged", throwIfNotFound: true);
            m_Subject_NeutralCommandTriggered = m_Subject.FindAction("NeutralCommandTriggered", throwIfNotFound: true);
            m_Subject_PositiveCommandTriggered = m_Subject.FindAction("PositiveCommandTriggered", throwIfNotFound: true);
            m_Subject_NegativeCommandTriggered = m_Subject.FindAction("NegativeCommandTriggered", throwIfNotFound: true);
            m_Subject_UserResponse = m_Subject.FindAction("UserResponse", throwIfNotFound: true);
            // Supervisor
            m_Supervisor = asset.FindActionMap("Supervisor", throwIfNotFound: true);
            m_Supervisor_Navigate = m_Supervisor.FindAction("Navigate", throwIfNotFound: true);
            m_Supervisor_Submit = m_Supervisor.FindAction("Submit", throwIfNotFound: true);
            m_Supervisor_Cancel = m_Supervisor.FindAction("Cancel", throwIfNotFound: true);
            m_Supervisor_Point = m_Supervisor.FindAction("Point", throwIfNotFound: true);
            m_Supervisor_Click = m_Supervisor.FindAction("Click", throwIfNotFound: true);
            m_Supervisor_ScrollWheel = m_Supervisor.FindAction("ScrollWheel", throwIfNotFound: true);
            m_Supervisor_MiddleClick = m_Supervisor.FindAction("MiddleClick", throwIfNotFound: true);
            m_Supervisor_RightClick = m_Supervisor.FindAction("RightClick", throwIfNotFound: true);
            m_Supervisor_TrackedDevicePosition = m_Supervisor.FindAction("TrackedDevicePosition", throwIfNotFound: true);
            m_Supervisor_TrackedDeviceOrientation = m_Supervisor.FindAction("TrackedDeviceOrientation", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // Subject
        private readonly InputActionMap m_Subject;
        private ISubjectActions m_SubjectActionsCallbackInterface;
        private readonly InputAction m_Subject_ValueChanged;
        private readonly InputAction m_Subject_NeutralCommandTriggered;
        private readonly InputAction m_Subject_PositiveCommandTriggered;
        private readonly InputAction m_Subject_NegativeCommandTriggered;
        private readonly InputAction m_Subject_UserResponse;
        public struct SubjectActions
        {
            private @ApollonAgencyAndThresholdPerceptionV2Control m_Wrapper;
            public SubjectActions(@ApollonAgencyAndThresholdPerceptionV2Control wrapper) { m_Wrapper = wrapper; }
            public InputAction @ValueChanged => m_Wrapper.m_Subject_ValueChanged;
            public InputAction @NeutralCommandTriggered => m_Wrapper.m_Subject_NeutralCommandTriggered;
            public InputAction @PositiveCommandTriggered => m_Wrapper.m_Subject_PositiveCommandTriggered;
            public InputAction @NegativeCommandTriggered => m_Wrapper.m_Subject_NegativeCommandTriggered;
            public InputAction @UserResponse => m_Wrapper.m_Subject_UserResponse;
            public InputActionMap Get() { return m_Wrapper.m_Subject; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(SubjectActions set) { return set.Get(); }
            public void SetCallbacks(ISubjectActions instance)
            {
                if (m_Wrapper.m_SubjectActionsCallbackInterface != null)
                {
                    @ValueChanged.started -= m_Wrapper.m_SubjectActionsCallbackInterface.OnValueChanged;
                    @ValueChanged.performed -= m_Wrapper.m_SubjectActionsCallbackInterface.OnValueChanged;
                    @ValueChanged.canceled -= m_Wrapper.m_SubjectActionsCallbackInterface.OnValueChanged;
                    @NeutralCommandTriggered.started -= m_Wrapper.m_SubjectActionsCallbackInterface.OnNeutralCommandTriggered;
                    @NeutralCommandTriggered.performed -= m_Wrapper.m_SubjectActionsCallbackInterface.OnNeutralCommandTriggered;
                    @NeutralCommandTriggered.canceled -= m_Wrapper.m_SubjectActionsCallbackInterface.OnNeutralCommandTriggered;
                    @PositiveCommandTriggered.started -= m_Wrapper.m_SubjectActionsCallbackInterface.OnPositiveCommandTriggered;
                    @PositiveCommandTriggered.performed -= m_Wrapper.m_SubjectActionsCallbackInterface.OnPositiveCommandTriggered;
                    @PositiveCommandTriggered.canceled -= m_Wrapper.m_SubjectActionsCallbackInterface.OnPositiveCommandTriggered;
                    @NegativeCommandTriggered.started -= m_Wrapper.m_SubjectActionsCallbackInterface.OnNegativeCommandTriggered;
                    @NegativeCommandTriggered.performed -= m_Wrapper.m_SubjectActionsCallbackInterface.OnNegativeCommandTriggered;
                    @NegativeCommandTriggered.canceled -= m_Wrapper.m_SubjectActionsCallbackInterface.OnNegativeCommandTriggered;
                    @UserResponse.started -= m_Wrapper.m_SubjectActionsCallbackInterface.OnUserResponse;
                    @UserResponse.performed -= m_Wrapper.m_SubjectActionsCallbackInterface.OnUserResponse;
                    @UserResponse.canceled -= m_Wrapper.m_SubjectActionsCallbackInterface.OnUserResponse;
                }
                m_Wrapper.m_SubjectActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @ValueChanged.started += instance.OnValueChanged;
                    @ValueChanged.performed += instance.OnValueChanged;
                    @ValueChanged.canceled += instance.OnValueChanged;
                    @NeutralCommandTriggered.started += instance.OnNeutralCommandTriggered;
                    @NeutralCommandTriggered.performed += instance.OnNeutralCommandTriggered;
                    @NeutralCommandTriggered.canceled += instance.OnNeutralCommandTriggered;
                    @PositiveCommandTriggered.started += instance.OnPositiveCommandTriggered;
                    @PositiveCommandTriggered.performed += instance.OnPositiveCommandTriggered;
                    @PositiveCommandTriggered.canceled += instance.OnPositiveCommandTriggered;
                    @NegativeCommandTriggered.started += instance.OnNegativeCommandTriggered;
                    @NegativeCommandTriggered.performed += instance.OnNegativeCommandTriggered;
                    @NegativeCommandTriggered.canceled += instance.OnNegativeCommandTriggered;
                    @UserResponse.started += instance.OnUserResponse;
                    @UserResponse.performed += instance.OnUserResponse;
                    @UserResponse.canceled += instance.OnUserResponse;
                }
            }
        }
        public SubjectActions @Subject => new SubjectActions(this);

        // Supervisor
        private readonly InputActionMap m_Supervisor;
        private ISupervisorActions m_SupervisorActionsCallbackInterface;
        private readonly InputAction m_Supervisor_Navigate;
        private readonly InputAction m_Supervisor_Submit;
        private readonly InputAction m_Supervisor_Cancel;
        private readonly InputAction m_Supervisor_Point;
        private readonly InputAction m_Supervisor_Click;
        private readonly InputAction m_Supervisor_ScrollWheel;
        private readonly InputAction m_Supervisor_MiddleClick;
        private readonly InputAction m_Supervisor_RightClick;
        private readonly InputAction m_Supervisor_TrackedDevicePosition;
        private readonly InputAction m_Supervisor_TrackedDeviceOrientation;
        public struct SupervisorActions
        {
            private @ApollonAgencyAndThresholdPerceptionV2Control m_Wrapper;
            public SupervisorActions(@ApollonAgencyAndThresholdPerceptionV2Control wrapper) { m_Wrapper = wrapper; }
            public InputAction @Navigate => m_Wrapper.m_Supervisor_Navigate;
            public InputAction @Submit => m_Wrapper.m_Supervisor_Submit;
            public InputAction @Cancel => m_Wrapper.m_Supervisor_Cancel;
            public InputAction @Point => m_Wrapper.m_Supervisor_Point;
            public InputAction @Click => m_Wrapper.m_Supervisor_Click;
            public InputAction @ScrollWheel => m_Wrapper.m_Supervisor_ScrollWheel;
            public InputAction @MiddleClick => m_Wrapper.m_Supervisor_MiddleClick;
            public InputAction @RightClick => m_Wrapper.m_Supervisor_RightClick;
            public InputAction @TrackedDevicePosition => m_Wrapper.m_Supervisor_TrackedDevicePosition;
            public InputAction @TrackedDeviceOrientation => m_Wrapper.m_Supervisor_TrackedDeviceOrientation;
            public InputActionMap Get() { return m_Wrapper.m_Supervisor; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(SupervisorActions set) { return set.Get(); }
            public void SetCallbacks(ISupervisorActions instance)
            {
                if (m_Wrapper.m_SupervisorActionsCallbackInterface != null)
                {
                    @Navigate.started -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnNavigate;
                    @Navigate.performed -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnNavigate;
                    @Navigate.canceled -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnNavigate;
                    @Submit.started -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnSubmit;
                    @Submit.performed -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnSubmit;
                    @Submit.canceled -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnSubmit;
                    @Cancel.started -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnCancel;
                    @Cancel.performed -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnCancel;
                    @Cancel.canceled -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnCancel;
                    @Point.started -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnPoint;
                    @Point.performed -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnPoint;
                    @Point.canceled -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnPoint;
                    @Click.started -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnClick;
                    @Click.performed -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnClick;
                    @Click.canceled -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnClick;
                    @ScrollWheel.started -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnScrollWheel;
                    @ScrollWheel.performed -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnScrollWheel;
                    @ScrollWheel.canceled -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnScrollWheel;
                    @MiddleClick.started -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnMiddleClick;
                    @MiddleClick.performed -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnMiddleClick;
                    @MiddleClick.canceled -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnMiddleClick;
                    @RightClick.started -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnRightClick;
                    @RightClick.performed -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnRightClick;
                    @RightClick.canceled -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnRightClick;
                    @TrackedDevicePosition.started -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnTrackedDevicePosition;
                    @TrackedDevicePosition.performed -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnTrackedDevicePosition;
                    @TrackedDevicePosition.canceled -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnTrackedDevicePosition;
                    @TrackedDeviceOrientation.started -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnTrackedDeviceOrientation;
                    @TrackedDeviceOrientation.performed -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnTrackedDeviceOrientation;
                    @TrackedDeviceOrientation.canceled -= m_Wrapper.m_SupervisorActionsCallbackInterface.OnTrackedDeviceOrientation;
                }
                m_Wrapper.m_SupervisorActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Navigate.started += instance.OnNavigate;
                    @Navigate.performed += instance.OnNavigate;
                    @Navigate.canceled += instance.OnNavigate;
                    @Submit.started += instance.OnSubmit;
                    @Submit.performed += instance.OnSubmit;
                    @Submit.canceled += instance.OnSubmit;
                    @Cancel.started += instance.OnCancel;
                    @Cancel.performed += instance.OnCancel;
                    @Cancel.canceled += instance.OnCancel;
                    @Point.started += instance.OnPoint;
                    @Point.performed += instance.OnPoint;
                    @Point.canceled += instance.OnPoint;
                    @Click.started += instance.OnClick;
                    @Click.performed += instance.OnClick;
                    @Click.canceled += instance.OnClick;
                    @ScrollWheel.started += instance.OnScrollWheel;
                    @ScrollWheel.performed += instance.OnScrollWheel;
                    @ScrollWheel.canceled += instance.OnScrollWheel;
                    @MiddleClick.started += instance.OnMiddleClick;
                    @MiddleClick.performed += instance.OnMiddleClick;
                    @MiddleClick.canceled += instance.OnMiddleClick;
                    @RightClick.started += instance.OnRightClick;
                    @RightClick.performed += instance.OnRightClick;
                    @RightClick.canceled += instance.OnRightClick;
                    @TrackedDevicePosition.started += instance.OnTrackedDevicePosition;
                    @TrackedDevicePosition.performed += instance.OnTrackedDevicePosition;
                    @TrackedDevicePosition.canceled += instance.OnTrackedDevicePosition;
                    @TrackedDeviceOrientation.started += instance.OnTrackedDeviceOrientation;
                    @TrackedDeviceOrientation.performed += instance.OnTrackedDeviceOrientation;
                    @TrackedDeviceOrientation.canceled += instance.OnTrackedDeviceOrientation;
                }
            }
        }
        public SupervisorActions @Supervisor => new SupervisorActions(this);
        private int m_CockpitSchemeIndex = -1;
        public InputControlScheme CockpitScheme
        {
            get
            {
                if (m_CockpitSchemeIndex == -1) m_CockpitSchemeIndex = asset.FindControlSchemeIndex("Cockpit");
                return asset.controlSchemes[m_CockpitSchemeIndex];
            }
        }
        private int m_KeyboardMouseSchemeIndex = -1;
        public InputControlScheme KeyboardMouseScheme
        {
            get
            {
                if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
                return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
            }
        }
        public interface ISubjectActions
        {
            void OnValueChanged(InputAction.CallbackContext context);
            void OnNeutralCommandTriggered(InputAction.CallbackContext context);
            void OnPositiveCommandTriggered(InputAction.CallbackContext context);
            void OnNegativeCommandTriggered(InputAction.CallbackContext context);
            void OnUserResponse(InputAction.CallbackContext context);
        }
        public interface ISupervisorActions
        {
            void OnNavigate(InputAction.CallbackContext context);
            void OnSubmit(InputAction.CallbackContext context);
            void OnCancel(InputAction.CallbackContext context);
            void OnPoint(InputAction.CallbackContext context);
            void OnClick(InputAction.CallbackContext context);
            void OnScrollWheel(InputAction.CallbackContext context);
            void OnMiddleClick(InputAction.CallbackContext context);
            void OnRightClick(InputAction.CallbackContext context);
            void OnTrackedDevicePosition(InputAction.CallbackContext context);
            void OnTrackedDeviceOrientation(InputAction.CallbackContext context);
        }
    }
}
