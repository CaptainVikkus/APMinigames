using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockNode : MonoBehaviour
{
    public LockpickController.Node nodeType;
    public bool isLocked;
    public bool isStart;

    public Vector3 startPos;
    private Vector2 mousePrev;
    private Vector2 mouseDelta;

    private void Start()
    {
        startPos = transform.localPosition;
    }

    private Vector2 ConvertMousetoWorld()
    {
        return new Vector2(
            (Input.mousePosition.x / (float)Screen.width) * 35,
            (Input.mousePosition.y / (float)Screen.height) * 20
            );
    }

    public void OnMouseDown()
    {
        Debug.Log("Node Hit");
        mousePrev = ConvertMousetoWorld();
    }

    public void OnMouseDrag()
    {
        if (isStart && !isLocked)
        {
            mouseDelta = ConvertMousetoWorld() - mousePrev;
            mousePrev = ConvertMousetoWorld();
            transform.localPosition += new Vector3(mouseDelta.x, mouseDelta.y, 0);
        }
    }

    public void OnMouseUp()
    {
        if (!isLocked)
        {
            transform.localPosition = startPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Huzzah");
        var node = other.gameObject.GetComponent<LockNode>();
        if (node != null && !node.isStart && node.nodeType == nodeType)
        {
            isLocked = true; //Success!
            transform.localPosition = new Vector3(
                node.transform.localPosition.x + 2.65f,
                node.transform.localPosition.y,
                node.transform.localPosition.z
                );
        }
    }
}
