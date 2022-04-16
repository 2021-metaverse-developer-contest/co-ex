namespace Gpm.WebView
{
    using System.Collections.Generic;
    using Gpm.WebView.Internal;

    public static class GpmWebView
    {
        public const string VERSION = "1.3.2";

        /// <summary>
        /// Create the webview and loads the web content referenced by the specified URL.
        /// </summary>
        /// <param name="url">The URL of the resource to load.</param>
        /// <param name="configuration">The configuration of GPM WebWiew. Refer to <see cref="GpmWebViewRequest.Configuration"/></param>
        /// <param name="openCallback">Notifies users when a WebView is opened.</param>
        /// <param name="closeCallback">Notifies users when a WebView is closed.</param>
        /// <param name="schemeList">Specifies the list of customized schemes a user wants.</param>
        /// <param name="schemeEvent">Notifies url including customized scheme specified by the schemeList with a callback.</param>
        public static void ShowUrl(
            string url,
            GpmWebViewRequest.Configuration configuration,
            GpmWebViewCallback.GpmWebViewErrorDelegate openCallback,
            GpmWebViewCallback.GpmWebViewErrorDelegate closeCallback,
            List<string> schemeList,
            GpmWebViewCallback.GpmWebViewDelegate<string> schemeEvent)
        {
            WebViewImplementation.Instance.ShowUrl(url, configuration, openCallback, closeCallback, schemeList, schemeEvent);
        }

        /// <summary>
        /// Create the webview and loads the web content from the specified file.
        /// </summary>
        /// <param name="filePath">The URL of a file that contains web content. This URL must be a file-based URL.</param>
        /// <param name="configuration">The configuration of GPM WebWiew. Refer to <see cref="GpmWebViewRequest.Configuration"/></param>
        /// <param name="openCallback">Notifies users when a WebView is opened.</param>
        /// <param name="closeCallback">Notifies users when a WebView is closed.</param>
        /// <param name="schemeList">Specifies the list of customized schemes a user wants.</param>
        /// <param name="schemeEvent">Notifies url including customized scheme specified by the schemeList with a callback.</param>
        public static void ShowHtmlFile(
            string filePath,
            GpmWebViewRequest.Configuration configuration,
            GpmWebViewCallback.GpmWebViewErrorDelegate openCallback,
            GpmWebViewCallback.GpmWebViewErrorDelegate closeCallback,
            List<string> schemeList,
            GpmWebViewCallback.GpmWebViewDelegate<string> schemeEvent)
        {
            WebViewImplementation.Instance.ShowHtmlFile(filePath, configuration, openCallback, closeCallback, schemeList, schemeEvent);
        }

        /// <summary>
        /// Create the webview and loads the contents of the specified HTML string.
        /// </summary>
        /// <param name="htmlString">The string to use as the contents of the webpage.</param>
        /// <param name="configuration">The configuration of GPM WebWiew. Refer to <see cref="GpmWebViewRequest.Configuration"/></param>
        /// <param name="openCallback">Notifies users when a WebView is opened.</param>
        /// <param name="closeCallback">Notifies users when a WebView is closed.</param>
        /// <param name="schemeList">Specifies the list of customized schemes a user wants.</param>
        /// <param name="schemeEvent">Notifies url including customized scheme specified by the schemeList with a callback.</param>
        public static void ShowHtmlString(
            string htmlString,
            GpmWebViewRequest.Configuration configuration,
            GpmWebViewCallback.GpmWebViewErrorDelegate openCallback,
            GpmWebViewCallback.GpmWebViewErrorDelegate closeCallback,
            List<string> schemeList,
            GpmWebViewCallback.GpmWebViewDelegate<string> schemeEvent)
        {
            WebViewImplementation.Instance.ShowHtmlString(htmlString, configuration, openCallback, closeCallback, schemeList, schemeEvent);
        }

        /// <summary>
        /// Execute the specified JavaScript string.
        /// </summary>
        /// <param name="script">The JavaScript string to execute.</param>
        public static void ExecuteJavaScript(string script)
        {
            WebViewImplementation.Instance.ExecuteJavaScript(script);
        }

        /// <summary>
        /// Close currently displayed WebView.
        /// </summary>
        public static void Close()
        {
            WebViewImplementation.Instance.Close();
        }
    }
}