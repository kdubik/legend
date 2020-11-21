using System.Collections.Generic;

namespace legend
{
    public enum ItemType { WHEAPON, ARMOR, MISC, BOOK };
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
    }
}