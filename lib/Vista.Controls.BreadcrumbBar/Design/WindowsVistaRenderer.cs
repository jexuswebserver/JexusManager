using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Vista.Controls.Design {
	/// <summary>
	/// Renders toolstrip items using Windows Vista look and feel
	/// </summary>
	/// <remarks>
	/// 2007 Jos?Manuel Menéndez Poo
	/// Visit my blog for upgrades and other renderers - www.menendezpoo.com
	/// </remarks>
	public class WindowsVistaRenderer
			: ToolStripRenderer {
		#region Static

		/// <summary>
		/// Creates the glow of the buttons
		/// </summary>
		/// <param name="rectangle"></param>
		/// <returns></returns>
		private static GraphicsPath CreateBottomRadialPath ( Rectangle rectangle ) {
			GraphicsPath path = new GraphicsPath ();
			RectangleF rect = rectangle;
			rect.X -= rect.Width * .35f;
			rect.Y -= rect.Height * .15f;
			rect.Width *= 1.7f;
			rect.Height *= 2.3f;
			path.AddEllipse ( rect );
			path.CloseFigure ();
			return path;
		}

		/// <summary>
		/// Creates the chevron for the overflow button
		/// </summary>
		/// <param name="overflowButtonSize"></param>
		/// <returns></returns>
		private static GraphicsPath CreateOverflowChevron ( Size overflowButtonSize ) {
			Rectangle r = new Rectangle ( Point.Empty, overflowButtonSize );
			GraphicsPath path = new GraphicsPath ();

			int segmentWidth = 3;
			int segmentHeight = 3;
			int segmentSeparation = 5;
			int chevronWidth = segmentWidth + segmentSeparation;
			int chevronHeight = segmentHeight * 2;
			int chevronLeft = ( r.Width - chevronWidth ) / 2;
			int chevronTop = ( r.Height - chevronHeight ) / 2;

			// Segment \
			path.AddLine (
					new Point ( chevronLeft, chevronTop ),
					new Point ( chevronLeft + segmentWidth, chevronTop + segmentHeight ) );

			// Segment /
			path.AddLine (
					new Point ( chevronLeft + segmentWidth, chevronTop + segmentHeight ),
					new Point ( chevronLeft, chevronTop + segmentHeight * 2 ) );

			path.StartFigure ();

			// Segment \
			path.AddLine (
					new Point ( segmentSeparation + chevronLeft, chevronTop ),
					new Point ( segmentSeparation + chevronLeft + segmentWidth, chevronTop + segmentHeight ) );

			// Segment /
			path.AddLine (
					new Point ( segmentSeparation + chevronLeft + segmentWidth, chevronTop + segmentHeight ),
					new Point ( segmentSeparation + chevronLeft, chevronTop + segmentHeight * 2 ) );


			return path;
		}

		#endregion

		#region Fields

		private WindowsVistaColorTable _colorTable;
		private bool _glossyEffect;
		private bool _bgglow;
		private int _toolstripRadius;
		private int _buttonRadius;

		#endregion

		#region Ctor

		public WindowsVistaRenderer () {
			ColorTable = new WindowsVistaColorTable ();

			GlossyEffect = true;
			BackgroundGlow = true;
			ToolStripRadius = 2;
			ButtonRadius = 2;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the buttons rectangle radius
		/// </summary>
		public int ButtonRadius {
			get { return _buttonRadius; }
			set { _buttonRadius = value; }
		}

		/// <summary>
		/// Gets or sets the radius of the rectangle of the hole ToolStrip
		/// </summary>
		public int ToolStripRadius {
			get { return _toolstripRadius; }
			set { _toolstripRadius = value; }
		}

		/// <summary>
		/// Gets ors sets if background glow should be rendered
		/// </summary>
		public bool BackgroundGlow {
			get { return _bgglow; }
			set { _bgglow = value; }
		}

		/// <summary>
		/// Gets or sets if glossy effect should be rendered
		/// </summary>
		public bool GlossyEffect {
			get { return _glossyEffect; }
			set { _glossyEffect = value; }
		}

		/// <summary>
		/// Gets or sets the color table of the renderer
		/// </summary>
		public WindowsVistaColorTable ColorTable {
			get { return _colorTable; }
			set { _colorTable = value; }
		}


		#endregion

		#region Methods

		/// <summary>
		/// Initializes properties for ToolStripMenuItem objects
		/// </summary>
		/// <param name="item"></param>
		protected virtual void InitializeToolStripMenuItem ( ToolStripMenuItem item ) {
			item.AutoSize = true;
			item.Height = 26;
			item.TextAlign = ContentAlignment.MiddleLeft;

			foreach ( ToolStripItem subitem in item.DropDownItems ) {
				if ( subitem is ToolStripMenuItem ) {
					InitializeToolStripMenuItem ( subitem as ToolStripMenuItem );
				}
			}
		}

		/// <summary>
		/// Gets a rounded rectangle representing the hole area of the toolstrip
		/// </summary>
		/// <param name="toolStrip"></param>
		/// <returns></returns>
		private GraphicsPath GetToolStripRectangle ( ToolStrip toolStrip ) {
			return GraphicsTools.CreateRoundRectangle (
					new Rectangle ( 0, 0, toolStrip.Width - 1, toolStrip.Height - 1 ), ToolStripRadius );
		}

		/// <summary>
		/// Draws the glossy effect on the toolbar
		/// </summary>
		/// <param name="g"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		private void DrawGlossyEffect ( Graphics g, ToolStrip t ) {
			DrawGlossyEffect ( g, t, 0 );
		}

		/// <summary>
		/// Draws the glossy effect on the toolbar
		/// </summary>
		/// <param name="g"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		private void DrawGlossyEffect ( Graphics g, ToolStrip t, int offset ) {
			Rectangle glossyRect = new Rectangle ( 0, offset,
							t.Width - 1,
							( t.Height - 1 ) / 2 );

			using ( LinearGradientBrush b = new LinearGradientBrush (
					glossyRect.Location, new PointF ( 0, glossyRect.Bottom ),
					ColorTable.GlossyEffectNorth,
					ColorTable.GlossyEffectSouth ) ) {
				using ( GraphicsPath border =
                    GraphicsTools.CreateTopRoundRectangle ( glossyRect, ToolStripRadius ) ) {
					g.FillPath ( b, border );
				}
			}
		}

		/// <summary>
		/// Renders the background of a button
		/// </summary>
		/// <param name="e"></param>
		private void DrawVistaButtonBackground ( ToolStripItemRenderEventArgs e ) {
			bool chk = false;

			if ( e.Item is ToolStripButton ) {
				chk = ( e.Item as ToolStripButton ).Checked;
			}

			DrawVistaButtonBackground ( e.Graphics,
					new Rectangle ( Point.Empty, e.Item.Size ),
					e.Item.Selected,
					e.Item.Pressed,
					chk );
		}

		/// <summary>
		/// Renders the background of a button on the specified rectangle using the specified device
		/// </summary>
		/// <param name="e"></param>
		private void DrawVistaButtonBackground ( Graphics g, Rectangle r, bool selected, bool pressed, bool checkd ) {
			g.SmoothingMode = SmoothingMode.AntiAlias;

			Rectangle outerBorder = new Rectangle ( r.Left, r.Top, r.Width - 1, r.Height - 1 );
			Rectangle border = outerBorder; border.Inflate ( -1, -1 );
			Rectangle innerBorder = border; innerBorder.Inflate ( -1, -1 );
			Rectangle glossy = outerBorder; glossy.Height /= 2;
			Rectangle fill = innerBorder; fill.Height /= 2;
			Rectangle glow = Rectangle.FromLTRB ( outerBorder.Left,
					outerBorder.Top + Convert.ToInt32 ( Convert.ToSingle ( outerBorder.Height ) * .5f ),
					outerBorder.Right, outerBorder.Bottom );

			if ( selected || pressed || checkd ) {
				#region Layers

				//Outer border
				using ( GraphicsPath path =
                    GraphicsTools.CreateRoundRectangle ( outerBorder, ButtonRadius ) ) {
					using ( Pen p = new Pen ( ColorTable.ButtonOuterBorder ) ) {
						g.DrawPath ( p, path );
					}
				}

				//Checked fill
				if ( checkd ) {
					using ( GraphicsPath path = GraphicsTools.CreateRoundRectangle ( innerBorder, 2 ) ) {
						using ( Brush b = new SolidBrush ( selected ? ColorTable.CheckedButtonFillHot : ColorTable.CheckedButtonFill ) ) {
							g.FillPath ( b, path );
						}
					}
				}

				//Glossy effefct
				using ( GraphicsPath path = GraphicsTools.CreateTopRoundRectangle ( glossy, ButtonRadius ) ) {
					using ( Brush b = new LinearGradientBrush (
							new Point ( 0, glossy.Top ),
							new Point ( 0, glossy.Bottom ),
							ColorTable.GlossyEffectNorth,
							ColorTable.GlossyEffectSouth ) ) {
						g.FillPath ( b, path );
					}
				}

				//Border
				using ( GraphicsPath path =
                    GraphicsTools.CreateRoundRectangle ( border, ButtonRadius ) ) {
					using ( Pen p = new Pen ( ColorTable.ButtonBorder ) ) {
						g.DrawPath ( p, path );
					}
				}

				Color fillNorth = pressed ? ColorTable.ButtonFillNorthPressed : ColorTable.ButtonFillNorth;
				Color fillSouth = pressed ? ColorTable.ButtonFillSouthPressed : ColorTable.ButtonFillSouth;

				//Fill
				using ( GraphicsPath path = GraphicsTools.CreateTopRoundRectangle ( fill, ButtonRadius ) ) {
					using ( Brush b = new LinearGradientBrush (
							new Point ( 0, fill.Top ),
							new Point ( 0, fill.Bottom ),
							fillNorth, fillSouth ) ) {
						g.FillPath ( b, path );
					}
				}

				Color innerBorderColor = pressed || checkd ? ColorTable.ButtonInnerBorderPressed : ColorTable.ButtonInnerBorder;

				//Inner border
				using ( GraphicsPath path =
                    GraphicsTools.CreateRoundRectangle ( innerBorder, ButtonRadius ) ) {
					using ( Pen p = new Pen ( innerBorderColor ) ) {
						g.DrawPath ( p, path );
					}
				}

				//Glow
				using ( GraphicsPath clip = GraphicsTools.CreateRoundRectangle ( glow, 2 ) ) {
					g.SetClip ( clip, CombineMode.Intersect );

					Color glowColor = ColorTable.Glow;

					if ( checkd ) {
						if ( selected ) {
							glowColor = ColorTable.CheckedGlowHot;
						} else {
							glowColor = ColorTable.CheckedGlow;
						}
					}

					using ( GraphicsPath brad = CreateBottomRadialPath ( glow ) ) {
						using ( PathGradientBrush pgr = new PathGradientBrush ( brad ) ) {
							unchecked {
								int opacity = 255;
								RectangleF bounds = brad.GetBounds ();
								pgr.CenterPoint = new PointF ( ( bounds.Left + bounds.Right ) / 2f, ( bounds.Top + bounds.Bottom ) / 2f );
								pgr.CenterColor = Color.FromArgb ( opacity, glowColor );
								pgr.SurroundColors = new Color[] { Color.FromArgb ( 0, glowColor ) };
							}
							g.FillPath ( pgr, brad );
						}
					}
					g.ResetClip ();
				}




				#endregion
			}
		}

		/// <summary>
		/// Draws the background of a menu, vista style
		/// </summary>
		/// <param name="e"></param>
		private void DrawVistaMenuBackground ( ToolStripItemRenderEventArgs e ) {

			DrawVistaMenuBackground ( e.Graphics,
			new Rectangle ( Point.Empty, e.Item.Size ),
			e.Item.Selected, e.Item.Owner is MenuStrip );

		}

		/// <summary>
		/// Draws the background of a menu, vista style
		/// </summary>
		/// <param name="g"></param>
		/// <param name="r"></param>
		/// <param name="highlighted"></param>
		private void DrawVistaMenuBackground ( Graphics g, Rectangle r, bool highlighted, bool isMainMenu ) {
			//g.Clear(ColorTable.MenuBackground);

			int margin = 2;
			int left = 22;

			#region IconSeparator

			if ( !isMainMenu ) {
				using ( Pen p = new Pen ( ColorTable.MenuDark ) ) {
					g.DrawLine ( p,
											new Point ( r.Left + left, r.Top ),
											new Point ( r.Left + left, r.Height - margin ) );
				}


				using ( Pen p = new Pen ( ColorTable.MenuLight ) ) {
					g.DrawLine ( p,
											new Point ( r.Left + left + 1, r.Top ),
											new Point ( r.Left + left + 1, r.Height - margin ) );
				}
			}

			#endregion

			if ( highlighted ) {
				#region Draw Rectangle

				using ( GraphicsPath path = GraphicsTools.CreateRoundRectangle (
						new Rectangle ( r.X + margin, r.Y + margin, r.Width - margin * 2, r.Height - margin * 2 ), 3 ) ) {

					using ( Brush b = new LinearGradientBrush (
							new Point ( 0, 2 ), new Point ( 0, r.Height - 2 ),
							ColorTable.MenuHighlightNorth,
							ColorTable.MenuHighlightSouth ) ) {
						g.FillPath ( b, path );
					}

					using ( Pen p = new Pen ( ColorTable.MenuHighlight ) ) {
						g.DrawPath ( p, path );
					}
				}

				#endregion
			}

		}

		/// <summary>
		/// Draws the border of the vista menu window
		/// </summary>
		/// <param name="g"></param>
		/// <param name="r"></param>
		private void DrawVistaMenuBorder ( Graphics g, Rectangle r ) {
			using ( Pen p = new Pen ( ColorTable.BackgroundBorder ) ) {
				g.DrawRectangle ( p,
						new Rectangle ( r.Left, r.Top, r.Width - 1, r.Height - 1 ) );
			}
		}

		#endregion

		protected override void Initialize ( ToolStrip toolStrip ) {
			base.Initialize ( toolStrip );

			toolStrip.AutoSize = true;
			toolStrip.Height = 35;
			toolStrip.ForeColor = ColorTable.Text;
			toolStrip.GripStyle = ToolStripGripStyle.Hidden;
		}

		protected override void InitializeItem ( ToolStripItem item ) {
			base.InitializeItem ( item );

			//Don't Affect ForeColor of TextBoxes and ComboBoxes
			if ( !( ( item is ToolStripTextBox ) || ( item is ToolStripComboBox ) ) ) {
				item.ForeColor = ColorTable.Text;
			}

			item.Padding = new Padding ( 5 );

			if ( item is ToolStripSplitButton ) {
				ToolStripSplitButton btn = item as ToolStripSplitButton;
				btn.DropDownButtonWidth = 18;

				foreach ( ToolStripItem subitem in btn.DropDownItems ) {
					if ( subitem is ToolStripMenuItem ) {
						InitializeToolStripMenuItem ( subitem as ToolStripMenuItem );
					}
				}
			}

			if ( item is ToolStripDropDownButton ) {
				ToolStripDropDownButton btn = item as ToolStripDropDownButton;
				btn.ShowDropDownArrow = false;

				foreach ( ToolStripItem subitem in btn.DropDownItems ) {
					if ( subitem is ToolStripMenuItem ) {
						InitializeToolStripMenuItem ( subitem as ToolStripMenuItem );
					}
				}
			}
		}

		protected override void OnRenderToolStripBorder ( ToolStripRenderEventArgs e ) {

			if ( e.ToolStrip is ToolStripDropDownMenu ) {
				#region Draw Rectangled Border

				DrawVistaMenuBorder ( e.Graphics,
						new Rectangle ( Point.Empty, e.ToolStrip.Size ) );

				#endregion
			} else {
				#region Draw Rounded Border
				e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

				using ( GraphicsPath path = GetToolStripRectangle ( e.ToolStrip ) ) {
					using ( Pen p = new Pen ( ColorTable.BackgroundBorder ) ) {
						e.Graphics.DrawPath ( p, path );
					}
				}
				#endregion
			}


		}

		protected override void OnRenderToolStripBackground ( ToolStripRenderEventArgs e ) {
			if ( e.ToolStrip is ToolStripDropDownMenu ) {
				return;
			}

			#region Background

			using ( LinearGradientBrush b = new LinearGradientBrush (
					Point.Empty, new PointF ( 0, e.ToolStrip.Height ),
					ColorTable.BackgroundNorth,
					ColorTable.BackgroundSouth ) ) {
				using ( GraphicsPath border = GetToolStripRectangle ( e.ToolStrip ) ) {
					e.Graphics.FillPath ( b, border );
				}
			}

			#endregion

			if ( GlossyEffect ) {
				#region Glossy Effect

				DrawGlossyEffect ( e.Graphics, e.ToolStrip, 1 );

				#endregion
			}

			if ( BackgroundGlow ) {
				#region BackroundGlow

				int glowSize = Convert.ToInt32 ( Convert.ToSingle ( e.ToolStrip.Height ) * 0.15f );
				Rectangle glow = new Rectangle ( 0,
						e.ToolStrip.Height - glowSize - 1,
						e.ToolStrip.Width - 1,
						glowSize );

				using ( LinearGradientBrush b = new LinearGradientBrush (
						new Point ( 0, glow.Top - 1 ), new PointF ( 0, glow.Bottom ),
						Color.FromArgb ( 0, ColorTable.BackgroundGlow ),
						ColorTable.BackgroundGlow ) ) {
					using ( GraphicsPath border =
                        GraphicsTools.CreateBottomRoundRectangle ( glow, ToolStripRadius ) ) {
						e.Graphics.FillPath ( b, border );
					}
				}


				#endregion
			}

		}

		protected override void OnRenderItemText ( ToolStripItemTextRenderEventArgs e ) {

			if ( e.Item is ToolStripButton ) {
				e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			}

			if ( e.Item is ToolStripMenuItem
					&& !( e.Item.Owner is MenuStrip ) ) {
				Rectangle r = new Rectangle ( e.TextRectangle.Location, new Size ( e.TextRectangle.Width, 24 ) );
				e.TextRectangle = r;
				e.TextColor = ColorTable.MenuText;
			}

			base.OnRenderItemText ( e );
		}

		protected override void OnRenderButtonBackground ( ToolStripItemRenderEventArgs e ) {
			DrawVistaButtonBackground ( e );
		}

		protected override void OnRenderDropDownButtonBackground ( ToolStripItemRenderEventArgs e ) {
			DrawVistaButtonBackground ( e );

			ToolStripDropDownButton item = e.Item as ToolStripDropDownButton; if ( item == null ) return;

			Rectangle arrowBounds = new Rectangle ( item.Width - 18, 0, 18, item.Height );

			DrawArrow ( new ToolStripArrowRenderEventArgs (
					e.Graphics, e.Item,
					arrowBounds,
					ColorTable.DropDownArrow, ArrowDirection.Down ) );
		}

		protected override void OnRenderSplitButtonBackground ( ToolStripItemRenderEventArgs e ) {
			DrawVistaButtonBackground ( e );

			ToolStripSplitButton item = e.Item as ToolStripSplitButton; if ( item == null ) return;

			Rectangle arrowBounds = item.DropDownButtonBounds;
			Rectangle buttonBounds = new Rectangle ( item.ButtonBounds.Location, new Size ( item.ButtonBounds.Width + 2, item.ButtonBounds.Height ) );
			Rectangle dropDownBounds = item.DropDownButtonBounds;

			DrawVistaButtonBackground ( e.Graphics, buttonBounds, item.ButtonSelected,
					item.ButtonPressed, false );

			DrawVistaButtonBackground ( e.Graphics, dropDownBounds, item.DropDownButtonSelected,
					item.DropDownButtonPressed, false );

			DrawArrow ( new ToolStripArrowRenderEventArgs (
					e.Graphics, e.Item,
					arrowBounds,
					ColorTable.DropDownArrow, ArrowDirection.Down ) );
		}

		protected override void OnRenderItemImage ( ToolStripItemImageRenderEventArgs e ) {
			if ( !e.Item.Enabled ) {
				base.OnRenderItemImage ( e );
			} else {
				if ( e.Image != null ) {
					e.Graphics.DrawImage ( e.Image, e.ImageRectangle );
				}
			}

		}

		protected override void OnRenderMenuItemBackground ( ToolStripItemRenderEventArgs e ) {
			if ( e.Item.Owner is MenuStrip ) {
				DrawVistaButtonBackground ( e );
			} else {
				DrawVistaMenuBackground ( e.Graphics,
					 new Rectangle ( Point.Empty, new Size(e.Item.Owner.Bounds.Width -1,e.Item.Bounds.Height) ),
					 e.Item.Selected, e.Item.Owner is MenuStrip );

			}


		}

		protected override void OnRenderSeparator ( ToolStripSeparatorRenderEventArgs e ) {
			if ( e.Item.IsOnDropDown ) {
				int left = 20;
				int right = e.Item.Width - 3;
				int top = e.Item.Height / 2; top--;
				//e.Graphics.Clear(ColorTable.MenuBackground);
				using ( Pen p = new Pen ( ColorTable.MenuDark ) ) {
					e.Graphics.DrawLine ( p,
							new Point ( left, top ),
							new Point ( right, top ) );
				}

				using ( Pen p = new Pen ( ColorTable.MenuLight ) ) {
					e.Graphics.DrawLine ( p,
							new Point ( left, top + 1 ),
							new Point ( right, top + 1 ) );
				}
			} else {
				int top = 3;
				int left = e.Item.Width / 2; left--;
				int height = e.Item.Height - top * 2;
				RectangleF separator = new RectangleF ( left, top, 0.5f, height );

				using ( Brush b = new LinearGradientBrush (
						separator.Location,
						new Point ( Convert.ToInt32 ( separator.Left ), Convert.ToInt32 ( separator.Bottom ) ),
						ColorTable.SeparatorNorth, ColorTable.SeparatorSouth ) ) {
					e.Graphics.FillRectangle ( b, separator );
				}
			}
		}

		protected override void OnRenderOverflowButtonBackground ( ToolStripItemRenderEventArgs e ) {
			DrawVistaButtonBackground ( e );

			//Chevron is obtained from the character: ?(Alt+0187)
			using ( Brush b = new SolidBrush ( e.Item.ForeColor ) ) {
				StringFormat sf = new StringFormat ();
				sf.Alignment = StringAlignment.Center;
				sf.LineAlignment = StringAlignment.Center;

				Font f = new Font ( e.Item.Font.FontFamily, 15 );

				e.Graphics.DrawString ("\u00BB", f, b, new RectangleF ( Point.Empty, e.Item.Size ), sf );
			}

		}

		protected override void OnRenderArrow ( ToolStripArrowRenderEventArgs e ) {
			if ( e.Item is ToolStripMenuItem && e.Item.Selected ) {
				e.ArrowColor = ColorTable.MenuText;
			}

			base.OnRenderArrow ( e );
		}
	}
}
