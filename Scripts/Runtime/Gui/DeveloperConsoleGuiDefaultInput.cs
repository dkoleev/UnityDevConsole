﻿using UnityEngine;

namespace Yogi.UnityDevConsole.Scripts.Runtime.Gui {
    public class DeveloperConsoleGuiDefaultInput : DevConsoleGUI {
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
            if (Event.current.keyCode == KeyCode.BackQuote) {
                if (!ConsoleIsActive) {
                    ConsoleIsActive = true;
                    _commandDelayCurrent = _commandDelay;
                }
            }
        }

        protected override void HandleKeyboardInGUI() {
            if (_commandDelayCurrent <= 0) {
                if (Event.current.keyCode == KeyCode.Return) {
                    HandleEnterCommand();
                    GUI.FocusControl("inputField");
                    _commandDelayCurrent = _commandDelay;
                }
                /*else if (Event.current.keyCode == KeyCode.UpArrow) {
                    _input = _console.GetBufferCommand(false);
                    GUI.FocusControl("inputField");
                    _commandDelayCurrent = _commandDelay;
                }else if (Event.current.keyCode == KeyCode.DownArrow) {
                    _input = _console.GetBufferCommand(true);
                    GUI.FocusControl("inputField");
                    _commandDelayCurrent = _commandDelay;
                } */
                else if (ConsoleIsActive && 
                           (Event.current.keyCode == KeyCode.BackQuote || Event.current.keyCode == KeyCode.Escape)) {
                    HandleEscape();
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