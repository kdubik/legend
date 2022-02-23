namespace LegendLibrary
{
    /*
    Behavior - friendliness:
    1. Friendly, do not attack, talks first (actions allowed)
    2. Standard, wait for player for conversation (actions allowed)
    3. Unfriendly - talk first before attack or some demand (no action allowed - cannost be scared, bribed etc.)
    4. Enemy - just attack player, (no action allowed)
    */

    /*
    --- Ako sa môžu NPC postavy pohybovať:
    1. Náhodná teleportácia po mape (RANDOM_MAP)
    2. Náhodný pohyb po mape (RANDOM_NEAREST)
    3. Po odchode hráča z lokality postava zmizne a viac sa nevráti (UNIQUE)
    4. Nasledovanie presnej trasy (a pripadne nazad) (PATH)
    5. Hladanie trasy z bodu A do BA (a pripadne nazad) (PATHFINDING)
    */
    public enum Movement { STATIC, RANDOM_MAP, RANDOM_NEAREST, UNIQUE};
    public class NPC:Enemy
    {
        public bool alive = true;
        public string position = "";    // Which room on actual map is this NPC currently present?
        public string actualMap = "";   // Which map this NPC belongs to?
        public BRClass brClass = BRClass.EXPERT;
    
        public string greeting = "";   // When NPC greets player (friendliness should be 1)
        public bool alreadyGreet = false;   // Does this NPC already greet a player?

        public int friendliness = 0;    //Standard enemy (1 friend, 0 none, -1 enemy, -2 angry enemy)

        public Movement movement = Movement.RANDOM_MAP; // By default, NPC is not moving, just stay

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">ID of this new NPC</param>
        public NPC(string id)
        {
            this.id = id;
            this.friendliness = 0;  // No enemy, just stranger
            this.position = "";
        }

        /// <summary>
        /// Get NPC specific attribute
        /// </summary>
        /// <param name="attr">Attribute code</param>
        /// <returns>Value (int)</returns>
        //public int GetAttr(Attribute attr) => this.attr[(int)attr];
    }
}