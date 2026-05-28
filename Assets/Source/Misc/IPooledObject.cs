public interface IPooledObject
{
    void OnRelease();

    void OnGet();
}