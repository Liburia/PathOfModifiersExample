# PathOfModifiersExample
An example of extending PoM affixes.

To add a custom affix to PoM all you need to do is inherit from `PathOfModifiers.Affixes.Prefix` or `Suffix` and call `PoMAffixController.RegisterMod(this);` from Mod.Load().
