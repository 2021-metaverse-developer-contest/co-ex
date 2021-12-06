namespace Gpm.WebView.Internal
{
    using System.Collections.Generic;
    using UnityEngine;

    public class DefaultWebView : IWebView
    {
        public void ShowUrl(
            string url,
            GpmWebViewRequest.Configuration configuration,
            GpmWebViewCallback.GpmWebViewErrorDelegate openCallback,
            GpmWebViewCallback.GpmWebViewErrorDelegate closeCallback,
            List<string> schemeList,
            GpmWebViewCallback.GpmWebViewDelegate<string> schemeEvent)
        {
            Debug.LogWarning("Not supported method in the editor");
        }

        public void ShowHtmlFile(
            string fileName,
            GpmWebViewRequest.Configuration configuration,
            GpmWebViewCallback.GpmWebViewErrorDelegate openCallback,
            GpmWebViewCallback.GpmWebViewErrorDelegate closeCallback,
            List<string> schemeList,
            GpmWebViewCallback.GpmWebViewDelegate<string> schemeEvent)
        {
            Debug.LogWarning("Not supported method in the editor");
        }

        public void ShowHtmlString(
            string source,
            GpmWebViewRequest.Configuration configuration,
            GpmWebViewCallback.GpmWebViewErrorDelegate openCallback,
            GpmWebViewCallback.GpmWebViewErrorDelegate closeCallback,
            List<string> schemeList,
            GpmWebViewCallback.GpmWebViewDelegate<string> schemeEvent)
        {
            Debug.LogWarning("Not supported method in the editor");
        }

        public void Close()
        {
            Debug.LogWarning("Not supported method in the editor");
        }

        public void ExecuteJavaScript(string script)
        {
            Debug.LogWarning("Not supported method in the editor");
        }

        public void SetFileDownloadPath(string path)
        {
            Debug.LogWarning("Not supported method in the editor");
        }
    }
}
