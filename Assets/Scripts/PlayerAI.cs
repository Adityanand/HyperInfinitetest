using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAI : MonoBehaviour
{
    Animator Player;
    public GameObject Enemy;
    public int Health;
    public Text HealthRemain;
    public Canvas GameOver;
    [Header("Sensor")]
    public float SensorLenght;
    public Vector3 FrontSensorPos;
    public float SideSensorPos;
    public float FrontSensorAngle;
    [Header("Throw")]
    public GameObject Stone;
    public Transform StoneSpawner;
    public float time;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        FrontSensorPos = new Vector3(0, 1f, 0.25f);
        SensorLenght = .5f;
        SideSensorPos = .1f;
        FrontSensorAngle = 30;
        Player = GetComponent<Animator>();
        speed = 10f;
        time = 2f;
        Health = 100;
        HealthRemain.text = Health.ToString();
    }
    void Sensor()
    {
        RaycastHit hit;
        Vector3 SensorStartingPos = transform.position;
        SensorStartingPos += transform.forward * FrontSensorPos.z;
        SensorStartingPos += transform.up * FrontSensorPos.y;
        //front Right Sensor Position
        SensorStartingPos += transform.right * SideSensorPos;
        if (Physics.Raycast(SensorStartingPos, transform.forward, out hit, SensorLenght))
        {
            if (!hit.collider.CompareTag("Ground"))
            {
                Debug.DrawLine(SensorStartingPos, hit.point);
                // Enemy.SetBool("Left", true);
                this.transform.Rotate(-Vector3.up * 45f);
            }
        }

        //front Right Angled Sensor Position
        else if (Physics.Raycast(SensorStartingPos, Quaternion.AngleAxis(FrontSensorAngle, transform.up) * transform.forward, out hit,SensorLenght))
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
            Player.SetBool("Left", false);
            Player.SetBool("Walking", true);
            this.transform.LookAt(Enemy.transform);
        }

        //front Left Sensor Position
        SensorStartingPos -= 2 * transform.right * SideSensorPos;
        if (Physics.Raycast(SensorStartingPos, transform.forward, out hit, SensorLenght))
        {
            if (!hit.collider.CompareTag("Ground"))
            {
                Debug.DrawLine(SensorStartingPos, hit.point);
                // Enemy.SetBool("Right", true);
                this.transform.Rotate(Vector3.up * 45f);
            }
        }

        //front Left Angled Sensor Position
        else if (Physics.Raycast(SensorStartingPos, Quaternion.AngleAxis(-FrontSensorAngle, transform.up) * transform.forward, out hit, SensorLenght))
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
            Player.SetBool("Right", false);
            this.transform.LookAt(Enemy.transform);
            Player.SetBool("Walking", true);

        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        HealthRemain.text = Health.ToString();
        Enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (Enemy == null)
            Health = 100;
        if ((Enemy != null) && (Vector3.Distance(Enemy.transform.position, transform.position) <= 4f))
        {
            time = time - 1 * Time.deltaTime;
            transform.LookAt(Enemy.transform);
            Player.SetBool("Walking", false);
            if (time <= 0)
            {
                Player.SetBool("Throw", true);
                var ThrowObj = Instantiate(Stone, StoneSpawner.position, transform.rotation);
                ThrowObj.GetComponent<Rigidbody>().velocity = transform.forward * speed;
                time = 2f;
            }
            else if (time >= 1.8f)
            {
                Player.SetBool("Throw", false);
            }
        }
        else
        Sensor();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Bullet")
        {
            Debug.Log("Hello");
            Health = Health - 20;
            StartCoroutine(Kill());
        }
    }
    IEnumerator Kill()
    {
        if (Health <= 0)
        {
            this.GetComponent<Animator>().SetBool("Dying", true);
            yield return new WaitForSeconds(5);
            GameOver.enabled = true;
            Destroy(this.gameObject);

        }
    }
}
