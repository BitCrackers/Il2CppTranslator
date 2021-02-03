using Il2CppInspector.Reflection;
using Il2CppTranslator.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Il2CppTranslator
{
    public static class TypeTranslator
    {
        public static TypeInfo GetMatchingType(Type type, Translator translator)
        {
            if (translator.Translated.Contains(type))
            {
                return null;
            }
            List<string> sequence = type.GetFieldSequence();

            List<TypeInfo> types = Helpers.FindTypesWithFieldSequence(type.GetFieldSequence(), translator)
                .Where(t => t.DeclaredFields.Count(f => !f.IsStatic && !f.IsLiteral) == type.GetDeclaredFields().Count(f => !f.IsStatic && !f.IsLiteral))
                .ToList();

            if (types.Count == 0)
            {
                return null;
            }
            if (types.Count == 1)
            {
                return types[0];
            }

            List<TypeInfo> fieldNameMatches = types.Where(t => type.GetDeclaredFields().Any(f => t.DeclaredFields.Any(f2 => f2.Name.Equals(f.Name)))).ToList();
            if (fieldNameMatches.Count == 1)
            {
                return fieldNameMatches[0];
            }

            List<TypeInfo> staticFieldCountMatches = types.Where(t => type.GetDeclaredFields().Count(f => f.IsStatic) == t.DeclaredFields.Count(f => f.IsStatic)).ToList();
            if (staticFieldCountMatches.Count == 1)
            {
                return staticFieldCountMatches[0];
            }

            return null;
        }

        public static void TranslateRecursively(Type clean, TypeInfo obf, Translator translator)
        {
            if (translator.Translated.Contains(clean))
            {
                return;
            }
            if (clean.Name != obf.Name)
            {
                translator.Translations.Add((obf.Name, clean.Name));
                obf.Name = clean.Name;
            }

            List<System.Reflection.FieldInfo> fields = clean.GetDeclaredFields();
            foreach (var field in fields)
            {
                FieldInfo match = FindMatchingField(field, obf.DeclaredFields);
                if (match == null)
                {
                    continue;
                }
                if (match.Name != field.Name)
                {
                    translator.Translations.Add((match.Name, field.Name));
                    match.Name = field.Name;
                }
                if (Attribute.IsDefined(field.FieldType, typeof(TranslatorAttribute)) && field.FieldType != clean)
                {
                    TranslateRecursively(field.FieldType, match.FieldType, translator);
                }
            }
            translator.Translated.Add(clean);
        }

        internal static FieldInfo FindMatchingField(System.Reflection.FieldInfo field, IEnumerable<FieldInfo> fields)
        {
            foreach (FieldInfo f in fields)
            {
                if (f.IsStatic != field.IsStatic || f.IsPrivate != field.IsPrivate || f.IsPublic != field.IsPublic)
                {
                    continue;
                }
                if (f.Name == field.Name)
                {
                    return f;
                }
                if (f.Offset == GetFieldOffset(field))
                {
                    return f;
                }
            }
            return null;
        }

        internal static int GetFieldOffset(System.Reflection.FieldInfo field)
        {
            if (!Attribute.IsDefined(field, typeof(TranslatorFieldOffsetAttribute)))
            {
                return -1;
            }
            else
            {
                return ((TranslatorFieldOffsetAttribute)Attribute.GetCustomAttribute(field, typeof(TranslatorFieldOffsetAttribute))).Offset;
            }
        }
    }
}