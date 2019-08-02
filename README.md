<font color = blue>@elevateSober</font>

## 1.项目实施资源

### 1.1学习视频推荐（包含项目所需资源）

> http://www.sikiedu.com/course/134

## 2.项目实施过程

### 2.1工程的建立

1. 新建一个2D的工程，命名为：angryBirds

> 说明：一般2D的工程和3D的工程相比，在主摄像机上有一个区别在于：3D的摄像机属性面板中Projection选项一般为perspective（透视图），2D的摄像机属性面板中Projection选项一般为orthographic。

### 2.2资源导入，场景简单搭建

1. 在下载好的资源中选中Source文件夹下的Image和Music两个文件夹，将这两个文件夹复制粘贴到angryBirds工程目录下的assets文件夹下。

>说明：对于一张图片中包含多个元素，且需要截出其中一个元素，可以使用uinty中的一个功能实现：选择这张包含多个元素的图片，选择属性面板中的sprite mode中的multiple（多样的），然后点击sprite editor，在弹出的面板中选择左上角的slice，在选中的面板中选择slice，这样就能将图片中的各个元素切开了，退出sprite editor面板，在工程面板中选择刚才那张包含多个元素的图片，会发现下面多了很多个图片，这些就是切下来的图片。

3. 在uinty工程面板中新建一个文件夹命名为：Scene（用来存放场景），新建三个场景，分别命名为：00-loading，01-level，02-game。
4.  在场景02-game中：选择image文件夹下的BIRDS_1图片并对其元素进行剪切，然后将BIRDS1_0，BIRDS1_8，BIRDS1_159三张图片拖动到场景中并重命名为：birds，right，left。
5. 修改这三张图片的层级关系：在图片所在属性面板中选择sorting layer中的add sorting layer，点击加号新建一个层级命名为player，然后将这三张图片的层级都选择为player，将这三张图片面板属性中的order in layer属性的值分别设置为1,0,2。

> 说明：uinty中物体默认的层级为default，如果新建新的层级，则在显示上新的层级的物体会覆盖默认的层级；同时在一个层级中，order in layer的数值高的会覆盖数值低的，在本次层级设置中，左边的物体会覆盖小鸟，小鸟会覆盖右边的物体，从而来达到实际的效果。

### 2.3spring joint 2D组件的加入

1. 在bird和right之间设置一个spring joint 2D组件：首先选中bird，添加组件spring joint 2D，然后选中right添加组件rigibody 2D，并将body type属性设置为static在bird的spring joint 2D组件属性中，将auto configure distance不勾选，将distance属性设置为0.4，将frequency属性设置为2，在connected rigid body属性中将right拖入。

> 说明：spring joint 2D组件（弹簧）是一个弹簧组件，弹簧的一段固定在一个物体上（该物体不会弹起），另一端固定一个物体（该物体会围绕另一个物体弹起），这里要求两个物体都为刚体；在spring joint 2D的组件属性中，auto configure distance属性表示弹簧弹起的距离是否自动计算，这里的距离是另一个物体到该弹起物体的极限距离，表示弹起的物体不会弹到这个极限距离之内；distance是极限距离，frequency表示弹动的频率，connected rigid body属性是表示弹簧被固定那端的物体；在刚体属性中，body type属性设置为static表示刚体物体没有重力不会落下。

### 2.4小鸟的拖拽

1. 给小鸟添加一个圆型碰撞器，在小鸟的属性面板中输入点击add component，选择circle collider 2D，通过调节radius属性的值来调节圆形的大小。
2. 给小鸟添加一个C#脚本，命名为bird,代码如下：

```
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
            //transform.position += new Vector3(0, 0, 6);//强制锁定小鸟的z轴值为-4（方法一）
            transform.position += new Vector3(0, 0, -Camera.main.transform.position.z);//将小鸟的z轴坐标加上主摄像机的z轴坐标的赋值来强制锁定小鸟的z轴为0（方法二）
        }
    }
}
```

> 说明：圆形碰撞器的大小用来表示鼠标拖动物体有效的范围，在范围内鼠标都能实现拖动小鸟的功能。

### 2.5限定小鸟的最大拖拽距离

1. 首先给right创建一个子物体rightPos，用来计算小鸟和rightPos的距离（这里如果用right来计算距离，会发现right的中心点不在spring joint 2D弹簧端点上，这样计算会有很大误差，新建一个子物体，并将其中心点移动到弹簧的端点上，这样计算就更加准确）
2. 在脚本bird中新增如下代码

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bird : MonoBehaviour {
    private bool isClick = false;
    public Transform rightPos;//Transform包含了物体的位置，旋转等信息，这里用来申明一个物体rightPos   
    public float maxDis = 3;//设置小鸟拖拽的最大距离
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
        }
    }
}
```

### 2.6小鸟的飞出
> Dynamic、Static与IsKinematic的区别与辨析
> 1.有Collider和RigidBody的GameObject， Unity视之为Dynamic。
> 2.只含有Collider的GameObject， Unity视之为Static，适用于场景上的固定物体。
> 3.对于那些不会因为其他物体的影响而发生物理上的改变但是又不固定的对象，可以给它加一个Collider和RigidBody组件，并且把IsKinematic勾选上，叫做运动学刚体。IsKinematic的物体虽然不受其他物体影响，但是仍然可以与其它物体发生物理互动。
> 4.在本次代码书写中，isKinematic属性是确定刚体是否接受动力学模拟，不仅包括重力感应，还包括速度、阻力、质量等的物理模拟，弹簧在拉伸过程中，如果不开启，小鸟会一直受到弹簧弹力导致飞出去速度很大。
 
 > 在脚本brid.cs中添加如下代码：
 
 ```
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bird : MonoBehaviour {
    private bool isClick = false;
    public Transform rightPos;//Transform包含了物体的位置，旋转等信息，这里用来申明一个物体rightPos   
    public float maxDis = 3;//设置小鸟拖拽的最大距离
    public SpringJoint2D sp;//定义一个SpringJoint2D类型的组件
    private Rigidbody2D rg;//定义一个组件
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
        }
    }
    public void Fly()//设定一个函数Fly()用来关闭弹簧的功能
    {
        sp.enabled = false;//当鼠标弹起时，将弹簧的功能禁用，可实现小鸟飞出的功能
    }
}
```

### 2.7猪的受伤
1. 添加地面场景：在Image文件夹下裁剪图片THEME_01，裁剪效果为：
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190801164027886.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0lUX1NvZnRFbmdpbmVlcg==,size_16,color_FFFFFF,t_70)
2. 到场景中，同时给图片添加一个碰撞器，然后复制多个，配置成如下地面效果图：
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190801164204518.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0lUX1NvZnRFbmdpbmVlcg==,size_16,color_FFFFFF,t_70)

3. 在Image文件夹下选择图片BIRDS_1_103，重命名为pig01拖动到场景中，为该物体添加刚体，添加碰撞器，添加脚本pig，代码如下：

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pig : MonoBehaviour {
    public float maxSpeed = 10;//设置相对速度的上线
    public float minSpeed = 5;//设置相对速度的下线
    private SpriteRenderer render;//定义一个组件
    public Sprite hurt;//定义一个精灵
    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();//初始化组件
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > maxSpeed)//判断相对速度的大小，由于速度是个矢量，这里只要大小
        {
            Destroy(gameObject);//直接毁掉物体
        }
        else if(collision.relativeVelocity.magnitude > minSpeed && collision.relativeVelocity.magnitude < maxSpeed)
        {
            render.sprite = hurt;//改变组件中的sprite的值为hurt（这里表示更换图片）
        }
    }
}
```

4. 在pig脚本所在的属性面板中将hurt的图片改为BIRDS_1_104

### 2.8弹弓划线操作
1. 对right添加组件LineRenderer，将LineRenderer材质修改为Sprites-Default，颜色修改为树梢的颜色（采用提取颜色的方法），然后拖动箭头修改渐变色效果，如图：
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190801175541835.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0lUX1NvZnRFbmdpbmVlcg==,size_16,color_FFFFFF,t_70)

2. 点击LineRenderer属性面板后面的设置图标，选择copy component，然后在left物体的属性面板中Sprite Renderer属性右边的设置图标中选择paste component as new。

```
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
```

> 说明：当画出的线和树梢之间有一定间隔的时候，可能时rightPos物体和leftPose物体出现了问题，如果在场景面板中点击这两个物体，在窗口中不显示可移动的坐标图标，则需要重新创建这两个物体。

### 2.9死亡，加分特效制作

1. 设置死亡烟雾动画特效的制定：动画可以理解为多个图片连续播放形成的结果。
> 在BIRDS_1中裁剪出需要连续播放的图片(当图片太零散是可以手动选择多个零散图片组合成一个所需的图片)，按住ctrl键选择多个图片后拖动到场景内，会显示一个窗口（在该窗口中在Assets文件夹中创建名为animation，用来存放动画，将所选择的多个图片保存在该文件夹中重命名为boom（当同时选择多个图片拖拽到场景中时，uinty会自动识别为动画）），回到场景中将动画的名称修改为boom。

> 说明：如何修改动画中图片的播放顺序？
> 选择场景中动画的物体，选择ctrl+6就可以打开播放设置，在这个界面可以进行图片的顺序调换，图片的删除等操作，同时还可以改变图片之间的间隔时间。这里由于只需要这个动画在实际操作中播放一次，而动画系统默认时循环播放，点击工程窗口中刚刚新建的animation，选择其中的boom并将loop time选项不勾选即可，在物体boom中添加代码用来实现毁灭物体的操作，同时在动画的设置界面中在动画的最后一帧中添加该事件。
> ![在这里插入图片描述](https://img-blog.csdnimg.cn/20190802123006379.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0lUX1NvZnRFbmdpbmVlcg==,size_16,color_FFFFFF,t_70)
> ![在这里插入图片描述](https://img-blog.csdnimg.cn/20190802123024989.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0lUX1NvZnRFbmdpbmVlcg==,size_16,color_FFFFFF,t_70)
代码如下：
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class boom : MonoBehaviour {
    public void destroyBoom()
    {
        Destroy(gameObject);
    }
	}

2. 在脚本pig.cs中添加代码：

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pig : MonoBehaviour
{
    public float maxSpeed = 10;//设置相对速度的上线
    public float minSpeed = 5;//设置相对速度的下线
    private SpriteRenderer render;//定义一个组件
    public Sprite hurt;//定义一个精灵
    public GameObject boom;//定义一个物体，用来表示动画
    public GameObject pigScore;//定义一个物体，用来表示分数显示
    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();//初始化组件
    }
    public void OnCollisionEnter2D(Collision2D collision)//碰撞器方法使用
    {
        if (collision.relativeVelocity.magnitude > maxSpeed)//判断相对速度的大小，由于速度是个矢量，这里只要大小
        {
            destroyPig();
        }
        else if (collision.relativeVelocity.magnitude > minSpeed && collision.relativeVelocity.magnitude < maxSpeed)
        {
            render.sprite = hurt;//改变组件中的sprite的值为hurt（这里表示更换图片）
        }
    }
    public void destroyPig()//定义一个函数用来实现猪死亡后的操作
    {
        Destroy(gameObject);//直接毁掉猪
        Instantiate(boom, transform.position, Quaternion.identity);//instantiate函数可以用来实例化物体（可以理解为显示物体），后面两个参数分别表示物体显示的位置和是否旋转，Quaternion.identity表示不旋转
        GameObject go = Instantiate(pigScore, transform.position + new Vector3(0, 0.6f, 0), Quaternion.identity);//这里表示将分数的显示位置的y轴向上偏移了
        Destroy(go, 1.0f);//在1秒后毁坏物体，前面动画播放完毕后动画消失的功能除了在结束时间的帧上添加一个毁坏物体的事件外，也可以采用这种方法
                          //这里存在一个问题：就是这里毁坏物体不能够直接输入物体的名称，而是必须将其赋值给一个物体，这是因为在本案例中，该物体不会存在与场景中，而是需要通过代码进行调用，这里必须将这种既不能存在于场景中又要被调用的物体添加到工程目录下的prefabs文件夹中（该文件夹用来存放物体的模板，以保证物体在场景中被删除时可以被正常调用），对于不存在与场景中的物体如果直接在代码进行毁灭会导致模板被毁掉的风险，导致下次无法调用，所以需要将其赋值给一个物体，通过该物体进行显示，对该物体进行毁灭；如果该物体存在于场景中就可以直接删除
    }

}
```

3. 显示分数：通过裁剪相关的分数，采取同上面相同的方法进行实现，代码如上。上面时显示动画，这里为显示图片，同理。

### 2.10游戏逻辑的判定，实现多只小鸟的飞出