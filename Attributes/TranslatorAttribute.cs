using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

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
