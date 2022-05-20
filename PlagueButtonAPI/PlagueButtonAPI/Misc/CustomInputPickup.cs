using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IL2CPPAssetBundleAPI;
using MelonLoader;
using PlagueButtonAPI.External_Libraries;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using VRC.SDKBase;
using Object = UnityEngine.Object;

namespace PlagueButtonAPI.Misc
{
    public class CustomInputPickup
    {
        public class KBConfig
        {
            public float Scale = 1f;
            public bool Pickup = true;
        }

        internal static ConfigLib<KBConfig> Config = new ConfigLib<KBConfig>(Environment.CurrentDirectory + "\\KBConfig.json");

        [RegisterTypeInIl2Cpp]
        internal class FreezeControlsTMP : MonoBehaviour
        {
            public FreezeControlsTMP(IntPtr instance) : base(instance) { }

            internal Action OnExit;
            internal Action OnEnterKeyPressed;

            internal TMP_InputField inputField;

            private VRC.UI.Elements.QuickMenu qm;

            void Start()
            {
                inputField = gameObject.GetComponent<TMP_InputField>();

                qm = ButtonAPI.GetQuickMenuInstance();
            }

            void OnDisable()
            {
                VRCInputManager.Method_Public_Static_Void_Boolean_0(false);
                qm.field_Private_Boolean_5 = false;
            }

            void OnDestroy()
            {
                VRCInputManager.Method_Public_Static_Void_Boolean_0(false);
                qm.field_Private_Boolean_5 = false;

                OnExit?.Invoke();
            }

            void Update()
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    OnEnterKeyPressed?.Invoke();
                }

                var ShouldLockControls = inputField.isFocused;

                VRCInputManager.Method_Public_Static_Void_Boolean_0(ShouldLockControls);
                qm.field_Private_Boolean_5 = ShouldLockControls;
            }
        }

        private static GameObject KB_Template;

        internal static void Init()
        {
            if (new IL2CPPAssetBundle() is var Bundle && Bundle.LoadBundle(Assembly.GetExecutingAssembly(), "PlagueButtonAPI.Resources.keyboard.asset"))
            {
                KB_Template = Bundle.Load<GameObject>("Keyboard.prefab");
            }
        }

        public static GameObject Spawn(Vector3 Location, Action<string> OnConfirm) => Spawn(Location, OnConfirm, new Quaternion(0f, 0f, 0f, 1f));

        public static GameObject Spawn(Vector3 Location, Action<string> OnConfirm, Quaternion Rotation)
        {
            var KB = Object.Instantiate(KB_Template).transform.Find("KB").gameObject;

            KB.AddComponent<VRCSDK2.VRC_UiShape>();

            KB.transform.localScale = new Vector3(KB.transform.localScale.x, KB.transform.localScale.x, KB.transform.localScale.x);

            // Fix Colours
            foreach (var button in KB.GetComponentsInChildren<Button>(true))
            {
                var newBlock = button.colors;

                newBlock.selectedColor = newBlock.normalColor;

                button.colors = newBlock;
            }

            var pickup = KB.transform.parent.gameObject.AddComponent<VRCSDK2.VRC_Pickup>();
            pickup.AutoHold = VRC_Pickup.AutoHoldMode.No;
            pickup.pickupable = true;
            pickup.allowManipulationWhenEquipped = true;

            var Input = KB.transform.Find("InputField").GetComponent<TMP_InputField>();

            if (!XRDevice.isPresent)
            {
                Input.gameObject.AddComponent<FreezeControlsTMP>();
            }

            var CaretPos = 0;

            var IsShifted = false;

            void SetShift(bool shifted)
            {
                IsShifted = shifted;

                KB.transform.Find("MainKeys/FunctionKeys/ToggleShift-Left/0x10-down").gameObject.SetActive(!shifted);
                KB.transform.Find("MainKeys/FunctionKeys/ToggleShift-Left/0x10-up").gameObject.SetActive(shifted);

                KB.transform.Find("MainKeys/FunctionKeys/ToggleShift-Right/0x10-down").gameObject.SetActive(!shifted);
                KB.transform.Find("MainKeys/FunctionKeys/ToggleShift-Right/0x10-up").gameObject.SetActive(shifted);

                KB.transform.Find("MainKeys/NormalKeys/InactiveCapsLock/InactiveShift").gameObject.SetActive(!shifted);
                KB.transform.Find("MainKeys/NormalKeys/InactiveCapsLock/ActiveShift").gameObject.SetActive(shifted);

                KB.transform.Find("MainKeys/NormalKeys/ActiveCapsLock/InactiveShift").gameObject.SetActive(!shifted);
                KB.transform.Find("MainKeys/NormalKeys/ActiveCapsLock/ActiveShift").gameObject.SetActive(shifted);
            }

            void SetCaret(TMP_InputField field, int pos)
            {
                CaretPos = pos;
                field.caretPosition = pos; // desired cursor position
                field.m_AllowInput = true;
                field.SetCaretVisible();
            }

            Input.onValueChanged.AddListener(new Action<string>(s =>
            {
                SetCaret(Input, Input.text.Length - 1);
            }));

            // Plain Letters
            foreach (var button in KB.transform.Find("MainKeys/NormalKeys").GetComponentsInChildren<Button>(true))
            {
                var Text = button.transform.Find("Text")?.GetComponent<Text>();

                if (Text != null && Text.text.Length == 1)
                {
                    button.onClick = new Button.ButtonClickedEvent();

                    button.onClick.AddListener(new Action(() =>
                    {
                        Text = IsShifted ? (button.transform.Find("Text (1)")?.GetComponent<Text>() ?? button.transform.Find("Text")?.GetComponent<Text>()) : button.transform.Find("Text")?.GetComponent<Text>(); // Re-Grab For Shift Support

                        //MelonLogger.Msg($"{Text.text.First()} Pressed!");
                        Input.Insert(Text.text.First());
                        Input.ForceLabelUpdate();
                        SetCaret(Input, Input.text.Length - 1);

                        SetShift(false);
                    }));

                    //MelonLogger.Msg($"Function For {Text.text} Added!");
                }
            }

            KB.transform.Find("MainKeys/Spacebar").GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            KB.transform.Find("MainKeys/Spacebar").GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                //MelonLogger.Msg("Spacebar Pressed!");
                Input.Insert(' ');
                Input.ForceLabelUpdate();
                SetCaret(Input, Input.text.Length - 1);
            }));

            KB.transform.Find("MainKeys/FunctionKeys/0x1B").GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            KB.transform.Find("MainKeys/FunctionKeys/0x1B").GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                //MelonLogger.Msg("Cancel Pressed!");
                Object.Destroy(KB);
            }));

            KB.transform.Find("MainKeys/FunctionKeys/0x08").GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            KB.transform.Find("MainKeys/FunctionKeys/0x08").GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                //MelonLogger.Msg("Backspace Pressed!");
                Input.Backspace();
                Input.ForceLabelUpdate();
                SetCaret(Input, Input.text.Length - 1);
            }));

            KB.transform.Find("MainKeys/FunctionKeys/0x09").GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            KB.transform.Find("MainKeys/FunctionKeys/0x09").GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                //MelonLogger.Msg("Tab Pressed!");
                Input.Insert("  "[0]);
                Input.ForceLabelUpdate();
                SetCaret(Input, Input.text.Length - 1);
            }));

            KB.transform.Find("MainKeys/FunctionKeys/0x0D").GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            KB.transform.Find("MainKeys/FunctionKeys/0x0D").GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                //MelonLogger.Msg("Enter / Confirm Pressed!");
                OnConfirm?.Invoke(Input.text);
                Object.Destroy(KB);
            }));

            KB.transform.Find("MainKeys/FunctionKeys/ToggleCapsLock/0x14-down").GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            KB.transform.Find("MainKeys/FunctionKeys/ToggleCapsLock/0x14-down").GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                //MelonLogger.Msg("Caps Lock On Pressed!");

                const bool CapsLock = true;
                
                //Update Indicator
                KB.transform.Find("Indicators/CapsLock/Up").gameObject.SetActive(!CapsLock);
                KB.transform.Find("Indicators/CapsLock/Down").gameObject.SetActive(CapsLock);

                KB.transform.Find("MainKeys/FunctionKeys/ToggleCapsLock/0x14-down").gameObject.SetActive(!CapsLock);
                KB.transform.Find("MainKeys/FunctionKeys/ToggleCapsLock/0x14-up").gameObject.SetActive(CapsLock);

                KB.transform.Find("MainKeys/NormalKeys/InactiveCapsLock").gameObject.SetActive(!CapsLock);
                KB.transform.Find("MainKeys/NormalKeys/ActiveCapsLock").gameObject.SetActive(CapsLock);
            }));

            KB.transform.Find("MainKeys/FunctionKeys/ToggleCapsLock/0x14-up").GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            KB.transform.Find("MainKeys/FunctionKeys/ToggleCapsLock/0x14-up").GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                //MelonLogger.Msg("Caps Lock Off Pressed!");

                const bool CapsLock = false;

                //Update Indicator
                KB.transform.Find("Indicators/CapsLock/Up").gameObject.SetActive(!CapsLock);
                KB.transform.Find("Indicators/CapsLock/Down").gameObject.SetActive(CapsLock);

                KB.transform.Find("MainKeys/FunctionKeys/ToggleCapsLock/0x14-down").gameObject.SetActive(!CapsLock);
                KB.transform.Find("MainKeys/FunctionKeys/ToggleCapsLock/0x14-up").gameObject.SetActive(CapsLock);

                KB.transform.Find("MainKeys/NormalKeys/InactiveCapsLock").gameObject.SetActive(!CapsLock);
                KB.transform.Find("MainKeys/NormalKeys/ActiveCapsLock").gameObject.SetActive(CapsLock);
            }));

            KB.transform.Find("MainKeys/FunctionKeys/ToggleShift-Left/0x10-down").GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            KB.transform.Find("MainKeys/FunctionKeys/ToggleShift-Left/0x10-down").GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                //MelonLogger.Msg("Left Shift On Pressed!");
                SetShift(true);
            }));

            KB.transform.Find("MainKeys/FunctionKeys/ToggleShift-Left/0x10-up").GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            KB.transform.Find("MainKeys/FunctionKeys/ToggleShift-Left/0x10-up").GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                //MelonLogger.Msg("Left Shift Off Pressed!");
                SetShift(false);
            }));

            KB.transform.Find("MainKeys/FunctionKeys/ToggleShift-Right/0x10-down").GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            KB.transform.Find("MainKeys/FunctionKeys/ToggleShift-Right/0x10-down").GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                //MelonLogger.Msg("Right Shift On Pressed!");
                SetShift(true);
            }));

            KB.transform.Find("MainKeys/FunctionKeys/ToggleShift-Right/0x10-up").GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            KB.transform.Find("MainKeys/FunctionKeys/ToggleShift-Right/0x10-up").GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                //MelonLogger.Msg("Right Shift Off Pressed!");
                SetShift(false);
            }));

            KB.transform.Find("MainKeys/FunctionKeys/0x25").GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            KB.transform.Find("MainKeys/FunctionKeys/0x25").GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                //MelonLogger.Msg("Left Arrow Pressed!");
                SetCaret(Input, (CaretPos - 1 >= 0 ? CaretPos - 1 : CaretPos));
            }));

            KB.transform.Find("MainKeys/FunctionKeys/0x27").GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            KB.transform.Find("MainKeys/FunctionKeys/0x27").GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                //MelonLogger.Msg("Right Arrow Pressed!");
                SetCaret(Input, (CaretPos + 1 < Input.text.Length ? CaretPos + 1 : CaretPos));
            }));

            KB.transform.Find("MainKeys/Pickuppable").GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            KB.transform.Find("MainKeys/Pickuppable").GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                //MelonLogger.Msg("Pickup Toggle Pressed!");
                var Text = KB.transform.Find("MainKeys/Pickuppable/Text").GetComponent<Text>();
                var ToggleState = Text.color != Color.green; // White When They Clicked It, Thus ToggleState True After.

                KB.transform.Find("Pickup Grab Area").gameObject.SetActive(ToggleState);
                pickup.pickupable = ToggleState;
                Config.InternalConfig.Pickup = ToggleState;

                Text.color = ToggleState ? Color.green : Color.white;
            }));

            KB.transform.Find("MainKeys/DecreaseScale").GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            KB.transform.Find("MainKeys/DecreaseScale").GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                //MelonLogger.Msg("Decrease Scale Pressed!");
                KB.transform.parent.localScale -= new Vector3(0.01f, 0.01f, 0.01f);

                Config.InternalConfig.Scale = KB.transform.parent.localScale.x;
            }));

            KB.transform.Find("MainKeys/IncreaseScale").GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            KB.transform.Find("MainKeys/IncreaseScale").GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                //Me1nLogger.Msg("Increase Scale Pressed!");
                KB.transform.parent.localScale += new Vector3(0.01f, 0.01f, 0.01f);

                Config.InternalConfig.Scale = KB.transform.parent.localScale.x;
            }));

            KB.transform.parent.position = Location;
            KB.transform.parent.localRotation = Rotation;
            KB.transform.parent.localScale = new Vector3(Config.InternalConfig.Scale, Config.InternalConfig.Scale, Config.InternalConfig.Scale);

            if (!Config.InternalConfig.Pickup)
            {
                KB.transform.Find("MainKeys/Pickuppable/Text").GetComponent<Text>().color = Color.white;

                KB.transform.Find("Pickup Grab Area").gameObject.SetActive(false);
                pickup.pickupable = false;
            }

            return KB;
        }
    }
}