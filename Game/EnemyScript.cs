using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//敵のHPを管理するスクリプト
//敵の移動、種類も管理するスクリプト
public class EnemyScript : MonoBehaviour
{
    private float enemy_hp;  //敵のHP
    Rigidbody2D enemy_rb2d; //敵のリジッドボディ

    private float move_speed_x;    //横方向に動くスピード
    private float move_speed_y;    //縦方向に動くスピード

    private int direction = 1;          //動く方向（x軸方向を往復）
    private int dir_x;
    private int dir_y;

    public bool Hiting;     //当たっているかの判定
    private bool isDeath;   //死んでいるかの判定

    private float y_pos;                //y座標の初期値

    private Animator enemy_animator;    //敵のアニメーター

    //移動範囲を示すオブジェクトを格納
    private Transform rightwall;
    private Transform leftwall;

    public EnemyType state;
    //敵の種類
    public enum EnemyType
    {
        normal,     //通常種
        fast,       //縦の移動はないが素早い・HP低
        height,     //縦の動きが大きい
        drunk,      //動きが不規則だが少し遅い・HP高
        infini,     //8の字で動き回る・HP低
        stealth     //姿が消える・HP低
    }


    //始めに１度呼び出される
    void Start()
    {
        isDeath = false;
        y_pos = transform.position.y;   //y座標の初期値を記録
        enemy_rb2d = GetComponent<Rigidbody2D>();
        //シーン上のオブジェクトを探し、そのTransform型で受け取る
        rightwall = GameObject.Find("RightWall").transform;
        leftwall = GameObject.Find("LeftWall").transform;
        enemy_animator = GetComponent<Animator>();
        EnemyTypeJudge();
        ImageDir();     //はじめの進行方向を決めておく
    }

    //毎フレーム呼び出される
    void Update()
    {
        StealthColor(); //ステルスの種類なら姿の出現判定を入れる
        if(!GameManagement.Instance.now_Gameing) return;    //ゲーム中でなければ処理をしない
        EnemyDes();     //敵の消滅判定
        //StealthColor(); //ステルスの種類なら姿の出現判定を入れる
    }

    //0.02秒毎に呼び出される
    void FixedUpdate()
    {
        if(isDeath == true) return;     //倒した奴は動きを止める
        if(!GameManagement.Instance.now_Gameing && !GameManagement.Instance.now_Event) return;    //ゲーム中でなければ処理をしない
        RoopMove();

        EnemyMoveType();
    }

    //HPを減らす関数（光線に当たっている時に実行）
    public void HPDown()
    {
        enemy_hp -= 0.01f;
    }

    void EnemyDes()
    {
        //HPが０になったとき敵オブジェクトを消す(３秒後)
        if(enemy_hp <= 0f && isDeath == false)
        {
            EnemyManager.Instance.remain_num--;       //残数を減らす
            GameManagement.Instance.RemainTextShow(EnemyManager.Instance.remain_num);   //UIを変化
            DesProcess();
        }
    }
    //やられたときのアニメーションと削除処理
    public void DesProcess()
    {
        SoundManager.Instance.PlaySE(1);
        isDeath = true;
        enemy_animator.SetTrigger("enemy_des");     //消えるアニメーションを実行し消す
        Destroy(this.gameObject, 3f);
    }

    //敵の種類を決定する関数
    void EnemyTypeJudge()
    {
        //敵の種類によって速さを変える
        switch(state)
        {
            case EnemyType.normal:
                move_speed_x = 1.0f;
                move_speed_y = 0.3f;
                enemy_hp = 3f;
                break;
            case EnemyType.fast:
                move_speed_x = 1.5f;
                move_speed_y = 0f;
                enemy_hp = 2f;
                break;
            case EnemyType.height:
                move_speed_x = 1f;
                move_speed_y = 2f;
                enemy_hp = 3f;
                break;
            case EnemyType.drunk:
                move_speed_x = 0.8f;
                move_speed_y = 0.8f;
                enemy_hp = 5f;
                break;
            case EnemyType.infini:
                move_speed_x = 1f;
                move_speed_y = 1f;
                enemy_hp = 2f;
                //８の字型の奴だけ２種類ある
                if((int)Random.Range(1.0f, 11.0f) % 2 == 0)
                {
                    transform.position = new Vector2(leftwall.position.x + 1.5f, rightwall.position.y); //初期位置を固定
                    dir_x = 1;
                    dir_y = 1;  //上方向に８の字型を書く種類
                }
                else
                {
                    transform.position = new Vector2(rightwall.position.x - 1.5f, leftwall.position.y); //初期位置を固定
                    dir_x = -1;
                    dir_y = -1;  //下方向に８の字型を書く種類
                }
                break;
            case EnemyType.stealth:
                move_speed_x = 1.0f;
                move_speed_y = 0.3f;
                enemy_hp = 2f;
                break;
            default:
                Debug.Log("エラー");
                break;
        }
    }

    //横方向の往復がある種類のための関数
    void RoopMove()
    {
        //種類によってはここの処理は行わない
        if(state == EnemyType.drunk || state == EnemyType.infini) return;
        //横方向移動の管理（往復移動させる）
        if(transform.position.x >= rightwall.position.x)
        {
            direction = -1;
            this.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if(transform.position.x <= leftwall.position.x)
        {
            direction = 1;
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    //敵の移動種類
    void EnemyMoveType()
    {
        switch(state)
        {
            case EnemyType.normal:
                //y方向は上下に往復移動させる
                transform.position = new Vector2(transform.position.x + move_speed_x * Time.fixedDeltaTime * direction
                                                ,(Mathf.Sin(Time.time) * move_speed_y) + y_pos);
                break;
            case EnemyType.fast:
                //y方向には動かない
                transform.position = new Vector2(transform.position.x + move_speed_x * Time.fixedDeltaTime * direction
                                                ,y_pos);
                break;
            case EnemyType.height:
                //y方向は上下に往復移動させる
                transform.position = new Vector2(transform.position.x + move_speed_x * Time.fixedDeltaTime * direction
                                                ,(Mathf.Sin(Time.time) * move_speed_y) + y_pos);
                break;
            case EnemyType.drunk:
                float ran_x = Random.Range(-10f, 10f);
                float ran_y = Random.Range(-10f, 10f);
                //y方向は上下に往復移動させる
                transform.position = new Vector2(transform.position.x + move_speed_x * Time.fixedDeltaTime * ran_x
                                                ,transform.position.y + move_speed_y * Time.fixedDeltaTime * ran_y);
                //範囲外に出たなら内側に引き戻す
                if(transform.position.x + 0.8f > rightwall.position.x) transform.position = new Vector2(transform.position.x - 1, transform.position.y);
                if(transform.position.x - 0.8f < leftwall.position.x) transform.position = new Vector2(transform.position.x + 1, transform.position.y);
                if(transform.position.y + 0.5f > leftwall.position.y) transform.position = new Vector2(transform.position.x, transform.position.y - 0.5f);
                if(transform.position.y - 0.5f < rightwall.position.y) transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
                //画像の方向転換
                if(ran_x > 0) this.GetComponent<SpriteRenderer>().flipX = true;
                else this.GetComponent<SpriteRenderer>().flipX = false;
                break;
            case EnemyType.infini:
                //y方向は上下に往復移動させる
                transform.position = new Vector2(transform.position.x + move_speed_x * Time.fixedDeltaTime * dir_x
                                                ,transform.position.y + move_speed_y * Time.fixedDeltaTime * dir_y);
                Infini_Dir();
                break;
            case EnemyType.stealth:
                //y方向は上下に往復移動させる
                transform.position = new Vector2(transform.position.x + move_speed_x * Time.fixedDeltaTime * direction
                                                ,(Mathf.Sin(Time.time) * move_speed_y) + y_pos);
                break;
            default:
                Debug.Log("エラー");
                break;
        }
    }

    //８の字型に動く方向を決める
    void Infini_Dir()
    {
        if(dir_y == 1)  //上に進む種類
        {
            //範囲の一番上に来たら、一番下に移動させる
            if(transform.position.y > leftwall.position.y)
            {
                if(transform.position.x > 0){   //右の方にいたら
                    transform.position = new Vector2(rightwall.position.x - 1.5f, rightwall.position.y);   //右下に移動
                    dir_x = -1;     //左に動くように変更
                }
                else{
                    transform.position = new Vector2(leftwall.position.x + 1.5f, rightwall.position.y);   //左下に移動
                    dir_x = 1;      //右に動くように変更
                }
            }
        }
        else if(dir_y == -1)    //下に進む種類
        {
            //範囲の一番下に来たら、一番上に移動させる
            if(transform.position.y < rightwall.position.y)
            {
                if(transform.position.x > 0){   //右の方にいたら
                    transform.position = new Vector2(rightwall.position.x - 1.5f, leftwall.position.y);    //右上に移動
                    dir_x = -1;     //左に動くように変更
                }
                else{
                    transform.position = new Vector2(leftwall.position.x + 1.5f, leftwall.position.y);    //左上に移動
                    dir_x = 1;      //右に動くように変更
                }
            }
        }
        if(dir_x > 0) this.GetComponent<SpriteRenderer>().flipX = true;
        else this.GetComponent<SpriteRenderer>().flipX = false;
    }

    //透明にするかどうかを決める
    void StealthColor()
    {
        if(state == EnemyType.stealth && !isDeath){
            if(Hiting == false)
            {
                this.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            }
            else
            {
                this.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
            }
        }
        else
        {
            this.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        }
    }

    //画像の進行方向を合わせるスクリプト
    void ImageDir()
    {
        //現在の位置が真ん中より左寄りだったら反転、右寄りならそのまま
        if(transform.position.x < 0) this.GetComponent<SpriteRenderer>().flipX = true;
        else this.GetComponent<SpriteRenderer>().flipX = false;
    }
}
