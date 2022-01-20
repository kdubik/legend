using System.Collections.Generic;

namespace LegendLibrary
{
    public enum ItemType { MISC, ASSET, WHEAPON, ARMOR, SHIELD, BOOK, TOOL };
    //public enum BodyPart { HEAD, ARMOR, BOOTS, CLOTHES_UP, CLOTHES_BOTTOM };
    public enum Rarity { COMMON, UNCOMMON, RARE }

    public class Item
    {
        public string id, name;
        public string say ="";
        public int value, weight;
        public Rarity rarity = Rarity.COMMON;
        public ItemType type = ItemType.MISC;
        public string param="";    // parametre predmetu / ak je to mec, tak je tu sila atd...
        public Dictionary<string,string> attributes = new Dictionary<string,string>();

        public Item(string id)
        {
            this.id=id;
        }

        public void AppendAttributes(string attributes)
        {
            string tmp = param;
            if (tmp!="") tmp = tmp + ";";
            tmp = tmp + attributes;
            param = tmp;
        }

        /// <summary>
        /// Converts information stored in attributes string
        /// into the dictionary
        /// </summary>
        public void ConverParamToDict()
        {
            string[] words = param.Split(";");

            if (words.Length>0)
            {
                foreach (string ln in words)
                {
                    if (ln!="")
                    {
                        string[] data = ln.Split(":");
                        attributes.Add(data[0],data[1]);
                    }
                }
            }
        }

        /// <summary>
        /// Try to get requested parameter from dictionary.
        /// </summary>
        /// <param name="attrName">Name of requested attribute (parameter)</param>
        /// <returns>Value of requested parameter as string.</returns>
        public string GetAttribute(string attrName)
        { 
            string ret = "";
            attributes.TryGetValue(attrName, out ret);
            return ret;
        }

        public int GetArmorBonus()
        {
            string armorType = GetAttribute("armor_type");
            int bonus = 0;
            switch (armorType)
            {
                case "light_armor" :
                    bonus = 1; break;
                case "medium_armor" :
                    bonus = 2; break;
                case "heavy_armor" :
                    bonus = 3; break;
            }
            return bonus;
        }


        public int GetArmorPenalty()
        {
            string armorType = GetAttribute("armor_type");
            int bonus = 0;
            switch (armorType)
            {
                case "medium_armor" :
                    bonus = -2; break;
                case "heavy_armor" :
                    bonus = -4; break;
            }
            return bonus;
        }

        public ItemType GetItemTypeFromString(string itemTypeString)
        {
            ItemType type = ItemType.WHEAPON;   // Default

            // if (words[0]=="wheapon") type = ItemType.WHEAPON;
            if (itemTypeString=="armor") type = ItemType.ARMOR;
            if (itemTypeString=="shield") type = ItemType.SHIELD;
            if (itemTypeString=="misc") type = ItemType.MISC;
            if (itemTypeString=="tool") type = ItemType.TOOL;
            if (itemTypeString=="asset") type = ItemType.ASSET;

            return type;
        }
        /*
        public Item(string dataString)
        {
            string[] words = dataString.Split(";");

            // 0 - Obect type
            if (words[0]=="wheapon") type = ItemType.WHEAPON;
            if (words[0]=="armor") type = ItemType.ARMOR;
            if (words[0]=="shield") type = ItemType.SHIELD;
            if (words[0]=="misc") type = ItemType.MISC;
            if (words[0]=="tool") type = ItemType.TOOL;
            if (words[0]=="asset") type = ItemType.ASSET;

            // 1 - Object ID
            id = words[1];

            // 2 - Object name
            name = words[2];

            if (type!=ItemType.ASSET)
            {
                // 3 - Raritness
                if (words[3]=="common") rarity = Rarity.COMMON;
                if (words[3]=="uncommon") rarity = Rarity.UNCOMMON;
                if (words[3]=="rare") rarity = Rarity.RARE;

                // 4 - Weight
                weight = int.Parse(words[4]);

                // 5 - Price
                value = int.Parse(words[5]);
            }

            // Rest
            param = dataString;
        }
        */
    }
}