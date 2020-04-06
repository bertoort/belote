using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck : MonoBehaviour
{
    [SerializeField]
    private GameObject _cardPrefab;

    private List<Card> _deck = new List<Card>();
    private List<Card> _discardPile = new List<Card>();

    public Card TakeCard()
    {
        if (_deck.Count == 0)
            return null;

        Card Card = _deck[0];
        _deck.RemoveAt(0);

        return Card;
    }

    public void Awake()
    {
        foreach (SuitEnum suit in System.Enum.GetValues(typeof(SuitEnum)))
        {
            foreach (RankEnum rank in System.Enum.GetValues(typeof(RankEnum)))
            {
                GameObject cardInstance = Instantiate(_cardPrefab);
                Card card = cardInstance.GetComponent<Card>();
                if (card != null)
                {
                    card.Init(suit, rank);
                    _deck.Add(card);
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Card card = TakeCard();
            Debug.Log(card);
        }
    }

}