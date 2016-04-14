using System.Runtime.InteropServices;

namespace CrossHairOverlay
{
	// MSLLHOOKSTRUCT https://msdn.microsoft.com/en-us/library/windows/desktop/ms644970(v=vs.85).aspx
	[StructLayout(LayoutKind.Sequential)]
	public class MouseHookLowLevelStruct
	{
		public POINT pt;
		public int mouseData;
		public int flags;
		public int time;
		public int dwExtraInfo;
	}
}