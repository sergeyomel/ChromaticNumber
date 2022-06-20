using System;
using System.Collections.Generic;
using System.Text;

namespace GraphLib.Writer
{
    public interface IWriter
    {
        public void WriteAdjancyMatrix(Graph graph);
        public void WriteEvaluationMatrix(Graph graph);
        public void WriteGraph(Graph graph);
        public void WriteColorGraph(Graph graph, Dictionary<Vertex, int> dColorGraph);
    }
}
