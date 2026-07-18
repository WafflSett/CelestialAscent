using UnityEngine;
using System.Collections.Generic;
using CelestialAscent;

public class DeckManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();
    private int currIndex = 0;
    HandManager handManager;

    public int startingHandSize = 6;
    public int currHandSize = 0;
    public int maxHandSize = 10;

    private void Start()
    {
        Card[] cards = Resources.LoadAll<Card>("Cards");
        allCards.AddRange(cards);

        handManager = FindFirstObjectByType<HandManager>();
        for (int i = 0; i < startingHandSize; i++)
        {
            DrawCard(handManager);
        }
    }

    private void Update()
    {
        currHandSize = handManager.cardsInHand.Count;
    }

    public void DrawCard(HandManager handManager)
    {
        if (allCards.Count == 0 || currHandSize >= maxHandSize)
            return;

        Card nextCard = allCards[currIndex];
        handManager.AddCardToHand(nextCard);
        currIndex = (currIndex + 1) % allCards.Count;
    }
}
