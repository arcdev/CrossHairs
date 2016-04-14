using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
namespace CrossHairOverlay
{
	// baseded on: http://www.dylansweb.com/2014/10/low-level-global-keyboard-hook-sink-in-c-net/

	public class LowLevelMouseListener
	{
		public const int WH_KEYBOARD_LL = 13;
		public const int WM_KEYDOWN = 0x0100;
		public const int WM_SYSKEYDOWN = 0x0104;

		public const int WH_MOUSE_LL = 14;
		public const uint WM_MOUSEMOVE = 0x200;
		public const uint WM_LBUTTONDOWN = 0x201;
		public const uint WM_LBUTTONUP = 0x202;
		public const uint WM_LBUTTONDBLCLK = 0x203;
		public const uint WM_RBUTTONDOWN = 0x204;
		public const uint WM_RBUTTONUP = 0x205;
		public const uint WM_RBUTTONDBLCLK = 0x206;
		public const uint WM_MBUTTONDOWN = 0x207;
		public const uint WM_MBUTTONUP = 0x208;
		public const uint WM_MBUTTONDBLCLK = 0x209;
		public const uint WM_MOUSEWHEEL = 0x20A;
		public const uint WM_MOUSEHWHEEL = 0x20E;

		public const uint WM_XBUTTONDBLCLK = 0x020D;
		public const uint WM_XBUTTONDOWN = 0x020B;
		public const uint WM_XBUTTONUP = 0x020C;


		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool UnhookWindowsHookEx(IntPtr hhk);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		public delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

		public event EventHandler<MouseMoveArgs> OnMouseMove;
		public event EventHandler<MouseDownArgs> OnMouseDown;
		public event EventHandler<MouseUpArgs> OnMouseUp;

		private readonly LowLevelMouseProc _proc;
		private IntPtr _hookId = IntPtr.Zero;

		public LowLevelMouseListener()
		{
			_proc = HookCallback;
		}

		public void HookMouse()
		{
			_hookId = SetHook(_proc);
		}

		public void UnHookMouse()
		{
			UnhookWindowsHookEx(_hookId);
		}

		private static IntPtr SetHook(LowLevelMouseProc proc)
		{
			using (var curProcess = Process.GetCurrentProcess())
			using (var curModule = curProcess.MainModule)
			{
				return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
			}
		}

		private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode < 0)
			{
				return CallNextHookEx(_hookId, nCode, wParam, lParam);
			}

			var hookData = (MouseHookLowLevelStruct) Marshal.PtrToStructure(lParam, typeof (MouseHookLowLevelStruct));
			if (wParam == (IntPtr) WM_MOUSEMOVE)
			{
				OnMouseMove?.Invoke(this, new MouseMoveArgs(wParam, hookData));
			}
			else if (wParam == (IntPtr) WM_LBUTTONDOWN || wParam == (IntPtr) WM_RBUTTONDOWN || wParam == (IntPtr) WM_MBUTTONDOWN || wParam == (IntPtr) WM_XBUTTONDOWN)
			{
				OnMouseDown?.Invoke(this, new MouseDownArgs(wParam, hookData));
			}
			else if (wParam == (IntPtr) WM_LBUTTONUP || wParam == (IntPtr) WM_RBUTTONUP || wParam == (IntPtr) WM_MBUTTONUP || wParam == (IntPtr) WM_XBUTTONUP)
			{
				OnMouseUp?.Invoke(this, new MouseUpArgs(wParam, hookData));
			}

			return CallNextHookEx(_hookId, nCode, wParam, lParam);
		}
	}
}