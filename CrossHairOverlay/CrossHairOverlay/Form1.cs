using System;
using System.Drawing;
using System.Windows.Forms;

namespace CrossHairOverlay
{
	public partial class Form1 : Form
	{
		//TODO: make this configurable
		private const int Gap = 4;
		
		//TODO: make this configurable
		private readonly Pen _pen = new Pen(Color.LightBlue, 1);

		//TODO: add some shortcut keys??


		private readonly LowLevelMouseListener _mouseListener = new LowLevelMouseListener();

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			ShowInTaskbar = false; // if you want to see it on the taskbar, set this to true

			//todo: consider adding a function to set the previous window/app active

			// note: only covers the primary monitor... for now
			//todo: add support for multiple monitors
			var bounds = Screen.GetBounds(new Point(0, 0));
			Width = bounds.Width;
			Height = bounds.Height;

			Left = 0;
			Top = 0;

			_mouseListener.OnMouseMove += GlobalMouseMoved;
			_mouseListener.HookMouse();
		}

		~Form1()
		{
			_mouseListener.UnHookMouse();
		}


		private void GlobalMouseMoved(object sender, MouseMoveArgs e)
		{
			//todo: remove this caption update
			var caption = "mouse moved @ " + DateTime.Now.Ticks;
			Text = caption;
			InvalidateOldLines();
		}

		private void InvalidateOldLines()
		{
			var x = MousePosition.X;
			var y = MousePosition.Y;
			var point = new Point(x, y);
			var bounds = Screen.GetBounds(point);

			// reduce the gap by 1 pixel to ensure the content is cleared without leaving any trace
			var localgap = Gap - 1;

			// might need to adjust the origin 
			var above = new Rectangle(0, 0, bounds.Width, y - localgap);
			Invalidate(above);

			var below = new Rectangle(0, y + localgap, bounds.Width, bounds.Height);
			Invalidate(below);

			var left = new Rectangle(0, 0, x - localgap, bounds.Height);
			Invalidate(left);

			var right = new Rectangle(x + localgap, 0, bounds.Width, bounds.Height);
			Invalidate(right);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			var x = MousePosition.X;
			var y = MousePosition.Y;

			//TODO: we can probably remove this, but shows how we *could* display a control with some data
			// hovering over everything else
			label1.Text = $"x={x:##0} y={y:##0}";
			DrawLines(e.Graphics);
		}

		private void DrawLines(Graphics g)
		{
			//todo: support multiple screens

			var point = MousePosition;
			var x = point.X;
			var y = point.Y;
			var bounds = Screen.GetBounds(point);

			// invalidate everything ABOVE the horizontal line
			// essentially wipes the old vertical line that was on TOP, or ABOVE, the cursor
			g.DrawLine(_pen, x, 0, x, y - Gap);
			// invalidate everything BELOW the horizontal line
			// essentially wipes the old vertical line that was UNDER, or BELOW, the cursor
			g.DrawLine(_pen, x, y + Gap, x, bounds.Height);


			// invalidate everything to the LEFT of the vertical line
			// essentially wipes the old horizontal line that was to the LEFT of the cursor
			g.DrawLine(_pen, 0, y, x - Gap, y);
			// invalidate everything to the RIGHT of the vertical line
			// essentially wipes the old horizontal line that was to the RIGHT of the cursor
			g.DrawLine(_pen, x + Gap, y, bounds.Width, y);
		}
	}
}