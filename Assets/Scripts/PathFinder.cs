using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//using static PriorityQueue;

public class PathFinder : MonoBehaviour
{
    
    Graph _graph;
    GraphView _graphView;
    Queue<Node> _boudaryNodes;                                                                                                            
    List<Node> _checkedNodes;
    List<Node> _quickestPathNodes;
    Node _startPositionNode;
    Node _endPositionNode;
    
    [Header("Algorithm Display settings")]
    public bool finishedSearching = false;
    public Color quickestPathColor;
    public Color boundaryNodesColor;
    public Color CheckedNodesColor; 
    public Color startPositionColor;
    public Color endPosirionColor;


    public void Initialize(Graph graph, GraphView graphView, Node start, Node end)
    {

        if (start.nodeType == NodeType.Blocked || end.nodeType == NodeType.Blocked)
        {
            Debug.LogWarning("Error: Both start and end position nodes must be open!");
            return;
        }

        _startPositionNode = start;
        _endPositionNode = end;
        _graph = graph;
        _graphView = graphView;

        DisplayNodeColors(graphView, start, end);

        _boudaryNodes = new Queue<Node>();
        _boudaryNodes.Enqueue(start);
       

        for (int x = 0; x < _graph.Width; x++)
        {
            for (int y = 0; y < _graph.Height; y++)
            {
                _graph.nodes[x, y].Reset();
            }
        }
        _checkedNodes = new List<Node>();
        _quickestPathNodes = new List<Node>();
        _startPositionNode.navigatedLength = 0;
        finishedSearching = false;
    }

    List<Node> GetPathNodes(Node endNode)
    {
        List<Node> path = new List<Node>();
        if (endNode == null)
        {
            return path;
        }
        path.Add(endNode);

        Node currentNode = endNode.previous;

        while (currentNode != null)
        {
            path.Insert(0, currentNode);
            currentNode = currentNode.previous;
        }
        return path;
    }



    void ImplmentSearchAlgorithm(Node node)
    {
        if (node != null)
        {
            for (int i = 0; i < node.neighbours.Count; i++)
            {
                if (!_checkedNodes.Contains(node.neighbours[i]))
                {
                    float proximityToNeighbor = _graph.GetNodeDistance(node, node.neighbours[i]);
                    float newMovementInterval = proximityToNeighbor + node.navigatedLength + (int)node.nodeType;

                    if (float.IsPositiveInfinity(node.neighbours[i].navigatedLength) ||
                        newMovementInterval < node.neighbours[i].navigatedLength)
                    {
                        node.neighbours[i].previous = node;
                        node.neighbours[i].navigatedLength = newMovementInterval;
                    }

                    if (!_boudaryNodes.Contains(node.neighbours[i]) && _graph != null)
                    {
                        int distanceToEndPosition = (int)_graph.GetNodeDistance(node.neighbours[i], _endPositionNode);
                        node.neighbours[i].priority = (int)node.neighbours[i].navigatedLength
                            + distanceToEndPosition;
                        _boudaryNodes.Enqueue(node.neighbours[i]);
                    }
                }
            }
        }
    }

    public IEnumerator SearchCoRoutine(float interval = 0.1f)
    {
        float startTime = Time.time;

        yield return null;

        while (!finishedSearching)
        {
            if (_boudaryNodes.Count > 0)
            {
                Node currentNode = _boudaryNodes.Dequeue();

                if (!_checkedNodes.Contains(currentNode))
                {
                    _checkedNodes.Add(currentNode);
                }
                    ImplmentSearchAlgorithm(currentNode);
               
                if (_boudaryNodes.Contains(_endPositionNode))
                {
                    _quickestPathNodes = GetPathNodes(_endPositionNode);
                    finishedSearching = true;

                }
                displayColorChnage(true, 0.5f);

                yield return new WaitForSeconds(interval);
            }
            else
            {
                finishedSearching = true;
            }
        }
    }


    void DisplayNodeColorsV2(bool interpolateToColor = false, float interpolationValue = 0.5f)
    {
        DisplayNodeColors(_graphView, _startPositionNode, _endPositionNode, interpolateToColor, interpolationValue);
    }

    void DisplayNodeColors(GraphView graphView, Node start, Node end, bool interpolateToColor = false, float interpolationValue = 0.5f)
    {
        if (_boudaryNodes != null)
        {
            graphView.ColorNodes(_boudaryNodes.ToList(), boundaryNodesColor, interpolateToColor, interpolationValue);
        }

        if (_checkedNodes != null)
        {
            graphView.ColorNodes(_checkedNodes, CheckedNodesColor, interpolateToColor, interpolationValue);
        }

        if (_quickestPathNodes != null && _quickestPathNodes.Count > 0)
        {
            graphView.ColorNodes(_quickestPathNodes, quickestPathColor, interpolateToColor, interpolationValue * 2f);
        }

        NodeView startPositionNodeView = graphView.nodeViews[start.xIndex, start.yIndex];

        if (startPositionNodeView != null)
        {
            startPositionNodeView.ColorThisNode(startPositionColor);
        }

        NodeView endPositionNodeView = graphView.nodeViews[end.xIndex, end.yIndex];

        endPositionNodeView.ColorThisNode(endPosirionColor);
    }

    private void displayColorChnage(bool lerpColor = false, float lerpValue = 0.5f)
    {
            DisplayNodeColorsV2(lerpColor, lerpValue);
    }

 

}