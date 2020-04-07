using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SuitEnum
{
    Hearts = 1,
    Clubs = 2,
    Diamonds = 3,
    Spades = 4,
}

public enum RankEnum
{
    Ace = 1,
    King = 2,
    Queen = 3,
    Jack = 4,
    Ten = 10,
    Nine = 9,
    Eight = 8,
    Seven = 7,
}


public class Card : MonoBehaviour
{
    private SuitEnum _suit;
    private RankEnum _rank;

    private GameObject _front;
    private GameObject _back;

    public Sprite hearts;
    public Sprite clubs;
    public Sprite diamonds;
    public Sprite spades;


    public Dictionary<RankEnum, string> Ranks = new Dictionary<RankEnum, string>() {
        { RankEnum.Ace, "A" },
        { RankEnum.King, "K" },
        { RankEnum.Queen, "Q" },
        { RankEnum.Jack, "J" },
        { RankEnum.Ten, "10" },
        { RankEnum.Nine, "9" },
        { RankEnum.Eight, "8" },
        { RankEnum.Seven, "7" },
    };

    public string id;
    
    public SuitEnum Suit { get { return _suit; } set { _suit = value; } }
    public RankEnum Rank { get { return _rank; } set { _rank = value; } }


    public void Init(SuitEnum suit, RankEnum rank, Vector3 location)
    {
        transform.position = location;
        transform.localScale = new Vector3(1.39f, 1.39f, 1);
        id = string.Format("Card_{0}_{1}", suit, rank);
        gameObject.name = id;

        _back = transform.Find("Back").gameObject;
        _front = transform.Find("Front").gameObject;

        _suit = suit;
        _rank = rank;

        UpdateRank();
        UpdateSuit();
    }

    public void UpdateRank()
    {
        GameObject rankObject = _front.transform.Find("Rank Canvas/Rank").gameObject;
        Text rank = rankObject.GetComponent<Text>();
        rank.text = Ranks[_rank];
        if (_suit == SuitEnum.Diamonds || _suit == SuitEnum.Hearts)
        {
            rank.color = new Color(0.79f, 0.16f, 0.11f);
        }
    }

    public void UpdateSuit()
    {
        GameObject suitObject = _front.transform.Find("Suit").gameObject;
        SpriteRenderer renderer = suitObject.GetComponent<SpriteRenderer>();
        switch (_suit)
        {
            case SuitEnum.Hearts:
                renderer.sprite = hearts;
                break;
            case SuitEnum.Diamonds:
                renderer.sprite = diamonds;
                break;
            case SuitEnum.Spades:
                renderer.sprite = spades;
                break;
            case SuitEnum.Clubs:
                renderer.sprite = clubs;
                break;
        }
    }
}