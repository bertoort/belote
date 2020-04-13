using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum CardHand
{
    PlayerOne = 1,
    PlayerOneDiscard = 2,
    PlayerTwo = 3,
    PlayerTwoDiscard = 4,
    Kitty = 5,
}

public class Deck : MonoBehaviour
{
    [SerializeField]
    private GameObject cardPrefab;
    private GameObject canvas;

    private List<Card> deck = new List<Card>();
    private List<Card> playerOne = new List<Card>();
    private List<Card> playerOneDiscard = new List<Card>();
    private List<Card> playerTwo = new List<Card>();
    private List<Card> playerTwoDiscard = new List<Card>();
    private List<Card> kitty = new List<Card>();

    public void Init(Vector3 position, float scaleRatio)
    {
        transform.position = position;
        canvas = GameObject.Find("Screen Space Canvas");
        foreach (SuitEnum suit in System.Enum.GetValues(typeof(SuitEnum)))
        {
            foreach (RankEnum rank in System.Enum.GetValues(typeof(RankEnum)))
            {
                GameObject cardInstance = Instantiate(cardPrefab);
                Card card = cardInstance.GetComponent<Card>();
                if (card != null)
                {
                    card.transform.SetParent(canvas.transform);
                    card.Init(suit, rank, transform.position, new Vector3(scaleRatio, scaleRatio, 1));
                    cardInstance.SetActive(false);
                    deck.Add(card);
                }
            }
        }
        Shuffle();
    }

    public Card TakeCard()
    {
        if (deck.Count == 0)
            return null;

        Card Card = deck[0];
        deck.RemoveAt(0);

        return Card;
    }

    private GameObject TakeCardInstance(string name)
    {
        {
            Transform[] trs = canvas.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in trs)
            {
                if (t.name == name)
                {
                    return t.gameObject;
                }
            }
            return null;
        }
    }

    public void DealCard(Vector3 position, bool faceUp, CardHand hand)
    {
        Card card = TakeCard();
        switch(hand)
        {
            case CardHand.PlayerOne:
                playerOne.Add(card);
                break;
            case CardHand.PlayerTwo:
                playerTwo.Add(card);
                break;
            case CardHand.Kitty:
                kitty.Add(card);
                break;
        }

        GameObject cardInstance = TakeCardInstance(card.id);
        if (cardInstance != null)
        {
            cardInstance.SetActive(true);
        }
        card.Move(transform.position, position, faceUp);
    }

    public void Shuffle()
    {
        System.Random random = new System.Random();
        for (int i = 0; i < deck.Count; i++)
        {
            int j = random.Next(i, deck.Count);
            Card temporary = deck[i];
            deck[i] = deck[j];
            deck[j] = temporary;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}