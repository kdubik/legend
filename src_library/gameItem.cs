using System;

namespace LegendLibrary
{
    public enum ILevel { EASY, STANDARD, HARD, EXTRA_HARD };

    public class GameItem
    {
        public string id;  // Referenced Item ID

        public ItemType itemType;
        public string itemName, itemSay;
        public string position; // RoomID, player, none (invisible)

        public bool trapped;        // Is it trapped?
        public ILevel trapLevel;    // How complicated trap is here?
        public bool hidden;         // If in the room, is it hidden?
        public ILevel hiddenLevel;  // How good is item hidden?

        public GameItem(string id, string position)
        {
            this.id = id;
            this.position = position;
        }

        public GameItem(string gameItemID, string position, Item sourceItem)
        {
            this.id = gameItemID;
            this.position = position;
            itemType = sourceItem.type;
            itemName = sourceItem.name;
            itemSay = sourceItem.say;

        }

        public static ILevel GetILevel(string input)
        {
            ILevel rv = ILevel.EASY;
            if (input=="standard") rv = ILevel.STANDARD;
            if (input=="hard") rv = ILevel.STANDARD;
            if (input=="veryhard") rv = ILevel.STANDARD;

            return rv;
        }

        public void AddTrap(string level)
        {
            trapped = true;
            trapLevel = GetILevel(level);
        }

        public void AddHide(string level)
        {
            hidden = true;
            hiddenLevel = GetILevel(level);
        }
    }
}