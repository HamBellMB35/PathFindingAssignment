using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{

    int _height;
    int _width;
    int[,] _mapData;
    public int Width { get { return _width; } }                               // _width getter
    public int Height { get { return _height; } }                            // _height getter
    public Node[,] nodes;
    public List<Node> walls = new List<Node>();

    public static readonly Vector2[] possibleDirections =
    {
        new Vector2(0f,1f),new Vector2(1f,1f),new Vector2(1f,0f),
        new Vector2(1f,-1f),new Vector2(0f,-1f),new Vector2(-1f,-1f),
        new Vector2(-1f,0f),new Vector2(-1f,1f)
    };

    public void Initialize(int[,] mapData)
    {
        _mapData = mapData;
        _width = mapData.GetLength(0);
        _height = mapData.GetLength(1);

        nodes = new Node[_width, _height];

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                NodeType type = (NodeType)mapData[x, y];
                Node newNode = new Node(x, y, type);
                nodes[x, y] = newNode;

                newNode.position = new Vector3(x, 0, y);

                if (type == NodeType.Blocked)
                {
                    walls.Add((newNode));
                }
            }
        }

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (nodes[x, y].nodeType != NodeType.Blocked)
                {
                    nodes[x, y].neighbours = GetNeighbours(x, y);
                }
            }
        }
    }

    public bool IsInsideMap(int x, int y)
    {
        return (x >= 0 && x < _width && y >= 0 && y < _height);
    }

    List<Node> GetNeighbours(int x, int y, Node[,] nodeArray, Vector2[] directions)
    {
        List<Node> neighborNodes = new List<Node>();

        foreach (Vector2 direction in directions)
        {
            int newX = x + (int)direction.x;
            int newY = y + (int)direction.y;

            if (IsInsideMap(newX, newY) && nodeArray[newX, newY] != null &&
                nodeArray[newX, newY].nodeType != NodeType.Blocked)
            {
                neighborNodes.Add(nodeArray[newX, newY]);
            }
        }
        return neighborNodes;

    }

    List<Node> GetNeighbours(int x, int y)
    {
        return GetNeighbours(x, y, nodes, possibleDirections);
    }

    public float GetNodeDistance(Node currentNode, Node targetNode)
    {
        int deltaX = Mathf.Abs(currentNode.xIndex - targetNode.xIndex);
        int deltaY = Mathf.Abs(currentNode.yIndex - targetNode.yIndex);

        int min = Mathf.Min(deltaX, deltaY);
        int max = Mathf.Max(deltaX, deltaY);

        int diagonalSteps = min;
        int straightSteps = max - min;

        return (1.2f * diagonalSteps + straightSteps);
    }

}
