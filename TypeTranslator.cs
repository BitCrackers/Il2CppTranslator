using Il2CppInspector.Reflection;
using System.Collections.Generic;

namespace Il2CppTranslator
{
    public class TypeTranslator
    {
        private List<FieldTranslator> _fields;
        private TypeInfo _type;
        private string ObfName;
        private string ObfNameSpace;

        public TypeTranslator(TypeInfo type, Translator translator)
        {
            translator.AddTranslator(this);
            _type = type;
            ObfName = _type.Name;
            ObfNameSpace = _type.Namespace;

            _fields = new List<FieldTranslator>();
        }

        public void AddFieldTranslator(FieldTranslator fieldTranslator)
        {
            _fields.Add(fieldTranslator);
        }

        public void TranslateFields()
        {
            if (_type == null) return;
            foreach (FieldTranslator translator in _fields)
                translator.Translate(_type);
        }

        public void SetName(string type_name)
        {
            if (_type == null) return;
            _type.Name = type_name;
        }

        public void SetNamespace(string type_namespace)
        {
            if (_type == null) return;
            _type.Namespace = type_namespace;
        }

        internal List<FieldTranslator> GetFields() => _fields;
        internal KeyValuePair<string, string> GetNameTranslation() => new KeyValuePair<string, string>(_type.Name, ObfName);
        internal KeyValuePair<string, string> GetNameSpaceTranslation() => new KeyValuePair<string, string>(_type.Namespace, ObfNameSpace);

    }
}