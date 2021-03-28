using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemPiece : MonoBehaviour
{
    public Gem gem;
    private Image image;
    private float xScale;
    private float yScale;

    [HideInInspector] public RectTransform rect;
    [HideInInspector] public Vector2 pos;

    public void Initialize(Gem g, Sprite s, float x, float y)
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();

        gem = g;
        xScale = x;
        yScale = y;
        image.sprite = s;

        SetPoint(gem.location);
        rect.anchoredPosition = pos;
        rect.sizeDelta = new Vector2(xScale, yScale);
    }

    public void SetPoint(Point p)
    {
        gem.location = p;
        SnapPosition();
        UpdateName();
    }

    public void SnapPosition()
    {
        pos = new Vector2(xScale * gem.location.x, -yScale * gem.location.y);
    }

    public void UpdateName()
    {
        transform.name = "Gem [" + gem.location.x + "," + gem.location.y + "]";
    }
}
