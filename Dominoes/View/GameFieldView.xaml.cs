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
    /// Логика взаимодействия для GameFieldView.xaml
    /// </summary>
    public partial class GameFieldView : UserControl
    {
        private readonly MainWindow window;

        public GameFieldView(MainWindow window)
        {
            this.window = window;
            InitializeComponent();

            var g = new GameFieldUI(this);
        }

        private Point? point;
        
        private void CanvasMoveMouseDown(object sender, MouseButtonEventArgs e)
        {
            point = e.GetPosition(Table);
            Table.CaptureMouse();
        }

        private void CanvasMoveMouseMove(object sender, MouseEventArgs e)
        {
            if (point == null)
                return;
            var p = e.GetPosition(ParentTable) - (Vector)point.Value;
            Canvas.SetLeft(Table, p.X);
            Canvas.SetTop(Table, p.Y);
        }

        private void CanvasMoveMouseUp(object sender, MouseButtonEventArgs e)
        {
            point = null;
            Table.ReleaseMouseCapture();
        }

        public void ClickBack(object sender, RoutedEventArgs e)
        {
            window.OutputView.Content = new MenuView(window);
        }
    }
}
