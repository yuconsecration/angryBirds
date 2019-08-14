12using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class win : MonoBehaviour {
    /// <summary>
    /// 动画播放完毕后显示星星的操作
    /// </summary>
	public void show()
    {
        gameManager._instance.showStars();
    }
}
