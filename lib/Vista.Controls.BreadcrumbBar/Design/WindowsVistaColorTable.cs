using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Vista.Controls.Design {
	/// <summary>
	/// Provides colors used by WindowsVista style rendering
	/// </summary>
	/// <remarks>
	/// 2007 José Manuel Menéndez Poo
	/// Visit my blog for upgrades and other renderers - www.menendezpoo.com
	/// </remarks>
	public class WindowsVistaColorTable {
		#region Fields
		private Color _bgNorth;
		private Color _bgSouth;
		private Color _glossyNorth;
		private Color _glossySouth;
		private Color _bgborder;
		private Color _bgglow;
		private Color _text;
		private Color _buttonInnerBorder;
		private Color _buttonBorder;
		private Color _buttonOuterBorder;
		private Color _buttonFill;
		private Color _buttonFillPressed;
		private Color _glow;
		private Color _buttonInnerBorderPressed;
		private Color _buttonFillSouth;
		private Color _buttonFillSouthPressed;
		private Color _dropDownArrow;
		private Color _menuHighlight;
		private Color _menuHiglightNorth;
		private Color _menuHighlightSouth;
		private Color _menuBackground;
		private Color _menuDark;
		private Color _menuLight;
		private Color _separatorNorth;
		private Color _separatorSouth;
		private Color _menuText;
		private Color _checkedGlow;
		private Color _checkedButtonFill;
		private Color _checkedButtonFillHot;
		private Color _checkedGlowHot;

		#endregion

		#region Ctor

		public WindowsVistaColorTable () {
			BackgroundNorth = Color.Black;
			BackgroundSouth = Color.Black;

			GlossyEffectNorth = Color.FromArgb ( 217, 0x68, 0x7C, 0xAC );
			GlossyEffectSouth = Color.FromArgb ( 74, 0xAA, 0xB5, 0xD0 );

			BackgroundBorder = Color.FromArgb ( 0x85, 0x85, 0x87 );
			BackgroundGlow = Color.FromArgb ( 0x43, 0x53, 0x7A );

			Text = Color.White;

			ButtonOuterBorder = Color.FromArgb ( 0x75, 0x7D, 0x95 );
			ButtonInnerBorder = Color.FromArgb ( 0xBF, 0xC4, 0xCE );
			ButtonInnerBorderPressed = Color.FromArgb ( 0x4b, 0x4b, 0x4b );
			ButtonBorder = Color.FromArgb ( 0x03, 0x07, 0x0D );
			ButtonFillNorth = Color.FromArgb ( 85, Color.White );
			ButtonFillSouth = Color.FromArgb ( 1, Color.White );
			ButtonFillNorthPressed = Color.FromArgb ( 150, Color.Black );
			ButtonFillSouthPressed = Color.FromArgb ( 100, Color.Black );

			Glow = Color.FromArgb ( 0x30, 0x73, 0xCE );
			DropDownArrow = Color.White;

			MenuHighlight = Color.FromArgb ( 0xA8, 0xD8, 0xEB );
			MenuHighlightNorth = Color.FromArgb ( 25, MenuHighlight );
			MenuHighlightSouth = Color.FromArgb ( 102, MenuHighlight );
			MenuBackground = Color.FromArgb ( 0xF1, 0xF1, 0xF1 );
			MenuDark = Color.FromArgb ( 0xE2, 0xE3, 0xE3 );
			MenuLight = Color.White;

			SeparatorNorth = BackgroundSouth;
			SeparatorSouth = GlossyEffectNorth;

			MenuText = Color.Black;

			CheckedGlow = Color.FromArgb ( 0x57, 0xC6, 0xEF );
			CheckedGlowHot = Color.FromArgb ( 0x70, 0xD4, 0xFF );
			CheckedButtonFill = Color.FromArgb ( 0x18, 0x38, 0x9E );
			CheckedButtonFillHot = Color.FromArgb ( 0x0F, 0x3A, 0xBF );

		}

		#endregion

		#region Properties


		public Color CheckedGlowHot {
			get { return _checkedGlowHot; }
			set { _checkedGlowHot = value; }
		}


		public Color CheckedButtonFillHot {
			get { return _checkedButtonFillHot; }
			set { _checkedButtonFillHot = value; }
		}


		public Color CheckedButtonFill {
			get { return _checkedButtonFill; }
			set { _checkedButtonFill = value; }
		}


		public Color CheckedGlow {
			get { return _checkedGlow; }
			set { _checkedGlow = value; }
		}


		public Color MenuText {
			get { return _menuText; }
			set { _menuText = value; }
		}


		public Color SeparatorNorth {
			get { return _separatorNorth; }
			set { _separatorNorth = value; }
		}


		public Color SeparatorSouth {
			get { return _separatorSouth; }
			set { _separatorSouth = value; }
		}


		public Color MenuLight {
			get { return _menuLight; }
			set { _menuLight = value; }
		}


		public Color MenuDark {
			get { return _menuDark; }
			set { _menuDark = value; }
		}


		public Color MenuBackground {
			get { return _menuBackground; }
			set { _menuBackground = value; }
		}


		public Color MenuHighlightSouth {
			get { return _menuHighlightSouth; }
			set { _menuHighlightSouth = value; }
		}


		public Color MenuHighlightNorth {
			get { return _menuHiglightNorth; }
			set { _menuHiglightNorth = value; }
		}


		public Color MenuHighlight {
			get { return _menuHighlight; }
			set { _menuHighlight = value; }
		}

		/// <summary>
		/// Gets or sets the color for the dropwown arrow
		/// </summary>
		public Color DropDownArrow {
			get { return _dropDownArrow; }
			set { _dropDownArrow = value; }
		}


		/// <summary>
		/// Gets or sets the south color of the button fill when pressed
		/// </summary>
		public Color ButtonFillSouthPressed {
			get { return _buttonFillSouthPressed; }
			set { _buttonFillSouthPressed = value; }
		}

		/// <summary>
		/// Gets or sets the south color of the button fill
		/// </summary>
		public Color ButtonFillSouth {
			get { return _buttonFillSouth; }
			set { _buttonFillSouth = value; }
		}

		/// <summary>
		/// Gets or sets the color of the inner border when pressed
		/// </summary>
		public Color ButtonInnerBorderPressed {
			get { return _buttonInnerBorderPressed; }
			set { _buttonInnerBorderPressed = value; }
		}

		/// <summary>
		/// Gets or sets the glow color
		/// </summary>
		public Color Glow {
			get { return _glow; }
			set { _glow = value; }
		}

		/// <summary>
		/// Gets or sets the buttons fill color
		/// </summary>
		public Color ButtonFillNorth {
			get { return _buttonFill; }
			set { _buttonFill = value; }
		}

		/// <summary>
		/// Gets or sets the buttons fill color when pressed
		/// </summary>
		public Color ButtonFillNorthPressed {
			get { return _buttonFillPressed; }
			set { _buttonFillPressed = value; }
		}

		/// <summary>
		/// Gets or sets the buttons inner border color
		/// </summary>
		public Color ButtonInnerBorder {
			get { return _buttonInnerBorder; }
			set { _buttonInnerBorder = value; }
		}

		/// <summary>
		/// Gets or sets the buttons border color
		/// </summary>
		public Color ButtonBorder {
			get { return _buttonBorder; }
			set { _buttonBorder = value; }
		}

		/// <summary>
		/// Gets or sets the buttons outer border color
		/// </summary>
		public Color ButtonOuterBorder {
			get { return _buttonOuterBorder; }
			set { _buttonOuterBorder = value; }
		}

		/// <summary>
		/// Gets or sets the color of the text
		/// </summary>
		public Color Text {
			get { return _text; }
			set { _text = value; }
		}

		/// <summary>
		/// Gets or sets the background glow color
		/// </summary>
		public Color BackgroundGlow {
			get { return _bgglow; }
			set { _bgglow = value; }
		}

		/// <summary>
		/// Gets or sets the color of the background border
		/// </summary>
		public Color BackgroundBorder {
			get { return _bgborder; }
			set { _bgborder = value; }
		}

		/// <summary>
		/// Background north part
		/// </summary>
		public Color BackgroundNorth {
			get { return _bgNorth; }
			set { _bgNorth = value; }
		}

		/// <summary>
		/// Background south color
		/// </summary>
		public Color BackgroundSouth {
			get { return _bgSouth; }
			set { _bgSouth = value; }
		}

		/// <summary>
		/// Gets ors sets the glossy effect north color
		/// </summary>
		public Color GlossyEffectNorth {
			get { return _glossyNorth; }
			set { _glossyNorth = value; }
		}

		/// <summary>
		/// Gets or sets the glossy effect south color
		/// </summary>
		public Color GlossyEffectSouth {
			get { return _glossySouth; }
			set { _glossySouth = value; }
		}


		#endregion
	}
}
