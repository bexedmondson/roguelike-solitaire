using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class FoundationUI : TextureRect
{
    [Export]
    private Suit suit = Suit.Diamond;

    private System.Collections.Generic.List<Card> cards = new();

    public event Action OnFoundationUpdated;

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

        OnFoundationUpdated?.Invoke();
    }

    private List<Card> m_helperCardList = new();
    public bool IsReady()
    {
        m_helperCardList = cards;
        for (int i = 0; i < 13; i++)
        {
            var found = m_helperCardList.FirstOrDefault(card => card.level == i);
            if (found != default(Card))
            {
                m_helperCardList.Remove(found);
            }
            else
            {
                return false;
            }
        }

        return true;
    }
}