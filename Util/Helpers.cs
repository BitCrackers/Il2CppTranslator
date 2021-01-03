using Il2CppInspector.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Il2CppTranslator.Util
{
    public class Helpers
    {
        private static bool init = false;
        private static List<TypeInfo> _types;
        private static List<FieldInfo> _fields;
        private static List<MethodInfo> _methods;
        public static void Initialize(TypeModel model, string assemblyName = "Assembly-CSharp.dll")
        {
            _types = model.Types.Where(t => t.Assembly.ShortName == assemblyName).ToList();
            _fields = _types.SelectMany(t => t.DeclaredFields).ToList();
            _methods = _types.SelectMany(t => t.GetAllMethods()).ToList();
            init = true;
        }
        public static TypeInfo GetType(string name)
        {
            CheckInit(); return _types.Where(t => t.Name.Equals(name)).FirstOrDefault();
        }
        public static FieldInfo GetField(string name)
        {
            CheckInit(); return _fields.Where(f => f.Name.Equals(name)).FirstOrDefault();
        }
        public static MethodInfo GetMethod(string name)
        {
            CheckInit(); return _methods.Where(m => m.Name.Equals(name)).FirstOrDefault();
        }
        public static IList<TypeInfo> GetTypeWithNumStaticFields(int count)
        {
            return GetTypes().Where(t => t.GetStaticFields().Count == count).ToList();
        }
        public static TypeInfo FindTypeWithSequence(IEnumerable<string> sequence)
        {
            CheckInit(); return _types.Where(t => t.FieldSequenceEqual(sequence)).FirstOrDefault();
        }
        internal static IList<TypeInfo> GetTypes()
        {
            CheckInit(); return _types;
        }
        internal static void CheckInit()
        {
            if (!init) throw new Exception("Call Initialized before calling any helper functions");
        }
    }
}
