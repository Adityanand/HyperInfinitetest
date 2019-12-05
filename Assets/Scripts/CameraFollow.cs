using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public Transform PlayerTransform;
    public float smoothSpeed = 0.125f;
    public Vector3 Offset;
    // Start is called before the first frame update
    void Start()
    {
        Offset = new Vector3(0, 2.2f, 1.5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlayerTransform != null)
        {
            Vector3 desiredPostion = PlayerTransform.position + Offset;
            Vector3 SmoothedPostion = Vector3.Lerp(transform.position, desiredPostion, smoothSpeed);
            transform.position = SmoothedPostion;
        }
        else
        {
            this.transform.position = new Vector3(0, 2, 2);
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void TryAgain()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
