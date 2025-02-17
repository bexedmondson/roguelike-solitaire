public static class SuitExtensions
{
    public static bool CanStackOnSuit(this Suit suit, Suit otherSuit)
    {
        if (otherSuit == Suit.Diamond || otherSuit == Suit.Heart)
            return suit == Suit.Club || suit == Suit.Spade;
        else
            return suit == Suit.Diamond || suit == Suit.Heart;
    }

    public static string Name(this Suit suit)
    {
        return suit.ToString().ToLower();
    }
}