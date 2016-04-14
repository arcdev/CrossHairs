using System;

namespace CrossHairOverlay
{
	public class MouseUpArgs : MouseClickArgsBase
	{
		public MouseUpArgs(IntPtr message, MouseHookLowLevelStruct data) : base(message, data)
		{
		}
	}
}