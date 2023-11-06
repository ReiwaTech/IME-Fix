using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Game;
using Dalamud.Hooking;

namespace HappyIME
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "HappyIME";

        private delegate int GetIMEModeDelegate(nint self);
        private readonly Hook<GetIMEModeDelegate>? getIMEModeHook;

        public Plugin(
            [RequiredVersion("1.0")] ISigScanner sigScanner)
        {
            nint address = sigScanner.ScanText("E8 ?? ?? ?? ?? 8B D0 EB 0D F6 D3");
            getIMEModeHook = Hook<GetIMEModeDelegate>.FromAddress(address, this.DetourGetIMEMode);
            getIMEModeHook.Enable();
        }

        private int DetourGetIMEMode(nint self)
        {
            var result = getIMEModeHook!.Original(self);
            return result == 3 ? 0 : result;
        }

        public void Dispose()
        {
            getIMEModeHook?.Dispose();
        }
    }
}
