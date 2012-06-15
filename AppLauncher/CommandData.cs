namespace AppLauncher
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;
    using System.IO;

    public class CommandData
    {
        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public string Target { get; set; }

        /// <summary>
        /// Gets or sets the 'start in' path.
        /// </summary>
        /// <value>
        /// The 'start in' path.
        /// </value>
        public string StartInPath { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }


        private ICommand _applicationCommand;


        /// <summary>
        /// Gets the execute command.
        /// </summary>
        public ICommand ExecuteCommand
        {
            get
            {
                if ( _applicationCommand == null )
                {
                    _applicationCommand = new ApplicationCommand(
                        param => this.Execute(),
                        param => this.CanExecute()
                    );
                }
                return _applicationCommand;
            }

        }


        /// <summary>
        /// Determines whether this instance can execute.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can execute; otherwise, <c>false</c>.
        /// </returns>
        private bool CanExecute()
        {
            return this.Type == "exe" ? File.Exists( this.Target ) : true;
        }


        /// <summary>
        /// Executes this instance.
        /// </summary>
        private void Execute()
        {
            try
            {
                if ( !string.IsNullOrWhiteSpace( this.Target ) )
                {
                    ThreadPool.QueueUserWorkItem( delegate
                    {
                        ProcessStartInfo info = new ProcessStartInfo();
                        info.WorkingDirectory = this.StartInPath;
                        info.FileName = this.Target;
                        Process.Start( info );
                    } );
                }
                else
                {
                    MessageBox.Show( "The path for this command is either invalid or not currently accessible.", "Command Error", MessageBoxButton.OK, MessageBoxImage.Error );
                }
            }
            catch ( Exception ex )
            {
                ; // TODO: do something with exception
            }
        }
    }
}
