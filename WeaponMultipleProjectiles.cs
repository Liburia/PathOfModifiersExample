using Microsoft.Xna.Framework;
using System;
using Terraria;
using System.IO;
using Terraria.ModLoader.IO;
using PathOfModifiers;
using PathOfModifiers.Affixes;

//Don't actually use this affix. It only works on some weapons.
namespace PathOfModifiersExample
{
    //Extend Suffix to let PoM load the class. Implement interface for convenience.
    public class WeaponMultipleProjectiles : Suffix, ITieredStatIntValueAffix
    {
        //Weight of the affix in the roll pool. Higher values mean the affix rolls more often. Most of PoM affixes have weight 0.5f and less.
        public override float weight => 9990.5f;

        //The text that appears in the item name. In this case it's one of the tierNames[] below.
        public override string addedText => addedTextTiered;
        //Weight of the text when deciding which affix's text to use.
        public override float addedTextWeight => addedTextWeightTiered;
        
        //Tier implementation. It's not required for an affix to have tiers.
        static int[] tiers = new int[] { 1, 2, 3, 4, 5 };
        static Tuple<int, double>[] tierWeights = new Tuple<int, double>[] {
            new Tuple<int, double>(0, 3),
            new Tuple<int, double>(1, 2.5),
            new Tuple<int, double>(2, 2),
            new Tuple<int, double>(3, 1.5),
            new Tuple<int, double>(4, 1),
        };
        static string[] tierNames = new string[] {
            "of Multishot 1",
            "of Multishot 2",
            "of Multishot 3",
            "of Multishot 4",
            "of Multishot 5",
        };
        static int maxTier => tiers.Length - 1;

        int tierText => maxTier - tier + 1;

        int tier = 0;
        string addedTextTiered = string.Empty;
        float addedTextWeightTiered = 1;
        
        //Whether the affix can roll on the item or not. In this case it can be rolled on any weapon.
        public override bool CanBeRolled(PoMItem pomItem, Item item)
        {
            return
                PoMItem.IsWeapon(item);
        }

        //Text that is used in the tolltip.
        public override string GetTolltipText(Item item)
        {
            return $"[T{tierText}] {Tiers[Tier]} additional projectiles";
        }

        //Override Affix's Shoot method to spawn additional projectiles.
        public override bool PlayerShoot(Item affixItem, Player player, Item item, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 originalVelocity = new Vector2(speedX, speedY);
            float minAngle = originalVelocity.ToRotation() - 0.4f;
            float maxAngle = minAngle + 0.8f;
            Vector2 velocity = Main.rand.NextFloat(minAngle, maxAngle).ToRotationVector2() * originalVelocity.Length();
            int projCount = Tiers[Tier];
            for (int i = 0; i < projCount; i++)
            {
                Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockBack, player.whoAmI, 0f, 0f);
            }


            return true;
        }

        #region Interface Properties
        public float Weight => weight;
        public int[] Tiers => tiers;
        public Tuple<int, double>[] TierWeights => tierWeights;
        public string[] TierNames => tierNames;
        public int MaxTier => maxTier;
        public int TierText => tierText;
        public int Tier { get { return tier; } set { tier = value; } }
        public string AddedTextTiered { get { return AddedTextTiered; } set { addedTextTiered = value; } }
        public float AddedTextWeightTiered { get { return addedTextWeightTiered; } set { addedTextWeightTiered = value; } }
        #endregion
        #region Helped Methods
        void SetTier(int tier)
        {
            TieredAffixHelper.SetTier(this, tier);
        }
        public override Affix Clone()
        {
            return TieredAffixHelper.Clone(this, (ITieredStatIntValueAffix)base.Clone());
        }
        public override void RollValue(bool rollTier = true)
        {
            TieredAffixHelper.RollValue(this, rollTier);
        }
        public override void ReforgePrice(Item item, ref int price)
        {
            TieredAffixHelper.ReforgePrice(this, item, ref price);
        }
        public override void Save(TagCompound tag, Item item)
        {
            TieredAffixHelper.Save(this, tag, item);
        }
        public override void Load(TagCompound tag, Item item)
        {
            TieredAffixHelper.Load(this, tag, item);
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            TieredAffixHelper.NetSend(this, item, writer);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            TieredAffixHelper.NetReceive(this, item, reader);
        }
        #endregion
    }
}
