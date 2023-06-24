using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

//ここでイベントを実行させる
public class EventManager : MonoBehaviour
{
    public static EventManager Instance;    //全体のイベントを管理するためインスタンス化しておく

    public PlayableDirector NowDirector;

    //ここにインスペクター上であらかじめ複数セットしておく
    public PlayableDirector[] director;

    public bool[] eventFlags;   //イベント確認判定（デバッグ用）

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

//--------------------------------------------------------------------------------------------
    void Start()
    {
        NowDirector = director[0];      //オープニングイベントをセット
        GameManagement.Instance.now_Event = true;   //現在イベント中であることにする
        NowDirector.Play();
    }

    //num番目に登録したTimelineを実行する関数
    public void PlayEvent(int num)
    {
        if(director[num] == null) return;   //目的のTimelineが登録されていなければ以下の処理は行わない

        NowDirector = director[num];        //実行したいTimelineをセットする
        eventFlags[num] = true;             //Timelineを実行した判定にする
        GameObject.Find("TutorialCanvas").GetComponent<TutorialScript>().SetTutorial();     //チュートリアル文章をセットする
        GameManagement.Instance.now_Event = true;   //現在イベント中であることにする
        NowDirector.Play();                 //Timelineを実行する
    }

    //実行中のタイムラインを一時停止させる関数
    public void pause(){
        NowDirector.Pause();
    }
    //一時停止中のタイムラインを再開させる関数
    public void restart(){
        NowDirector.Resume();
    }
    //タイムラインを停止させる関数
    public void stoptimeline(){
        NowDirector.Stop();
    }
    //Timelineのイベントが終わったことをシグナルで伝える
    public void ExitEvent()
    {
        if(GameManagement.Instance.now_Event != true) return;   //イベント中で無ければ以下の処理をしない
        GameManagement.Instance.now_Event = false;              //現在イベント中でないことにする
    }
}
