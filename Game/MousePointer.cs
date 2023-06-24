using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour
{
    private Vector2 mousePosition;
    private Vector2 lookPosition;
    
    void Update()
    {
        //if(Input.GetMouseButtonDown(0))
        //左クリック長押ししたとき
        if(Input.GetMouseButton(0))
        {
            //左クリックした地点のスクリーン座標を受け取る
            mousePosition = Input.mousePosition;
            //スクリーン座標からワールド座標に変換
            lookPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            //このスクリプトをつけている物の座標に代入
            this.transform.position = lookPosition;
        }
    }
}
