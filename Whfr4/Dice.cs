namespace Whfr4
{
    public static class Dice
    {
        public struct DramaticTestResult
        {
            public int SkillBase;
            public bool Success;
            public int SuccessLevel;
            public int Roll;
            public override string ToString()
            {
                return string.Format("(S: {0}, Success: {1}, SL: {2}, Roll: {3})", SkillBase, Success, SuccessLevel, Roll);
            }
        }

        public struct OpposedTestResult
        {
            public int ActiveBase;
            public int PassiveBase;
            public int Winner;
            public int SuccessLevel;
            public DramaticTestResult Test1;
            public DramaticTestResult Test2;

            public override string ToString()
            {
                return string.Format("(A: {0}, P: {1}, Win: {2}, SL: {3}, {4}, {5})", ActiveBase, PassiveBase, Winner, SuccessLevel, Test1, Test2);
            }
        }

        static readonly Random _rand = new Random();

        public static int D10 { get { return _rand.Next(1, 10); } }
        public static int D100 { get { return _rand.Next(1, 100); } }

        public static bool SimpleTest(int skillBase)
        {
            var roll = D100;
            if (roll < 6)
                return true;
            if (roll > 95)
                return false;
            return roll <= skillBase;
        }

        public static void DramaticTest(int skillBase, out DramaticTestResult result)
        {
            var roll = D100;
            result.SkillBase = skillBase;
            result.Success = roll <= skillBase;
            result.Roll = roll;
            result.SuccessLevel = skillBase.Bonus() - roll.Bonus();
        }

        public static void OpposedTest(int activeBase, int passiveBase, out OpposedTestResult result)
        {
            DramaticTest(activeBase, out result.Test1);
            DramaticTest(passiveBase, out result.Test2);
            
            result.ActiveBase = activeBase;
            result.PassiveBase = passiveBase;

            if(result.Test1.SuccessLevel > result.Test2.SuccessLevel)
            {
                result.Winner = 1;
                result.SuccessLevel = result.Test1.SuccessLevel - result.Test2.SuccessLevel;

            } else if (result.Test1.SuccessLevel < result.Test2.SuccessLevel)
            {
                result.Winner = 2;
                result.SuccessLevel = result.Test2.SuccessLevel - result.Test1.SuccessLevel;

            }
            else
            {
                // TODO: handle strict equality
                // for now it's not exactly the official rules
                if(activeBase >= passiveBase)
                {
                    result.Winner = 1;
                    result.SuccessLevel = 0;
                }
                else
                {
                    result.Winner = 1;
                    result.SuccessLevel = 0;
                }
            }
        }
    }
}
