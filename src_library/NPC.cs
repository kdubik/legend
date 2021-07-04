namespace LegendLibrary
{
    /*
    Behavior - friendliness:
    1. Friendly, do not attack, talks first (actions allowed)
    2. Standard, wait for player for conversation (actions allowed)
    3. Unfriendly - talk first before attack or some demand (no action allowed - cannost be scared, bribed etc.)
    4. Enemy - just attack player, (no action allowed)
    */

    public class NPC:Enemy
    {
        public bool alive = true;
        public string position = "";
        public BRClass brClass = BRClass.EXPERT;
        public int friendliness = 1;    //Standard NPC

        public string greetings;   // When NPC greets player (friendliness should be 1)

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">ID of this new NPC</param>
        public NPC(string id)
        {
            this.id = id;
        }

        /// <summary>
        /// Get NPC specific attribute
        /// </summary>
        /// <param name="attr">Attribute code</param>
        /// <returns>Value (int)</returns>
        //public int GetAttr(Attribute attr) => this.attr[(int)attr];
    }
}