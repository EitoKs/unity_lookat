using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//フェードアウト・インを行うためのスクリプト
public class FadeScript : MonoBehaviour
{
    public static FadeScript Instance;

    [SerializeField]
    private GameObject FadePanel;       //フェード操作を行うパネル

    private Animator FadeAnimator;      //パネルのアニメーター

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        FadeAnimator = FadePanel.GetComponent<Animator>();  //取得
        //FadeAnimator.keepAnimatorControllerStateOnDisable = true;
        FadePanel.SetActive(true);
    }

    //徐々に暗くなる処理（フェードアウト）
    public void FadeOut()
    {
        Debug.Log("通ったよ");
        FadePanel.SetActive(true);      //表示する
        FadeAnimator.SetBool("Fadeout", true);      //フェードアウトをできるようにする
    }

    //徐々に明るくなる処理（フェードイン）
    public void FadeIn()
    {
        FadeAnimator.SetBool("Fadeout", false);     //フェードアウトの処理をできなくする
        //FadePanel.SetActive(true);      //表示する
        FadeAnimator.SetBool("Fadein", true);      //フェードアウトをできるようにする
        StartCoroutine("AfterFadeIn");
    }
    
    //フェードインの後に行う処理
    IEnumerator AfterFadeIn()
    {
        yield return new WaitForSeconds(2f);   //1秒止める
        FadeAnimator.SetBool("Fadein", false);
        FadePanel.SetActive(false);      //非表示にする
    }
}
