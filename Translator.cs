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
        private IPlugin Plugin { get; }
        private List<TypeTranslator> Translators = new List<TypeTranslator>();
        public Translator(object plugin)
        {
            if (!(plugin is IPlugin)) throw new ArgumentException("Argument must implement IPlugin");
            Plugin = (IPlugin)plugin;
        }
        public void StartTranslating()
        {
            PluginServices services = PluginServices.For(Plugin);

            foreach (var translator in LocateTranslators(Plugin.GetType().Assembly))
            {
                services.StatusUpdate($"Translating {translator.Name}");
                translator.Initialize(this);
                translator.TranslateFields();
                translator.TranslateMethods();
                translator.TranslateDerivedTypes();
            }
        }

        public IDictionary<string, string> GetTranslations()
        {
            IDictionary<string, string> translations = new Dictionary<string, string>();
            foreach(TypeTranslator typeTranslator in Translators)
            {
                if (typeTranslator.GetNameTranslation().Value != null) translations.Add(typeTranslator.GetNameTranslation());
                if (typeTranslator.GetNameSpaceTranslation().Value != null) translations.Add(typeTranslator.GetNameSpaceTranslation());
                typeTranslator.GetFields().ForEach((fieldTranslator) => {
                    if (fieldTranslator.GetFieldNameTranslation().Key != null || fieldTranslator.GetFieldNameTranslation().Value != null) translations.Add(fieldTranslator.GetFieldNameTranslation());
                    if (fieldTranslator.GetTypeNameTranslation().Key != null || fieldTranslator.GetTypeNameTranslation().Value != null) translations.Add(fieldTranslator.GetTypeNameTranslation());
                });
            }
            return translations;
        }

        private IEnumerable<ITranslator> LocateTranslators(Assembly assembly)
        {
            var translators = assembly.GetTypes().Where(t => typeof(ITranslator).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract).Select(t => (ITranslator)Activator.CreateInstance(t));
            return translators.TopologicalSort(t => t.GetType(), t => t.Dependencies);
        }

        internal void AddTranslator(TypeTranslator translator) => Translators.Add(translator);
    }
}