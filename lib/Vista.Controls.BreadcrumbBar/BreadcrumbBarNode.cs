using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using Vista.Controls.Design;

namespace Vista.Controls {

	public class BreadcrumbBarNode : Control {
		public event CancelEventHandler DropDownMenuOpening;
		public event EventHandler DropDownMenuOpened;
		public event ToolStripDropDownClosedEventHandler DropDownMenuClosed;
		public event ToolStripDropDownClosingEventHandler DropDownMenuClosing;

		public BreadcrumbBarNode ()
			: this ( string.Empty, string.Empty ) {

		}

		public BreadcrumbBarNode ( string text )
			: this ( text, text ) {

		}

		public BreadcrumbBarNode ( string text, string value )
			: this ( text, value, null, null ) {

		}

		public BreadcrumbBarNode ( string text, EventHandler click )
			: this ( text, text, click, null ) {

		}

		public BreadcrumbBarNode ( string text, string value, EventHandler click )
			: this ( text, value, click, null ) {

		}


		public BreadcrumbBarNode ( string text, object tag )
			: this ( text, text, null, tag ) {

		}

		public BreadcrumbBarNode ( string text, string value, object tag )
			: this ( text, value, null, tag ) {

		}

		public BreadcrumbBarNode ( string text, EventHandler click, object tag ) :
			this ( text, text, click, tag ) {
		}

		public BreadcrumbBarNode ( string text, string value, EventHandler click, object tag ) {
			this.Text = text;
			this.Value = value;
			this.DoubleBuffered = true;
			this.SetStyle ( ControlStyles.OptimizedDoubleBuffer | ControlStyles.StandardClick | ControlStyles.SupportsTransparentBackColor, true );
			this.HasChildNodes = true;
			this.DropDownMenu = new ContextMenuStrip ();
			if ( !Areo.IsLegacyOS ) {
				this.DropDownMenu.Renderer = new WindowsVistaRenderer ();
			}
			this.DropDownMenuItems = new BreadcrumbDropDownItems ();
			this.DropDownMenuItems.ItemAdded += new EventHandler ( DropDownMenuItems_ItemAdded );
			this.DropDownMenuItems.ItemRemoved += new EventHandler ( DropDownMenuItems_ItemRemoved );
			this.DropDownMenu.Opening += new CancelEventHandler ( DropDownMenu_Opening );
			this.DropDownMenu.Opened += new EventHandler ( DropDownMenu_Opened );
			this.DropDownMenu.Closed += new ToolStripDropDownClosedEventHandler ( DropDownMenu_Closed );
			this.DropDownMenu.Closing += new ToolStripDropDownClosingEventHandler ( DropDownMenu_Closing );

			this.DropDownMenu.MaximumSize = new Size ( this.DropDownMenu.MaximumSize.Width, 450 );
			this.NavigateDelegate = click;
			this.Tag = tag;
		}

		public void AddDropDownItem ( BreadcrumbDropDownItem item ) {
			this.DropDownMenuItems.Add ( item );
			if ( !this.HasChildNodes ) {
				this.HasChildNodes = true;
			}
		}

		public EventHandler NavigateDelegate { get; set; }
		public object Tag { get; set; }
		public Size DropDownSize { get { return new Size ( this.HasChildNodes ? 16 : 0, this.HasChildNodes ? this.Bounds.Height : 0 ); } }
		public virtual Rectangle DropDownBounds {
			get {
				return new Rectangle ( new Point ( this.HasChildNodes ? this.TextBounds.Width - 1 : 0, 0 ), new Size ( this.HasChildNodes ? DropDownSize.Width : 0, this.Bounds.Height ) );
			}
		}
		public Rectangle TextBounds {
			get {
				return new Rectangle ( new Point ( 0, 0 ), new Size ( this.Bounds.Width - DropDownSize.Width, this.Bounds.Height ) );
			}
		}
		public string Value { get; set; }

		internal ContextMenuStrip DropDownMenu { get; private set; }
		internal BreadcrumbDropDownItems DropDownMenuItems { get; private set; }

		public bool HasChildNodes { get; /* { return this.DropDownMenu.Items.Count > 0; }*/ set; }

		internal bool IsMouseOver { get; set; }
		internal bool IsMouseDown { get; set; }
		internal bool IsMouseOverDropDown { get; set; }
		internal bool IsMouseOverText { get; set; }
		internal bool IsDropDownVisible { get { return this.DropDownMenu.Visible; } }

		protected override void OnClick ( EventArgs e ) {
			if ( this.IsMouseOverText ) {
				base.OnClick ( e );
			}
		}

		protected override void OnMouseDown ( MouseEventArgs e ) {
			if ( !this.IsMouseDown ) {
				this.IsMouseDown = true;
				if ( this.IsMouseOverDropDown ) {

					this.DropDownMenu.Show ( this, new Point ( this.TextBounds.Left + ( this.TextBounds.Width / 2 ), this.TextBounds.Height ) );
				}
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

		protected override void OnMouseMove ( MouseEventArgs e ) {
			base.OnMouseMove ( e );
			this.IsMouseOverDropDown = Rectangle.Intersect ( new Rectangle ( this.PointToClient ( MousePosition ), new Size ( 1, 1 ) ),
				this.DropDownBounds ) != Rectangle.Empty;
			this.IsMouseOverText = Rectangle.Intersect ( new Rectangle ( this.PointToClient ( MousePosition ), new Size ( 1, 1 ) ),
				this.TextBounds ) != Rectangle.Empty;
			this.Invalidate ();

		}

		protected override void OnMouseEnter ( EventArgs e ) {
			if ( !this.IsMouseOver ) {
				this.IsMouseOver = true;

				IsMouseOverDropDown = Rectangle.Intersect ( new Rectangle ( this.PointToClient ( MousePosition ), new Size ( 1, 1 ) ),
					this.DropDownBounds ) != Rectangle.Empty;
				IsMouseOverText = Rectangle.Intersect ( new Rectangle ( this.PointToClient ( MousePosition ), new Size ( 1, 1 ) ),
					this.TextBounds ) != Rectangle.Empty;


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
				IsMouseOverDropDown = false;
				IsMouseOverText = false;
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
				BreadcrumbBar parent = this.Parent as BreadcrumbBar;
				g.Clear ( parent.IsMouseOver ? Color.FromArgb ( parent.BackgroundAlpha, parent.HoverBackColor ) : Color.FromArgb ( parent.BackgroundAlpha, parent.BackColor ) );
				if ( this.IsMouseOver || this.IsDropDownVisible ) {
					if ( this.IsMouseOverText ) {
						FillBackground ( g, TextBounds, new Color[] { 
							Color.FromArgb ( 255, 234, 246, 253 ),
							Color.FromArgb ( 255, 215, 239, 252 ),
							Color.FromArgb ( 255, 189, 230, 253 ),
							Color.FromArgb ( 255, 166, 214, 244 )
						} );
						DrawBorder ( g, TextBounds, Color.FromArgb ( 200, 60, 127, 177 ) );
					} else {
						FillBackground ( g, TextBounds, new Color[] { 
							Color.FromArgb ( 255, 234, 234, 234 ),
							Color.FromArgb ( 255, 242, 242, 242 ),
							Color.FromArgb ( 255, 207, 207, 207 ),
							Color.FromArgb ( 255, 220, 220, 220 )
						} );
						DrawBorder ( g, TextBounds, Color.FromArgb ( 200, 95, 96, 97 ) );
					}

					if ( ( ( this.IsMouseOverDropDown || this.IsMouseOverText ) && this.HasChildNodes ) || this.IsDropDownVisible ) {
						FillBackground ( g, DropDownBounds, new Color[] { 
							Color.FromArgb ( 255, 234, 246, 253 ),
							Color.FromArgb ( 255, 215, 239, 252 ),
							Color.FromArgb ( 255, 189, 230, 253 ),
							Color.FromArgb ( 255, 166, 214, 244 )
						} );
						DrawBorder ( g, DropDownBounds, Color.FromArgb ( 200, 60, 127, 177 ) );
					}
				}
				if ( this.HasChildNodes ) {
					if ( ( this.IsMouseDown && this.IsMouseOverDropDown ) || this.IsDropDownVisible ) {
						DrawDownArrow ( g, DropDownBounds );
					} else {
						DrawRightArrow ( g, DropDownBounds );
					}
				}
				DrawNodeText ( g, this.Text, TextBounds );
			}
		}


		protected Point CalcStartPoint ( Size rectSize, Size imageSize ) {
			Point p = new Point (
				(int)Math.Ceiling ( (double)( rectSize.Width / 2 ) ) - (int)Math.Ceiling ( (double)( imageSize.Width / 2 ) ),
				 (int)Math.Ceiling ( (double)( rectSize.Height / 2 ) ) - (int)Math.Ceiling ( (double)( imageSize.Height / 2 ) ) );
			return p;
		}

		protected void DrawDownArrow ( Graphics g, Rectangle rectangle ) {
			Point startPoint = CalcStartPoint ( DropDownBounds.Size, new Size ( 7, 4 ) );
			startPoint.Offset ( DropDownBounds.X - 1, DropDownBounds.Y );

			Color clr = Color.FromArgb ( 255, 0, 0, 0 );
			using ( Pen pen = new Pen ( clr, 1 ) ) {
				g.DrawLine ( pen, startPoint, new Point ( startPoint.X + 7, startPoint.Y ) );
				startPoint.Offset ( 1, 1 );
				g.DrawLine ( pen, startPoint, new Point ( startPoint.X + 5, startPoint.Y ) );
				startPoint.Offset ( 1, 1 );
				g.DrawLine ( pen, startPoint, new Point ( startPoint.X + 3, startPoint.Y ) );
				startPoint.Offset ( 1, 1 );
				g.DrawLine ( pen, startPoint, new Point ( startPoint.X + 1, startPoint.Y ) );
			}
		}

		protected void DrawRightArrow ( Graphics g, Rectangle rectangle ) {
			Point startPoint = CalcStartPoint ( DropDownBounds.Size, new Size ( 4, 7 ) );
			startPoint.Offset ( DropDownBounds.X + 1, DropDownBounds.Y - 1 );

			Color clr = Color.FromArgb ( 255, 0, 0, 0 );
			using ( Pen pen = new Pen ( clr, 1 ) ) {
				g.DrawLine ( pen, startPoint, new Point ( startPoint.X, startPoint.Y + 7 ) );
				startPoint.Offset ( 1, 1 );
				g.DrawLine ( pen, startPoint, new Point ( startPoint.X, startPoint.Y + 5 ) );
				startPoint.Offset ( 1, 1 );
				g.DrawLine ( pen, startPoint, new Point ( startPoint.X, startPoint.Y + 3 ) );
				startPoint.Offset ( 1, 1 );
				g.DrawLine ( pen, startPoint, new Point ( startPoint.X, startPoint.Y + 1 ) );
			}
		}

		private void DrawNodeText ( Graphics g, string text, Rectangle rect ) {
			g.DrawString ( text, this.Font, new SolidBrush ( this.ForeColor ), rect, GetStringFormat () );
		}

		internal StringFormat GetStringFormat () {
			StringFormat sf = new StringFormat ( StringFormatFlags.FitBlackBox | StringFormatFlags.NoWrap );
			sf.Alignment = StringAlignment.Center;
			sf.Trimming = StringTrimming.EllipsisCharacter;
			sf.LineAlignment = StringAlignment.Center;
			return sf;
		}

		protected void FillBackground ( Graphics g, Rectangle r, Color[] colors ) {
			if ( r.Width == 0 || r.Height == 0 ) {
				return;
			}

			if ( colors == null || colors.Length != 4 ) {
				throw new ArgumentException ( "colors is not a length of 4", "colors" );
			}

			Rectangle r1 = new Rectangle ( r.X + 2, r.Y + 1, r.Width - 4, (int)Math.Floor ( (double)r.Height / 2 ) );
			Rectangle r2 = new Rectangle ( r.X + 2, r1.Height, r.Width - 4, r1.Height - 1 );

			using ( LinearGradientBrush lgb = new LinearGradientBrush ( r1, colors[ 0 ], colors[ 1 ], LinearGradientMode.Vertical ) ) {
				g.FillRectangle ( lgb, r1 );
			}
			using ( LinearGradientBrush lgb = new LinearGradientBrush ( r2, colors[ 2 ], colors[ 3 ], LinearGradientMode.Vertical ) ) {
				g.FillRectangle ( lgb, r2 );
			}
		}

		protected virtual void DrawBorder ( Graphics g, Rectangle r, Color color ) {
			g.DrawLine ( new Pen ( color, 1 ), new Point ( r.X, r.Y - 1 ), new Point ( r.X, r.Height ) );
			g.DrawLine ( new Pen ( color, 1 ), new Point ( r.X + r.Width - 1, r.Y - 1 ), new Point ( r.X + r.Width - 1, r.Height ) );
		}

		#region Dropdown Menu Events
		void DropDownMenu_Closing ( object sender, ToolStripDropDownClosingEventArgs e ) {
			if ( DropDownMenuClosing != null ) {
				this.DropDownMenuClosing ( this, e );
			}
		}

		void DropDownMenu_Closed ( object sender, ToolStripDropDownClosedEventArgs e ) {
			this.Parent.Invalidate ();
			if ( DropDownMenuClosed != null ) {
				this.DropDownMenuClosed ( this, e );
			}
		}

		void DropDownMenu_Opened ( object sender, EventArgs e ) {
			if ( DropDownMenuOpened != null ) {
				this.DropDownMenuOpened ( this, e );
			}
		}

		void DropDownMenu_Opening ( object sender, CancelEventArgs e ) {
			if ( DropDownMenuOpening != null ) {
				this.DropDownMenuOpening ( this, e );
			}
		}

		void DropDownMenuItems_ItemRemoved ( object sender, EventArgs e ) {
			BuildDropDownMenu ();
		}

		void DropDownMenuItems_ItemAdded ( object sender, EventArgs e ) {
			BuildDropDownMenu ();
		}

		private void BuildDropDownMenu () {
			this.DropDownMenu.Items.Clear ();
			foreach ( BreadcrumbDropDownItem item in this.DropDownMenuItems ) {
				ToolStripMenuItem tsmi = new ToolStripMenuItem ( item.Text, item.Image, delegate ( object s, EventArgs e1 ) {
					ToolStripMenuItem i = s as ToolStripMenuItem;
					BreadcrumbDropDownItem bddi = i.Tag as BreadcrumbDropDownItem;
					if ( bddi.NavigateDelegate != null ) {
						bddi.NavigateDelegate.Invoke ( bddi, EventArgs.Empty );
					}
				}, item.Text );
				tsmi.Tag = item.Tag;
			}
		}
		#endregion

	}
}
