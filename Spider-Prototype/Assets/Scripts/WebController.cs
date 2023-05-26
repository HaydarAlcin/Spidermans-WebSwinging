using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WebController : MonoBehaviour
{
    private Rigidbody _playerRb;
    private Rigidbody _rb;
    [SerializeField] GameObject web, player;
    [SerializeField] GameObject[] Walls;



    [SerializeField] Vector3 Hitpoint, currentPos, swingForce;
    [SerializeField] LineRenderer lr;
    [SerializeField] SpringJoint joint;

    public bool paste;
    

    [SerializeField] LayerMask Swingable;

    [SerializeField] LayerMask wallLayer;

    Vector3 firstPos;
    Quaternion firstRotate;

    [SerializeField] float JointForce;
    [SerializeField] float JointDamp;
    [SerializeField] float maxSpringDis = 0.2f;
    [SerializeField] float minSpringDis = 0f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _playerRb = player.GetComponent<Rigidbody>();

        lr = player.GetComponent<LineRenderer>();
        lr.enabled = false;
    }

    
    void Update()
    {
        
        Walls = GameObject.FindGameObjectsWithTag("wall");
        currentPos = transform.position;
        lr.SetPosition(0, currentPos);
        

        if (Input.GetMouseButtonDown(0))
        {
            paste = true;
            StartSwing();
        }
        
        else if (Input.GetMouseButtonUp(0))
        {
            
            paste = false;
            _playerRb.AddForce(player.transform.forward + swingForce,ForceMode.Force);
            StopSwing();
        }
    }

    


    public void WebMove()
    {
        
        Vector3 closest;
        //Vector3 distance;
        //distance = currentPos - Hitpoint;
        GameObject closestWall = Walls.OrderBy(obj => Vector3.Distance(transform.position, obj.transform.position)).FirstOrDefault();
        closest = closestWall.transform.GetChild(0).GetComponent<BoxCollider>().ClosestPoint(lr.GetPosition(0));
        lr.SetPosition(1, closest);

        Vector3 direction = closest - player.transform.position; // Ýki nokta arasýndaki yön vektörü

        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, direction);
        player.transform.rotation = Quaternion.Euler(player.transform.rotation.eulerAngles.x, player.transform.rotation.eulerAngles.y, rotation.eulerAngles.z);

        Hitpoint = closest;
        //Debug.Log(distance);
    }


    public void StartSwing()
    {
        lr.enabled = true;
        WebMove();
        player.AddComponent<SpringJoint>();
        
        joint = player.GetComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = Hitpoint;

        float PointTohandDis = Vector3.Distance(Hitpoint, this.transform.position);
        joint.maxDistance = PointTohandDis * maxSpringDis;
        joint.minDistance = PointTohandDis * minSpringDis;


        joint.spring = JointForce;
        joint.massScale = JointForce;
        joint.damper = JointDamp;

       
    }

    void StopSwing()
    {
        lr.enabled = false;
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation,Quaternion.Euler(0, player.transform.rotation.eulerAngles.y, 0),1f);
        Destroy(joint);

    }
}
