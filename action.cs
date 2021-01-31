using System.Collections.Generic;

namespace legend
{

    public enum ActionTarget { ITEM, NPC };
    public enum ActionType { DEFAULT, TEST, ATTRIBUTE, ITEM };
    // DEFAULT - ziaden test, ak hrac klikne sem, proste sa vykonaju patricne akcie
    // TEST - skill test, ak hrac klikne, udeje sa skill check a ak uspeje, vykonaju sa patricne akcie
    // ATTRIBUTE - testneme zvoleny atribut, ci je jeho hodnota > ako cilso zadave v LEVEL
    public class Action
    {
        public string id;       // ID samotnej akcie

        public string itemId; // ID objektu, na ktorom je to zavesene
        public ActionTarget actioTarget = ActionTarget.ITEM;
        public ActionType action = ActionType.DEFAULT;
        public string desc;
        
        // IS THIS OPTIOIN SHOWN? ----
        public bool enabled = true; // Is this action enabled?

        // TEST DATA ----
        public Attribute attribute = Attribute.WILL;   // Ktory atribut postavy testujeme?
        public string focus = "RUNNING";         // Ktory focus je mozne zohladnit?
        public int level = 8;
        // successActions - akcie, ktore sa vykonaju po uspesnom teste
        // failedActions - akcie, ktore sa vykonaju, ak test neprejde
        public List<string> successActions = new List<string>();
        public List<string> failedActions = new List<string>();

        public Action(string id, string actionType, ActionTarget actionTarget)
        {
            if (actionType=="test") action = ActionType.TEST;
            if (actionType=="attribute") action = ActionType.ATTRIBUTE;

            this.actioTarget = actionTarget;
            this.id = id;
        }        
    }
}