namespace Gpm.WebView
{
    public static class GpmWebViewRequest
    {
        public class Configuration
        {
            /// <summary>
            /// These constants indicate the type of launch style such as popup, fullscreen.
            /// Refer to <see cref="GpmWebViewStyle"/>
            /// </summary>
            public int style;

            /// <summary>
            /// Clear cookies.
            /// </summary>
            public bool isClearCookie;

            /// <summary>
            /// Clear cache.
            /// </summary>
            public bool isClearCache;

            /// <summary>
            /// Sets the visibility of the navigation bar.
            /// </summary>
            public bool isNavigationBarVisible;

            /// <summary>
            /// Sets the color of the navigation bar.
            /// (e.g. #000000 ~ #FFFFFF)
            /// Default: #4B96E6
            /// </summary>
            public string navigationBarColor = "#4B96E6";

            /// <summary>
            /// The page title.
            /// </summary>
            public string title;

            /// <summary>
            /// Sets the visibility of the back button.
            /// </summary>
            public bool isBackButtonVisible;

            /// <summary>
            /// Sets the visibility of the forward button.
            /// </summary>
            public bool isForwardButtonVisible;

            /// <summary>
            /// iOS only.
            /// The content mode for the web view to use when it loads and renders a webpage.
            /// Refer to <see cref="GpmWebViewContentMode"/>
            /// </summary>
            public int contentMode;

            /// <summary>
            /// Android only.
            /// Sets whether the WebView whether supports multiple windows.
            /// </summary>
            public bool supportMultipleWindows;
        }
    }
}
