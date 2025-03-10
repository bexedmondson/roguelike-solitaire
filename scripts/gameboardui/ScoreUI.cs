
using Godot;

public partial class ScoreUI : Label
{
    private ScoreManager scoreManager;

    public override void _EnterTree()
    {
        base._EnterTree();
        scoreManager = InjectionManager.Get<ScoreManager>();
        scoreManager.OnScoreChanged += OnScoreChanged;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        scoreManager.OnScoreChanged -= OnScoreChanged;
        scoreManager = null;
    }

    private void OnScoreChanged(int scoreDelta)
    {
        Text = scoreManager.score.ToString();
    }
}