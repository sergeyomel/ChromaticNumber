using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Shapes;
using GraphLib;

namespace Graphics.Connectors
{
    public class GraphicsVertexConnector
    {
        public GraphicsVertex startVertex;
        public GraphicsVertex endVertex;
        public VertexConnector vertConnector;

        protected double proportionTb = 3.0 / 7.0;
        protected double proportionArrow = 1.0 / 2.0;

        public GraphicsVertexConnector(VertexConnector vConnector, GraphicsVertex startV, GraphicsVertex endV)
        {
            vertConnector = vConnector;
            startVertex = startV;
            endVertex = endV;
        }

        public static GraphicsVertexConnector ConvertConnector(GraphicsVertexConnector connector) 
        {
            if (connector.GetType() == typeof(GraphicsArc))
                return new GraphicsEdge(connector.vertConnector, connector.startVertex, connector.endVertex);
            if (connector.GetType() == typeof(GraphicsEdge))
                return new GraphicsArc(connector.vertConnector, connector.startVertex, connector.endVertex);
            return null;
        }

        public virtual List<Line> Draw() { return new List<Line>() { new Line() }; }
    }
}
