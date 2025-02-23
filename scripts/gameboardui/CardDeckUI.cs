using Godot;

public partial class CardDeckUI : CardStackUI
{
    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        return false;
    }

    public void OnClick()
    {
        //GD.Print("deck input");
        if (cards.Count == 0)
            return;
        
        GD.Print("deck before:");

        foreach (Card card in cards)
        {
            GD.Print($"  {card.suit.Name()}{card.level}");
        }

        var temp = cards[^1];
        cards.RemoveAt(cards.Count - 1);
        cards.Insert(0, temp);

        UpdateCardVisuals();

        GD.Print("deck after:");

        foreach (Card card in cards)
        {
            GD.Print($"  {card.suit.Name()}{card.level}");
        }
    }
}