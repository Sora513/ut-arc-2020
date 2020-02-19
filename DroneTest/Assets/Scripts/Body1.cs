using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Body1 : MonoBehaviour {
    
    public float ThrustLimit;//N/s
    public float ThrustChangeLimit;//N/s*s
    public float ServoSpeedLimit;//オイラー角/sec
    public Rigidbody rb;
    public Rigidbody RA_rb,LA_rb;
    public GameObject RA,LA;
    public HingeJoint[] servos;
    public HingeJoint servo_r,servo_l;
    public JointSpring SpringR,SpringL;
    public ConstantForce ThrustR,ThrustL;
    
    void Start () {
        ServoSpeedLimit = 100f;
        ThrustChangeLimit = 20f;
        ThrustLimit = 20f;
        rb = GetComponent<Rigidbody>();
        servos = GetComponents<HingeJoint>();
        servo_r = servos[0];
        servo_l = servos[1];
        

        RA = GameObject.Find("Arm_right_ver1");
        RA_rb = RA.GetComponent<Rigidbody>();
        SpringR = servo_r.spring;
        SpringR.spring = 1e38f;
        SpringR.damper = 3;
        SpringR.targetPosition = 0;
        servo_r.spring = SpringR;
        ThrustR = GameObject.Find("propeller_right").GetComponent<ConstantForce>();

       
        LA = GameObject.Find("Arm_left_ver1");
        LA_rb = LA.GetComponent<Rigidbody>();
        SpringL = servo_l.spring;
        SpringL.spring = 1e38f;
        SpringL.damper = 3;
        SpringL.targetPosition = 0;
        servo_l.spring = SpringL;
        ThrustL = GameObject.Find("propeller_right").GetComponent<ConstantForce>();
        
        
        Debug.Log(GameObject.Find("propeller_left").GetComponent<ConstantForce>().relativeForce.y);
    }

    void JointReflect(float r,float l){
        /*
        サーボモータへの反映
        但し、速度制限あり。ServoSpeedLimitで変更
        */
        SpringR = servo_r.spring;
        SpringL = servo_l.spring;
        float speed = ServoSpeedLimit*Time.deltaTime;

        if(r-SpringR.targetPosition > speed)
        {
            SpringR.targetPosition += speed;
        }
        else if(SpringR.targetPosition - r > speed)
        {
            SpringR.targetPosition -= speed;
        }
        else
        {
            SpringR.targetPosition = r;
        }
        servo_r.spring = SpringR;

        if(l-SpringL.targetPosition > speed)
        {
            SpringL.targetPosition += speed;
        }
        else if(SpringL.targetPosition - l > speed)
        {
            SpringL.targetPosition -= speed;
        }
        else
        {
            SpringL.targetPosition = l;
        }
        servo_l.spring = SpringL;
    }
     
    void ThrustReflect(float r,float l)
    {
        if(r > ThrustLimit)r = ThrustLimit;
        if(l > ThrustLimit)l = ThrustLimit;
        float speed = ThrustChangeLimit * Time.deltaTime;
        float y_r = ThrustR.relativeForce.y;
        float y_l = ThrustL.relativeForce.y;
        if(r- y_r> speed)
        {
            y_r += speed;
        }
        else if(y_r - r > speed)
        {
            y_r -= speed;
        }
        else
        {
            y_r = r;
        }
        ThrustR.relativeForce = new Vector3(0f,y_r,0f);

       if(l- y_l> speed)
        {
            y_l += speed;
        }
        else if(y_l - l > speed)
        {
            y_l -= speed;
        }
        else
        {
            y_l = l;
        }
        ThrustL.relativeForce = new Vector3(0f,y_l,0f);

        
    }
    void FixedUpdate () {
        /*
            JointReflect(右サーボ角度、左サーボ角度);但しオイラー
            ThrustReflect(右出力、左出力)それぞれ15.19Nでつり合い
        */
        if(Input.GetKey(KeyCode.W))
        {
            JointReflect(45,45);
        }
        
         else if(Input.GetKey(KeyCode.S))
        {
            JointReflect(-45,-45);
        }
        
        else if(Input.GetKey(KeyCode.D))
        {
            JointReflect(-45,45);
        }

        else if(Input.GetKey(KeyCode.A))
        {
            JointReflect(45,-45);
        }
        else{
            JointReflect(0,0);
        }

        if(Input.GetKey(KeyCode.UpArrow))
        {
            ThrustReflect(20,20);
            Debug.Log(ThrustR.relativeForce.y);
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            ThrustReflect(0,0);
        }
        //rb.AddForce(transform.up * thrust);
    }

}
