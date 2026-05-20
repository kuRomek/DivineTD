using kuRomek.SimpleVG;

public class MainCameraView : View
{
    protected override Presenter CreatePresenter()
    {
        return new MainCameraPresenter(this, new MainCamera(transform));
    }
}
