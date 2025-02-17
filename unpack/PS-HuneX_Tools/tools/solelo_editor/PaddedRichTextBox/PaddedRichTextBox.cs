// SHOWMSGS is used for debugging purposes to display a user-selected subset
// of the system messages passed to the control.
#define SHOWMSGS

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;

namespace MyControls
{
    /// <summary>
    /// This control inherits from the standard RichTextBox but adds the
    /// BorderColor, BorderWidth, FixedSingleLineColor and FixedSingleLineWidth 
    /// properties. By using the same color for BorderColor as the text box 
    /// background you effectively provide a uniform margin around the text.  
    /// Using different colors for the BorderColor and FixedSingleLineColor
    /// (which is only visible when the BorderStyle is FixedSingle) can make
    /// a text box be more visually appealing and make it stand out.
    /// </summary>
    // To display a custom bitmap add: [ToolboxBitmap(typeof(TestControl), "namespace.TestControl.bmp")]
    [DefaultPropertyAttribute("BorderColor"), 
     ToolboxBitmap(typeof(PaddedRichTextBox), "MyControls.PaddedRichTextBox.ico")]
    public class PaddedRichTextBox : System.Windows.Forms.RichTextBox
    {
        #region Declarations
        private System.ComponentModel.Container components = null;
        private int m_BorderWidth = 5;
        private int m_FixedSingleLineWidth = 1;
        private Color m_BorderColor = Color.WhiteSmoke;
        private Color m_FixedSingleLineColor = Color.CadetBlue;
        private NCCALCSIZE_PARAMS nccsp;
        private Timer timer = new Timer();

        private bool doRedraw = false;
        private bool hideCaret = false;
        private bool first = true;
        private bool doPaint = false;

        private readonly uint setWindowPosFlags = 
            (uint)(SWP.SWP_NOMOVE | SWP.SWP_NOSIZE | SWP.SWP_NOZORDER | SWP.SWP_FRAMECHANGED);  // 2633

        #endregion Declarations

        #region DEBUG declarations
#if DEBUG
        private int paintCount = 0;
        private int redrawCount = 0;
#endif
        #endregion DEBUG declarations

        #region Deprecated declarations
        // This application specific message number is used to force the control to be
        // recreated when it is resized. If your application is already using this message
        // number then you will want to change this one to an unused value:
        // const int REDRAW_MSG_NUMBER = (int)Win32Messages.WM_APP + 1;
        #endregion Deprecated declarations

        #region Custom properties: BorderWidth, BorderColor, FixedSingleLineColor, FixedSingleLineWidth

        [Browsable(true), CategoryAttribute("Appearance"), 
         Description("This is the width of the outer border around the text box.")]
        public int BorderWidth
        {
            get { return m_BorderWidth; }
            set
            {
                m_BorderWidth = value >= 0 ? value : 0;         // BorderWidth must not be negative
                Redraw();                                 
            }
        }

        [Browsable(true), CategoryAttribute("Appearance"),
         Description("This is the color of the outer border around the text box.")]
        public Color BorderColor
        {
            get { return m_BorderColor; }
            set
            {
                m_BorderColor = value;
                Redraw(); 
            }
        }

        [Browsable(true), CategoryAttribute("Appearance"),
         Description("This is the color of the inner line border around the text box when the BorderStyle is FixedSingle.")]
        public Color FixedSingleLineColor
        {
            get { return m_FixedSingleLineColor; }
            set
            {
                m_FixedSingleLineColor = value;
                Redraw();  
            }
        }

        [Browsable(true), CategoryAttribute("Appearance"),
        Description("This is the width of the inner line border around the text box when the BorderStyle is FixedSingle.")]
        public int FixedSingleLineWidth
        {
            get { return m_FixedSingleLineWidth; }
            set
            {
                m_FixedSingleLineWidth = value >= 0 ? value : 0;         // FixedSingleLineWidth must not be negative
                Redraw();
            }
        }

        #endregion Custom properties: BorderWidth, BorderColor, FixedSingleLineColor, FixedSingleLineWidth

        #region Win32 API Prototypes
        // 2633 -- Start
        [DllImport("User32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
                                               int X, int Y, int cx, int cy, uint uFlags);
        [Flags]
        public enum SWP
        {
            SWP_NOSIZE = 0x0001,
            SWP_NOMOVE = 0x0002,
            SWP_NOZORDER = 0x0004,
            SWP_NOREDRAW = 0x0008,
            SWP_NOACTIVATE = 0x0010,
            SWP_FRAMECHANGED = 0x0020,
            SWP_SHOWWINDOW = 0x0040,
            SWP_HIDEWINDOW = 0x0080,
            SWP_NOCOPYBITS = 0x0100,
            SWP_NOOWNERZORDER = 0x0200,
            SWP_NOSENDCHANGING = 0x0400,
            SWP_DRAWFRAME = SWP_FRAMECHANGED,
            SWP_NOREPOSITION = SWP_NOOWNERZORDER,
            SWP_DEFERERASE = 0x2000,
            SWP_ASYNCWINDOWPOS = 0x4000
        }
        // 2633 -- End

        [DllImport("User32.dll")]
        public extern static IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("User32.dll")]
        public extern static int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32")]
        private static extern bool HideCaret(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential)]                   // This is the default layout for a structure
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        /// <summary>
        /// The NCCALCSIZE_PARAMS structure contains information that an application can use 
        /// while processing the WM_NCCALCSIZE message to calculate the size, position, and 
        /// valid contents of the client area of a window. 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]                   // This is the default layout for a structure
        public struct NCCALCSIZE_PARAMS
        {
            public RECT rect0, rect1, rect2;                    // Can't use an array here so simulate one
            public IntPtr lppos;
        }

        #endregion Win32 API Prototypes

        #region Constructor
        public PaddedRichTextBox()
        {
            InitializeComponent();
            timer.Interval = 200;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Enabled = true;
        }

        #endregion Constructor

        protected override void Dispose(bool disposing)
        {
            timer.Enabled = false;                              // 2636 -- Make sure this is disabled before exiting
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
        #endregion

        #region Message processing: WndProc
        protected override void WndProc(ref Message m)
        {
            #region WndProc debug code
            // Use the following code in debug mode to view the windows messages by name optionally excluding some:
            #if DEBUG && SHOWMSGS
                string s = MsgType.GetMessageName(m.Msg, false,
                                // Messages containing the following substrings will be ignored:
                                "WM_USER", "WM_REFLECT", "NOTIFY", "TIMER", "HIT", "MOUSE", "CURSOR", "FOCUS", "IME_", "_KEY", "_CHAR", "_GETDLG");
                Debug.WriteLineIf(s.Length > 0, s);
            #endif
            #endregion WndProc debug code

            switch (m.Msg)
            {
                #region case WM_NCCALCSIZE:
                case (int)Win32Messages.WM_NCCALCSIZE:
                    #region Non-client area definition
                    /*
                     * The non-client area is the part of the window managed by the operating 
                     * system and responsible for the appearance of borders, size-grippers and 
                     * title-bars. This can be managed in Windows Forms by overriding WndProc and 
                     * handling the WM_NCCALCSIZE, WM_NCPAINT, etc. message family. 
                     */
                    #endregion Non-client area definition

                    #region WM_NCCALCSIZE message parameters
                    /*
                        Parameters:

                        wParam
                        If wParam is TRUE, it specifies that the application should indicate which part of the 
                        client area contains valid information. The system copies the valid information to the 
                        specified area within the new client area.
                     
                        If wParam is FALSE, the application does not need to indicate the valid part of the client area.

                        lParam
                        If wParam is TRUE, lParam points to an NCCALCSIZE_PARAMS structure that contains 
                        information an application can use to calculate the new size and position of the client rectangle. 
                        If wParam is FALSE, lParam points to a RECT structure. On entry, the structure 
                        contains the proposed window rectangle for the window. On exit, the structure should 
                        contain the screen coordinates of the corresponding window client area.

                        Return Value
                        If the wParam parameter is FALSE, the application should return zero. 

                        If wParam is TRUE and an application returns zero, the old client area is preserved and 
                        is aligned with the upper-left corner of the new client area.
                    */

                        #region Return values
                    /*
                        If wParam is TRUE, the application should return zero or a combination of the following values:
                        WVR_ALIGNTOP    Specifies that the client area of the window is to be preserved and aligned with 
                                        the top of the new position of the window. For example, to align the client area 
                                        to the upper-left corner, return the WVR_ALIGNTOP and WVR_ALIGNLEFT values. 
                        WVR_ALIGNRIGHT  Specifies that the client area of the window is to be preserved and aligned with 
                                        the right side of the new position of the window. For example, to align the client 
                                        area to the lower-right corner, return the WVR_ALIGNRIGHT and WVR_ALIGNBOTTOM values. 
                        WVR_ALIGNLEFT   Specifies that the client area of the window is to be preserved and aligned with 
                                        the left side of the new position of the window. For example, to align the client 
                                        area to the lower-left corner, return the WVR_ALIGNLEFT and WVR_ALIGNBOTTOM values. 
                        WVR_ALIGNBOTTOM Specifies that the client area of the window is to be preserved and aligned with 
                                        the bottom of the new position of the window. For example, to align the client area 
                                        to the top-left corner, return the WVR_ALIGNTOP and WVR_ALIGNLEFT values. 
                        WVR_HREDRAW     Used in combination with any other values, except WVR_VALIDRECTS, causes the window
                                        to be completely redrawn if the client rectangle changes size horizontally. This 
                                        value is similar to CS_HREDRAW class style 
                        WVR_VREDRAW     Used in combination with any other values, except WVR_VALIDRECTS, causes the window 
                                        to be completely redrawn if the client rectangle changes size vertically. This value
                                        is similar to CS_VREDRAW class style 
                        WVR_REDRAW      This value causes the entire window to be redrawn. It is a combination of WVR_HREDRAW
                                        and WVR_VREDRAW values. 
                        WVR_VALIDRECTS  This value indicates that, upon return from WM_NCCALCSIZE, the rectangles specified 
                                        by the rgrc[1] and rgrc[2] members of the NCCALCSIZE_PARAMS structure contain valid 
                                        destination and source area rectangles, respectively. The system combines these 
                                        rectangles to calculate the area of the window to be preserved. The system copies 
                                        any part of the window image that is within the source rectangle and clips the image
                                        to the destination rectangle. Both rectangles are in parent-relative or screen-relative
                                        coordinates. This flag cannot be combined with any other flags. 
                                        This return value allows an application to implement more elaborate client-area 
                                        preservation strategies, such as centering or preserving a subset of the client area.
                    */
                        #endregion Return values
                    #endregion WM_NCCALCSIZE message parameters

                    int adjustment = this.BorderStyle == BorderStyle.FixedSingle ? 2 : 0;

                    if ((int)m.WParam == 0)                     // False
                    {

                        #region Marshal.PtrToStructure summary
                        /* Marshal.PtrToStructure
                         * Summary:
                         *      Marshals data from an unmanaged block of memory to a newly allocated managed 
                         *      object of the specified type.
                         *
                         * Parameters:
                         *      ptr: A pointer to an unmanaged block of memory.
                         *      structureType: 
                         *          The System.Type of object to be created. 
                         *          This type object must represent a formatted class or a structure.
                         *
                         * Returns:
                         *      A managed object containing the data pointed to by the ptr parameter.
                         */
                        #endregion Marshal.PtrToStructure summary

                        RECT rect = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT));

                        // Adjust (shrink) the client rectangle to accommodate the border:
                        rect.Top += m_BorderWidth - adjustment;
                        rect.Bottom -= m_BorderWidth - adjustment;
                        rect.Left += m_BorderWidth - adjustment;
                        rect.Right -= m_BorderWidth - adjustment;

                        #region Marshal.StructureToPtr summary
                        /* Marshal.StructureToPtr
                         * Summary:
                         *      Marshals data from a managed object to an unmanaged block of memory.
                         *
                         * Parameters:
                         *      ptr: 
                         *          A pointer to an unmanaged block of memory, which must be allocated before 
                         *          this method is called.
                         *      structure: 
                         *          A managed object holding the data to be marshaled. 
                         *          This object must be an instance of a formatted class.
                         *      fDeleteOld: 
                         *          true to have the System.Runtime.InteropServices.Marshal.DestroyStructure
                         *          (System.IntPtr,System.Type) method called on the ptr parameter before 
                         *          this method executes. Note that passing false can lead to a memory leak.
                         */
                        #endregion Marshal.StructureToPtr summary

                        Marshal.StructureToPtr(rect, m.LParam, false);

                        m.Result = IntPtr.Zero;
                    }
                    else if ((int)m.WParam == 1)                // True
                    {

                        #region Marshal.PtrToStructure summary
                        /* Marshal.PtrToStructure
                         * Summary:
                         *      Marshals data from an unmanaged block of memory to a newly allocated managed 
                         *      object of the specified type.
                         *
                         * Parameters:
                         *      ptr: A pointer to an unmanaged block of memory.
                         *      structureType: 
                         *          The System.Type of object to be created. 
                         *          This type object must represent a formatted class or a structure.
                         *
                         * Returns:
                         *      A managed object containing the data pointed to by the ptr parameter.
                         */
                        #endregion Marshal.PtrToStructure summary

                        nccsp = (NCCALCSIZE_PARAMS)Marshal.PtrToStructure(m.LParam, typeof(NCCALCSIZE_PARAMS));

                        // Adjust (shrink) the client rectangle to accommodate the border:
                        nccsp.rect0.Top += m_BorderWidth - adjustment;
                        nccsp.rect0.Bottom -= m_BorderWidth - adjustment;
                        nccsp.rect0.Left += m_BorderWidth - adjustment;
                        nccsp.rect0.Right -= m_BorderWidth - adjustment;

                        #region Marshal.StructureToPtr summary
                        /* Marshal.StructureToPtr
                         * Summary:
                         *      Marshals data from a managed object to an unmanaged block of memory.
                         *
                         * Parameters:
                         *      ptr: 
                         *          A pointer to an unmanaged block of memory, which must be allocated before 
                         *          this method is called.
                         *      structure: 
                         *          A managed object holding the data to be marshaled. 
                         *          This object must be an instance of a formatted class.
                         *      fDeleteOld: 
                         *          true to have the System.Runtime.InteropServices.Marshal.DestroyStructure
                         *          (System.IntPtr,System.Type) method called on the ptr parameter before 
                         *          this method executes. Note that passing false can lead to a memory leak.
                         */
                        #endregion Marshal.StructureToPtr summary

                        Marshal.StructureToPtr(nccsp, m.LParam, false);

                        m.Result = IntPtr.Zero;
                    }

                    base.WndProc(ref m);
                    break;

                #endregion case WM_NCCALCSIZE

                case (int)Win32Messages.WM_PAINT:
                    // Hide the caret if the text is readonly:
                    hideCaret = this.ReadOnly;
                    base.WndProc(ref m);
                    break;

                case (int)Win32Messages.WM_NCPAINT:
                    base.WndProc(ref m);
                    doPaint = true;

                    #region DEBUG paintCount++
                    #if DEBUG
                        paintCount++;
                    #endif
                    #endregion DEBUG paintCount++

                    break;

                #region Deprecated code
                // This message is manually posted when the control is resized.  It is necessary to "redraw" the
                // control whenever this occurs.
                //case REDRAW_MSG_NUMBER:
                //    base.WndProc(ref m);
                //    Redraw();
                //    break;
                #endregion Deprecated code

                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        #endregion Message processing: WndProc

        #region Drawing routines: PaintBorderRect, Redraw
        /// <summary>
        /// This code draws the borders of the non-client area.
        /// </summary>
        /// <param name="hWnd">The handle of the control to draw on.</param>
        /// <param name="width">The width (linesize) of the edges of the rectangle.</param>
        /// <param name="color">The color of the rectangle to draw.</param>
        /// <param name="borderLineColor">If non-null then an inner border in the specified color 
        /// will be drawn around the text box inside of the primary border.</param>
        private void PaintBorderRect(IntPtr hWnd, int width, Color color, object borderLineColor)
        {
            if (width == 0) return;                             // Without this test there may be artifacts

            IntPtr hDC = GetWindowDC(hWnd);
            using (Graphics g = Graphics.FromHdc(hDC))
            {
                using (Pen p = new Pen(color, width))
                {
                    p.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                    // 2634 -- Start
                    // There is a bug when drawing a line of width 1 so we have to special case it and adjust
                    // the height and width down 1 to circumvent it:
                    int adjustment = (width == 1 ? 1 : 0);
                    g.DrawRectangle(p, new Rectangle(0, 0, Width - adjustment, Height - adjustment));
                    // 2634 -- End
                    
                    // Draw the border line if a color is specified and there is room:
                    if (borderLineColor != null && width >= m_FixedSingleLineWidth && m_FixedSingleLineWidth > 0)   // 2635
                    {
                        p.Color = (Color)borderLineColor;
                        p.Width = m_FixedSingleLineWidth;
                        int offset = width - m_FixedSingleLineWidth;    // Overlay the inner border edge with the border line
                        // 2634 -- Start
                        // There is a bug when drawing a line of width 1 so we have to special case it and adjust
                        // the height and width down 1 to circumvent it:
                        adjustment = (m_FixedSingleLineWidth == 1 ? 1 : 0);
                        g.DrawRectangle(p, new Rectangle(offset, offset, Width - offset - offset - adjustment, 
                                                         Height - offset - offset - adjustment)); 
                        // 2634 -- End
                    }
                }
            }
            ReleaseDC(hWnd, hDC);
        }

        /// <summary>
        /// This is needed to get the control to repaint correctly.  UpdateStyles is NOT sufficient since
        /// it leaves artifacts when the control is resized.  
        /// </summary>
        private void Redraw()
        {
            // Make sure there is no recursion while recreating the handle:
            if (!this.RecreatingHandle) doRedraw = true;        // doRedraw = !this.RecreatingHandle;

            #region DEBUG redrawCount++
            #if DEBUG
                redrawCount++;
                Debug.WriteLine("****** Redraw()");
            #endif
            #endregion DEBUG redrawCount++
        }
        #endregion Drawing routines: PaintBorderRect, Redraw

        #region Events: timer_tick, OnSizeChanged, OnGotFocus, OnFontChanged, OnClick
        /// <summary>
        /// This is the timer routine which does the actual redrawing.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        void timer_Tick(object sender, EventArgs e)
        {
            if (hideCaret)
            {
                hideCaret = false;
                HideCaret(this.Handle);
            }
            if (doPaint)
            {
                #region DEBUG
		        #if DEBUG
                    Debug.WriteLine("****** Paint count = " + paintCount.ToString()); paintCount = 0;
                #endif 
                #endregion DEBUG

                doPaint = false;
                // Draw the inner border if BorderStyle.FixedSingle is selected.  
                // The last argument will be null if there will be no inner border.
                PaintBorderRect(this.Handle, m_BorderWidth, m_BorderColor,
                    (BorderStyle == BorderStyle.FixedSingle) ? (object)FixedSingleLineColor : null);
            }
            if (doRedraw)
            {
                #region DEBUG
                #if DEBUG
                    Debug.WriteLine("****** Redraw count = " + redrawCount.ToString()); redrawCount = 0;
                #endif
                #endregion DEBUG

                // 2633 -- Start
                // We use RecreateHandle for the Fixed3D border style to force the control to be recreated. 
                // It calls DestroyHandle and CreateHandle setting RecreatingHandle to true.  The
                // downside of this is that it will cause the control to flash.
                if (BorderStyle == BorderStyle.Fixed3D)
                {
                    // This is only needed to prevent artifacts for the Fixed3D border style
                    RecreateHandle();
                }
                else
                {
                    // The SWP_FRAMECHANGED (SWP_DRAWFRAME) flag will generate WM_NCCALCSIZE and WM_NCPAINT messages among others.
                    // uint setWindowPosFlags = (uint)(SWP.SWP_NOMOVE | SWP.SWP_NOSIZE | SWP.SWP_NOZORDER | SWP.SWP_FRAMECHANGED)
                    SetWindowPos(Handle, IntPtr.Zero, 0, 0, 0, 0, setWindowPosFlags);
                }
                // 2633 -- End
                doRedraw = false;                               // This must follow RecreateHandle()
            }
       }

        /// <summary>
        /// When the control is resized it must be completely redrawn to display correctly.  
        /// This can only happen AFTER the event has completed.  
        /// </summary>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Redraw(); 

            #region Deprecated code
            //PostMessage(this.Handle, REDRAW_MSG_NUMBER, 0, 0);
            #endregion Deprecated code
        }

        /// <summary>
        /// Make sure that the control gets drawn correctly the first time.
        /// </summary>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (first) { first = false; Redraw(); }
            hideCaret = this.ReadOnly;

            #region DEBUG
            Debug.Print("****** OnGotFocus(e)");
            #endregion DEBUG
        }

        /// <summary>
        /// Changing the font can cause artifacts so redraw the control.
        /// </summary>
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            Redraw();
        }

        /// <summary>
        /// Hide the caret if readonly.
        /// </summary>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            hideCaret = this.ReadOnly;
        }

        #endregion Events: timer_tick, OnSizeChanged, OnGotFocus, OnFontChanged, OnClick

    }
}
