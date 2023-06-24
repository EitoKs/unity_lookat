using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;        //TMPをつかうとき必要
//操作ガイドを表示するためのスクリプト
//操作ガイドの文章もインスペクタ画面で変えられるようにする
public class TutorialScript : MonoBehaviour
{
    [SerializeField]
    private GameObject TutorialPanel;       //チュートリアル画面
    [SerializeField]
    private TextMeshProUGUI TutorialText;    //チュートリアル画面のテキスト
    [SerializeField]
    private Image TutorialImage;

    public string[] tutorial_text;    //チュートリアル画面に表示される文章(インスペクタ画面で設定)
    private bool _isActive;           //画面を開いているかどうかを判定

    void Start()
    {
        _isActive = false;
        TutorialPanel.SetActive(false);
    }

    //ステージ番号を引数にする
    public void SetTutorial()
    {
        int stage = StageManager.Instance.now_Stage;
        if(stage == 0) return;                          //ボーナスステージなら処理をしない
        if(tutorial_text[stage-1] == null) return;      //ステージのチュートリアルがなければ処理をしない

        TutorialText.text = tutorial_text[stage - 1];   //各ステージごとのチュートリアル文章を設定
    }

    //チュートリアル画面の表示・非表示を切り替える関数
    public void TutorialActive()
    {
        if(_isActive)       //画面を開いていたら
        {
            _isActive = false;
            TutorialPanel.SetActive(false);
            EventManager.Instance.restart();    //Timelineを再開
        }
        else                //開いていなければ
        {
            SoundManager.Instance.PlaySE(4);
            _isActive = true;
            TutorialPanel.SetActive(true);
            EventManager.Instance.pause();      //Timelineを止める
        }
    }
}
