using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyData
{
    // FirstScene, LoadingScene, MenuScene을 제외한 9개의 Scene에 대한 데이터
    class AllCategoryData
	{
        // SQLite DB에서 값을 가져오기 때문에 필요가 없다.
    }

    public class LoginSceneData
	{
        public bool isLogin { get; set; } = false;
        public User user { get; set; }
        public bool isAdvertise { get; set; } = false; //우리 매장 홍보하기를 누르고 로그인을 했나?
    }

    public class UploadSceneData
    {
        public bool isBeforeMenu { get; set; } = false;
    }

    public class StoreListSceneData
    {
        public string categorySub { get; set; } = "";
    }

    public class SearchSceneData
    {
        public string searchStr { get; set; } = "";
    }

    public class SelectSceneData
    {
        public string searchStr { get; set; } = "";
    }

    public class RegisterSceneData
    {
        // 내부적으로 모두 해결되는 변수만 가지고 있
    }

    public class StoreSceneData
	{
        public string storeName = "사봉";
        public string categoryMain = "뷰티";
        public string categorySub = "바디&향수";
        public string floor = "";
    }

}
