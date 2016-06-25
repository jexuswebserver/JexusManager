using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing;

namespace Vista.Controls {
	public class BreadcrumbBarButton : Control {
		public BreadcrumbBarButton () {
			this.DoubleBuffered = true;
			this.SetStyle ( ControlStyles.FixedHeight | ControlStyles.FixedWidth | ControlStyles.OptimizedDoubleBuffer |
				ControlStyles.StandardClick | ControlStyles.SupportsTransparentBackColor, true );
			this.Width = 24;
			this.Height = 20;
		}

		public string ToolTipText { get; set; }
		public Image Image { get; set; }

		internal bool IsMouseOver { get; set; }
		internal bool IsMouseDown { get; set; }

		protected override void OnResize ( EventArgs e ) {
			if ( this.Height != 20 ) {
				this.Height = 20;
			}

			if ( this.Width != 24 ) {
				this.Width = 24;
			}
			base.OnResize ( e );
		}

		protected override void OnMouseDown ( MouseEventArgs e ) {
			if ( !this.IsMouseDown ) {
				this.IsMouseDown = true;
				this.Invalidate ();
			}
			base.OnMouseDown ( e );
		}

		protected override void OnMouseUp ( MouseEventArgs e ) {
			if ( this.IsMouseDown ) {
				this.IsMouseDown = false;
				this.Invalidate ();
			}
			base.OnMouseUp ( e );
		}

		protected override void OnMouseEnter ( EventArgs e ) {
			if ( !this.IsMouseOver ) {
				this.IsMouseOver = true;
				if ( this.Parent != null && this.Parent is BreadcrumbBar ) {
					( this.Parent as BreadcrumbBar ).IsMouseOver = true;
					( this.Parent as BreadcrumbBar ).Invalidate ();
				}
				this.Invalidate ();
			}
			base.OnMouseEnter ( e );
		}

		protected override void OnMouseLeave ( EventArgs e ) {
			if ( this.IsMouseOver ) {
				this.IsMouseOver = false;
				if ( this.Parent != null && this.Parent is BreadcrumbBar ) {
					( this.Parent as BreadcrumbBar ).IsMouseOver = false;
					( this.Parent as BreadcrumbBar ).Invalidate ();
				}
				this.Invalidate ();
			}
			base.OnMouseLeave ( e );
		}

		protected override void OnPaint ( PaintEventArgs e ) {
			base.OnPaint ( e );
			Graphics g = e.Graphics;
			if ( this.Parent != null && this.Parent is BreadcrumbBar ) {
				BreadcrumbBar bar =  this.Parent as BreadcrumbBar;
				g.Clear ( bar.IsMouseOver ? Color.FromArgb ( bar.BackgroundAlpha, bar.HoverBackColor ) : Color.FromArgb ( bar.BackgroundAlpha, bar.BackColor ) );
			} else {
				g.Clear ( this.BackColor );
			}
			Rectangle r = new Rectangle ( 0, 0, this.Width, this.Height );
			if ( this.IsMouseOver ) {
				FillBackground ( g, r, new Color[] { 
					Color.FromArgb ( 255, 234, 246, 253 ),
					Color.FromArgb ( 255, 215, 239, 252 ),
					Color.FromArgb ( 255, 189, 230, 253 ),
					Color.FromArgb ( 255, 166, 214, 244 )
				} );
			} else {
				/*FillBackground ( g, r, new Color[ ] { 
					Color.Transparent,
					Color.Transparent,
					Color.Transparent,
					Color.Transparent
				} );*/
			}
			r.Offset ( 2, 1 );
			DrawImage ( g, r );
			r.Offset ( -2, -1 );
			DrawBorder ( g, r );
		}

		private void DrawBorder ( Graphics g, Rectangle r ) {
			Color borderColor = Color.FromArgb ( 200, 95, 96, 97 );
			if ( this.IsMouseOver ) {
				borderColor = Color.FromArgb ( 200, 60, 127, 177 );
			}
			g.DrawLine ( new Pen ( borderColor, 1 ), new Point ( r.X, r.Y - 1 ), new Point ( r.X, r.Height ) );
		}

		private void FillBackground ( Graphics g, Rectangle r, Color[] colors ) {
			if ( r.Width == 0 || r.Height == 0 ) {
				return;
			}

			if ( colors == null || colors.Length != 4 ) {
				throw new ArgumentException ( "colors is not a length of 4", "colors" );
			}

			Rectangle r1 = new Rectangle ( r.X + 2, r.Y + 1, r.Width, r.Height / 2 );
			Rectangle r2 = new Rectangle ( r.X + 2, r1.Height, r.Width, r1.Height - 1 );

			using ( LinearGradientBrush lgb = new LinearGradientBrush ( r1, colors[ 0 ], colors[ 1 ], LinearGradientMode.Vertical ) ) {
				g.FillRectangle ( lgb, r1 );
			}
			using ( LinearGradientBrush lgb = new LinearGradientBrush ( r2, colors[ 2 ], colors[ 3 ], LinearGradientMode.Vertical ) ) {
				g.FillRectangle ( lgb, r2 );
			}

			if ( this.IsMouseDown ) {
				// TODO: fix the shadow. its not drawn right and it looks like crap
				//DrawDownShadow ( g, r );
			}
		}

		private void DrawDownShadow ( Graphics g, Rectangle r ) {
			Color clr = Color.FromArgb ( 255, 106, 135, 153 );
			r.Offset ( 0, -1 );
			r.Inflate ( 1, 0 );
			/*using ( Pen p = new Pen ( clr, 1 ) ) {
				g.DrawLine ( p, r.X, r.Y, r.Width, r.Y );
			}*/
			r.Offset ( 2, 0 );
			clr = Color.FromArgb ( 255, 113, 156, 179 );
			using ( Pen p = new Pen ( clr, 1 ) ) {
				g.DrawLine ( p, r.X, r.Y, r.Width, r.Y );
			}
			r.Offset ( 0, 1 );
			clr = Color.FromArgb ( 255, 169, 198, 214 );
			using ( Pen p = new Pen ( clr, 1 ) ) {
				g.DrawLine ( p, r.X, r.Y, r.Width, r.Y );
			}
			r.Offset ( 0, 1 );
			clr = Color.FromArgb ( 255, 185, 217, 236 );
			using ( Pen p = new Pen ( clr, 1 ) ) {
				g.DrawLine ( p, r.X, r.Y, r.Width, r.Y );
			}
		}

		private void DrawImage ( Graphics g, Rectangle r ) {
			if ( this.Image != null ) {
				r.Offset ( this.IsMouseDown ? 2 : 1, this.IsMouseDown ? 1 : 0 );
				Rectangle tr = new Rectangle ( (int)Math.Ceiling ( (decimal)r.Width / (decimal)this.Image.Width ) + r.X, (int)Math.Ceiling ( (decimal)r.Height / (decimal)this.Image.Height ) + r.Y, this.Image.Width, this.Image.Height );
				g.DrawImage ( this.Image, tr );
			}
		}
	}
}
