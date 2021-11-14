using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImgScrolling : MonoBehaviour
{
	public RectTransform content; //움직일 오브젝트
	public int count; //나눠야할 값
	private float pos; //content의 LocalPosition
	private float movepos; //움직일 값
	private bool isScroll = false; //움직여야하는 지 구별

	// Start is called before the first frame update
	void Start()
	{
		pos = content.localPosition.x;
		movepos = content.rect.xMax - content.rect.xMax / count;
		Debug.Log($"Start's movepos, content.rect.xMax {movepos.ToString()}, {content.rect.xMax}");
		Debug.Log($"Start's rect.width {content.rect.width}");
		//while (Vector2.Distance(content.localPosition, new Vector2(2812, 0)) >= 0.1f)
		//    content.localPosition = Vector2.Lerp(content.localPosition, new Vector2(2812, 0), Time.deltaTime * 5);
	}

	public void Right()
	{
		Debug.Log($"Start's no   movepos, content.rect.xMax {movepos.ToString()}, {content.rect.xMax}");
		Debug.Log($"Start's no   rect.width {content.rect.width}");
		if (content.rect.xMin + content.rect.xMax / count == movepos)
		{
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
		Debug.Log($"Start's left   movepos, content.rect.xMax {movepos.ToString()}, {content.rect.xMax}");
		Debug.Log($"Start's left   rect.width {content.rect.width}");
		if (content.rect.xMax - content.rect.xMax / count == movepos)
		{
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
		while (isScroll)
		{
			content.localPosition = Vector2.Lerp(content.localPosition, new Vector2(movepos, 0), Time.deltaTime * 5);
			if (Vector2.Distance(content.localPosition, new Vector2(movepos, 0)) < 0.1f)
			{
				isScroll = false;
			}
			yield return null;
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
