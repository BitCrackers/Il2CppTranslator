﻿using System;

namespace Il2CppTranslator
{
    [AttributeUsage(AttributeTargets.Field)]
    public class TranslatorFieldOffsetAttribute : Attribute
    {
        public int Offset;
        public TranslatorFieldOffsetAttribute(int offset)
        {
            Offset = offset;
        }
    }
}
