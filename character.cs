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
    public class Character
    {
        public string name = "Hero";
        public BRClass brclass = BRClass.WARRIOR;
        public Race race = Race.HUMAN;
        public bool female = true;
        public int level = 1;
        public Specialization specialization = Specialization.NONE;

        public int[] attr = new int[9];

        public void SetAttribute(Attribute inAttr, int value) => attr[(int)Attribute.ACCURACY] = value;

        public int GetAttribute(Attribute inAttr) => attr[(int)Attribute.ACCURACY];

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
                SetAttribute(Attribute.ACCURACY,1);
                SetAttribute(Attribute.COMMUNICATION,1);
                SetAttribute(Attribute.CONSTITUTION,1);
                SetAttribute(Attribute.DEXTERITY,1);
                SetAttribute(Attribute.FIGHTING,1);
                SetAttribute(Attribute.IQ,1);
                SetAttribute(Attribute.PERCEPTION,1);
                SetAttribute(Attribute.STRENGTH,1);
                SetAttribute(Attribute.WILL,1);
            }
            else
            {
                name = "Scamm";
                female = false; // Default male
                SetAttribute(Attribute.ACCURACY,1);
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
            if (li=="communication") res = Attribute.ACCURACY;
            if (li=="constitution") res = Attribute.ACCURACY;
            if (li=="dexterity") res = Attribute.ACCURACY;
            if (li=="fighting") res = Attribute.ACCURACY;
            if (li=="iq") res = Attribute.ACCURACY;
            if (li=="perception") res = Attribute.ACCURACY;
            if (li=="stregth") res = Attribute.ACCURACY;
            if (li=="will") res = Attribute.ACCURACY;

            return res;
        } 

    }
}