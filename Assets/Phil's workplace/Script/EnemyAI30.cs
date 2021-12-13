
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//2021/4/13最後更新
/*
public class EnemyAI30 : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject player;
    public float speed;
    [SerializeField] Transform target;
    //判定是否可以行走(插件) ,追擊速率(仍需要製作,先保留變數) , 製作可設定追擊目標


    [SerializeField]
    private Transform[] waypoints;
    //排列行走目標
    [SerializeField]
    private float moveSpeed = 2f;
    //正常行走速率
    private int waypointIndex = 0;

    public Vector2 deltaSpeed;
    public float delay = 2f;
    bool waiting = false;

    //行走目標代號
    //初始化陣列
    private void Start()
    {
       

    }

    // 每偵必續更新的數據
    private void Update()
    {

    }

    // 讓敵人行動的關鍵元素
    private void Move()
    {
        // 如果敵人沒有到達最後的過路標點那它可以繼續移動
        // 默認else,如果敵人到達最後一個標點會停止移動

     
    }

    IEnumerator Delay()
    {
        waiting = true;
        yield return new WaitForSeconds(delay);
        if (waypointIndex == waypoints.Length - 1)
        {
            waypointIndex = 0;
        }
        else
        {
            waypointIndex++;
        }
        waiting = false;
    }

}
*/