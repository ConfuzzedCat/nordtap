using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Godot;

namespace Nordtap.GameComponents;

public partial class Card : RigidBody2D
{
	[Export]
	public string CardImageTexturePath { get; set; }

	public CardManager CardManager { get; set; }
	
	
	private Sprite2D _cardImage;
	private CollisionShape2D _collision;
	private HttpRequest _imageFetcher;
	
	private bool _hovering;
	private bool _dragging;
	private Vector2 _startMousePosition;

	public Card(string cardImageTexturePath = "")
	{
		CardImageTexturePath = cardImageTexturePath;
	}

	public Card() { }
	
	public override void _Ready()
	{
		
		_cardImage = GetNode<Sprite2D>("./PanelContainer/CardImage");
		_collision = GetNode<CollisionShape2D>("./Collision");
		_imageFetcher = GetNode<HttpRequest>("./ImageFetcher");
		
		LoadTexture(CardImageTexturePath);
	}

	private void LoadTexture(string cardImageTexturePath)
	{
		if (string.IsNullOrWhiteSpace(cardImageTexturePath) == false)
		{
			if (cardImageTexturePath.StartsWith("http://") ||  cardImageTexturePath.StartsWith("https://"))
			{
				_imageFetcher.RequestCompleted += HttpRequestCompleted;
				Error error = _imageFetcher.Request(cardImageTexturePath);
				if (error == Error.Ok)
				{
					return;
				}
				GD.PushError("An error occurred in the HTTP request.");
			}
		}
		_cardImage.Texture = ImageTexture.CreateFromImage(Image.LoadFromFile("res://Assets/Dev/ody-227-volley-of-boulders.jpg"));
	}

	private void HttpRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		if (result != (long)HttpRequest.Result.Success)
		{
			GD.PushError("Image couldn't be downloaded. Try a different image.");
			return;
		}
		var image = new Image();
		Error error = image.LoadJpgFromBuffer(body);
		if (error != Error.Ok)
		{
			GD.PushError("Couldn't load the image. Tried JPG format.");
			error = image.LoadPngFromBuffer(body);
			if (error != Error.Ok)
			{
				GD.PushError("Couldn't load the image. Tried PNG format.");
				return;
			}
		}

		_cardImage.Texture = ImageTexture.CreateFromImage(image);
	}

	public override void _Process(double delta)
	{
		if (_hovering || _dragging)
		{
			if (Input.IsActionJustPressed("left_mouse_button"))
			{
				_startMousePosition = GetLocalMousePosition();
				CardManager.SetCurrentMovableCard(this);
			}
			
			if (Input.IsActionPressed("left_mouse_button") 
			    && CardManager.CurrentMovableCard != null 
			    && CardManager.CurrentMovableCard == this)
			{ 
				var globalPosition = GetGlobalMousePosition();
				var resultPosition = globalPosition - _startMousePosition; 
				Position = resultPosition;
				_dragging = true;
			}
			
			if (Input.IsActionJustReleased("left_mouse_button"))
			{
				_dragging = false;
				_startMousePosition = Vector2.Zero;
				CardManager.SetCurrentMovableCard(null);
			}
		}
	}

	public override void _MouseEnter()
	{
		GD.Print("Mouse enter");
		_hovering = true;
	}
	
	public override void _MouseExit()
	{
		GD.Print("Mouse exit");
		_hovering = false;
	}

	public void Clone(Card other)
	{
		this.CardImageTexturePath = other.CardImageTexturePath;
		this._cardImage = other._cardImage;
		this._collision = other._collision;
		this._imageFetcher = other._imageFetcher;
		this._hovering = other._hovering;
		this._dragging = other._dragging;
		this._startMousePosition = other._startMousePosition;
		this.CardManager = other.CardManager;
	}

	public void AddManager(CardManager cardManager)
	{
		this.CardManager = cardManager;
		CardManager.AddManagerForCard(this);
	}
}
