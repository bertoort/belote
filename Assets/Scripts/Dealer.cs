using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    [SerializeField]
    private GameObject _deckPrefab;
    private GameObject _canvas;
    private Deck _deck;

    void Start()
    {
        AddDeck();
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            DealCards();
        }
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

    public void AddDeck()
    {
        _canvas = GameObject.Find("Screen Space Canvas");
        GameObject deckInstance = Instantiate(_deckPrefab);
        _deck = deckInstance.GetComponent<Deck>();
        _deck.transform.SetParent(_canvas.transform);
        _deck.Init(new Vector3(650, -350, 300));
    }
}
