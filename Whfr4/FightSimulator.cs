namespace Whfr4
{

    public class FightSimulator
    {
        private DiceRoller Dice { get; }

        public FightSimulator()
        {
            Dice = new DiceRoller();
        }

        public bool GroupDown(IList<FightingCharacter> players, uint group)
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

        public uint NumberOfAttackers(FightingCharacter target, IList<FightingCharacter> players)
        {
            uint attackers = 0;
            foreach(var p in players)
            {
                if (p.IsDown)
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

        void Attack(ILogger log, FightingCharacter active, FightingCharacter passive)
        {
            DiceRoller.OpposedTestResult test;
            Dice.OpposedTest(active.Character.WeaponSkill + 10 * active.Advantage, passive.Character.WeaponSkill + 10 * passive.Advantage, out test);
            log.Debug("{0}", test);

            if(test.Winner == 1)
            {
                active.Advantage++;
                passive.Advantage = 0;
                int weapongDamage = 4;

                var damage = (weapongDamage + Dice.Bonus(active.Character.Strength) + test.SuccessLevel) - Dice.Bonus(passive.Character.Toughness);

                log.Debug("{0} + {1} + {2} - {3}", weapongDamage, Dice.Bonus(active.Character.Strength), test.SuccessLevel, Dice.Bonus(passive.Character.Toughness));
                passive.Wounds -= damage;

                log.Info("{0} hits {1}, {2} damage", active.Character.Name, passive.Character.Name, damage);
                if (passive.IsDown)
                    log.Info("{0} is down", passive.Character.Name);

            }
            else
            {
                active.Advantage = 0;
                passive.Advantage++;
                log.Info("{0} misses {1}", active.Character.Name, passive.Character.Name);
            }
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
            for(var c = 0; c < players.Count; c++)
            {
                log.Info("Name:{0}, Init: {1}", players[c].Character.Name, players[c].Character.Initiative);
                players[c].Order = (uint)c;
            }

            uint round = 0;
            while(true)
            {
                // start the round
                log.Info("Round {0}", round);

                foreach (var attacker in players)
                {
                    if (attacker.IsDown)
                        continue;

                    log.Debug("{0}' turn", attacker.Character.Name);

                    // select a target
                    var target = SelectTarget(log, attacker, players);
                    if (target == null)
                        break;

                    Attack(log, attacker, target);
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
