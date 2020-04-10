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
    private GameObject _front;
    private GameObject _back;
    [SerializeField]
    private bool _faceUp = false;
    private Vector3 _target;

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

    public SuitEnum Suit { get; set; }
    public RankEnum Rank { get; set; }

    private void Update()
    {
        bool change = UpdatePosition();
        if (!change)
        {
            UpdateFlip();
        }
    }

    private bool UpdatePosition()
    {
        float smooth = 1000.0f;
        float stopDistance = 10f;
        
        if (Vector3.Distance(transform.position, _target) > stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target, smooth * Time.deltaTime);
            return true;
        }
        return false;
    }

    private void UpdateFlip()
    {
        float smooth = 5.0f;
        float tiltAngle = 60.0f;

        float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
        float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;

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
        Debug.Log(transform.rotation.y);
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
        transform.rotation = Quaternion.Euler(0, -180, 0);
        id = string.Format("Card_{0}_{1}", suit, rank);
        gameObject.name = id;

        _back = transform.Find("Back").gameObject;
        _front = transform.Find("Front").gameObject;

        Suit = suit;
        Rank = rank;

        _front.SetActive(false);

        UpdateRank();
        UpdateSuit();
    }

    public void UpdateRank()
    {
        GameObject rankObject = _front.transform.Find("Rank Canvas/Rank").gameObject;
        Text rank = rankObject.GetComponent<Text>();
        rank.text = Ranks[Rank];
        if (Suit == SuitEnum.Diamonds || Suit == SuitEnum.Hearts)
        {
            rank.color = new Color(0.79f, 0.16f, 0.11f);
        }
        if (Rank == RankEnum.Ten)
        {
            rank.transform.position += new Vector3(13.5f, 0, 0);
        }
    }

    public void UpdateSuit()
    {
        GameObject suitObject = _front.transform.Find("Suit").gameObject;
        SpriteRenderer renderer = suitObject.GetComponent<SpriteRenderer>();
        switch (Suit)
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

    public void Move(Vector3 target)
    {
        _target = target;
    }

    public void Move(Vector3 target, bool flip)
    {
        if (flip)
        {
            Flip();
        }
        _target = target;
    }

    public void Move(Vector3 start, Vector3 target, bool flip)
    {
        if (flip)
        {
            Flip();
        }
        transform.position = start;
        _target = target;
    }
}