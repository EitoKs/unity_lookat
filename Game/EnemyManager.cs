using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;        //TMPをつかうとき必要
//どの敵を何体出現させるか等を設定しランダムに出現させるスクリプト
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    //敵のプレハブ
    [Header("敵のプレハブ")]
    [SerializeField]
    private GameObject[] EnemyPrefab;
    //敵の出現範囲を示すオブジェクト
    [Header("出現範囲")]
    [SerializeField]
    private Transform RangeRight;
    [SerializeField]
    private Transform RangeLeft;
    //---------------------------------------------------------

    private GameObject[] enemyBox;  //敵の残数を確認する用の配列（クリア判定に必要）

    public float interval;     //出現間隔
    private float time;         //タイマー

    private int quota;  //ゲームクリアノルマ数（設定必須）
    private int enemy_occ;   //現時点での出現数（クリア判定で必要）

    private int[] type_quota = new int[6];    //敵の種類による出現数（設定必須）
    private int[] type_occ = new int[6];      //種類ごとの現時点での出現数
    private int respawn_type;   //出現させる種類

    public int remain_num;     //現在の残数をカウント用
    //-----------------------------------------------------------

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  //シーンをまたいでも破棄されない
        }
        else
        {
            Destroy(gameObject);            //ロード2回目以降は新しい方のオブジェクトを破棄(シングルトン)
        }
    }


    void Start()
    {
        //仮の設定
        int stage_quota = 6;
        int[] stage_type_quota = new int[6] {2,3,1,0,0,0};
        interval = 3f;
        remain_num = stage_quota;
        ResetStage(stage_quota, stage_type_quota);      //引数の条件のゲームが始まる
    }

    void Update()
    {
        if(!this.GetComponent<GameManagement>().now_Gameing) return;    //ゲームが始まっていなければ以下の処理をしない
        if(StageManager.Instance.now_Stage == 7) return;                //ステージ７なら以下の処理をしない
        CountEnemy();
        RandomRespawn();
    }

    //敵の残数を確認するための関数
    //ここでクリア判定もする
    void CountEnemy()
    {
        enemyBox = GameObject.FindGameObjectsWithTag("mosquito");
        //ノルマ数分の敵が出現して、ステージ上に敵がいなくなったらクリア
        if(enemyBox.Length == 0 && remain_num <= 0)  //enemy_occ == quota
        {
            Debug.Log("すべて消した");
            if(StageManager.Instance.now_Stage != 7)    //最後のステージでなければ
            {
                GameManagement.Instance.GameClearText();
                StageManager.Instance.StageClear();             //遊んだステージをクリアした判定にする
            }
        }
    }

    //敵をランダムに出現させる関数
    void RandomRespawn()
    {
        time += Time.deltaTime;     //出現する間隔を計測
        //現時点での出現数がノルマ数に達していなければ出現させる
        if(enemy_occ < quota && time > interval)
        {
            //出現範囲を設定
            float x,y;

            //x座標は左右どちらかにランダム
            if((int)Random.Range(1.0f, 11.0f) % 2 == 0)
            {
                x = Random.Range(RangeRight.position.x - 1f, RangeRight.position.x + 1f);
            }
            else
            {
                x = Random.Range(RangeLeft.position.x - 1f, RangeLeft.position.x + 1f);
            }
            //y座標もランダムに
            y = Random.Range(RangeRight.position.y, RangeLeft.position.y);

            EnemyTypeRespawn();     //出現させる種類を決める

            //敵プレハブを設定した座標に出現させる
            Instantiate(EnemyPrefab[respawn_type],
                        new Vector3(x, y, 0f),
                        EnemyPrefab[respawn_type].transform.rotation);
            
            enemy_occ++;    //現時点での合計出現数を加算
            time = 0f;  //タイマーをリセット
        }
    }

    //敵の種類によって出現数を制限
    //type_quota - 要素番号が敵の種類
    //EnemyPrefab - 要素番号が敵の種類
    void EnemyTypeRespawn()
    {
        //ここで何の種類の敵を出現させるかランダムに決める(割る数は種類数)
        int ran_num = (int)Random.Range(1.0f, 11.0f) % 6;
        //種類によっての出現数がノルマを超えていなければ出現させる
        //そうでなければもう一度この関数を呼び出し、出現させるまで繰り返す
        if(ran_num >= 0 && ran_num <= 6)
        {
            TypeProcess(ran_num);
        }
        else
        {
            Debug.Log("エラー");
        }
    }

    //種類を決めるときの処理
    void TypeProcess(int i)
    {
        if(type_occ[i] < type_quota[i])
        {
            respawn_type = i;
            type_occ[i]++;
        }
        else
        {
            EnemyTypeRespawn();
        }
    }

    //ステージ情報を変更する関数
    public void ResetStage(int stage_quota, int[] stage_type_quota)
    {
        enemy_occ = 0;  //現時点での出現数をクリア
        type_occ = new int[6]{0,0,0,0,0,0}; //種類ごとの出現数もクリア
        time = 0f;      //タイマーもリセット
        //念のためステージ上に敵がいるかチェックし、いたら消す
        AllEnemyDes();

        //ノルマ数（種類ごとも）を設定
        quota = stage_quota;
        type_quota = stage_type_quota;
        remain_num = stage_quota;   //残数をノルマ数で初期化
    }

    public void AllEnemyDes()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("mosquito");
        foreach(GameObject enemy in enemys)
        {
            Destroy(enemy);
        }
    }
}
