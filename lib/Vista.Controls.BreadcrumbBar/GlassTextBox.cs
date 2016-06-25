using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Vista.Controls {
	public class GlassTextBox : TextBox {
		public GlassTextBox () {
			IsGlassSupported = Areo.IsGlassSupported;
			this.SetStyle ( 
				ControlStyles.OptimizedDoubleBuffer | 
				ControlStyles.Selectable | 
				ControlStyles.StandardClick, true );
			if ( IsGlassSupported ) {
				//we need to call this on every new thread to initialize the buffered painting
				Areo.BufferedPaintInit ();
			}
		}

		~GlassTextBox () {
			if ( IsGlassSupported ) {
				//on every thread you call BufferedPaintInit() it must be followed by a BufferedPaintUnInit() to clean up
				Areo.BufferedPaintUnInit ();
			}
		}

		internal bool IsMouseDown { get; set; }
		internal bool IsGlassSupported { get; set; }

		//dont paint the background to prevent flicker
		protected override void OnPaintBackground ( PaintEventArgs pevent ) {
			base.OnPaintBackground(pevent);
		}

		//override the default WindowProc and hijack the WM_PAINT command
		protected override void WndProc ( ref Message m ) {
			
			//let windows handle the message by default
			base.WndProc ( ref m );

			if ( IsGlassSupported ) {
				if ( m.Msg == Areo.WM_PAINT ) {
					//create a graphics object for this control
					Graphics gfx = this.CreateGraphics ();

					//obtain a handle to the native device contex
					IntPtr hdc = gfx.GetHdc ();

					//create a empty device context
					IntPtr BufferedDC = IntPtr.Zero;

					//Cast the ClientRectangle to a native RECT             
					Areo.RECT ClientRect = (Areo.RECT)ClientRectangle;

					//obtain the buffered device context from BeginBufferedPaint
					IntPtr BuffDCHandle = Areo.BeginBufferedPaint ( hdc, ref ClientRect, Areo.BP_BUFFERFORMAT.BPBF_TOPDOWNDIB, IntPtr.Zero, out BufferedDC );

					//paint the client to the buffered device context
					Areo.SendMessage ( Handle, Areo.WM_PRINTCLIENT, BufferedDC, Areo.PRF_CLIENT );

					//set the ALPHA level to fully opaque
					Areo.BufferedPaintSetAlpha ( BuffDCHandle, IntPtr.Zero, 255 );

					//end the buffered painting session
					Areo.EndBufferedPaint ( BuffDCHandle, true );

					//release the controls device context handle
					gfx.ReleaseHdc ( hdc );
				}
			}
		}

		//redraw the control when text is changed
		protected override void OnTextChanged ( EventArgs e ) {
			base.OnTextChanged ( e );
			if ( IsGlassSupported ) {
				Areo.InvalidateRect ( Handle, (Areo.RECT)ClientRectangle, false );
			}
		}

		//redraw the control When the mouse buttons are pressed
		protected override void OnMouseDown ( MouseEventArgs e ) {
			base.OnMouseDown ( e );
			IsMouseDown = true;
			if ( IsGlassSupported ) {
				Areo.InvalidateRect ( Handle, (Areo.RECT)ClientRectangle, false );
			}
		}

		protected override void OnMouseUp ( MouseEventArgs mevent ) {
			base.OnMouseUp ( mevent );
			IsMouseDown = false;
			if ( IsGlassSupported ) {
				Areo.InvalidateRect ( Handle, (Areo.RECT)ClientRectangle, false );
			}
		}

		//redraw when pressing the arrow keys
		protected override void OnKeyDown ( KeyEventArgs e ) {
			base.OnKeyDown ( e );
			if ( IsGlassSupported ) {
				if ( e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right )
					Areo.InvalidateRect ( Handle, (Areo.RECT)ClientRectangle, false );
			}
		}

		//if user is dragging(selecting text) then redraw
		//this will still cause some flickering but if we dont 
		//do it text will just change back to transparent for a 
		//few milliseconds and look even uglier
		protected override void OnMouseMove ( MouseEventArgs e ) {
			base.OnMouseMove ( e );
			if ( IsGlassSupported ) {
				if ( IsMouseDown ) {
					Areo.InvalidateRect ( Handle, (Areo.RECT)ClientRectangle, false );
				}
			}
		}
	}
}
