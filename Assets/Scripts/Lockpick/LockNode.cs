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
    public LineRenderer line;

    private void Start()
    {
        startPos = transform.localPosition;
        if (isStart)
        {//Set up Line
            line = gameObject.AddComponent<LineRenderer>();
            line.positionCount = 2;
            line.widthMultiplier = 0.05f;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, transform.position);
        }
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
        if (isStart)
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, transform.position);
        }
    }

    public void OnMouseDrag()
    {
        if (isStart && !isLocked)
        {
            mouseDelta = ConvertMousetoWorld() - mousePrev;
            mousePrev = ConvertMousetoWorld();
            transform.localPosition += new Vector3(mouseDelta.x, mouseDelta.y, 0);
            line.SetPosition(1, transform.position);
        }
    }

    public void OnMouseUp()
    {
        if (!isLocked)
        {
            transform.localPosition = startPos;
            line.SetPosition(1, transform.position);
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
            line.SetPosition(1, transform.position);
        }
    }
}
