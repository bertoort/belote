using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Dealer : MonoBehaviour
{
    [SerializeField]
    private GameObject deckPrefab;
    [SerializeField]
    private GameObject buttonPrefab;


    public Camera MainCamera;
    private Vector2 screenBounds;
    private float scaleRatio;
    private float spaceAdjustment = 0.6f;
    private float sizeAdjustment = 0.8f;

    private string screenCanvas = "GameCanvas";
    private string worldCanvas = "FrontCanvas";

    private GameObject startButton;
    private GameObject canvas;
    private GameObject front;
    private Deck deck;
    private bool started;

    public GameObject playerOnePrefab;
    private Dashboard playerOneDashboard;
    public GameObject playerTwoPrefab;
    private Dashboard playerTwoDashboard;

    public GameObject trumpSelectionPrefab;
    public GameObject currentTrumpPrefab;
    private GameObject trumpSelection;
    private GameObject currentTrump;

    float kittyDistance;
    float topPlayerY;
    float lowerPlayerY;
    float overCardDistance;

    void Start()
    {
        screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
        scaleRatio = (screenBounds.x / screenBounds.y);

        AddStartButton();
        AddDeck();
    }

    void Update()
    {
        Button button = startButton.GetComponent<Button>();
        button.onClick.AddListener(StartGame);
        if (Input.GetKeyDown(KeyCode.K))
        {
            deck.FlipKitty(kittyDistance);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            deck.RemovePlayerOneCards(new Vector3(0, screenBounds.y + 200, 0));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            deck.RemovePlayerTwoCards(new Vector3(0, (screenBounds.y * -1) - 200, 0));
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            deck.MoveKittyY(topPlayerY + overCardDistance, true);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            deck.MoveKittyY(lowerPlayerY + overCardDistance, false);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            ShowTrumpSelection();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ShowPlayerOneCards();
        }
    }

    public void DealCards()
    {
        float shrinkRatio = scaleRatio * spaceAdjustment;
    
        topPlayerY = 200 * shrinkRatio;
        lowerPlayerY = -200 * shrinkRatio;
        overCardDistance = 50 * shrinkRatio;

        float cardX = 200 * shrinkRatio;
        float cardZ = 200 * shrinkRatio;
        float nextCardDistance = 200 * shrinkRatio;
        int handedCards = 4;

        for (int i = 0; i < handedCards; i++)
        {
            // top player down           
            deck.DealCard(new Vector3(cardX - (i * nextCardDistance), topPlayerY, cardZ), false, CardHand.PlayerOne);
            // top player up
            deck.DealCard(new Vector3(cardX - (i * nextCardDistance) - overCardDistance, topPlayerY + overCardDistance, cardZ - overCardDistance), true, CardHand.PlayerOne);
            // bottom player down
            deck.DealCard(new Vector3(cardX - (i * nextCardDistance), lowerPlayerY, cardZ), false, CardHand.PlayerTwo);
            // bottom player up
            deck.DealCard(new Vector3(cardX - (i * nextCardDistance) - overCardDistance, lowerPlayerY + overCardDistance, cardZ - overCardDistance), true, CardHand.PlayerTwo);
        }

        int kittyCards = 3;
        kittyDistance = 50 * shrinkRatio;

        float kittyX = 400 * shrinkRatio;
        float kittyDepth = 195 * shrinkRatio;
        float kittyDepthChange = 5 * shrinkRatio;

        for (int i = 0; i < kittyCards; i++)
        {
            // kitty
            deck.DealCard(new Vector3(kittyX + (i * kittyDistance), kittyDistance * (i - 1), kittyDepth + (i * kittyDepthChange)), false, CardHand.Kitty);
        }

        deck.Hide();
    }

    public void DealHands()
    {
        int handedCards = 11;
        for (int i = 0; i < handedCards; i++)
        {
            deck.DealCard(new Vector3(0, 0, 0), false, CardHand.PlayerOne);
            deck.DealCard(new Vector3(0, 0, 0), false, CardHand.PlayerTwo);
        }
        deck.HidePlayerCards();
        deck.Hide();
    }

    public void DisplayDashboard()
    {
        GameObject playerOne = Instantiate(playerOnePrefab);
        playerOneDashboard = playerOne.GetComponent<Dashboard>();
        playerOneDashboard.Init();

        GameObject playerTwo = Instantiate(playerTwoPrefab);
        playerTwoDashboard = playerTwo.GetComponent<Dashboard>();
        playerTwoDashboard.Init();

        float boardX = 400;
        float boardY = 50;
        playerOne.transform.position = new Vector3(screenBounds.x * -1 + boardX, screenBounds.y * -1 + boardY, 300);
        playerTwo.transform.position = new Vector3(screenBounds.x * -1 + boardX, screenBounds.y - boardY, 300);

        playerOne.transform.SetParent(front.transform);
        playerTwo.transform.SetParent(front.transform);
    }

    public void StartGame()
    {
        if (!started)
        {
            started = true;
            startButton.SetActive(false);
            //DealCards();
            DealHands();
            DisplayDashboard();
        }
    }

    public void AddStartButton()
    {
        front = GameObject.Find(worldCanvas);
        startButton = Instantiate(buttonPrefab);
        startButton.transform.position = new Vector3(0, 0, 300);
        startButton.transform.SetParent(front.transform);
    }

    public void AddDeck()
    {
        canvas = GameObject.Find(screenCanvas);
        GameObject deckInstance = Instantiate(deckPrefab);
        deck = deckInstance.GetComponent<Deck>();
        deck.transform.SetParent(canvas.transform);
        float sizeRatio = scaleRatio * sizeAdjustment;
        deck.Init(new Vector3(screenBounds.x - 200, (screenBounds.y * -1) + 200, 300), sizeRatio, screenCanvas);
    }

    public void ShowTrumpSelection()
    {
        trumpSelection = Instantiate(trumpSelectionPrefab);
        trumpSelection.transform.SetParent(front.transform);

        Trump trump = trumpSelection.GetComponent<Trump>();
        trump.Init(SetSpadeTrump, SetHeartsTrump, SetDiamondsTrump, SetClubsTrump);
    }

    public void SetSpadeTrump()
    {
        SetTrump(SuitEnum.Spades);
    }
    public void SetHeartsTrump()
    {
        SetTrump(SuitEnum.Hearts);
    }
    public void SetDiamondsTrump()
    {
        SetTrump(SuitEnum.Diamonds);
    }
    public void SetClubsTrump()
    {
        SetTrump(SuitEnum.Clubs);
    }

    public void SetTrump(SuitEnum suit)
    {
        trumpSelection.gameObject.SetActive(false);
        currentTrump = Instantiate(currentTrumpPrefab);
        currentTrump.transform.SetParent(front.transform);

        currentTrump.GetComponent<CurrentTrump>().Init(new Vector3(screenBounds.x - 100, screenBounds.y - 50, 300), suit);
    }

    public void ShowPlayerOneCards()
    {
        float shrinkRatio = scaleRatio * spaceAdjustment;
        deck.ShowPLayerOneCards(shrinkRatio);
    }
}
