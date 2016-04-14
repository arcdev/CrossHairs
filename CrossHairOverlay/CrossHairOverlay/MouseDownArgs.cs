using System;

namespace CrossHairOverlay
{
	public class MouseDownArgs : MouseClickArgsBase
	{
		public MouseDownArgs(IntPtr message, MouseHookLowLevelStruct data) : base(message, data)
		{
		}
	}
}