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
