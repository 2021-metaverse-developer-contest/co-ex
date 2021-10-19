//using System.Collections;
//using System.Collections.Generic;
//using System.Threading;
//using UnityEngine;

//public class ShowToastMessage : MonoBehaviour
//{
//    private AndroidJavaObject curActivity = null;
//    private void Awake()
//    {
//#if UNITY_ANDROID
//    AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//    curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
//#endif
//    }

//    public void OnGUI()
//    {
//#if UNITY_ANDROID
//    AndroidJavaObject toast = new AndroidJavaObject("android.widget.Toast", curActivity);
//    curActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
//    {
//        toast.CallStatic<AndroidJavaObject>("makeText", curActivity, "한 번 더 누르시면 종료됩니다.", 0).Call("show");
//    }));
//    Thread.Sleep(300);
//    curActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
//    {
//        toast.Call("cancel");
//    }));
//#endif
//    }
//}
using System.Threading;
using UnityEngine;

public class Toast : MonoBehaviour
{
    private static Toast _toast = null;

    public static Toast Instance
    {
        get {
            if (_toast == null)
                _toast = new Toast();
            return _toast;
        }
    }

    /// <summary>
    /// Show toast Message
    /// </summary>
    /// <param name="message">Showing message</param>
    /// <param name="showTime">Time to show(milliseconds)</param>
    public void ShowToastMessage(string message, int showTime)
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

