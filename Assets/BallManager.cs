using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallManager : MonoBehaviour
{
    public GameObject templateBall = null;
    public Slider sliderPower = null;
    public Slider sliderMaxTorque = null;
    private GameObject currentBall = null;
    private bool isShootKey = false;
    private float shootTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.templateBall.SetActive( false );
        CreateBall();
    }

    // Update is called once per frame
    void Update()
    {
        // マウスクリックを見張る
        if( Input.GetMouseButtonDown( 0 ) )
        {
            this.isShootKey = true;
        }

        // 一定時間経過でボールを複製
        if( this.currentBall == null )
        {
            this.shootTimer += Time.deltaTime;
            if( this.shootTimer > 1.5f )
            {
                CreateBall();
                this.shootTimer = 0;
            }
        }
    }

    /*
     * 剛体へ力を加える場合はFixedUpdateで行う方が処理速度による結果の誤差が少ない
     */
    void FixedUpdate()
    {
        if( this.isShootKey )
        {
            Shoot();
            this.isShootKey = false;
        }
    }

    /*
     * ボールを打ち出す
     */
    void Shoot()
    {
        if( this.currentBall == null )
        {
            return;
        }

        // 現在のボールをクリックしたか？
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        Debug.DrawRay( ray.origin, ray.direction, Color.red, 100 );
        RaycastHit hit;
        if( !Physics.Raycast( ray, out hit, Mathf.Infinity ) )
        {
            return;
        }
        if( hit.transform != this.currentBall.transform )
        {
            return;
        }

        // クリック地点に対して水平に力を加える(跳ねないように)
        Rigidbody rigidbody = this.currentBall.GetComponent<Rigidbody>();
        //rigidbody.AddForceAtPosition( Vector3.forward * sliderPower.value, hit.point, ForceMode.Acceleration );
        rigidbody.AddForceAtPosition( ( hit.point - Camera.main.transform.position ).normalized * sliderPower.value, hit.point, ForceMode.Acceleration );
        rigidbody.constraints = rigidbody.constraints & ~RigidbodyConstraints.FreezeAll;
        this.currentBall = null;
    }

    /*
     * ボールを複製
     */
    void CreateBall()
    {
        this.currentBall = Instantiate( this.templateBall );
        this.currentBall.SetActive( true );
        Rigidbody rigidbody = this.currentBall.GetComponent<Rigidbody>();
        rigidbody.maxAngularVelocity = sliderMaxTorque.value;
        rigidbody.constraints = RigidbodyConstraints.FreezePositionY;

    }
}
