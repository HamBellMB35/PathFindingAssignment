using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GraphView : MonoBehaviour
{

    public GameObject nodeViewPrefab;
    public NodeView[,] nodeViews;
    public void Init(Graph graph)
    {
        if (graph == null)
        {
            Debug.LogWarning("Error: No graphObject to initialize!");
            return;
        }
        nodeViews = new NodeView[graph.Width, graph.Height];

        foreach (Node node in graph.nodes)
        {
            GameObject instance = Instantiate(nodeViewPrefab, Vector3.zero, Quaternion.identity);
            NodeView nodeView = instance.GetComponent<NodeView>();

            if (nodeView != null)
            {
                nodeView.Initialize(node);
                nodeViews[node.xIndex, node.yIndex] = nodeView;

                Color originalColor = MapData.GetColorFromNodeType(node.nodeType);
                nodeView.ColorThisNode(originalColor);
               
            }
        }
    }

    public void ColorNodes(List<Node> nodes, Color color, bool interpolationColor = false, float interpolationValue = 0.5f)
    {
        foreach (Node node in nodes)
        {
            if (node != null)
            {
                NodeView nodeView = nodeViews[node.xIndex, node.yIndex];
                Color newColor = color;

                if (interpolationColor)
                {
                    Color originalColor = MapData.GetColorFromNodeType(node.nodeType);
                    newColor = Color.Lerp(originalColor, newColor, interpolationValue);
                }

                if (nodeView != null)
                {
                    nodeView.ColorThisNode(newColor);
                }
            }
        }
    }

}
