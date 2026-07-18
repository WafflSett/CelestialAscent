using CelestialAscent;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;
    public Image cardImage;
    public Image typeImage;
    public TMP_Text titleText;
    public TMP_Text healthText;
    public TMP_Text damageText;
    public TMP_Text costText;

    public Sprite[] elementSprites;


    private Dictionary<DamageType, (Sprite, Color)> elementImages;

    private void Start()
    {
        elementImages = new Dictionary<DamageType, (Sprite, Color)>()
        {
            { DamageType.Physical, (elementSprites[0], Color.gray)},
            { DamageType.Lunar, (elementSprites[1], Color.lightBlue)},
            { DamageType.Solar, (elementSprites[2], Color.yellow)},
            { DamageType.Stardust, (elementSprites[3], Color.lightSkyBlue)},
            { DamageType.Void, (elementSprites[4], Color.purple)},
        };
        UpdateCardDisplay();    
    }

    public void UpdateCardDisplay() {
        cardImage.sprite = cardData.sprite;
        titleText.text = cardData.title;
        healthText.text = cardData.health.ToString();
        damageText.text = cardData.attack.ToString();
        costText.text = cardData.cost.ToString();
        typeImage.sprite = elementImages[cardData.damageType].Item1;
        typeImage.color = elementImages[cardData.damageType].Item2;
    }

}
