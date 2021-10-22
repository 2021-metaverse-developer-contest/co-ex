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
	public static bool naviStart = false;

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

	//hyojlee 2021/10/18
    //뒤로가기 2번 클릭 시 종료되도록 하기 위해 키 이벤트 카운트할 변수
	int backCount = 0;
	GameObject panelBackground;

	void Awake()
	{
		if (vPSTrackablesList == null)
			vPSTrackablesList = new List<VPSTrackable>();
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;

		AndroidRuntimePermissions.Permission[] result = AndroidRuntimePermissions.RequestPermissions("android.permission.WRITE_EXTERNAL_STORAGE", "android.permission.CAMERA", "android.permission.ACCESS_FINE_LOCATION", "android.permission.ACCESS_COARSE_LOCATION");
		if (result[0] == AndroidRuntimePermissions.Permission.Granted && result[1] == AndroidRuntimePermissions.Permission.Granted)
			Debug.Log("We have all the permissions!");
		else
		{
			Debug.Log("Some permission(s) are not granted...");
			Toast.Instance.ShowToastMessage("권한을 허용해주세요!", 500);
			AndroidRuntimePermissions.RequestPermissions("android.permission.WRITE_EXTERNAL_STORAGE", "android.permission.CAMERA", "android.permission.ACCESS_FINE_LOCATION", "android.permission.ACCESS_COARSE_LOCATION");
		}
		//else
		//	Debug.Log("Some permission(s) are not granted...");

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
		//hyojlee 2021/10/19
		panelBackground = GameObject.Find("Panel_Background").gameObject;
		
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
		// 2021/10/18 hyojlee
		// MaxstScene에서 뒤로가기 연속 클릭 시 앱 종료하는 부분
		// 한 번 누르면 종료하지 않고 안드로이드의 토스트 메시지 뜨도록 함
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			backCount++;
			if (!IsInvoking("ResetBackCount"))
				Invoke("ResetBackCount", 1.0f);
			if (backCount == 2)
			{
				CancelInvoke("ResetBackCount");
				Application.Quit();
#if !UNITY_EDITOR
	System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
			}
			Toast.Instance.ShowToastMessage("한 번 더 누르시면 종료됩니다.", 250);
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
				panelBackground.SetActive(false);
				disableRenderer(currentLocalizerLocation);
				if (naviStart == true)
				{
					naviStart = false;
					StartNavigation(resetNaviValue); // 스토어씬에서 호출된 네비게이션
				}
			}
		}
		else
		{
			foreach (VPSTrackable eachTrackable in vPSTrackablesList)
			{
				// hyojlee 2021/10/20
				// 공간 인식이 완료된 시점이면 애니메이션이 꺼져야함.
				panelBackground.SetActive(true);
				eachTrackable.gameObject.SetActive(false);
			}
			currentLocalizerLocation = "";
		}

	}

	static class FloorConstants
	{
		public const string COEX_B1 = "landmark_coex_b1";
		public const string COEX_B2 = "landmark_coex_b2";
		public const string COEX_1 = "landmark_coex_f1";
		public const string COEX_OUTDOOR = "outdoor";
	}

	private void disableRenderer(string localizerLocation)
	{
		string parentName = null;
		if (localizerLocation.EndsWith(FloorConstants.COEX_B1))
			parentName = nameof(FloorConstants.COEX_B1);
		if (localizerLocation.EndsWith(FloorConstants.COEX_B2))
			parentName = nameof(FloorConstants.COEX_B2);
		if (localizerLocation.EndsWith(FloorConstants.COEX_1))
			parentName = nameof(FloorConstants.COEX_1);
		if (localizerLocation.EndsWith(FloorConstants.COEX_OUTDOOR))
			parentName = nameof(FloorConstants.COEX_OUTDOOR);

		GameObject parent = GameObject.Find(parentName);
		Renderer[] renders = parent.GetComponentsInChildren<Renderer>();
		foreach (Renderer it in renders)
        {
			it.enabled = false;
        }
    }

    void ResetBackCount()
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

	private Action resetNaviValue = () =>
	{
		naviStoreName = "";
		naviStoreFloor = "";
		naviStoreCategorySub = "";
		return;
	};

	public void StartNavigation(Action action)
	{
		NavigationDest naviDest = new NavigationDest(MaxstSceneManager.naviStoreName, MaxstSceneManager.naviStoreFloor, MaxstSceneManager.naviStoreCategorySub);
		if ((naviDest.name == "" || naviDest.floor == "" || naviDest.categorySub == "") == true)
		{
			print("세가지 값 중 하나라도 전달되지 않으면 에러");
		}
		print(naviDest.name);
		print(naviDest.floor);
		print(naviDest.categorySub);
		GameObject location = new GameObject();
		string parentOfFloor = naviDest.floor + "_Stores";

		GameObject parent = GameObject.Find(parentOfFloor);
		location.transform.parent = parent.transform;
		location.transform.localPosition = naviDest.getNavigationLocation();

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
				NavigationController navigationController = GameObject.Find("SceneManager").GetComponent<NavigationController>(); ;
				navigationController.MakePath(currentLocalizerLocation, arCamera.transform.position, naviDest.getEndLocation(), location.transform.position, vPSTrackablesList.ToArray(),
				() =>
				{
					Debug.Log("No Path");
				});
				//}, "coex_outdoor");
			}
		}
		action.Invoke();
	}

	public void OnClickNavigation()
	{
		print("OnClickNavigation()");
		// Textbox에 있는 값으로 재할당
		string tempName = storeNameTextBox.text.Substring(0, storeNameTextBox.text.Length - 1);
		string tempFloor = storeFloorTextBox.text.Substring(0, storeFloorTextBox.text.Length - 1);
		NavigationDest naviDest = new NavigationDest(tempName, tempFloor);

		
		GameObject location = new GameObject();
		GameObject parent = GameObject.Find(naviDest.floor);
		location.transform.parent = parent.transform;
		location.transform.localPosition = naviDest.getNavigationLocation();

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

    private void OnDisable()
    {
		vPSTrackablesList = null;
	}
}