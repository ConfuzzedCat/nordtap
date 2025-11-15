using System;
using Godot;
using Nordtap.GameManager;

namespace Nordtap.GameComponents;

public partial class PlayerBoard : Node
{
    [Export]
    public PackedScene CardScene { get; set; }
    
    private IServiceProvider  _serviceProvider;
    private CardManager _cardManger;
    

    public PlayerBoard()
    {
        _serviceProvider = ServiceProviderManager.GetServiceProvider();
        _cardManger = new CardManager();
    }

    public override void _Ready()
    {
        _cardManger.CardScene = this.CardScene;
    }

    public void AddCardFromName(string cardName)
    {
        
    }
    
}