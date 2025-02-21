using Godot;

public partial class CardUI : Control
{
    [Export]
    public TextureRect textureRect;

    [Export]
    public PackedScene stackContainerScene;

    public Card card;

    private CardStackUI cardStackUI;

    public void SetStack(CardStackUI cardStackUI)
    {
        this.cardStackUI = cardStackUI;
    }
 
   /* public override void _GuiInput(InputEvent @event)
    {
        if (@event.IsReleased())
        {
            cardStackUI._GuiInput(@event);
        }
    }*/

    bool mouseOn = false;

    public void ClickStart()
    {
        //GD.Print("mouse on");
        mouseOn = true;
    }

    public void MouseExit()
    {
        //GD.Print("mouse on");
        mouseOn = false;
    }

    public void ClickEnd()
    {
        //GD.Print("click end");
        if (mouseOn && cardStackUI is CardDeckUI cardDeckUI)
        {
            mouseOn = false;
            cardDeckUI.OnClick();
        }
    }

    public override Variant _GetDragData(Vector2 atPosition) 
    {
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