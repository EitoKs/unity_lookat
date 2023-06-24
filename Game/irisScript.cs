using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//虹彩の動きを管理するスクリプト
public class irisScript : MonoBehaviour
{
    public GameObject target;   //クリックしている場所
    public GameObject eye;      //目のオブジェクト
    private GameObject Lay;     //光線オブジェクト
    private float speed;        //虹彩の移動スピード

    private Vector2 eyePos;     //目の中心位置
    private Vector2 irisPos;    //虹彩の相対位置
    private Vector2 lookVec;    //目の中心から見ている方向のベクトル

    private float rotate_angle; //x軸からの回転角度


    void Start()
    {
        speed = 0.1f;   //移動スピードを決める
        eyePos = eye.transform.position;    //目の中心の座標を取得
        Lay = transform.GetChild(0).gameObject;     //光線である子オブジェクトを取得
    }

    void Update()
    {
        if(!GameManagement.Instance.now_Gameing) return;     //ゲーム中でなければ動かせない
        if(GameManagement.Instance.now_Pause) return;       //ポーズ画面を表示中なら動かせない
        LayVisible();
        EyeMove();
        EyeRotate();
    }

    //虹彩を移動させる関数
    public void EyeMove()
    {
        //目の中心からクリック位置までのベクトルを求める
        lookVec = (Vector2)target.transform.position - eyePos;
        //求めたベクトルを正規化し、さらに半分以下にする
        irisPos = (lookVec.normalized)/2.4f;
        //Debug.Log(irisPos);
        //現在の虹彩の位置から求めたベクトルまでの場所まで移動
        transform.localPosition = Vector2.MoveTowards(transform.localPosition, irisPos, speed);
    }

    //虹彩を回転させて、ビームの位置を調整する関数
    public void EyeRotate()
    {
        //ｘ軸と見ている方向のベクトルのなす角度を求める
        //そしてラジアンから度に変換
        rotate_angle = Mathf.Atan2(lookVec.y, lookVec.x) * Mathf.Rad2Deg;
        //Debug.Log(rotate_angle);
        //ローカル座標を基準に回転を取得
        Vector3 localAngle = transform.localEulerAngles;
        //ｚ軸周りの回転を前に求めた角度を代入
        localAngle.z = rotate_angle;
        //変更した値を現在の角度に反映させる
        transform.localEulerAngles = localAngle;
    }
    
    //光線の表示・非表示を管理する関数
    void LayVisible()
    {
        //真ん中に虹彩があるときビームを消す
        if(transform.localPosition == new Vector3(0f, 0f, 0f))
        {
            Lay.SetActive(false);
        }
        else{
            Lay.SetActive(true);
        }
    }
}
