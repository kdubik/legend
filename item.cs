using System.Collections.Generic;

namespace legend
{
    public enum ItemType { MISC, ASSET, WHEAPON, ARMOR, BOOK, TOOL };
    //public enum BodyPart { HEAD, ARMOR, BOOTS, CLOTHES_UP, CLOTHES_BOTTOM };
    public enum Rarity { COMMON, UNCOMMON, RARE }

    public class Item
    {
        public string id, name;
        public int value, weight;
        public Rarity rarity = Rarity.COMMON;
        public ItemType type = ItemType.MISC;

        public string param;    // parametre predmetu / ak je to mec, tak je tu sila atd...

        public Item(string id, ItemType type)
        {
            this.id = id;
            name = id;
            this.type = type;
        }

        public Item(string dataString)
        {
            string[] words = dataString.Split(";");

            // 0 - Obect type
            if (words[0]=="wheapon") type = ItemType.WHEAPON;
            if (words[0]=="armor") type = ItemType.ARMOR;
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
    }
}