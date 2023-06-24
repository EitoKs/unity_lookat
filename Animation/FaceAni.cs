using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//顔のアニメーションを管理するスクリプト
public class FaceAni : MonoBehaviour
{
    [SerializeField]
    private Animator mouseAnimator;
    [SerializeField]
    private Animator mayuRAnimator;
    [SerializeField]
    private Animator mayuLAnimator;

    void Start()
    {
        if(!GameManagement.Instance.clear_judge) SleepState1();
        else SleepState2();
    }

    public void MouseMove(int i)
    {
        mouseAnimator.SetInteger("mouseState", i);
    }

    public void MayuRMove(int i)
    {
        mayuRAnimator.SetInteger("mayuRState", i);
    }

    public void MayuLMove(int i)
    {
        mayuLAnimator.SetInteger("mayuLState", i);
    }

    //眠っている状態（クリア前）
    public void SleepState1()
    {
        MouseMove(3);
        MayuRMove(1);
        MayuLMove(1);
    }

    //眠っている状態（クリア後）
    public void SleepState2()
    {
        MouseMove(2);
        MayuRMove(1);
        MayuLMove(1);
    }
}
