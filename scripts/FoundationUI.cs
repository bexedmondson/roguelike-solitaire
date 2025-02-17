using Godot;
using System;

public partial class FoundationUI : TextureRect
{
    [Export]
    private Suit suit = Suit.Diamond;

    private System.Collections.Generic.List<Card> cards = new();

    public override void _Ready() 
    {
        if (cards.Count < 1)
        {
            return;
        }

        Texture = GD.Load<Texture2D>($"res://textures/cards/{cards[0].suit.ToString().ToLower()}_{cards[cards.Count - 1].level}.tres");
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        if (data.VariantType != Variant.Type.Object)
            return false;

        var dropData = (CardDrag)data.AsGodotObject();

        if (dropData.cards.Count > 1)
            return false;

        var card = dropData.cards[0];
        if (card.suit != suit)
            return false;
        
        return card.level == cards.Count + 1;
    }

    public override void _DropData(Vector2 atPosition, Variant data) 
    {
        var dropData = (CardDrag)data.AsGodotObject();
        cards.Add(dropData.cards[0]); //TODO worth doing extra checks? or are the checks in CanDropData enough. hmm

        dropData.OnDrop(this);

        Texture = GD.Load<Texture2D>($"res://textures/cards/{suit.ToString().ToLower()}_{cards[cards.Count - 1].level}.tres");
    }
}