using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;        //TMPをつかうとき必要
//ポーズ画面のスクリプト
public class Pause : MonoBehaviour
{
    public static Pause Instance;

    [SerializeField]
    private GameObject PausePanel;     //ポーズ画面のパネル
    
    [SerializeField]
    private GameObject ConfirmPanel;   //確認画面のパネル


    [SerializeField]
    private GameObject PauseButton;     //ポーズボタン


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

    //ポーズボタンを押したときの処理
    public void PauseButtonClick()
    {
        SoundManager.Instance.PlaySE(7);
        Time.timeScale = 0;     //FixedUpdate関数の処理を止めることで敵の動きを止める
        GameManagement.Instance.now_Pause = true;
        ConfirmPanel.SetActive(false);  //確認画面を非表示にしておく
        PausePanel.SetActive(true);     //ポーズ画面を表示
    }

    //再開ボタンを押したときの処理
    public void RestartButtonClick()
    {
        SoundManager.Instance.PlaySE(7);
        PausePanel.SetActive(false);     //ポーズ画面を非表示
        GameManagement.Instance.now_Pause = false;
        Time.timeScale = 1f;            //処理を再開
    }

    //中断ボタンを押したときの処理
    public void SuspendButtonClick()
    {
        SoundManager.Instance.PlaySE(5);
        ConfirmPanel.SetActive(true);   //確認画面を表示
    }

    //Yesボタンを押してゲームを中断する処理
    public void YesButtonClick()
    {
        Time.timeScale = 1f;            //処理を再開
        SoundManager.Instance.FadeOutBGM();     //BGMをフェードアウトさせる
        SoundManager.Instance.PlaySE(7);
        if(GameManagement.Instance.now_BonusMode) GameManagement.Instance.now_BonusMode = false;
        PausePanel.SetActive(false);     //ポーズ画面を非表示
        PauseButton.SetActive(false);   //ポーズボタンも非表示
        GameManagement.Instance.now_Pause = false;
        GameManagement.Instance.now_Gameing = false;
        EnemyManager.Instance.AllEnemyDes();    //敵をすべて消す
        StartCoroutine("PauseProcess");
    }

    //Noボタンを押してゲームを中断しない処理
    public void NoButtonClick()
    {
        SoundManager.Instance.PlaySE(6);
        ConfirmPanel.SetActive(false);      //確認画面を非表示
    }
    //ポーズボタンを表示
    public void PauseButtonShow()
    {
        PausePanel.SetActive(false);     //ポーズ画面も非表示
        PauseButton.SetActive(true);
    }
    //ポーズボタンを非表示
    public void PauseButtonErase()
    {
        PausePanel.SetActive(false);     //ポーズ画面も非表示
        PauseButton.SetActive(false);
    }

    IEnumerator PauseProcess()
    {
        FadeScript.Instance.FadeOut();      //徐々に暗くする
        yield return new WaitForSeconds(2f);        //２秒待機
        AniState.Instance.SleepState();                 //寝ている状態にする
        FadeScript.Instance.FadeIn();      //徐々に明るくする
        SoundManager.Instance.PlayBGM(0);       //BGMをタイトル用に差し替える
        this.GetComponent<TitleManager>().ExitPause();  //タイトル画面にとぶ,操作不能にする
    }
}
