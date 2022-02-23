namespace LegendLibrary
{
    public struct EnemyWheapon
    {
        public string name;
        public int attackRoll;
        public string damage;
        public CharAttr attribute;
    }

    public class Enemy
    {
        public string id, name;
        public string desc; // Appearance, description (ID of text block)

        public int speed, health, defense, armor;
        
        public int[] attr = new int[9];

        public EnemyWheapon wheapon;

        public Enemy(string id)
        {
            this.id = id;
        }

        public Enemy()
        {
            id = "";
        }

        /// <summary>
        /// Set up enemy wheapon. (Only 1 whapon for now)
        /// </summary>
        /// <param name="name">Wheapon name</param>
        /// <param name="attackRoll">Modification to attack roll, to be used when attacking</param>
        /// <param name="damage">Damage on success test</param>
        public void AddWheapon(string name, int attackRoll, string damage, CharAttr attr)
        {
            wheapon.name = name;
            wheapon.attackRoll = attackRoll;
            wheapon.damage = damage;
            wheapon.attribute = attr;
        }

        public void SetAttribute(CharAttr attr, int value) => this.attr[(int)attr] = value;
        public int GetAttribute(CharAttr attr) => this.attr[(int)attr];
    }
}