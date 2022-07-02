using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using App.ViewModels;
using Whfr4;

namespace App.Views
{
    public partial class CharacterView : Window
    {
        public CharacterView(Character character)
        {
            Width = 300;
            Height = 300;

            if (character == null)
                DataContext = new CharacterViewModel();
            else
                DataContext = new CharacterViewModel(character);

            InitializeComponent();

        }

        public CharacterView() : this(null)
        {
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }


        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as CharacterViewModel;
                
            Close(vm!.Character);
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
