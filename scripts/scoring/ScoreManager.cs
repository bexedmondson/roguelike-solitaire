
using System;
using Godot;

public class ScoreManager : IInjectable
{
    public event Action<int> OnScoreChanged;

    public int score { get; private set; }

    private const int foundationCardAddedScore = 10;
    private const int deckCardRemovedScore = 5;
    private const int stackCardFlippedScore = 5;

    private bool isTracking = false;

    public ScoreManager()
    {
        InjectionManager.Register(this);
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void SetScoreTracking(bool tracking)
    {
        isTracking = tracking;
    }

    public void OnCardMoved(Stack sourceStack, Stack targetStack)
    {
        if (!isTracking)
            return;
        
        if (targetStack is Foundation)
        {
            score += foundationCardAddedScore;
            GD.Print($"adding {foundationCardAddedScore} score due to foundation add");
            OnScoreChanged?.Invoke(foundationCardAddedScore);
        }
        
        if (sourceStack is DiscardPile)
        {
            score += deckCardRemovedScore;
            GD.Print($"adding {deckCardRemovedScore} score due to deck remove");
            OnScoreChanged?.Invoke(deckCardRemovedScore);
        }
    }

    public void OnCardFlipped(Stack sourceStack)
    {
        if (!isTracking)
            return;
        
        if (sourceStack is not Foundation && sourceStack is not DiscardPile && sourceStack is not DrawPile)
        {
            score += stackCardFlippedScore;
            GD.Print($"adding {stackCardFlippedScore} score due to card flip");
            OnScoreChanged?.Invoke(stackCardFlippedScore);
        }
    }
}