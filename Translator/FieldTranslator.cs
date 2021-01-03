using Il2CppInspector.Reflection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Il2CppTranslator.Translator
{
    public class FieldTypes
    {
        public static string SystemByte { get => "byte"; }
        public static string SystemSByte { get => "sbyte"; }
        public static string SystemInt16 { get => "short"; }
        public static string SystemUInt16 { get => "ushort"; }
        public static string SystemInt32 { get => "int"; }
        public static string SystemUInt32 { get => "uint"; }

    }
    public class FieldTranslator
    {
        long Offset { get; }
        string Type { get; }
        bool? Static { get; }
        bool? Literal { get; }
        string Name { get; }
        bool TranslateType { get; }

        private int GenericIndex { get; }

        public FieldTranslator(long offset, string type_name, bool? static_field, bool? literal, string field_name, int generic_index, bool translate_type)
        {
            Offset = offset;
            Type = type_name;
            Static = static_field;
            Literal = literal;
            Name = field_name;
            GenericIndex = generic_index;
            TranslateType = translate_type;
        }
        public void Translate(TypeInfo type)
        {
            if (GenericIndex != -1 && TranslateType) TranslateGenericFieldType(type);
            else if(TranslateType) TranslateFieldType(type);
            else if(!TranslateType) TranslateFieldName(type);

        }
        private void TranslateFieldType(TypeInfo type)
        {
            FieldInfo field = GetField(type);
            if (field == null) return;
            if (field.FieldType.IsArray)
                field.FieldType.ElementType.Name = Name;
            else
                field.FieldType.Name = Name;
        }
        private void TranslateFieldName(TypeInfo type)
        {
            FieldInfo field = GetField(type);
            if (field == null) return;
            field.Name = Name;
        }
        private void TranslateGenericFieldType(TypeInfo type)
        {
            FieldInfo field = GetField(type);
            if (field == null) return;
            field.FieldType.GetGenericArguments()[0].Name = Type;
        }

        private FieldInfo GetField(TypeInfo type)
        {
            foreach (var fieldInfo in type.DeclaredFields)
            {
                bool offsetMatch = (Offset == -1 || Offset == fieldInfo.Offset);
                bool typeMatch = (string.IsNullOrEmpty(Type) || fieldInfo.FieldType.CSharpName == Type);
                bool staticMatch = (!Static.HasValue || fieldInfo.IsStatic == Static.Value);
                bool literalMatch = (!Literal.HasValue || fieldInfo.IsLiteral == Literal.Value);
                bool nameMatch = (string.IsNullOrEmpty(Name) || fieldInfo.CSharpName == Name);
                if (offsetMatch && staticMatch && literalMatch && (typeMatch || nameMatch))
                {
                    return fieldInfo;
                }
            }
            return null;
        }
    }
}
