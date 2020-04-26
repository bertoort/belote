using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dashboard : MonoBehaviour
{
    private bool hasAction = false;

    private Text scoreText;
    private Text bidText;
    private Text actionText;

    private GameObject bidBoard;
    private GameObject leftArrow;
    private GameObject rightArrow;
    private GameObject actionButton;

    private int score = 0;
    private int bid = 60;
    private int bidIncrements = 5;
    private int bidMin = 50;
    private int bidMax = 210;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && hasAction)
        {
            actionButton.SetActive(true);
            leftArrow.SetActive(true);
            rightArrow.SetActive(true);
        }
    }

    public void Init()
    {
        GameObject scoreObject = transform.Find("ScoreBoard/Score").gameObject;
        scoreText = scoreObject.GetComponent<Text>();
        scoreText.text = score.ToString();

        GameObject bidObject = transform.Find("BidBoard/Score").gameObject;
        bidText = bidObject.GetComponent<Text>();
        bidText.text = bid.ToString();

        Transform actionButtonTransform = transform.Find("ActionButton");
        if (actionButtonTransform != null)
        {
            hasAction = true;
            actionButton = actionButtonTransform.gameObject;
            actionText = actionButton.GetComponent<Text>();
            actionButton.SetActive(false);
            Button button = actionButton.GetComponent<Button>();
            button.onClick.AddListener(Action);
        }

        Transform leftArrowTransform = transform.Find("BidBoard/LeftArrow");
        if (leftArrowTransform != null)
        {
            leftArrow = leftArrowTransform.gameObject;
            leftArrow.SetActive(false);
            Button button = leftArrow.GetComponent<Button>();
            button.onClick.AddListener(SubtractBid);
        }
        Transform rightArrowTransform = transform.Find("BidBoard/RightArrow");
        if (rightArrowTransform != null)
        {
            rightArrow = rightArrowTransform.gameObject;
            rightArrow.SetActive(false);
            Button button = rightArrow.GetComponent<Button>();
            button.onClick.AddListener(AddBid);
        }
    }

    private void SubtractBid()
    {
        int newBid = bid - bidIncrements;
        if (newBid >= bidMin)
        {
            SetBid(newBid);
        }
    }

    private void AddBid()
    {
        int newBid = bid + bidIncrements;
        if (newBid <= bidMax)
        {
            SetBid(newBid);
        }
    }

    private void Action()
    {
        Debug.Log("clicking action");
    }

    public void SetScore(int newScore)
    {
        score = newScore;
        scoreText.text = score.ToString();
    }

    public void SetBid(int newBid)
    {
        bid = newBid;
        bidText.text = bid.ToString();
    }

    public void SetActionText(string text)
    {
        actionText.text = text;
    }
}
