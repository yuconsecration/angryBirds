using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public List<bird> birds;//创建鸟的集合
    public List<pig> pigs;//创建猪的集合
    public static gameManager _instance;//用来为其他脚本访问该脚本提供接口
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        Initialized();
    }
    /// <summary>
    /// 初始化小鸟
    /// </summary>
    private void Initialized()
    {
        //遍历鸟的集合
        for (int i = 0; i < birds.Count; i++)
        {
            if (i == 0)//第一只小鸟
            {
                birds[i].enabled = true;
                //要想访问sp组件，必须保证bird类的sp组件的访问权限为public
                birds[i].sp.enabled = true;//启用组件
            }
            else
            {
                birds[i].enabled = false;
                birds[i].sp.enabled = false;//禁用组件
            }
        }

    }
    /// <summary>
    /// 判定游戏逻辑
    /// </summary>
    public void NextBird()
    {
        if (pigs.Count > 0)
        {
            if (birds.Count > 0)
            {
                //下一只小鸟飞出
                Initialized();
            }
            else
            {
                //输了
            }
        }
        else
        {
            //赢了
        }
    }
}
    