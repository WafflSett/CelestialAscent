using System.Collections.Generic;
using UnityEngine;

namespace CelestialAscent{

    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]
    public class Card : ScriptableObject {
        public string title;
        public string description;
        public int cost;
        public int attack;
        public int health;
        public DamageType damageType;
        public CardType cardType;
        public List<Keyword> keywords;
        public Rarity rarity;
        public Sprite sprite;
        public GameObject boardPrefab;
    }

    public enum CardType
    {
        Unit,
        Spell,
    }
    public enum DamageType
    {
        Physical,
        Solar,
        Lunar,
        Stardust,
        Void
    }

    public enum Keyword
    {
        Quickstrike,
        Overwhelm,
        Elusive,
        Tough
    }

    public enum Rarity
    {
        Common,
        Rare,
        Epic,
        Legendary
    }
}