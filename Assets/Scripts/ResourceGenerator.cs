using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ResourceGenerator : MonoBehaviour
{
    [Range(32, 64)]
    public int rows = 32;
    [Range(32, 64)]
    public int columns = 32;
    [Tooltip("Number of high value nodes available")]
    public int numHigh;

    public GameObject node;
    private GameObject[,] nodeGrid;
    public Camera resCam;
    public float camHeight = 30;
    public RenderTexture screenTex;
    private float nodeOffset = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(node.GetComponent<ResourceNodeController>());
        resCam.targetTexture = screenTex;
    }

    public void GenerateNodes()
    {
        //Delete any existing nodes
        if (nodeGrid != null && nodeGrid.Length != 0)
        {
            foreach (GameObject gameObject in nodeGrid)
            {
                Destroy(gameObject);
            }
            nodeGrid = null;
        }
        //Populate new Grid
        nodeGrid = new GameObject[rows, columns];
        for (int i = 0; i < rows; i++ )
        {
            for (int j = 0; j < columns; j++)
            {//Create new node and set its local position along grid
                Vector3 position = new Vector3(i * nodeOffset, 0, j * nodeOffset);
                nodeGrid[i, j] = Instantiate(node, transform);
                nodeGrid[i, j].transform.localPosition = position;
            }
        }
        //Add high value nodes
        if (nodeGrid.Length > 0 && numHigh > 0)
        {//If there are nodes available
            for (int i = 0; i < numHigh; i++)
            {
                int x = Random.Range(0, rows - 1);
                int y = Random.Range(0, columns - 1);
                PopulateNodes(x, y);
            }
        }
        //Set Camera position
        resCam.transform.localPosition = new Vector3((rows * nodeOffset / 2)- 2, camHeight, (columns * nodeOffset / 2)- 2);
        resCam.orthographicSize = rows / 2.5f;
    }

    //Populate the resources in a 5x5 around a high value node
    private void PopulateNodes(int row, int column)
    {
        //Set chosen node as high value
        PopulateNode(row, column, ResourceNodeController.Level.HIGH);
        //Set surrounding nodes as medium value
        PopulateNode(row, column - 1, ResourceNodeController.Level.MEDIUM);
        PopulateNode(row, column + 1, ResourceNodeController.Level.MEDIUM);
        PopulateNode(row - 1, column, ResourceNodeController.Level.MEDIUM);
        PopulateNode(row + 1, column, ResourceNodeController.Level.MEDIUM);
        PopulateNode(row - 1, column - 1, ResourceNodeController.Level.MEDIUM);
        PopulateNode(row + 1, column - 1, ResourceNodeController.Level.MEDIUM);
        PopulateNode(row - 1, column + 1, ResourceNodeController.Level.MEDIUM);
        PopulateNode(row + 1, column + 1, ResourceNodeController.Level.MEDIUM);
        //Set furthest nodes as low value
        PopulateNode(row, column - 2, ResourceNodeController.Level.LOW);
        PopulateNode(row, column + 2, ResourceNodeController.Level.LOW);
        PopulateNode(row + 1, column - 2, ResourceNodeController.Level.LOW);
        PopulateNode(row + 1, column + 2, ResourceNodeController.Level.LOW);
        PopulateNode(row - 1, column - 2, ResourceNodeController.Level.LOW);
        PopulateNode(row - 1, column + 2, ResourceNodeController.Level.LOW);
        PopulateNode(row + 2, column - 2, ResourceNodeController.Level.LOW);
        PopulateNode(row + 2, column + 2, ResourceNodeController.Level.LOW);
        PopulateNode(row - 2, column - 2, ResourceNodeController.Level.LOW);
        PopulateNode(row - 2, column + 2, ResourceNodeController.Level.LOW);
        PopulateNode(row - 2, column, ResourceNodeController.Level.LOW);
        PopulateNode(row - 2, column - 1, ResourceNodeController.Level.LOW);
        PopulateNode(row - 2, column + 1, ResourceNodeController.Level.LOW);
        PopulateNode(row + 2, column, ResourceNodeController.Level.LOW);
        PopulateNode(row + 2, column - 1, ResourceNodeController.Level.LOW);
        PopulateNode(row + 2, column + 1, ResourceNodeController.Level.LOW);
    }

    //Populate a node at position if the node exists
    private void PopulateNode(int row, int column, ResourceNodeController.Level level)
    {
        //Check that picked node is within the boundaries for the grid
        if ((row >= 0 && row < rows) && (column >= 0 && column < columns))
        {
            //Debug.Log(row + " " + column);
            nodeGrid[row, column].GetComponent<ResourceNodeController>().SetLevel(level);
        }
    }

    //Return resource from valid node, otherwise 0
    public int CollectNode(int row, int col)
    {
        if ((row >= 0 && row < rows) && (col >= 0 && col < columns))
            return nodeGrid[row, col].GetComponent<ResourceNodeController>().Collect();
        else
            return 0;
    }
}
