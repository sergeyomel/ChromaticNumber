using System.Collections.Generic;
using System.Text;

namespace GraphLib.Writer
{
    public class WriterFromFile : IWriter
    {
        private string path;

        public WriterFromFile(string path)
        {
            this.path = path;
        }

        public void SetPathWriter(string path)
        {
            this.path = path;
        }

        public void WriteAdjancyMatrix(Graph graph)
        {
            using(var stream = new System.IO.StreamWriter(path))
            {
                var strGraph = PrinterGraph.AdjancyMatrix(graph, ConvertGraph.GetAdjancyMatrix(graph));
                stream.Write(strGraph);
            }
        }

        public void WriteEvaluationMatrix(Graph graph)
        {
            using (var stream = new System.IO.StreamWriter(path))
            {
                var strGraph = PrinterGraph.EvaluationMatrix(graph);
                stream.Write(strGraph);
            }
        }

        public void WriteGraph(Graph graph)
        {
            using (var stream = new System.IO.StreamWriter(path))
            {
                stream.Write(graph.ToString());
            }
        }

        public void WriteColorGraph(Graph graph, Dictionary<Vertex,int> dColorGraph)
        {
            var sb = new StringBuilder("");
            var copyGraph = graph.GetGraph();
            var lColor = new List<int>();
            bool flag = false;
            foreach (var key in copyGraph.Keys)
            {
                sb.Append(key + $"({dColorGraph[key]})|");
                foreach (var vc in copyGraph[key])
                {
                    sb.Append(vc.GetEndV().ToString() + $"({dColorGraph[vc.GetEndV()]}) ");
                    lColor.Add(dColorGraph[vc.GetEndV()]);
                }
                if (lColor.Contains(dColorGraph[key]))
                    flag = true;
                lColor = new List<int>();
                sb.Append("\n");
            }
            if (!flag)
                sb.Append("Ошибок нет");
            else
                sb.Append("Ошибка раскраски");
            using (var stream = new System.IO.StreamWriter(path))
            {
                stream.Write(sb.ToString());
            }
        }
    }
}
