using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    private Vector3 mouseRotatePos = new Vector3();
    private Vector3 mouseMovePos = new Vector3();

    private float mouseAngleX = 0;
    private float mouseAngleY = 0;

    public float mouseRotateBias = 100;
    public float mouseMoveBias = 10;

    private Vector3 mousePos = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // マウス操作によるカメラの左右回転
        UpdateMouseRotate();
        UpdateMouseMove();
        UpdateMouseWheel();
    }

    void UpdateMouseRotate()
    {
        if( Input.GetMouseButtonDown( 1 ) )
        {
            this.mouseRotatePos = Input.mousePosition;
            this.mouseAngleX = this.gameObject.transform.eulerAngles.x;
            this.mouseAngleY = this.gameObject.transform.eulerAngles.y;
        }

        if( Input.GetMouseButton( 1 ) )
        {
            //(移動開始座標 - マウスの現在座標) / 解像度 で正規化
            float x = ( this.mouseRotatePos.x - Input.mousePosition.x ) / Screen.width;
            float y = ( this.mouseRotatePos.y - Input.mousePosition.y ) / Screen.height;

            //回転開始角度 ＋ マウスの変化量 * マウス感度
            float eulerX = this.mouseAngleX + y * this.mouseRotateBias;
            float eulerY = this.mouseAngleY + x * this.mouseRotateBias;

            this.gameObject.transform.rotation = Quaternion.Euler( eulerX, eulerY, 0 );
        }
    }

    void UpdateMouseMove()
    {
        if( Input.GetMouseButtonDown( 2 ) )
        {
            this.mouseMovePos = Input.mousePosition;
            this.mousePos = this.gameObject.transform.position;
        }

        if( Input.GetMouseButton( 2 ) )
        {
            //(移動開始座標 - マウスの現在座標) / 解像度 で正規化
            Vector3 vec = new Vector3();
            vec.x = ( this.mouseMovePos.x - Input.mousePosition.x ) / Screen.width;
            vec.y = ( this.mouseMovePos.y - Input.mousePosition.y ) / Screen.height;

            //回転開始角度 ＋ マウスの変化量 * マウス感度
            vec = this.gameObject.transform.rotation * vec * this.mouseMoveBias;
            this.gameObject.transform.position = this.mousePos + vec;
        }
    }

    void UpdateMouseWheel()
    {
        float scroll = Input.mouseScrollDelta.y;
        if( scroll == 0 )
        {
            return;
        }

           //(移動開始座標 - マウスの現在座標) / 解像度 で正規化
            Vector3 vec = new Vector3( 0, 0, scroll );
            vec = this.gameObject.transform.rotation * vec;
            this.gameObject.transform.position += vec;
    }

}
