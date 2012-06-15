namespace AppLauncher
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;
    using System.Diagnostics;



    public class ApplicationCommand : ICommand
    {
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        public ApplicationCommand( Action<object> execute )
            : this( execute, null )
        {
            ;
        }

        public ApplicationCommand( Action<object> execute, Predicate<object> canExecute )
        {
            if ( execute == null )
            {
                throw new ArgumentNullException( "execute" );
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        [DebuggerStepThrough]
        public bool CanExecute( object parameters )
        {
            return _canExecute == null ? true : _canExecute( parameters );
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute( object parameters )
        {
            _execute( parameters );
        }

    }
}
