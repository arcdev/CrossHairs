using System;

namespace CrossHairOverlay
{
	public abstract class MouseMessageArgsBase : EventArgs
	{
		public IntPtr Message { get; }
		public MouseHookLowLevelStruct Data { get; }

		protected MouseMessageArgsBase(IntPtr message, MouseHookLowLevelStruct data)
		{
			Message = message;
			Data = data;
		}
	}
}