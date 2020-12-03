using System;

namespace legend
{
    public class GameItem
    {
        public string id;  // Referenced Item ID

        public ItemType itemType;
        public string itemName;
        public string position; // RoomID, player, none (invisible)

        public GameItem(string id, string position)
        {
            this.id = id;
            this.position = position;
        }
    }
}