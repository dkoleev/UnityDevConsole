using System.Collections.Generic;
using UnityEngine;

namespace Yogi.UnityDevConsole.Scripts.Runtime.Gui {
    public class DeveloperConsoleGuiDefaultInput : DevConsoleGUI {
        [SerializeField] private KeyCode openConsoleKeyCode = KeyCode.BackQuote;
        [SerializeField] [Tooltip("When you invoke a command it's add to commands buffer.")] private KeyCode prevCommandInBuffer = KeyCode.UpArrow;
        [SerializeField] [Tooltip("When you invoke a command it's add to commands buffer.")] private KeyCode nextCommandInBuffer = KeyCode.DownArrow;
        [SerializeField] private List<KeyCode> closeConsoleKeyCodes = new List<KeyCode>();
        
        private float _commandDelay = 0.1f;
        private float _commandDelayCurrent;

        private void Update() {
#if ENABLE_DEV_CONSOLE
            _commandDelayCurrent -= Time.deltaTime;
            if (_commandDelayCurrent < 0) {
                _commandDelayCurrent = 0;
            }
#endif
        }

        protected override void HandleShowConsole() {
            if (_commandDelayCurrent <= 0) {
                if (Event.current.keyCode == openConsoleKeyCode) {
                    if (!ConsoleIsActive) {
                        ConsoleIsActive = true;
                        _commandDelayCurrent = _commandDelay;
                    }
                }
            }
        }

        protected override void HandleKeyboardInGUI() {
            if (_commandDelayCurrent <= 0) {
                if (ConsoleIsActive && Event.current.isKey && closeConsoleKeyCodes.Contains(Event.current.keyCode)) {
                    HandleEscape();
                    _commandDelayCurrent = _commandDelay;
                    return;
                }
                
                if (Event.current.keyCode == KeyCode.Return) {
                    HandleEnterCommand();
                    GUI.FocusControl("inputField");
                    _commandDelayCurrent = _commandDelay;
                }
                else if (Event.current.keyCode == prevCommandInBuffer && !DevConsoleController.InputBufferEmpty) {
                    _input = DevConsoleController.GetBufferCommand(true);
                    GUI.FocusControl("inputField");
                    _commandDelayCurrent = _commandDelay;
                }else if (Event.current.keyCode == nextCommandInBuffer && !DevConsoleController.InputBufferEmpty) {
                    _input = DevConsoleController.GetBufferCommand(false);
                    GUI.FocusControl("inputField");
                    _commandDelayCurrent = _commandDelay;
                }
            }
        }
        
        protected override void SetFocusTextField() {
            if (!_inputFocus && _commandDelayCurrent <= 0) {
                _inputFocus = true;
                GUI.FocusControl("inputField");
                _input = string.Empty;
            }
        }
    }
}