using Il2CppInspector.PluginAPI;
using Il2CppInspector.PluginAPI.V100;
using Il2CppTranslator.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Il2CppTranslator.Translator
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TranslatorAttribute : Attribute
    {
        public string Name { get; private set; } = "";
        public string[] Dependencies { get; private set; } = new string[0];

        public TranslatorAttribute(string Name)
        {
            this.Name = Name;
        }
        public TranslatorAttribute(string Name, params string[] Dependencies)
        {
            this.Name = Name;
            this.Dependencies = Dependencies;
        }

        public static string GetKey(Type source)
        {
            return source.GetCustomAttribute<TranslatorAttribute>().Name;
        }
        public static IEnumerable<string> GetDependencies(Type source)
        {
            return source.GetCustomAttribute<TranslatorAttribute>().Dependencies;
        }
    }
}
