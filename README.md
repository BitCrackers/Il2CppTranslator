# Il2CppTranslator [![](https://img.shields.io/github/v/release/OsOmE1/Il2CppTranslator)](https://github.com/OsOmE1/Il2CppTranslator/releases/latest)
A C# Library to help make deobfuscation plugins for [Il2CppInspector](https://github.com/djkaty/Il2CppInspector)

# Examples
A translator class must always have the translator attribute and implement the ITranslator interface.
More information on how to structure your plugin can be found here https://github.com/OsOmE1/Il2CppTranslator/wiki/Structuring-your-plugin  
```CS
[Translator("YourClass")]
class YourClass : ITranslator
{
    public TypeTranslator Translator => Locate();

    public TypeTranslator Locate()
    {
        return new TypeTranslator(Helpers.GetField("YouField").DeclaringType);
    }

    public void Translate()
    {
        Translator.AddField(field_offset: 0x0, static_field: false, field_name: "YourField");
        Translator.TranslateFields();
    }
}
```

# Beebyte-Deobfuscator
You can use the [Beebyte-Deobfuscator](https://github.com/OsOmE1/Beebyte-Deobfusctator) to generate classes like the example given above.
Go to the plugin settings and select **"Il2CppTranslator Classes"** from the export dropdown menu and select your export path.
![](https://i.imgur.com/OdxxC4Z.png)

# Installation
To use Il2CppTranslator you can install the latest nuget package from https://github.com/OsOmE1/Il2CppTranslator/packages/

# Build Instructions
Clone [Il2CppInspector](https://github.com/djkaty/Il2CppInspector) and [Il2CppTranslator](https://github.com/OsOmE1/Il2CppTranslator) into the same directory.  
`$ git clone --recursive https://github.com/djkaty/Il2CppInspector.git`  
`$ git clone https://github.com/OsOmE1/Il2CppTranslator.git`  
Then build Il2CppInspector in debug mode and then build Il2CppTranslator

# Documentation
The documentation can be found here https://github.com/OsOmE1/Il2CppTranslator/wiki
