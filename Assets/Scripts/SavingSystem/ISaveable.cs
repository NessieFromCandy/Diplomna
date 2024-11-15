public interface ISaveable
{
    string saveIdentifier { get; }

    object CaptureState();

    void RestoreState(object state);
}