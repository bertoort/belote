using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck : MonoBehaviour
{
    [SerializeField]
    private GameObject _cardPrefab;
    private GameObject _canvas;

    private List<Card> _deck = new List<Card>();
    private List<Card> _playerOne = new List<Card>();
    private List<Card> _playerTwo = new List<Card>();
    private List<Card> _kitty = new List<Card>();

    public void Init(Vector3 position)
    {
        transform.position = position;
    }

    public Card TakeCard()
    {
        if (_deck.Count == 0)
            return null;

        Card Card = _deck[0];
        _deck.RemoveAt(0);

        return Card;
    }

    private GameObject TakeCardInstance(string name)
    {
        {
            Transform[] trs = _canvas.GetComponentsInChildren<Transform>(true);
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

    public void Awake()
    {
        _canvas = GameObject.Find("Screen Space Canvas");
        foreach (SuitEnum suit in System.Enum.GetValues(typeof(SuitEnum)))
        {
            foreach (RankEnum rank in System.Enum.GetValues(typeof(RankEnum)))
            {
                GameObject cardInstance = Instantiate(_cardPrefab);
                Card card = cardInstance.GetComponent<Card>();
                if (card != null)
                {
                    card.transform.SetParent(_canvas.transform);
                    card.Init(suit, rank, transform.position);
                    cardInstance.SetActive(false);
                    _deck.Add(card);
                }
            }
        }
        Shuffle();
    }

    public void DealCard(Vector3 position, bool faceUp)
    {
        Card card = TakeCard();
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
        for (int i = 0; i < _deck.Count; i++)
        {
            int j = random.Next(i, _deck.Count);
            Card temporary = _deck[i];
            _deck[i] = _deck[j];
            _deck[j] = temporary;
        }
    }

}