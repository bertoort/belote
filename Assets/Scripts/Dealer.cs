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

    private string screenCanvas = "Screen Space Canvas";
    private string worldCanvas = "World Canvas";

    private GameObject startButton;
    private GameObject canvas;
    private GameObject world;
    private Deck deck;
    private bool started;

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

        // limits
        //deck.DealCard(new Vector3(0,0, 300), false, CardHand.PlayerOne);
        //deck.DealCard(new Vector3(screenBounds.x * -1, screenBounds.y * -1, 300), false, CardHand.PlayerOne);
        //deck.DealCard(new Vector3(screenBounds.x * -1, screenBounds.y, 300), false, CardHand.PlayerOne);
        //deck.DealCard(new Vector3(screenBounds.x, screenBounds.y * -1, 300), false, CardHand.PlayerOne);
        //deck.DealCard(new Vector3(screenBounds.x, screenBounds.y, 300), false, CardHand.PlayerOne);


        deck.Hide();
    }

    public void StartGame()
    {
        if (!started)
        {
            started = true;
            startButton.SetActive(false);
            DealCards();
        }
    }

    public void AddStartButton()
    {
        world = GameObject.Find(worldCanvas);
        startButton = Instantiate(buttonPrefab);
        startButton.transform.position = new Vector3(0, 0, 300);
        startButton.transform.SetParent(world.transform);
    }

    public void AddDeck()
    {
        canvas = GameObject.Find(screenCanvas);
        GameObject deckInstance = Instantiate(deckPrefab);
        deck = deckInstance.GetComponent<Deck>();
        deck.transform.SetParent(canvas.transform);
        float sizeRatio = scaleRatio * sizeAdjustment;
        deck.Init(new Vector3(screenBounds.x - 200, (screenBounds.y * -1) + 200, 300), sizeRatio);
    }
}
