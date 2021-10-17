using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
#endif

#if UNITY_EDITOR
[InitializeOnLoadAttribute]
public static class PlayModeStateChanged
{
    // register an event handler when the class is initialized
    static PlayModeStateChanged()
    {
        EditorApplication.playModeStateChanged += VPSStudioController.PlayModeState;
    }
}
#endif
public class VPSStudioController : MonoBehaviour
{
#if UNITY_EDITOR
    public static void PlayModeState(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            ReferenceCameraController referenceCameraController = FindObjectOfType<ReferenceCameraController>();
            if (referenceCameraController != null)
            {
                referenceCameraController.OnReload();
            }
        }
    }
#endif
    [HideInInspector]
    [SerializeField]
    public string vpsPath = "";

    [HideInInspector]
    [SerializeField]
    public string vpsServerName = "";

    [HideInInspector]
    [SerializeField]
    private int selectIndex;

    [HideInInspector]
    [SerializeField]
    public string vpsSimulatePath = "";

    [HideInInspector]
    [SerializeField]
    private int simulate_selectIndex;

    [HideInInspector]
    [SerializeField]
    private GameObject meshObject;

    public int SelectIndex
    {
        get
        {
            return selectIndex;
        }
        set
        {
            selectIndex = value;
        }
    }

    public int Simulate_SelectIndex
    {
        get
        {
            return simulate_selectIndex;
        }
        set
        {
            simulate_selectIndex = value;
        }
    }

    [SerializeField]
    public static string vpsName = "";

    public int GetSelectedIndex()
    {
        return selectIndex;
    }

    public void SetSelectedIndex(int index)
    {
        selectIndex = index;
    }

#if UNITY_EDITOR
    public void LoadMap()
    {
        VPSLoader.Instance.Clear();
        VPSLoader.Instance.SetVPSPath(vpsPath);
        VPSLoader.Instance.Load();

        /** 2021/10/01 hyojlee
         * Contents 폴더에 Maxst/Content 에 로드한 맵 데이터가 없는 경우에 확실히 문제가 발생하는 것을 확인함(Mesh의 missing 이라던가) 
         * vpsName이 coex_indoor_1, coex_outdoor 등을 뜻하는데 막상 내용을 확인해보면 모두 같은 내용을 가진 것을 알 수 있음.
         * 그래서 그냥 하나만 냅두려고 함. => 문자열로 때려박을 것
         */
        //var name = Path.GetFileName(vpsPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        //vpsName = name;
        //print("This is name " + name);
        vpsName = "coex_data";


        ReferenceCameraController referenceCameraController = GetComponent<ReferenceCameraController>();
        referenceCameraController.Clear();
        referenceCameraController.MakeCameras();
        EditorCoroutineUtility.StartCoroutine(LoadAssetResource(vpsPath, vpsName), this);
    }

    public IEnumerator LoadAssetResource(string path, string vpsName)
    {
        string destinationfolderPath = Application.streamingAssetsPath + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "MaxstAR" + Path.DirectorySeparatorChar + "Contents" + Path.DirectorySeparatorChar + vpsName;

        if (!Directory.Exists(destinationfolderPath))
        {
            Directory.CreateDirectory(destinationfolderPath);
        }
        string mapPath = path;
        string[] files = Directory.GetFiles(mapPath);
        string destinationFolder = destinationfolderPath;
        List<string> loadPrefabs = new List<string>();
        foreach (string file in files)
        {
            string destinationFile = "";
            string extension = Path.GetExtension(file);

            if (extension == ".fbx" || extension == ".meta" || extension == ".prefab")
            {
                destinationFile = destinationFolder + Path.DirectorySeparatorChar + Path.GetFileName(file);
                if (Path.GetFileNameWithoutExtension(destinationFile).Contains("Trackable") && Path.GetExtension(destinationFile) == ".prefab")
                {
                    loadPrefabs.Add(destinationFile);
                }
            }

            if (destinationFile != "")
            {
                System.IO.File.Copy(file, destinationFile, true);
            }
        }
      
        yield return new WaitForEndOfFrame();

        AssetDatabase.Refresh();

        foreach(string eachLoadFile in loadPrefabs)
        {
            GameObject local_meshObject = PrefabUtility.LoadPrefabContents(eachLoadFile); //prefab가져오는 부분
            meshObject = Instantiate(local_meshObject); //hyojlee trackable이 계속 생기는 부분
            //tag를 이용해서 FindTag?를 써서 지울 수 있지 않을까싶음..
        }
    }
    
    public void Clear()
    {
        ReferenceCameraController referenceCameraController = GetComponent<ReferenceCameraController>();
        referenceCameraController.Clear();
    }
#endif

    public void ReloadName()
    {
        //var name = Path.GetFileName(vpsPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        //vpsName = name;
        vpsName = "coex_data";
    }
}
