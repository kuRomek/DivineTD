using kuRomek.SimpleVG;

public class MainCameraPresenter : Presenter
{
    protected new MainCamera Model => base.Model as MainCamera;

    protected new MainCameraView View => base.View as MainCameraView;

    public MainCameraPresenter(View view, Model model) : base(view, model)
    {
    }
}
