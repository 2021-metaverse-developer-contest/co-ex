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
		float changedSpeed;
#if UNITY_EDITOR
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider.tag == "Character")
				{
					touchCount = (touchCount + 1) % 4;
					changedSpeed = 1f + (0.5f * touchCount);
					GameObject.Find("SceneManager").GetComponent<NavigationController>().characterMoveSpeed = changedSpeed;
					Debug.Log($"{GetCharacterName()} 속도: {changedSpeed}배속");
					Debug.Log($"Speed {changedSpeed}");
				}
			}
		}
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Ended)
			{
				Ray ray = Camera.main.ScreenPointToRay(touch.position);
				RaycastHit hit;

				if (Physics.Raycast(ray, out hit))
				{
					if (hit.collider.tag == "Character")
					{
						touchCount = (touchCount + 1) % 4;
						changedSpeed = 1f + (0.5f * touchCount);
						GameObject.Find("SceneManager").GetComponent<NavigationController>().characterMoveSpeed = changedSpeed;
						Toast.ShowToastMessage($"{GetCharacterName()} 속도: {changedSpeed}배속", Toast.Term.shortTerm);
					}
				}
			}
		}
#endif
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

	public static string GetCharacterName()
	{
		string retName = "";
		if (NavigationController.characterType == NavigationController.e_character.astronaut)
			retName = "우주인";
		else if (NavigationController.characterType == NavigationController.e_character.rabbit)
			retName = "토끼";
		else if (NavigationController.characterType == NavigationController.e_character.coco)
			retName = "꼬꼬";
		return retName;
	}
}
