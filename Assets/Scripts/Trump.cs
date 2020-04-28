using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Trump : MonoBehaviour
{
    public void Init(UnityAction setSpades, UnityAction setHearts, UnityAction setDiamonds, UnityAction setClubs )
    {
        GameObject spades = GameObject.Find("Spades");
        Button spadesButton = spades.GetComponent<Button>();
        spadesButton.onClick.AddListener(setSpades);

        GameObject hearts = GameObject.Find("Hearts");
        Button heartsButton = hearts.GetComponent<Button>();
        heartsButton.onClick.AddListener(setHearts);

        GameObject diamonds = GameObject.Find("Diamonds");
        Button diamondsButton = diamonds.GetComponent<Button>();
        diamondsButton.onClick.AddListener(setDiamonds);

        GameObject clubs = GameObject.Find("Clubs");
        Button clubsButton = clubs.GetComponent<Button>();
        clubsButton.onClick.AddListener(setClubs);
    }
}
