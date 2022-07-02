using Whfr4;

namespace App.ViewModels
{
    internal class CombatLogger : ILogger
    {
        private readonly MainWindowViewModel vm;

        internal CombatLogger(MainWindowViewModel vm)
        {
            this.vm = vm;
        }

        public void Info(string format, params object[] args)
        {
            vm.AddInfo(format, args);
        }

        public void Debug(string format, params object[] args)
        {
            //vm.AddInfo(format, args);
        }
    }
}
