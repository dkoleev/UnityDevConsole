using System;
using System.Linq;
using UnityEngine;

namespace Yogi.UnityDevConsole.Scripts.Runtime.Gui {
    public abstract class DevConsoleGUI : MonoBehaviour {
       // [SerializeField] private KeyCode openConsoleKeyCode = KeyCode.BackQuote;
        [SerializeField] [Range(0.1f, 2f)] protected float scale = 1f;
        
        protected string _input;
        protected Vector2 _scroll;
        protected bool _inputFocus;

        protected bool ConsoleIsActive;
        protected bool HelpMenuIsActive;
        
        private void Awake() {
#if ENABLE_DEV_CONSOLE
            RegisterCommands();
#endif
        }
        
        private void RegisterCommands() {
            DevConsoleController.RegisterCommands();
        }
        
        private void OnGUI() {
#if ENABLE_DEV_CONSOLE
            DrawGUI();
#endif
        }
        
        protected void HandleEnterCommand() {
            if (ConsoleIsActive) {
                if (string.IsNullOrWhiteSpace(_input)) {
                    return;
                }

                var command = _input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                switch (command.Length) {
                    case 0:
                        return;
                    case 1:
                        DevConsoleController.ExecuteCommand(command[0]);
                        break;
                    default:
                        DevConsoleController.ExecuteCommand(command[0], command.ToList().GetRange(1, command.Length - 1).ToArray());
                        break;
                }

                _input = "";
            }
        }

        protected void HandleEscape() {
            if (HelpMenuIsActive) {
                HelpMenuIsActive = false;
                GUI.FocusControl("inputField");
            } else {
                ConsoleIsActive = false;
                _inputFocus = false;
            }
        }

        protected virtual void HandleShowConsole() {
        }
        
        protected virtual void HandleKeyboardInGUI() {
        }
        
        private void DrawGUI() {
            if (!ConsoleIsActive) {
                HandleShowConsole();
                return;
            }
            
            HandleKeyboardInGUI();

            var inputHeight = 100 * scale;
            var y = Screen.height - inputHeight;

            if (HelpMenuIsActive) {
                ShowHelp(y);
            }

            GUI.Box(new Rect(0, y, Screen.width, 100 * scale), "");
            GUI.backgroundColor = Color.black;
            
            var labelStyle = new GUIStyle("TextField");
            labelStyle.fontStyle = FontStyle.Normal;
            var fontSize = 40 * scale;
            labelStyle.fontSize = (int)fontSize;
            labelStyle.alignment = TextAnchor.MiddleLeft;
            GUI.SetNextControlName("inputField");
            _input = GUI.TextField(new Rect(10f, y + 10f, Screen.width - 20, 70f * scale), _input, labelStyle);

            SetFocusTextField();
        }

        protected virtual void SetFocusTextField() {
            if (!_inputFocus) {
                _inputFocus = true;
                GUI.FocusControl("inputField");
                _input = string.Empty;
            }
        }

        private void ShowHelp(float y) {
            /*GUI.Box(new Rect(0, y - 500 * scale, Screen.width, 500* scale), "");
            var viewPort = new Rect(0, 0, Screen.width - 30, 80 * _console.Commands.Count * scale);
            _scroll = GUI.BeginScrollView(new Rect(0, y - 480f * scale, Screen.width, 480 * scale), _scroll, viewPort);

            int i = 0;
            foreach (var command in _console.Commands) {
                var label = $"{command.Id} - {command.Description}";
                var labelRect = new Rect(10, 50 * i * scale, viewPort.width-100, 50 * scale);
                    
                var labelStyleHelp = new GUIStyle("label");
                labelStyleHelp.fontStyle = FontStyle.Normal;
                var fontSize = 30 * scale;
                labelStyleHelp.fontSize = (int)fontSize;
                    
                GUI.Label(labelRect, label, labelStyleHelp);
                
                i++;
            }
                
            GUI.EndScrollView();*/
        }
    }
}
