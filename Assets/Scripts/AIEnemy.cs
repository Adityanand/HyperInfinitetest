using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemy : MonoBehaviour
{
    [Header("Enemy")]
    public Animator Enemy;
    public float speed;
    [Header("Sensor")]
    public float SensorLength;
    public Vector3 FrontSensorPos;
    public float SideSensorPos;
    public float FrontSensorAngle;
    public GameObject Player;
    [Header("Throw")]
    public GameObject Stone;
    public Transform StoneSpawner;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        FrontSensorPos = new Vector3(0, 1f, 0.25f);
        SensorLength = .5f;
        SideSensorPos = .1f;
        FrontSensorAngle =45;
        Enemy = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        speed = 10f;
        time = 3f;
    }
    void Sensor()
    {
        RaycastHit hit;
        Vector3 SensorStartingPos = transform.position;
        SensorStartingPos += transform.forward * FrontSensorPos.z;
        SensorStartingPos += transform.up * FrontSensorPos.y;



        //front Right Sensor Position
        SensorStartingPos += transform.right * SideSensorPos;
        if (Physics.Raycast(SensorStartingPos, transform.forward, out hit, SensorLength))
        {
            if (!hit.collider.CompareTag("Ground"))
            {
                Debug.DrawLine(SensorStartingPos, hit.point);
               // Enemy.SetBool("Left", true);
                this.transform.Rotate(-Vector3.up * 45f);
            }
        }

        //front Right Angled Sensor Position
        else if (Physics.Raycast(SensorStartingPos, Quaternion.AngleAxis(FrontSensorAngle, transform.up) * transform.forward, out hit, SensorLength))
        {
            if (!hit.collider.CompareTag("Ground"))
            {
                Debug.DrawLine(SensorStartingPos, hit.point);
               // Enemy.SetBool("Left", true);
                this.transform.Rotate(-Vector3.up * 45f);
            }
        }
        else
        {
            Enemy.SetBool("Left", false);
        }

        //front Left Sensor Position
        SensorStartingPos -= 2 * transform.right * SideSensorPos;
        if (Physics.Raycast(SensorStartingPos, transform.forward, out hit, SensorLength))
        {
            if (!hit.collider.CompareTag("Ground"))
            {
                Debug.DrawLine(SensorStartingPos, hit.point);
               // Enemy.SetBool("Right", true);
                this.transform.Rotate(Vector3.up * 45f);
            }
        }

        //front Left Angled Sensor Position
        else if (Physics.Raycast(SensorStartingPos, Quaternion.AngleAxis(-FrontSensorAngle, transform.up) * transform.forward, out hit, SensorLength))
        {
            if (!hit.collider.CompareTag("Ground"))
            {
                Debug.DrawLine(SensorStartingPos, hit.point);
                //Enemy.SetBool("Right", true);
                this.transform.Rotate(Vector3.up * 45f);
            }
        }
        else
        {
            Enemy.SetBool("Right", false);
        }
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        Sensor();
        if (Vector3.Distance(Player.transform.position, transform.position) > 5f)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, 0f * Time.deltaTime);
            Enemy.SetBool("Walking", true);
            
        }
        else if(Vector3.Distance(Player.transform.position, transform.position) <= 4f)
        {
            time = time - 1 * Time.deltaTime;
            transform.LookAt(Player.transform);
            Enemy.SetBool("Walking", false);
            if(time<=0)
            {
                Enemy.SetBool("StoneThrow", true);
                var ThrowObj = Instantiate(Stone, StoneSpawner.position, transform.rotation);
                ThrowObj.GetComponent<Rigidbody>().velocity = transform.forward * speed;
                time = 3f;
            }
            else if(time>=2.8f)
            {
                Enemy.SetBool("StoneThrow", false);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag=="Bullet")
        {
            Debug.Log("hello");
            StartCoroutine(Kill());
        }
    }
    public IEnumerator Kill()
    {
        Enemy.SetBool("Dying", true);
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }
}
