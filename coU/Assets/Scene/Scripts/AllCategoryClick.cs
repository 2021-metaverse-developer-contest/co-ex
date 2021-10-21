using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AllCategoryClick : MonoBehaviour
{
    public void CategoryMainClick()
    {
        GameObject clickObj = EventSystem.current.currentSelectedGameObject;
        print("clickObj " + clickObj.transform.Find("Panel_Center").GetComponentInChildren<TextMeshProUGUI>().text);
        GameObject subCt = clickObj.transform.parent.Find("Panel_SubCt").gameObject;
        Image img = clickObj.transform.Find("Panel_Right").gameObject.GetComponentInChildren<Image>();

        if (subCt.active)
        {
            img.sprite = Resources.Load("toggle_off_icon", typeof(Sprite)) as Sprite;
            subCt.SetActive(false);
        }
        else
        {
            img.sprite = Resources.Load("toggle_on_icon", typeof(Sprite)) as Sprite;
            subCt.SetActive(true);
        }

        // 2021/10/13 hyojlee
        // 카테고리 메인 클릭 시 다른 카테고리 클릭 전까지 뭉개지는 현상(content size fitter가 즉시 적용되지 않음)을 해결하기 위해
        // Canvas.ForceUpadateCanvases();를 사용하려고 했으나 개선되지도 않고 CPU 사이클을 많이 소모한다고 함
        // 그래서 아래와 같은 코드로 고침
        // 참고 사이트: https://forum.unity.com/threads/content-size-fitter-refresh-problem.498536/
        LayoutRebuilder.ForceRebuildLayoutImmediate(clickObj.transform.parent.GetComponent<RectTransform>());
    }


    public void CategorySubBtnOnClick()
    {
        GameObject clickObj = EventSystem.current.currentSelectedGameObject;
        print("clickObj " + clickObj.transform.Find("Panel_Left").GetComponentInChildren<TextMeshProUGUI>().text);
        StoreListSceneManager.categorySub = clickObj.transform.Find("Panel_Left").GetComponentInChildren<TextMeshProUGUI>().text;
        Stack.Instance.Push(new SceneInfo(SceneManager.GetActiveScene().buildIndex, StoreListSceneManager.categorySub, false));
        SceneManager.LoadScene("StoreListScene");
    }
}
