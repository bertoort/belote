using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
}

public class Card : MonoBehaviour
{
    [SerializeField]
    private SuitEnum _suit;
    [SerializeField]
    private RankEnum _rank;
    [SerializeField]
    private string _id;
    
    public SuitEnum Suit { get { return _suit; } set { _suit = value; } }
    public RankEnum Rank { get { return _rank; } set { _rank = value; } }


    public void Init(SuitEnum suit, RankEnum rank)
    {
        _id = string.Format("Card_{0}_{1}", suit, rank);
        gameObject.name = _id;
        _suit = suit;
        _rank = rank;
    }
}