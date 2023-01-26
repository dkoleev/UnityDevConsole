using System;

namespace Yogi.UnityDevConsole.Scripts.Runtime {
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class DevConsoleCommandAttribute : Attribute {
        public readonly string CommandName;

        public DevConsoleCommandAttribute(string commandName) {
            CommandName = commandName;
        }
    }
}