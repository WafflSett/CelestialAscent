using UnityEngine;
using System.Collections.Generic;
using CelestialAscent;
using System;

public class HandManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform handTransform; // hand position root

    public float fanSpread = -10f;
    public float horizontalSpacing = 100f;
    public float verticalSpacing = 25f;
    
    public List<GameObject> cardsInHand = new List<GameObject>();

    void Start()
    {

    }

    private void Update()
    {
        //UpdateHandVisuals();

    }

    public void AddCardToHand(Card cardData)
    {
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        cardsInHand.Add(newCard);
        newCard.GetComponent<CardDisplay>().cardData = cardData;
        UpdateHandVisuals();
    }

    public void UpdateHandVisuals()
    {
        int cardCount = cardsInHand.Count;

        if (cardCount == 1)
        {
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }

        for (int i = 0; i < cardCount; i++)
        {
            //float rotationAngle = fanSpread * i;
            float rotationAngle = (fanSpread * (i - (cardCount - 1) / 2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

            float horizontalOffset = (horizontalSpacing * (i - (cardCount - 1) / 2f));

            float normalizedPos = (2f * i / (cardCount - 1) - 1f);
            float verticalOffset = verticalSpacing * (1-normalizedPos*normalizedPos);
            cardsInHand[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);
        }
    }
}
