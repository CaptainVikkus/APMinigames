using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumUI : MonoBehaviour
{
    [SerializeField] private int Number;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private int State;
    [SerializeField] private Image image;

    public void SetNumber(int num)
    {
        Number = num;
        text.text = Number.ToString();
    }
    public int GetNumber()
    {
        return Number;
    }

    public void SetState(int state)
    {
        State = state;
        switch (State)
        {
            case -1: //Header
                image.color = Color.blue;

                break;
            case 1: //Wrong
                image.color = Color.red;
                break;
            case 2: //Correct
                image.color = Color.green;
                break;
            case 3: //Wrong Spot
                image.color = Color.yellow;
                break;
            default:
                image.color = Color.clear;
                break;
        }
    }
    public int GetState()
    {
        return State;
    }

    public void SetCode(int num, int state)
    {
        SetNumber(num);
        SetState(state);
    }
}
