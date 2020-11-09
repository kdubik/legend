using System.Collections.Generic;
 
namespace legend
{
    public enum ContainerType { FURNITURE, CHEST, DEAD_BODY, HIDEWAY };
    public enum ContainerSize { SMALL, MEDIUM, LARGE };
    public class Container
    {
        public string id;
        public string name;
        //public string position = ""; // Room ID

        public ContainerType type = ContainerType.FURNITURE;// Typ kontajnera
        public ContainerSize size = ContainerSize.MEDIUM;   // KOlko objektov sa sem vojde?

        public bool locked = false;
        public int key = 0;             // If locked, which kee is working?
        public bool trapped = false;    // Trap on this container?
        public int trapLevel = 12;      // Level for disarming trap

        public bool spawner = false;    // Is here generated content?
        public bool used = false;       // If yes, was already generated?

        public bool hidden = false;     // Je tento kontajner skryty?
        public int discoveryLevel = 9;  // Sanca na objav
        //public List<Citem>

        public Container(string name, ContainerType type, ContainerSize size)
        {
            this.name = name;
            this.type = type;
            this.size = size;
            //this.position = position;
        }
    }
}