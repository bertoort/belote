using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Dealer : MonoBehaviour
{
    [SerializeField]
    private GameObject _deckPrefab;
    [SerializeField]
    private GameObject _buttonPrefab;

    private GameObject _startButton;
    private GameObject _canvas;
    private GameObject _world;
    private Deck _deck;
    private bool _started;

    void Start()
    {
        AddStartButton();
        AddDeck();
    }

    void Update()
    {
        Button button = _startButton.GetComponent<Button>();
        button.onClick.AddListener(StartGame);
    }

    public void DealCards()
    {
        // face down
        _deck.DealCard(new Vector3(-400, 200, 200f), false);
        _deck.DealCard(new Vector3(-200, 200, 200f), false);
        _deck.DealCard(new Vector3(0, 200, 200f), false);
        _deck.DealCard(new Vector3(200, 200, 200f), false);

        _deck.DealCard(new Vector3(-400, -200, 200f), false);
        _deck.DealCard(new Vector3(-200, -200, 200f), false);
        _deck.DealCard(new Vector3(0, -200, 200f), false);
        _deck.DealCard(new Vector3(200, -200, 200f), false);

        // face up
        _deck.DealCard(new Vector3(-450, 250, 150f), true);
        _deck.DealCard(new Vector3(-250, 250, 150f), true);
        _deck.DealCard(new Vector3(-50, 250, 150f), true);
        _deck.DealCard(new Vector3(150, 250, 150f), true);

        _deck.DealCard(new Vector3(-450, -250, 150f), true);
        _deck.DealCard(new Vector3(-250, -250, 150f), true);
        _deck.DealCard(new Vector3(-50, -250, 150f), true);
        _deck.DealCard(new Vector3(150, -250, 150f), true);

        // kitty
        _deck.DealCard(new Vector3(550, 50, 205f), false);
        _deck.DealCard(new Vector3(600, 0, 200f), false);
        _deck.DealCard(new Vector3(650, -50, 195f), false);
    }

    public void StartGame()
    {
        if (!_started)
        {
            _started = true;
            _startButton.SetActive(false);
            DealCards();
        }
    }

    public void AddStartButton()
    {
        _world = GameObject.Find("World Canvas");
        _startButton = Instantiate(_buttonPrefab);
        _startButton.transform.position = new Vector3(0, 0, 300);
        _startButton.transform.SetParent(_world.transform);
    }

    public void AddDeck()
    {
        _canvas = GameObject.Find("Screen Space Canvas");
        GameObject deckInstance = Instantiate(_deckPrefab);
        _deck = deckInstance.GetComponent<Deck>();
        _deck.transform.SetParent(_canvas.transform);
        _deck.Init(new Vector3(650, -350, 300));
    }
}
