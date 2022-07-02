using Whfr4;

namespace App.ViewModels
{
    public class CharacterViewModel : ViewModelBase
    {
        public Character Character { get; set; }


        public CharacterViewModel(Character character)
        {
            Character = character;
        }

        public CharacterViewModel()
        {
            Character = new Character();
        }
    }
}
