using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using maxstAR;
using JsonFx.Json;

//#if UNITY_EDITOR
//using Unity.EditorCoroutines.Editor;
//#endif

public class NavigationController : MonoBehaviour
{
    public enum e_character
    {
        none,
        astronaut,
        rabbit,
        coco
    };
    public static e_character characterType;

    private static string pathURL = "http://ec2-3-34-147-254.ap-northeast-2.compute.amazonaws.com:5501";

    private Vector3 startPosition;
    private Vector3 endPosition;

    public float arrowPositionDistance = 3.0f;
    public float arrowVisibleDistance = 20.0f;
    public float arrowPathHeight = -0.5f;
    public GameObject arrowPrefab;

    //hyojlee 2021.10.23
    public GameObject arrival;
    // yunslee 2021.11.14
    public GameObject rabbitPrefab;
    public GameObject astronautPrefab;
    public GameObject cocoPrefab;
    public float characterMoveSpeed = 1f;
    public float characterScale = 1f;

    private List<GameObject> pathObjects = new List<GameObject>();
    private List<GameObject> arrowItems = new List<GameObject>();

    public void UpdateVisibleArrow(GameObject arCameraObject)
    {
        foreach(GameObject eachArrowItem in arrowItems)
        {
            Vector3 arCameraPosition = arCameraObject.transform.position;
            Vector3 arrowPosition = eachArrowItem.transform.position;

            float distacne = Vector3.Distance(arCameraPosition, arrowPosition);
            if(distacne > arrowVisibleDistance)
            {
                eachArrowItem.SetActive(false);
            }
            else
            {
                eachArrowItem.SetActive(true);
            }
        }
    }

    public static void FindPath(string startLocation, Vector3 start, string endLocation, Vector3 end, Action<Dictionary<string,List<PathModel>>> success, Action fail, MonoBehaviour behaviour, string server_name = "")
    {
        Dictionary<string, string> headers = new Dictionary<string, string>()
        {
            { "Content-Type", "application/json"}
        };

        string startPositionString = start.x + "," + start.z + "," + start.y;
        string endPositionString = end.x + "," + (end.z) + "," + (end.y);
        Dictionary<string, string> parameters = new Dictionary<string, string>()
        {
            { "start_location", startLocation },
            { "start_position", startPositionString},
            { "end_location", endLocation },
            { "end_position", endPositionString},
            { "server_name", server_name}
        };

//#if UNITY_EDITOR
//        EditorCoroutineUtility.StartCoroutine(APIController.POST(pathURL + "/v1/path", headers, parameters, 10, (resultString) =>
//        {
//            if (resultString != "")
//            {
//                PathModel[] paths = JsonReader.Deserialize<PathModel[]>(resultString);
//                Dictionary<string, List<PathModel>> pathDictionary = new Dictionary<string, List<PathModel>>();
//                foreach (PathModel eachPathModel in paths)
//                {
//                    if (!pathDictionary.ContainsKey(eachPathModel.location))
//                    {
//                        pathDictionary[eachPathModel.location] = new List<PathModel>();
//                    }
//                    List<PathModel> pathList = pathDictionary[eachPathModel.location];
//                    if (pathList == null)
//                    {
//                        pathList = new List<PathModel>();
//                    }

//                    pathList.Add(eachPathModel);
//                }
//                success(pathDictionary);
//            }
//            else
//            {
//                fail();
//            }

//        }), behaviour);
//#else
        behaviour.StartCoroutine(APIController.POST(pathURL + "/v1/path", headers, parameters, 10, (resultString) =>
        {
            if(resultString != "")
            {
                PathModel[] paths = JsonReader.Deserialize<PathModel[]>(resultString);
                Dictionary<string, List<PathModel>> pathDictionary = new Dictionary<string, List<PathModel>>();
                foreach(PathModel eachPathModel in paths)
                {
                    if(!pathDictionary.ContainsKey(eachPathModel.location))
                    {
                        pathDictionary[eachPathModel.location] = new List<PathModel>();
                    }
                    List<PathModel> pathList = pathDictionary[eachPathModel.location];
                    if(pathList == null)
                    {
                        pathList = new List<PathModel>();
                    }

                    pathList.Add(eachPathModel);
                }
                success(pathDictionary);
            }
            else
            {
                fail();
            }
        }));
//#endif
    }


    public void MakePath(string start_location, Vector3 start_position, string end_location, Vector3 end_position, VPSTrackable[] trackables, Action fail, string serverName = "")
    {  
        startPosition = start_position;
        endPosition = end_position;

        FindPath(start_location, startPosition, end_location, endPosition, (paths) =>
        {
            RemovePaths();

            foreach (VPSTrackable vPSTrackable in trackables)
            {
                string location = vPSTrackable.localizerLocation[0];

                foreach (string pathLocation in paths.Keys)
                {
                    if (location.Contains(pathLocation))
                    {
                        GameObject pathObject = MakeArrowPath(paths[pathLocation].ToArray(), vPSTrackable);
                        pathObjects.Add(pathObject);
                        break;
                    }
                }

            }
        },
        () =>
        {
            fail();
        }, this);
    }

    public void RemovePaths()
    {
        arrowItems.Clear();
        foreach(GameObject pathObject in pathObjects)
        {
            Destroy(pathObject);
        }
        pathObjects.Clear();
    }

    private GameObject MakeArrowPath(PathModel[] paths, VPSTrackable vPSTrackable)
    {
        int i;

        List<Vector3> vectors = new List<Vector3>();
        foreach (PathModel eachModel in paths)
        {
            vectors.Add(new Vector3(eachModel.x, eachModel.y, eachModel.z));
        }

        List<Vector3> positions = new List<Vector3>();
        List<Vector3> directions = new List<Vector3>();
        CalculateMilestones(vectors, positions, directions, arrowPositionDistance, true);

        List<Vector3> convertVectorPath = new List<Vector3>();
        for (i = 0; i < positions.Count; i++)
        {
            Vector3 eachPath = positions[i];
            Vector3 pathPoint = new Vector3(eachPath.x, eachPath.z, eachPath.y);
            convertVectorPath.Add(pathPoint);
        }


        GameObject naviGameObject = new GameObject();
        naviGameObject.name = "Navigation";
        naviGameObject.transform.position = new Vector3(0, 0, 0);
        naviGameObject.transform.eulerAngles = new Vector3(0, 0, 0);
        naviGameObject.transform.localScale = new Vector3(1, 1, 1);

        naviGameObject.transform.parent = vPSTrackable.transform;

        Dictionary<GameObject, Dictionary<GameObject, float>> intersectionGameObjects = new Dictionary<GameObject, Dictionary<GameObject, float>>();

        Vector3 first;
        Vector3 second;
        Vector3 vec;
        for (i = 1; i < convertVectorPath.Count - 2; i++)
        {
            first = convertVectorPath[i] + new Vector3(0.0f, arrowPathHeight, 0.0f);
            second = convertVectorPath[i + 1] + new Vector3(0.0f, arrowPathHeight, 0.0f);

            vec = first - second;
            vec.Normalize();
            Quaternion q = Quaternion.LookRotation(vec);

            GameObject arrowGameObject = Instantiate(arrowPrefab);
            arrowGameObject.transform.position = first;
            arrowGameObject.transform.eulerAngles = arrowGameObject.transform.eulerAngles + q.eulerAngles;
            arrowGameObject.transform.parent = naviGameObject.transform;
            arrowGameObject.name = "arrow" + i;
     
            arrowItems.Add(arrowGameObject);
        }

        //hyojlee 2021.10.23
        //도착지를 나타낼 곳
        first = convertVectorPath[i] + new Vector3(0.0f, arrowPathHeight, 0.0f);
        second = convertVectorPath[i + 1] + new Vector3(0.0f, arrowPathHeight, 0.0f);

        vec = first - second;
        vec.Normalize();
        GameObject destination = Instantiate(arrival);
        destination.transform.position = first + new Vector3(0f, 2f, 0f);
        destination.transform.eulerAngles = destination.transform.eulerAngles + Quaternion.LookRotation(vec).eulerAngles ;
        destination.transform.parent = naviGameObject.transform;
        //Vector3 v3 = arrowItems[arrowItems.ToArray().Length - 1].transform.forward;
        //destination.transform.forward = -v3;
        destination.transform.rotation *= Quaternion.Euler(new Vector3(-80f, 0f, 0f));
        destination.name = "destination";
        MaxstSceneManager.chkNavi = true;
        MaxstSceneManager.DestroyFakeDestination();
		//
		// yunslee 2021.11.14 animation 적용
		if (NavigationController.characterType != e_character.none)
		{
			List<GameObject> naviTracks = new List<GameObject>();
			naviTracks.AddRange(GameObject.FindGameObjectsWithTag("naviTrack"));
            GameObject characterPrefab = null;
            switch (NavigationController.characterType)
			{
                case e_character.astronaut:
                    characterPrefab = astronautPrefab;
                    break;
                case e_character.coco:
                    characterPrefab = cocoPrefab;
                    break;
                case e_character.rabbit:
                    characterPrefab = rabbitPrefab;
                    break;
            }
            StartCoroutine(followTrack(naviTracks, characterPrefab));
			// 애니메이션 객체 생성 및 startcoroutine 돌리기
		}


		return naviGameObject;
    }

    void stopMotion(Animator animator, NavigationController.e_character characterType)
    {
        switch (characterType)
		{
            case e_character.none:
                break;
            case e_character.astronaut:
                animator.SetInteger("AnimationPar", 0);
                break;
            case e_character.coco:
                animator.SetInteger("Walk", 0);
                break;
            case e_character.rabbit:
                animator.SetInteger("AnimIndex", 0);
                animator.SetTrigger("Next");
                break;
        }
	}

    void runMotion(Animator animator, NavigationController.e_character characterType)
    {
        switch (characterType)
        {
            case e_character.none:
                break;
            case e_character.astronaut:
                animator.SetInteger("AnimationPar", 1);
                break;
            case e_character.coco:
                animator.SetInteger("Walk", 1);
                break;
            case e_character.rabbit:
                break;
        }
    }

    void showMessageWhenNaviTrackDestroy(NavigationController.e_character characterType)
	{
#if UNITY_EDITOR
        switch (characterType)
        {
            case e_character.none:
                break;
            case e_character.astronaut:
                Debug.Log("우주인이 길을 잃었습니다.");
                break;
            case e_character.coco:
                Debug.Log("꼬꼬가 길을 잃었습니다.");
                break;
            case e_character.rabbit:
                Debug.Log("토끼가 길을 잃었습니다.");
                break;
        }
#elif UNITY_ANDROID && !UNITY_EDITOR
        switch (characterType)
        {
            case e_character.none:
                break;
            case e_character.astronaut:
                Toast.ShowToastMessage("우주인이 길을 잃었습니다.", 2000);
                break;
            case e_character.coco:
                Toast.ShowToastMessage("꼬꼬가 잃었습니다.", 2000);
                break;
            case e_character.rabbit:
                Toast.ShowToastMessage("토끼가 잃었습니다.", 2000);
                break;
        }
#endif
    }

    IEnumerator followTrack(List<GameObject> naviTracks, GameObject characterPrefab)
	{
        if (characterPrefab == null)
            yield break;
        GameObject character = Instantiate(characterPrefab);
        character.transform.localPosition = naviTracks[0].transform.localPosition;
        character.transform.localScale = character.transform.localScale * characterScale;

        int i = 0;
        int count = naviTracks.Count;
        Animator animator = character.GetComponent<Animator>();
        Vector3 dir = new Vector3();

		if (NavigationController.characterType == e_character.rabbit)
		{
			animator.SetInteger("AnimIndex", 1);
			animator.SetTrigger("Next");
		}

        while (true)
        {
			if (naviTracks[i] == null)
			{
				showMessageWhenNaviTrackDestroy(NavigationController.characterType);
				stopMotion(animator, NavigationController.characterType);
				yield break;
			}

			//print($"Distance: {Vector3.Distance(arrowGroupList[i].transform.position, arrow.transform.position)}");
			if (Vector3.Distance(naviTracks[i].transform.position, character.transform.position) <= 0.1f)
            {
                i++;
                print($"{i}번째 track following");
                if (i == count)
                    break;
            }
            else
            {
                // 나와의 거리가 가까운 경우
                runMotion(animator, NavigationController.characterType);
                dir = naviTracks[i].transform.position - character.transform.position;
                dir.Normalize();
                character.transform.position += dir * characterMoveSpeed * Time.deltaTime;
                character.transform.forward = dir;
                yield return null;
                // 나와의 거리가 먼 경우
                // 멈춰서(Motion Stop) 나를 기다리도록 구현해야 함

            }
        }
        stopMotion(animator, NavigationController.characterType);
        print("Navigation End");
		yield break;
	}
    
    private Vector3 DivideBetweenTwoPoints(in Vector3 from, in Vector3 to, double ratio)
    {
        Vector3 res = new Vector3(0, 0, 0);
        if (ratio < 0.0 || ratio > 1.0)
            return res;

        res = from * (float)(1.0 - ratio) + to * (float)ratio;
        return res;
    }

    private void InterpolateByBezier(out Vector3 p, out Vector3 d, in Vector3 p0, in Vector3 p1, in Vector3 p2, double t)
    {
        double one_minus_t = 1.0 - t;
        p = (float)(one_minus_t * one_minus_t) * p0 + (float)(2.0 * t * one_minus_t) * p1 + (float)(t * t) * p2;
        d = (float)(-2.0 * one_minus_t) * p0 + (float)(2.0 * one_minus_t - 2.0 * t) * p1 + (float)(2.0 * t) * p2;
        d.Normalize();
    }

    private void CalculateMilestones(List<Vector3> path, in List<Vector3> pos, in List<Vector3> dir, double interval, bool useBezier)
    {
        double totalDist = 0.0;
        List<double> dists = new List<double>();
        dists.Add(0.0);
        int finalIndex = path.Count - 1;
        for (int i = 0; i < finalIndex; i++)
        {
            Vector3 cur = path[i];
            Vector3 next = path[i + 1];
            Vector3 diff = next - cur;
            totalDist += diff.magnitude;
            dists.Add(totalDist);
        }

        pos.Clear();
        dir.Clear();

        for (double d = 0.0; d <= totalDist; d += interval)
        {
            int next = 1;
            while (next < dists.Count && d > dists[next])
                next++;

            int cur = next - 1;
            Vector3 nextPoi = path[next];
            Vector3 curPoi = path[cur];

            double len = d - dists[cur];
            double overallLen = dists[next] - dists[cur];
            double ratio = len / overallLen;

            Vector3 po = DivideBetweenTwoPoints(curPoi, nextPoi, ratio);
            Vector3 di = nextPoi - curPoi;
            di.Normalize();

            pos.Add(po);
            dir.Add(di);
        }

        if (useBezier)
        {
            List<Vector3> newPos = pos;
            List<Vector3> newDir = dir;
            for (int i = 1; i < pos.Count - 1; i++)
            {
                int prev = i - 1;
                int cur = i;
                int next = i + 1;

                Vector3 outNewPos = Vector3.zero;
                Vector3 outNewDir = Vector3.zero;
                InterpolateByBezier(out outNewPos, out outNewDir, pos[prev], pos[cur], pos[next], 0.5);
                newPos[i] = outNewPos;
                newDir[i] = outNewDir;
            }
        }
    }
}
