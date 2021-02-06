using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ExtractorMode
{
    EXTRACTION,
    SCANNING,
    COMPLETE
}

public class ExtractorController : MonoBehaviour
{
    public ExtractorMode mode;
    public int resourceTotal;

    public GameObject screenGrid;
    private GameObject[,] buttonGrid;
    public GameObject screenButton;
    public ResourceGenerator grid;

    public LayerMask scanMask;
    public LayerMask extractMask;
    public int extractTry = 3;
    private int currExTry;
    public int scanTry = 6;
    private int currScnTry;
    public TextMeshProUGUI ExtractTriesUI;
    public TextMeshProUGUI ScanTriesUI;
    public TextMeshProUGUI resourceUI;

    public void Start()
    {
        ResetGrid();
    }

    //Extraction = 0, Scanning = 1
    public void ChangeMode(int newMode)
    {
        //Cannot override complete unless reset
        if (mode == ExtractorMode.COMPLETE)
        { return; }

        //Change mode and functionality
        mode = (ExtractorMode)newMode;
        switch (mode)
        {
            case ExtractorMode.EXTRACTION:
                foreach (var button in buttonGrid)
                {
                    button.GetComponent<BoxCollider>().enabled = true;
                }
                Debug.Log("Extract Mode Activated");
                break;
            case ExtractorMode.SCANNING:
                foreach (var button in buttonGrid)
                {
                    button.GetComponent<BoxCollider>().enabled = true;
                }
                Debug.Log("Scan Mode Activated");
                break;
            case ExtractorMode.COMPLETE:
                foreach (var button in buttonGrid)
                {
                    button.GetComponent<BoxCollider>().enabled = false;
                }
                Debug.Log("Extraction Complete");
                break;
            default:
                break;
        }
    }

    public void CollectResource(int row, int col)
    {
        if (currExTry >= 1)
        {
            resourceTotal += grid.CollectNode(row, col);
            currExTry--;
        }
        if (currExTry <= 0)
        {
            ChangeMode((int)ExtractorMode.COMPLETE);
        }
        UpdateUI();
    }

    public void ScanResource(int row, int col, float radius)
    {
        if (currScnTry >= 1)
        {
            grid.ScanNode(row, col, radius);
            currScnTry--;
        }
        UpdateUI();
    }

    public void BuildScreen()
    {
        //Set Grid Size
        var rect = screenGrid.GetComponent<RectTransform>().rect;
        float x = rect.width / grid.rows;
        float y = rect.height / grid.columns;
        screenGrid.GetComponent<GridLayoutGroup>().cellSize = new Vector2(x, y);
        //Build Buttons under new Grid
        buttonGrid = new GameObject[grid.rows, grid.columns];
        for (int i = 0; i < grid.rows; i++)
        {
            for (int j = 0; j < grid.columns; j++)
            { 
                //set the size of the collider to button size
                var button = Instantiate(screenButton, screenGrid.transform);
                button.GetComponent<BoxCollider>().size = new Vector3(x, y, 1);
                //Set callback variables
                var vars = button.GetComponent<ExtractorButton>();
                vars.row = i;
                vars.col = j;
                vars.controller = this;
                //Add button to grid
                buttonGrid[i, j] = button;
            }
        }
    }

    public void ResetGrid()
    {
        currExTry = extractTry;
        currScnTry = scanTry;
        if (buttonGrid != null)
        {
            foreach (var button in buttonGrid)
            {
                Destroy(button);
            }
            buttonGrid = null;
        }
        grid.GenerateNodes();
        BuildScreen();
        mode = ExtractorMode.EXTRACTION;
        ChangeMode((int)ExtractorMode.EXTRACTION);
        resourceTotal = 0;

        UpdateUI();
    }

    public void UpdateUI()
    {
        ExtractTriesUI.text = currExTry.ToString() + " / " + extractTry.ToString();
        ScanTriesUI.text = currScnTry.ToString() + " / " + scanTry.ToString();
        resourceUI.text = resourceTotal.ToString();
    }
}
