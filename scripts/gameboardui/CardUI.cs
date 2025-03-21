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

    private Tableau tableau;
    private CardStackUI cardStackUI;

    public void Setup(Card card, CardStackUI cardStackUI)
    {
        this.card = card;
        this.cardStackUI = cardStackUI;
    }

    public override void _EnterTree()
    {
        tableau = InjectionManager.Get<TableauManager>().CurrentTableau;
        OnDebugToggled();
        GameDebug.OnGameDebugToggled += OnDebugToggled;
    }

    public override void _ExitTree()
    {
        tableau = null;
        GameDebug.OnGameDebugToggled -= OnDebugToggled;
    }

    private void OnDebugToggled()
    {
        if (debugLabel != null)
            debugLabel.Visible = GameDebug.On;
    }

    private bool mouseOn = false;
    private double timeSinceLastClick = clickThreshold;
    private const double clickThreshold = 0.5;

    public override void _Process(double delta)
    {
        if (!mouseOn)
            return;
        
        if (timeSinceLastClick < clickThreshold)
        {
            timeSinceLastClick += delta;
            //GD.Print(timeSinceLastClick);
        }
    }

    public void ClickStart()
    {
        //GD.Print("mouseOn time is " + timeSinceLastClick);
        //if it's been less than the threshold since the last click, this is a double click and we don't want to click again
        if (mouseOn)
            return;

        //GD.Print("mouse on");
        mouseOn = true;
    }

    public void MouseExit()
    {
        //GD.Print("mouseExit time is " + timeSinceLastClick);
        //GD.Print("mouse exit");
        mouseOn = false;
        timeSinceLastClick = clickThreshold;
    }

    public void ClickEnd()
    {
        //GD.Print("clickend time is " + timeSinceLastClick);
        //GD.Print("click end");

        if (!mouseOn) //mouse has moved off the card, so this is a drag and we shouldn't do anything with clicks here
            return;

        //GD.Print("time since last " + timeSinceLastClick);

        if (timeSinceLastClick >= clickThreshold)
        {
            //GD.Print("---------click");
            timeSinceLastClick = 0;

            if (cardStackUI is DrawPileUI cardDeckUI)
                cardDeckUI.OnClick();
        }
        else
        {
            //GD.Print("---------double click");
            timeSinceLastClick = 0;

            if (cardStackUI is not DrawPileUI && cardStackUI is not FoundationUI)
                OnCardDoubleClick();
        }
    }

    private void OnCardDoubleClick()
    {
        //GD.Print($"on card double click {card.Name}");
        tableau.TryAutoMoveCard(cardStackUI.stack, card);
    }

    public override Variant _GetDragData(Vector2 atPosition) 
    {
        if (!this.card.flipped)
        {
            return default(Variant);
        }

        float width = this.Size.X;
        var stackContainerPreview = stackContainerScene.Instantiate<StackDragUI>(); 
        stackContainerPreview.container.CustomMinimumSize = Vector2.One * width;
        stackContainerPreview.SetAnchorsPreset(LayoutPreset.TopRight);

        var stackDragData = cardStackUI.GetDragData(this);
        for (int i = stackDragData.cards.Count - 1; i >= 0; i--)
        {
            Card card = stackDragData.cards[i];
            var newCardUI = (CardUI)this.Duplicate();
            newCardUI.Visible = true;
            newCardUI.textureRect.Texture = GD.Load<Texture2D>($"res://textures/cards/{card.suit.Name()}_{card.level}.tres");
            newCardUI.OffsetLeft = -width / 2;
            newCardUI.OffsetRight = -width / 2;
            stackContainerPreview.container.AddChild(newCardUI);
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