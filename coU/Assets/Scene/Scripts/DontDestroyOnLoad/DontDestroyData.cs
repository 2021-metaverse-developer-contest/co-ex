using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyData
{
    // FirstScene, LoadingScene을 제외한 9개의 Scene에 대한 데이터
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

}
