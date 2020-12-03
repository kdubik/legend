namespace legend
{
    public enum Path { NORTH, SOUTH, WEST, EAST, UP, DOWN };
    public enum Direction { BOTH, TO_TARGET, TO_SOURCE };
    public enum DoorType { EXTRA, HEAVY, WOODEN, BARS };

    public struct Door
    {
        public bool present;    // Su dvere pritomne?
        public DoorType type;   // Aky typ? Mreze sa nedaju zapalit!
        public int lockNo;      // Lock no
        public bool locked;     // Locked?
        public bool mechanic;   // You have to open with mechanism, not key
        public int lockLevel;   // TN for lockpicking
    }

    public class Road
    {
        public string sourceRoom;    // Room 1 ID
        public string targetRoom;    // Room 2 ID
        public Path direction1;       // Na ktorej svetovej strane room 1
        public Path direction2;       // Na ktorej svetovej strane room 2

        public Door door;

        public Direction bothWay = Direction.BOTH;    // Obojsmerna prevadzka by default
        public bool enabled = true;

        public Road(string sourceRoom, string targetRoom, Path direction)
        {
            this.sourceRoom = sourceRoom;
            this.targetRoom = targetRoom;
            direction1 = direction;

            if (direction1==Path.NORTH) direction2 = Path.SOUTH;
            if (direction1==Path.SOUTH) direction2 = Path.NORTH;
            if (direction1==Path.WEST) direction2 = Path.EAST;
            if (direction1==Path.EAST) direction2 = Path.WEST;
            if (direction1==Path.UP) direction2 = Path.DOWN;
            if (direction1==Path.DOWN) direction2 = Path.UP;

            door.present = false;
            door.type = DoorType.WOODEN;
            door.locked = false;
            door.mechanic = false;
        }

        public static Path GetOpositePath(Path pathway)
        {
            Path res = Path.NORTH;

            if (pathway==Path.NORTH) res = Path.SOUTH;
            if (pathway==Path.SOUTH) res = Path.NORTH;
            if (pathway==Path.WEST) res = Path.EAST;
            if (pathway==Path.EAST) res = Path.WEST;
            if (pathway==Path.UP) res = Path.DOWN;
            if (pathway==Path.DOWN) res = Path.UP;

            return res;
        }

        public static Path PathFromString(string pathway)
        {
            Path res = Path.NORTH;

            string lPath = pathway.ToLower();
            if (lPath=="south") res = Path.SOUTH;
            if (lPath=="west") res = Path.WEST;
            if (lPath=="east") res = Path.EAST;
            if (lPath=="up") res = Path.UP;
            if (lPath=="down") res = Path.DOWN;

            return res;
        }

        public static string GetPathName(Path pathway)
        {
            string res = "sever";

            if (pathway==Path.SOUTH) res = "juh";
            if (pathway==Path.WEST) res = "zapad";
            if (pathway==Path.EAST) res = "vychod";
            if (pathway==Path.UP) res = "hore";
            if (pathway==Path.DOWN) res = "dole";

            return res;
        }
    }
}