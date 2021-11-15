using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChooseCharacter : MonoBehaviour
{
    public void NextBtnOnClick()
    {
        TextMeshProUGUI tmpChoose = GameObject.Find("TMP_Choose").GetComponent<TextMeshProUGUI>();
        GameObject[] imgs = new GameObject[4];
        Transform imgsParent = GameObject.Find("Panel_ChooseImg").transform;

        imgs[0] = imgsParent.Find("Img_None").gameObject;
        imgs[1] = imgsParent.Find("Img_Astronaut").gameObject;
        imgs[2] = imgsParent.Find("Img_Rabbit").gameObject;
        imgs[3] = imgsParent.Find("Img_Coco").gameObject;

        if (tmpChoose.text == "선택안함")
        {
            tmpChoose.text = "우주인";
            imgs[0].SetActive(false);
            imgs[1].SetActive(true);
        }
        else if (tmpChoose.text == "우주인")
        {
            tmpChoose.text = "토끼";
            imgs[1].SetActive(false);
            imgs[2].SetActive(true);
        }
        else if (tmpChoose.text == "토끼")
        {
            tmpChoose.text = "꼬꼬";
            imgs[2].SetActive(false);
            imgs[3].SetActive(true);
        }
        else
        {
            tmpChoose.text = "선택안함";
            imgs[3].SetActive(false);
            imgs[0].SetActive(true);
        }
    }

    public void PrevBtnOnClick()
    {
        TextMeshProUGUI tmpChoose = GameObject.Find("TMP_Choose").GetComponent<TextMeshProUGUI>();
        GameObject[] imgs = new GameObject[4];
        Transform imgsParent = GameObject.Find("Panel_ChooseImg").transform;

        imgs[0] = imgsParent.Find("Img_None").gameObject;
        imgs[1] = imgsParent.Find("Img_Astronaut").gameObject;
        imgs[2] = imgsParent.Find("Img_Rabbit").gameObject;
        imgs[3] = imgsParent.Find("Img_Coco").gameObject;

        if (tmpChoose.text == "토끼")
        {
            tmpChoose.text = "우주인";
            imgs[2].SetActive(false);
            imgs[1].SetActive(true);
        }
        else if (tmpChoose.text == "꼬꼬")
        {
            tmpChoose.text = "토끼";
            imgs[3].SetActive(false);
            imgs[2].SetActive(true);
        }
        else if (tmpChoose.text == "선택안함")
        {
            tmpChoose.text = "꼬꼬";
            imgs[0].SetActive(false);
            imgs[3].SetActive(true);
        }
        else
        {
            tmpChoose.text = "선택안함";
            imgs[1].SetActive(false);
            imgs[0].SetActive(true);
        }
    }
}
