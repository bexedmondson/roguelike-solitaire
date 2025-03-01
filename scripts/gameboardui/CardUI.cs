using System;
using Godot;

public partial class CardUI : Control
{
    [Export]
    public TextureRect textureRect;

    [Export]
    public PackedScene stackContainerScene;

    [Export]
    private Label debugLabel;

    //TODO change to autoproperty if/when godot moves to c#>13, .net>10
    private Card m_card;
    public Card card {
        get => m_card;
        set {
            m_card = value;
            if (debugLabel != null)
                debugLabel.Text = m_card.Name;
        }
    }

    private CardStackUI cardStackUI;

    public void SetStack(CardStackUI cardStackUI)
    {
        this.cardStackUI = cardStackUI;
    }

    public override void _EnterTree()
    {
        OnDebugToggled();
        GameDebug.OnGameDebugToggled += OnDebugToggled;
    }

    public override void _ExitTree()
    {
        GameDebug.OnGameDebugToggled -= OnDebugToggled;
    }

    private void OnDebugToggled()
    {
        if (debugLabel != null)
            debugLabel.Visible = GameDebug.On;
    }

    private bool mouseOn = false;
    private double timeSinceLastClick = 0;
    private const double clickThreshold = 0.5;

    public override void _Process(double delta)
    {
        if (!mouseOn && timeSinceLastClick < clickThreshold)
        {
            timeSinceLastClick += delta;
        }
    }

    public void ClickStart()
    {
        //GD.Print("mouseOn is " + timeSinceLastClick);
        //if it's been less than the threshold since the last click, this is a double click and we don't want to click again
        if (mouseOn || timeSinceLastClick < clickThreshold)
            return;

        //GD.Print("mouse on");
        mouseOn = true;
    }

    public void MouseExit()
    {
        //GD.Print("mouseExit is " + timeSinceLastClick);
        //GD.Print("mouse on");
        mouseOn = false;
        timeSinceLastClick = 0;
    }

    public void ClickEnd()
    {
        //GD.Print("clickend is " + timeSinceLastClick);
        //GD.Print("click end");
        if (mouseOn && timeSinceLastClick > clickThreshold && cardStackUI is DrawPileUI cardDeckUI)
        {
            mouseOn = false;
            timeSinceLastClick = 0;
            cardDeckUI.OnClick();
        }
    }

    public override Variant _GetDragData(Vector2 atPosition) 
    {
        if (!this.card.flipped)
        {
            return default(Variant);
        }

        var stackContainerPreview = stackContainerScene.Instantiate<Container>(); 
        stackContainerPreview.CustomMinimumSize = Vector2.One * 64;
        stackContainerPreview.SetAnchorsPreset(LayoutPreset.TopRight);

        var stackDragData = cardStackUI.GetDragData(this);
        foreach (var card in stackDragData.cards)
        {
            var newCardUI = (CardUI)this.Duplicate();
            newCardUI.Visible = true;
            newCardUI.textureRect.Texture = GD.Load<Texture2D>($"res://textures/cards/{card.suit.Name()}_{card.level}.tres");
            stackContainerPreview.AddChild(newCardUI);
        }
        
        SetDragPreview(stackContainerPreview);

        return stackDragData;
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        return cardStackUI._CanDropData(atPosition, data);
    }

    public override void _DropData(Vector2 atPosition, Variant data) 
    {
        cardStackUI._DropData(atPosition, data);
    }
}