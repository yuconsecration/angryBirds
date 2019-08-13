using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class gameManager : MonoBehaviour
{
    public List<bird> birds;//创建鸟的集合
    public List<pig> pigs;//创建猪的集合
    public static gameManager _instance;//用来为其他脚本访问该脚本提供接口  
    private Vector3 originPos;//初始化的位置
    public GameObject win;
    public GameObject lose;
    public GameObject[] stars;//设定一个数组用来存放星星的数量（将组件中的星星进行赋值）
    private void Awake()
    {
        _instance = this;
        if(birds.Count > 0) {
            originPos = birds[0].transform.position;
        }
        
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
        for (int i = 0; i < birds.Count; i++)//由于每一只小鸟释放后在集合中都被移除了故下一只小鸟的下标依然为0
        {
            if (i == 0)//第一只小鸟
            {
                birds[i].transform.position = originPos;//将第一支小鸟的位置赋值给后面每一只小鸟的位置
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
                lose.SetActive(true);
            }
        }
        else
        {
            //赢了
            win.SetActive(true);
        }
    }
    /// <summary>
    /// 通过游戏中小鸟的数量来设置星星的颗数
    /// </summary>
    public void showStars()
    {
        StartCoroutine("show");//启用设定的协程函数show()
    }
    /// <summary>
    /// 协程的书写来控制星星一颗一颗的显示
    /// </summary>
    /// <returns></returns>
    IEnumerator show()//设置一个协程函数show用来实现等待功能（用于实现星星一颗一颗出现且间隔有一个时间差）
    {
        for (int i = 0; i < birds.Count + 1; i++)//零个小鸟显示一颗星星,加一的原因在于如果当前的游戏场景中小鸟的数量就是零会出现一个bug，导致无法显示一颗星星
        {
            
            stars[i].SetActive(true);
            yield return new WaitForSeconds(0.4f);//表示等待0.4秒

        }
    }
    /// <summary>
    /// 重玩点击事件,需要加入命名空间UnityEngine.SceneManagement，重玩点击后重新加载gameManager场景
    /// </summary>
    public void Replay()
    {
        SceneManager.LoadScene(2); //重玩界面为重新加载game场景，2表示game场景的默认下标为2（在Uinty中点击file选择build setttings后，将设置的多个场景直接拖入到界面中，系统会给每一个场景设置一个下标）
    }
    /// <summary>
    /// 返回主菜单点击事件
    /// </summary>
    public void Home()
    {
        SceneManager.LoadScene(1); 
    }
}
    