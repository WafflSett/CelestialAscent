using System;
using UnityEngine;

public class UIObjectPositioner : MonoBehaviour
{
    private RectTransform targetObject;

    public int widthDivider = 2;
    public int heightDivider = 2;
    public float widthMult = 1f;
    public float heightMult = 1f;

    public bool updatePosition = false;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetObject = GetComponent<RectTransform>();
        SetUIObjectPosition();
    }


    // Update is called once per frame
    void Update()
    {
        if (updatePosition)
        {
            SetUIObjectPosition();
        }
    }
    private void SetUIObjectPosition()
    {
        if (targetObject == null || widthDivider == 0 || heightDivider == 0)
            return;
        float anchorX = widthMult / widthDivider;
        float anchorY = heightMult / heightDivider;

        targetObject.anchorMin = new Vector2(anchorX, anchorY);
        targetObject.anchorMax = new Vector2(anchorX, anchorY);
        targetObject.pivot = new Vector2(0.5f, 0.5f);

        targetObject.anchoredPosition = Vector2.zero;
    }
}
