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
            ""name"": ""Move"",
            ""id"": ""e8a865ae-1c37-4c53-8eca-aea200a30cd4"",
            ""actions"": [
                {
                    ""name"": ""Turn"",
                    ""type"": ""Value"",
                    ""id"": ""5074b856-ae64-4bb0-8dd0-6440f4d54ecc"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""ba34b786-73a5-4443-bee8-6d06b0c0b9cf"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Accelerate"",
                    ""type"": ""Button"",
                    ""id"": ""b52a9ad6-0256-4309-8481-cb5874900d4c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rise"",
                    ""type"": ""Value"",
                    ""id"": ""3e26f876-c614-48ac-9f7e-5c5d734c2dc6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fall"",
                    ""type"": ""Value"",
                    ""id"": ""9acae49b-d052-404f-849b-cbd8efe96c47"",
                    ""expectedControlType"": ""Analog"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Boost"",
                    ""type"": ""Button"",
                    ""id"": ""5aa6692b-a2ea-41a2-8d02-2302894a910a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""ff9e56f7-9aee-41e6-9494-25b2886cb61d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d00762e8-fd97-4ac5-a13d-79ed294962c0"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Turn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""993b1fcf-2ac1-4acf-96ee-c786ffc3b599"",
                    ""path"": ""<SwitchProControllerHID>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Turn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fd6793bd-7fe4-4c96-81bf-1781facf7682"",
                    ""path"": ""<WebGLGamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Turn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7feba8a6-8f2d-4dd3-a409-53e171866a59"",
                    ""path"": ""<XboxOneGampadiOS>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Turn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b07072a4-0569-477a-b89c-931d8614bc22"",
                    ""path"": ""<DualShockGamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Turn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""454a9517-051d-45bc-a942-e8fbce37fa6c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Turn"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b13555b9-754a-4814-ad9d-c5426336514f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Turn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ca1e6c23-4e28-486f-8aa9-48f6e1f98edb"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Turn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""08e599a3-3039-459f-b4eb-d5ca0be53962"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Turn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""18774323-d750-4f14-9b8c-8e0301d7ce5a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Turn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2be6137a-6a06-4663-a9d8-a885b25cbdb5"",
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
                    ""id"": ""16df3b78-745d-46c0-8b4e-3a433b7afc70"",
                    ""path"": ""<SwitchProControllerHID>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0e2ec9e4-6223-41f4-bc3a-259861236434"",
                    ""path"": ""<XboxOneGampadiOS>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1671a650-cad2-4196-8b58-5118e2234ba8"",
                    ""path"": ""<WebGLGamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""96d0d03c-b9ec-4617-97e2-b75e7434eab2"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=0.1)"",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0419296f-8780-4c38-90f9-58e0497f0643"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accelerate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bc44c325-5e85-4903-8d1f-f5655ffa3442"",
                    ""path"": ""<SwitchProControllerHID>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accelerate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4d0bd16c-f006-4369-94b5-b2207cca86cb"",
                    ""path"": ""<WebGLGamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accelerate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2e70b739-eb9d-4224-b15b-95fc5e739b1a"",
                    ""path"": ""<DualShockGamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accelerate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""abc8c5d6-17e2-4d8b-8188-86a58ebfd05a"",
                    ""path"": ""<XInputController>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accelerate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f4293c12-17c0-4447-a805-533e4fbc5ada"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accelerate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""97f94600-b1c6-451a-b4a2-51c005a73dbe"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accelerate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a7bdecc8-7b62-4a22-9cf0-cfb2d45c208c"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rise"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""33214363-5399-4028-b96a-4708d683742e"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rise"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""ed36d71f-0da9-420e-bc1a-d8c145c47def"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rise"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""c934f068-f075-4ca6-a70b-6225d1adb2e7"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rise"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""87bd8302-e382-44d5-b955-353a90f85081"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fall"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""67bc4e62-46f8-4ec0-bf6b-e24fa4af859c"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fall"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""63f5a52f-819d-4c83-86ad-0e4991778c02"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fall"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""1d70bd74-82b9-4c65-a6c0-d2f9f2647d0c"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fall"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""efaaa244-e988-4697-b62c-bfa456700c7f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fall"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""43943971-88b7-4787-85a3-8ebd37eb5e54"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""235bc60b-97a5-4dc3-a8c3-b8c8ff72ea8c"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Boost"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""f466a388-1f8d-4e5d-b3bc-e2952dcbd9ee"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""b7ed41e0-a797-410c-8f04-03fdd9678948"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis mouse"",
                    ""id"": ""80e624a9-3740-467d-8a9c-826581675e94"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Boost"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""f0975b0e-e104-4c4c-b28b-75c8cc5746b3"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""e3db2121-fe48-4590-8471-41f9e804d602"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Button With One Modifier"",
                    ""id"": ""dcc8aa6a-e087-4868-84d9-bd92a5470f93"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Boost"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Modifier"",
                    ""id"": ""34a7d0b1-8707-4235-976c-f337333c17e4"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Button"",
                    ""id"": ""088d7d0e-427a-4fbb-87e5-02e87bf811f5"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""21395db8-4ff6-4a60-8a01-5f09176d4ea6"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Move
        m_Move = asset.FindActionMap("Move", throwIfNotFound: true);
        m_Move_Turn = m_Move.FindAction("Turn", throwIfNotFound: true);
        m_Move_Rotate = m_Move.FindAction("Rotate", throwIfNotFound: true);
        m_Move_Accelerate = m_Move.FindAction("Accelerate", throwIfNotFound: true);
        m_Move_Rise = m_Move.FindAction("Rise", throwIfNotFound: true);
        m_Move_Fall = m_Move.FindAction("Fall", throwIfNotFound: true);
        m_Move_Boost = m_Move.FindAction("Boost", throwIfNotFound: true);
        m_Move_Attack = m_Move.FindAction("Attack", throwIfNotFound: true);
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

    // Move
    private readonly InputActionMap m_Move;
    private IMoveActions m_MoveActionsCallbackInterface;
    private readonly InputAction m_Move_Turn;
    private readonly InputAction m_Move_Rotate;
    private readonly InputAction m_Move_Accelerate;
    private readonly InputAction m_Move_Rise;
    private readonly InputAction m_Move_Fall;
    private readonly InputAction m_Move_Boost;
    private readonly InputAction m_Move_Attack;
    public struct MoveActions
    {
        private @PlayerControls m_Wrapper;
        public MoveActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Turn => m_Wrapper.m_Move_Turn;
        public InputAction @Rotate => m_Wrapper.m_Move_Rotate;
        public InputAction @Accelerate => m_Wrapper.m_Move_Accelerate;
        public InputAction @Rise => m_Wrapper.m_Move_Rise;
        public InputAction @Fall => m_Wrapper.m_Move_Fall;
        public InputAction @Boost => m_Wrapper.m_Move_Boost;
        public InputAction @Attack => m_Wrapper.m_Move_Attack;
        public InputActionMap Get() { return m_Wrapper.m_Move; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MoveActions set) { return set.Get(); }
        public void SetCallbacks(IMoveActions instance)
        {
            if (m_Wrapper.m_MoveActionsCallbackInterface != null)
            {
                @Turn.started -= m_Wrapper.m_MoveActionsCallbackInterface.OnTurn;
                @Turn.performed -= m_Wrapper.m_MoveActionsCallbackInterface.OnTurn;
                @Turn.canceled -= m_Wrapper.m_MoveActionsCallbackInterface.OnTurn;
                @Rotate.started -= m_Wrapper.m_MoveActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_MoveActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_MoveActionsCallbackInterface.OnRotate;
                @Accelerate.started -= m_Wrapper.m_MoveActionsCallbackInterface.OnAccelerate;
                @Accelerate.performed -= m_Wrapper.m_MoveActionsCallbackInterface.OnAccelerate;
                @Accelerate.canceled -= m_Wrapper.m_MoveActionsCallbackInterface.OnAccelerate;
                @Rise.started -= m_Wrapper.m_MoveActionsCallbackInterface.OnRise;
                @Rise.performed -= m_Wrapper.m_MoveActionsCallbackInterface.OnRise;
                @Rise.canceled -= m_Wrapper.m_MoveActionsCallbackInterface.OnRise;
                @Fall.started -= m_Wrapper.m_MoveActionsCallbackInterface.OnFall;
                @Fall.performed -= m_Wrapper.m_MoveActionsCallbackInterface.OnFall;
                @Fall.canceled -= m_Wrapper.m_MoveActionsCallbackInterface.OnFall;
                @Boost.started -= m_Wrapper.m_MoveActionsCallbackInterface.OnBoost;
                @Boost.performed -= m_Wrapper.m_MoveActionsCallbackInterface.OnBoost;
                @Boost.canceled -= m_Wrapper.m_MoveActionsCallbackInterface.OnBoost;
                @Attack.started -= m_Wrapper.m_MoveActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_MoveActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_MoveActionsCallbackInterface.OnAttack;
            }
            m_Wrapper.m_MoveActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Turn.started += instance.OnTurn;
                @Turn.performed += instance.OnTurn;
                @Turn.canceled += instance.OnTurn;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
                @Accelerate.started += instance.OnAccelerate;
                @Accelerate.performed += instance.OnAccelerate;
                @Accelerate.canceled += instance.OnAccelerate;
                @Rise.started += instance.OnRise;
                @Rise.performed += instance.OnRise;
                @Rise.canceled += instance.OnRise;
                @Fall.started += instance.OnFall;
                @Fall.performed += instance.OnFall;
                @Fall.canceled += instance.OnFall;
                @Boost.started += instance.OnBoost;
                @Boost.performed += instance.OnBoost;
                @Boost.canceled += instance.OnBoost;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
            }
        }
    }
    public MoveActions @Move => new MoveActions(this);
    public interface IMoveActions
    {
        void OnTurn(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
        void OnAccelerate(InputAction.CallbackContext context);
        void OnRise(InputAction.CallbackContext context);
        void OnFall(InputAction.CallbackContext context);
        void OnBoost(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
    }
}
