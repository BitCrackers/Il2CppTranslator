using Il2CppInspector.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Il2CppTranslator.Util
{
    public class Helpers
    {
        public static TypeInfo GetType(string name, Translator translator)
        {
            return translator.Types.Where(t => t.FullName.Equals(name)).FirstOrDefault();
        }

        public static FieldInfo GetField(string name, Translator translator)
        {
            return translator.Fields.Where(f => f.Name.Equals(name)).FirstOrDefault();
        }

        public static IList<TypeInfo> GetTypeWithNumStaticFields(int count, Translator translator)
        {
            return translator.Types.Where(t => t.GetStaticFields().Count == count).ToList();
        }

        public static IEnumerable<TypeInfo> FindTypesWithFieldSequence(List<string> sequence, Translator translator)
        {
            return translator.Types.Where(t => t.FieldSequenceEqual(sequence));
        }

        public static TypeInfo FindTypeWithStaticFieldSequence(List<string> sequence, Translator translator)
        {
            return translator.Types.Where(t => t.StaticFieldSequenceEqual(sequence)).FirstOrDefault();
        }
    }
}