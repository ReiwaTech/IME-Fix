using Dalamud.Plugin;
using Dalamud.Game;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;

namespace HappyIME
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "HappyIME";

        private delegate int GetIMEModeDelegate(nint self);
        private readonly Hook<GetIMEModeDelegate>? getIMEModeHook;
        public Plugin(
            ISigScanner sigScanner,
            IGameInteropProvider gameInteropProvider)
        {
            getIMEModeHook = gameInteropProvider.HookFromSignature<GetIMEModeDelegate>("E8 ?? ?? ?? ?? 8B D0 EB 0D F6 D3", this.DetourGetIMEMode);
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
