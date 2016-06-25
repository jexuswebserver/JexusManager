using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Vista.Controls {
	public class ExplorerAddressNavigation : Control {

		#region fields
		private bool _dockInGlass = false;
		private bool _showRefresh = true;
		#endregion

		#region events
		public event EventHandler DockOnGlassChanged;
		public event  EventHandler RefreshClick;
		#endregion

		public ExplorerAddressNavigation () {
			this.SetStyle ( ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true );
			InitializeComponents ();
		}

		#region Internal properties
		[TypeConverter ( typeof ( ExpandableObjectConverter ) )]
		public ExplorerNavigation Navigation { get; set; }
		[TypeConverter ( typeof ( ExpandableObjectConverter ) )]
		public BreadcrumbBar Address { get; set; }

		#endregion

		#region Public properties

		public bool ShowRefresh {
			get {
				return this._showRefresh;
			}
			set {
				bool oldVal = this.ShowRefresh;
				if ( oldVal != value ) {
					this._showRefresh = value;
					this.Invalidate ( true );
				}
			}
		}

		public bool DockOnGlass {
			get {
				return this._dockInGlass;
			}
			set {
				bool oldVal = this.DockOnGlass;
				if ( oldVal != value ) {
					this._dockInGlass = value;
					OnDockOnGlassChanged ( EventArgs.Empty );
					this.Invalidate ( true );
				}
			}
		}
		#endregion

		#region protected event handlers
		protected virtual void OnDockOnGlassChanged ( EventArgs e ) {

			if ( this.DockOnGlass ) {
				Form f = this.FindForm ();
				if ( f != null ) {
					f.ExtendFrameIntoClientArea ( this );
					this.Navigation.PaintForGlass = true;
				} else {
					// when the handle is created fire this event again.
					this.HandleCreated += delegate ( object sender, EventArgs e1 ) {
						OnDockOnGlassChanged ( e );
					};
				}
			} else {
				this.BackColor = SystemColors.Control;
				this.Navigation.PaintForGlass = false;
			}

			if ( DockOnGlassChanged != null ) {
				DockOnGlassChanged ( this, e );
			}
		}

		void ExplorerAddressNavigation_HandleCreated ( object sender, EventArgs e ) {
			throw new NotImplementedException ();
		}

		protected void OnRefreshClick ( object sender, EventArgs e ) {
			if ( this.RefreshClick != null ) {
				this.RefreshClick ( this, e );
			}
		}

		#endregion

		#region Private methods
		private void InitializeComponents () {
			this.Height = 34;
			this.Width = 150;

			this.Navigation = new ExplorerNavigation ();
			this.Address = new BreadcrumbBar ();

			this.Navigation.Anchor = AnchorStyles.Left | AnchorStyles.Top;
			this.Navigation.BackColor = System.Drawing.Color.Transparent;
			this.Navigation.Location = new System.Drawing.Point ( 0, 0 );
			this.Navigation.Name = "Navigation";
			this.Navigation.Invalidate ();

			this.Navigation.Padding = new Padding ( 1, 3, 1, 3 );
			this.Address.Padding = new Padding ( 1, 3, 1, 3 );
			this.Padding = new Padding ( 1, 3, 1, 3 );

			this.Address.Location = new Point ( this.Navigation.Width + 1, 3 );
			this.Address.Size = new Size ( this.Width - this.Navigation.Width - 2, 22 );
			this.Address.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

			BreadcrumbBarButton refresh = new BreadcrumbBarButton ();
			refresh.Image = Properties.Resources.refresh;
			refresh.Click += new EventHandler ( OnRefreshClick );

			this.Address.Buttons.Add ( refresh );

			this.Controls.Add ( this.Address );
			this.Controls.Add ( this.Navigation );

			OnDockOnGlassChanged ( EventArgs.Empty );
		}
		#endregion
	}
}
