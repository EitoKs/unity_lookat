using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;        //TMPをつかうとき必要

public class MuteButton : MonoBehaviour
{
    //ミュートボタンのON/OFFによる文字と色の違いを切り替える
    public void MuteButtonChange(bool isMute)
    {
        //ミュートされていたら
        if(isMute){
            this.GetComponent<Button>().image.color = new Color32(60, 114, 178, 255);
            this.GetComponentInChildren<TextMeshProUGUI> ().text = "OFF";
        }
        else
        {
            this.GetComponent<Button>().image.color = new Color32(192, 74, 74, 255);
            this.GetComponentInChildren<TextMeshProUGUI> ().text = "ON";
        }
    }
}
