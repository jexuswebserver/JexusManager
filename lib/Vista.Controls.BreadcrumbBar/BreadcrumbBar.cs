using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;
using System.Globalization;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Drawing.Design;

namespace Vista.Controls {
	public class BreadcrumbBar : Control {
		public event EventHandler ViewModeChanged;
		public event EventHandler Navigate;
		public event EventHandler<BreadcrumbBarNodeClickedEventArgs> NodeClicked;

		public enum ViewModes {
			Nodes,
			Text
		}

		private ViewModes _viewMode;

		public BreadcrumbBar () {
			this.DoubleBuffered = true;
			this.SetStyle ( ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
				ControlStyles.Selectable | ControlStyles.StandardClick | ControlStyles.StandardDoubleClick |
				ControlStyles.SupportsTransparentBackColor | ControlStyles.FixedHeight | ControlStyles.ContainerControl, true );
			this.PathSeparator = new string ( new char[] { Path.AltDirectorySeparatorChar } );
			this.Nodes = new BreadcrumbBarNodeCollection ( this );
			this.Buttons = new BreadcrumbBarButtonCollection ( this );
			this.BackColor = SystemColors.Window;  //Color.FromArgb ( 255, 230, 235, 241 );
			this.HoverBackColor = SystemColors.Window;
			this.ForeColor = SystemColors.WindowText;
			this.Height = 22;
			this.Root = new BreadcrumbBarRootNode ( this, Properties.Resources.folder );

			this.Root.Height = this.Height - 2;
			this.Root.Left = 1;
			this.Root.Top = 1;
			//this.Root.Width = this.Root.DropDownBounds.Width + this.Root.ImageBounds.Width + 4;
			this.Root.HasChildNodes = true;
			this.Controls.Add ( this.Root );
			//this.Nodes.Add ( this.Root );

			this.TextBox = new GlassTextBox ();
			this.TextBox.Visible = false;
			this.TextBox.Margin = new Padding ( 2, 5, 5, 2 );
			this.TextBox.BorderStyle = BorderStyle.None;
			this.TextBox.KeyUp += new KeyEventHandler ( TextBox_KeyUp );
			this.TextBox.KeyPress += new KeyPressEventHandler ( TextBox_KeyPress );
			this.TextBox.KeyDown += new KeyEventHandler ( TextBox_KeyDown );
			this.TextBox.Leave += new EventHandler ( TextBox_Leave );
			this.TextBox.LostFocus += new EventHandler ( TextBox_LostFocus );
			this.TextBox.DragDrop += new DragEventHandler ( TextBox_DragDrop );
			this.TextBox.DragEnter += new DragEventHandler ( TextBox_DragEnter );
			this.TextBox.DragLeave += new EventHandler ( TextBox_DragLeave );
			this.TextBox.DragOver += new DragEventHandler ( TextBox_DragOver );
			this.TextBox.Enter += new EventHandler ( TextBox_Enter );
			this.TextBox.GotFocus += new EventHandler ( TextBox_GotFocus );
			this.TextBox.MouseClick += new MouseEventHandler ( TextBox_MouseClick );
			this.TextBox.MouseDoubleClick += new MouseEventHandler ( TextBox_MouseDoubleClick );
			this.TextBox.MouseDown += new MouseEventHandler ( TextBox_MouseDown );
			this.TextBox.MouseEnter += new EventHandler ( TextBox_MouseEnter );
			this.TextBox.MouseHover += new EventHandler ( TextBox_MouseHover );
			this.TextBox.MouseLeave += new EventHandler ( TextBox_MouseLeave );
			this.TextBox.MouseMove += new MouseEventHandler ( TextBox_MouseMove );
			this.TextBox.MouseUp += new MouseEventHandler ( TextBox_MouseUp );
			this.TextBox.MouseWheel += new MouseEventHandler ( TextBox_MouseWheel );

			this.Controls.Add ( this.TextBox );
			this.BackgroundAlpha = 255;
			this.ViewMode = ViewModes.Nodes;
		}

		internal GlassTextBox TextBox { get; set; }
		internal bool IsOverDropArrow { get; set; }
		internal bool IsOverDeadSpace { get; set; }
		internal bool IsMouseOver { get; set; }
		internal bool IsMouseDown { get; set; }
		internal bool IsTextBoxVisible { get { return this.TextBox.Visible; } }
		internal bool IsNodeDropVisible {
			get {
				foreach ( BreadcrumbBarNode item in this.Nodes ) {
					if ( item.IsDropDownVisible ) {
						this.IsMouseOver = true;
						return true;
					}
				}
				this.IsMouseOver = this.IsMouseOver || false;
				return false;
			}
		}

		/// <summary>
		/// Gets if there are any hidden nodes
		/// </summary>
		internal bool HasHiddenNodes {
			get {
				foreach ( BreadcrumbBarNode item in this.Nodes ) {
					if ( !item.Visible ) {
						return true;
					}
				}
				return false;
			}
		}

		internal Rectangle TextBoxBounds {
			get {
				Point p = new Point ( this.Root.ImageBounds.Width + this.Root.ImageBounds.X + 4, 1 );
				return new Rectangle ( p, new Size ( DropArrowBounds.Left - p.X - 4, this.Height - 2 ) );
			}
		}

		/// <summary>
		/// Gets the 
		/// </summary>
		internal Rectangle NodesBounds {
			get {
				int w = 0;
				foreach ( BreadcrumbBarNode item in this.Nodes ) {
					if ( item.Visible ) {
						w += item.Width;
					}
				}

				return new Rectangle ( 1, 1, w, this.Height - 2 );
			}
		}

		internal Rectangle ButtonsBounds {
			get {
				Rectangle rect = new Rectangle ( this.Bounds.Width - 1, 2, 0, this.Bounds.Height - 4 );
				foreach ( BreadcrumbBarButton item in this.Buttons ) {
					rect.Width += item.Width;
				}
				return rect;
			}
		}
		internal Rectangle DropArrowBounds {
			get {
				Rectangle rect = new Rectangle ( this.ButtonsBounds.Left - this.ButtonsBounds.Width - 20, 1, 20, this.Bounds.Height - 2 );
				return rect;
			}
		}
		internal Rectangle DeadBounds {
			get {
				int nRight = NodesBounds.Width + NodesBounds.X;
				Rectangle rect = new Rectangle ( this.DropArrowBounds.Left - nRight, 2, nRight, this.Bounds.Height - 4 );
				return rect;
			}
		}

		[Category ( "Appearance" ), DefaultValue ( 255 )]
		public byte BackgroundAlpha { get; set; }

		[Category ( "Appearance" ), DefaultValue ( ViewModes.Nodes )]
		public ViewModes ViewMode {
			get { return this._viewMode; }
			set {
				if ( this._viewMode != value ) {
					this._viewMode = value;
					OnViewModeChanged ( EventArgs.Empty );
				}
			}
		}

		[Browsable ( false )]
		public int DeadSpaceWidth { get { return NodesBounds.Width + NodesBounds.X; } }

		[Category ( "Appearance" ), Browsable ( false ), EditorBrowsable ( EditorBrowsableState.Never )]
		public new Color BackColor { get { return base.BackColor; } set { base.BackColor = value; } }

		[Category ( "Appearance" ), Browsable ( false ), EditorBrowsable ( EditorBrowsableState.Never )]
		public Color HoverBackColor { get; set; }

		[Category ( "Appearance" ), DefaultValue ( "\\" )]
		public string PathSeparator { get; set; }

		[Browsable ( false )]
		public BreadcrumbBarRootNode Root { get; private set; }

		public BreadcrumbBarNodeCollection Nodes { get; private set; }

		public BreadcrumbBarButtonCollection Buttons { get; private set; }

		[Browsable ( false )]
		public string FullPath {
			get {
				StringBuilder sb = new StringBuilder ();
				foreach ( BreadcrumbBarNode item in this.Nodes ) {
					sb.AppendFormat ( CultureInfo.InvariantCulture, "{0}{1}", item.Value, this.PathSeparator );
				}
				return sb.ToString ();
			}
			set {
				SetNodesFromPath ( value );
			}
		}

		private void SetNodesFromPath ( string value ) {
			this.Nodes.Clear ();
			StringBuilder tpath = new StringBuilder ();
			foreach ( string s in value.Split ( new string[] { this.PathSeparator }, StringSplitOptions.RemoveEmptyEntries ) ) {
				tpath.AppendFormat ( "{0}{1}", s, this.PathSeparator );
				this.Nodes.Add ( new BreadcrumbBarNode ( s, delegate ( object sender, EventArgs e ) {
					BreadcrumbBarNode item = sender as BreadcrumbBarNode;
					OnNodeClicked ( new BreadcrumbBarNodeClickedEventArgs ( item ) );
				}, tpath ) );
			}

			this.Invalidate ();

		}

		protected void OnShowTextBox ( EventArgs e ) {
			this.TextBox.Text = this.FullPath;
			this.TextBox.Visible = true;
			this.TextBox.Focus ();
			this.TextBox.SelectAll ();
		}

		protected void OnShowBreadcrumb ( EventArgs e ) {
			this.Root.Visible = true;
			this.Nodes.ShowBreadcrumbs ();
		}

		protected void OnHideBreadcrumb ( EventArgs e ) {
			this.Root.Visible = false;
			this.Nodes.HideBreadcrumbs ();
		}

		protected void OnHideTextBox ( EventArgs e ) {
			this.TextBox.Visible = false;
		}

		protected void OnNavigate ( EventArgs e ) {
			if ( this.Navigate != null ) {
				this.Navigate ( this, e );
			}
		}

		protected void OnNodeClicked ( BreadcrumbBarNodeClickedEventArgs e ) {
			if ( this.NodeClicked != null ) {
				this.NodeClicked ( this, e );
			}
		}

		protected void OnViewModeChanged ( EventArgs e ) {
			switch ( this.ViewMode ) {
				case ViewModes.Nodes:
					OnHideTextBox ( e );
					OnShowBreadcrumb ( e );
					break;
				case ViewModes.Text:
					OnHideBreadcrumb ( e );
					OnShowTextBox ( e );
					break;
			}
			this.Invalidate ();

			if ( this.ViewModeChanged != null ) {
				this.ViewModeChanged ( this, e );
			}
		}

		protected override void OnKeyUp ( KeyEventArgs e ) {
			if ( e.KeyCode == Keys.Escape && this.IsTextBoxVisible ) {
				this.ViewMode = ViewModes.Nodes;
			} else if ( ( e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return ) ) {
				if ( this.IsTextBoxVisible ) {
					this.FullPath = this.TextBox.Text;
					this.ViewMode = ViewModes.Nodes;
				}
				OnNavigate ( EventArgs.Empty );
			}
			base.OnKeyUp ( e );
		}

		protected override void OnHandleCreated ( EventArgs e ) {
			base.OnHandleCreated ( e );
			this.TextBox.Width = this.TextBoxBounds.Width;
			this.TextBox.Height = this.TextBoxBounds.Height;
			this.TextBox.Left = this.TextBoxBounds.X;
			this.TextBox.Top = this.TextBoxBounds.Y + 3;
		}

		protected override void OnResize ( EventArgs e ) {
			base.OnResize ( e );
			if ( this.Height != 22 ) {
				this.Height = 22;
			}
		}

		protected override void OnClick ( EventArgs e ) {
			if ( this.IsOverDeadSpace ) {
				this.ViewMode = ViewModes.Text;
			} else {
				bool isOverRootImageBounds =  Rectangle.Intersect ( new Rectangle ( this.PointToClient ( MousePosition ), new Size ( 1, 1 ) ),
				this.Root.ImageBounds ) != Rectangle.Empty;
				if ( isOverRootImageBounds && this.IsTextBoxVisible ) {
					this.ViewMode = ViewModes.Nodes;
				}
			}
			base.OnClick ( e );
		}

		protected override void OnMouseEnter ( EventArgs e ) {
			if ( !IsMouseOver ) {
				this.IsMouseOver = true;
				this.Invalidate ();
			}
			base.OnMouseEnter ( e );
		}

		protected override void OnMouseMove ( MouseEventArgs e ) {
			base.OnMouseMove ( e );
			this.IsOverDeadSpace = Rectangle.Intersect ( new Rectangle ( this.PointToClient ( MousePosition ), new Size ( 1, 1 ) ),
				this.DeadBounds ) != Rectangle.Empty;
			bool bln = Rectangle.Intersect ( new Rectangle ( this.PointToClient ( MousePosition ), new Size ( 1, 1 ) ),
	this.DropArrowBounds ) != Rectangle.Empty;
			if ( bln != this.IsOverDropArrow ) {
				this.IsOverDropArrow = bln;
				this.Invalidate ();
			}
		}

		protected override void OnMouseLeave ( EventArgs e ) {
			if ( this.IsMouseOver ) {
				this.IsMouseOver = false;
				this.IsOverDropArrow = false;
				this.Invalidate ();
			}
			base.OnMouseLeave ( e );
		}

		protected override void OnMouseDown ( MouseEventArgs e ) {
			base.OnMouseDown ( e );
			if ( !this.IsMouseDown ) {
				this.IsMouseDown = true;
				this.Invalidate ();
			}
		}

		protected override void OnMouseUp ( MouseEventArgs e ) {
			base.OnMouseUp ( e );
			if ( this.IsMouseDown ) {
				this.IsMouseDown = false;
				this.Invalidate ();
			}
		}

		protected override void OnPaint ( PaintEventArgs e ) {
			base.OnPaint ( e );
			Graphics g = e.Graphics;
			g.Clear ( this.IsMouseOver || this.IsNodeDropVisible ? Color.FromArgb ( this.BackgroundAlpha, this.HoverBackColor ) : Color.FromArgb ( this.BackgroundAlpha, this.BackColor ) );

			DrawBorder ( g, new Rectangle ( 0, 0, this.Bounds.Width, this.Bounds.Height ) );

			DrawDropArrow ( g );

			foreach ( Control item in this.Controls ) {
				if ( item.Visible ) {
					item.Invalidate ();
				}
			}

			if ( this.IsTextBoxVisible ) {
				this.TextBox.BackColor = this.IsMouseOver || this.IsNodeDropVisible ? this.HoverBackColor : this.BackColor;
				if ( this.Root != null && this.Root.Image != null ) {
					g.DrawImage ( this.Root.Image, this.Root.ImageBounds.X + 1, this.Root.ImageBounds.Y + 2, this.Root.Image.Width, this.Root.Image.Height );
				}
			}


			//g.FillRectangle ( new SolidBrush ( Color.Fuchsia ), this.DeadBounds );
		}

		private void DrawDropArrow ( Graphics g ) {
			Point startPoint = CalcStartPoint ( this.DropArrowBounds.Size, new Size ( 7, 4 ) );
			startPoint.Offset ( this.DropArrowBounds.X - 1, this.DropArrowBounds.Y );
			Color clr = Color.FromArgb ( 255, 0, 1, 91 );

			if ( IsOverDropArrow ) {

				FillBackground ( g, DropArrowBounds, new Color[] {
          Color.FromArgb ( 255, 234, 246, 253 ),
          Color.FromArgb ( 255, 215, 239, 252 ),
          Color.FromArgb ( 255, 189, 230, 253 ),
          Color.FromArgb ( 255, 166, 214, 244 )
        } );

				Color borderColor = Color.FromArgb ( 200, 60, 127, 177 );
				using ( Pen p = new Pen ( borderColor, 1 ) ) {
					g.DrawLine ( p, new Point ( DropArrowBounds.X, DropArrowBounds.Y ), new Point ( DropArrowBounds.X, DropArrowBounds.Height + DropArrowBounds.Y - 1 ) );
				}
			} else {
				using ( Brush brsh = new SolidBrush ( IsMouseOver || this.IsNodeDropVisible ? Color.FromArgb ( BackgroundAlpha, HoverBackColor ) : Color.FromArgb ( BackgroundAlpha, BackColor ) ) ) {
					g.FillRectangle ( brsh, DropArrowBounds );
				}
			}
			using ( Pen pen = new Pen ( clr, 1 ) ) {
				if ( this.IsMouseDown && this.IsOverDropArrow ) {
					startPoint.Offset ( 1, 1 );
				}
				g.DrawLine ( pen, startPoint, new Point ( startPoint.X + 7, startPoint.Y ) );
				startPoint.Offset ( 1, 1 );
				g.DrawLine ( pen, startPoint, new Point ( startPoint.X + 5, startPoint.Y ) );
				startPoint.Offset ( 1, 1 );
				g.DrawLine ( pen, startPoint, new Point ( startPoint.X + 3, startPoint.Y ) );
				startPoint.Offset ( 1, 1 );
				g.DrawLine ( pen, startPoint, new Point ( startPoint.X + 1, startPoint.Y ) );
			}
		}

		protected void FillBackground ( Graphics g, Rectangle r, Color[] colors ) {
			if ( r.Width == 0 || r.Height == 0 ) {
				return;
			}

			if ( colors == null || colors.Length != 4 ) {
				throw new ArgumentException ( "colors is not a length of 4", "colors" );
			}

			Rectangle r1 = new Rectangle ( r.X + 2, r.Y + 1, r.Width - 4, (int)Math.Ceiling ( (double)r.Height / 2 ) );
			Rectangle r2 = new Rectangle ( r.X + 2, r1.Height, r.Width - 4, r1.Height );

			using ( LinearGradientBrush lgb = new LinearGradientBrush ( r1, colors[ 0 ], colors[ 1 ], LinearGradientMode.Vertical ) ) {
				g.FillRectangle ( lgb, r1 );
			}
			using ( LinearGradientBrush lgb = new LinearGradientBrush ( r2, colors[ 2 ], colors[ 3 ], LinearGradientMode.Vertical ) ) {
				g.FillRectangle ( lgb, r2 );
			}
		}

		private Point CalcStartPoint ( Size rectSize, Size imageSize ) {
			Point p = new Point (
				(int)Math.Ceiling ( (double)( rectSize.Width / 2 ) ) - (int)Math.Ceiling ( (double)( imageSize.Width / 2 ) ),
				 (int)Math.Ceiling ( (double)( rectSize.Height / 2 ) ) - (int)Math.Ceiling ( (double)( imageSize.Height / 2 ) ) );
			return p;
		}

		private void DrawBorder ( Graphics g, Rectangle rect ) {

			using ( Pen lightPen = new Pen ( Color.FromArgb ( 255, 158, 167, 177 ), 1 ) ) {
				// bottom line
				g.DrawLine ( new Pen ( Color.FromArgb ( 255, 180, 190, 200 ), 1 ), new Point ( rect.X, rect.Height - 1 ), new Point ( rect.X + 1, rect.Height - 1 ) );
				g.DrawLine ( new Pen ( Color.FromArgb ( 255, 167, 176, 186 ), 1 ), new Point ( rect.X + 1, rect.Height - 1 ), new Point ( rect.X + 2, rect.Height - 1 ) );
				g.DrawLine ( lightPen, new Point ( rect.X + 2, rect.Height - 1 ), new Point ( rect.Width - 2, rect.Height - 1 ) );
				g.DrawLine ( new Pen ( Color.FromArgb ( 255, 158, 170, 182 ), 1 ), new Point ( rect.Width - 2, rect.Height - 1 ), new Point ( rect.Width - 1, rect.Height - 1 ) );
				g.DrawLine ( new Pen ( Color.FromArgb ( 255, 171, 183, 196 ), 1 ), new Point ( rect.Width - 1, rect.Height - 1 ), new Point ( rect.Width, rect.Height - 1 ) );

				// inner line
				g.DrawLine ( new Pen ( Color.FromArgb ( 255, 248, 250, 251 ), 1 ), new Point ( rect.X + 2, rect.Height - 2 ), new Point ( rect.Width - 2, rect.Height - 2 ) );
				// bottom line end
			}

			using ( Pen darkPen = new Pen ( Color.FromArgb ( 255, 72, 77, 83 ), 1 ) ) {
				// top line
				g.DrawLine ( new Pen ( Color.FromArgb ( 255, 160, 169, 178 ), 1 ), new Point ( rect.X, rect.Y ), new Point ( rect.X + 1, rect.Y ) );
				g.DrawLine ( new Pen ( Color.FromArgb ( 255, 111, 116, 122 ), 1 ), new Point ( rect.X + 1, rect.Y ), new Point ( rect.X + 2, rect.Y ) );
				g.DrawLine ( darkPen, new Point ( rect.X + 2, rect.Y ), new Point ( rect.Width - 2, rect.Y ) );
				g.DrawLine ( new Pen ( Color.FromArgb ( 255, 104, 111, 119 ), 1 ), new Point ( rect.Width - 2, rect.Y ), new Point ( rect.Width - 1, rect.Y ) );
				g.DrawLine ( new Pen ( Color.FromArgb ( 255, 149, 160, 173 ), 1 ), new Point ( rect.Width - 1, rect.Y ), new Point ( rect.Width, rect.Y ) );

				// inner line
				g.DrawLine ( new Pen ( Color.FromArgb ( 255, 213, 218, 223 ), 1 ), new Point ( rect.X + 1, rect.Y + 1 ), new Point ( rect.X + 2, rect.Y + 1 ) );
				g.DrawLine ( new Pen ( Color.FromArgb ( 255, 248, 250, 251 ), 1 ), new Point ( rect.X + 2, rect.Y + 1 ), new Point ( rect.Width - 2, rect.Y + 1 ) );
				g.DrawLine ( new Pen ( Color.FromArgb ( 255, 206, 213, 220 ), 1 ), new Point ( rect.Width - 2, rect.Y + 1 ), new Point ( rect.Width - 1, rect.Y + 1 ) );
				// top line end
			}

			using ( LinearGradientBrush leftBrush = new LinearGradientBrush ( new Point ( rect.X, rect.Y + 2 ), new Point ( rect.X, rect.Height - 2 ), Color.FromArgb ( 255, 86, 89, 93 ), Color.FromArgb ( 255, 135, 141, 148 ) ) ) {
				using ( Pen leftPen = new Pen ( leftBrush, 1 ) ) {
					// left line
					g.DrawLine ( new Pen ( Color.FromArgb ( 255, 113, 118, 124 ), 1 ), new Point ( rect.X, rect.Y + 1 ), new Point ( rect.X, rect.Y + 2 ) );
					g.DrawLine ( leftPen, new Point ( rect.X, rect.Y + 2 ), new Point ( rect.X, rect.Height - 2 ) );
					g.DrawLine ( new Pen ( Color.FromArgb ( 255, 150, 158, 166 ), 1 ), new Point ( rect.X, rect.Height - 2 ), new Point ( rect.X, rect.Height - 1 ) );

					//inner line
					g.DrawLine ( new Pen ( Color.FromArgb ( 255, 211, 218, 225 ), 1 ), new Point ( rect.X + 1, rect.Height - 3 ), new Point ( rect.X + 1, rect.Height - 2 ) );
					g.DrawLine ( new Pen ( Color.FromArgb ( 255, 248, 250, 251 ), 1 ), new Point ( rect.X + 1, rect.Y + 2 ), new Point ( rect.X + 1, rect.Height - 3 ) );
					// left line end
				}
			}

			using ( LinearGradientBrush rightBrush = new LinearGradientBrush ( new Point ( this.Width - 1, rect.Y + 2 ), new Point ( rect.Width - 1, rect.Height - 2 ), Color.FromArgb ( 255, 81, 86, 91 ), Color.FromArgb ( 255, 129, 137, 145 ) ) ) {
				using ( Pen rightPen = new Pen ( rightBrush, 1 ) ) {
					// right line
					g.DrawLine ( new Pen ( Color.FromArgb ( 255, 105, 113, 120 ), 1 ), new Point ( rect.Width - 1, rect.Y + 1 ), new Point ( rect.Width - 1, rect.Y + 2 ) );
					g.DrawLine ( rightPen, new Point ( rect.Width - 1, rect.Y + 2 ), new Point ( rect.Width - 1, rect.Height - 2 ) );
					g.DrawLine ( new Pen ( Color.FromArgb ( 255, 129, 137, 145 ), 1 ), new Point ( rect.Width - 1, rect.Height - 2 ), new Point ( this.Width - 1, this.Height - 2 ) );

					// inner line
					g.DrawLine ( new Pen ( Color.FromArgb ( 255, 204, 213, 222 ), 1 ), new Point ( rect.Width - 2, rect.Height - 3 ), new Point ( rect.Width - 2, rect.Height - 2 ) );
					g.DrawLine ( new Pen ( Color.FromArgb ( 255, 248, 250, 251 ), 1 ), new Point ( rect.Width - 2, 2 ), new Point ( rect.Width - 2, rect.Height - 3 ) );
					// right line end
				}
			}
		}

		#region TextBox Events

		void TextBox_MouseWheel ( object sender, MouseEventArgs e ) {

		}

		void TextBox_MouseUp ( object sender, MouseEventArgs e ) {
			this.OnMouseUp ( e );
		}

		void TextBox_MouseMove ( object sender, MouseEventArgs e ) {
			this.OnMouseMove ( e );
		}

		void TextBox_MouseLeave ( object sender, EventArgs e ) {
			this.OnMouseLeave ( e );
		}

		void TextBox_MouseHover ( object sender, EventArgs e ) {
			this.OnMouseHover ( e );
		}

		void TextBox_MouseEnter ( object sender, EventArgs e ) {
			this.OnMouseEnter ( e );
		}

		void TextBox_MouseDown ( object sender, MouseEventArgs e ) {
			this.OnMouseDown ( e );
		}

		void TextBox_MouseDoubleClick ( object sender, MouseEventArgs e ) {

		}

		void TextBox_MouseClick ( object sender, MouseEventArgs e ) {

		}

		void TextBox_GotFocus ( object sender, EventArgs e ) {

		}

		void TextBox_Enter ( object sender, EventArgs e ) {

		}

		void TextBox_DragOver ( object sender, DragEventArgs e ) {

		}

		void TextBox_DragLeave ( object sender, EventArgs e ) {

		}

		void TextBox_DragEnter ( object sender, DragEventArgs e ) {

		}

		void TextBox_DragDrop ( object sender, DragEventArgs e ) {

		}

		void TextBox_LostFocus ( object sender, EventArgs e ) {

		}

		void TextBox_Leave ( object sender, EventArgs e ) {

		}

		void TextBox_KeyDown ( object sender, KeyEventArgs e ) {
			this.OnKeyDown ( e );
		}

		void TextBox_KeyPress ( object sender, KeyPressEventArgs e ) {
			this.OnKeyPress ( e );
		}

		void TextBox_KeyUp ( object sender, KeyEventArgs e ) {
			this.OnKeyUp ( e );
		}
		#endregion

		#region internal Collections
		#region BreadcrumbBarNodeCollection
		public class BreadcrumbBarNodeCollection : ControlCollection {
			List<BreadcrumbBarNode> nodes;
			public BreadcrumbBarNodeCollection ( Control owner )
				: base ( owner ) {
				nodes = new List<BreadcrumbBarNode> ();
			}

			public override void Add ( Control value ) {
				lock ( nodes ) {
					if ( value is BreadcrumbBarNode ) {
						BreadcrumbBarNode v = value as BreadcrumbBarNode;
						v.Height = this.Owner.Height - 2;
						v.Width = CalculateNodeWidth ( v );
						int w = ( Owner as BreadcrumbBar ).Root.Width + 1;

						foreach ( var item in nodes ) {
							w += item.Width;
						}

						v.Left = w;
						v.Top = 1;
						v.Parent = Owner as BreadcrumbBar;
						nodes.Add ( v );
						Owner.Controls.Add ( v );
					}
				}
			}


			internal int CalculateNodeWidth ( BreadcrumbBarNode node ) {
				using ( Graphics g = node.CreateGraphics () ) {
					return (int)g.MeasureString (
						node.Text,
						node.Font,
						new SizeF (
							Owner.Width, Owner.Height ),
							node.GetStringFormat () ).Width +
							( node.HasChildNodes ? node.DropDownBounds.Width : 0 ) +
							( node is BreadcrumbBarRootNode ?
								( node as BreadcrumbBarRootNode ).ImageBounds.Width +
								( node as BreadcrumbBarRootNode ).ImageBounds.X + 2 :
								8
							);
				}
			}

			public void ResizeNodes () {
				lock ( nodes ) {
					int w = ( Owner as BreadcrumbBar ).Root != null ? ( Owner as BreadcrumbBar ).Root.Width + 1 : 1;
					foreach ( var item in nodes ) {
						if ( item.Visible || !item.IsAccessible ) {
							item.Width = CalculateNodeWidth ( item );
							item.Left = w;
							w += item.Width;
						}
					}
				}
			}

			public void ShowBreadcrumbs () {
				foreach ( var item in nodes ) {
					item.Visible = true;
				}
			}

			public void HideBreadcrumbs () {
				foreach ( var item in nodes ) {
					item.Visible = false;
				}
			}

			public override int Count {
				get {
					return nodes.Count;
				}
			}

			public override void AddRange ( Control[] controls ) {
				lock ( nodes ) {
					foreach ( var item in controls ) {
						this.Add ( item );
					}
				}
			}

			public override void Clear () {
				lock ( nodes ) {
					for ( int i = 0; i < this.nodes.Count; i++ ) {
						BreadcrumbBarNode item = this.nodes[ i ];
						//if ( item.Equals ( ( this.Owner as BreadcrumbBar ).Root ) || item is BreadcrumbBarRootNode ) {
						//	continue;
						//} else {
						Owner.Controls.Remove ( item );

						//}
					}
					//BreadcrumbBarRootNode root = ( this.Owner as BreadcrumbBar ).Root.Clone () as BreadcrumbBarRootNode;
					nodes.Clear ();
					//( this.Owner as BreadcrumbBar ).Root = root;
					//this.nodes.Add ( root );
				}
			}

			public override bool ContainsKey ( string key ) {
				return Owner.Controls.ContainsKey ( key );
			}

			public override System.Collections.IEnumerator GetEnumerator () {
				return nodes.GetEnumerator ();
			}

			public override int GetChildIndex ( Control child, bool throwException ) {
				return Owner.Controls.GetChildIndex ( child, throwException );
			}

			public override int IndexOfKey ( string key ) {
				return Owner.Controls.IndexOfKey ( key );
			}

			public override void Remove ( Control value ) {
				nodes.Remove ( value as BreadcrumbBarNode );
				Owner.Controls.Remove ( value );
			}

			public override void RemoveByKey ( string key ) {
				Owner.Controls.RemoveByKey ( key );
			}

			public override void SetChildIndex ( Control child, int newIndex ) {
				Owner.Controls.SetChildIndex ( child, newIndex );
			}

			public override Control this[ int index ] {
				get {
					return nodes[ index ];
				}
			}

			public override Control this[ string key ] {
				get {
					return Owner.Controls[ key ];
				}
			}
		}
		#endregion
		#region BreadcrumbBarButtonCollection
		public class BreadcrumbBarButtonCollection : ControlCollection {
			List<BreadcrumbBarButton> buttons;
			public BreadcrumbBarButtonCollection ( Control owner )
				: base ( owner ) {
				buttons = new List<BreadcrumbBarButton> ();
			}
			public override void Add ( Control value ) {
				if ( value is BreadcrumbBarButton ) {
					BreadcrumbBarButton v = value as BreadcrumbBarButton;
					v.Height = 20;
					v.Width = 24;
					v.Left = this.Owner.Width - ( v.Width * ( buttons.Count + 1 ) ) - 2;
					v.Anchor = AnchorStyles.Top | AnchorStyles.Right;
					v.Top = 1;
					v.Parent = Owner as BreadcrumbBar;
					buttons.Add ( v );
					Owner.Controls.Add ( v );
				}
			}

			public override int Count {
				get {
					return buttons.Count;
				}
			}

			public override void AddRange ( Control[] controls ) {
				foreach ( var item in controls ) {
					this.Add ( item );
				}
			}

			public override void Clear () {
				foreach ( var item in buttons ) {
					Owner.Controls.Remove ( item );
				}
				buttons.Clear ();
			}

			public override bool ContainsKey ( string key ) {
				return Owner.Controls.ContainsKey ( key );
			}

			public override System.Collections.IEnumerator GetEnumerator () {
				return buttons.GetEnumerator ();
			}

			public override int GetChildIndex ( Control child, bool throwException ) {
				return Owner.Controls.GetChildIndex ( child, throwException );
			}

			public override int IndexOfKey ( string key ) {
				return Owner.Controls.IndexOfKey ( key );
			}

			public override void Remove ( Control value ) {
				buttons.Remove ( value as BreadcrumbBarButton );
				Owner.Controls.Remove ( value );
			}

			public override void RemoveByKey ( string key ) {
				Owner.Controls.RemoveByKey ( key );
			}

			public override void SetChildIndex ( Control child, int newIndex ) {
				Owner.Controls.SetChildIndex ( child, newIndex );
			}

			public override Control this[ int index ] {
				get {
					return buttons[ index ];
				}
			}

			public override Control this[ string key ] {
				get {
					return Owner.Controls[ key ];
				}
			}
		}
		#endregion
		#endregion
	}
}
