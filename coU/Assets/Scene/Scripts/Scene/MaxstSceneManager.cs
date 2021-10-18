using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using maxstAR;
using UnityEngine.UI;
using System.IO;
using System;

using TMPro; // public TextMeshProUGUI 쓰기위함 -> InputField-TextMeshPro

public class MaxstSceneManager : MonoBehaviour
{
	// navi 기능을 위해서 추가함
	public static string naviStoreName = "";
	public static string naviStoreCategorySub = "";
	public static string naviStoreFloor = "";

	public TextMeshProUGUI storeNameTextBox;
	public TextMeshProUGUI storeFloorTextBox;
	public GameObject marker;
	//

	private CameraBackgroundBehaviour cameraBackgroundBehaviour = null;
	//hyojlee private GameObject arCamera = null;
	private static GameObject arCamera = null;
	private VPSStudioController vPSStudioController = null;

	public List<GameObject> disableObjects = new List<GameObject>();
	public List<GameObject> occlusionObjects = new List<GameObject>();
	private static List<VPSTrackable> vPSTrackablesList = new List<VPSTrackable>();
	//hyojlee private List<VPSTrackable> vPSTrackablesList = new List<VPSTrackable>();

	public Material buildingMaterial;
	public Material runtimeBuildingMaterial;

	public GameObject maxstLogObject;

	public bool isOcclusion = true;
	private static string currentLocalizerLocation = "";
	//hyojlee private string currentLocalizerLocation = "";

	private string serverName = "";

	//뒤로가기 2번 클릭 시 종료되도록 하기 위해 키 이벤트 카운트할 변수
	int backCount = 0;
	ShowToastMessage toast = new ShowToastMessage();

	void Awake()
	{
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;

		AndroidRuntimePermissions.Permission[] result = AndroidRuntimePermissions.RequestPermissions("android.permission.WRITE_EXTERNAL_STORAGE", "android.permission.CAMERA", "android.permission.ACCESS_FINE_LOCATION", "android.permission.ACCESS_COARSE_LOCATION");
		if (result[0] == AndroidRuntimePermissions.Permission.Granted && result[1] == AndroidRuntimePermissions.Permission.Granted)
			Debug.Log("We have all the permissions!");
		else
			Debug.Log("Some permission(s) are not granted...");

		ARManager arManagr = FindObjectOfType<ARManager>();
		if (arManagr == null)
		{
			Debug.LogError("Can't find ARManager. You need to add ARManager prefab in scene.");
			return;
		}
		else
		{
			arCamera = arManagr.gameObject;
		}

		vPSStudioController = FindObjectOfType<VPSStudioController>();
		if (vPSStudioController == null)
		{
			Debug.LogError("Can't find VPSStudioController. You need to add VPSStudio prefab in scene.");
			return;
		}
		else
		{
			string tempServerName = vPSStudioController.vpsServerName;
			serverName = tempServerName;
			vPSStudioController.gameObject.SetActive(false);
		}

		cameraBackgroundBehaviour = FindObjectOfType<CameraBackgroundBehaviour>();
		if (cameraBackgroundBehaviour == null)
		{
			Debug.LogError("Can't find CameraBackgroundBehaviour.");
			return;
		}

		VPSTrackable[] vPSTrackables = FindObjectsOfType<VPSTrackable>(true);
		if (vPSTrackables != null)
		{
			vPSTrackablesList.AddRange(vPSTrackables);
		}
		else
		{
			Debug.LogError("You need to add VPSTrackables.");
		}

		foreach (GameObject eachObject in disableObjects)
		{
			if (eachObject != null)
			{
				eachObject.SetActive(false);
			}
		}
	}

	void Start()
	{
		if (isOcclusion)
		{
			foreach (GameObject eachGameObject in occlusionObjects)
			{
				if (eachGameObject == null)
				{
					continue;
				}

				Renderer[] cullingRenderer = eachGameObject.GetComponentsInChildren<Renderer>();
				foreach (Renderer eachRenderer in cullingRenderer)
				{
					eachRenderer.material.renderQueue = 1900;
					eachRenderer.material = runtimeBuildingMaterial;
				}
			}
		}

		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
		{
			string simulatePath = vPSStudioController.vpsSimulatePath;
			if (Directory.Exists(simulatePath))
			{
				CameraDevice.GetInstance().Start(simulatePath);
				MaxstAR.SetScreenOrientation((int)ScreenOrientation.Portrait);
			}
		}
		else
		{
			if (CameraDevice.GetInstance().IsFusionSupported(CameraDevice.FusionType.ARCamera))
			{
				CameraDevice.GetInstance().Start();
			}
			else
			{
				TrackerManager.GetInstance().RequestARCoreApk();
			}
		}

		TrackerManager.GetInstance().StartTracker();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
        {
			toast.OnGUI();
			backCount++;
			if (!IsInvoking("backBtnOnClick"))
				Invoke("backBtnOnClick", 1.0f);
        }
		else if (backCount == 2)
        {
			CancelInvoke("backBtnOnClick");
			Application.Quit();
#if !UNITY_EDITOR
	System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
		}

		TrackerManager.GetInstance().UpdateFrame();

		ARFrame arFrame = TrackerManager.GetInstance().GetARFrame();

		TrackedImage trackedImage = arFrame.GetTrackedImage();

		if (trackedImage.IsTextureId())
		{
			IntPtr[] cameraTextureIds = trackedImage.GetTextureIds();
			cameraBackgroundBehaviour.UpdateCameraBackgroundImage(cameraTextureIds);
		}
		else
		{
			cameraBackgroundBehaviour.UpdateCameraBackgroundImage(trackedImage);
		}

		if (arFrame.GetARLocationRecognitionState() == ARLocationRecognitionState.ARLocationRecognitionStateNormal)
		{
			Matrix4x4 targetPose = arFrame.GetTransform();

			arCamera.transform.position = MatrixUtils.PositionFromMatrix(targetPose);
			arCamera.transform.rotation = MatrixUtils.QuaternionFromMatrix(targetPose);
			arCamera.transform.localScale = MatrixUtils.ScaleFromMatrix(targetPose);

			string localizerLocation = arFrame.GetARLocalizerLocation();

			if (currentLocalizerLocation != localizerLocation)
			{
				currentLocalizerLocation = localizerLocation;
				foreach (VPSTrackable eachTrackable in vPSTrackablesList)
				{
					bool isLocationInclude = false;
					foreach (string eachLocation in eachTrackable.localizerLocation)
					{
						if (currentLocalizerLocation == eachLocation)
						{
							isLocationInclude = true;
							break;
						}
					}
					eachTrackable.gameObject.SetActive(isLocationInclude);
				}
			}
		}
		else
		{
			foreach (VPSTrackable eachTrackable in vPSTrackablesList)
			{
				eachTrackable.gameObject.SetActive(false);
			}
			currentLocalizerLocation = "";
		}
	}

    void backBtnOnClick()
    {
		backCount = 0;
    }

    void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			CameraDevice.GetInstance().Stop();
			TrackerManager.GetInstance().StopTracker();
		}
		else
		{
			if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
			{
				string simulatePath = vPSStudioController.vpsSimulatePath;
				if (Directory.Exists(simulatePath))
				{
					CameraDevice.GetInstance().Start(simulatePath);
					MaxstAR.SetScreenOrientation((int)ScreenOrientation.Portrait);
				}
			}
			else
			{
				if (CameraDevice.GetInstance().IsFusionSupported(CameraDevice.FusionType.ARCamera))
				{
					CameraDevice.GetInstance().Start();
				}
				else
				{
					TrackerManager.GetInstance().RequestARCoreApk();
				}
			}

			TrackerManager.GetInstance().StartTracker();
		}
	}

	void OnDestroy()
	{
		CameraDevice.GetInstance().Stop();
		TrackerManager.GetInstance().StopTracker();
		TrackerManager.GetInstance().DestroyTracker();
	}


	public class NavigationDest
	{
		public string name { get; set; }
		public string floor { get; set; }
		public string categorySub { get; set; }

		public NavigationDest(string name, string floor, string categorySub)
		{
			this.name = name;
			this.floor = floor;
			this.categorySub = categorySub;
		}
		public NavigationDest(string name, string floor)
		{
			this.name = name;
			this.floor = floor;
			this.categorySub = "";
		}
		public string getEndLocation()
		{
			if (floor == "B1")
				return ("landmark_coex_b1");
			else if (floor == "B2")
				return ("landmark_coex_b2");
			else if (floor == "1F")
				return ("landmark_coex_f1");
			else
				return ("outdoor");
		}
		public Vector3 getNavigationLocation()
        {
			Vector3 destVector = new Vector3();
			string query = String.Format($"Select modifiedX, modifiedY FROM Stores AS S WHERE S.name = '{this.name}' AND S.floor = '{this.floor}'");
			print("query: " + query);
			List<Store> Stores = GetDBData.getStoresData(query);
			print(Stores.ToArray().Length);
			destVector.x = ((float)Stores[0].modifiedX);
			destVector.y = ((float)Stores[0].modifiedY);
			print(destVector.x);
			print(destVector.y);
			return destVector;
        }

	}

	static Action resetNaviValue = () =>
	{
		naviStoreName = "";
		naviStoreFloor = "";
		naviStoreCategorySub = "";
		return;
	};

	public static Vector3 getNavigationLocation(string name, string floor, bool substringNeed) //현재는 B1 층 밖에 안되니까
	{
		print("---getNavigationLocation---");
		if (substringNeed == true)
        {
			name = name.Substring(0, name.Length - 1);
			floor = floor.Substring(0, floor.Length - 1);
        }
		print($"{name}:{name.Length}");
		print($"{floor}:{floor.Length}");

		Vector3 destVector = new Vector3();
		GameObject destTemp = GameObject.Find(floor).transform.Find(name).gameObject;
		if (destTemp == null)
        {
			return Vector3.forward;
		}
		else
        {
			Transform destTransform = destTemp.GetComponentInChildren<Transform>(); // null 일 가능성 배제
			//print(destTransform.transform.position);
			//print(destTransform.transform.localPosition);
			destVector = destTransform.transform.position;
		}
		resetNaviValue();
		return (destVector);
	}

	public static void StartNavigation(string storeName, string categorySub, string floor)
	{
		NavigationDest navigationDest;
		Vector3 dest;
		bool substringNeed = false;
		
		navigationDest = new NavigationDest(storeName, floor);
		dest = getNavigationLocation(navigationDest.name, navigationDest.floor, substringNeed);

		if (currentLocalizerLocation != null)
		{
			GameObject trackingObject = null;
			foreach (VPSTrackable eachTrackable in vPSTrackablesList)
			{
				foreach (string eachLocation in eachTrackable.localizerLocation)
				{
					if (currentLocalizerLocation == eachLocation)
					{
						trackingObject = eachTrackable.gameObject;
						break;
					}
				}
			}

			//Vector3 temp = new Vector3()

			if (trackingObject != null)
			{
				NavigationController navigationController = GameObject.Find("SceneManager").GetComponent<NavigationController>();

				navigationController.MakePath(currentLocalizerLocation, arCamera.transform.position, navigationDest.getEndLocation(), dest, vPSTrackablesList.ToArray(),
				() =>
				{
					Debug.Log("No Path");
				});
				//}, "coex_outdoor");
			}
		}
	}

	public void OnClickNavigation()
	{
		print("OnClickNavigation()");
		print(MaxstSceneManager.naviStoreName);
		print(MaxstSceneManager.naviStoreFloor);
		print("============================");


		NavigationDest naviDest = new NavigationDest(MaxstSceneManager.naviStoreName, MaxstSceneManager.naviStoreFloor, MaxstSceneManager.naviStoreCategorySub);
		if ((naviDest.name == "" || naviDest.floor == "" || naviDest.floor == "") == false)
		{
			/*
			 * Query를 쓴다면 여기서 쓰자!
			string query = "Select name, floor, modifiedX, modifiedY from Stores Where name = '" + MaxstSceneManager.naviStoreName + "'";
			if (MaxstSceneManager.naviStoreCategorySub != "")
				query += "AND categorySub = '" + MaxstSceneManager.naviStoreCategorySub + "'";
            List<Stores> naviStore = getDBData.getStoresData(query);
            floor = naviStore[0].floor;
			*/
		}
		else
        {
			// Textbox에 있는 값으로 재할당
			string tempName = storeNameTextBox.text.Substring(0, storeNameTextBox.text.Length - 1);
			string tempFloor = storeFloorTextBox.text.Substring(0, storeFloorTextBox.text.Length - 1);
			naviDest = new NavigationDest(tempName, tempFloor);
		}

		
		GameObject location = new GameObject();
		GameObject parent = GameObject.Find(naviDest.floor);
		location.transform.parent = parent.transform;
		location.transform.localPosition = naviDest.getNavigationLocation();
		//print(location.transform.position.ToString());
		//print(location.transform.localPosition.ToString());

		if (currentLocalizerLocation != null)
		{
			GameObject trackingObject = null;
			foreach (VPSTrackable eachTrackable in vPSTrackablesList)
			{
				foreach (string eachLocation in eachTrackable.localizerLocation)
				{
					if (currentLocalizerLocation == eachLocation)
					{
						trackingObject = eachTrackable.gameObject;
						break;
					}
				}
			}

			if (trackingObject != null)
			{
				NavigationController navigationController = GetComponent<NavigationController>();

				navigationController.MakePath(currentLocalizerLocation, arCamera.transform.position, naviDest.getEndLocation(), location.transform.position, vPSTrackablesList.ToArray(),
				() =>
				{
					Debug.Log("No Path");
				});
				//}, "coex_outdoor");
			}
		}
	}

	void FixedUpdate()
	{
		if (Input.GetMouseButtonUp(0))
		{
			AttachLogo();
		}
	}

	public void AttachLogo()
	{
		Vector2 vTouchPos = Input.mousePosition;

		Ray ray = Camera.main.ScreenPointToRay(vTouchPos);

		RaycastHit vHit;
		if (Physics.Raycast(ray.origin, ray.direction, out vHit))
		{
			maxstLogObject.transform.position = vHit.point;
			maxstLogObject.transform.rotation = Quaternion.FromToRotation(Vector3.forward, vHit.normal) * Quaternion.Euler(-90.0f, 0.0f, 0.0f);
		}
	}
}