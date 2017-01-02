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

namespace NetworkAdapterSwitch
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //SetColor
            Random rand = new Random();
            int R = rand.Next(50, 150);
            int G = rand.Next(50, 150);
            int B = rand.Next(50, 150);
            this.OuterBackground.Background = new SolidColorBrush(Color.FromArgb(255, (byte)R, (byte)G, (byte)B));
            this.InnerBackground.Background = new SolidColorBrush(Color.FromArgb(255, (byte)(R - 50), (byte)(G - 50), (byte)(B - 50)));

            //Set Adapters
            LoadNetworkAdapters();
        }
        private void LoadNetworkAdapters()
        {
            Dictionary<string, bool> nAdapters = NetworkAdapterConnection.GetNetworkAdapters();
            int gridCount = 0;

            foreach (KeyValuePair<string, bool> nAdapterPair in nAdapters)
            {
                Grid newGrid = new Grid();
                Grid.SetRow(newGrid, gridCount++);

                NetworkAdapter nAdapterControl = new NetworkAdapter(nAdapterPair.Key);
                nAdapterControl.NetworkAdapterEnabled = nAdapterPair.Value;

                newGrid.Children.Add(nAdapterControl);

                RowDefinition newGridRow = new RowDefinition();
                newGridRow.Height = new GridLength(1, GridUnitType.Star);

                this.GridContents.RowDefinitions.Add(newGridRow);
                this.GridContents.Children.Add(newGrid);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
