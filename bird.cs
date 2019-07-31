using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bird : MonoBehaviour {
    private bool isClick = false;
    private void OnMouseDown()//当用户在GUIElement或者碰撞器中按下鼠标时系统会自动调用的函数
    {
        isClick = true;
    }
    private void OnMouseUp()//当用户在GUIElement或者碰撞器中抬起鼠标时系统会自动调用的函数
    {
        isClick = false;
    }
    private void Update()
    {
        if (isClick)//当鼠标一直按下时
        {
            //transform.position = Input.mousePosition;//将鼠标的位置赋值给当前物体的位置
            //使用上一行代码会存在一个问题，鼠标的位置和当前物体的位置不在同一个坐标系下，小鸟的位置是在世界坐标系下(例如在unity中默认的(0,0)点位置和鼠标所在(0,0)点的位置(鼠标的(0,0)点是在屏幕的左下角)不同)需要来统一坐标系
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);//将屏幕坐标转化为世界坐标
            //这里存在一个问题，拖拽小鸟时出现小鸟的z轴被强制赋值为-10，和相机的z轴保持一致，在3D视角下可以发现，当小鸟的z轴小于-10或者大于0则相机都无法拍摄到小鸟的位置，所以需要强制锁定小鸟拖拽后的z轴的值在-10到0之间
            transform.position += new Vector3(0, 0, 6);//强制锁定小鸟的z轴值为-4
        }
    }
}

