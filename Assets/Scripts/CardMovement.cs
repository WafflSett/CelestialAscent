using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using CelestialAscent;

public class CardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private GridManager gridManager;
    private RectTransform rectTransform;
    private RectTransform canvasRectTransform;
    private Canvas canvas;
    private int currentState = 0;

    // starting rectTransform transforms
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    [SerializeField] private float selectScale = 1.1f;
    [SerializeField] private Vector2 cardPlay;
    [SerializeField] private Vector3 playPosition;
    [SerializeField] private GameObject glowEffect;
    [SerializeField] private GameObject playArrow;
    [SerializeField] private float lerpFactor = 0.1f;
    [SerializeField] private int cardPlayDivider = 4;
    [SerializeField] private float cardPlayMultiplier = 1f;
    [SerializeField] private bool toUpdateCardPosition = false;
    [SerializeField] private int playPositionYDivider = 4;
    [SerializeField] private float playPositionYMultiplier = 1f;
    [SerializeField] private int playPositionXDivider = 4;
    [SerializeField] private float playPositionXMultiplier = 1f;
    [SerializeField] private bool toUpdatePlayPosition = false;


    void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvasRectTransform = canvas.GetComponent<RectTransform>();
        }
        SetDefaultValues();
        UpdateCardPlayPosition();
        UpdatePlayPosition();
    }
    private void UpdateCardPlayPosition()
    {
        if (cardPlayDivider == 0 || canvasRectTransform == null)
            return;
        float segment = cardPlayMultiplier / cardPlayDivider;
        cardPlay.y = canvasRectTransform.rect.height * segment;
    }

    private void UpdatePlayPosition()
    {
        if (playPositionXDivider == 0 || playPositionYDivider == 0 || canvasRectTransform == null)
            return;
        float segmentX = playPositionXMultiplier / playPositionXDivider;
        float segmentY = playPositionYMultiplier / playPositionYDivider;

        playPosition.x = canvasRectTransform.rect.width * segmentX;
        playPosition.y = canvasRectTransform.rect.height * segmentY;
    }

    private void SetDefaultValues()
    {
        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.localPosition;
        originalRotation = rectTransform.rotation;
    }

    void Update()
    {
        if (toUpdateCardPosition)
        {
            UpdateCardPlayPosition();
        }
        if (toUpdatePlayPosition)
        {
            UpdatePlayPosition();
        }

        switch (currentState)
        {
            case 1:
                HandleHoverState();
                break;
            case 2:
                HandleDragState();
                if (!Mouse.current.leftButton.isPressed)
                {
                    PlayHeldCard();
                }
                break;
            case 3:
                HandlePlayState();
                break;
            default:
                break;
        }
    }

    private void TransitionToDefaultState()
    {
        currentState = 0;
        rectTransform.localPosition = originalPosition;
        rectTransform.localRotation = originalRotation;
        rectTransform.localScale = originalScale;
        glowEffect.SetActive(false);
        playArrow.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentState == 1) {
            TransitionToDefaultState();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState == 0)
        {
            SetDefaultValues();
            currentState = 1;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentState == 1)
        {
            currentState = 2;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Card cardData = GetComponent<CardDisplay>().cardData;
        if (currentState == 2 && Mouse.current.position.ReadValue().y > cardPlay.y)
        {
            switch (cardData.cardType)
            {
                case CardType.Spell:
                    currentState = 3;
                    playArrow.SetActive(true);
                    rectTransform.localPosition = Vector3.Lerp(rectTransform.position, playPosition, lerpFactor);
                    break;
                default:
                    break;
            }
        }
    }

    private void HandleHoverState()
    {
        glowEffect.SetActive(true);
        rectTransform.localScale = originalScale*selectScale;
    }

    private void HandleDragState()
    {
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.position = Vector3.Lerp(rectTransform.position, Mouse.current.position.ReadValue(), lerpFactor);
    }

    private void HandlePlayState()
    {
        rectTransform.localPosition = playPosition;
        rectTransform.localRotation = Quaternion.identity;
        if (Mouse.current.leftButton.isPressed == false)
        {
            PlayHeldCard();
        }

        if (Mouse.current.position.ReadValue().y < cardPlay.y)
        {
            currentState = 2;
            playArrow.SetActive(false);
        }
    }
 
    
    private void PlayHeldCard()
    {
        Card cardData = GetComponent<CardDisplay>().cardData;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        // check for gridcell only if its a card
        if (hit.collider != null && hit.collider.GetComponent<GridCell>())
        {
            GridCell cell = hit.collider.GetComponent<GridCell>();
            Vector2 targetPos = cell.gridIndex;
            switch (cardData.cardType)
            {
                case (CardType.Unit):
                    if (gridManager.AddObjectToGrid(cardData.boardPrefab, targetPos))
                    {
                        HandManager handmanager = FindFirstObjectByType<HandManager>();
                        handmanager.cardsInHand.Remove(gameObject);
                        handmanager.UpdateHandVisuals();
                        Destroy(gameObject);
                    }
                    break;
                default:
                    break;
            }
        }
        TransitionToDefaultState();
    }
}
