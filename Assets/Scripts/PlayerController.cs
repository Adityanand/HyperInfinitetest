using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Animator Players;
    BulletContainer Bullets;
    public GameObject Stone;
    public Transform StoneSpawner;
    public float speed;
    public int Health;
    public Canvas GameOver;
    // Start is called before the first frame update
    void Start()
    {
        speed = 10f;
        Players=this.gameObject.GetComponent<Animator>();
        Bullets = BulletContainer.Instance;
        Health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            Players.SetBool("Running", true);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            Players.SetBool("Walking",true);
        }
        else
        {
            Players.SetBool("Running", false);
            Players.SetBool("Walking", false);
        }
        if(Input.GetKey(KeyCode.A)&& (Players.GetBool("Walking")== true||Players.GetBool("Running") == true))
        {
            transform.Rotate(Vector3.down * 25f * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.A))
        {
            Players.SetBool("Left", true);
        }
        else
        {
            Players.SetBool("Left", false);
        }
        if (Input.GetKey(KeyCode.D) && (Players.GetBool("Walking") == true||Players.GetBool("Running") == true))
        {
            transform.Rotate(Vector3.up * 25f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Players.SetBool("Right", true);
        }
        else
        {
            Players.SetBool("Right", false);
        }
        if (Input.GetKey(KeyCode.Space) && (Players.GetBool("Walking") == true ||Players.GetBool("Running") == true))
        {
            Players.SetBool("Jump", true);
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            Players.SetBool("Jump", true);
        }
        else
        {
            Players.SetBool("Jump", false);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && (Players.GetBool("Walking") == true || Players.GetBool("Running") == true))
        {
            Players.SetBool("Throw", true);
            // Invoke("throwObject", 1);
            StartCoroutine(Throw());
        }
       else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Players.SetBool("Throw", true);
            // Invoke("throwObject", 1);
            StartCoroutine(Throw());
        }
        else
        {
            Players.SetBool("Throw", false);
        }
    }
    public IEnumerator Throw()
    {
        yield return new WaitForSeconds(.75f);
        var ThrowObj=Instantiate(Stone, StoneSpawner.position, transform.rotation);
        ThrowObj.GetComponent<Rigidbody>().velocity = transform.forward * speed;

    }
    public void throwObject()
    {
        Bullets.SpawnFromPool("Bullet",StoneSpawner.transform.position ,transform.position, Quaternion.identity);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag =="Bullet")
        {
            Debug.Log("Hello");
            Health = Health - 20;
            StartCoroutine(Kill());
        }
    }
    IEnumerator Kill()
    {
        if(Health<=0)
        {
            this.GetComponent<Animator>().SetBool("Dying", true);
            yield return new WaitForSeconds(5);
            GameOver.enabled = true;
            Destroy(this.gameObject);
           
        }
    }
}
