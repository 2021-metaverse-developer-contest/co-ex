using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ClickController : MonoBehaviour, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		//SceneManager.LoadSceneAsync("SelectStoreScene", LoadSceneMode.Additive);
		Transform canvasSelectStore = GameObject.Find("Canvas_SelectStore").transform;

		canvasSelectStore.Find("Panel_SelectStore").gameObject.SetActive(true);
		GameObject.Find("SelectStore").transform.Find("SelectStoreSceneManager").gameObject.SetActive(true);
	}
}
