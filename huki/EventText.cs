using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//会話イベントの内容をインスペクター画面から入力し、実行するスクリプト
//Timelineのインスタンスにくっつける
public class EventText : MonoBehaviour
{
    public bool isSearch;   //すでに１度、見たかどうかを判定

    public int Talk_Num;        //会話イベントの数（１つのTimelineで複数回メッセージウィンドウが消えるとき）
    private int now_talknum;   //現在の会話イベントの番号

    //インスペクタ上で指定できるようクラスを作成
    //配列をもつクラスを作り、そのクラスを配列にすることで多重配列を作る
    [System.Serializable]
    public class TextArrayClass
    {
        //string、int型のメンバを用意
        public string[] text_string;        //表示させる文章の配列
        public int[] text_name;             //誰が話しているかを判別用の配列

        //コンストラクタ（呼び出されたときに実行）
        public TextArrayClass(string[] text_string, int[] text_name)
        {
            //自分のクラスのメンバに引数の値を代入(this.の方がメンバ)
            this.text_string = text_string;
            this.text_name = text_name;
        }
    }

    [SerializeField]
    private TextArrayClass[] textArrayClasses;      //上で作成したクラスの配列を作成

    void Start()
    {
        ReSetTextEvent();
    }

    //イベントで使用するテキストを表示させる関数
    public void Show_Text(){
        if(now_talknum >= Talk_Num)     //現在の会話イベントが最大値に達していたら
        {
            isSearch = true;    //調べた判定にする
            return;
        }

        NovelScript.Instance.NovelUIOpen(textArrayClasses[now_talknum].text_string, textArrayClasses[now_talknum].text_name);     //文章を表示
        now_talknum++;
    }

    //会話イベントを初期化する関数
    public void ReSetTextEvent()
    {
        now_talknum = 0;        //はじめは０にする
    }
}
