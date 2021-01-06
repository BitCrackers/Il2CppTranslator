using Il2CppInspector.PluginAPI;
using Il2CppInspector.PluginAPI.V100;
using Il2CppTranslator.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Il2CppTranslator
{
    public class Translator
    {
        public static void StartTranslating(object plugin)
        {
            if (!(plugin is IPlugin)) throw new ArgumentException("Argument must implement IPlugin");

            PluginServices services = PluginServices.For((IPlugin)plugin);

            foreach (var translator in LocateTranslators(plugin.GetType().Assembly))
            {
                services.StatusUpdate($"Translating {translator.Name}");
                translator.Initialize();
                translator.TranslateFields();
                translator.TranslateMethods();
                translator.TranslateDerivedTypes();
            }
        }

        private static IEnumerable<ITranslator> LocateTranslators(Assembly assembly)
        {
            var translators = assembly.GetTypes().Where(t => typeof(ITranslator).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract).Select(t => (ITranslator)Activator.CreateInstance(t));
            return translators.TopologicalSort(t => t.GetType(), t => t.Dependencies);
        }
    }
}