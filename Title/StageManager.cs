using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;        //TMPをつかうとき必要
//ステージ選択画面を管理するスクリプト
public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    public GameObject StagePanel;       //ステージ選択画面に表示されるPanel

    public TextMeshProUGUI stage_text;     //ステージ選択画面のステージ番号のテキスト

    public int now_Stage;      //ステージ番号(ステージ数は７個の予定)

    private int stage_quota;                        //ステージの合計ノルマ数
    private int[] stage_type_quota = new int[6];    //種類ごとの出現数

    public bool[] stage_clear;          //各ステージのクリア判定

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
        now_Stage = 1;
        stage_text.text = "第"+ now_Stage.ToString() +"夜";     //はじめは「第１夜」で固定

    }

    //ステージ選択ボタンの右側のボタンを押したときに実行(ボタン実行のためpublic)
    public void Stage_Up()
    {
        if(now_Stage >= 1 && now_Stage < ClearStageNumber())   //最大ステージ数７より小さい値ならば
        {
            now_Stage++;    //ステージ番号を１加算する
            stage_text.text = "第"+ now_Stage.ToString() +"夜";     //それをテキストに反映
            SoundManager.Instance.PlaySE(7);
        }
        else
        {
            //拒否するSEを鳴らすなりしておく
            SoundManager.Instance.PlaySE(6);
        }
    }

    //ステージ選択ボタンの左側のボタンを押したときに実行(ボタン実行のためpublic)
    public void Stage_Down()
    {
        if(now_Stage > 1)     //ステージ数１より大きければ
        {
            now_Stage--;    //ステージ番号を１減らす
            stage_text.text = "第"+ now_Stage.ToString() +"夜";     //それをテキストに反映
            SoundManager.Instance.PlaySE(7);
        }
        else
        {
            //拒否するSEを鳴らすなりしておく
            SoundManager.Instance.PlaySE(6);
        }
    }

    //「眠りハジメル」ボタンを押したときに実行
    //現在のnow_Stageのステージを始める
    public void Start_Stage()
    {
        GameManagement.Instance.now_Event = true;   //イベントが開始されるためTrueにする
        GameSetting();                              //各ステージの処理を行う関数を実行
    }

    //各ステージの設定を決める
    //-----必要な情報-----
    //ノルマ数（種類ごとのノルマ数も含む）
    //
    void GameSetting()
    {
        StagePanel.SetActive(false);        //ステージ選択画面UIを非表示
        int stage_quota = 0;                //1ゲームの撃破ノルマ数
        int[] stage_type_quota = new int[6] {0,0,0,0,0,0};;      //各種類を何体出現させるか
        //ステージによる差分
        switch(now_Stage)
        {
            case 1:
                stage_quota = 6;             //全体ノルマ数：６体
                stage_type_quota = new int[6] {6,0,0,0,0,0};        //通常種を６体出現
                EventManager.Instance.PlayEvent(1);     //Timelineを実行
                //GetComponent<EndroalScript>().EndroalStart();
                break;
            case 2:
                stage_quota = 8;             //全体ノルマ数：８体
                stage_type_quota = new int[6] {4,4,0,0,0,0};        //通常種を６体出現
                EventManager.Instance.PlayEvent(3);     //Timelineを実行
                break;
            case 3:
                stage_quota = 10;             //全体ノルマ数：１０体
                stage_type_quota = new int[6] {2,4,4,0,0,0};        //通常種を６体出現
                EventManager.Instance.PlayEvent(5);     //Timelineを実行
                break;
            case 4:
                stage_quota = 12;             //全体ノルマ数：１２体
                stage_type_quota = new int[6] {2,3,3,4,0,0};        //通常種を６体出現
                EventManager.Instance.PlayEvent(7);     //Timelineを実行
                break;
            case 5:
                stage_quota = 13;             //全体ノルマ数：１３体
                stage_type_quota = new int[6] {2,2,2,2,5,0};        //通常種を６体出現
                EventManager.Instance.PlayEvent(9);     //Timelineを実行
                break;
            case 6:
                stage_quota = 10;             //全体ノルマ数：１０体
                stage_type_quota = new int[6] {0,0,0,0,0,10};        //通常種を６体出現
                EventManager.Instance.PlayEvent(11);     //Timelineを実行
                break;
            case 7:
                //ボス戦のため他の敵は出現しない
                EventManager.Instance.PlayEvent(13);     //Timelineを実行
                break;
            default:
                break;
        }
        SoundManager.Instance.FadeOutBGM();
        EnemyManager.Instance.ResetStage(stage_quota, stage_type_quota);     //設定をセット
    }

    //現在選択しているステージをクリアしたとき実行
    //クリア後のTimelineを実行する
    public void StageClear()
    {
        if(now_Stage != 0) stage_clear[now_Stage - 1] = true;
        GetComponent<SaveManager>().SaveStageData(now_Stage);
        switch(now_Stage)
        {
            case 1:
                EventManager.Instance.PlayEvent(2);     //Timelineを実行
                break;
            case 2:
                EventManager.Instance.PlayEvent(4);     //Timelineを実行
                break;
            case 3:
                EventManager.Instance.PlayEvent(6);     //Timelineを実行
                break;
            case 4:
                EventManager.Instance.PlayEvent(8);     //Timelineを実行
                break;
            case 5:
                EventManager.Instance.PlayEvent(10);     //Timelineを実行
                break;
            case 6:
                EventManager.Instance.PlayEvent(12);     //Timelineを実行
                break;
            case 7:
                EventManager.Instance.PlayEvent(14);     //Timelineを実行
                break;
            case 0:         //ボーナスステージをクリアした時
                EventManager.Instance.PlayEvent(18);     //Timelineを実行
                break;
            default:
                break;
        }
        SoundManager.Instance.PlayBGM(2);
    }

    //どこまでクリアしたかを判定する関数
    public int ClearStageNumber()
    {
        for(int i=0; i < 7; i++)
        {
            if(stage_clear[i] == false)
            {
                return i+1;     //iに１加えた値がステージ数
            }
        }
        return 7;       //stage_clearがすべてtrueなら最大ステージ数７を返す
    }

    //タイトル画面からステージ画面に飛んだ時、「第１夜」になるようにする関数
    public void StageReset()
    {
        EnemyManager.Instance.interval = 3f;        //インターバルを調調整
        now_Stage = 1;
        stage_text.text = "第"+ now_Stage.ToString() +"夜";     //はじめは「第１夜」で固定
    }

    //ステージクリア後、次のステージ選択画面になるようにする関数
    public void NextStage()
    {
        EnemyManager.Instance.interval = 3f;    //インターバルを調調整
        now_Stage = ClearStageNumber();
        stage_text.text = "第"+ now_Stage.ToString() +"夜";
    }

    //ロードしたデータからどこまでクリアしているかを受け取る
    public void LoadClearStage(int LoadNum)
    {
        //引数の数字がクリアしたステージ番号であるため
        //その値-1が引数である場所までクリア判定にする
        for(int i=0; i < 7; i++)
        {
            if(i < LoadNum) stage_clear[i] = true;      //クリア判定にする
            else stage_clear[i] = false;                //クリアしてない判定にする
        }
        //最後のステージがクリアされていたら
        //ストーリーモードをクリア判定にする
        if(stage_clear[6]) GameManagement.Instance.clear_judge = true;
        else GameManagement.Instance.clear_judge = false;
    }
}
