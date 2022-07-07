namespace Whfr4
{

    public class FightSimulator
    {

        public FightSimulator()
        {
        }

        public bool GroupDown(IList<FightingCharacter> players, int group)
        {
            bool allDown = true;
            
            foreach (var c in players)
            {
                if (c.Group == group && !c.IsDown)
                {
                    allDown = false;
                    break;
                }
            }

            return allDown;
        }

        public int NumberOfAttackers(FightingCharacter target, IList<FightingCharacter> players)
        {
            int attackers = 0;
            foreach(var p in players)
            {
                if (p.IsDown || p.Conditions.Has(Condition.Prone))
                    continue;
                if (p.Target == target)
                    attackers++;
            }

            return attackers;
        }

        FightingCharacter? SelectTarget(ILogger log, FightingCharacter active, IList<FightingCharacter> players)
        {
            if (active.Target == null || active.Target.IsDown)
            {
                // find the list of possible opponents
                var opponents = players.Where(p => p.Group != active.Group && !p.IsDown);
                if (opponents.Count() == 0)
                    return null;

                active.Target = opponents.OrderBy(p => NumberOfAttackers(p, players)).First();

                log.Info("{0} targets {1}", active.Character.Name, active.Target.Character.Name);
            }

            return active.Target;
        }

        int GetAttackerBonus(ILogger log, IList<FightingCharacter> players, FightingCharacter attacker, FightingCharacter defender)
        {
            if (defender.Conditions.Has(Condition.Stunned))
                attacker.Advantage++;

            var bonus = attacker.Advantage * 10;

            if (defender.Conditions.Has(Condition.Prone))
                bonus += 20;

            var attackerCount = NumberOfAttackers(defender, players);
            if (attackerCount == 2)
                bonus += 20;
            if (attackerCount >= 3)
                bonus += 40;

            return bonus;
        }

        int GetDefenderBonus(ILogger log, FightingCharacter attacker, FightingCharacter defender)
        {
            return defender.Advantage * 10;
        }

        void ResolveConditions(ILogger log, FightingCharacter attacker)
        {
            if (attacker.Conditions.Has(Condition.Bleeding))
                attacker.Wounds--;

            if (attacker.Conditions.Has(Condition.Stunned))
                if (Dice.SimpleTest(attacker.Character.Toughness))
                    attacker.Conditions.Remove(Condition.Stunned);
        }

        void Attack(ILogger log, IList<FightingCharacter> players, FightingCharacter attacker, FightingCharacter defender)
        {
            Dice.OpposedTestResult test;

            int attackerBonus = GetAttackerBonus(log, players, attacker, defender);
            int defenderBonus = GetDefenderBonus(log, attacker, defender);

            Dice.OpposedTest(attacker.Character.WeaponSkill + attackerBonus, defender.Character.WeaponSkill + defenderBonus, out test);

            log.Debug("{0}", test);

            // regular test
            if (test.Winner == 1)
            {
                attacker.Advantage++;
                defender.Advantage = 0;

                var damage = attacker.Character.WeaponDamage + attacker.Character.Strength.Bonus() + test.SuccessLevel - defender.Character.Toughness.Bonus();

                log.Debug("{0} + {1} + {2} - {3}", attacker.Character.WeaponDamage, attacker.Character.Strength.Bonus(), test.SuccessLevel, defender.Character.Toughness.Bonus());
                defender.Wounds -= damage;

                log.Info("{0} hits {1}, {2} damage", attacker.Character.Name, defender.Character.Name, damage);
            }
            else
            {
                attacker.Advantage = 0;
                defender.Advantage++;
                log.Info("{0} misses {1}", attacker.Character.Name, defender.Character.Name);
            }

            // handle critical / fumble
            if (test.Test1.Roll.IsDouble())
            {
                if (test.Test1.Success)
                {
                    var critical = Critical.GetCritical(Dice.D100.Localize(), Dice.D100);

                    log.Info("*** {0} critical hits ({1}) {2}", attacker.Character.Name, critical.Description, defender.Character.Name);
                    defender.Wounds -= critical.Wounds;
                    critical.Apply(defender);
                    defender.Advantage = 0;
                }
                else
                {
                    log.Info("*** {0} fumbles", attacker.Character.Name);
                }
            }

            if (test.Test2.Roll.IsDouble())
            {
                if (test.Test2.Success)
                {
                    var critical = Critical.GetCritical(Dice.D100.Localize(), Dice.D100);

                    log.Info("*** {0} critical hits ({1}) {2}", defender.Character.Name, critical.Description, attacker.Character.Name);
                    attacker.Wounds -= critical.Wounds;
                    critical.Apply(attacker);
                    attacker.Advantage = 0;
                }
                else
                {
                    log.Info("*** {0} fumble", defender.Character.Name);
                }
            }

            ResolveConditions(log, attacker);

            if(attacker.IsDown)
                log.Info("{0} is down", attacker.Character.Name);
            if (defender.IsDown)
                log.Info("{0} is down", defender.Character.Name);
        }

        public bool SimulateFight(ILogger log, IList<Character> pc, IList<Character> npc)
        {
            log.Info("Fight {0} vs {1}, Initiative order:", pc.Count, npc.Count);

            // generate a list of FigthingCharacters
            var players = new List<FightingCharacter>();
            foreach (var c in pc)
                players.Add(new FightingCharacter(c, 1));
            foreach (var c in npc)
                players.Add(new FightingCharacter(c, 2));

            // order by Initiative
            players.Sort((a,b) => -a.Character.Initiative.CompareTo(b.Character.Initiative));
            foreach(var c in players)
                log.Info("Name:{0}, Init: {1}", c.Character.Name, c.Character.Initiative);

            int round = 0;
            while(true)
            {
                // start the round
                log.Info("Round {0}", round);

                foreach (var attacker in players)
                {
                    if (attacker.IsDown || attacker.Conditions.Has(Condition.Stunned))
                    {
                        ResolveConditions(log, attacker);
                        continue;
                    }

                    log.Debug("{0}' turn", attacker.Character.Name);

                    // select a target
                    var target = SelectTarget(log, attacker, players);
                    if (target == null)
                        break;

                    Attack(log, players, attacker, target);
                }

                // check if either group is down
                if (GroupDown(players, 2))
                {
                    log.Info("PCs wins");
                    return true;
                }

                if (GroupDown(players, 1))
                {
                    log.Info("NPCs wins");
                    return false;
                }

                // any character in numeric inferiority looses 1 advantage at the end of the round
                foreach (var attacker in players)
                {
                    if (attacker.IsDown)
                        continue;

                    if(NumberOfAttackers(attacker, players) > 1)
                    {
                        if (attacker.Advantage > 1)
                            attacker.Advantage--;
                    }
                }

                round++;
            }
        }

        public void FightStatistics(ILogger log, IList<Character> pcs, IList<Character> npcs)
        { 
            var nullLogger = new NullLogger();

            var trials = 100000;
            var counter = 0;
            for(var i = 0; i < trials; i++)
            {
                if (SimulateFight(nullLogger, pcs, npcs))
                    counter++;
            }

            log.Info("Win probability {0:0.0}%", 100.0f * counter / trials);
        }
    }
}
