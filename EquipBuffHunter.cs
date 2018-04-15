using System;
using Terraria;
using PathOfModifiers;
using PathOfModifiers.Affixes;
using Terraria.ID;
using Terraria.ModLoader.IO;
using System.IO;

namespace PathOfModifiersExample
{
    public class EquipBuffHunter : Suffix
    {
        public override float weight => 9990.5f;

        public override string addedText => "of the Hunter";
        public override float addedTextWeight => 0.5f;

        //Chance to get the hunter buff every tick.
        public float chance;

        public override bool CanBeRolled(PoMItem pomItem, Item item)
        {
            return
                PoMItem.IsAnyArmor(item) ||
                PoMItem.IsAccessory(item);
        }

        public override string GetTolltipText(Item item)
        {
            return $"Player can sometimes see enemies, {chance * 100}%";
        }

        public override void UpdateEquip(Item item, PoMPlayer player)
        {
            if (Main.rand.NextFloat(0, 1) < chance)
                player.player.AddBuff(BuffID.Hunter, 30, false);
        }

        public override void ReforgePrice(Item item, ref int price)
        {
            price = (int)Math.Round(price * 0.4f * weight * chance);
        }
        
        public override void RollValue(bool rollTier = true)
        {
            chance = Main.rand.NextFloat(0, 0.03f);
        }
        
        //Save custom fields
        public override void Save(TagCompound tag, Item item)
        {
            tag.Set("chance", chance);
        }
        //Load custom fields
        public override void Load(TagCompound tag, Item item)
        {
            chance = tag.GetFloat("chance");
        }
        //Send custom fields in MP
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(chance);
        }
        //Receive custom fields in MP
        public override void NetReceive(Item item, BinaryReader reader)
        {
            chance = reader.ReadSingle();
        }
        //Clone the affix
        public override Affix Clone()
        {
            EquipBuffHunter affix = (EquipBuffHunter)base.Clone();
            affix.chance = chance;
            return affix;
        }
    }
}
