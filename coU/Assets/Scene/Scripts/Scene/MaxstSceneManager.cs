using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using maxstAR;
using UnityEngine.UI;
using System.IO;
using System;

using TMPro; // public TextMeshProUGUI 쓰기위함 -> InputField-TextMeshPro
using UnityEngine.SceneManagement;
using System.Threading;

public class MaxstSceneManager : MonoBehaviour
{
	// navi 기능을 위해서 추가함
	public static string naviStoreName = "";
	public static string naviStoreCategorySub = "";
	public static string naviStoreFloor = "";
	public static bool chkNaviBtnClick = false;

	public TextMeshProUGUI storeNameTextBox;
	public TextMeshProUGUI storeFloorTextBox;
	public GameObject marker;
	//
	public static Vector3 vAR = new Vector3();

	[SerializeField]
	private bool prefabRendering = false;

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
	GameObject panelFloor;
	GameObject panelToilet;
	public static bool onceDetectARLocation = false;

	//hyojlee 2021.10.23
	public static bool chkNavi = false;
	GameObject destination = null;
	public static string floor;

	//yunslee 2021.11.15
	public TextMeshProUGUI floorTextBox;

	// yunslee 2021.11.19
	GameObject[] naviTrackArray;
	public float trackVisableMaxRange = 25f;

	void Awake()
	{
		onceDetectARLocation = false;
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
#if UNITY_EDITOR
			Debug.Log("권한을 허용해주세요!");
#elif UNITY_ANDROID
			Toast.ShowToastMessage("권한을 허용해주세요!", Toast.Term.shortTerm);
#endif
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

	private IEnumerator setvAR()
    {
		while (true)
        {
			vAR = arCamera.transform.position;
			yield return new WaitForSeconds(0.1f);
        }
    }

	void Start()
	{
		//화면 꺼지지 않도록 2021.10.24
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		StartCoroutine(setvAR());

		//hyojlee 2021/10/19
		panelBackground = GameObject.Find("Panel_Background").gameObject;
		panelFloor = GameObject.Find("Panel_Floor");
		panelToilet = GameObject.Find("Panel_Toilet");

		panelToilet.GetComponent<Button>().onClick.AddListener(delegate { bathroomBtnClick(); });

		panelFloor.SetActive(false);
		panelToilet.SetActive(false);

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
		if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.sceneCount < 2)
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
#if UNITY_EDITOR
			Debug.Log("한 번 더 누르시면 종료됩니다.");
#elif UNITY_ANDROID
			Toast.ShowToastMessage("한 번 더 누르시면 종료됩니다.", Toast.Term.shortTerm);
#endif
		}

		// yunslee 2021.11.21
		if (chkNaviBtnClick == true && panelBackground.activeSelf == false)
		{
			chkNaviBtnClick = false;
			StartNavigation(resetNaviValue); // 스토어씬에서 호출된 네비게이션
		}
		//hyojlee 2021.10.23
		if (chkNavi)
		{
			if (destination == null)
			{
				if (GameObject.Find("destination") != null)
				{
					destination = GameObject.Find("destination").gameObject;
					naviTrackArray = GameObject.FindGameObjectsWithTag("naviTrack");
				}
			}
			if (destination != null) //else가 아닌 이유: destination 찾자마자 실행되어야하므
				destination.transform.forward = arCamera.transform.forward;
			updateVisibleTrack();
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

		//print(ARLocationRecognitionState.ARLocationRecognitionStateNormal); //
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
				showFloor(localizerLocation);
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
				panelFloor.SetActive(true);
				panelToilet.SetActive(true);
				onceDetectARLocation = true;
				string substr = currentLocalizerLocation.Substring(currentLocalizerLocation.LastIndexOf('_') + 1).ToUpper();
				PutMarkerManager.floor = (substr == "F1") ? "1F" : substr;
				if (prefabRendering == false)
					disableRenderer(currentLocalizerLocation);
			}
		}
		else
		{
			foreach (VPSTrackable eachTrackable in vPSTrackablesList)
			{
				// hyojlee 2021/10/20
				// 공간 인식이 완료된 시점이면 애니메이션이 꺼져야함.
				panelBackground.SetActive(true);
				panelFloor.SetActive(false);
				panelToilet.SetActive(false);
				eachTrackable.gameObject.SetActive(false);
			}
			currentLocalizerLocation = "";
		}

	}

	void showFloor(string localizerLocation)
	{
		string whichfloor = localizerLocation.Substring(localizerLocation.LastIndexOf("_") + 1);
		if (whichfloor == "f1")
			whichfloor = "1F";
		else if (whichfloor == "outdoor")
			whichfloor = "야외";
		else if (whichfloor == "b1")
			whichfloor = "B1";
		else if (whichfloor == "b2")
			whichfloor = "B2";

		if (floorTextBox.text != whichfloor)
		{
			floorTextBox.text = whichfloor;
			string toastMessage = $"코엑스 {whichfloor}으로 공간 인식했습니다.";
			#if UNITY_EDITOR
				Debug.Log(toastMessage);
			#elif UNITY_ANDROID && !UNITY_EDITOR
				StartCoroutine(Toast.ShowToastMessageCoroutine(toastMessage, 5));
			#endif
		}
	}

	static class FloorConstants
	{
		public const string COEX_B1 = "landmark_coex_b1";
		public const string COEX_B2 = "landmark_coex_b2";
		public const string COEX_1 = "landmark_coex_f1";
		public const string COEX_OUTDOOR = "outdoor";
	}

	// inspector 체크박스로 렌더링 할지 안할지 결정할 수 있도록 리팩토링하고 싶음(함수명도 적당한 것으로 바꾸고 싶음)
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
			if (SceneManager.sceneCount == 1)
			{
				SceneManager.LoadScene("MaxstScene");
			}
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
					//#if UNITY_EDITOR
					//					Debug.Log("같은 위치에서 인식해주세요!", 300);
					//#elif UNITY_ANDROID
					//					Toast.ShowToastMessage("같은 위치에서 인식해주세요!", Toast.Term.shortTerm);
					//#endif
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
			destVector.z = (float)0;
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
		bool noPath = false;
		NavigationDest naviDest = new NavigationDest(MaxstSceneManager.naviStoreName, MaxstSceneManager.naviStoreFloor, MaxstSceneManager.naviStoreCategorySub);
		floor = naviDest.floor;
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
					Debug.Log("No Path2");
					noPath = PopNoPath();
					Debug.Log("No Path");
				});
				//}, "coex_outdoor");
			}
		}
		if (!noPath)
			ActivePanelChange();
		action.Invoke();
	}

	//hyojlee 2021.10.24
	/// <summary>
    /// If the floor of the destination and the floor of the current location are different,
    /// the destination obejct of the current floor is removed.
    /// </summary>
	public static void DestroyFakeDestination()
	{
		floor = floor == "1F" ? "F1" : floor; 
		foreach (VPSTrackable vpsTrack in vPSTrackablesList)
		{
			Debug.Log("Destroy the canvas_arrival of the destination floor " + vpsTrack.gameObject.name);
			if (vpsTrack.gameObject.transform.Find("Navigation/destination") != null)
			{
				if (!vpsTrack.gameObject.name.EndsWith(floor.ToLower() + "(Clone)"))
				{
					DestroyImmediate(vpsTrack.gameObject.transform.Find("Navigation/destination").gameObject);
					break;
				}
			}
		}
	}

	bool PopNoPath()
    {
		GameObject panel_NoPath = GameObject.Find("Canvas_Navi").transform.Find("Panel_NoPath").gameObject;

		panel_NoPath.SetActive(true);
		Thread.Sleep(400);
		panel_NoPath.SetActive(false);
		return true;
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

    // hyojlee 2021.10.22
    /// <summary>
    /// If this function is called when Navigation starts or ends,
    /// active panel changes.
    /// </summary>
    public static void ActivePanelChange()
    {
        Transform parentTransform = GameObject.Find("Canvas_Overlay").transform;
        GameObject panelNavi = parentTransform.Find("Panel_Navi").gameObject;
        GameObject panelOn = parentTransform.Find("Panel_On").gameObject;
		GameObject panelToilet = parentTransform.Find("Panel_RightUp/Panel_Toilet").gameObject;

		if (panelNavi.active)
        {
			chkNavi = false;
            panelNavi.SetActive(false);
            panelOn.SetActive(true);
			panelToilet.SetActive(true);

			foreach (VPSTrackable vps in vPSTrackablesList)
			{
				Debug.Log("VPSNAME " + vps.gameObject.name);
				if (vps.gameObject.transform.Find("Navigation") != null)
					DestroyImmediate(vps.gameObject.transform.Find("Navigation").gameObject);
			}
        }
        else
        {
            panelNavi.SetActive(true);
            panelOn.SetActive(false);
			panelToilet.SetActive(false);
        }
    }

	/*
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
	*/

    private void OnDisable()
    {
		vPSTrackablesList = null;
	}

	public void updateVisibleTrack()
	{
		foreach (GameObject eachArrowItem in naviTrackArray)
		{
			Vector3 arCameraPosition = arCamera.transform.position;
			Vector3 arrowPosition = eachArrowItem.transform.position;
			float distacne = Vector3.Distance(arCameraPosition, arrowPosition);
			if (distacne > trackVisableMaxRange)
			{
				eachArrowItem.SetActive(false);
			}
			else
			{
				eachArrowItem.SetActive(true);
			}
		}
	}


	void StartToiletNavigation(GameObject location)
	{
		string floor = MaxstSceneManager.floor;
		floor = "B2"; //지워야 함;
		bool noPath = false;

		if (currentLocalizerLocation != null)
		{

			NavigationController navigationController = GameObject.Find("SceneManager").GetComponent<NavigationController>();
			navigationController.MakePath(currentLocalizerLocation, arCamera.transform.position, getEndLocation(floor), location.transform.position, vPSTrackablesList.ToArray(),
			() =>
			{
				Debug.Log("No Path");
			});
			//}, "coex_outdoor");
		}
		if (!noPath)
			ActivePanelChange();
	}

	GameObject closestBathroom()
	{
		//string floor = floorTextBox.text;
		string floor = MaxstSceneManager.floor;
		floor = "B2"; //지워야 함;
		string query = $"Select * from Facilities Where floor='{floor}' AND type='toilet'";
		List<Facility> bathroomList = GetDBData.getFacilitiesData(query);

		Vector3 tempVectorZero = new Vector3();
		string parentObjectName = floor + "_Stores";
		GameObject parentCriterion = GameObject.Find(parentObjectName);
		GameObject empty = new GameObject();
		float closestDistance = 0f;
		foreach (var i in bathroomList)
		{
			float calculation = 0f;
			GameObject child = Instantiate(empty, parentCriterion.transform);
			child.name = "toilet";
			Vector3 rawLocation = new Vector3(((float)i.modifiedX), ((float)i.modifiedY), 0);
			child.transform.localPosition = rawLocation;
			//i.distance = Vector3.Distance(child.transform.position, MaxstSceneManager.vAR);
			calculation = Vector3.Distance(child.transform.position, tempVectorZero);
			if (closestDistance > calculation)
				Destroy(child);
			else
			{
				closestDistance = calculation;
				//closeObject = child;
			}
		}
		Destroy(empty);
		return GameObject.Find("toilet");
	}

	private static string getEndLocation(string floor)
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

	void bathroomBtnClick()
	{
		GameObject temp = closestBathroom();
		Debug.Log($"temp.name: {temp.name}");
		StartToiletNavigation(temp);
	}

}