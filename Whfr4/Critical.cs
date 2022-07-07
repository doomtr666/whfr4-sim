namespace Whfr4
{
    public struct Critical
    {
        public delegate void ApplyCritical(FightingCharacter character);

        public int MaxRoll;
        public string Description;
        public int Wounds;
        public ApplyCritical Apply;

        private Critical(int maxRoll, string description, int wounds, ApplyCritical apply)
        {
            MaxRoll = maxRoll;
            Description = description;
            Wounds = wounds;
            Apply = apply;
        }

        private static readonly Critical[] BodyCritical = new[]
        {
            new Critical(10, "Tis But A Scratch!", 1, c=> { c.Conditions.Add(Condition.Bleeding); }),
            new Critical(20, "Gut blow", 1, c=>{ c.Conditions.Add(Condition.Stunned); if(!Dice.SimpleTest(c.Character.Toughness + 40)) c.Conditions.Add(Condition.Prone); }),
            new Critical(25, "Low blow", 1, c=>{ if(!Dice.SimpleTest(c.Character.Toughness - 20)) c.Conditions.Add(Condition.Stunned,3); }),
            new Critical(30, "Twisted back", 1, c=>{ c.Injuries.Add(Injury.MinorTornMuscle); }),
            new Critical(35, "Winded", 2, c=>{ c.Conditions.Add(Condition.Stunned); if(!Dice.SimpleTest(c.Character.Toughness + 20)) c.Conditions.Add(Condition.Prone); }),
            new Critical(40, "Bruised Ribs", 2, c=>{}),
            new Critical(45, "Wrenched Collar Bone", 2, c=>{}),
            new Critical(50, "Ragged Wound", 2, c=>{ c.Conditions.Add(Condition.Bleeding,2); }),
            new Critical(55, "Cracked Ribs", 3, c=>{ c.Conditions.Add(Condition.Stunned); c.Injuries.Add(Injury.MinorBrokenBone); }),
            new Critical(60, "Gaping Wound", 3, c=>{ c.Conditions.Add(Condition.Bleeding, 3); }),
            new Critical(65, "Painful Cut", 3, c=>{ c.Conditions.Add(Condition.Bleeding,2); c.Conditions.Add(Condition.Stunned,2); if(!Dice.SimpleTest(c.Character.Toughness -20)) c.Conditions.Add(Condition.Unconscious); }),
            new Critical(70, "Arterial Damage", 3, c=>{ c.Conditions.Add(Condition.Bleeding,4); }),
            new Critical(75, "Pulled Back", 4, c=>{c.Injuries.Add(Injury.MajorTornMuscle);}),
            new Critical(80, "Fractured Hip", 4, c=>{c.Conditions.Add(Condition.Stunned); if(!Dice.SimpleTest(c.Character.Toughness)) c.Conditions.Add(Condition.Prone); c.Injuries.Add(Injury.MinorBrokenBone); }),
            new Critical(85, "Major Chest Wound", 4, c=>{ c.Conditions.Add(Condition.Bleeding,4);}),
            new Critical(90, "Gut Wound", 4, c=>{ c.Conditions.Add(Condition.Bleeding,2);}),
            new Critical(93, "Smashed Rib Cage", 5, c=>{ c.Conditions.Add(Condition.Stunned); c.Injuries.Add(Injury.MajorBrokenBone); }),
            new Critical(96, "Broken Collar Bone", 5, c=>{ c.Conditions.Add(Condition.Unconscious); c.Injuries.Add(Injury.MajorBrokenBone); }),
            new Critical(99, "Internal bleeding", 5, c=>{ c.Conditions.Add(Condition.Bleeding); }),
            new Critical(100, "Torn Apart", 100, c=>{}),
        };

        public static Critical GetCritical(Localization localization, int roll)
        {
            var table = BodyCritical;

            foreach (var t in table)
                if (roll <= t.MaxRoll)
                    return t;
            throw new ArgumentOutOfRangeException();
        }
    }
}
