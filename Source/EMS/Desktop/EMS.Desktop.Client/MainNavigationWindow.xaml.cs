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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EMS.Desktop.Client
{
    /// <summary>
    /// Interaction logic for MainNavigationWindow.xaml
    /// </summary>
    public partial class MainNavigationWindow : NavigationWindow
    {
        public MainNavigationWindow()
        {
            InitializeComponent();
            this.ShowsNavigationUI = false;
        }
    }
}
