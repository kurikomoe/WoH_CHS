using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace MessageFiltering
{
    /// <summary>Redirects all message of a type to one control</summary>
    [SecurityPermission(SecurityAction.LinkDemand)]
    class RedirectMessageFilter : IMessageFilter
    {
        /// <summary>Send message to a window (platform invoke)</summary>
        /// <param name="hWnd">Window handle to send to</param>
        /// <param name="msg">Message</param>
        /// <param name="wParam">wParam</param>
        /// <param name="lParam">lParam</param>
        /// <returns>Zero if failure, otherwise non-zero</returns>
        [DllImport("User32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        /// <summary>Constructor</summary>
        /// <param name="message">Message to redirect</param>
        /// <param name="hWndTo">Window handle to redirect to</param>
        public RedirectMessageFilter(WindowsMessages message, IntPtr hWndTo)
        {
            _message = message;
            _hWndTo = hWndTo;
        }

        /// <summary>Message to redirect</summary>
        WindowsMessages _message;
        /// <summary>Windows to redirect to</summary>
        IntPtr _hWndTo;


        /// <summary>The message filter</summary>
        /// <param name="m">Message</param>
        /// <returns>True if handled, false if not</returns>
        /// <remarks>True will signal that the message has been handled and it will not be sent to any other control in the application.</remarks>
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == (int)_message && m.HWnd != _hWndTo)
            {
                IntPtr result = PostMessage(_hWndTo, m.Msg, m.WParam, m.LParam);
                return true;
            }

            // Not handled
            return false;
        }
    }

    /// <summary>
    /// Windows Messages
    /// Defined in winuser.h from Windows SDK v6.1
    /// Documentation pulled from MSDN.
    /// </summary>
    public enum WindowsMessages : uint
    {
        WM_KEYDOWN = 0x0100,
        WM_MOUSEWHEEL = 0x020A
    }

    /// <summary>
    /// Virtual Keys
    /// </summary>
    public enum VirtualKeys : uint
    {
        /// <summary>
        /// PAGE UP key
        /// </summary>
        VK_PRIOR = 0x21,
        /// <summary>
        /// PAGE DOWN key
        /// </summary>
        VK_NEXT = 0x22,
        VK_END = 0x23,
        VK_HOME = 0x24
    }}
