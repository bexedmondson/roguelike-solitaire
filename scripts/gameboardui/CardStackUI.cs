using System.Collections.Generic;
using Godot;

public partial class CardStackUI : TextureRect
{
    [Export]
    private Texture2D emptyTexture;

    [Export]
    private Container cardContainer;

    [Export]
    private bool forceCardsFullOverlap = false;

    [Export]
    private PackedScene cardTextureRectScene;

    [Export]
    private BoosterStackUI boosterStackUI;

    [Export]
    private Label debugLabel;

    public virtual bool CanDrag => true;

    public Stack stack { get; protected set; }

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
        boosterStackUI?.SetStack(stack);

        UpdateCardVisuals();
        this.stack.OnStackUpdated += UpdateCardVisuals;
    }

    public virtual CardDrag GetDragData(CardUI selectedCardUI)
    {
        //TODO return empty if booster active
        var dragCards = stack.GetStackSectionFromSelectedCard(selectedCardUI.card);

        foreach (CardUI cardUI in cardTextureRects)
        {
            if (dragCards.Contains(cardUI.card))
                cardUI.Visible = false;
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
            var cardControl = cardTextureRectScene.Instantiate<CardUI>();
            cardControl.Setup(card, this);
            
            if (card.flipped)
                cardControl.textureRect.Texture = GD.Load<Texture2D>($"res://textures/cards/{card.suit.Name()}_{card.level}.tres");
            else
                cardControl.textureRect.Texture = GD.Load<Texture2D>($"res://textures/cards/card_back.tres");
            
            cardControl.MouseFilter = card.flipped ? MouseFilterEnum.Stop : MouseFilterEnum.Pass;
            cardContainer.AddChild(cardControl);
            cardContainer.MoveChild(cardControl, 0);
            cardTextureRects.Add(cardControl);
        }

        cardContainer.ChildEnteredTree += SetSeparation;
    }

    private void SetSeparation(Node node)
    {
        cardContainer.ChildEnteredTree -= SetSeparation;

        var childControl = node as CardUI;
        childControl.ItemRectChanged += () => OnCardReady(childControl);
    }

    private void OnCardReady(CardUI cardUI)
    {
        int separation = 0;
        if (forceCardsFullOverlap)
        {
            //GD.Print("child size in " + this.Name + " is " + cardUI.Size + ", " + cardUI.Name);
            //GD.Print(Name + " separation to " + -cardUI.Size.Y );
            separation = Mathf.RoundToInt( -cardUI.Size.Y );
        }
        else
        {
            //GD.Print("child size in " + this.Name + " is " + cardUI.Size + ", " + cardUI.Name);
            double adjustedSize = -cardUI.Size.Y * 0.75;
            //GD.Print(Name + " separation to " + adjustedSize);
            separation = Mathf.RoundToInt( adjustedSize );
        }

        cardContainer.AddThemeConstantOverride("separation", separation);
    }
}