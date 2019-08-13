using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//用于暂停后弹开面板的点击事件操作
public class pausePanel : MonoBehaviour {
    private Animator anim;
    public GameObject button;
    private void Awake()
    {
        anim = GetComponent<Animator>();//获取面板自身的动画状态机
    }
    public void Retry()
    {

    }
public void Pause()
    {
        //点击播放动画
        anim.SetBool("isPause", true);
        button.SetActive(false);//按钮图标隐藏
        
    }
public void Home()
    {

    }
    /// <summary>
    /// 点击了继续按钮
    /// </summary>
public void Resume()
    {
        //点击播放动画
        Time.timeScale = 1;//表示播放动画，用来处理动画状态机之间的关系
        anim.SetBool("isPause", false);
    }
    /// <summary>
    /// pause动画播放完调用
    /// </summary>
public void pauseAnimEnd()
    {
        Time.timeScale = 0;//表示暂停动画
    }
    /// <summary>
    /// resume动画播放完调用
    /// </summary>
    public void resumeAnimEnd()
    {
        button.SetActive(true);   
    }
}
