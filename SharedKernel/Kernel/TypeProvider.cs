using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;

namespace Core.Kernel
{
    public static class TypeProvider
    {
        private static readonly ConcurrentDictionary<Assembly, Type[]> _types = new();

        /// <summary>
        /// Возвращаем все типы сборки
        /// </summary>
        /// <param name="currentAssembly">Сборка, типы которой нам нужны</param>
        /// <returns></returns>
        public static Type[] GetTypesByAssembly(Assembly currentAssembly)
        {
            if (!_types.TryGetValue(currentAssembly, out Type[]? cachedTypes))
            {
                cachedTypes = currentAssembly.GetTypes();
                _types[currentAssembly] = cachedTypes;
            }
            return cachedTypes!;
        }

        /// <summary>
        /// Свойство возвращает все типы сборки, откуда мы вызываем это свойство.
        /// !!!Возможны проблемы с производительностью!!!
        /// </summary>
        public static Type[] Types
        {
            get
            {
                StackFrame? frame = new StackTrace(1, false).GetFrame(0);
                Assembly currentAssembly = frame?.GetMethod()?.DeclaringType?.Assembly ?? Assembly.GetExecutingAssembly();
                return GetTypesByAssembly(currentAssembly);
            }
        }
    }
}