using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFunction : MonoBehaviour,IPooledObject
{
    public float speed;
    // Start is called before the first frame update
   public void OnObjectSpawn()
    {
        speed = 10f;
        GetComponent<Rigidbody>().velocity = -transform.forward * speed;
    }
    public void Start()
    {
        StartCoroutine(Kill());
    }
    IEnumerator Kill()
    {
        yield return new WaitForSeconds(4);
        Destroy(this.gameObject);
    }

}
