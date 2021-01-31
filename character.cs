namespace legend
{
    public enum Background { ALDIN, FOREST_FOLK, JARZONI, KERNISH, LARTYAN, OUTCAST, ROAMER };
    public enum Race { HUMAN, NIGHT, SEA, DARK_VATA, VATA };
    public enum BRClass { ADEPT, WARRIOR, EXPERT };
    public enum Specialization { NONE, BERSERKER, BEAST_FRIEND, BARD, DIPLOMAT, DUELIST, HEALER, HUNTER, MARTIAL_ARTIST, SHAPER, SLAYER, SPIRIT_DANCER, SPY };
    public enum Attribute
    { 
        ACCURACY, COMMUNICATION, CONSTITUTION, DEXTERITY, FIGHTING, IQ, PERCEPTION, STRENGTH, WILL 
    }

    public enum BodySlot { WHEAPON, SHIELD, ARMOR };

    public class Character
    {
        public string name = "Hero";
        public BRClass brclass = BRClass.WARRIOR;
        public Race race = Race.HUMAN;
        public bool female = true;
        public int level = 1;
        public Specialization specialization = Specialization.NONE;

        // Mensie info
        public int max_health, health;
        public int speed, defense, armor;


        // Character attributes
        public int[] attr = new int[9];

        // Character body equipment
        // Obsahuje ID equipnuteho predmetu, alebo nic
        public string[] bodySlots = new string[3];

        public void SetAttribute(Attribute inAttr, int value) => attr[(int)inAttr] = value;

        public int GetAttribute(Attribute inAttr) => attr[(int)inAttr];

        /// <summary>
        /// Returns ID of item (wheapon), that character has equipped.false
        /// If there is no wheapon, "empty_hands" is returned.
        /// </summary>
        /// <returns>ID of item (wheapon), that character has equipped</returns>
        public string GetActualWheaponId()
        {
            string res = "";

            res = bodySlots[(int)BodySlot.WHEAPON];
            if (res!="") res = "";

            return res;
        }

        /// <summary>
        /// Constructor - prepare default character
        /// </summary>
        /// <param name="female">Is character female? (Man by default)</param>
        public Character(bool female)
        {
            // Defaults
            // Race = HUMAN
            // Class = WARRIOR
            // Level = 1

            if (female)
            {
                name = "Sienna";
                female = true; // Default male
                max_health = 25;
                health = max_health;
                defense = 12;
                speed = 11;
                armor = 0;
                SetAttribute(Attribute.ACCURACY,1);
                SetAttribute(Attribute.COMMUNICATION,1);
                SetAttribute(Attribute.CONSTITUTION,1);
                SetAttribute(Attribute.DEXTERITY,1);
                SetAttribute(Attribute.FIGHTING,1);
                SetAttribute(Attribute.IQ,2);
                SetAttribute(Attribute.PERCEPTION,1);
                SetAttribute(Attribute.STRENGTH,1);
                SetAttribute(Attribute.WILL,1);
            }
            else
            {
                name = "Scamm";
                female = false; // Default male
                max_health = 25;
                health = max_health;
                defense = 12;
                speed = 11;
                armor = 0;
                SetAttribute(Attribute.ACCURACY,2);
                SetAttribute(Attribute.COMMUNICATION,1);
                SetAttribute(Attribute.CONSTITUTION,1);
                SetAttribute(Attribute.DEXTERITY,1);
                SetAttribute(Attribute.FIGHTING,1);
                SetAttribute(Attribute.IQ,1);
                SetAttribute(Attribute.PERCEPTION,1);
                SetAttribute(Attribute.STRENGTH,1);
                SetAttribute(Attribute.WILL,1);
            }
        }

        public static Attribute GetAttributeFromString(string attrString)
        {
            Attribute res = Attribute.ACCURACY;
            string li = attrString.ToLower();

            if (li=="accuracy") res = Attribute.ACCURACY;
            if (li=="communication") res = Attribute.COMMUNICATION;
            if (li=="constitution") res = Attribute.CONSTITUTION;
            if (li=="dexterity") res = Attribute.DEXTERITY;
            if (li=="fighting") res = Attribute.FIGHTING;
            if (li=="iq") res = Attribute.IQ;
            if (li=="perception") res = Attribute.PERCEPTION;
            if (li=="strength") res = Attribute.STRENGTH;
            if (li=="will") res = Attribute.WILL;

            return res;
        } 

    }
}