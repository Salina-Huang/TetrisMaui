#if WINDOWS10_0_19041_0
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;

namespace TetrisMaui
{
    public static partial class Platform
    {
        public static Window? CurrentWindow =>
            Application.Current?.Windows?.FirstOrDefault();
    }
}
#endif