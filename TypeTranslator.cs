using Il2CppInspector.Reflection;
using Il2CppTranslator.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Il2CppTranslator
{
    public static class TypeTranslator
    {
        public static TypeInfo GetMatchingType(Type type, Translator translator)
        {
            if(translator.Translated.Contains(type)) 
            {
                return null;
            }
            List<string> sequence = type.GetFieldSequence();

            List<TypeInfo> types = Helpers.FindTypesWithFieldSequence(type.GetFieldSequence(), translator).ToList();
            if (types.Count == 0) return null;
            if (types.Count == 1) return types[0];

            List<TypeInfo> fieldNameMatches = types.Where(t => type.GetFields().Any(f => t.DeclaredFields.Any(f2 => f2.Name.Equals(f.Name)))).ToList();
            if (fieldNameMatches.Count == 1) return fieldNameMatches[0];

            List<TypeInfo> staticFieldCountMatches = types.Where(t => type.GetFields().Count(f => f.IsStatic) == t.DeclaredFields.Count(f => f.IsStatic)).ToList();
            if (staticFieldCountMatches.Count == 1) return staticFieldCountMatches[0];

            return null;
        }

        public static void TranslateRecursively(Type clean, TypeInfo obf, Translator translator)
        {
            translator.Translations.Add((obf.Name, clean.Name));
            obf.Name = clean.Name;
            translator.Translated.Add(clean);

            List<FieldInfo> obfGenericFields = obf.DeclaredFields.Where(f => !f.IsStatic && !f.IsLiteral).ToList();
            List<System.Reflection.FieldInfo> cleanGenericFields = clean.GetFields().Where(f => !f.IsStatic && !f.IsLiteral).ToList();
            foreach (var obField in obfGenericFields.Select((Value, Index) => new { Value, Index }))
            {
                if (cleanGenericFields.Count() == obField.Index) break;
                //System.Reflection.FieldInfo cleanField = cleanGenericFields.FirstOrDefault(f => f.FieldType.Name == obField.Value.CSharpName);
                
                System.Reflection.FieldInfo cleanField = cleanGenericFields.FirstOrDefault(f => ((TranslatorFieldOffsetAttribute)Attribute.GetCustomAttribute(f, typeof(TranslatorFieldOffsetAttribute))).Offset == obField.Value.Offset && f.IsStatic == obField.Value.IsStatic);
                if(cleanField == null)
                {
                    cleanField = cleanGenericFields[obField.Index];
                }

                int off = ((TranslatorFieldOffsetAttribute)Attribute.GetCustomAttribute(cleanField, typeof(TranslatorFieldOffsetAttribute))).Offset;
                if (obField.Value.Offset != off) continue;

                if (obField.Value.Name != cleanField.Name)
                {
                    translator.Translations.Add((obField.Value.Name, cleanField.Name));
                    obField.Value.Name = cleanField.Name;
                }
                if(Attribute.IsDefined(cleanField.FieldType, typeof(TranslatorAttribute)) && !translator.Translated.Contains(cleanField.FieldType))
                {
                    TranslateRecursively(cleanField.FieldType, obField.Value.FieldType, translator);
                }
            }

            List<FieldInfo> obfStaticFields = obf.DeclaredFields.Where(f => f.IsStatic).ToList();
            List<System.Reflection.FieldInfo> cleanStaticFields = clean.GetFields().Where(f => f.IsStatic).ToList();
            foreach (var obField in obfStaticFields.Select((Value, Index) => new { Value, Index }))
            {
                if (cleanStaticFields.Count() == obField.Index) break;

                System.Reflection.FieldInfo cleanField = cleanStaticFields[obField.Index];
                if (!Attribute.IsDefined(cleanField, typeof(TranslatorFieldOffsetAttribute)))
                {
                    continue;
                }

                if (obField.Value.Offset != ((TranslatorFieldOffsetAttribute)Attribute.GetCustomAttribute(cleanField, typeof(TranslatorFieldOffsetAttribute))).Offset) continue;

                if (obField.Value.Name != cleanField.Name)
                {
                    translator.Translations.Add((obField.Value.Name, cleanField.Name));
                    obField.Value.Name = cleanField.Name;
                }

                if(cleanField.FieldType.ContainsGenericParameters 
                    && obField.Value.FieldType.ContainsGenericParameters 
                    && obField.Value.FieldType.GetGenericArguments().Count() == cleanField.FieldType.GetGenericArguments().Count())
                {
                    int i = 0;
                    foreach(Type t in cleanField.FieldType.GetGenericArguments())
                    {
                        if (Attribute.IsDefined(t, typeof(TranslatorAttribute)) && !translator.Translated.Contains(t))
                        {
                            TranslateRecursively(t, obField.Value.FieldType.GetGenericArguments()[i], translator);
                        }
                        i++;
                    }
                }

                if (Attribute.IsDefined(cleanField.FieldType, typeof(TranslatorAttribute)) && !translator.Translated.Contains(cleanField.FieldType))
                {
                    TranslateRecursively(cleanField.FieldType, obField.Value.FieldType, translator);
                }
            }
        }
    }
}