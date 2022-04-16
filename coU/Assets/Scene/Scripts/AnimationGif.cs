using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AnimationGif : MonoBehaviour
{
    const int numOfImg = 284;
    //Sprite[] spritesAstronaut;
    //Sprite[] spritesRabbit;
    //Sprite[] spritesCoco;
    Sprite[][] sprites = new Sprite[3][];
    //public Image[] imgs = new Image[4];
    public RawImage[] raws = new RawImage[3];

    // Start is called before the first frame update
    void Start()
    {
        //Texture2D texture;
        //sprites[0] = new Sprite[numOfImg];
        //sprites[1] = new Sprite[numOfImg];
        //sprites[2] = new Sprite[numOfImg];
        //for (int i = 0; i < numOfImg; i++)
        //{
        //    sprites[0][i] = Resources.Load($"Navi_Character/astronaut/astronaut{i}", typeof(Sprite)) as Sprite;
        //    //sprites[0][i] = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0f, 0f), 100f);
        //}
        //for (int i = 0; i < numOfImg; i++)
        //{
        //    sprites[1][i] = Resources.Load($"Navi_Character/rabbit/rabbit{i}", typeof(Sprite)) as Sprite;
        //    //sprites[1][i] = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0f, 0f), 100f);
        //}
        //for (int i = 0; i < numOfImg; i++)
        //{
        //    sprites[2][i] = Resources.Load($"Navi_Character/coco/coco{i}", typeof(Sprite)) as Sprite;
        //    //sprites[2][i] = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0f, 0f), 100f);
        //}
    }

    // Update is called once per frame
    void Update()
    {
		//imgs[1].sprite = sprites[0][(int)(Time.time * 10) % numOfImg];

        //for (int i = 1; i < 4; i++)
		//{
		//	if (imgs[i].gameObject.activeSelf)
		//	{
		//		imgs[i].sprite = sprites[i - 1][(int)(Time.time * 10) % numOfImg];
		//	}
		//}

		//imgs[2].sprite = sprites[1][(int)(Time.time * 10) % numOfImg];
		//imgs[3].sprite = sprites[2][(int)(Time.time * 10) % numOfImg];
	}
}
