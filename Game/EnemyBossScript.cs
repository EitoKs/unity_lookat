using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;        //TMPを使ってHP表示するため追加
using UnityEngine.UI;
//ボスの行動、HPを管理するスクリプト
public class EnemyBossScript : MonoBehaviour
{
    private float enemy_hp;     //敵のHP
    Rigidbody2D enemy_rb2d;     //敵のリジッドボディ

    private float move_speed_x;
    private float move_speed_y;

    private int direction = 1;   //動く方向
    private int dir_x;
    private int dir_y;

    public bool Hiting;       //当たっているかの判定  
    private bool isDeath;     //死んでいるかの判定

    private float y_pos;                //y座標の初期値

    private Animator enemy_animator;    //敵のアニメーター

    //移動範囲を示すオブジェクトを格納
    private Transform rightwall;
    private Transform leftwall;

    [SerializeField]
    private GameObject HpCanvas;        //Hpを表示するCanvas
    [SerializeField]
    private TextMeshProUGUI hpText;     //現在のHPを表示するテキスト

    public EnemyType state;
    //敵の種類
    public enum EnemyType
    {
        normal,     //通常種
        fast,       //縦の移動はないが素早い・HP低
        height,     //縦の動きが大きい
        infini,     //8の字で動き回る・HP低
        stealth     //姿が消える・HP低
    }

    void Start()
    {
        enemy_hp = 30f;
        isDeath = false;
        y_pos = transform.position.y;   //y座標の初期値を記録
        enemy_rb2d = GetComponent<Rigidbody2D>();
        HpCanvas.SetActive(false);
        //シーン上のオブジェクトを探し、そのTransform型で受け取る
        rightwall = GameObject.Find("RightWall").transform;
        leftwall = GameObject.Find("LeftWall").transform;
        enemy_animator = GetComponent<Animator>();
        EnemyTypeJudge();
        ImageDir();     //はじめの進行方向を決めておく
        enemy_animator.applyRootMotion = false;
        enemy_animator.SetTrigger("boss_tojo");
        hpText.text = enemy_hp.ToString("n2");
    }

    void Update()
    {
        if(!GameManagement.Instance.now_Gameing) return;    //ゲーム中でなければ処理をしない
        HPShow();       //HPの表示判定
        EnemyDes();     //敵の消滅判定
        StealthColor(); //ステルスの種類なら姿の出現判定を入れる
    }

    //0.02秒毎に呼び出される
    void FixedUpdate()
    {
        if(isDeath) return;     //倒した奴は動きを止める
        if(!GameManagement.Instance.now_Gameing) return;    //ゲーム中でなければ処理をしない

        PositionChange();

        EnemyMoveType();
    }

    //HPを減らす関数（光線に当たっている時に実行）
    public void HPDown()
    {
        enemy_hp -= 0.01f;
    }

    public void HPShow()
    {
        if(Hiting && !isDeath && enemy_hp > 0){     //光線が当たっているかつ死んでいない時
            HpCanvas.SetActive(true);
            hpText.text = enemy_hp.ToString("n2");
        }
        else        //当たっていないとき
        {
            HpCanvas.SetActive(false);
        }
    }

    void EnemyDes()
    {
        //HPが０になったとき敵オブジェクトを消す(３秒後)
        if(enemy_hp <= 0f && isDeath == false)
        {
            isDeath = true;
            GameManagement.Instance.GameClearPro();         //ゲームをクリアした後の処理
            StageManager.Instance.StageClear();             //遊んだステージをクリアした判定にする
        }
    }
    //やられたときのアニメーションと削除処理
    public void DesProcess()
    {
        isDeath = true;
        enemy_animator.SetTrigger("enemy_boss_des");     //消えるアニメーションを実行し消す
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
                break;
            case EnemyType.fast:
                move_speed_x = 1.5f;
                move_speed_y = 0f;
                break;
            case EnemyType.height:
                move_speed_x = 1f;
                move_speed_y = 2f;
                break;
            case EnemyType.infini:
                move_speed_x = 1f;
                move_speed_y = 1f;
                break;
            case EnemyType.stealth:
                move_speed_x = 1.0f;
                move_speed_y = 0.3f;
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
        if(state == EnemyType.infini) 
        {
            return;
        }
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
                    PositionRight();
                }
                else{
                    transform.position = new Vector2(leftwall.position.x + 1.5f, rightwall.position.y);   //左下に移動
                    dir_x = 1;      //右に動くように変更
                    PositionLeft();
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
                    PositionRight();
                }
                else{
                    transform.position = new Vector2(leftwall.position.x + 1.5f, leftwall.position.y);    //左上に移動
                    dir_x = 1;      //右に動くように変更
                    PositionLeft();
                }
            }
        }
        if(dir_x > 0) this.GetComponent<SpriteRenderer>().flipX = true;
        else this.GetComponent<SpriteRenderer>().flipX = false;
    }

    //透明にするかどうかを決める
    void StealthColor()
    {
        if(state == EnemyType.stealth){
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

    //敵のタイプをランダムで変える関数
    void EnemyTypeChange()
    {
        int ran_num = Random.Range(1, 500) % 5;     //0~4のランダムな値を取得
        //ランダムな値によってタイプを変える
        switch(ran_num)
        {
            case 0:
                state = EnemyType.normal;
                break;
            case 1:
                state = EnemyType.fast;
                break;
            case 2:
                state = EnemyType.height;
                break;
            case 3:
                state = EnemyType.infini;
                break;
            case 4:
                state = EnemyType.stealth;
                break;
            default:
                break;
        }
    }
    //画面の外に出たとき位置を調整する関数
    void PositionChange()
    {
        //右の方に見切れた時
        if(transform.position.x  >= rightwall.position.x)
        {
            PositionRight();
        }
        //左の方に見切れた時
        else if(transform.position.x <= leftwall.position.x)
        {
            PositionLeft();
        }
    }
    //ｙ座標を３パターンで変える
    void YPositionChange()
    {
        int ran_num = Random.Range(1, 100) % 3;     //0~2のランダムな値を取得
        if(ran_num == 0) y_pos = 0f;
        else if(ran_num == 1) y_pos = 3f;
        else if(ran_num == 2) y_pos = -3f;
    }
    //右に見切れた時の処理
    void PositionRight()
    {
        EnemyTypeChange();
        EnemyTypeJudge();
        YPositionChange();
        switch(state)
        {
            case EnemyType.normal:
                transform.position = new Vector2(rightwall.position.x - 0.5f , y_pos);  //位置を調整
                direction = -1;     //左向きにする
                break;
            case EnemyType.fast:
                transform.position = new Vector2(rightwall.position.x - 0.5f , y_pos);  //位置を調整
                direction = -1;     //左向きにする
                break;
            case EnemyType.height:
                transform.position = new Vector2(rightwall.position.x - 0.5f , y_pos);  //位置を調整
                direction = -1; 
                break;
            case EnemyType.infini:
                transform.position = new Vector2(rightwall.position.x - 1.5f, leftwall.position.y); //初期位置を固定
                dir_x = -1;
                dir_y = -1;  //下方向に８の字型を書く種類
                break;
            case EnemyType.stealth:
                transform.position = new Vector2(rightwall.position.x - 0.5f , y_pos);  //位置を調整
                direction = -1; 
                break;
            default:
                break;
        }
        this.GetComponent<SpriteRenderer>().flipX = false;
    }
    //左に見切れた時の処理
    void PositionLeft()
    {
        EnemyTypeChange();
        EnemyTypeJudge();
        YPositionChange();
        switch(state)
        {
            case EnemyType.normal:
                transform.position = new Vector2(leftwall.position.x + 0.5f , y_pos);  //位置を調整
                direction = 1;     //左向きにする
                break;
            case EnemyType.fast:
                transform.position = new Vector2(leftwall.position.x + 0.5f , y_pos);  //位置を調整
                direction = 1;     //左向きにする
                break;
            case EnemyType.height:
                transform.position = new Vector2(leftwall.position.x + 0.5f , y_pos);  //位置を調整
                direction = 1; 
                break;
            case EnemyType.infini:
                transform.position = new Vector2(leftwall.position.x + 1.5f, rightwall.position.y); //初期位置を固定
                dir_x = 1;
                dir_y = 1;  //上方向に８の字型を書く種類
                break;
            case EnemyType.stealth:
                transform.position = new Vector2(leftwall.position.x + 0.5f , y_pos);  //位置を調整
                direction = 1; 
                break;
            default:
                break;
        }
        this.GetComponent<SpriteRenderer>().flipX = true;
    }

    public void ApplyRootOn()
    {
        enemy_animator.applyRootMotion = true;
    }
}
