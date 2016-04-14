using System;

namespace CrossHairOverlay
{
	public abstract class MouseClickArgsBase : MouseMessageArgsBase
	{
		protected MouseClickArgsBase(IntPtr message, MouseHookLowLevelStruct data) : base(message, data)
		{
		}

		public virtual bool IsLeft => Message == (IntPtr) LowLevelMouseListener.WM_LBUTTONDOWN || Message == (IntPtr) LowLevelMouseListener.WM_LBUTTONUP;
		public virtual bool IsRight => Message == (IntPtr) LowLevelMouseListener.WM_RBUTTONDOWN || Message == (IntPtr) LowLevelMouseListener.WM_RBUTTONUP;
		public virtual bool IsMiddle => Message == (IntPtr) LowLevelMouseListener.WM_MBUTTONDOWN || Message == (IntPtr) LowLevelMouseListener.WM_MBUTTONUP;

		public virtual int XButtonId
		{
			get
			{
				if (Message != (IntPtr) LowLevelMouseListener.WM_XBUTTONDOWN && Message != (IntPtr) LowLevelMouseListener.WM_XBUTTONUP && Message != (IntPtr) LowLevelMouseListener.WM_XBUTTONDBLCLK)
				{
					return 0;
				}

				return Data.mouseData;
			}
		}
	}
}