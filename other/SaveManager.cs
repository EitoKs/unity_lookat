using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//セーブデータを管理するスクリプト
public class SaveManager : MonoBehaviour
{

    public int Clear_Stage;

    void Start()
    {
        //エンドの回収データがあるか判定
        if(PlayerPrefs.HasKey("StageNum")){
            Clear_Stage = PlayerPrefs.GetInt("StageNum");   //保存していたデータを受け取る
        }else{      //無ければ０で初期化
            Clear_Stage = 0;
            PlayerPrefs.SetInt("StageNum", 0);
            PlayerPrefs.Save();     // 保存
        }
        StageManager.Instance.LoadClearStage(Clear_Stage);  //ステージのクリア状況を知らせる
        GameObject.Find("UIManager").GetComponent<TitleManager>().BonusTextName();
        AniState.Instance.SleepState();
    }

    //クリアステージをセーブする処理
    //引数がクリアしたステージ番号
    public void SaveStageData(int i)
    {
        //引数の値が今のところクリアしているステージ番号より大きければ更新する
        if(i > Clear_Stage)
        {
            Clear_Stage = i;
            PlayerPrefs.SetInt("StageNum", i);
            PlayerPrefs.Save();     // 保存
        }
    }

    //データを全消しする処理
    public void SaveDataReset()
    {
        PlayerPrefs.DeleteKey("StageNum");  //データを消す
    }
}
