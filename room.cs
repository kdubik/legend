namespace legend
{
    public enum EPath { NORTH, SOUTH, WEST, EAST, UP, DOWN };
    public class Room
    {
        public string id, name;
        public string desc;
        public string[] paths = new string[6]; // Cesty v ramci
        public bool teleport = false;
        public int danger;  // Probability of encounter with enemies

        public Container container = null;

        public Room()
        {
            for (int i = 0; i < 6; i++)
            {
                paths[i] = "np";
            }
        }

        public string GetRoadTarget(EPath path) => paths[(int)path];
        public void SetRoadTarget(EPath path, string targetRoomId) => paths[(int)path] = targetRoomId;
    }
}