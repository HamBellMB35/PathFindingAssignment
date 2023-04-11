using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("Pathfinder Settings")]
    public int startvalueX;
    public int startValueY;
    public int endValueX;
    public int endValueY;

    [Header("Pathfinder components")]
    public PathFinder pathfinderObject;
    public MapData mapDataObejct;
    public Graph graphObject;
    public float interval = 0.1f;

    void Start()
    {
        if (mapDataObejct != null && graphObject != null)
        {
            int[,] map = mapDataObejct.MakeMap();
            graphObject.Initialize(map);

            GraphView graphView = graphObject.gameObject.GetComponent<GraphView>();

            if (graphView != null)
            {
                graphView.Init(graphObject);
            }

            if (graphObject.IsInsideMap(startvalueX, startValueY) && graphObject.IsInsideMap(endValueX, endValueY) && pathfinderObject != null)
            {
                Node startPositionNode = graphObject.nodes[startvalueX, startValueY];
                Node endPositionNode = graphObject.nodes[endValueX, endValueY];
               
                pathfinderObject.Initialize(graphObject, graphView, startPositionNode, endPositionNode);
                StartCoroutine(pathfinderObject.SearchCoRoutine(interval));
            }
        }
    }

}
