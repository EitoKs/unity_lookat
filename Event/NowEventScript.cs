using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

//イベントシーンの時操作を受け付けなくさせるスクリプト
public class NowEventScript : MonoBehaviour
{
    public PlayableDirector director;

    void OnEnable()
    {
        director.stopped += OnPlayableDirectorStopped;
    }
    //イベント終了時の処理をここに書く
    void OnPlayableDirectorStopped(PlayableDirector aDirector){
        Debug.Log("Event now stopped.");
        EventManager.Instance.ExitEvent();
    }

    void OnDisable()
    {
        director.stopped -= OnPlayableDirectorStopped;
    }
}
