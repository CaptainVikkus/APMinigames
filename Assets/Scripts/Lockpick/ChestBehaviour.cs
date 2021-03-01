using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChestBehaviour : MonoBehaviour
{
    public Animation lidOpen;
    public Collider lockSpot;
    public TextMeshProUGUI message;
    public TextMeshProUGUI button;
    public LockpickController lockBox;

    private bool won = false;


    // Start is called before the first frame update
    void Start()
    {
        lockBox.OnLockpickEnd += LockPickEnd;
    }

    private void LockPickEnd(bool won)
    {
        if (won)
        {
            lidOpen.Play();
            message.text = "Access Granted";
            message.fontSize = 1;
            lockSpot.enabled = false;
        }
        else
        {
            message.text = "Access Denied";
            message.fontSize = 1;
        }
        button.text = "Restart";
        this.won = won;
    }

    public void ResetBox()
    {
        message.text = "Insert Key";
        message.fontSize = 2;
        button.text = "Open";

        if (won)
        {
            lidOpen.Play("LidClose");
            lockSpot.enabled = true;
        }
    }
}
