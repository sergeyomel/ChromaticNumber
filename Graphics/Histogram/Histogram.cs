using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graphics.Histogram
{
    public class Histogram
    {
        private int widthCanvasHistogram = 600, heightCanvasHistogram = 400;
        private int widthCanvasLegend = 150, heightCanvasLegend = 400;

        private Window window;
        private Canvas mainCanvas;
        private Canvas canvasHistogram;
        private Border borderCanvasHistogram;
        private Canvas canvasLegend;
        private Border borderCanvasLegend;

        private List<Color> lColor = new List<Color> { 
            Color.FromRgb(255, 0, 0), 
            Color.FromRgb(127, 255, 0), 
            Color.FromRgb(0, 0, 255), 
            Color.FromRgb(128, 0, 128), 
            Color.FromRgb(255, 215, 0) 
        };

        private int indentAxis = 30;
        private int indentOrdinat = 25;
        private int countVerticalRisk = 10;
        private int countHorizontalRisk = 20;

        private double lengthAxis;
        private double lengthOrdinat;

        private double minAxisValue, maxAxisValue;
        private double minOrdinatValue, maxOrdinatValue;

        private bool IsHaveGroupName = true;             //Подписана ли группа
        private bool IsHaveSignatureHistogram = true;   //Подписана ли ось Х
        private bool IsHaveSignatureOrdinat = true;     //Подписана ли ось Y
        public bool IsDrawColumnHeight = true;           //Отрисовывать ли значение колонки
        public bool IsLowerBound = false;                //Обрезается ли гистограмма под наименьшее значение

        private List<string> lKey = null;
        private List<string> lGroupName = null;
        private string SignatureHistogram = "";
        private string SignatureOrdinat = "";

        private (double x, double y) startPoint;

        private List<Dictionary<double, List<double>>> lData;

        public Window GetWindow() => window;

        public Histogram() 
        {
            lData = new List<Dictionary<double, List<double>>>();
            lKey = new List<string> { "колонка 1",
                                      "колонка 2",
                                      "колонка 3",
                                      "колонка 4",
                                      "колонка 5"};

            countHorizontalRisk = lData.Count;
            
            canvasHistogram = new Canvas();
            canvasHistogram.Height = heightCanvasHistogram;
            canvasHistogram.Width = widthCanvasHistogram;
            canvasHistogram.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            borderCanvasHistogram = new Border();
            borderCanvasHistogram.Width = widthCanvasHistogram+6;
            borderCanvasHistogram.Height = heightCanvasHistogram + 6;
            borderCanvasHistogram.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            borderCanvasHistogram.BorderThickness = new Thickness(3);
            borderCanvasHistogram.CornerRadius = new CornerRadius(3);
            borderCanvasHistogram.Child = canvasHistogram;

            canvasLegend = new Canvas();
            canvasLegend.Width = widthCanvasLegend;
            canvasLegend.Height = heightCanvasLegend;
            canvasLegend.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            borderCanvasLegend = new Border();
            borderCanvasLegend.Width = widthCanvasLegend + 6;
            borderCanvasLegend.Height = heightCanvasLegend + 6;
            borderCanvasLegend.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            borderCanvasLegend.BorderThickness = new Thickness(3);
            borderCanvasLegend.CornerRadius = new CornerRadius(3);
            borderCanvasLegend.Child = canvasLegend;

            mainCanvas = new Canvas();
            mainCanvas.Width = borderCanvasHistogram.Width + borderCanvasLegend.Width + 20;
            mainCanvas.Height = borderCanvasHistogram.Height;

            Canvas.SetLeft(borderCanvasHistogram, 0);
            Canvas.SetTop(borderCanvasHistogram, 0);
            Canvas.SetLeft(borderCanvasLegend, borderCanvasHistogram.Width + 20);
            Canvas.SetTop(borderCanvasLegend, 0);
            mainCanvas.Children.Add(borderCanvasHistogram);
            mainCanvas.Children.Add(borderCanvasLegend);

            window = new Window();
            window.Title = "Гистограмма";
            window.Height = borderCanvasHistogram.Height + 80;
            window.Width = borderCanvasHistogram.Width + borderCanvasLegend.Width + 60;
            window.Background = new SolidColorBrush(Color.FromRgb(200, 200, 200));
        }

        public void SetTitleHistogram(string title) => window.Title = title;

        #region Setter\Remover Signature
        public void SetGroupName(List<string> name)
        {
            lGroupName = name;
            IsHaveGroupName = true;
        }
        public void RemoveGroupName()
        {
            lGroupName = null;
            IsHaveGroupName = false;
        }
        public void SetSignatureHistogram(string sign)
        {
            SignatureHistogram = sign;
            IsHaveSignatureHistogram = true;
        }
        public void RemoveSignatureHistogram()
        {
            SignatureHistogram = null;
            IsHaveSignatureHistogram = false;
        }
        public void SetSignatureOrdinat(string sign)
        {
            SignatureOrdinat = sign;
            IsHaveSignatureOrdinat = true;
        }
        public void RemoveSignatureOrdinat()
        {
            SignatureOrdinat = null;
            IsHaveSignatureOrdinat = false;
        }
        public void SetColumnName(List<string> lName)
        {
            if (lName.Count > 5)
                lName = lName.Take(5).ToList();
            lKey = lName;
        }
        public void ResetColumnName()
        {
            lKey = new List<string> { "колонка 1",
                                      "колонка 2",
                                      "колонка 3",
                                      "колонка 4",
                                      "колонка 5"};
        }
        #endregion

        /// <summary>
        /// Функция добавления группы колонок.
        /// Пример: месяц года.
        /// </summary>
        /// <param name="data"></param>
        public void AddGroup(Dictionary<double, List<double>> data)
        {
            if (data[data.ElementAt(0).Key].Count > 5)
                data[data.ElementAt(0).Key] = data[data.ElementAt(0).Key].Take(5).ToList();
            lData.Add(data);
            countHorizontalRisk += 1;
        }

        /// <summary>
        /// Поиск нижней левой точки гистограмы.
        /// </summary>
        private void FindStartPoint()
        {
            var posX = indentAxis;
            var posY = heightCanvasHistogram - indentOrdinat;

            if (IsHaveGroupName)
                posY -= 15;
            if (IsHaveSignatureHistogram)
                posY -= 25;
            if (IsHaveSignatureOrdinat)
                posX += 35;

            startPoint = (posX, posY);
        } 

        private void FindRangeAxis()
        {
            lengthAxis = widthCanvasHistogram - startPoint.x - indentAxis;

            var lValueAxis = new List<double>();
            if(lData.Count == 0)
            {
                maxAxisValue = 200;
                minAxisValue = 0;
                return;
            }
            foreach (var listTuple in lData)
                foreach (var item in listTuple)
                    lValueAxis.Add(item.Key);
            maxAxisValue = lValueAxis.Max();
            minAxisValue = lValueAxis.Min();
        }
        private void FindRangeOrdinat()
        {
            lengthOrdinat = startPoint.y - indentOrdinat;

            var lValueOrdinat = new List<double>();
            if (lData.Count == 0)
            {
                maxOrdinatValue = 200;
                minOrdinatValue = 0;
                return;
            }
            foreach (var dict in lData)
                foreach(var key in dict.Keys.ToList())
                    lValueOrdinat.AddRange(dict[key]);
            maxOrdinatValue = lValueOrdinat.Max();
            minOrdinatValue = 0;
            if (IsLowerBound)
                minOrdinatValue = lValueOrdinat.Min();
        }

        private void DrawOrdinateLine()
        {
            var ordinatLine = new Line();
            ordinatLine.X1 = startPoint.x;
            ordinatLine.X2 = startPoint.x;
            ordinatLine.Y1 = startPoint.y;
            ordinatLine.Y2 = indentOrdinat; // startPoint.y - heightCanvas + indentOrdinat*2;
            ordinatLine.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            ordinatLine.StrokeThickness = 2;
            Canvas.SetZIndex(ordinatLine, -1);
            canvasHistogram.Children.Add(ordinatLine);

            var verticalIndent = (startPoint.y - indentOrdinat)*1.0 / countVerticalRisk;
            var buffPosY = startPoint.y;
            var indentOrdinatRange = (maxOrdinatValue - minOrdinatValue) * 1.0 / countVerticalRisk;
            var buffOrdinatValue = minOrdinatValue;
            for(int index = 0; index < countVerticalRisk; index++)
            {
                var vertRisk = new Line();
                vertRisk.X1 = startPoint.x;
                vertRisk.X2 = startPoint.x-5;
                vertRisk.Y1 = buffPosY;
                vertRisk.Y2 = buffPosY;
                vertRisk.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                vertRisk.StrokeThickness = 2;
                Canvas.SetZIndex(vertRisk, -1);
                canvasHistogram.Children.Add(vertRisk);

                var tb = new TextBlock();
                tb.Text = ((int)buffOrdinatValue).ToString();
                tb.FontSize = 11;
                tb.FontWeight = FontWeights.Bold;
                tb.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                Canvas.SetLeft(tb, startPoint.x - 18 - (Math.Round(buffOrdinatValue, 1).ToString().Length * 3));
                Canvas.SetTop(tb, indentOrdinat + buffPosY - 30);
                Canvas.SetZIndex(tb, 0);
                canvasHistogram.Children.Add(tb);

                buffPosY -= verticalIndent;
                buffOrdinatValue += indentOrdinatRange;
            }
            var maxVertRisk = new Line();
            maxVertRisk.X1 = startPoint.x;
            maxVertRisk.X2 = startPoint.x - 5;
            maxVertRisk.Y1 = buffPosY;
            maxVertRisk.Y2 = buffPosY;
            maxVertRisk.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            maxVertRisk.StrokeThickness = 2;
            Canvas.SetZIndex(maxVertRisk, -1);
            canvasHistogram.Children.Add(maxVertRisk);

            var tbMaxRisk = new TextBlock();
            tbMaxRisk.Text = Math.Round(maxOrdinatValue, 1).ToString();
            tbMaxRisk.FontSize = 11;
            tbMaxRisk.FontWeight = FontWeights.Bold;
            tbMaxRisk.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            Canvas.SetLeft(tbMaxRisk, startPoint.x - 18 - (Math.Round(buffOrdinatValue, 1).ToString().Length * 3));
            Canvas.SetTop(tbMaxRisk, indentOrdinat + buffPosY - 30);
            Canvas.SetZIndex(tbMaxRisk, 0);
            canvasHistogram.Children.Add(tbMaxRisk);
        }
        private void DrawAxisLine()
        {
            var axisLine = new Line();
            axisLine.X1 = startPoint.x;
            axisLine.X2 = widthCanvasHistogram - indentAxis;
            axisLine.Y1 = startPoint.y;
            axisLine.Y2 = startPoint.y;
            axisLine.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            axisLine.StrokeThickness = 2;
            Canvas.SetZIndex(axisLine, -1);
            canvasHistogram.Children.Add(axisLine);

            /*var horizontalIndent = (widthCanvas - startPoint.x - indentAxis) * 1.0 / countHorizontalRisk;
            var buffPosX = startPoint.x;
            var indentAxisRange = (maxAxisValue - minAxisValue) * 1.0 / countHorizontalRisk;
            var buffAxisValue = minAxisValue;
            for (int index = 0; index < countHorizontalRisk + 1; index++)
            {
                var vertRisk = new Line();
                vertRisk.X1 = buffPosX;
                vertRisk.X2 = buffPosX;
                vertRisk.Y1 = startPoint.y;
                vertRisk.Y2 = startPoint.y+5;
                vertRisk.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                vertRisk.StrokeThickness = 2;
                Canvas.SetZIndex(vertRisk, -1);
                canvas.Children.Add(vertRisk);

                var tb = new TextBlock();
                tb.Text = ((int)buffAxisValue).ToString();
                tb.FontSize = 11;
                tb.FontWeight = FontWeights.Bold;
                tb.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                Canvas.SetLeft(tb, buffPosX - ((int)buffAxisValue).ToString().Length*3);
                Canvas.SetTop(tb, startPoint.y + 10);
                Canvas.SetZIndex(tb, 0);
                canvas.Children.Add(tb);

                buffPosX += horizontalIndent;
                buffAxisValue += indentAxisRange;
            }*/

            var lengthGroup = lengthAxis / lData.Count;
            var buffPosX = startPoint.x + lengthGroup/2;
            foreach(var item in lData)
            {
                var vertRisk = new Line();
                vertRisk.X1 = buffPosX;
                vertRisk.X2 = buffPosX;
                vertRisk.Y1 = startPoint.y;
                vertRisk.Y2 = startPoint.y + 5;
                vertRisk.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                vertRisk.StrokeThickness = 2;
                Canvas.SetZIndex(vertRisk, -1);
                canvasHistogram.Children.Add(vertRisk);

                var tb = new TextBlock();
                tb.Text = item.ElementAt(0).Key.ToString();
                tb.FontSize = 11;
                tb.FontWeight = FontWeights.Bold;
                tb.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                Canvas.SetLeft(tb, buffPosX - item.ElementAt(0).Key.ToString().Length * 3);
                Canvas.SetTop(tb, startPoint.y + 10);
                Canvas.SetZIndex(tb, 0);
                canvasHistogram.Children.Add(tb);

                buffPosX += lengthGroup;
            }
        }

        private void DrawSignatureHistogram()
        {
            if (!IsHaveSignatureHistogram)
                return;
            var tb = new TextBlock();
            tb.FontSize = 18;
            tb.FontWeight = FontWeights.Bold;
            tb.Text = SignatureHistogram;
            Canvas.SetLeft(tb, widthCanvasHistogram / 2 - SignatureHistogram.Length * 10 / 2);
            if (IsHaveGroupName)
                Canvas.SetTop(tb, startPoint.y + 45);
            else
                Canvas.SetTop(tb, startPoint.y + 25);
            canvasHistogram.Children.Add(tb);
        }
        private void DrawSignatureOrdinat()
        {
            if (!IsHaveSignatureOrdinat)
                return;
            var tb = new TextBlock();
            tb.Text = SignatureOrdinat;
            tb.FontSize = 18;
            tb.FontWeight = FontWeights.Bold;
            tb.RenderTransform = new RotateTransform(-90);
            Canvas.SetLeft(tb, startPoint.x - 60);
            Canvas.SetTop(tb, heightCanvasHistogram / 2 + SignatureOrdinat.Length * 7 / 2);
            canvasHistogram.Children.Add(tb);
        }
        private void DrawGroupName()
        {
            if (lGroupName == null)
                return;
            var startPosX = startPoint.x+10;
            var startPosY = startPoint.y + 25;
            var axisIncrement = lengthAxis / lGroupName.Count;
            foreach (var name in lGroupName)
            {
                var tb = new TextBlock();
                tb.Text = name;
                tb.FontSize = 14;
                tb.FontWeight = FontWeights.Normal;
                Canvas.SetLeft(tb, startPosX + axisIncrement/2 - name.Length*10/2);
                Canvas.SetTop(tb, startPosY);
                canvasHistogram.Children.Add(tb);

                startPosX += axisIncrement;
            }
        }

        private void DrawLegendHistogram()
        {
            var indentX = 5;
            var sizeCircle = 10;
            var buffPosY = 5;
            for (int index = 0; index < lData[0][lData[0].ElementAt(0).Key].Count; index++)
            {

                var buffText = lKey[index];
                var text = "";
                int buffPos = 0;
                while(buffPos < buffText.Length)
                {
                    if (buffPos % 15 == 0 && buffPos != 0)
                    {
                        text += "\n";
                        buffPosY += 8;
                    }
                    text += buffText[buffPos];
                    buffPos += 1;    
                }
                if(index != 0)
                    buffPosY += 8;

                var circle = new Ellipse();
                circle.Fill = new SolidColorBrush(lColor[index]);
                circle.Width = sizeCircle;
                circle.Height = sizeCircle;
                Canvas.SetLeft(circle, indentX);
                Canvas.SetTop(circle, buffPosY+7);
                Canvas.SetZIndex(circle, 1);

                var tb = new TextBlock();
                tb.Text = text; //lKey[index].Count() <= 15 ? lKey[index] : lKey[index].Substring(0, 15);
                tb.FontSize = 14;
                tb.FontWeight = FontWeights.Normal;
                Canvas.SetLeft(tb, indentX*2+sizeCircle + 5);
                Canvas.SetTop(tb, buffPosY);
                Canvas.SetZIndex(tb, 1);

                canvasLegend.Children.Add(circle);
                canvasLegend.Children.Add(tb);

                buffPosY += sizeCircle + 20; //<-- расстояние между строчками
            }
        }

        private double ConvertHeight(double height)
        {
            if (IsLowerBound)
                height = height - minOrdinatValue + 1;
            if (height < 0)
                height = 0;
            var procent = height / (maxOrdinatValue - minOrdinatValue);
            var scaleHeight = procent * lengthOrdinat;
            return scaleHeight;
        }
        private void DrawColumn()
        {
            //Отступ слева-справа в группе
            var indentAxisInGroup = 10;
            //Отступ справа от колонки
            var indentRightColumn = 5;
            //Длина места для отрисовки колонок
            var incrementLengthGroup = lengthAxis / lData.Count; 
            var lengthDrawGroup = incrementLengthGroup - 2 * indentAxisInGroup;
            //Ширина колонки
            var countColumn = lData[0][lData[0].ElementAt(0).Key].Count;
            var widthColumn = (lengthDrawGroup - indentRightColumn * (countColumn - 1)) / countColumn; // <--

            var buffStartX = startPoint.x;
            var buffPosXInGroup = buffStartX + indentAxisInGroup;

            foreach(var dict in lData)
            {
                var lValue = dict[dict.ElementAt(0).Key];
                for (int index = 0; index < lValue.Count; index++)
                {
                    var result = Column.GetColumn(buffPosXInGroup, startPoint.y - 2, widthColumn, ConvertHeight(lValue[index]), Math.Round(lValue[index],2), lColor[index]);
                    canvasHistogram.Children.Add(result.column);
                    if (IsDrawColumnHeight)
                        canvasHistogram.Children.Add(result.tb);
                    buffPosXInGroup += widthColumn + indentRightColumn;
                }
                buffStartX += incrementLengthGroup;
                buffPosXInGroup = buffStartX + indentAxisInGroup;
            }
        }

        public void DrawHistogram() 
        {
            FindStartPoint();
            FindRangeAxis();
            FindRangeOrdinat();

            DrawAxisLine();
            DrawOrdinateLine();

            DrawSignatureOrdinat();
            DrawSignatureHistogram();
            DrawGroupName();

            DrawColumn();

            DrawLegendHistogram();

            window.Content = mainCanvas;
            window.Show();
        }

    }
}
