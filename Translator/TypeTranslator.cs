using Il2CppInspector.Reflection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Il2CppTranslator.Translator
{
    public class TypeTranslator
    {
        List<FieldTranslator> _fields;
        TypeInfo _type;

        public TypeTranslator(TypeInfo type)
        {
            _type = type;
            _fields = new List<FieldTranslator>();
        }

        private void AddField(long field_offset = -1, string type_name = "", bool? static_field = null, bool? literal = null, string field_name = "", int generic_index = -1, bool translate_type = false)
        {
            _fields.Add(new FieldTranslator(field_offset, type_name, static_field, literal, field_name, generic_index, translate_type));
        }
        public void AddField(long field_offset, string field_name, bool static_field = false, bool literal_field = false) { AddField(field_offset: field_offset, static_field: static_field, literal: literal_field, field_name: field_name, translate_type: false); }
        public void AddField(long field_offset, string field_type, bool static_field = false, bool literal_field = false, bool translate_type = true) { AddField(field_offset: field_offset, type_name: field_type, static_field: static_field, literal: literal_field, translate_type: translate_type); }
        public void AddField(string field_type, bool static_field = false, bool literal_field = false, bool translate_type = true) { AddField(type_name: field_type, static_field: static_field, literal: literal_field, translate_type: translate_type); }
        public void AddField(string field_name, string type_name, bool translate_type = true) { AddField(type_name: type_name, field_name: field_name, translate_type: translate_type); }
        public void AddField(string field_name, int generic_index, string type_name, bool translate_type = true) { AddField(type_name: type_name, field_name: field_name, generic_index: generic_index, translate_type: translate_type); }
        public void AddField(long field_offset, int generic_index, string type_name, bool translate_type = false) { AddField(type_name: type_name, field_offset: field_offset, generic_index: generic_index, translate_type: translate_type); }

        public void TranslateFields()
        {
            if (_type == null) return;
            foreach (FieldTranslator translator in _fields)
                translator.Translate(_type);
        }
        public void SetName(string name)
        {
            if (_type == null) return;
            _type.Name = name;
        }
    }
}
