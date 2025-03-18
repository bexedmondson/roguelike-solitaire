using System;
using Godot;

public class BoosterShuffleStack : AbstractBooster<BoosterShuffleStackData>
{
    public override void DoBoosterEffect()
    {
        //GD.Print("do booster effect: shuffle stack");
        //data.cardStackUI.SetProcess(false);
        //data.cardStackUI.cardContainer.SetProcess(false);

        data.previousCardState = data.stack.cards.ToArray();
        data.stack.Shuffle();
        data.newCardState = data.stack.cards.ToArray();
        //GD.Print("do booster effect: shuffle stack complete");

        DoVisualEffect();
    }

    private void DoVisualEffect()
    {
        GD.Print("visual start");
        var cardUIs = data.cardStackUI.cardUIs;
        var positions = new Vector2[cardUIs.Length];
        for (int i = 0; i < cardUIs.Length; i++)
        {
            positions[i] = cardUIs[i].Position;
        }

        data.cardStackUI.tempCardContainer.Size = data.cardStackUI.cardContainer.Size;
        foreach (CardUI cardUI in data.cardStackUI.cardUIs)
        {
            data.cardStackUI.cardContainer.RemoveChild(cardUI);
            data.cardStackUI.tempCardContainer.AddChild(cardUI);
        }

        foreach (var cardUI in cardUIs)
        {
            var initialPositionIndex = Array.IndexOf(data.previousCardState, cardUI.card);
            var initialPosition = cardUIs[initialPositionIndex].Position;
            cardUI.SetPosition(initialPosition);
        }

        var cardTweener = data.cardStackUI.CreateTween();

        cardTweener.SetParallel(true);
        var centrePosition = positions[0] / 2;

        foreach (var cardUI in cardUIs)
        {
            cardTweener.TweenProperty(cardUI, "position", centrePosition, 0.5f).SetEase(Tween.EaseType.In);
        }

        cardTweener.SetParallel(false);
        cardTweener.TweenInterval(0.7);
        cardTweener.SetParallel(true);

        int t = 0;
        foreach (var cardUI in cardUIs)
        {
            var finalPositionIndex = Array.IndexOf(data.newCardState, cardUI.card);
            var finalPosition = cardUIs[finalPositionIndex].Position;

            cardTweener.TweenProperty(cardUI, "position", finalPosition, 0.3f).SetEase(Tween.EaseType.Out).SetDelay(t * 0.1f);
            t++;
        }

        cardTweener.Finished += OnTweenFinished;
        cardTweener.Play();
    }

    private void OnTweenFinished()
    {
        foreach (CardUI cardUI in data.cardStackUI.cardUIs)
        {
            data.cardStackUI.tempCardContainer.RemoveChild(cardUI);
            data.cardStackUI.cardContainer.AddChild(cardUI);
        }
        CompleteEffect();
    }
}

public class BoosterShuffleStackData : AbstractBoosterData
{
    public Stack stack;
    public CardStackUI cardStackUI;

    public Card[] previousCardState;
    public Card[] newCardState;
}