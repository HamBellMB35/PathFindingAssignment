using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeView : MonoBehaviour
{

    Node _node;
    public GameObject tile;
    float borderSize = 0.05f;

    public void Initialize(Node node)
    {
        if (tile != null)
        {
            gameObject.transform.position = node.position;
            tile.transform.localScale = new Vector3(1f - borderSize, 1f, 1f - borderSize);
            _node = node;

        }
    }

    void ColorThisNode(Color color, GameObject go)
    {
        if (go != null)
        {
            Renderer gameobjRenderer = go.GetComponent<Renderer>();

            if (gameobjRenderer != null)
            {
                gameobjRenderer.material.color = color;
            }
        }
    }

    public void ColorThisNode(Color color)
    {
        ColorThisNode(color, tile);
    }

}

   
