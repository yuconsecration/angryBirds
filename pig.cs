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
    public bool isPig = false;//判断是不是猪，用来指定消失的物体是猪，可以在属性面板中指定是不是猪
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
        if (isPig)
        {
            gameManager._instance.pigs.Remove(this);//移除集合中的猪，在执行是可以在属性面板中看到集合中相关物体的消失
        }
        Destroy(gameObject);//直接毁掉猪
        Instantiate(boom, transform.position, Quaternion.identity);//instantiate函数可以用来实例化物体（可以理解为显示物体），后面两个参数分别表示物体显示的位置和是否旋转，Quaternion.identity表示不旋转
        GameObject go = Instantiate(pigScore, transform.position + new Vector3(0, 0.6f, 0), Quaternion.identity);//这里表示将分数的显示位置的y轴向上偏移了
        Destroy(go, 1.0f);//在1秒后毁坏物体，前面动画播放完毕后动画消失的功能除了在结束时间的帧上添加一个毁坏物体的事件外，也可以采用这种方法
                          //这里存在一个问题：就是这里毁坏物体不能够直接输入物体的名称，而是必须将其赋值给一个物体，这是因为在本案例中，该物体不会存在与场景中，而是需要通过代码进行调用，这里必须将这种既不能存在于场景中又要被调用的物体添加到工程目录下的prefabs文件夹中（该文件夹用来存放物体的模板，以保证物体在场景中被删除时可以被正常调用），对于不存在与场景中的物体如果直接在代码进行毁灭会导致模板被毁掉的风险，导致下次无法调用，所以需要将其赋值给一个物体，通过该物体进行显示，对该物体进行毁灭；如果该物体存在于场景中就可以直接删除
    }

}