using Godot;

public partial class CardUI : Control
{
    [Export]
    public TextureRect textureRect;

    [Export]
    public PackedScene stackContainerScene;

    public Card card;

    private CardStackUI cardStackUI;

    public void SetStack(CardStackUI cardStackUI)
    {
        this.cardStackUI = cardStackUI;
    }

    public override Variant _GetDragData(Vector2 atPosition) 
    {
        var stackContainerPreview = stackContainerScene.Instantiate<Container>(); 
        stackContainerPreview.CustomMinimumSize = Vector2.One * 64;
        stackContainerPreview.SetAnchorsPreset(LayoutPreset.TopRight);
        
        SetDragPreview(stackContainerPreview);

        return cardStackUI.GetDragData(this);
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        return cardStackUI._CanDropData(atPosition, data);
    }

    public override void _DropData(Vector2 atPosition, Variant data) 
    {
        cardStackUI._DropData(atPosition, data);
    }
}