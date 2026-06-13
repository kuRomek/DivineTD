using kuRomek.SimpleVG;

public class Checkpoint : GridObject
{
    public Checkpoint(View view, Model model, GridSystem gridSystem) : base(view, model, gridSystem)
    {
        Model.NumberChanged += View.DisplayNumber;
    }

    public new CheckpointModel Model => base.Model as CheckpointModel;
    public new CheckpointView View => base.View as CheckpointView;
}
