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

    public void Init(Vector3 position, float scaleRatio, string canvasName)
    {
        transform.position = position;
        canvas = GameObject.Find(canvasName);
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

        Card card = deck[0];
        deck.RemoveAt(0);

        return card;
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

    public void HideDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            deck[i].gameObject.SetActive(false);
        }
    }

    public void HidePlayerCards()
    {
        for (int i = 0; i < playerOne.Count; i++)
        {
            playerOne[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < playerTwo.Count; i++)
        {
            playerTwo[i].gameObject.SetActive(false);
        }
    }

    public void FlipKitty(float kittyDistance)
    {
        for (int i = 0; i < kitty.Count; i++)
        {
            Vector3 currentPosition = kitty[i].transform.position;
            kitty[i].Move(
                currentPosition,
                new Vector3(currentPosition.x + (kittyDistance * i * 2), currentPosition.y - (kittyDistance * (i - 1)), currentPosition.z),
                true
            );
        }
    }
    
    public void RemovePlayerOneCards(Vector3 target)
    {
        for (int i = 0; i < playerOne.Count; i++)
        {
            playerOne[i].Move(playerOne[i].transform.position, target, false);
        }
    }

    public void RemovePlayerTwoCards(Vector3 target)
    {
        for (int i = 0; i < playerTwo.Count; i++)
        {
            playerTwo[i].Move(playerTwo[i].transform.position, target, false);
        }
    }

    public void MoveKittyY(float y, bool toPlayerOne)
    {
        for (int i = 0; i < kitty.Count; i++)
        {
            kitty[i].Move(
               kitty[i].transform.position,
               new Vector3(kitty[i].transform.position.x, y, kitty[i].transform.position.z),
               false
           );
            if (toPlayerOne)
            {
                playerOne.Add(kitty[i]);
            }
            else
            {
                playerTwo.Add(kitty[i]);
            }
        }
        kitty.Clear();
    }

    public void ShowPLayerOneCards(float shrinkRatio)
    {
        Debug.Log(shrinkRatio);
        float y = -350 * shrinkRatio;
        float change = 130 * shrinkRatio;
        float cardWidth = 120;
        float scale = 0.8f;
        float x = (playerOne.Count * cardWidth * scale) / 2;
        for (int i = 0; i < playerOne.Count; i++)
        {
            Card card = playerOne[i];
            card.gameObject.SetActive(true);
            card.transform.localScale = new Vector3(scale, scale, 1);
            card.Move(new Vector3(x - (i * change), y, 200 * shrinkRatio), true);
        }
    }
}