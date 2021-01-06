using Il2CppInspector.Reflection;

namespace Il2CppTranslator
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
        public bool TranslateName { get; set; } = false;
        public bool TranslateType { get; set; } = false;
        public bool? Literal { get; set; } = null;
        public bool? Static { get; set; } = null;
        public int GenericIndex { get; set; } = 0;
        public long? Offset { get; set; } = null;
        public string Name { get; set; } = "";
        public string TypeName { get; set; } = "";
        public string TypeNamespace { get; set; } = "";

        public void Translate(TypeInfo type)
        {
            FieldInfo field = GetField(type);
            if (field == null) return;

            if (TranslateType)
            {
                if (field.FieldType.IsGenericType)
                    TranslateFieldType(field.FieldType.GetGenericArguments()[GenericIndex]);
                if (field.FieldType.IsArray)
                    TranslateFieldType(field.FieldType.ElementType);
                else
                    TranslateFieldType(field.FieldType);
            }

            if (TranslateName)
            {
                field.Name = Name;
            }
        }

        private void TranslateFieldType(TypeInfo type)
        {
            if (!string.IsNullOrEmpty(TypeNamespace))
                type.Namespace = TypeNamespace;

            type.Name = TypeName;
        }

        private FieldInfo GetField(TypeInfo type)
        {
            foreach (var fieldInfo in type.DeclaredFields)
            {
                bool staticMatch = !Static.HasValue || fieldInfo.IsStatic == Static.Value;
                bool literalMatch = !Literal.HasValue || fieldInfo.IsLiteral == Literal.Value;
                bool offsetMatch = !Offset.HasValue || fieldInfo.Offset == Offset.Value;
                bool typeMatch = (!TranslateType && !string.IsNullOrEmpty(TypeName)) && (fieldInfo.FieldType.CSharpName == TypeName || fieldInfo.FieldType.CSharpBaseName == TypeName);
                bool nameMatch = (!TranslateName && !string.IsNullOrEmpty(Name)) && (fieldInfo.Name == Name);
                if ((offsetMatch || typeMatch || nameMatch) && (staticMatch && literalMatch))
                    return fieldInfo;
            }
            return null;
        }
    }
}