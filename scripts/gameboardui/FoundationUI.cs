using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class FoundationUI : CardStackUI
{
    [Export]
    private Suit suit = Suit.Diamond;
    public event Action OnFoundationUpdated;

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
        
        return card.level == stack.Count + 1;
    }

    public override void _DropData(Vector2 atPosition, Variant data) 
    {
        var dropData = (CardDrag)data.AsGodotObject();
        stack.cards.Add(dropData.cards[0]); //TODO worth doing extra checks? or are the checks in CanDropData enough. hmm

        dropData.DoCardDrop(stack);

        Texture = GD.Load<Texture2D>($"res://textures/cards/{suit.ToString().ToLower()}_{stack.cards[stack.cards.Count - 1].level}.tres");

        OnFoundationUpdated?.Invoke();
    }

    private List<Card> m_helperCardList = new();
    public bool IsReady()
    {
        m_helperCardList = stack.cards;
        //card levels start at 1:
        for (int i = 1; i <= 13; i++)
        {
            var found = m_helperCardList.FirstOrDefault(card => card.level == i);
            if (found != default(Card))
            {
                m_helperCardList.Remove(found);
            }
            else
            {
                GD.Print(this.suit.Name() + " not ready, missing card " + i);
                return false;
            }
        }

        return true;
    }
}