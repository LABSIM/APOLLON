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
            ""id"": ""9c962b40-6dea-468b-9d8e-3c080a150a80"",
            ""actions"": [
                {
                    ""name"": ""Navigate"",
                    ""type"": ""Value"",
                    ""id"": ""795ad46c-2752-4f61-9e8e-723c490efe29"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""19c5e4e4-6bd1-47ac-98f1-c55bfdaf8492"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""d8e5d3a3-942b-496a-9a07-34680da642b9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""fbe1890b-a165-4309-9fdc-f890715252e0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""PassThrough"",
                    ""id"": ""17aa93b8-e867-475b-9ddf-51fa6283bc0c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ScrollWheel"",
                    ""type"": ""PassThrough"",
                    ""id"": ""0e11ad54-f15b-4ec0-a0e7-6c3cfd935fbe"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MiddleClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d93f95eb-2114-49da-b429-1c9eaaa721a6"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""ac09e699-b07f-4248-9e68-98ec946ad617"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TrackedDevicePosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b1a52617-19ef-4779-af28-40cb75b4301d"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TrackedDeviceOrientation"",
                    ""type"": ""PassThrough"",
                    ""id"": ""1fe945d5-095a-4a48-b931-d0cf070c3292"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""4d04fe29-5d09-400d-ad93-d79213dd03ff"",
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
                    ""id"": ""f1a7cba6-4b63-41ca-a12b-3f76305c5670"",
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
                    ""id"": ""0f433baa-00e9-4615-a117-7bcade09a60c"",
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
                    ""id"": ""6e55d37b-9d42-445f-8205-24a611b44a86"",
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
                    ""id"": ""922e215c-e899-4d39-b1bf-390e91405ce4"",
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
                    ""id"": ""a6bab0a3-71ad-4032-9e2a-ef36e0549389"",
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
                    ""id"": ""b6a58601-a992-4c04-9c9a-bad3470afea7"",
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
                    ""id"": ""19657e33-37e9-4d60-b28a-13f71e5e3ee9"",
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
                    ""id"": ""ab19a505-94c6-413a-90e4-a82f4b8539e8"",
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
                    ""id"": ""ec495a0f-c0f3-4406-a2c8-37640aa7af78"",
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
                    ""id"": ""ce696627-051b-4dce-ad63-67085ce12008"",
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
                    ""id"": ""34ac63f8-78ac-4e14-b9d5-603534f7eb8b"",
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
                    ""id"": ""44e3ec1a-fe30-43a6-a855-1f56fc8a2047"",
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
                    ""id"": ""fb23bc5e-75f8-4f27-8d1f-777288138e8d"",
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
                    ""id"": ""30643301-9f46-43d3-a885-a22be64fd67c"",
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
                    ""id"": ""59f2186d-a054-4e93-9169-d0251ffe416c"",
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
                    ""id"": ""ce2a1b81-128b-4e9b-9b7d-2abeed21a602"",
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
                    ""id"": ""0a354f01-b498-4df2-91a5-f474217fbbb8"",
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
                    ""id"": ""39e76df2-717d-410a-9860-354715c9335d"",
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
                    ""id"": ""6a1fd6a7-af7a-4fab-bb9c-279420aed6c1"",
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
                    ""id"": ""83a4cf70-9bca-422e-94d3-19a1e0503501"",
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
                    ""id"": ""2e54272a-b383-419d-8ed9-510074a863f2"",
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
                    ""id"": ""9ea2e22c-4172-4598-9a50-715f503e0030"",
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
                    ""id"": ""8aba0cd6-cd30-470d-a73d-28d6ab080513"",
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
                    ""id"": ""5774bed2-6f4c-4a77-98bf-51de0893b4d0"",
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
                    ""id"": ""7ec6a47a-a779-4a79-a9cc-ccedf2a23ee2"",
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
                    ""id"": ""3f86f091-9ec3-4d71-aa92-0b83345842f7"",
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
                    ""id"": ""5ec5c450-2dad-4b82-b3c6-35f4b7e6163d"",
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
                    ""id"": ""1949568a-10d0-41fc-98e8-7ed798fc5836"",
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
                    ""id"": ""0cd79eb6-e199-4995-a1b4-4336631cbbe5"",
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
                    ""id"": ""f36ac3c9-272e-47b6-8f6d-1fd771b0603f"",
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
                    ""id"": ""536a79a9-7543-4622-8a63-0a19c5ede3d8"",
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
                    ""id"": ""aad1aa84-879a-4cfe-b2c3-26b48d2fdc22"",
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
                    ""id"": ""49176bc6-03d0-4800-9ed3-67653d0ea0c2"",
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
                    ""id"": ""3cc1b25f-e5b0-42e4-8f73-6a887d0d3034"",
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
                    ""id"": ""680628dd-cddb-4ff7-92f6-c426f2fc2811"",
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
                    ""id"": ""ca8e8e98-2c52-491b-b4b1-bd0b8000302b"",
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
                    ""id"": ""f7e00a0a-d01a-4459-ae11-1ac1d3ce8918"",
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
