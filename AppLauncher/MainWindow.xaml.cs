namespace AppLauncher
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Xml.Linq;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent(); 
            
            try
            {
                XDocument doc = XDocument.Load( Properties.Settings.Default.SOURCE );
                icApplications.ItemsSource = doc.Root
                    .Elements()
                    .Select( node => new CommandData()
                    {
                        Label = node.Element( "label" ).Attribute( "value" ).Value,
                        Target = node.Element( "path" ).Attribute( "value" ).Value,
                        StartInPath = node.Element( "start_in" ).Attribute( "value" ).Value,
                        Type = node.Element( "path" ).Attribute( "type" ).Value
                    } )
                    .OrderBy( node => node.Label );
            }
            catch ( Exception ex )
            {
                MessageBox.Show( ex.Message );
            }
        }


        private ICommand _exitCommand;
        public ICommand ExitCommand
        {
            get
            {
                if ( _exitCommand == null )
                {
                    _exitCommand = new ApplicationCommand(
                        param => this.Exit(),
                        param => this.CanExecute()
                    );
                }
                return ApplicationCommands.Close;
            }

        }


        private ICommand _lookupCommand;
        public ICommand LookupCommand
        {
            get
            {
                if ( _lookupCommand == null )
                {
                    _lookupCommand = new ApplicationCommand(
                        param => this.DoLookup(),
                        param => this.CanExecute()
                    );
                }
                return _lookupCommand;
            }

        }


        /// <summary>
        /// Determines whether the command can execute.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the command can execute; otherwise, <c>false</c>.
        /// </returns>
        private bool CanExecute()
        {
            return true;
        }

        /// <summary>
        /// Executes a call to display an application shortcut editor
        /// </summary>
        private void DoLookup()
        {
            try
            {
                var window = new ApplicationEditorWindow();
                bool? result = window.ShowDialog();
                if ( result.HasValue && result.Value )
                {
                    // write the new application data to file
                    // reload the application list
                }
            }
            catch ( Exception ex )
            {
                MessageBox.Show( ex.Message, "Application Error", MessageBoxButton.OK, MessageBoxImage.Error );
            }
        }

        /// <summary>
        /// Exits this instance.
        /// </summary>
        private void Exit()
        {
            Application.Current.Shutdown();
        }


        /// <summary>
        /// Handles the Click event of the ExitButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void ExitButton_Click( object sender, RoutedEventArgs e )
        {
            this.Exit();
        }


        /// <summary>
        /// Handles the Click event of the AddApplicationButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void AddApplicationButton_Click( object sender, RoutedEventArgs e )
        {
            this.DoLookup();
        }
    }
}
