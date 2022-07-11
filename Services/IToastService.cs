using System;

namespace IdleSheep
{
    public interface IToastService
    {
        event Action<string> OnShow;
        event Action OnHide;
        void ShowToast(string message);
    }
}
