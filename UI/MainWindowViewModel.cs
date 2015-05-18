using Devkoes.ReleaseManager.Discovery;
using Devkoes.ReleaseManager.Discovery.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace UI
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action _commandAction;

        public RelayCommand(Action commandAction)
        {
            _commandAction = commandAction;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _commandAction();
        }
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private Solution _selectedSolution;

        private List<Solution> _allSolutions { get; } = new List<Solution>();
        public ObservableCollection<Solution> Solutions { get; } = new ObservableCollection<Solution>();
        public ObservableCollection<string> ConsoleOutput { get; } = new ObservableCollection<string>();
        public RelayCommand BuildSolution { get; private set; }
        public RelayCommand SearchSolutionCommand { get; private set; }
        public string SearchText { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            BuildSolution = new RelayCommand(this.HandleBuildCommand);
            SearchSolutionCommand = new RelayCommand(this.HandleSearchSolutionCommand);

            InitializeSolutionList();
        }

        private void InitializeSolutionList()
        {
            Solutions.Clear();

            ICollectionView solView = CollectionViewSource.GetDefaultView(Solutions);
            solView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            solView.Filter = FilterSolutions;

            foreach (var s in new SolutionDiscovery().Discover(@"c:\_projects"))
            {
                _allSolutions.Add(s);
                Solutions.Add(s);
            }
        }

        private bool FilterSolutions(object obj)
        {
            var sol = obj as Solution;
            if(sol == null)
            {
                return false;
            }

            return string.IsNullOrWhiteSpace(SearchText) || sol.Name.ToUpperInvariant().Contains(SearchText.ToUpperInvariant());
        }

        private async void HandleBuildCommand()
        {
            ConsoleOutput.Clear();

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "msbuild.exe",
                    Arguments = SelectedSolution.BuildScriptAbsolutePath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();

            await Task.Run(() =>
            {
                while (!proc.StandardOutput.EndOfStream)
                {
                    var output = proc.StandardOutput.ReadLine();
                    Application.Current.Dispatcher.Invoke(() => ConsoleOutput.Insert(0, output));
                }
            });
        }

        private void HandleSearchSolutionCommand()
        {
            CollectionViewSource.GetDefaultView(Solutions).Refresh();
        }

        public Solution SelectedSolution
        {
            get { return _selectedSolution; }
            set
            {
                _selectedSolution = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedSolution"));
            }
        }

    }
}
