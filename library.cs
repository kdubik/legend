using System;
using System.Collections.Generic;

namespace legend
{
    public class Library
    {
        public List<Room> rooms = new List<Room>();
        public List<Item> items = new List<Item>();

        public int LoadData()
        {

            // Rooms, map 1
            Room lRoom = new Room();
            lRoom.id = "1";
            lRoom.name = "Brana k domu";
            lRoom.desc = "Stojis pri hlavnej brane.\nJe to brana";
            lRoom.SetRoadTarget(EPath.NORTH,"2");
            lRoom.danger = 0;   // Zero chance of attack from enemies
            rooms.Add(lRoom);

            lRoom = new Room();
            lRoom.id = "2";
            lRoom.name = "Vstupna hala";
            lRoom.desc = "Si uprostred vstupnej haly. Chodba je dlha\na plna roznych gotickych soch.";
            lRoom.SetRoadTarget(EPath.SOUTH,"1");
            lRoom.danger = 20;   // 20% chance of attack from enemies
            
            lRoom.container = new Container("Velka skrina", ContainerType.FURNITURE, ContainerSize.MEDIUM);
            lRoom.container.spawner = true;  // Obsah je generovany podla velkosti, typu kontajnera a levelu hraca
            rooms.Add(lRoom);

            Item lItem = new Item("dyka",ItemType.WHEAPON);
            items.Add(lItem);

            lItem = new Item("mec",ItemType.WHEAPON);
            items.Add(lItem);

            return 0;
        }

        public Room GetRoom(string roomId)
        {
            Room ret = null;
            foreach (Room rm in rooms)
            {
                if (rm.id==roomId)
                {
                    ret = rm;
                    break;
                }
            }
            return ret;
        }
    
        public Item GetItem(string itemId)
        {
            Item ret = null;
            foreach (Item rm in items)
            {
                if (rm.id==itemId)
                {
                    ret = rm;
                    break;
                }
            }
            return ret;
        }
    }
}