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

namespace Dominoes
{
    /// <summary>
    /// Логика взаимодействия для MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        private readonly MainWindow window;

        public MenuView(MainWindow window)
        {
            this.window = window;
            InitializeComponent();

        }

        private void ClickStartGame(object sender, RoutedEventArgs e)
        {
            window.OutputView.Content = new GameFieldView(window);
        }

        private void ClickExit(object sender, RoutedEventArgs e)
        {
            window.Close();
        }
    }
}
