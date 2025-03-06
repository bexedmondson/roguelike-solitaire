using System.Collections.Generic;
using Godot;

public partial class CardStackUI : TextureRect
{
    [Export]
    private Texture2D emptyTexture;

    [Export]
    private Container cardContainer;

    [Export]
    private PackedScene cardTextureRectScene;

    [Export]
    private Label debugLabel;

    public virtual bool CanDrag => true;

    protected Stack stack;

    private List<CardUI> cardTextureRects = new();

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

    public void InitialiseWithStack(Stack stack)
    {
        this.stack = stack;
        UpdateCardVisuals();
        this.stack.OnStackUpdated += UpdateCardVisuals;
    }

    public virtual CardDrag GetDragData(CardUI selectedCardUI)
    {
        var dragCards = stack.GetStackSectionFromSelectedCard(selectedCardUI.card);

        //GD.Print($"drag count {dragCards.Count} {dragCards[0].suit}{dragCards[0].level}");
        //if (dragCards.Count > 1)
            //GD.Print($"  {dragCards[1].suit}{dragCards[1].level}");

        if (cardTextureRects == null || cardTextureRects.Count == 0)
        {
            //GD.PrintErr("stack empty!");
        }

        foreach (CardUI cardUI in cardTextureRects)
        {
            //if (cardUI == null)
                //GD.PrintErr("card null!");
            if (dragCards.Contains(cardUI.card))
            {
                cardUI.Visible = false;
            }
        }

        var cardDrag = new CardDrag();
        cardDrag.InitialiseCardDrag(stack, dragCards);

        return cardDrag;
    }

    public override void _Notification(int notification)
    {
        if (notification == NotificationDragEnd && !IsDragSuccessful())
        {
            foreach (CardUI cardUI in cardTextureRects)
            {
                cardUI.Visible = true;
            }
        }
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        if (data.VariantType != Variant.Type.Object)
            return false;

        var dropData = (CardDrag)data.AsGodotObject();

        return stack.CanDropCards(dropData.cards);
    }

    public override void _DropData(Vector2 atPosition, Variant data) 
    {
        var dropData = (CardDrag)data.AsGodotObject();
        dropData.DoCardDrop(stack);
    }

    protected void UpdateCardVisuals()
    {
        if (debugLabel != null)
            debugLabel.Text = stack != null ? stack.Count.ToString() : "0";

        foreach (CardUI cardTextureRect in cardTextureRects)
        {
            cardTextureRect.QueueFree();
        }
        cardTextureRects.Clear();

        if (stack.IsEmpty)
            return;

        foreach (Card card in stack.cards)
        {
            //GD.Print($"making card {card.Name}");
            var cardControl = cardTextureRectScene.Instantiate<CardUI>();
            cardControl.card = card;
            cardControl.SetStack(this);
            var textureRect = cardControl.textureRect;
            //GD.Print($"trying to get texture {card.suit.Name()}_{card.level}");
            if (!card.flipped)
                textureRect.Texture = GD.Load<Texture2D>($"res://textures/cards/card_back.tres");
    //        else
    //            textureRect.Texture = GD.Load<Texture2D>($"res://textures/cards/{card.suit.Name()}_{card.level}.tres");
            
            var cardScene = GD.Load<PackedScene>($"res://scenes/card_faces/{card.level}/{card.Name}.tscn");
            GD.Print($"res://scenes/card_faces/{card.level}/{card.Name}.tscn");
            var cardInstance = cardScene.Instantiate();
            cardControl.cardSceneContainer.AddChild(cardInstance);
            cardControl.cardSceneContainer.MoveChild(cardInstance, 0);

            cardControl.MouseFilter = card.flipped ? MouseFilterEnum.Stop : MouseFilterEnum.Pass;
            cardContainer.AddChild(cardControl);
            cardContainer.MoveChild(cardControl, 0);
            cardTextureRects.Add(cardControl);
        }
    }
}