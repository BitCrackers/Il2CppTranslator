using Il2CppInspector.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Il2CppTranslator.Util
{
    public static class Extensions
    {
        private class DummyEnumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerator<T> enumerator;

            public DummyEnumerable(IEnumerator<T> enumerator) => this.enumerator = enumerator;

            public IEnumerator<T> GetEnumerator() => enumerator;

            IEnumerator IEnumerable.GetEnumerator() => enumerator;
        }

        public static IEnumerable<TItem> TopologicalSort<TItem, TKey>(this IEnumerable<TItem> source, Func<TItem, TKey> getKey, Func<TItem, IEnumerable<TKey>> getDependencies)
        {
            return new DummyEnumerable<TItem>(new TopologicalSorting<TItem, TKey>(source, getKey, getDependencies));
        }

        public static IList<FieldInfo> GetFields(this TypeInfo typeInfo)
        {
            return typeInfo.DeclaredFields.Where(f => !f.IsLiteral && !f.IsStatic).ToList();
        }

        public static bool FieldSequenceEqual(this TypeInfo typeInfo, List<string> fieldSequence)
        {
            var typeNames = typeInfo.GetFields().Select(f => new { f.FieldType.CSharpName, f.FieldType.CSharpBaseName }).ToList();
            if (fieldSequence.Count != typeNames.Count) return false;

            for (int i = 0; i < fieldSequence.Count; i++)
            {
                if (fieldSequence[i] == "*") continue;
                if (typeNames[i].CSharpName != fieldSequence[i] && typeNames[i].CSharpBaseName != fieldSequence[i]) return false;
            }

            return true;
        }

        public static IList<FieldInfo> GetStaticFields(this TypeInfo typeInfo)
        {
            return typeInfo.DeclaredFields.Where(f => !f.IsLiteral && f.IsStatic).ToList();
        }

        public static IList<MethodInfo> GetMethods(this TypeInfo typeInfo)
        {
            return typeInfo.DeclaredMethods.Except(typeInfo.GetPropertyMethods()).ToList();
        }

        public static IList<MethodInfo> GetPropertyMethods(this TypeInfo typeInfo)
        {
            return typeInfo.DeclaredProperties.SelectMany(p => new[] { p.GetMethod, p.SetMethod }).Where(m => m != null).ToList();
        }

        public static IList<TypeInfo> GetChildren(this TypeInfo typeInfo)
        {
            Helpers.CheckInit();
            return Helpers.GetTypes().Where(t => t.BaseType != null && t.BaseType == typeInfo).ToList();
        }

        public static IList<TypeInfo> GetInstances(this TypeInfo typeInfo)
        {
            Helpers.CheckInit();
            return Helpers.GetTypes().Where(t => t.ImplementedInterfaces != null && t.ImplementedInterfaces.Contains(typeInfo)).ToList();
        }
    }
}