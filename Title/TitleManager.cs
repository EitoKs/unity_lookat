using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;        //TMPをつかうとき必要
//タイトル画面に関する処理をまとめたスクリプト
public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject TitleCanvas;     //タイトル画面に表示されるCanvas
    
    [SerializeField]
    private GameObject TitlePanel;      //タイトルに表示されるPanel
    [SerializeField]
    private GameObject StagePanel;      //ステージ選択画面に表示されるPanel
    [SerializeField]
    private GameObject OptionPanel;      //音量調節のPanel

    [SerializeField]
    private GameObject BonusPanel;      //ボーナスモードの警告パネル
    [SerializeField]
    private TextMeshProUGUI BonusText;

    [SerializeField]
    private GameObject ExitPanel;      //終了選択パネル

    [SerializeField]
    private GameObject BonusPanel2;     //ボーナスモードをするかしないかをきめるパネル

    void Start()
    {
        //if(GameManagement.Instance.clear_judge) BonusText.text = "大量モード";
        //else BonusText.text = "？？？";
    }

    //タイトル画面で「はじめる」ボタンを押したときの処理
    public void StartButtonClick()
    {
        SoundManager.Instance.PlaySE(8);
        TitlePanel.SetActive(false);
        StageManager.Instance.StageReset();
        StagePanel.SetActive(true);
    }

    //ステージクリア後、選択画面に戻るとき
    public void NextStageSelect()
    {
        SoundManager.Instance.PlayBGM(0);
        StageManager.Instance.NextStage();
        StagePanel.SetActive(true);
    }

    //ステージ選択画面で「タイトルへ」ボタンを押したときの処理
    public void BackToTitle()
    {
        SoundManager.Instance.PlaySE(9);
        StagePanel.SetActive(false);
        StartCoroutine("ResetTitleState");
    }

    //タイトル画面で「オプション」ボタンを押したときの処理
    public void OptionButtonClick()
    {
        SoundManager.Instance.PlaySE(8);
        TitlePanel.SetActive(false);
        OptionPanel.SetActive(true);
    }

    //音量調節画面で「タイトルへ」ボタンを押したときの処理
    public void OptionToTitle()
    {
        SoundManager.Instance.PlaySE(9);
        OptionPanel.SetActive(false);
        TitlePanel.SetActive(true);
    }

    //クリア後、タイトル画面に戻るとき
    public void AfterClearTitle()
    {
        BonusText.text = "大量モード";
        GameManagement.Instance.clear_judge = true;     //ストーリーモードクリア判定にする
        GameObject.Find("StageCanvas/FacePanel").GetComponent<FaceAni>().SleepState2();
        TitlePanel.SetActive(true);
        OptionPanel.SetActive(false);
        StagePanel.SetActive(false);
    }

    //タイトル画面でボーナスボタンを押したときの処理
    public void TitleToBonus()
    {
        if(GameManagement.Instance.clear_judge)     //ストーリーモードをクリアしていれば
        {
            SoundManager.Instance.PlaySE(8);
            BonusPanel2.SetActive(true);
        }
        else    //クリアしていなければパネルを表示
        {
            SoundManager.Instance.PlaySE(8);
            BonusPanel.SetActive(true);
        }
    }

    public void BonusToTitle()
    {
        SoundManager.Instance.PlaySE(9);
        BonusPanel.SetActive(false);
    }

    public void BonusToTitle2()
    {
        SoundManager.Instance.PlaySE(9);
        BonusPanel2.SetActive(false);
    }
    //ボーナスモードを始める処理
    public void StartBonus()
    {
        //表示しているパネル類を消す
        SoundManager.Instance.PlaySE(8);
        TitlePanel.SetActive(false);        //タイトルパネルを消す
        BonusPanel2.SetActive(false);       //ボーナスパネルを消す

        //ここでボーナスステージの設定をする
        EnemyManager.Instance.interval = 0.5f;    //インターバルを調調整
        int stage_quota = 999;                //1ゲームの撃破ノルマ数
        int[] stage_type_quota = new int[6] {166,169,166,166,166,166};      //各種類を何体出現させるか
        EnemyManager.Instance.ResetStage(stage_quota, stage_type_quota);     //設定をセット
        GameManagement.Instance.now_BonusMode = true;
        StageManager.Instance.now_Stage = 0;        //ステージ番号をリセットする
        EventManager.Instance.PlayEvent(16);     //Timelineを実行
    }

    //ゲームを終了させる処理
    public void GameExit()
    {
        Application.Quit();
    }
    //終了ボタンを押したときの処理
    public void TitleToExit()
    {
        SoundManager.Instance.PlaySE(8);
        ExitPanel.SetActive(true);  //パネルを表示
    }
    //やめるボタンを押したときの処理
    public void ClickYes()
    {
        GameExit();
    }
    //やめないボタンを押したときの処理
    public void ClickNo()
    {
        SoundManager.Instance.PlaySE(9);
        ExitPanel.SetActive(false);
    }
    //ポーズ画面からタイトル画面に戻るときの処理
    public void ExitPause()
    {
        GameManagement.Instance.GameExitPro();
        TitlePanel.SetActive(true);        //タイトルパネルを出す
    }

    //クリア状況によって初期状態を変える
    //ステージからタイトル画面の遷移時
    IEnumerator ResetTitleState()
    {
        FadeScript.Instance.FadeOut();      //徐々に暗くする
        yield return new WaitForSeconds(2f);        //２秒待機
        AniState.Instance.SleepState();                 //寝ている状態にする
        FadeScript.Instance.FadeIn();      //徐々に明るくする
        TitlePanel.SetActive(true);
    }

    public void BonusTextName()
    {
        if(GameManagement.Instance.clear_judge) BonusText.text = "大量モード";
        else BonusText.text = "？？？";
    }
}
