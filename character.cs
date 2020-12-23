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

        public static Attribute GetAttribute(string attrString)
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