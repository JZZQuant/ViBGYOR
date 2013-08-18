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

namespace ViBGYOR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class FramelessWindow : Window
    {
        public static RoutedCommand AddNewCultureElementCommand = new RoutedCommand();

        public FramelessWindow()
        {
            InitializeComponent();
            MouseLeftButtonDown += delegate { DragMove(); };
        }

        private void ExpandMainContextMenu(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddNewCultureElement(object sender, ExecutedRoutedEventArgs e)
        {
            var vc = new ViBGYOR.Controls.CultureElement();
            vc.Curvature = 7;
            vc.Background = Brushes.Red;
            vc.Height = 20;
            vc.HorizontalAlignment = HorizontalAlignment.Stretch;
            DockPanel.SetDock(vc, Dock.Top);
            this.LeftDock.Children.Add(vc);
        }
    }
}
