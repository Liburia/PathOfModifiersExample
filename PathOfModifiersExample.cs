using PathOfModifiers;
using Terraria.ModLoader;

namespace PathOfModifiersExample
{
	class PathOfModifiersExample : Mod
	{
		public PathOfModifiersExample()
		{
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
        }
        public override void Load()
        {
            PoMAffixController.RegisterMod(this);
        }
    }
}
