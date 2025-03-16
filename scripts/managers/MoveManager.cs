

using System.Collections.Generic;
using Godot;

public class MoveManager : IInjectable
{
    private class Move
    {
        public Godot.Collections.Array<Card> movedCards = new();
        public Stack sourceStack;
        public Stack targetStack;
    }

    private Move lastMove = null;

    public bool IsMoveInProgress {get; private set;}

    public MoveManager()
    {
        InjectionManager.Register(this);
        GameDebug.OnGameDebugToggled += OnDebugToggled;
    }

    public void OnMovePerformed(Stack sourceStack, Stack targetStack, Godot.Collections.Array<Card> movedCards)
    {
        lastMove = new Move();
        lastMove.sourceStack = sourceStack;
        lastMove.targetStack = targetStack;
        lastMove.movedCards = movedCards;

        //GD.Print("move started");
        //IsMoveInProgress = true;
        
        //var sceneAccessor = InjectionManager.Get<CurrentSceneAccessor>();
        //sceneAccessor.CurrentSceneTree.Root. += OnTreeReady;
    }

    private void OnTreeReady()
    {
        //GD.Print("move ended");
        IsMoveInProgress = false;
    }

    private void OnDebugToggled()
    {
        if (!GameDebug.On)
            return;
        
        GD.Print("---MoveManager---");
        GD.Print("move in progress? " + IsMoveInProgress);

        GD.Print("last move:");
        if (lastMove != null)
        {
            GD.Print("  source: " + lastMove.sourceStack);
            GD.Print("  target: " + lastMove.targetStack);
            foreach (Card card in lastMove.movedCards)
            {
                GD.Print("    " + card.Name);
            }
        }
        else
        {
            GD.Print("  null");
        }
    }
}