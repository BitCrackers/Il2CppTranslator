# Il2CppTranslator [![](https://img.shields.io/github/v/release/BitCrackers/Il2CppTranslator)](https://github.com/BitCrackers/Il2CppTranslator/releases/latest) [![Discord](https://img.shields.io/badge/Discord-Invite-7289DA.svg?logo=Discord&style=flat-square)](https://discord.gg/AUpXd3VUh8) [![Paypal](https://img.shields.io/badge/PayPal-Donate-Green.svg?logo=Paypal&style=flat-square)](https://www.paypal.com/donate/?hosted_button_id=TYMU92FD9D9UW)
A C# Library to help make deobfuscation plugins for [Il2CppInspector](https://github.com/djkaty/Il2CppInspector)

# Examples
A translator struct must always have the translator attribute.
More information on how to structure your plugin can be found here https://github.com/OsOmE1/Il2CppTranslator/wiki/Structuring-your-plugin  
```CS
[Translator]
public struct AmongUsClient
{
    [TranslatorFieldOffset(0x0)] static public AmongUsClient Instance;
    [TranslatorFieldOffset(0x70)] public int AutoOpenStore;
    [TranslatorFieldOffset(0x74)] public object GameMode;
    [TranslatorFieldOffset(0x78)] public object OnlineScene;
    [TranslatorFieldOffset(0x7C)] public object MainMenuScene;
    [TranslatorFieldOffset(0x80)] public GameData GameDataPrefab;
    [TranslatorFieldOffset(0x84)] public PlayerControl PlayerPrefab;
    [TranslatorFieldOffset(0x88)] public List<ShipStatus> ShipPrefabs;
    [TranslatorFieldOffset(0x8C)] public int TutorialMapId;
    [TranslatorFieldOffset(0x90)] public float SpawnRadius;
    [TranslatorFieldOffset(0x94)] public object discoverState;
    [TranslatorFieldOffset(0x98)] public List<object> DisconnectHandlers;
    [TranslatorFieldOffset(0x9C)] public List<object> GameListHandlers;
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
