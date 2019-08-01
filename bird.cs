using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bird : MonoBehaviour {
    private bool isClick = false;  
    public float maxDis = 3;//设置小鸟拖拽的最大距离
    public SpringJoint2D sp;//定义一个SpringJoint2D类型的组件
    private Rigidbody2D rg;//定义一个组件
    public LineRenderer left;//定义左边的线
    public Transform leftPos;//Transform包含了物体的位置，旋转等信息，这里用来申明一个物体leftPos 
    public LineRenderer right;//定义右边的线
    public Transform rightPos;//Transform包含了物体的位置，旋转等信息，这里用来申明一个物体rightPos 
    //Awake()是在脚本对象实例化时被调用的，而Start()是在对象的第一帧时被调用的，而且是在Update()之前。
    private void Awake()
    {
        sp = GetComponent<SpringJoint2D>();//脚本将自动识别物体中的SpringJoint2D组件，并将其赋值给sp
        rg = GetComponent<Rigidbody2D>();//组件赋值
    }
    private void OnMouseDown()//当用户在GUIElement或者碰撞器中按下鼠标时系统会自动调用的函数
    {
        isClick = true;
        //
        rg.isKinematic = true;//表示开启运动学
    }
    private void OnMouseUp()//当用户在GUIElement或者碰撞器中抬起鼠标时系统会自动调用的函数
    {
        //该部分力学分析：当不更改任何条件时，小鸟在被拖拽起来后受到重力和弹簧的弹力两个力的作用，当受力达不到理想的效果的时候，小鸟就有可能达不到理想的平抛效果，这是可以采取一种方法来实现小鸟的平抛效果，首先
        isClick = false;
        rg.isKinematic = false;//表示关闭运动学
        Invoke("Fly", 0.1f);//使用Invoke函数来实现延时执行函数，前面表示执行的函数名称，后面的表示延时的时长
    }
    private void Update()
    {
        if (isClick)//当鼠标一直按下时
        {
            //transform.position = Input.mousePosition;//将鼠标的位置赋值给当前物体的位置，transform理解为当前物体
            //使用上一行代码会存在一个问题，鼠标的位置和当前物体的位置不在同一个坐标系下，小鸟的位置是在世界坐标系下(例如在unity中默认的(0,0)点位置和鼠标所在(0,0)点的位置(鼠标的(0,0)点是在屏幕的左下角)不同)需要来统一坐标系
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);//将屏幕坐标转化为世界坐标
            //这里存在一个问题，拖拽小鸟时出现小鸟的z轴被强制赋值为-10，和相机的z轴保持一致，在3D视角下可以发现，当小鸟的z轴小于-10或者大于0则相机都无法拍摄到小鸟的位置，所以需要强制锁定小鸟拖拽后的z轴的值在-10到0之间
            //transform.position += new Vector3(0, 0, 6);//强制锁定小鸟的z轴值为-4（方法一），Vector3表示一个三维向量，包含位置，方向等信息
            transform.position += new Vector3(0, 0, -Camera.main.transform.position.z);//将小鸟的z轴坐标加上主摄像机的z轴坐标的赋值来强制锁定小鸟的z轴为0（方法二）
            if (Vector3.Distance(transform.position, rightPos.position) > maxDis)//Vector3中有一个方法Distance用来计算两个向量的位移，这里用来计算小鸟和rightPos之间的位移
            {
                Vector3 pos = (transform.position - rightPos.position).normalized;//单位化向量，normalized属性用来实现单位化向量
                pos *= maxDis;//得到最大长度向量
                transform.position = pos + rightPos.position;//小鸟的位置限制
            }
            Line();
        }
    }
    public void Fly()//设定一个函数Fly()用来关闭弹簧的功能
    {
        sp.enabled = false;//当鼠标弹起时，将弹簧的功能禁用，可实现小鸟飞出的功能
    }
    public void Line()//划线操作，原理：两点确定一条直线
    {
        right.SetPosition(0, rightPos.position);//锁定第一个点，索引为0
        right.SetPosition(1, transform.position);//锁定第二个点，索引为1
        left.SetPosition(0, leftPos.position);
        left.SetPosition(1, transform.position);
    }
}
