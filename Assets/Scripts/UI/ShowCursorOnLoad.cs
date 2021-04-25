using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCursorOnLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AppEvents.Invoke_OnMouseCursorEnable(true);
    }
}
