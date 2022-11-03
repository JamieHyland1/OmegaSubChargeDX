// GENERATED AUTOMATICALLY FROM 'Assets/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Ground_Move"",
            ""id"": ""cecde46a-c54f-4f2a-a49d-1be0553b35c7"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""930c8e0f-3025-416e-beab-690576d7444f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""d8b8ab87-0918-44cb-9429-2ab784bfa976"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""13fa60e7-38cd-47bc-89a8-3692f9396806"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Slide"",
                    ""type"": ""Button"",
                    ""id"": ""7ac3bc54-8573-43fb-88d8-c155e7d4e823"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""9fca10c8-910b-499f-9114-39b00c33541a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""02f1b6b3-6188-4b6d-bcbc-6b1f2e2375ff"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2cf363d4-1114-4101-b9a8-9390becd6496"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5a3c4665-51d6-4dc0-8683-c2bc285bb361"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a5156a76-6797-40de-9f35-fbcc975e9205"",
                    ""path"": ""<XInputController>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Slide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7c559db9-73b3-4a67-b13d-e19e50a71182"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Slide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a61273b1-9b51-46e7-af4d-892128fd0533"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Air_Move"",
            ""id"": ""7bdef28e-ea00-490f-b5de-aa2a0d097946"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""32a02d7c-503c-4ef2-a076-89ba2fcd6832"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""71732ee3-a32e-4344-b906-5b1f397afbba"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""3b4d4d73-dcec-4a08-86aa-90555369daaa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""e39c797f-a85b-41e8-a955-e9762f78df54"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2b5d1349-87b7-43c6-aa0e-e12c33fd0323"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""97edd5ae-8ceb-467c-8799-b6147fff196a"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""44cfd377-659c-4e27-9735-1beeff527ed4"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""be1cde16-0f2f-44ec-8a08-0cdcc8656a80"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Sliding"",
            ""id"": ""2266a33f-d1f2-498d-95eb-54195380cef5"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""5239e8e5-b55c-4df6-8e5b-1c98aca1364b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""444dbb10-fc5a-4b2f-9933-06b664d83706"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""62f0df47-bb35-418e-9a31-e326336426d3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Slide"",
                    ""type"": ""Button"",
                    ""id"": ""945d321d-a17c-4af7-9cef-8478ecc0ad0e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3886f4ba-75a9-49f8-90de-98b43a0311f5"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""84b826d2-851a-4a81-ba2d-c68d5aa80997"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f096a431-6822-4aea-9cb0-82504142f5a3"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a69703c3-684d-4d6c-8431-7f17ac3633fa"",
                    ""path"": ""<XInputController>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Slide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Dash"",
            ""id"": ""6feb2e0f-de12-4f36-9677-48e713c4389d"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""03530f72-835b-4c5c-a150-1b9f1abb8d0b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""00502188-9782-4ba2-95b6-58d838fd804f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""44ba5cc1-4368-4279-b3f7-5c499ef52a01"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d336eab2-1ac5-4f4c-b2dd-f33419f3669b"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""WallJump"",
            ""id"": ""f42d5a21-3651-46b2-8fa6-e0c7f144d649"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""1b283562-4c22-47b8-a949-1ba9cd312dfa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""94210c8c-8593-4e2c-b14d-ac767b7b2e41"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""f21ea219-56da-491f-97b3-4360d62eb1df"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""72e99536-1414-4f9c-ab42-4adfa15815f5"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a88b0426-f1d3-4a7d-98d0-cd8575078858"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4c97ebd3-8c9c-4863-9ffd-1b6ef587eea8"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""New control scheme"",
            ""bindingGroup"": ""New control scheme"",
            ""devices"": [
                {
                    ""devicePath"": ""<DualShockGamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Ground_Move
        m_Ground_Move = asset.FindActionMap("Ground_Move", throwIfNotFound: true);
        m_Ground_Move_Jump = m_Ground_Move.FindAction("Jump", throwIfNotFound: true);
        m_Ground_Move_Move = m_Ground_Move.FindAction("Move", throwIfNotFound: true);
        m_Ground_Move_Rotate = m_Ground_Move.FindAction("Rotate", throwIfNotFound: true);
        m_Ground_Move_Slide = m_Ground_Move.FindAction("Slide", throwIfNotFound: true);
        m_Ground_Move_Dash = m_Ground_Move.FindAction("Dash", throwIfNotFound: true);
        // Air_Move
        m_Air_Move = asset.FindActionMap("Air_Move", throwIfNotFound: true);
        m_Air_Move_Move = m_Air_Move.FindAction("Move", throwIfNotFound: true);
        m_Air_Move_Rotate = m_Air_Move.FindAction("Rotate", throwIfNotFound: true);
        m_Air_Move_Dash = m_Air_Move.FindAction("Dash", throwIfNotFound: true);
        m_Air_Move_Jump = m_Air_Move.FindAction("Jump", throwIfNotFound: true);
        // Sliding
        m_Sliding = asset.FindActionMap("Sliding", throwIfNotFound: true);
        m_Sliding_Jump = m_Sliding.FindAction("Jump", throwIfNotFound: true);
        m_Sliding_Move = m_Sliding.FindAction("Move", throwIfNotFound: true);
        m_Sliding_Rotate = m_Sliding.FindAction("Rotate", throwIfNotFound: true);
        m_Sliding_Slide = m_Sliding.FindAction("Slide", throwIfNotFound: true);
        // Dash
        m_Dash = asset.FindActionMap("Dash", throwIfNotFound: true);
        m_Dash_Move = m_Dash.FindAction("Move", throwIfNotFound: true);
        m_Dash_Rotate = m_Dash.FindAction("Rotate", throwIfNotFound: true);
        // WallJump
        m_WallJump = asset.FindActionMap("WallJump", throwIfNotFound: true);
        m_WallJump_Jump = m_WallJump.FindAction("Jump", throwIfNotFound: true);
        m_WallJump_Move = m_WallJump.FindAction("Move", throwIfNotFound: true);
        m_WallJump_Rotate = m_WallJump.FindAction("Rotate", throwIfNotFound: true);
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

    // Ground_Move
    private readonly InputActionMap m_Ground_Move;
    private IGround_MoveActions m_Ground_MoveActionsCallbackInterface;
    private readonly InputAction m_Ground_Move_Jump;
    private readonly InputAction m_Ground_Move_Move;
    private readonly InputAction m_Ground_Move_Rotate;
    private readonly InputAction m_Ground_Move_Slide;
    private readonly InputAction m_Ground_Move_Dash;
    public struct Ground_MoveActions
    {
        private @PlayerControls m_Wrapper;
        public Ground_MoveActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_Ground_Move_Jump;
        public InputAction @Move => m_Wrapper.m_Ground_Move_Move;
        public InputAction @Rotate => m_Wrapper.m_Ground_Move_Rotate;
        public InputAction @Slide => m_Wrapper.m_Ground_Move_Slide;
        public InputAction @Dash => m_Wrapper.m_Ground_Move_Dash;
        public InputActionMap Get() { return m_Wrapper.m_Ground_Move; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(Ground_MoveActions set) { return set.Get(); }
        public void SetCallbacks(IGround_MoveActions instance)
        {
            if (m_Wrapper.m_Ground_MoveActionsCallbackInterface != null)
            {
                @Jump.started -= m_Wrapper.m_Ground_MoveActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_Ground_MoveActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_Ground_MoveActionsCallbackInterface.OnJump;
                @Move.started -= m_Wrapper.m_Ground_MoveActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_Ground_MoveActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_Ground_MoveActionsCallbackInterface.OnMove;
                @Rotate.started -= m_Wrapper.m_Ground_MoveActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_Ground_MoveActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_Ground_MoveActionsCallbackInterface.OnRotate;
                @Slide.started -= m_Wrapper.m_Ground_MoveActionsCallbackInterface.OnSlide;
                @Slide.performed -= m_Wrapper.m_Ground_MoveActionsCallbackInterface.OnSlide;
                @Slide.canceled -= m_Wrapper.m_Ground_MoveActionsCallbackInterface.OnSlide;
                @Dash.started -= m_Wrapper.m_Ground_MoveActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_Ground_MoveActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_Ground_MoveActionsCallbackInterface.OnDash;
            }
            m_Wrapper.m_Ground_MoveActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
                @Slide.started += instance.OnSlide;
                @Slide.performed += instance.OnSlide;
                @Slide.canceled += instance.OnSlide;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
            }
        }
    }
    public Ground_MoveActions @Ground_Move => new Ground_MoveActions(this);

    // Air_Move
    private readonly InputActionMap m_Air_Move;
    private IAir_MoveActions m_Air_MoveActionsCallbackInterface;
    private readonly InputAction m_Air_Move_Move;
    private readonly InputAction m_Air_Move_Rotate;
    private readonly InputAction m_Air_Move_Dash;
    private readonly InputAction m_Air_Move_Jump;
    public struct Air_MoveActions
    {
        private @PlayerControls m_Wrapper;
        public Air_MoveActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Air_Move_Move;
        public InputAction @Rotate => m_Wrapper.m_Air_Move_Rotate;
        public InputAction @Dash => m_Wrapper.m_Air_Move_Dash;
        public InputAction @Jump => m_Wrapper.m_Air_Move_Jump;
        public InputActionMap Get() { return m_Wrapper.m_Air_Move; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(Air_MoveActions set) { return set.Get(); }
        public void SetCallbacks(IAir_MoveActions instance)
        {
            if (m_Wrapper.m_Air_MoveActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_Air_MoveActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_Air_MoveActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_Air_MoveActionsCallbackInterface.OnMove;
                @Rotate.started -= m_Wrapper.m_Air_MoveActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_Air_MoveActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_Air_MoveActionsCallbackInterface.OnRotate;
                @Dash.started -= m_Wrapper.m_Air_MoveActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_Air_MoveActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_Air_MoveActionsCallbackInterface.OnDash;
                @Jump.started -= m_Wrapper.m_Air_MoveActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_Air_MoveActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_Air_MoveActionsCallbackInterface.OnJump;
            }
            m_Wrapper.m_Air_MoveActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
            }
        }
    }
    public Air_MoveActions @Air_Move => new Air_MoveActions(this);

    // Sliding
    private readonly InputActionMap m_Sliding;
    private ISlidingActions m_SlidingActionsCallbackInterface;
    private readonly InputAction m_Sliding_Jump;
    private readonly InputAction m_Sliding_Move;
    private readonly InputAction m_Sliding_Rotate;
    private readonly InputAction m_Sliding_Slide;
    public struct SlidingActions
    {
        private @PlayerControls m_Wrapper;
        public SlidingActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_Sliding_Jump;
        public InputAction @Move => m_Wrapper.m_Sliding_Move;
        public InputAction @Rotate => m_Wrapper.m_Sliding_Rotate;
        public InputAction @Slide => m_Wrapper.m_Sliding_Slide;
        public InputActionMap Get() { return m_Wrapper.m_Sliding; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SlidingActions set) { return set.Get(); }
        public void SetCallbacks(ISlidingActions instance)
        {
            if (m_Wrapper.m_SlidingActionsCallbackInterface != null)
            {
                @Jump.started -= m_Wrapper.m_SlidingActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_SlidingActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_SlidingActionsCallbackInterface.OnJump;
                @Move.started -= m_Wrapper.m_SlidingActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_SlidingActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_SlidingActionsCallbackInterface.OnMove;
                @Rotate.started -= m_Wrapper.m_SlidingActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_SlidingActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_SlidingActionsCallbackInterface.OnRotate;
                @Slide.started -= m_Wrapper.m_SlidingActionsCallbackInterface.OnSlide;
                @Slide.performed -= m_Wrapper.m_SlidingActionsCallbackInterface.OnSlide;
                @Slide.canceled -= m_Wrapper.m_SlidingActionsCallbackInterface.OnSlide;
            }
            m_Wrapper.m_SlidingActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
                @Slide.started += instance.OnSlide;
                @Slide.performed += instance.OnSlide;
                @Slide.canceled += instance.OnSlide;
            }
        }
    }
    public SlidingActions @Sliding => new SlidingActions(this);

    // Dash
    private readonly InputActionMap m_Dash;
    private IDashActions m_DashActionsCallbackInterface;
    private readonly InputAction m_Dash_Move;
    private readonly InputAction m_Dash_Rotate;
    public struct DashActions
    {
        private @PlayerControls m_Wrapper;
        public DashActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Dash_Move;
        public InputAction @Rotate => m_Wrapper.m_Dash_Rotate;
        public InputActionMap Get() { return m_Wrapper.m_Dash; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DashActions set) { return set.Get(); }
        public void SetCallbacks(IDashActions instance)
        {
            if (m_Wrapper.m_DashActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_DashActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_DashActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_DashActionsCallbackInterface.OnMove;
                @Rotate.started -= m_Wrapper.m_DashActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_DashActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_DashActionsCallbackInterface.OnRotate;
            }
            m_Wrapper.m_DashActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
            }
        }
    }
    public DashActions @Dash => new DashActions(this);

    // WallJump
    private readonly InputActionMap m_WallJump;
    private IWallJumpActions m_WallJumpActionsCallbackInterface;
    private readonly InputAction m_WallJump_Jump;
    private readonly InputAction m_WallJump_Move;
    private readonly InputAction m_WallJump_Rotate;
    public struct WallJumpActions
    {
        private @PlayerControls m_Wrapper;
        public WallJumpActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_WallJump_Jump;
        public InputAction @Move => m_Wrapper.m_WallJump_Move;
        public InputAction @Rotate => m_Wrapper.m_WallJump_Rotate;
        public InputActionMap Get() { return m_Wrapper.m_WallJump; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(WallJumpActions set) { return set.Get(); }
        public void SetCallbacks(IWallJumpActions instance)
        {
            if (m_Wrapper.m_WallJumpActionsCallbackInterface != null)
            {
                @Jump.started -= m_Wrapper.m_WallJumpActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_WallJumpActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_WallJumpActionsCallbackInterface.OnJump;
                @Move.started -= m_Wrapper.m_WallJumpActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_WallJumpActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_WallJumpActionsCallbackInterface.OnMove;
                @Rotate.started -= m_Wrapper.m_WallJumpActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_WallJumpActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_WallJumpActionsCallbackInterface.OnRotate;
            }
            m_Wrapper.m_WallJumpActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
            }
        }
    }
    public WallJumpActions @WallJump => new WallJumpActions(this);
    private int m_NewcontrolschemeSchemeIndex = -1;
    public InputControlScheme NewcontrolschemeScheme
    {
        get
        {
            if (m_NewcontrolschemeSchemeIndex == -1) m_NewcontrolschemeSchemeIndex = asset.FindControlSchemeIndex("New control scheme");
            return asset.controlSchemes[m_NewcontrolschemeSchemeIndex];
        }
    }
    public interface IGround_MoveActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
        void OnSlide(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
    }
    public interface IAir_MoveActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
    }
    public interface ISlidingActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
        void OnSlide(InputAction.CallbackContext context);
    }
    public interface IDashActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
    }
    public interface IWallJumpActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
    }
}
