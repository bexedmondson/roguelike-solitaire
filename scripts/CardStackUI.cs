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

    private System.Collections.Generic.List<Card> cards = new();

    private List<CardUI> cardTextureRects = new();

    public void SetCards(List<Card> cards)
    {
        this.cards = cards;
        UpdateCardVisuals();
    }

    public CardDrag GetDragData(CardUI selectedCard)
    {
        GD.Print($"selectedcard {selectedCard.card.suit}{selectedCard.card.level}");
        GD.Print($"index {cards.IndexOf(selectedCard.card)}, lastindex {cards.Count - 1}");
        int selectedCardIndex = cards.IndexOf(selectedCard.card);
        var dragCards = cards.GetRange(selectedCardIndex, cards.Count - selectedCardIndex);

        GD.Print($"drag count {dragCards.Count} {dragCards[0].suit}{dragCards[0].level}");
        if (dragCards.Count > 1)
            GD.Print($"  {dragCards[1].suit}{dragCards[1].level}");

        foreach (CardUI cardUI in cardTextureRects)
        {
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
        if (data.VariantType != Variant.Type.Object)
            return false;

        var dropData = (CardDrag)data.AsGodotObject();

        if (cards.Count == 0)
        {
            return dropData.cards[0].level == 13;
        }

        Card currentEndCard = cards[^1];
        GD.Print($"current stack end card {currentEndCard.suit} {currentEndCard.level}");
        
        var card = dropData.cards[0];
        GD.Print($"current drop card {card.suit} {card.level}");
        if (!card.suit.CanStackOnSuit(currentEndCard.suit))
            return false;
        
        return card.level == currentEndCard.level - 1;
    }

    public override void _DropData(Vector2 atPosition, Variant data) 
    {
        var dropData = (CardDrag)data.AsGodotObject();

        foreach (Card newCard in dropData.cards)
        {
            cards.Add(newCard); //TODO worth doing extra checks? or are the checks in CanDropData enough. hmm
        }

        dropData.OnDrop(this);

        UpdateCardVisuals();
    }

    public void RemoveCards(Godot.Collections.Array<Card> cardsToRemove)
    {
        GD.Print($"removing cards from stack: {cardsToRemove.Count}, stack count {cards.Count}");
        for (int i = cardsToRemove.Count - 1; i >= 0; i--)
        {
            Card card = cardsToRemove[i];
            if (cards[^1] != card)
            {
                GD.PushError("trying to pop card from stack that isn't at the end?? stopping here");
                break;
            }
            cards.Remove(card);
        }

        UpdateCardVisuals();
    }

    private void UpdateCardVisuals()
    {
        foreach (CardUI cardTextureRect in cardTextureRects)
        {
            cardTextureRect.QueueFree();
        }
        cardTextureRects.Clear();

        if (cards.Count == 0)
            return;
        
        var cardList = new List<Card>(cards);
        cardList.Reverse();

        for (int i = cardList.Count - 1; i >= 0; i--)
        {
            Card card = cardList[i];

            if (!card.flipped && i == 0)
                card.flipped = true;

            GD.Print($"making card {card.suit.Name()} {card.level}");
            var cardControl = cardTextureRectScene.Instantiate<CardUI>();
            cardControl.card = card;
            cardControl.SetStack(this);
            var textureRect = cardControl.textureRect;
            GD.Print($"trying to get texture {card.suit.Name()}_{card.level}");
            if (card.flipped)
                textureRect.Texture = GD.Load<Texture2D>($"res://textures/cards/{card.suit.Name()}_{card.level}.tres");
            else
                textureRect.Texture = GD.Load<Texture2D>($"res://textures/cards/card_back.tres");
            
            cardControl.MouseFilter = card.flipped ? MouseFilterEnum.Stop : MouseFilterEnum.Pass;
            cardContainer.AddChild(cardControl);
            cardTextureRects.Add(cardControl);
        }
    }
}