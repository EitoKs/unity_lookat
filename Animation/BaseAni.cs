using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ステージのアニメーターを管理するスクリプト
public class BaseAni : MonoBehaviour
{
    //ステージのベースのアニメーター
    [SerializeField]
    private Animator baseAnimator;

    //間接照明のアニメーター
    [SerializeField]
    private Animator lightAnimator;

    //髪の毛のアニメーター
    [SerializeField]
    private Animator Hair1Animator;
    [SerializeField]
    private Animator Hair2Animator;
    [SerializeField]
    private Animator Hair3Animator;
    [SerializeField]
    private Animator Hair4Animator;

    [SerializeField]
    private GameObject BlackPanel;

    void Start()
    {
        SleepState();
    }

    public void BaseMove(bool on)   //trueで寝る
    {
        baseAnimator.SetBool("Base", on);
    }

    public void LightMove(bool on)  //trueでライトが付く
    {
        SoundManager.Instance.PlaySE(0);
        lightAnimator.SetBool("Light", on);
    }

    //状態：１～３
    public void Hair1Move(int i)
    {
        Hair1Animator.SetInteger("hair1", i);
    }

    //状態：１～３
    public void Hair2Move(int i)
    {
        Hair2Animator.SetInteger("hair2", i);
    }

    //状態：１～２
    public void Hair3Move(int i)
    {
        Hair3Animator.SetInteger("hair3", i);
    }

    //状態：１～２
    public void Hair4Move(int i)
    {
        Hair4Animator.SetInteger("hair4", i);
    }

    //眠ている状態
    public void SleepState()
    {
        //ゲームを起動したときの初期状態
        BaseMove(true);
        LightMove(false);
        Hair1Move(3);
        Hair2Move(3);
        Hair3Move(3);
        Hair4Move(3);
        BlackPanel.SetActive(true);
    }

    //起きている状態
    public void AwakeState()
    {
        BaseMove(false);
        Hair1Move(1);
        Hair2Move(1);
        Hair3Move(1);
        Hair4Move(1);
    }

    //---------------------------------------------
    //電気をつける処理
    public void LightOn()
    {
        StartCoroutine("Lighton");
    }

    IEnumerator Lighton()
    {
        Hair4Move(2);       //蛇をライトまで移動させる
        yield return new WaitForSeconds(1);     //１秒停止
        LightMove(true);        //ライトをつけた状態にする
        yield return new WaitForSeconds(0.5f);     //１秒停止
        BlackPanel.SetActive(false);    ///暗いのを取る
        yield return new WaitForSeconds(1);     //１秒停止
        Hair4Move(1); 
    }

    //電気を消す処理
    public void LightOff()
    {
        StartCoroutine("Lightoff");
    }

    IEnumerator Lightoff()
    {
        Hair4Move(2);       //蛇をライトまで移動させる
        yield return new WaitForSeconds(1);     //１秒停止
        LightMove(false);        //ライトを消した状態にする
        yield return new WaitForSeconds(0.5f);     //１秒停止
        BlackPanel.SetActive(true);    ///暗くする
        yield return new WaitForSeconds(1);     //１秒停止
        Hair4Move(1); 
    }
}
