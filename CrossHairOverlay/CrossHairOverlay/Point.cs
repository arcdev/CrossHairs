using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace CrossHairOverlay
{
	[StructLayout(LayoutKind.Sequential)]
	public class POINT
	{
		public int x;
		public int y;
	}
}