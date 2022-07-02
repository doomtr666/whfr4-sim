using Avalonia.Controls;
using Avalonia.Interactivity;
using App.ViewModels;
using Whfr4;

namespace App.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void OnAddCharacterClick(object sender, RoutedEventArgs e)
        {
            var dialog = new CharacterView();
            var result = await dialog.ShowDialog<Character?>(this);

            if(result != null)
            {
                var vm = DataContext as MainWindowViewModel;
                vm!.Characters.Add(result);
            }
        }

        private async void OnEditCharacterClick(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;
            if (vm == null || vm.SelectedCharacter < 0)
                return;

            var copyCharacter = new Character(vm.Characters[vm.SelectedCharacter]);

            var dialog = new CharacterView(copyCharacter);
            var result = await dialog.ShowDialog<Character?>(this);

            if (result != null)
                vm.Characters[vm.SelectedCharacter] = result;
        }

        private async void OnAddMonsterClick(object sender, RoutedEventArgs e)
        {
            var dialog = new CharacterView();
            var result = await dialog.ShowDialog<Character?>(this);

            if (result != null)
            {
                var vm = DataContext as MainWindowViewModel;
                vm!.Monsters.Add(result);
            }
        }

        private async void OnEditMonsterClick(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;
            if (vm == null || vm.SelectedMonster < 0)
                return;

            var copyMonster = new Character(vm.Monsters[vm.SelectedMonster]);

            var dialog = new CharacterView(copyMonster);
            var result = await dialog.ShowDialog<Character?>(this);

            if (result != null)
                vm.Monsters[vm.SelectedMonster] = result;
        }
    }
}
