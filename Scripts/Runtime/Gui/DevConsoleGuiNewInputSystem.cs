using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Yogi.UnityDevConsole.Scripts.Runtime.Gui {
    public class DevConsoleGuiNewInputSystem : DevConsoleGUI {
        private void Update() {
#if ENABLE_INPUT_SYSTEM && ENABLE_DEV_CONSOLE
            var keyboard = Keyboard.current;
            if (keyboard.backquoteKey.wasPressedThisFrame) {
                if (!ConsoleIsActive) {
                    ConsoleIsActive = true;
                }
            }else if (keyboard.enterKey.wasPressedThisFrame) {
                HandleEnterCommand();
            }else if (keyboard.escapeKey.wasPressedThisFrame) {
                if (ConsoleIsActive) {
                    HandleEscape();
                }
            }else if (keyboard.upArrowKey.wasPressedThisFrame) {
                _input = DevConsoleController.GetBufferCommand(true);
                GUI.FocusControl("inputField");
            }else if (keyboard.downArrowKey.wasPressedThisFrame) {
                _input = DevConsoleController.GetBufferCommand(false);
                GUI.FocusControl("inputField");
            }
#endif
        }
    }
}
