using System.Collections.Generic;

namespace Il2CppTranslator
{
    public interface ITranslator
    {
        public string Name { get; }
        public List<System.Type> Dependencies { get; }

        public void Initialize();

        void TranslateFields() { }

        void TranslateMethods() { }

        void TranslateDerivedTypes() { }
    }
}