using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;

public class ArcRenderer : MonoBehaviour
{
    public GameObject arrowPrefab;
    public GameObject dotPrefab;
    public int poolSize = 50;
    private List<GameObject> dotPool = new List<GameObject>();
    private GameObject arrowInstance;

    public float spacing = 50;
    public float arrowAngleAdjustment = 0;
    public int dotsToSkip = 1;
    private Vector3 arrowDirection;
    public float baseScreenWidth = 1920f;
    [SerializeField] private float spacingScale;

    void Start()
    {
        arrowInstance = Instantiate(arrowPrefab, transform);
        arrowInstance.transform.localPosition = Vector3.zero;
        InitializeDotPool(poolSize);
        spacingScale = Screen.width / baseScreenWidth;
    }

    private void OnEnable()
    {
        spacingScale = Screen.width / baseScreenWidth;
    }

    private void InitializeDotPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject dot = Instantiate(dotPrefab, Vector3.zero, Quaternion.identity, transform);
            dot.SetActive(false);
            dotPool.Add(dot);
        }
    }

    void Update()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = 0;
        Vector3 startPos = transform.position;
        Vector3 midPoint = CalculateMidPoint(startPos, mousePos);

        UpdateArc(startPos, midPoint, mousePos);
        PositionAndRotateArrow(mousePos);
    }

    private void UpdateArc(Vector3 start, Vector3 mid, Vector3 end)
    {
        int numDots = Mathf.CeilToInt(Vector3.Distance(start, end)/(spacingScale * spacing));

        for (int i = 0; i < numDots && i < dotPool.Count; i++) {
            float t = i / (float)numDots;
            t = Mathf.Clamp01(t);

            Vector3 position = QuadraticBezierPoint(start, mid, end, t);

            if (i != numDots - dotsToSkip)
            {
                dotPool[i].transform.position = position;
                dotPool[i].SetActive(true);
            }
            if (i == numDots - (dotsToSkip +1) && i-dotsToSkip+1 >= 0)
            {
                arrowDirection = dotPool[i].transform.position;
            }
        }

        for (int i = numDots - dotsToSkip; i < dotPool.Count; i++)
        {
            if (i > 0)
            {
                dotPool[i].SetActive(false);
            }
        }
    }

    private Vector3 QuadraticBezierPoint(Vector3 start, Vector3 control, Vector3 end, float t)
    {
        // Bézier formula B(t) = (1-t)² · start + 2(1-t)t · control + t² · end

        float u = 1 - t;         // u is the "inverse" of t, i.e. how far from the end
        float tt = t * t;        // t²
        float uu = u * u;        // (1-t)²

        Vector3 point = uu * start;           // (1-t)² · start
        point += 2 * u * t * control;         // + 2(1-t)t · control
        point += tt * end;                     // + t² · end
        return point;
    }

    private Vector3 CalculateMidPoint(Vector3 start, Vector3 end)
    {
        Vector3 midPoint = (start + end) / 2;
        float arcHeight = Vector3.Distance(start, end) / 3f;
        midPoint.y += arcHeight;
        return midPoint;
    }

    private void PositionAndRotateArrow(Vector3 position)
    {
        arrowInstance.transform.position = position;
        Vector3 direction = arrowDirection - position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += arrowAngleAdjustment;
        arrowInstance.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
