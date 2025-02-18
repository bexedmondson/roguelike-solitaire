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

    private System.Collections.Generic.Stack<Card> cards = new();

    private List<CardUI> cardTextureRects = new();

    public override void _Ready() 
    {
        //temp for testing
        cards.Push(new Card());


        if (cards.Count < 1)
        {
            return;
        }

        UpdateCardVisuals();
    }

    public override Variant _GetDragData(Vector2 atPosition) 
    {
        if (cards.Count == 0)
            return default; //don't love this, but it's a godot issue so no great alternative. tracked here: https://github.com/godotengine/godot/issues/78507

        //TextureRect p = new() { Texture = Texture, ExpandMode = ExpandModeEnum.IgnoreSize, StretchMode = StretchModeEnum.KeepAspect };
        var preview = (Container)cardContainer.Duplicate();
        preview.CustomMinimumSize = Vector2.One * 64;
        preview.SetAnchorsPreset(LayoutPreset.TopRight);
        
        SetDragPreview(preview);
        return new CardDrag(){sourceStack = this, cards = new Godot.Collections.Array<Card>(cards)};
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        if (data.VariantType != Variant.Type.Object)
            return false;

        var dropData = (CardDrag)data.AsGodotObject();

        if (dropData.cards.Count > 1)
            return false;

        if (cards.Count == 0)
        {
            return dropData.cards[0].level == 13;
        }

        Card currentEndCard = cards.Peek();
        
        var card = dropData.cards[0];
        if (!card.suit.CanStackOnSuit(currentEndCard.suit))
            return false;
        
        return card.level == currentEndCard.level - 1;
    }

    public override void _DropData(Vector2 atPosition, Variant data) 
    {
        var dropData = (CardDrag)data.AsGodotObject();
        Card newCard = dropData.cards[0];
        cards.Push(newCard); //TODO worth doing extra checks? or are the checks in CanDropData enough. hmm

        dropData.OnDrop(this);

        UpdateCardVisuals();
    }

    public void RemoveCards(Godot.Collections.Array<Card> cardsToRemove)
    {
        GD.Print($"removing cards from stack: {cardsToRemove.Count}");
        for (int i = cardsToRemove.Count - 1; i >= 0; i--)
        {
            Card card = cardsToRemove[i];
            if (cards.Peek() != card)
            {
                GD.PushError("trying to pop card from stack that isn't at the end?? stopping here");
                break;
            }
            cards.Pop();
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

        foreach (Card card in cardList)
        {
            GD.Print($"making card {card.suit.Name()} {card.level}");
            var cardControl = cardTextureRectScene.Instantiate<CardUI>();
            var textureRect = cardControl.textureRect;
            textureRect.Texture = GD.Load<Texture2D>($"res://textures/cards/{card.suit.Name()}_{card.level}.tres");
            cardContainer.AddChild(cardControl);
            cardTextureRects.Add(cardControl);
        }
    }
}