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
    public class Translator
    {
        public static void StartTranslating(object plugin)
        {
            if (!(plugin is IPlugin)) throw new ArgumentException("Argument must implement IPlugin");

            PluginServices services = PluginServices.For((IPlugin)plugin);

            foreach (var atr in LocateTranslators(plugin.GetType().Assembly))
            {
                object instance = Activator.CreateInstance(atr);
                if (!(instance is ITranslator)) throw new CustomAttributeFormatException("Classes with the translator attribute must implement ITranslator");

                services.StatusUpdate($"Translating {atr.Name}");
                ITranslator translator = (ITranslator)instance;
                translator.Translator.SetName(atr.Name);
                translator.Translate();
            }
        }
        static IEnumerable<Type> LocateTranslators(Assembly assembly)
        {
            var translators = assembly.GetTypes().Where(type => Attribute.IsDefined(type, typeof(TranslatorAttribute)));
            return translators.TopologicalSort(TranslatorAttribute.GetKey, TranslatorAttribute.GetDependencies);
        }
    }
}
