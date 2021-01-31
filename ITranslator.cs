using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Il2CppTranslator
{
    public interface ITranslator
    {
        public string Name { get; }
        public TypeTranslator Type { get; }
        public List<System.Type> Dependencies { get; }

        Translator Parent { get; }
        public void Initialize(Translator parent);

        void TranslateFields() { }

        void TranslateMethods() { }

        void TranslateDerivedTypes() { }
    }
}