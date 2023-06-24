using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//目から出る光線のスクリプト
public class LayScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "mosquito")
        {
            other.gameObject.GetComponent<EnemyScript>().Hiting = true;
        }
        else if(other.gameObject.tag == "mosquito_boss")
        {
            other.gameObject.GetComponent<EnemyBossScript>().Hiting = true;
        }
    }

    //光線が敵と重なっている間呼び出される処理
    void OnTriggerStay2D(Collider2D other)
    {
        //敵オブジェクトと重なっている時、当ててる敵のHPを減らす
        if(other.gameObject.tag == "mosquito")
        {
            other.gameObject.GetComponent<EnemyScript>().HPDown();
        }
        else if(other.gameObject.tag == "mosquito_boss")
        {
            other.gameObject.GetComponent<EnemyBossScript>().HPDown();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "mosquito")
        {
            other.gameObject.GetComponent<EnemyScript>().Hiting = false;
        }
        else if(other.gameObject.tag == "mosquito_boss")
        {
            other.gameObject.GetComponent<EnemyBossScript>().Hiting = false;
        }
    }
}
