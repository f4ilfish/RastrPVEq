﻿using System;
using System.Collections.Generic;
using System.Linq;
using RastrPVEqConsole.Models.Topology;

namespace RastrPVEqConsole.Infrastructure.Topology
{
    public static class DepthFirstTraversal<T>
    {
        /// <summary>
        /// Recursive method. Prints the order of visited vertices to console
        /// </summary>
        public static void DepthFirstSearchIterative(IGraph<T> graph, IVertex<T> startVertex)
        {
            if (!graph.ContainsVertex(startVertex))
            {
                throw new InvalidOperationException("Vertex does not belong to graph.");
            }

            if (startVertex.IsVisited)
            {
                return;
            }

            startVertex.Visit();

            Console.WriteLine(startVertex);

            var unvisitedVertices = startVertex.AdjacentUnvisitedVertices();

            foreach (var vertex in unvisitedVertices)
            {
                DepthFirstSearchIterative(graph, vertex);
            }
        }

        /// <summary>
        /// Returns the list of vertices, which are path from start vertex to end vertex
        /// </summary>
        public static IEnumerable<IVertex<T>> DepthFirstIterative(IGraph<T> graph, IVertex<T> startVertex,
            IVertex<T> searchVertex)
        {
            if (!graph.ContainsVertex(startVertex) || !graph.ContainsVertex(searchVertex))
            {
                throw new InvalidOperationException("One or more vertices are not belong to graph.");
            }

            var stack = new Stack<IVertex<T>>();

            stack.Push(startVertex);

            while (stack.Any())
            {
                var vertex = stack.Pop();

                vertex.Visit();

                //yield return vertex;

                if (vertex.Equals(searchVertex))
                {
                    yield return vertex;
                    yield break;
                }

                var unvisitedVertices = vertex.AdjacentUnvisitedVertices();

                if (unvisitedVertices.Count != 0)
                {
                    yield return vertex;
                }

                foreach (var v in unvisitedVertices)
                {
                    stack.Push(v);
                }
            }
        }
    }
}
