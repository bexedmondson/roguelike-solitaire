
using System;
using Godot;

public partial class Card : GodotObject
{
    public Tableau tableau;
    public Suit suit = (Suit)Enum.GetValues(typeof(Suit)).GetValue(new RandomNumberGenerator().RandiRange(0, 3));
    public int level = 1;
    public bool flipped = false;
    public string Name => suit.Name() + level;
}