using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using RastrPVEq.Models.RastrWin3;
using RastrPVEq.Models.Topology;
using RastrPVEq.Infrastructure.RastrWin3;
using RastrPVEq.Infrastructure.Commands;

namespace RastrPVEq.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// List of nodes field
        /// </summary>
        private List<Node> _nodes = new();

        /// <summary>
        /// Gets or sets list of nodes
        /// </summary>
        public List<Node> Nodes { get => _nodes; set => Set(ref _nodes, value); }

        /// <summary>
        /// DownloadFileCommand command property
        /// </summary>
        public AsyncCommand DownloadFileCommand { get; }

        /// <summary>
        /// On DownloadFileCommand execute
        /// </summary>
        /// <param name="p"></param>
        private void OnDownloadFileCommandExecute(object p)
        {
            OpenFileDialog openFileDialog = new()
            {
                Title = "Открыть файл режима",
                Filter = "Файл режима (*.rg2)|*.rg2"
            };

            bool? response = openFileDialog.ShowDialog();

            if (response == false) return;

            try
            {
                var templatePath = "C:\\Users\\mishk\\source\\repos\\RastrPVEq\\RastrPVEqConsole\\Resources\\Templates\\режим.rg2";
                RastrSupplier.LoadFileByTemplate(openFileDialog.FileName, templatePath);

                var nodeTask = RastrSupplierAsync.GetNodesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Can DownloadFileCommand execute
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool CanDownloadFileCommandExecute(object p) => true;

        /// <summary>
        /// MainWindowViewModel instance constructor
        /// </summary>
        public MainWindowViewModel()
        {
            DownloadFileCommand = new AsyncCommand();
        }
    }
}
