/// <summary>
/// An interface for classes that handle input. To be utilized in creating CPUs.
/// </summary>
public interface IInputHandler
{
    void Possess(IAvatar target);
    void Free(bool destroy_avatar);
}
