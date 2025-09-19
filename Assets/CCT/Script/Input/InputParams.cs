using System.Collections.Generic;

namespace CCT.Script
{
    public class InputParams
    {
        public static bool AttackTap { get; private set; }
        public static bool StartAttackHold { get; private set; }
        public static bool AttackInHold { get; private set; }
        public static bool AttackOutHold { get; private set; }

        public void Analyze(List<FrameInputInfo> frameInputInfos)
        {
            ResetFrameFlags();
            AnalysisAttack(frameInputInfos);
        }

        private void AnalysisAttack(List<FrameInputInfo> inputs)
        {
            var tap = false;
            var holdPerformed = false;
            var holdCanceled  = false;

            foreach (var input in inputs)
            {
                if (input.buttonType != ButtonType.Attack) continue;
                tap           |= input.inputType == InputType.Tap;
                holdPerformed |= input.inputType == InputType.HoldPerformed;
                holdCanceled  |= input.inputType == InputType.HoldCanceled;
            }

            var wasInHold = AttackInHold;

            AttackOutHold   = (wasInHold || holdPerformed) && holdCanceled;
            StartAttackHold = holdPerformed && !holdCanceled;
            AttackInHold    = (wasInHold || holdPerformed) && !holdCanceled;

            AttackTap = tap && !StartAttackHold && !AttackOutHold && !AttackInHold;
        }

        private void ResetFrameFlags()
        {
            AttackTap = false;
            StartAttackHold = false;
            AttackOutHold = false;
        }
    }
}