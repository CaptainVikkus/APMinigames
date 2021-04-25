using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUI : MonoBehaviour
{
    public GameObject ui;
    public CameraController player;
    public List<Canvas> playerUI;
    public bool triggerImediate = true;
    private bool entered = false;
    private bool change = false;

    protected virtual void Start()
    { //Only Start update if triggerImediate is false
        if (triggerImediate)
            StartCoroutine(UICheck());
    }

    //Custom Update runs once a frame if triggerImediate is active at start
    IEnumerator UICheck()
    {
        while(isActiveAndEnabled)
        {
            if (change)
            {
                EnableUI(entered);
                change = false;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && triggerImediate)
        {
            EnableUI(true);
        }
        entered = true;
        change = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && triggerImediate)
        {
            EnableUI(false);
        }
        entered = false;
        change = true;
    }

    protected virtual void EnableUI(bool enable)
    {
        //Set UI Visibility
        ui.SetActive(enable);
        //Lock/Unlock Player
        player.enabled = !enable;
        foreach (var ui in playerUI)
        {
            ui.enabled = !enable;
        }
        AppEvents.Invoke_OnMouseCursorEnable(enable);
    }
}
