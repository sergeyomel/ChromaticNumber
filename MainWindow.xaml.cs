using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GraphLib;
using Genetic;
using Genetic.Generations.GenerationInterface;
using Genetic.Crossover;
using System.Diagnostics;
using GraphLib.Writer;
using Genetic.Generations;
using Genetic.Selection;
using Genetic.Mutation;
using Genetic.PowerFunction;
using Genetic.JSON.HistogramConstructionModel;
using Genetic.JSON.SetBuiltDependencies;
using System;
using System.Collections.Generic;
using Genetic.JSON;

namespace Graphics
{
    public partial class MainWindow : Window
    {
        private GraphicsGraph graphicsGraph;

        private Genetic.Interface.ICrossing crossover;
        private Genetic.Interface.IMutation mutation;
        private Genetic.Interface.ISelect selector;
        private Genetic.Interface.IPowerFunction powerFunction;

        private bool IsCheckVert = false;
        private GraphicsVertex bufferGrVert = null;
        private int StrokeTh = 2;

        public MainWindow()
        {
            InitializeComponent();
            Title = "РаскрасьМеняПолностью";

            crossover = new SinglePointCrossing();
            mutation = new DoublePointMutation();
            selector = new MinimumTargetFunctionSelection();
            powerFunction = new ColoringNonAdjacentVertices();

            

            graphicsGraph = new GraphicsGraph(CanvasPanel);
            Logger.InitializeLogger();
        }

        private void GenerateColoringGraph(Graph graph, Population population, Individual individ, int count = 0, string title = "example")
        {
            var window = new Window();
            window.Title = title;
            window.Height = 850;
            window.Width = 1280;
            window.Background = new SolidColorBrush(Color.FromRgb(220, 220, 220));
            var canvas = new Canvas();
            canvas.Height = 850;
            canvas.Width = 1280;
            canvas.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            window.Content = canvas;

            var converter = new ConverterGraphToGraphicsGraph(canvas);
            var graphicsGraph2 = converter.ConvertGraph(graph);
            graphicsGraph2.ColoringGraph(individ);

            var tbC = new TextBlock();
            tbC.Text = "Colors: "+count.ToString();
            tbC.FontSize = 18;
            tbC.FontWeight = FontWeights.Bold;
            Canvas.SetLeft(tbC, canvas.Width - count.ToString().Length * 20 - 80);
            Canvas.SetTop(tbC, 25);
            canvas.Children.Add(tbC);

            window.Show();

            SaverToPicture.Save(window, @"C:\Users\myacc\Desktop\pictureGraph");
            //SaverInfoAboutPopulation.Save(population);
            
        }

        private void ClickLeftButton(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(CanvasPanel);
            var graphVert = graphicsGraph.ContainVertex(p.X, p.Y);

            if (graphVert == null)
            {
                if (IsCheckVert)
                {
                    bufferGrVert.SetStrokeTh(StrokeTh);
                    graphicsGraph.Draw();
                    IsCheckVert = false;
                    bufferGrVert = null;
                }
                else
                    graphicsGraph.AddGraphVertex(p.X, p.Y);
            }
            else
            {
                if (IsCheckVert)
                {
                    bufferGrVert.SetStrokeTh(StrokeTh);
                    graphicsGraph.AddVertConnector(bufferGrVert, graphVert);
                    IsCheckVert = false;
                    bufferGrVert = null;
                }
                else
                {
                    IsCheckVert = true;
                    bufferGrVert = graphVert;
                    StrokeTh = bufferGrVert.GetStrokeTh();
                    bufferGrVert.SetStrokeTh(4);
                    graphicsGraph.Draw();
                }
            }
        }

        private void ClickRightButton(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(this);
            graphicsGraph.RemoveGraphVertex(p.X, p.Y);
        }

        private void ColoringCurrentGraph(object sender, RoutedEventArgs e)
        {
            if (!GeneratorGraph.IsConnectedGraph(graphicsGraph.graph))
            {
                MessageBox.Show("Граф не является полносвязным.");
                return;
            }
            var population = new Population(100, 100, 100, 0, 100, graphicsGraph.graph, new FillingRandomIndividuals(), crossover, mutation, selector, powerFunction);
            population.FindBestIndivid();
            graphicsGraph.ColoringGraph(population.GetBestIndividInPopulation());
        }
        private void ColoringFileGraph(object sender, RoutedEventArgs e)
        {
            /*var reader = new ReaderFromFile();
            var strAdjMatrix = reader.Read(@"C:\Users\myacc\OneDrive\Рабочий стол\ChromaGraph\ExampleGraphThreeColor3.txt");
            var graph = ConverterStringAdjMatrix.GetGraph(strAdjMatrix);*/

            var reader = new GraphLib.Reader.ReaderFromFile();
            var strmatrix = reader.Read(@"C:\Users\myacc\OneDrive\Рабочий стол\matrix (2).txt");
            var graph = ConverterStringAdjMatrix.GetGraph(strmatrix);

            var population = new Population(100, 100, 100, 0, 100, graphicsGraph.graph, new FillingRandomIndividuals(), crossover, mutation, selector, powerFunction);
            population.FindBestIndivid();
            
            GenerateColoringGraph(graph, population, population.GetBestIndividInPopulation(), population.GetBestIndividInPopulation().TargetFunction());
        }
        private void ColoringRandomGraph(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                var randomGraph = GeneratorGraph.Generate(50, 150);

                var population = new Population(
                    100,
                    50,
                    1000,
                    5,
                    100,
                    randomGraph,
                    new FillingRandomIndividuals(),
                    new AccordingToEvalutionMatrixCrossover(),
                    new DoublePointMutation(),
                    new MinimumTargetFunctionSelection(),
                    new NonDecreasingDegrees());
                population.FindBestIndivid();
                JSONWriter.Write(population);

                population = new Population(
                    100,
                    50,
                    1000,
                    750,
                    100,
                    randomGraph,
                    new FillingEliteIndividuals(),
                    new AccordingToEvalutionMatrixCrossover(),
                    new DoublePointMutation(),
                    new MinimumTargetFunctionSelection(),
                    new VozikaAlgorithm());
                population.FindBestIndivid();
                JSONWriter.Write(population);

                //GenerateColoringGraph(randomGraph, population, population.GetBestIndividInPopulation(), population.GetBestIndividInPopulation().TargetFunction(), "RoadCrossover");
                //var writer = new WriterFromFile(@"C:\Users\myacc\Desktop\graphColor.txt");
                //writer.WriteColorGraph(randomGraph, new Genetic.PowerFunction.NonDecreasingDegrees().IndividualPowerFunction(population.GetBestIndividInPopulation()));
            }
        }

        private void ResetColor(object sender, RoutedEventArgs e)
        {
            graphicsGraph.ReserGraphicsVertColor();
        }

        private void ChangeType(object sender, RoutedEventArgs e)
        {
            graphicsGraph.ChangeTypeGraphicsGraph();
        }

        private void DrawHistogram(object sender, RoutedEventArgs e)
        {
            #region Histogram

            DrawHistogram9(null, null);
            //DrawHistogram10(null, null);

            /*Graphics.SaverToPicture.Save(histogram.GetWindow(), @"C:\Users\myacc\OneDrive\Рабочий стол\GraphsPicture");*/
            #endregion
        }

        private void DrawHistogram9(object sender, RoutedEventArgs e)
        {
            #region Histogram
            var requestModel = new RequestModel(DependentElement.ChromaticNumber,
                                                RequestParameter.FillingType,
                                                ParameterGroupColumn.CountIndividInGeneration);
            var signature = new ParameterToStringConverter().GetStringParameter(requestModel);
            var histogram = new Histogram.Histogram();
            var pattern = new Pattern();

            List<ModelJSON> data;
            try
            {
                data = JSONReader.Read();

                foreach (var item in pattern.GetData(data, requestModel))
                    histogram.AddGroup(item);
            }
            catch (Exception ex)
            {
                Logger.PushMessage(ex);
                MessageBox.Show(ex.Message);
                return;
            };

            var lColumnName = pattern.signatureColumn;
            histogram.SetColumnName(new PropertyToStringConverter().GetStringParameter(lColumnName));

            histogram.SetTitleHistogram($"Зависимость {signature[0]} от {signature[1]}");
            histogram.SetSignatureOrdinat(signature[0]);
            histogram.SetSignatureHistogram(signature[2]);

            histogram.DrawHistogram();

            /*Graphics.SaverToPicture.Save(histogram.GetWindow(), @"C:\Users\myacc\OneDrive\Рабочий стол\GraphsPicture");*/
            #endregion
        }

        private void DrawHistogram10(object sender, RoutedEventArgs e)
        {
            #region Histogram
            var requestModel = new RequestModel(DependentElement.ChromaticNumber,
                                                RequestParameter.ProbabilityMutation,
                                                ParameterGroupColumn.CountIndividInGeneration);
            var signature = new ParameterToStringConverter().GetStringParameter(requestModel);
            var histogram = new Histogram.Histogram();
            var pattern = new Pattern();

            List<ModelJSON> data;
            try
            {
                data = JSONReader.Read();

                foreach (var item in pattern.GetData(data, requestModel))
                    histogram.AddGroup(item);
            }
            catch (Exception ex)
            {
                Logger.PushMessage(ex);
                MessageBox.Show(ex.Message);
                return;
            };

            var lColumnName = pattern.signatureColumn;
            histogram.SetColumnName(lColumnName);

            histogram.SetTitleHistogram($"Зависимость {signature[0]} от {signature[1]}");
            histogram.SetSignatureOrdinat(signature[0]);
            histogram.SetSignatureHistogram(signature[2]);

            histogram.DrawHistogram();

            /*Graphics.SaverToPicture.Save(histogram.GetWindow(), @"C:\Users\myacc\OneDrive\Рабочий стол\GraphsPicture");*/
            #endregion
        }

        private void DrawCoordinateLine()
        {
            #region CoordinateLine
            var widthRegion = 600;
            var heightRegion = 600;

            var lineX = new System.Windows.Shapes.Line();
            lineX.X1 = 0;
            lineX.X2 = widthRegion;
            lineX.Y1 = heightRegion / 2;
            lineX.Y2 = heightRegion / 2;
            lineX.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            lineX.StrokeThickness = 2;
            Canvas.SetZIndex(lineX, 1);
            CanvasPanel.Children.Add(lineX);

            var lineY = new System.Windows.Shapes.Line();
            lineY.X1 = widthRegion / 2;
            lineY.X2 = widthRegion / 2;
            lineY.Y1 = 0;
            lineY.Y2 = heightRegion;
            lineY.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            lineY.StrokeThickness = 2;
            Canvas.SetZIndex(lineY, 1);
            CanvasPanel.Children.Add(lineY);
            #endregion
        }
    }
}
