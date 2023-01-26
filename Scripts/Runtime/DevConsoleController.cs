using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Yogi.UnityDevConsole.Scripts.Runtime {
    internal class DevConsoleController {
        private static Dictionary<string, MethodInfo> _commands;

        internal static void ExecuteCommand(string methodName, params string[] args) {
            if (_commands == null || !_commands.ContainsKey(methodName)) {
                Debug.LogWarning($"[UnityDevConsole]: Command `{methodName}` is not registered.");
                return;
            }

            var methodInfo = _commands[methodName];
            var parametersInfo = methodInfo.GetParameters();
            if (parametersInfo.Length != args.Length) {
                Debug.LogWarning($"[UnityDevConsole]: Command `{methodName}` requires {parametersInfo.Length} args, while {args.Length} were provided.");
                return;
            }

            var parameters = new object[parametersInfo.Length];
            for (int i = 0; i < args.Length; i++) {
                parameters[i] = Convert.ChangeType(args[i], parametersInfo[i].ParameterType, System.Globalization.CultureInfo.InvariantCulture);
            }

            methodInfo.Invoke(null, parameters);
        }

        internal static void RegisterCommands() {
            _commands = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => !assembly.IsDynamic)
                .SelectMany(assembly => assembly.GetExportedTypes())
                .SelectMany(type => type.GetMethods(BindingFlags.Static | BindingFlags.Public))
                .Where(method => method.GetCustomAttribute<DevConsoleCommandAttribute>() != null)
                .ToDictionary(method => method.GetCustomAttribute<DevConsoleCommandAttribute>().CommandName, StringComparer.OrdinalIgnoreCase);
        }
    }
}
