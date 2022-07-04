using System.Collections.ObjectModel;
using Whfr4;
using ReactiveUI;

namespace App.ViewModels
{

    public class MainWindowViewModel : ViewModelBase
    {
        private string combatLog = "";

        private int selectedCharacter;
        private bool characterButtonEnabled;

        private int selectedMonster;
        private bool monsterButtonEnabled;

        public MainWindowViewModel()
        {
            Characters = new ObservableCollection<Character>
            {
                new Character { Name = "Bob", WeaponSkill = 42, BallisticSkill = 27, Initiative = 37, Toughness = 32, Strength = 41, Wounds = 16, WeaponDamage = 4,},
                new Character { Name = "Alice", WeaponSkill = 39, BallisticSkill = 31, Initiative = 25, Toughness = 42, Strength = 32, Wounds = 14, WeaponDamage = 2 }
            };

            Monsters = new ObservableCollection<Character>
            {
                new Character { Name = "Zombie", WeaponSkill = 15, BallisticSkill = 1, Initiative = 5, Toughness = 30, Strength = 30, Wounds = 12, WeaponDamage = 4 },
                new Character { Name = "Zombie", WeaponSkill = 15, BallisticSkill = 1, Initiative = 5, Toughness = 30, Strength = 30, Wounds = 12, WeaponDamage = 4 },
                new Character { Name = "Zombie", WeaponSkill = 15, BallisticSkill = 1, Initiative = 5, Toughness = 30, Strength = 30, Wounds = 12, WeaponDamage = 4 },
                new Character { Name = "Zombie", WeaponSkill = 15, BallisticSkill = 1, Initiative = 5, Toughness = 30, Strength = 30, Wounds = 12, WeaponDamage = 4 },
                new Character { Name = "Zombie", WeaponSkill = 15, BallisticSkill = 1, Initiative = 5, Toughness = 30, Strength = 30, Wounds = 12, WeaponDamage = 4 },
                new Character { Name = "Zombie", WeaponSkill = 15, BallisticSkill = 1, Initiative = 5, Toughness = 30, Strength = 30, Wounds = 12, WeaponDamage = 4 }
            };

            selectedCharacter = -1;
            characterButtonEnabled = false;
            selectedMonster = -1;
            monsterButtonEnabled = false;
        }

        public ObservableCollection<Character> Characters { get; }

        public int SelectedCharacter
        {
            get => selectedCharacter;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedCharacter, value);
                CharacterButtonEnabled = value >= 0;
            }
        }

        public bool CharacterButtonEnabled { get => characterButtonEnabled; set => this.RaiseAndSetIfChanged(ref characterButtonEnabled, value); }

        public ObservableCollection<Character> Monsters { get; }

        public int SelectedMonster
        {
            get => selectedMonster;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedMonster, value);
                MonsterButtonEnabled = value >= 0;
            }
        }

        public bool MonsterButtonEnabled { get => monsterButtonEnabled; set => this.RaiseAndSetIfChanged(ref monsterButtonEnabled, value); }

        public string CombatLog { get => combatLog; set => this.RaiseAndSetIfChanged(ref combatLog, value); }

        public void ClearLogs()
        {
            CombatLog = "";
        }
        public void AddInfo(string format, params object[] args)
        {
            CombatLog += string.Format(format + "\n", args);
        }

        public void FightCommand()
        {
            ClearLogs();

            var sim = new FightSimulator();

            var logger = new CombatLogger(this);
            sim.SimulateFight(logger, Characters, Monsters);
        }

        public void StatsCommand()
        {
            ClearLogs();

            var sim = new FightSimulator();
            var logger = new CombatLogger(this);
            sim.FightStatistics(logger, Characters, Monsters);
        }

        public void RemoveCharacterCommand()
        {
            if (selectedCharacter >= 0)
                Characters.RemoveAt(selectedCharacter);
        }

        public void RemoveMonsterCommand()
        {
            if (selectedMonster >= 0)
                Monsters.RemoveAt(selectedMonster);
        }

    }
}
