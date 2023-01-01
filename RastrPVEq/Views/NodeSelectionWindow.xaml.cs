using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RastrPVEq.ViewModels;

namespace RastrPVEq.Views
{
    /// <summary>
    /// Логика взаимодействия для NodeSelection.xaml
    /// </summary>
    public partial class NodeSelectionWindow : Window
    {
        public NodeSelectionWindow(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            DataContext = mainWindowViewModel;
        }
    }
}
