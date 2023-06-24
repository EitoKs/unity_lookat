using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//イベントで出てくる敵を制御するスクリプト
public class EventEnemy : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Enemy;     //敵のプレハブを入れておく

    public int[] eventControl;      //各イベントの実行順序を制御する変数

    public GameObject eventEnemy;

    void Start()
    {
        
    }

    //「第１夜」のゲーム前イベントの処理
    public void EnemyEvent1_1()
    {
        if(eventControl[0] == 0){
            //ノーマルな敵を左から出現させる
            eventEnemy = Instantiate(Enemy[0],
                                    new Vector3(-4, 2, 0f),
                                    Enemy[0].transform.rotation);
        }
        else if(eventControl[0] == 1)
        {
            if(eventEnemy == null) return;
            //if(eventEnemy.transform.position.x >= 3) 
            Destroy(eventEnemy);   //右まで到達したら一旦敵を消す
        }
        else if(eventControl[0] == 2)
        {
            //再び右から左へ移動を始める
            eventEnemy = Instantiate(Enemy[0],
                                    new Vector3(4, 2, 0f),
                                    Enemy[0].transform.rotation);
        }
        else if(eventControl[0] == 3)
        {
            if(eventEnemy == null) return;
            eventEnemy.GetComponent<EnemyScript>().DesProcess();    //光線を当てられて石にされて消える
            eventControl[0] = 0;
        }
        else{
            return;
        }
        eventControl[0]++;
    }
    //「第２夜」のゲーム前イベント
    public void EnemyEvent2_1()
    {
        if(eventControl[1] == 0)
        {
            //速いやつを左から出現させる
            eventEnemy = Instantiate(Enemy[1],
                                    new Vector3(-4, 2, 0f),
                                    Enemy[1].transform.rotation);
        }
        else if(eventControl[1] == 1)
        {
            if(eventEnemy == null) return;
            Destroy(eventEnemy);   //右まで到達したら一旦敵を消す
            eventControl[1] = 0;
        }
        else
        {
            return;
        }
        eventControl[1]++;
    }

    //「第３夜」のゲーム前イベント
    public void EnemyEvent3_1()
    {
        if(eventControl[2] == 0)
        {
            //速いやつを右から出現させる
            eventEnemy = Instantiate(Enemy[2],
                                    new Vector3(4, 2, 0f),
                                    Enemy[2].transform.rotation);
        }
        else if(eventControl[2] == 1)
        {
            if(eventEnemy == null) return;
            Destroy(eventEnemy);   //右まで到達したら一旦敵を消す
            eventControl[2] = 0;
        }
        else
        {
            return;
        }
        eventControl[2]++;
    }

    //「第４夜」のゲーム前イベント
    public void EnemyEvent4_1()
    {
        if(eventControl[3] == 0)
        {
            //速いやつを右から出現させる
            eventEnemy = Instantiate(Enemy[3],
                                    new Vector3(4, 2, 0f),
                                    Enemy[3].transform.rotation);
        }
        else if(eventControl[3] == 1)
        {
            if(eventEnemy == null) return;
            Destroy(eventEnemy);   //右まで到達したら一旦敵を消す
            eventControl[3] = 0;
        }
        else
        {
            return;
        }
        eventControl[3]++;
    }

    //「第5夜」のゲーム前イベント
    public void EnemyEvent5_1()
    {
        if(eventControl[4] == 0)
        {
            //速いやつを左から出現させる
            eventEnemy = Instantiate(Enemy[4],
                                    new Vector3(-4, 2, 0f),
                                    Enemy[4].transform.rotation);
        }
        else if(eventControl[4] == 1)
        {
            if(eventEnemy == null) return;
            Destroy(eventEnemy);   //右まで到達したら一旦敵を消す
            eventControl[4] = 0;
        }
        else
        {
            return;
        }
        eventControl[4]++;
    }

    //「第6夜」のゲーム前イベント
    public void EnemyEvent6_1()
    {
        if(eventControl[5] == 0){
            //ノーマルな敵を左から出現させる
            eventEnemy = Instantiate(Enemy[5],
                                    new Vector3(-4, 2, 0f),
                                    Enemy[5].transform.rotation);
        }
        else if(eventControl[5] == 1)
        {
            if(eventEnemy == null) return;
            Destroy(eventEnemy);   //右まで到達したら一旦敵を消す
        }
        else if(eventControl[5] == 2)
        {
            //再び右から左へ移動を始める
            eventEnemy = Instantiate(Enemy[5],
                                    new Vector3(4, 2, 0f),
                                    Enemy[5].transform.rotation);
        }
        else if(eventControl[5] == 3)
        {
            if(eventEnemy == null) return;
            eventEnemy.GetComponent<EnemyScript>().DesProcess();    //光線を当てられて石にされて消える
            eventControl[5] = 0;
        }
        else{
            return;
        }
        eventControl[5]++;
    }

    //「第7夜」のゲーム前イベント
    public void EnemyEvent7_1()
    {
        if(eventControl[6] == 0){
            //ノーマルな敵を真ん中から出現させる
            eventEnemy = Instantiate(Enemy[6],
                                    new Vector3(0, 0, 0f),
                                    Enemy[6].transform.rotation);
            eventControl[6] = 0;
        }
        else{
            return;
        }
        eventControl[6]++;
    }

    //「第7夜」のゲーム前イベント
    public void EnemyEvent7_2()
    {
        if(eventControl[7] == 0){
            //ノーマルな敵を真ん中に移動させる
            eventEnemy.transform.position = new Vector3(0, 0, 0);
        }
        else if(eventControl[7] == 1)
        {
            if(eventEnemy == null) return;
            eventEnemy.GetComponent<EnemyBossScript>().DesProcess();    //光線を当てられて石にされて消える
            eventControl[7] = 0;
        }
        else{
            return;
        }
        eventControl[7]++;
    }
}
