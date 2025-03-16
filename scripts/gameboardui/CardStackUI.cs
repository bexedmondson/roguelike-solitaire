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

        cardContainer.ChildEnteredTree += OnContainerChildAdded;
        //cardContainer.ChildOrderChanged += OnContainerChildAdded;
    }

    public override void _ExitTree()
    {
        GameDebug.OnGameDebugToggled -= OnDebugToggled;

        cardContainer.ChildEnteredTree -= OnContainerChildAdded;
        //cardContainer.ChildOrderChanged -= OnContainerChildAdded;
        this.stack.OnStackUpdated -= UpdateCardVisuals;
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
        var dragCards = stack.GetDraggableStackSectionFromSelectedCard(selectedCardUI.card);

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
        //GD.PushWarning("start update card visuals " + this.Name);
        if (debugLabel != null)
            debugLabel.Text = stack != null ? stack.Count.ToString() : "0";

        List<Card> cardsWithUI = new();

        for (int i = cardTextureRects.Count - 1; i >= 0; i--)
        {
            var cardTextureRect = cardTextureRects[i];
            Card textureRectCard = cardTextureRect.card;
            if (!stack.cards.Contains(textureRectCard))
            {
                cardTextureRect.QueueFree();
                cardTextureRects.RemoveAt(i);
            }
            else
            {
                cardTextureRect.UpdateCardFlipState();
                cardsWithUI.Add(textureRectCard);
            }
        }
        //cardTextureRects.Clear();

        if (stack.cards.Count != cardsWithUI.Count)
        {
            foreach (Card card in stack.cards)
            {
                //GD.Print(card.Name);
                if (cardsWithUI.Contains(card))
                    continue;

                var cardControl = cardTextureRectScene.Instantiate<CardUI>();
                cardControl.Setup(card, this);
                cardContainer.AddChild(cardControl);
                cardTextureRects.Add(cardControl);
            }
        }

        UpdateSortOrder();

        //GD.PushWarning("end update card visuals " + this.Name);
    }

    private void UpdateSortOrder()
    {
        //GD.PushWarning("start update sort order " + this.Name);
        if (!GetAllCardUIsReady())
            return;

        var containerChildren = cardContainer.GetChildren();
        foreach (var containerChild in containerChildren)
        {
            var childCardUI = containerChild as CardUI;
            int index = containerChildren.Count - stack.cards.IndexOf(childCardUI.card) - 1;
            //GD.Print($"moving {containerChild.Name} from position {containerChild.GetIndex()} to {index}");
            cardContainer.MoveChild(containerChild, index);
        }
        
        //GD.PushWarning("end update sort order " + this.Name);
    }

    private void OnContainerChildAdded(Node addedNode)
    {
        //GD.PushWarning($"start {nameof(OnContainerChildAdded)} {this.Name}");
        if (cardContainer.GetChildCount() == 0)
            return;

        var addedCardUI = addedNode as CardUI;
        addedCardUI.ItemRectChanged += () => OnCardReadyUpdateSeparation(addedCardUI);

        //GD.PushWarning($"end {nameof(OnContainerChildAdded)} {this.Name}");
    }

    private void OnCardReadyUpdateSeparation(CardUI cardUI)
    {
        if (!GetAllCardUIsReady())
            return;

        UpdateSortOrder();

        //GD.PushWarning("start oncardready update separation " + this.Name + cardUI.card.Name);
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
        //GD.PushWarning("end oncardready update separation " + this.Name + cardUI.card.Name);
    }

    private bool GetAllCardUIsReady()
    {
        foreach (var containerChild in cardContainer.GetChildren())
        {
            //wait for children to all be ready before continuing
            if (!containerChild.IsNodeReady())
            {
                GD.Print("child node not ready " + containerChild.Name);
                return false;
            }
        }
        return true;
    }
}