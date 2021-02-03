using System;

namespace Il2CppTranslator
{
    [AttributeUsage(AttributeTargets.Struct)]
    public class TranslatorAttribute : Attribute
    {
        public TranslatorAttribute()
        {
        }
    }
}
