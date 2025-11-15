using Godot;
using System;
using Nordtap.GameComponents;

public partial class CardSpawner : Button
{

    [Export]
    public PackedScene CardScene { get; set; }
    
    private CardManager _cardManger;

    public CardSpawner()
    {
        _cardManger = new CardManager();
    }


    public override void _Ready()
    {
        _cardManger.CardScene = this.CardScene;
    }

    public override void _Pressed()
    {
        GD.Print("pressed");
        Card card = new Card();
        card.CardImageTexturePath =
            "https://cards.scryfall.io/large/front/c/8/c84ea0fd-efc7-4614-9f8f-41a3c71fceaa.jpg?1696636503";
        _cardManger.InstantiateCardUnderNode(card, GetParent());
    }
}
