using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using Vista.Controls.Design;

namespace Vista.Controls {
	[ToolboxBitmap(typeof(ExplorerNavigation),"explorernavigation.bmp")]
	public class ExplorerNavigation : Control {
		private bool _showClearHistory;

		public ExplorerNavigation () {
			this.SetStyle (
				ControlStyles.ResizeRedraw |
				ControlStyles.StandardClick |
				ControlStyles.SupportsTransparentBackColor, true );
			this.BackColor = Color.Transparent;
			this.History = new ExplorerNavigationHistory ();
			this.ToolTip = new ToolTip ();
			this.HistoryMenu = new ContextMenuStrip ();
			this.HistoryMenu.AutoSize = true;
			base.Size = this.Size;

			if ( !Areo.IsLegacyOS ) {
				this.HistoryMenu.Renderer = new WindowsVistaRenderer ();
			}

			this.HistoryIndex = -1;
			this.ShowClearHistory = true;

			this.History.ItemAdded += delegate ( object sender, EventArgs e ) {
				BuildContextMenu ();
			};

			this.History.ItemRemoved += delegate ( object sender, EventArgs e ) {
				BuildContextMenu ();
			};

			//this.HistoryMenu.MinimumSize = new Size ( this.Width, this.HistoryMenu.MinimumSize.Width );

			this.HistoryMenu.Opening += delegate ( object sender, CancelEventArgs e ) {
				foreach ( ToolStripItem xitem in HistoryMenu.Items ) {
					if ( xitem is ToolStripMenuItem && xitem.Tag is ExplorerNavigationHistoryItem ) {
						ExplorerNavigationHistoryItem hi = xitem.Tag as ExplorerNavigationHistoryItem;
						( xitem as ToolStripMenuItem ).Checked = false;
						( xitem as ToolStripMenuItem ).Font = new Font ( xitem.Font, FontStyle.Regular );
						( xitem as ToolStripMenuItem ).Image = hi.Image;
					}

				}
				if ( HistoryIndex >= 0 && HistoryIndex < History.Count ) {
					ToolStripMenuItem tsi = ( this.HistoryMenu.Items[ HistoryIndex ] as ToolStripMenuItem );
					tsi.Checked = true;
					tsi.Font = new Font ( tsi.Font, FontStyle.Bold );
				}
				this.IsMenuVisible = true;

			};


			this.HistoryMenu.Closing += delegate ( object sender, ToolStripDropDownClosingEventArgs e ) {
				this.IsMenuVisible = false;
				this.Invalidate ();
			};
		}

		~ExplorerNavigation () {

		}


		#region internal properties
		#region Sizes
		public new Size Size { get { return new Size ( 74, 29 ); } set { return; } }
		public new int Width { get { return this.Size.Width; } set { return; } }
		public new int Height { get { return this.Size.Height; } set { return; } }
		internal Size ButtonSize { get { return new Size ( 25, 25 ); } }

		internal Size MenuButtonSize { get { return new Size ( 22, 21 ); } }

		#endregion

		#region Images
		internal new Image BackgroundImage { get { return Properties.Resources.background2; } }

		internal Image LeftImage { get { return Properties.Resources.leftbuttons2; } }

		internal Image RightImage { get { return Properties.Resources.rightbuttons2; } }
		#endregion

		#region Background

		internal Rectangle BackgroundNoStateRectangle { get { return new Rectangle ( Point.Empty, this.Size ); } }

		internal Rectangle BackgroundHoverMenuRectangle {
			get {
				return new Rectangle ( BackgroundNoStateRectangle.Right, 0, BackgroundNoStateRectangle.Width, BackgroundNoStateRectangle.Height );
			}
		}

		internal Rectangle BackgroundClickMenuRectangle {
			get {
				return new Rectangle ( BackgroundHoverMenuRectangle.Right, 0, BackgroundHoverMenuRectangle.Width, BackgroundHoverMenuRectangle.Height );
			}
		}

		internal Rectangle BackgroundDisabledRectangle {
			get {
				return new Rectangle ( BackgroundClickMenuRectangle.Right, 0, BackgroundClickMenuRectangle.Width, BackgroundClickMenuRectangle.Height );
			}
		}
		#endregion

		#region MenuButton

		internal Rectangle MenuButtonBounds {
			get { return new Rectangle ( new Point ( 52, 4 ), this.MenuButtonSize ); }
		}

		internal bool IsMouseOverMenuButton { get; set; }
		#endregion

		#region RightButton
		internal Rectangle RightButtonBounds {
			get { return new Rectangle ( new Point ( 30, 2 ), this.ButtonSize ); }
		}

		internal bool RightEnabled {
			get { return HistoryIndex > 0; }
		}

		internal bool IsMouseOverRightButton { get; set; }

		internal Rectangle RightButtonNoStateRectangle {
			get {
				return LeftButtonNoStateRectangle;
			}
		}

		internal Rectangle RightButtonHoverRectangle {
			get {
				return LeftButtonHoverRectangle;
			}
		}

		internal Rectangle RightButtonClickRectangle {
			get {
				return LeftButtonClickRectangle;
			}
		}

		internal Rectangle RightButtonDisabledRectangle {
			get {
				return LeftButtonDisabledRectangle;
			}
		}
		#endregion

		#region LeftButton
		internal Rectangle LeftButtonBounds {
			get { return new Rectangle ( new Point ( 2, 2 ), this.ButtonSize ); }
		}

		internal bool LeftEnabled {
			get { return HistoryIndex < History.Count - 1 && ( HistoryIndex >= 0 && History.Count > 0 ); }
		}

		internal bool IsMouseOverLeftButton { get; set; }

		internal Rectangle LeftButtonNoStateRectangle {
			get {
				return new Rectangle ( new Point ( 0, 0 ), this.ButtonSize );
			}
		}

		internal Rectangle LeftButtonHoverRectangle {
			get {
				return new Rectangle ( new Point ( LeftButtonNoStateRectangle.Right, LeftButtonNoStateRectangle.Y ), this.ButtonSize );
			}
		}

		internal Rectangle LeftButtonClickRectangle {
			get {
				return new Rectangle ( new Point ( LeftButtonHoverRectangle.Right, LeftButtonHoverRectangle.Y ), this.ButtonSize );
			}
		}

		internal Rectangle LeftButtonDisabledRectangle {
			get {
				return new Rectangle ( new Point ( LeftButtonClickRectangle.Right, LeftButtonClickRectangle.Y ), this.ButtonSize );
			}
		}
		#endregion

		internal ContextMenuStrip HistoryMenu { get; set; }

		internal bool IsMouseDown { get; set; }
		internal bool IsMouseOver { get; set; }
		internal bool IsMenuVisible { get; set; }
		internal new bool Enabled { get { return this.LeftEnabled || this.RightEnabled || this.HasHistory; } }
		internal bool HasHistory { get { return this.History.Count > 0; } }
		internal int HistoryIndex { get; set; }
		internal ToolTip ToolTip { get; set; }
		#endregion

		#region public properties
		public ExplorerNavigationHistory History { get; private set; }
		/// <summary>
		/// Gets or sets if the background should be painted to support being positioned within the 
		/// "Glass" section of the form
		/// </summary>
		[Category ( "Appearance" ), DefaultValue ( false )]
		public bool PaintForGlass { get; set; }

		[Category ( "Appearance" ), DefaultValue ( true )]
		public bool ShowClearHistory {
			get { return this._showClearHistory; }
			set {
				this._showClearHistory = value;
				BuildContextMenu ();
			}
		}
		#endregion

		#region Overrides

		protected override void OnPaintBackground ( PaintEventArgs pevent ) {
			base.OnPaintBackground ( pevent );
		}


		protected override void OnDockChanged ( EventArgs e ) {
			if ( base.Dock != DockStyle.None ) {
				base.Dock = DockStyle.None;
			}
		}

		protected override void OnResize ( EventArgs e ) {
			if ( base.Width != this.Size.Width ) {
				base.Width = this.Size.Width;
			}

			if ( base.Height != this.Size.Height ) {
				base.Height = this.Size.Height;
			}
		}

		protected override void OnEnabledChanged ( EventArgs e ) {
			base.OnEnabledChanged ( e );
			this.Invalidate ();
		}

		protected override void OnClick ( EventArgs e ) {
			if ( this.Enabled ) {
				if ( this.IsMouseOverMenuButton && !this.IsMenuVisible && this.HasHistory ) {
					this.HistoryMenu.Show ( this, new Point ( this.ClientRectangle.Left, this.ClientRectangle.Bottom ), ToolStripDropDownDirection.Default );
				} else if ( this.IsMouseOverLeftButton && this.LeftEnabled ) {
					NavigateHistory ( HistoryIndex + 1 );
				} else if ( this.IsMouseOverRightButton && this.RightEnabled ) {
					NavigateHistory ( HistoryIndex - 1 );
				} else {
					base.OnClick ( e );
				}
			}
		}

		protected override void OnMouseClick ( MouseEventArgs e ) {
			if ( this.Enabled ) {
				base.OnMouseClick ( e );
			}
		}

		protected override void OnMouseDoubleClick ( MouseEventArgs e ) {
			if ( this.Enabled ) {
				base.OnMouseDoubleClick ( e );
			}
		}

		protected override void OnMouseDown ( MouseEventArgs e ) {
			if ( !this.IsMouseDown && this.Enabled ) {
				this.IsMouseDown = true;
				this.Invalidate ();
				base.OnMouseDown ( e );
			}
		}

		protected override void OnMouseEnter ( EventArgs e ) {
			if ( !this.IsMouseOver && this.Enabled ) {
				this.IsMouseOver = true;
				base.OnMouseEnter ( e );
				this.Invalidate ();
			}

		}

		protected override void OnMouseHover ( EventArgs e ) {
			if ( this.Enabled ) {
				base.OnMouseHover ( e );
			}
		}

		protected override void OnMouseLeave ( EventArgs e ) {
			if ( this.IsMouseOver && this.Enabled ) {
				this.IsMouseOver = this.IsMouseOverMenuButton = this.IsMouseOverRightButton = this.IsMouseOverLeftButton = false;
				base.OnMouseLeave ( e );
				this.Invalidate ();
			}

		}

		protected override void OnMouseMove ( MouseEventArgs e ) {
			if ( this.Enabled ) {
				this.IsMouseOverRightButton = Rectangle.Intersect ( new Rectangle ( this.PointToClient ( MousePosition ), new Size ( 1, 1 ) ),
					this.RightButtonBounds ) != Rectangle.Empty && this.RightEnabled;
				this.IsMouseOverLeftButton = Rectangle.Intersect ( new Rectangle ( this.PointToClient ( MousePosition ), new Size ( 1, 1 ) ),
					this.LeftButtonBounds ) != Rectangle.Empty && this.LeftEnabled;
				this.IsMouseOverMenuButton = Rectangle.Intersect ( new Rectangle ( this.PointToClient ( MousePosition ), new Size ( 1, 1 ) ),
					this.MenuButtonBounds ) != Rectangle.Empty && !this.IsMouseOverRightButton && this.HasHistory;

				if ( this.IsMouseOverRightButton && HistoryIndex - 1 >= 0 ) {
					ExplorerNavigationHistoryItem item = this.History[ HistoryIndex - 1 ];
					string caption = string.Format ( System.Globalization.CultureInfo.InvariantCulture, "Forward to {0}", item.Text );
					this.ToolTip.SetToolTip ( this, caption );
				} else if ( this.IsMouseOverLeftButton && HistoryIndex + 1 < this.History.Count ) {
					ExplorerNavigationHistoryItem item = this.History[ HistoryIndex + 1 ];
					string caption = string.Format ( System.Globalization.CultureInfo.InvariantCulture, "Back to {0}", item.Text );
					this.ToolTip.SetToolTip ( this, caption );
				} else {
					this.ToolTip.SetToolTip ( this, string.Empty );
				}

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
			Graphics g = e.Graphics;
			if ( Areo.IsGlassSupported && this.PaintForGlass ) {
				using ( SolidBrush blackBrush = new SolidBrush ( Color.Black ) ) {
					e.Graphics.FillRectangle ( blackBrush, this.ClientRectangle );
				}
			} else {
				using ( SolidBrush brush = new SolidBrush ( Color.Transparent ) ) {
					g.FillRectangle ( brush, this.ClientRectangle );
				}
			}

			if ( this.Enabled ) {
				// draw background
				if ( ( !this.IsMouseOverMenuButton && !this.IsMenuVisible ) || !HasHistory ) {
					g.DrawImage ( this.BackgroundImage, this.ClientRectangle, this.BackgroundNoStateRectangle, GraphicsUnit.Pixel );
				} else {
					if ( this.IsMouseDown || this.IsMenuVisible ) {
						g.DrawImage ( this.BackgroundImage, this.ClientRectangle, this.BackgroundClickMenuRectangle, GraphicsUnit.Pixel );
					} else {
						g.DrawImage ( this.BackgroundImage, this.ClientRectangle, this.BackgroundHoverMenuRectangle, GraphicsUnit.Pixel );
					}
				}

				// left button
				if ( !LeftEnabled ) {
					g.DrawImage ( this.LeftImage, this.LeftButtonBounds, this.LeftButtonDisabledRectangle, GraphicsUnit.Pixel );
				} else {
					if ( this.IsMouseOverLeftButton ) {
						if ( this.IsMouseDown ) {
							g.DrawImage ( this.LeftImage, this.LeftButtonBounds, this.LeftButtonClickRectangle, GraphicsUnit.Pixel );
						} else {
							g.DrawImage ( this.LeftImage, this.LeftButtonBounds, this.LeftButtonHoverRectangle, GraphicsUnit.Pixel );
						}
					} else {
						g.DrawImage ( this.LeftImage, this.LeftButtonBounds, this.LeftButtonNoStateRectangle, GraphicsUnit.Pixel );
					}
				}

				// right button
				if ( !RightEnabled ) {
					g.DrawImage ( this.RightImage, this.RightButtonBounds, this.RightButtonDisabledRectangle, GraphicsUnit.Pixel );
				} else {
					if ( this.IsMouseOverRightButton ) {
						if ( this.IsMouseDown ) {
							g.DrawImage ( this.RightImage, this.RightButtonBounds, this.RightButtonClickRectangle, GraphicsUnit.Pixel );
						} else {
							g.DrawImage ( this.RightImage, this.RightButtonBounds, this.RightButtonHoverRectangle, GraphicsUnit.Pixel );
						}
					} else {
						g.DrawImage ( this.RightImage, this.RightButtonBounds, this.RightButtonNoStateRectangle, GraphicsUnit.Pixel );
					}
				}
			} else {
				using ( SolidBrush brush = new SolidBrush ( Color.FromArgb ( 200, this.BackColor ) ) ) {
					g.DrawImage ( this.BackgroundImage, this.ClientRectangle, this.BackgroundDisabledRectangle, GraphicsUnit.Pixel );
					g.DrawImage ( this.LeftImage, this.LeftButtonBounds, this.LeftButtonDisabledRectangle, GraphicsUnit.Pixel );
					g.DrawImage ( this.RightImage, this.RightButtonBounds, this.RightButtonDisabledRectangle, GraphicsUnit.Pixel );
				}
			}
		}

		#endregion

		#region Public methods
		public void AddHistory ( ExplorerNavigationHistoryItem item ) {
			AddHistory ( item, false );
		}
		public void AddHistory ( ExplorerNavigationHistoryItem item, bool navigate ) {
			if ( navigate ) {
				this.History.Insert ( 0, item );
				this.HistoryIndex = 0;
			} else {
				this.History.Add ( item );
			}
			this.Invalidate ();
		}

		public void HistoryGo ( ExplorerNavigationHistoryItem item ) {
			NavigateHistory ( item );
		}

		public void HistoryGo ( int historyIndex ) {
			NavigateHistory ( historyIndex );
		}

		public void ClearHistory () {
			this.History.Clear ();
			this.HistoryIndex = -1;
			this.Invalidate ();
		}
		#endregion

		#region Private methods
		private void BuildContextMenu () {
			this.HistoryMenu.Items.Clear ();

			foreach ( var item in this.History ) {
				ToolStripMenuItem tsmi = new ToolStripMenuItem ( item.Text, item.Image, delegate ( object s, EventArgs ea ) {
					ToolStripMenuItem titem = ( s as ToolStripMenuItem );
					if ( titem != null ) {
						ExplorerNavigationHistoryItem hi = titem.Tag as ExplorerNavigationHistoryItem;
						foreach ( ToolStripItem xitem in HistoryMenu.Items ) {
							if ( xitem is ToolStripMenuItem && xitem.Tag is ExplorerNavigationHistoryItem ) {
								( xitem as ToolStripMenuItem ).Checked = false;
								( xitem as ToolStripMenuItem ).Font = new Font ( xitem.Font, FontStyle.Regular );
								( xitem as ToolStripMenuItem ).Image = hi.Image;
							}
						}
						titem.Checked = true;
						titem.Font = new Font ( titem.Font, FontStyle.Bold );
						NavigateHistory ( hi );
					}
				} );
				tsmi.MouseEnter += delegate ( object s, EventArgs e ) {
					ToolStripMenuItem titem = ( s as ToolStripMenuItem );
					if ( !titem.Checked ) {
						ExplorerNavigationHistoryItem i = titem.Tag as ExplorerNavigationHistoryItem;
						if ( this.History.IndexOf ( i ) < this.HistoryIndex ) {
							titem.Image = Properties.Resources.GoToNextHS;
						} else {
							titem.Image = Properties.Resources.GoToPrevious;
						}
					}
				};

				tsmi.MouseLeave += delegate ( object s, EventArgs e ) {
					ToolStripMenuItem titem = ( s as ToolStripMenuItem );
					if ( !titem.Checked ) {
						ExplorerNavigationHistoryItem i = titem.Tag as ExplorerNavigationHistoryItem;
						titem.Image = i.Image;
					}
				};
				tsmi.Tag = item;
				this.HistoryMenu.Items.Add ( tsmi );
			}

			if ( this.HistoryMenu.Items.Count == 1 ) {
				ToolStripMenuItem tsi = ( this.HistoryMenu.Items[ 0 ] as ToolStripMenuItem );
				tsi.Checked = true;
				tsi.Font = new Font ( tsi.Font, FontStyle.Bold );
				HistoryIndex = 0;
			}


			if ( this.HistoryMenu.Items.Count > 0 && this.ShowClearHistory ) {
				this.HistoryMenu.Items.Add ( new ToolStripSeparator () );
				this.HistoryMenu.Items.Add ( new ToolStripMenuItem ( "&Clear History", null, delegate ( object sender, EventArgs e ) {
					this.ClearHistory ();
				}, "clearHistory" ) );
			}
		}

		private void NavigateHistory ( int historyIndex ) {
			if ( historyIndex < History.Count && historyIndex >= 0 ) {
				HistoryIndex = historyIndex;
				ExplorerNavigationHistoryItem item = this.History[ historyIndex ];
				if ( item.NavigateDelegate != null ) {
					item.NavigateDelegate.Invoke ( item, EventArgs.Empty );
				}
				this.Invalidate ();
			}
		}

		private void NavigateHistory ( ExplorerNavigationHistoryItem item ) {
			NavigateHistory ( this.History.IndexOf ( item ) );
		}

		#endregion
	}
}
