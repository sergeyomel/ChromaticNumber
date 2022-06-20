using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graphics.Histogram
{
    public static class Column
    {
        public static (Rectangle column, TextBlock tb) GetColumn(double x, double y, double width, double height, double value, Color color)
        {
            var column = new Rectangle();
            column.Width = width;
            column.Height = height;
            column.Fill = new SolidColorBrush(color);
            Canvas.SetLeft(column, x);
            Canvas.SetTop(column, y-height);
            Canvas.SetZIndex(column, 2);

            var tb = new TextBlock();
            tb.Text = value.ToString();
            tb.FontSize = 11;
            Canvas.SetLeft(tb, x + width / 2 - value.ToString().Length * 8 / 2);
            Canvas.SetTop(tb, y - height - 15);
            Canvas.SetZIndex(tb, 2);

            return (column, tb);
        }
    }

}
