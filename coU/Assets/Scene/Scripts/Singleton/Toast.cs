using System.Threading;
using UnityEngine;

public class Toast
{
    /// <summary>
    /// Show toast Message
    /// </summary>
    /// <param name="message">Showing message</param>
    /// <param name="showTime">Time to show(milliseconds)</param>
    public static void ShowToastMessage(string message, int showTime)
    {
#if UNITY_ANDROID
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
        AndroidJavaObject toast = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);

        if (unityActivity != null)
        {
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                toast.Call("show");
            }));
            Thread.Sleep(showTime);
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                toast.Call("cancel");
            }));
        }
#endif
    }
}

