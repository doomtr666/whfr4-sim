namespace Whfr4
{
    public class DiceRoller
    {
        public struct DramaticTestResult
        {
            public uint SkillBase;
            public bool Success;
            public int SuccessLevel;
            public uint Roll;
            public override string ToString()
            {
                return string.Format("(S: {0}, Success: {1}, SL: {2}, Roll: {3})", SkillBase, Success, SuccessLevel, Roll);
            }
        }

        public struct OpposedTestResult
        {
            public uint ActiveBase;
            public uint PassiveBase;
            public uint Winner;
            public int SuccessLevel;
            public DramaticTestResult Test1;
            public DramaticTestResult Test2;

            public override string ToString()
            {
                return string.Format("(A: {0}, P: {1}, Win: {2}, SL: {3}, {4}, {5})", ActiveBase, PassiveBase, Winner, SuccessLevel, Test1, Test2);
            }
        }

        Random _rand;

        public DiceRoller()
        {
            _rand = new Random();
        }

        public uint D10 { get { return (uint)_rand.Next(1, 10); } }
        public uint D100 { get { return (uint)_rand.Next(1, 100); } }

        public int Bonus(uint value)
        {
            return (int)value / 10;
        }

        public void DramaticTest(uint skillBase, out DramaticTestResult result)
        {
            var roll = D100;
            result.SkillBase = skillBase;
            result.Success = roll <= skillBase;
            result.Roll = roll;
            result.SuccessLevel = (int)Bonus(skillBase) - (int)Bonus(roll);
        }

        public void OpposedTest(uint activeBase, uint passiveBase, out OpposedTestResult result)
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
