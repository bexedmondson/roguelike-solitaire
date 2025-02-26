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

    public virtual bool CanDrag => true;

    protected System.Collections.Generic.List<Card> cards = new();

    private List<CardUI> cardTextureRects = new();

    public void SetCards(List<Card> cards)
    {
        this.cards = cards;
        UpdateCardVisuals();
    }

    public virtual CardDrag GetDragData(CardUI selectedCard)
    {
        //GD.Print($"selectedcard {selectedCard.card.suit}{selectedCard.card.level}");
        //GD.Print($"index {cards.IndexOf(selectedCard.card)}, lastindex {cards.Count - 1}");
        int selectedCardIndex = cards.IndexOf(selectedCard.card);
        var dragCards = cards.GetRange(0, selectedCardIndex+1);//, cards.Count - selectedCardIndex);

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

        return new CardDrag(){sourceStack = this, cards = new Godot.Collections.Array<Card>(dragCards)};
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
        GD.Print(data);
        if (data.VariantType != Variant.Type.Object)
            return false;

        var dropData = (CardDrag)data.AsGodotObject();

        if (cards.Count == 0)
        {
            return dropData.cards[^1].level == 13;
        }

        Card currentEndCard = cards[0];
        GD.Print($"current stack end card {currentEndCard.suit} {currentEndCard.level}");
        
        var card = dropData.cards[^1];
        GD.Print($"current drop card {card.suit} {card.level}");
        if (!card.suit.CanStackOnSuit(currentEndCard.suit))
            return false;
        
        return card.level == currentEndCard.level - 1;
    }

    public override void _DropData(Vector2 atPosition, Variant data) 
    {
        var dropData = (CardDrag)data.AsGodotObject();

        for (int i = dropData.cards.Count - 1; i >= 0; i--)
        {
            Card newCard = dropData.cards[i];
            cards.Insert(0, newCard); //TODO worth doing extra checks? or are the checks in CanDropData enough. hmm
        }

        dropData.OnDrop(this);

        UpdateCardVisuals();
    }

    public void RemoveCards(Godot.Collections.Array<Card> cardsToRemove)
    {
        //GD.Print($"removing cards from stack: {cardsToRemove.Count}, stack count {cards.Count}");
        for (int i = 0; i < cardsToRemove.Count; i++)
        {
            Card card = cardsToRemove[i];
            if (cards[0] != card)
            {
                GD.PushError("trying to pop card from stack that isn't at the end?? stopping here");
                break;
            }
            cards.Remove(card);
        }

        UpdateCardVisuals();
    }

    protected void UpdateCardVisuals()
    {
        foreach (CardUI cardTextureRect in cardTextureRects)
        {
            cardTextureRect.QueueFree();
        }
        cardTextureRects.Clear();

        if (cards.Count == 0)
            return;

        UpdateCardFlipStatus();

        foreach (Card card in cards)
        {
            //GD.Print($"making card {card.suit.Name()} {card.level}");
            var cardControl = cardTextureRectScene.Instantiate<CardUI>();
            cardControl.card = card;
            cardControl.SetStack(this);
            var textureRect = cardControl.textureRect;
            //GD.Print($"trying to get texture {card.suit.Name()}_{card.level}");
            if (card.flipped)
                textureRect.Texture = GD.Load<Texture2D>($"res://textures/cards/{card.suit.Name()}_{card.level}.tres");
            else
                textureRect.Texture = GD.Load<Texture2D>($"res://textures/cards/card_back.tres");
            
            cardControl.MouseFilter = card.flipped ? MouseFilterEnum.Stop : MouseFilterEnum.Pass;
            cardContainer.AddChild(cardControl);
            cardContainer.MoveChild(cardControl, 0);
            cardTextureRects.Add(cardControl);
        }
    }

    protected virtual void UpdateCardFlipStatus()
    {
        if (!cards[0].flipped)
            cards[0].flipped = true;
    }
}