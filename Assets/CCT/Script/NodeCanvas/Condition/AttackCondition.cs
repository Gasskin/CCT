using NodeCanvas.Framework;

namespace CCT.Script
{
    public enum AttackType
    {
        Tap,
        StartHold,
        InHold,
        OutHold
    }
    
    public class AttackCondition : ConditionTask
    {
        public AttackType attackType = AttackType.Tap;
        
        protected override bool OnCheck()
        {
            switch (attackType)
            {
                case AttackType.Tap:
                    return InputParams.AttackTap;
                case AttackType.StartHold:
                    return InputParams.StartAttackHold;
                case AttackType.InHold:
                    return InputParams.AttackInHold;
                case AttackType.OutHold:
                    return InputParams.AttackOutHold;
            }
            return false;
        }
    }
}