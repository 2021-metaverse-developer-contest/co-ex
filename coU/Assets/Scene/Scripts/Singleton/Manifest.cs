using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//https://pluu.github.io/blog/android/2015/09/07/android-metadata/
//https://blueasa.tistory.com/2383
public class Manifest : MonoBehaviour
{
    /// <summary>
    /// Get Meta-data in AndroidManifest.xml
    /// </summary>
    /// <param name="name">android:name</param>
    /// <returns>android:value</returns>
    public static string GetManifestData(string name)
    {
        string ret = "";

        AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        string packageName = activity.Call<string>("getPackageName");
        Debug.Log("packageName " + packageName);
        AndroidJavaObject manager = activity.Call<AndroidJavaObject>("getPackageManager");
        AndroidJavaObject packageInfo = manager.Call<AndroidJavaObject>("getApplicationInfo", packageName, manager.GetStatic<int>("GET_META_DATA"));
        AndroidJavaObject aBundle = packageInfo.Get<AndroidJavaObject>("metaData");
        ret = aBundle.Call<string>("getString", name);
        Debug.Log("ret " + ret);
        Debug.Log("boolean " + aBundle.Call<bool>("getBoolean", name).ToString());
        return (ret);
    }
}
