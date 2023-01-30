using UnityEngine;

namespace Yogi.UnityDevConsole.Scripts.Runtime.Gui {
    public abstract class DevConsoleGUI : MonoBehaviour {
        [SerializeField] [Range(0.1f, 2f)] protected float scale = 1f;
        
        protected string Input;
        protected Vector2 Scroll;
        protected bool ConsoleIsActive;
        protected int InputBufferIndex = 0;
        protected const string InputFieldName = "inputField";
        
        private bool _inputFocused;
        private bool _helpMenuIsActive;

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
                if (string.IsNullOrWhiteSpace(Input)) {
                    return;
                }

                DevConsoleController.ExecuteCommand(Input);
                Input = string.Empty;
            }
        }

        protected void Hide() {
            if (_helpMenuIsActive) {
                _helpMenuIsActive = false;
                GUI.FocusControl(InputFieldName);
            } else {
                Input = string.Empty;
                ConsoleIsActive = false;
                _inputFocused = false;
            }
        }

        protected virtual void HandleKeyboardInGUI() {
        }
        
        protected virtual void HandleEscape() {
        }

        private void DrawGUI() {
            if (!ConsoleIsActive) {
                return;
            }
            
            var inputHeight = 100 * scale;
            var y = Screen.height - inputHeight;

            if (_helpMenuIsActive) {
                ShowHelp(y);
            }

            GUI.Box(new Rect(0, y, Screen.width, 100 * scale), "");
            GUI.backgroundColor = Color.black;

            var labelStyle = new GUIStyle("TextField");
            labelStyle.fontStyle = FontStyle.Normal;
            var fontSize = 40 * scale;
            labelStyle.fontSize = (int)fontSize;
            labelStyle.alignment = TextAnchor.MiddleLeft;
            GUI.SetNextControlName(InputFieldName);
            Input = GUI.TextField(new Rect(10f, y + 10f, Screen.width - 20, 70f * scale), Input, labelStyle);

            SetFocusTextField();
            HandleEscape();
            if (GUI.GetNameOfFocusedControl() == InputFieldName) {
                 HandleKeyboardInGUI();
            }
        }

        protected virtual void SetFocusTextField() {
            if (!_inputFocused) {
                _inputFocused = true;
                GUI.FocusControl(InputFieldName);
                Input = string.Empty;
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
