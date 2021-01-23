namespace legend
{
    public class Room
    {
        public string id, name;
        public string desc;
        public bool teleport = false;
        public bool water = false;  // Zatopena miestnost

        public string enemyGroup = "";  // No enemies here

        public Room(string id)
        {
           this.id = id;
           name = id;
        }
    }
}