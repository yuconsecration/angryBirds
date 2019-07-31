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
            transform.position += new Vector3(0, 0, 6);//强制锁定小鸟的z轴值为-4
        }
    }
}
```

> 说明：圆形碰撞器的大小用来表示鼠标拖动物体有效的范围，在范围内鼠标都能实现拖动小鸟的功能。

### 2.5
