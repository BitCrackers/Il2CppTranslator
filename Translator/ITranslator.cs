using Il2CppInspector.Reflection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Il2CppTranslator.Translator
{
    public partial interface ITranslator
    {
        public TypeTranslator Translator { get; }
        public void Translate();
        public TypeTranslator Locate();
    }
}
    