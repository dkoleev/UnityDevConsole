using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Yogi.UnityDevConsole.Scripts.Runtime {
    internal class DevConsoleController {
        public static bool InputBufferEmpty => _inputBuffer.Count == 0;
        
        private static Dictionary<string, MethodInfo> _commands;
        private static readonly List<string> _inputBuffer = new List<string>();
        private static int _inputBufferIndex = 0;

        public static string GetBufferCommand(bool prev) {
            _inputBufferIndex += prev ? -1 : 1;
            if (_inputBufferIndex < 0) {
                _inputBufferIndex = _inputBuffer.Count - 1;
            }else if (_inputBufferIndex >= _inputBuffer.Count) {
                _inputBufferIndex = 0;
            }

            return _inputBuffer[_inputBufferIndex];
        }

        internal static void ExecuteCommand(string input) {
            var command = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (command.Length == 0) {
                return;
            }

            string[] args = Array.Empty<string>();
            var methodName = command[0];
            if (command.Length > 1) {
                args = command.ToList().GetRange(1, command.Length - 1).ToArray();
            }

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

            if (!_inputBuffer.Contains(input)) {
                _inputBuffer.Add(input);
            }
            _inputBufferIndex = 0;
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
