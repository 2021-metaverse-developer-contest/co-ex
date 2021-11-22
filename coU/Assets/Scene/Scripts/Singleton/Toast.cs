using System.Collections;
using System.Threading;
using UnityEngine;

public class Toast
{
    public enum Term
    {
        shortTerm = 0,
        longTerm,
    };
    /// <summary>
    /// Show toast Message
    /// </summary>
    /// <param name="message">Showing message</param>
    /// <param name="showTime">Time to show(milliseconds)</param>
    public static void ShowToastMessage(string message, Term flag)
    {
#if UNITY_ANDROID
		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
        AndroidJavaObject toast = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, flag);

        if (unityActivity != null)
        {
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                toast.Call("show");
            }));
        }
#endif
	}

	public static IEnumerator ShowToastMessageCoroutine(string message, int seconds)
    {
#if UNITY_ANDROID
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
        ShowToastMessage(message, Term.longTerm); // 한번은 무조건 호출
        float toastIntervalTime = 3.0f;
        if (unityActivity != null)
        {
            int i = 0;
            while (i++ < (int)(seconds / toastIntervalTime))
            {
                AndroidJavaObject toast = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 1);
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    toast.Call("show");
                }));
                yield return new WaitForSecondsRealtime(toastIntervalTime * 0.6f);
                //unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                //{
                //    toast.Call("cancel");
                //}));
            }
        }
#endif
    }
}

