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
        public List<Type> Translated;
        public Il2CppInspector.Reflection.TypeModel Model;
        public List<Il2CppInspector.Reflection.TypeInfo> Types;
        public List<Il2CppInspector.Reflection.FieldInfo> Fields;

        public List<(string, string)> Translations;

        public Translator(object plugin, Il2CppInspector.Reflection.TypeModel model)
        {
            Translations = new List<(string, string)>();
            Translated = new List<Type>();

            if (!(plugin is IPlugin)) throw new ArgumentException("Argument 0 must implement IPlugin");
            Plugin = (IPlugin)plugin;
            Model = model;
            Types = Model.Types.Where(t => t.Assembly.ShortName == "Assembly-CSharp.dll").ToList();
            Fields = Types.SelectMany(t => t.DeclaredFields).ToList();
        }
        public void StartTranslating()
        {
            PluginServices services = PluginServices.For(Plugin);

            foreach (var translator in LocateTranslators(Plugin.GetType().Assembly))
            {
                services.StatusUpdate($"Translating {translator.Name}");
                Il2CppInspector.Reflection.TypeInfo type = TypeTranslator.GetMatchingType(translator, this);
                if (type == null) continue;
                TypeTranslator.TranslateRecursively(translator, type, this);
            }
        }

        private IEnumerable<Type> LocateTranslators(Assembly assembly)
        {
            var translators = assembly.GetTypes().Where(type => Attribute.IsDefined(type, typeof(TranslatorAttribute)));
            return translators;
        }
    }
}