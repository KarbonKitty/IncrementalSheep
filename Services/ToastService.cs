using System.Diagnostics.CodeAnalysis;
using System.Timers;
using Timer = System.Timers.Timer;

namespace IncrementalSheep;

public sealed class ToastService : IToastService, IDisposable
{
    public event Action<string>? OnShow;
    public event Action? OnHide;
    private Timer? Countdown;

    public void Dispose() => Countdown?.Dispose();

    public void ShowToast(string message)
    {
        OnShow?.Invoke(message);
        StartCountdown();
    }

    private void StartCountdown()
    {
        SetCountdown();

        Countdown.Stop();
        Countdown.Start();
    }

    [MemberNotNull(nameof(Countdown))]
    private void SetCountdown()
    {
        if (Countdown is null)
        {
            Countdown = new Timer(5000);
            Countdown.Elapsed += HideToast;
            Countdown.AutoReset = false;
        }
    }

    private void HideToast(object? source, ElapsedEventArgs args)
    {
        OnHide?.Invoke();
    }
}
