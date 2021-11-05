using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField]
    private RectTransform currentTransform;
    private GameObject mainContent;
    private Vector3 currentPosition;

    private int totalChild;

    public void OnPointerDown(PointerEventData eventData)
    {
        currentPosition = currentTransform.position;
        print("This is CurrentPosition First " + currentPosition.y.ToString());
        mainContent = currentTransform.parent.gameObject; //reorderable 항목들을 감싼 패널
        totalChild = mainContent.transform.childCount; //reorderable 항목들의 개 
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentTransform.position = new Vector3(currentTransform.position.x, eventData.position.y, currentTransform.position.z);

        for (int i = 0; i < totalChild; i++)
        {
            if (i != currentTransform.GetSiblingIndex())
            {
                Transform otherTransform = mainContent.transform.GetChild(i); //선택된 애 제외한 형제 
                int distance = (int)Vector3.Distance(currentTransform.position, otherTransform.position);
                print("CurrentTransform.GetSibilingIndex() is " + currentTransform.GetSiblingIndex().ToString() + "\t" + i.ToString() + "번째와의 distance is " + distance.ToString());
                if ((distance <= 50 && distance >= 0) || (distance < 0 && distance >= -50))
                {
                    Vector3 otherTransformOldPosition = otherTransform.position;
                    otherTransform.position = new Vector3(otherTransform.position.x, currentPosition.y, otherTransform.position.z);
                    currentTransform.position = new Vector3(currentTransform.position.x, otherTransform.position.y, currentTransform.position.z);
                    currentTransform.SetSiblingIndex(otherTransform.GetSiblingIndex());
                    currentPosition = currentTransform.position;
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //currentTransform.position = currentPosition;
        //int idx;
        //float firstPosY = -125f;
        //float currentY = currentTransform.localPosition.y;
        //print("What is currentY " + currentY.ToString());
        //int height = 150;
        //idx = (currentY - firstPosY) % height > 50 ? Mathf.Abs((int)(currentY - firstPosY) / height) : Mathf.Abs((int)(currentY - firstPosY) / height) + 1;
        //print("What is idx? " + idx.ToString());
        //currentY = firstPosY - height * idx;
        //print("After currentY " + currentY.ToString());
        //currentTransform.localPosition = new Vector3(736.5f, currentY, 0);

        currentTransform.localPosition = new Vector3(720f, -125 - currentTransform.GetSiblingIndex() * 150, 0f);
    }

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    currentTransform.localPosition = new Vector3(736.5f, -125 - currentTransform.GetSiblingIndex() * 150, 0f);
    //}
}
