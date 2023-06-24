using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//目の状態を管理するスクリプト
public class EyeAni : MonoBehaviour
{
    [SerializeField]
    private Animator EyeAnimator;  //左目のアニメーター

    [SerializeField]
    private GameObject iris;       //虹彩

    [SerializeField]
    private GameObject beem;        //光線


    public enum EyeState{
        open,   //ぱっちり開けている
        half,   //半分だけ開けている
        close,   //閉じている
        beemon,  //光線を出す
        beemoff  //光線を消す
    }
    EyeState state;

    void Start()
    {
        state = EyeState.close;     //はじめは目を閉じている
        EyeStatePro((int)state);
    }

    public void EyeMove(int i)
    {
        EyeAnimator.SetInteger("eye", i);
    }

    //状態によっての処理(引数に状態を入れる)
    public void EyeStatePro(int _state)
    {
        switch(_state)
        {
            case (int)EyeState.open:
                iris.transform.localPosition = new Vector3(0f, 0f, 0f);
                iris.SetActive(true);
                beem.SetActive(false);      //光線は非表示にしておく
                EyeMove(1);
                break;
            case (int)EyeState.half:
                iris.transform.localPosition = new Vector3(0f, -0.2f, 0f);
                iris.SetActive(true);
                beem.SetActive(false);      //光線は非表示にしておく
                EyeMove(2);
                break;
            case (int)EyeState.close:
                iris.SetActive(false);
                EyeMove(3);
                break;
            case (int)EyeState.beemon:
                EyeMove(1);         //目を開けた状態にする
                iris.transform.localPosition = new Vector3(-0.4f, 0.4f, 0f);
                Vector3 localAngle = iris.transform.localEulerAngles;
                localAngle.z = 135;
                iris.transform.localEulerAngles = localAngle;
                beem.SetActive(true);      //光線は表示しておく
                break;
            case (int)EyeState.beemoff:
                beem.SetActive(false);      //光線は非表示にしておく
                EyePosReset();      //位置をリセット
                break;
            default:
                break;
        }
    }

    //目の位置を基準の位置に戻す処理
    public void EyePosReset()
    {
        iris.transform.localPosition = new Vector3(0f, 0f, 0f);     //位置を中心に戻す
        //回転を０にする
        Vector3 localAngle = iris.transform.localEulerAngles;
        localAngle.x = 0;
        localAngle.y = 0;
        localAngle.z = 0;
        iris.transform.localEulerAngles = localAngle;
    }
}
