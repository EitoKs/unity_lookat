using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//各アニメーションの状態を一つにまとめてここで操作を行えるようにする
public class AniState : MonoBehaviour
{
    public static AniState Instance;

    [SerializeField] GameObject LeftEye;
    [SerializeField] GameObject RightEye;
    [SerializeField] GameObject BasePanel;
    [SerializeField] GameObject FacePanel;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    //完全に寝ている状態(タイトル画面、ゲーム開始前・後の状態)
    public void SleepState()
    {
        BasePanel.GetComponent<BaseAni>().SleepState();     //髪とかが寝ている
        if(!GameManagement.Instance.clear_judge)FacePanel.GetComponent<FaceAni>().SleepState1();    //クリア前の顔の状態
        else FacePanel.GetComponent<FaceAni>().SleepState2();    //クリア後の顔の状態
        LeftEye.GetComponent<EyeAni>().EyeStatePro(4);      //目のビームを消す
        RightEye.GetComponent<EyeAni>().EyeStatePro(4);     //目のビームを消す
        LeftEye.GetComponent<EyeAni>().EyeStatePro(2);      //目を閉じる
        RightEye.GetComponent<EyeAni>().EyeStatePro(2);     //目を閉じる
    }
}
