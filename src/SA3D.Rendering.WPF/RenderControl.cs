using OpenTK.Windowing.Common;
using OpenTK.Wpf;
using SA3D.Rendering.Input;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace SA3D.Rendering.WPF
{
	/// <summary>
	/// WPF control for updating and rendering a <see cref="RenderContext"/>
	/// </summary>
	public class RenderControl : GLWpfControl
	{
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetCursorPos(int X, int Y);


		private bool _mouseLocked;

		private Vector2 _center;

		private readonly RenderContext _context;

		/// <summary>
		/// Creates a new graphics control for a render context.
		/// </summary>
		/// <param name="context">The render context to use</param>
		public RenderControl(RenderContext context) : base()
		{
			_context = context;

			context.Input.OnSetCursorPosition += (v2) =>
			{
				if(!_mouseLocked)
				{
					Point p = ToScreenPos(v2);
					SetCursorPos((int)p.X, (int)p.Y);
				}
			};

			context.Input.OnSetMouselock += (v) =>
			{
				_mouseLocked = v;
				Mouse.OverrideCursor = v ? Cursors.None : null;
			};

			Focusable = true;

			Loaded += (o, e) =>
			{
				_center = new(FrameBufferWidth * 0.5f, FrameBufferHeight * 0.5f);
				_context.Viewport = new(FrameBufferWidth, FrameBufferHeight);
			};

			Ready += _context.Initialize;

			Render += (time) =>
			{
				_context.Update(IsFocused, time.TotalSeconds);

				if(_mouseLocked && IsFocused)
				{
					Point p = ToScreenPos(_center);
					SetCursorPos((int)p.X, (int)p.Y);
				}

				context.Render(Framebuffer);
			};


			GLWpfControlSettings settings = new()
			{
				ContextFlags = ContextFlags.ForwardCompatible,
				MajorVersion = 4,
				MinorVersion = 6
			};

			Start(settings);
		}

		/// <inheritdoc/>
		protected override void OnRenderSizeChanged(SizeChangedInfo info)
		{
			base.OnRenderSizeChanged(info);
			_center = new(FrameBufferWidth * 0.5f, FrameBufferHeight * 0.5f);
			_context.Viewport = new(FrameBufferWidth, FrameBufferHeight);
		}



		#region Input handling

		/// <inheritdoc/>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			_context.Input.SetInput((InputCode)e.Key, true);
		}

		/// <inheritdoc/>
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			_context.Input.SetInput((InputCode)e.Key, false);
		}

		/// <inheritdoc/>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			Point pos = e.GetPosition(this);
			Vector2 posV2 = new((float)pos.X, (float)pos.Y);
			if(_mouseLocked)
			{
				_context.Input.SetCursorPos(posV2, _center);
			}
			else
			{
				_context.Input.SetCursorPos(posV2, null);
			}
		}
		/// <inheritdoc/>
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			base.OnMouseLeave(e);
			_context.Input.ClearInputs();
		}

		/// <inheritdoc/>
		protected override void OnMouseWheel(System.Windows.Input.MouseWheelEventArgs e)
		{
			base.OnMouseWheel(e);
			_context.Input.SetScroll(e.Delta / 120f);
		}

		/// <inheritdoc/>
		protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);
			if(!IsFocused)
			{
				Focus();
			}

			_context.Input.SetInput((InputCode)(-(int)e.ChangedButton - 1), true);
		}

		/// <inheritdoc/>
		protected override void OnMouseUp(System.Windows.Input.MouseButtonEventArgs e)
		{
			base.OnMouseUp(e);
			_context.Input.SetInput((InputCode)(-(int)e.ChangedButton - 1), false);
		}

		/// <inheritdoc/>
		private Point ToScreenPos(Vector2 relative)
		{
			return PointToScreen(new Point(relative.X, relative.Y));
		}
		
		#endregion
	}
}
