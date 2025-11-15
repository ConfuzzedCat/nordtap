using System.Collections.Generic;
using Godot;

namespace Nordtap.GameComponents;

public class CardManager
{
    public PackedScene CardScene { get; set; }
    
    private readonly List<Card> _cards;
    public Card CurrentMovableCard { get; private set; }

    public CardManager()
    {
        _cards = [];
    }

    public void InstantiateCardUnderNode(Card card, Node parent)
    {
        var instantiate = CardScene.Instantiate<Card>();
        instantiate.Clone(card);
        instantiate.AddManager(this);
        parent.AddChild(instantiate);
    }
    
    public void AddManagerForCard(Card card)
    {
        _cards.Add(card);
    }

    public void SetCurrentMovableCard(Card card)
    {
        if (card is not null && card.CardManager != this)
        {
            // TODO: fail, wrong manager
            return;
        }
        CurrentMovableCard = card;
    }
    
    public void DeleteCard(Card card)
    {
        _cards.Remove(card);
    }
    
    public void DeleteAllCards()
    {
        _cards.Clear();
    }
}