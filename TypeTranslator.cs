using Il2CppInspector.Reflection;
using System.Collections.Generic;

namespace Il2CppTranslator
{
    public class TypeTranslator
    {
        private List<FieldTranslator> _fields;
        private TypeInfo _type;

        public TypeTranslator(TypeInfo type)
        {
            _type = type;
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
    }
}