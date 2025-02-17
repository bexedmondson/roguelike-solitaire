
using System;
using Godot;

public partial class Card : GodotObject
{
    public Suit suit = (Suit)Enum.GetValues(typeof(Suit)).GetValue(new RandomNumberGenerator().RandiRange(0, 3));
    public int level = new RandomNumberGenerator().RandiRange(1, 13);
}