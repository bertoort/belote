using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTrump : MonoBehaviour
{
    public Sprite hearts;
    public Sprite clubs;
    public Sprite diamonds;
    public Sprite spades;

    public void Init(Vector3 position, SuitEnum suit)
    {
        transform.position = position;
        GameObject suitObject = transform.Find("Suit").gameObject;
        SpriteRenderer renderer = suitObject.GetComponent<SpriteRenderer>();
        switch (suit)
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
