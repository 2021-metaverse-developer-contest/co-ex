using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImgScrolling : MonoBehaviour
{
	public RectTransform content; //움직일 오브젝트
	Button nextBtn;
	private int count = 0; //나눠야할 값
	private float pos; //content의 LocalPosition
	private float movepos; //움직일 값
	private bool isScroll = false; //움직여야하는 지 구별
	private float imgWidth;
	float nextTime;
	float timeLeft = 5.0f;

	private void Awake()
	{
		nextBtn = GameObject.Find("Btn_Next").GetComponent<Button>();	
	}

	// Start is called before the first frame update
	void Start()
	{
		nextTime = Time.time + timeLeft;
		imgWidth = GameObject.Find("Img_Logo").GetComponent<RectTransform>().rect.width;
		Debug.Log($"Start's imgWidth {imgWidth}");
		//foreach (Transform c in content.transform)
		//{
		//	if (c.gameObject.activeSelf)
		//		count++;
		//}

		//Debug.Log($"Start's child count {count}");
		//Debug.Log($"Start's movepos, content.rect.xMax {movepos.ToString()}, {content.rect.xMax}");
		//Debug.Log($"Start's rect.width {content.rect.width}");

		/*
		 * movepos = content.rect.xMax - content.rect.xMax / count
		 *         = content.rect.xMax * (count - 1) /count
		 * content.rect.xMax = imgWidth * count / 2
		 * => movepos = 1406 * (count - 1) / 2
		 */
		ReadImgDBCoroutine();
	}

	void ReadImgDBCoroutine()
	{
		StartCoroutine(ReadImgDB());
	}

	IEnumerator ReadImgDB()
	{
		FirebaseRealtimeManager.Instance.readStoreImgs(StoreSceneManager.storeName);
		yield return WaitServer.Instance.waitServer();
		count = FirebaseRealtimeManager.Instance.ListStoreImgs.ToArray().Length;
		Debug.Log($"ImgScrolling count {count}");
		movepos = imgWidth * (count - 1) / 2;
		while (Vector2.Distance(content.localPosition, new Vector2(movepos, 0)) >= 0.1f)
			content.localPosition = Vector2.Lerp(content.localPosition, new Vector2(movepos, 0), Time.deltaTime * 5);
		pos = content.localPosition.x;
	}

	public void Right()
	{
		//Debug.Log($"Start's right   movepos			  {movepos.ToString()}");
		//Debug.Log($"Start's right   content.rect.xMax {content.rect.xMax}");
		//Debug.Log($"Start's right   content.rect.xMin {content.rect.xMin}");
		//Debug.Log($"currentLocalPosition {content.localPosition.ToString()}");
		//Debug.Log($"Start's right {content.rect.xMin + content.rect.xMax / count}");
		if (content.rect.xMin + content.rect.xMax / count  == Math.Round(movepos))
		{
			movepos = imgWidth * (count - 1) / 2;
			while (Vector2.Distance(content.localPosition, new Vector2(movepos, 0)) >= 0.1f)
				content.localPosition = Vector2.Lerp(content.localPosition, new Vector2(movepos, 0), Time.deltaTime * 5);
			pos = content.localPosition.x;
			Debug.Log("Don't Move Right"); //Left로 가야됨
		}
		else
		{
			isScroll = true;
			movepos = pos - content.rect.width / count;
			pos = movepos;
			StartCoroutine(Scroll());
		}
	}

	public void Left()
	{
		//Debug.Log($"Start's left   movepos			  {Math.Round(movepos).ToString()}");
		//Debug.Log($"Start's left   content.rect.xMax {content.rect.xMax}");
		//Debug.Log($"Start's left   content.rect.xMin {content.rect.xMin}");
		//Debug.Log($"currentLocalPosition {content.localPosition.ToString()}");
		//Debug.Log($"Start's left {content.rect.xMax - content.rect.xMax / count}");
		if (content.rect.xMax - content.rect.xMax / count == Math.Round(movepos))
		{
			movepos = imgWidth * (count - 1) / 2 * -1;
			while (Vector2.Distance(content.localPosition, new Vector2(movepos, 0)) >= 0.1f)
				content.localPosition = Vector2.Lerp(content.localPosition, new Vector2(movepos, 0), Time.deltaTime * 5);
			pos = content.localPosition.x;
			Debug.Log("Don't Move Left"); //Right로 가야됨
		}
		else
		{
			isScroll = true;
			movepos = pos + content.rect.width / count;
			pos = movepos;
			StartCoroutine(Scroll());
		}
	}

	IEnumerator Scroll()
	{
		nextTime = Time.time + timeLeft;
		while (isScroll)
		{
			content.localPosition = Vector2.Lerp(content.localPosition, new Vector2(movepos, 0), Time.deltaTime * 5);
			if (Vector2.Distance(content.localPosition, new Vector2(movepos, 0)) < 0.1f)
			{
				Debug.Log($"Start's Coroutine {content.localPosition}");
				isScroll = false;
			}
			yield return null;
		}
	}

	private void Update()
	{
		if (Time.time > nextTime)
		{
			nextTime = Time.time + timeLeft;
			Right();
		}
	}
}
