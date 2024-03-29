using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bird : MonoBehaviour {
    [HideInInspector]
    public bool isClick = false;  
    public float maxDis = 3;//设置小鸟拖拽的最大距离
    public SpringJoint2D sp;//定义一个SpringJoint2D类型的组件
    [HideInInspector]//在属性面板中隐藏组件（如果组件的形式为Private则在属性面板中不显示，如果组件的形式为public则在属性面板中显示）
    public Rigidbody2D rg;//定义一个组件
    public LineRenderer left;//定义左边的线
    public Transform leftPos;//Transform包含了物体的位置，旋转等信息，这里用来申明一个物体leftPos 
    public LineRenderer right;//定义右边的线
    public Transform rightPos;//Transform包含了物体的位置，旋转等信息，这里用来申明一个物体rightPos 
    public GameObject boom;
    private TestMyTrail myTrail;
    private bool canMove = true;//用来判断小鸟能否移动（解决在小鸟飞出到消失的时间内点击小鸟仍出现划线的效果的bug）
    //Awake()是在脚本对象实例化时被调用的，而Start()是在对象的第一帧时被调用的，而且是在Update()之前。
    public float smooth = 3;//设置相机跟随小鸟的速率
    public float posX;
    private void Awake()
    {
        sp = GetComponent<SpringJoint2D>();//脚本将自动识别物体中的SpringJoint2D组件，并将其赋值给sp
        rg = GetComponent<Rigidbody2D>();//组件赋值
        myTrail = GetComponent<TestMyTrail>();//获取拖尾效果脚本
    }
    private void OnMouseDown()//当用户在GUIElement或者碰撞器中按下鼠标时系统会自动调用的函数
    {
        if (gameManager._instance.can == false)
            canMove = false;
        if (canMove) {
            isClick = true;
            rg.isKinematic = true;//表示开启运动学
            
        }
        
    }
    private void OnMouseUp()//当用户在GUIElement或者碰撞器中抬起鼠标时系统会自动调用的函数
    {
        if (canMove)
        {
            right.enabled = false;
            left.enabled = false;//鼠标抬起时禁用划线的功能
                                 //该部分力学分析：当不更改任何条件时，小鸟在被拖拽起来后受到重力和弹簧的弹力两个力的作用，当受力达不到理想的效果的时候，小鸟就有可能达不到理想的平抛效果，这是可以采取一种方法来实现小鸟的平抛效果，首先
            isClick = false;
            rg.isKinematic = false;//表示关闭运动学
            Invoke("Fly", 0.1f);//使用Invoke函数来实现延时执行函数，前面表示执行的函数名称，后面的表示延时的时长
                                //禁用划线操作
            canMove = false;//当鼠标抬起时到下一个小鸟飞起时禁用小鸟的划线功能和小鸟位置随鼠标移动的功能
        }
        
    }
    private void Update()
    {
        if (isClick)//当鼠标一直按下时，使用相关的运动学进行有关的使用及输出
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
            Line();//这里存在一个bug，由于每一只小鸟飞出5秒后才被毁灭，由于小鸟在飞出的那一刻就禁用了sp弹簧组件故5秒内弹簧功能就失效了，但是如果5秒内点击小鸟该部分代码仍然可以被执行且划线功能仍存在，不能达到理想的效果（解决方案：）
        }
        //相机跟随
        posX = transform.position.x;//获取到小鸟的x轴的位置
        //利用差值来实现相机的跟随，第一个值表示初始点，第二个值表示目标点，第三个值表示速率，即理解为物体以一个速率从初始点向目标点移动
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(Mathf.Clamp(posX, 0, 15), Camera.main.transform.position.y, Camera.main.transform.position.z), smooth*Time.deltaTime);//Mathf.Clamp(posX, 0, 15）中的0和15分别表示小鸟的坐标下限和坐标上限，其中当小鸟的坐标小于下限或者小鸟的坐标大于上限时按照临界点进行输出

    }
    public void Fly()//设定一个函数Fly()用来关闭弹簧的功能
    {
        myTrail.trailStart();
        sp.enabled = false;//当鼠标弹起时，将弹簧的功能禁用，可实现小鸟飞出的功能
        Invoke("Next", 5);//实现5秒后实现Next函数
    }
    public void Line()//划线操作，原理：两点确定一条直线
    {
        right.enabled = true;
        left.enabled = true;//开启组件实现划线功能
        right.SetPosition(0, rightPos.position);//锁定第一个点，索引为0
        right.SetPosition(1, transform.position);//锁定第二个点，索引为1
        left.SetPosition(0, leftPos.position);
        left.SetPosition(1, transform.position);
    }
    /// <summary>
    /// 下一只小鸟的飞出
    /// </summary>
    void Next()
    {
        gameManager._instance.birds.Remove(this);//移除集合内的bird（第一个飞出的小鸟）
        Destroy(gameObject);
        Instantiate(boom, transform.position, Quaternion.identity);//显示特效
        gameManager._instance.NextBird(); 
    }
    /// <summary>
    /// 一个碰撞器用来实现当小鸟碰撞到猪时关闭拖尾效果
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        myTrail.trailClear();
    }
}
