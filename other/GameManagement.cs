using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;        //TMPをつかうとき必要
//ゲーム全体を管理するスクリプト
public class GameManagement : MonoBehaviour
{
    public static GameManagement Instance;  //ゲームの進行を管理するためインスタンス化しておく

    public bool now_Title;       //タイトル画面にいるかどうかの判定
    public bool now_Gameing;     //ゲームの開始判定
    public bool now_Result;      //リザルト画面かどうかの判定
    public bool now_Pause;      //ポーズ画面かどうかの判定
    public bool now_Event;      //イベント中かどうかの判定
    public bool now_Endroal;    //エンドロール中かどうかの判定

    public bool clear_judge;    //(ステージをすべて)クリアしたかの判定
    public bool now_BonusMode;  //ボーナスモードをプレイしているかどうか

    [SerializeField]
    private GameObject StartText;   //ゲームスタート時に出るテキスト
    [SerializeField]
    private GameObject ClearText;   //ゲームクリア時に出るテキスト

    [SerializeField]
    private GameObject RemainText;      //残数を表示するテキスト
    private TextMeshProUGUI Remain_text;

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
        now_Title = true;
        now_Gameing = false;
        now_Result = false;
        //clear_judge = false;
        now_Pause = false;
        now_Event = false;
        now_BonusMode = false;
        
        StartText.SetActive(false);
        ClearText.SetActive(false);
        Remain_text = RemainText.GetComponent<TextMeshProUGUI>();
        RemainText.SetActive(false);
        Screen.SetResolution(300, 534, FullScreenMode.Windowed, 60);
    }

    public void GameStart()
    {
        StartCoroutine(TextProcess(StartText));     //「始め」の演出
        now_Gameing = true;
        SoundManager.Instance.PlayBGM(4);
        RemainTextShow(EnemyManager.Instance.remain_num);   //UIを変化
        RemainText.SetActive(true);
        Pause.Instance.PauseButtonShow();       //ポーズボタンを表示
    }

    public void GameClearText()
    {
        StartCoroutine(TextProcess(ClearText));     //「全滅」の演出
        GameClearPro();
    }
    //ボス戦の場合の処理
    public void GameStartBoss()
    {
        StartCoroutine(TextProcess(StartText));     //「始め」の演出
        now_Gameing = true;
        SoundManager.Instance.PlayBGM(4);        //BGMを変える
        Pause.Instance.PauseButtonShow();       //ポーズボタンを表示
    }

    public void GameClearPro()
    {
        now_Gameing = false;    //ゲーム中でないことにする
        RemainText.SetActive(false);
        Pause.Instance.PauseButtonErase();          //ポーズボタンとパネルを非表示
    }
    //中断したときの処理
    public void GameExitPro()
    {
        now_Gameing = false;    //ゲーム中でないことにする
        RemainText.SetActive(false);
    }

    //スタート・クリア時の演出
    IEnumerator TextProcess(GameObject ActionText)
    {
        ActionText.SetActive(true);
        ActionText.GetComponent<Animator>().SetBool("isActive", true);   //アニメーションをOnにする
        SoundManager.Instance.PlaySE(3);
        yield return new WaitForSeconds(2f);                          //1.2秒停止
        ActionText.GetComponent<Animator>().SetBool("isActive", false);   //アニメーションをOnにする
        ActionText.SetActive(false);
    }

    //引数が表示する数
    public void RemainTextShow(int quota_num)
    {
        Remain_text.text = "残り："+ quota_num.ToString() + "匹";
    }
}
