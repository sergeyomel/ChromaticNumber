using System;
using System.Collections.Generic;
using System.Text;

namespace GraphLib
{
    public static class ExtendList
    {
        public static List<Vertex> Shuffle(this List<Vertex> lVertex)
        {
            /*var rnd = new Random();
            var countVertex = lVert.Count;
            for (int count = 0; count < countVertex; count++)
            {
                var posOne = rnd.Next(0, countVertex);
                var posTwo = rnd.Next(0, countVertex);
                var buffVert = lVert[posOne];
                lVert[posOne] = lVert[posTwo];
                lVert[posTwo] = buffVert;
            }
            return lVert;*/
            var rnd = new Random();
            var buffListVertex = new List<Vertex>(lVertex);
            var shuffleListVertex = new List<Vertex>();
            while (shuffleListVertex.Count != lVertex.Count)
            {
                var position = rnd.Next(0, buffListVertex.Count);
                shuffleListVertex.Add(buffListVertex[position]);
                buffListVertex.RemoveAt(position);
            }
            return shuffleListVertex;
        }
    }
}
