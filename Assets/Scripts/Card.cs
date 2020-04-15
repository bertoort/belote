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
    private GameObject front;
    private GameObject back;
    [SerializeField]
    private bool faceUp = false;
    private Vector3 target;

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
        float stopDistance = 1f;
        
        if (Vector3.Distance(transform.position, target) > stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, smooth * Time.deltaTime);
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

        if (faceUp && transform.rotation.y < 0f)
        {
            Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);

        }
        else if (!faceUp && transform.rotation.y > -1f)
        {
            Quaternion target = Quaternion.Euler(tiltAroundX, 180, tiltAroundZ);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);

        }
        if (transform.rotation.y < -0.7f && !back.gameObject.activeSelf)
        {
            back.SetActive(true);
            front.SetActive(false);
        } else if (transform.rotation.y > -0.7f && !front.gameObject.activeSelf)
        {
            back.SetActive(false);
            front.SetActive(true);
        }
    }

    public void Init(SuitEnum suit, RankEnum rank, Vector3 location, Vector3 scale)
    {
        transform.position = location;
        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(0, -180, 0);
        id = string.Format("Card{0}{1}", suit, rank);
        gameObject.name = id;

        back = transform.Find("Back").gameObject;
        front = transform.Find("Front").gameObject;

        Suit = suit;
        Rank = rank;

        front.SetActive(false);

        UpdateRank();
        UpdateSuit();
    }

    public void UpdateRank()
    {
        GameObject rankObject = front.transform.Find("Rank Canvas/Rank").gameObject;
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
        GameObject suitObject = front.transform.Find("Suit").gameObject;
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
        faceUp = !faceUp;
    }

    public void Move(Vector3 newTarget)
    {
        target = newTarget;
    }

    public void Move(Vector3 newTarget, bool flip)
    {
        target = newTarget;
        if (flip)
        {
            Flip();
        }
    }

    public void Move(Vector3 start, Vector3 newTarget, bool flip)
    {
        transform.position = start;
        target = newTarget;
        if (flip)
        {
            Flip();
        }
    }
}