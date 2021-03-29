using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GemPiece : MonoBehaviour, IPointerClickHandler
{
    public Point location;
    public Image selection;
    private Image image;
    private float xScale;
    private float yScale;

    [HideInInspector] public RectTransform rect; //current position
    [HideInInspector] public Vector2 pos; //target position in grid
    [HideInInspector] public Match3 manager { set; private get; }
    public void Initialize(Point p, Sprite s, float x, float y)
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();

        location = p;
        xScale = x;
        yScale = y;
        image.sprite = s;

        SetPoint(location);
        rect.anchoredPosition = pos + (Vector2.up * yScale * 10); //set 10 steps up
        rect.sizeDelta = new Vector2(xScale, yScale);
        StartCoroutine(FallToPos());
    }

    public void SetPoint(Point p)
    {
        location = p;
        SnapPosition();
        UpdateName();
    }

    public void SnapPosition()
    {
        pos = new Vector2(xScale * location.x, -yScale * location.y);
    }

    public void UpdateName()
    {
        transform.name = "Gem [" + location.x + "," + location.y + "]";
    }

    public IEnumerator FallToPos(float fallSpeed = 3f)
    {
        Vector2 direction = pos - rect.anchoredPosition;
        direction.Normalize();

        while (rect.anchoredPosition != pos)
        {
            yield return new WaitForEndOfFrame();
            rect.anchoredPosition = rect.anchoredPosition + (direction * fallSpeed);
            //Snap when close
            if (Vector2.Distance(rect.anchoredPosition, pos) <= fallSpeed)
            {
                rect.anchoredPosition = pos;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(manager != null)
        {
            manager.SwapPiece(this);
        }
    }
}
