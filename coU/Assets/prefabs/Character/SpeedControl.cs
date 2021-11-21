using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedControl : MonoBehaviour
{
    static int touchCount = 0;

	// Start is called before the first frame update
	void Start()
    {
        touchCount = 0;
    }

	private void Update()
	{
		clickHandler();
	}

	void clickHandler()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Ended)
			{
				Ray ray = Camera.main.ScreenPointToRay(touch.position);
				RaycastHit hit;

				if (Physics.Raycast(ray, out hit))
				{
					if (hit.collider.tag == "naviTrack")
					{
						touchCount = (touchCount + 1) % 4;
						GameObject.Find("SceneManager").GetComponent<NavigationController>().characterMoveSpeed = 1f + (0.5f * touchCount);
						Debug.Log($"Speed {GameObject.Find("SceneManager").GetComponent<NavigationController>().characterMoveSpeed}");
					}
				}
			}
		}
		else if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider.tag == "naviTrack")
				{
					touchCount = (touchCount + 1) % 4;
					GameObject.Find("SceneManager").GetComponent<NavigationController>().characterMoveSpeed = 1f + (0.5f * touchCount);
					Debug.Log($"Speed {GameObject.Find("SceneManager").GetComponent<NavigationController>().characterMoveSpeed}");
				}
			}
		}
	}

	//}

	//Only PC
	//void OnMouseDown()
	//{
	//	touchCount = (touchCount + 1) % 3;
	//	GameObject.Find("SceneManager").GetComponent<NavigationController>().characterMoveSpeed = 1f + (2f * touchCount);
	//	Debug.Log($"Speed {GameObject.Find("SceneManager").GetComponent<NavigationController>().characterMoveSpeed}");
	//}

	//public void OnPointerClick(PointerEventData eventData)
	//{
	//	touchCount = (touchCount + 1) % 3;
	//	GameObject.Find("SceneManager").GetComponent<NavigationController>().characterMoveSpeed = 1f + (0.5f * touchCount);
	//	Debug.Log($"Speed {GameObject.Find("SceneManager").GetComponent<NavigationController>().characterMoveSpeed}");
	//}
}
