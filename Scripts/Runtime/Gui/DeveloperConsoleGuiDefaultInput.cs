using System.Collections.Generic;
using UnityEngine;

namespace Yogi.UnityDevConsole.Scripts.Runtime.Gui {
    public class DeveloperConsoleGuiDefaultInput : DevConsoleGUI {
        [SerializeField] private KeyCode openConsoleKeyCode = KeyCode.BackQuote;
        [SerializeField] [Tooltip("When you invoke a command it's add to commands buffer.")] private KeyCode prevCommandInBuffer = KeyCode.UpArrow;
        [SerializeField] [Tooltip("When you invoke a command it's add to commands buffer.")] private KeyCode nextCommandInBuffer = KeyCode.DownArrow;
        [SerializeField] private List<KeyCode> closeConsoleKeyCodes = new List<KeyCode>();
        
        private void Update() {
#if ENABLE_DEV_CONSOLE
            if (!ConsoleIsActive) {
                if (UnityEngine.Input.GetKeyUp(openConsoleKeyCode)) {
                    ConsoleIsActive = true;
                }
            }
#endif
        }

        protected override void HandleEscape() {
            if (ConsoleIsActive && Event.current.isKey && closeConsoleKeyCodes.Contains(Event.current.keyCode)) {
                Hide();
            }
        }

        protected override void HandleKeyboardInGUI() {
            if (!Event.current.isKey) {
                return;
            }

            if (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter) {
                HandleEnterCommand();
                GUI.FocusControl(InputFieldName);
            }
            else if (Event.current.keyCode == prevCommandInBuffer && !DevConsoleController.InputBufferEmpty) {
                Input = DevConsoleController.GetBufferCommand(true);
                GUI.FocusControl(InputFieldName);
            }
            else if (Event.current.keyCode == nextCommandInBuffer && !DevConsoleController.InputBufferEmpty) {
                Input = DevConsoleController.GetBufferCommand(false);
                GUI.FocusControl(InputFieldName);
            }
        }
    }
}