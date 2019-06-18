using Aiv.Fast2D;

public class Time
{
    public static float DeltaTime { get { return Time.window.deltaTime; } }

    private static Window window;

    public Time(Window window)
    {
        Time.window = window;
    }
}