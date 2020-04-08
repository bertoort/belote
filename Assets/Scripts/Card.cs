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
    private bool _faceUp;

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

    private void Update()
    {
        UpdateFlip();
    }

    private void UpdateFlip()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            this.Flip();
        }
        float smooth = 5.0f;
        float tiltAngle = 60.0f;

        // Smoothly tilts a transform towards a target rotation.
        float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
        float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;
        Debug.Log(transform.rotation.y);
        if (_faceUp && transform.rotation.y < 0f)
        {
            Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);

        }
        else if (!_faceUp && transform.rotation.y > -1f)
        {
            Quaternion target = Quaternion.Euler(tiltAroundX, 180, tiltAroundZ);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);

        }
        if (transform.rotation.y < -0.7f && !_back.gameObject.activeSelf)
        {
            _back.SetActive(true);
            _front.SetActive(false);
        } else if (transform.rotation.y > -0.7f && !_front.gameObject.activeSelf)
        {
            _back.SetActive(false);
            _front.SetActive(true);
        }
    }

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

        _back.SetActive(false);

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
        if (_rank == RankEnum.Ten)
        {
            rank.transform.position += new Vector3(-13.5f, 0, 0);
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

    public void Flip()
    {
        _faceUp = !_faceUp;
    }
}