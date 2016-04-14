using System;

namespace CrossHairOverlay
{
	public class MouseMoveArgs : MouseMessageArgsBase
	{
		public MouseMoveArgs(IntPtr message, MouseHookLowLevelStruct data) : base(message, data)
		{
		}
	}
}