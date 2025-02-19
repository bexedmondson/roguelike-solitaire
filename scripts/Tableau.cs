using System;
using System.Collections.Generic;
using Godot;

public class Tableau
{
    private int stackCount = 7;
    public List<List<Card>> stacks = new();

    public List<Card> deck = new();

    public Tableau()
    {
        for (int i = 0; i < stackCount; i++)
        {
            stacks.Add(new List<Card>());
        }

        for (int i = 1; i <= 13; i++)
        {
            deck.Add(new Card(){suit = Suit.Diamond, level = i});
            deck.Add(new Card(){suit = Suit.Heart, level = i});
            deck.Add(new Card(){suit = Suit.Club, level = i});
            deck.Add(new Card(){suit = Suit.Spade, level = i});
        }
    }

    public void Deal()
    {
        var rng = new RandomNumberGenerator();

        int n = deck.Count;  
        while (n > 1) 
        {  
            n--;  
            int k = rng.RandiRange(0, n + 1);  
            Card temp = deck[k];  
            deck[k] = deck[n];  
            deck[n] = temp;  
        }  

        for (int i = 0; i < stacks.Count; i++)
        {
            stacks[i] = new List<Card>();
            for (int j = 0; j < i + 1; j++)
            {
                stacks[i].Add(deck[0]);
                deck.RemoveAt(0);
            } 
        }
    }
}