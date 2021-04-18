using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HackingGame : MonoBehaviour
{
    [Range(1, 3)]
    public int Difficulty = 1;
    public int Tries = 3;
    [SerializeField] private GameObject NumPrefab;
    [SerializeField] private GridLayoutGroup Display;
    [SerializeField] private TextMeshProUGUI TriesDisplay;
    [SerializeField] private HorizontalLayoutGroup CodeDisplay;
    [SerializeField] private GameObject Lock;

    private List<int> code;
    private List<NumUI> ui;
    private int curTry;

    public void SetSkill(int tries) { Tries = tries; }
    public void SetDifficulty(int difficulty){ Difficulty = difficulty; }

    public void BuildCode()
    {
        curTry = 2 + Difficulty;
        SetupUI();

        //Build code list
        for (int i = 0; i < curTry; i++)
        {
            code.Add(Random.Range(1, 9));
        }
    }

    private void SetupUI()
    {
        //Clear any Tries
        if (code != null) { code.Clear(); }
        else { code = new List<int>(); }
        if (ui != null) { ui.Clear(); }
        else { ui = new List<NumUI>(); }

        ClearScreen(Display.transform);
        ClearScreen(CodeDisplay.transform);
        TriesDisplay.text = $"Tries Left: {Tries}";
        Display.constraintCount = curTry;
        //Build Columns
        for (int i = 1; i <= curTry; i++)
        {
            NumUI numUI = Instantiate(NumPrefab, Display.transform).GetComponent<NumUI>();
            numUI.SetCode(i, -1);
        }

    }

    //Enter a number into decoder for current attempt
    public void EnterNumber(int num)
    {
        if (curTry > 0)
        {
            NumUI numUI = Instantiate(NumPrefab, Display.transform).GetComponent<NumUI>();
            numUI.SetCode(num, 0);
            ui.Add(numUI);
            curTry--;
        }
    }

    public bool CheckCode()
    {
        bool win = true;

        if (Tries > 0) //Check valid try and reduce tries
        {
            if (ui.Count > code.Count)
            { Debug.LogError("Fatal Hacking Code Error"); return false; }

            while (ui.Count < code.Count) //Make Valid Length
            {
                EnterNumber(0);
            }

            for (int i = 0; i < ui.Count; i++)
            {
                if (ui[i].GetNumber() == code[i])
                {//Correct Number and Placement
                    ui[i].SetState(2);
                }
                else if (code.Contains(ui[i].GetNumber()))
                {//Correct Number wrong Placement
                    ui[i].SetState(3);
                    win = false;
                }
                else
                {
                    ui[i].SetState(1);
                    win = false;
                }
                Debug.Log($"Input: {ui[i].GetNumber()} vs Code: {code[i]}");
            }

            Tries--;
        }
        else { win = false; }
        //reset attempt
        TriesDisplay.text = $"Tries Left: {Tries}";
        ui.Clear();
        curTry = 2 + Difficulty;

        return win;
    }

    public void Check()
    {
        if (CheckCode())
        {
            curTry = 0; // Locks Keys
            StartCoroutine(DisplayCode());
            Lock.SetActive(false); //Unlock
        }
        else if (Tries <= 0)
        {
            curTry = 0; //Locks Keys
        }
    }

    IEnumerator DisplayCode()
    {
        foreach (var num in code)
        {
            var numUI = Instantiate(NumPrefab, CodeDisplay.transform).GetComponent<NumUI>();
            numUI.SetCode(num, 0);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void ClearScreen(Transform screen)
    {
        foreach (Transform child in screen)
        {
            Destroy(child.gameObject);
        }
    }
}
