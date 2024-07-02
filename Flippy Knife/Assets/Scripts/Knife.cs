using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Knife : MonoBehaviour
{
    public Rigidbody rb;

    public float force = 5f;
    public float torque = 20f;

    private float startSwipeTime;
    private float minimumTimeInAir = 0.05f;

    private float restartDelay = 0.5f;

    private Vector2 startSwipe;
    private Vector2 endSwipe;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!rb.isKinematic)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            startSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            endSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Swipe();
        }
    }

    void Swipe()
    {
        rb.isKinematic = false;

        startSwipeTime = Time.time;

        Vector2 swipe = endSwipe - startSwipe;

        rb.AddForce(swipe * force, ForceMode.Impulse);
        rb.AddTorque(0f, 0f, -torque, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GetTimeInAir() >= minimumTimeInAir)
        {
            if (other.tag == "WoodenBlock")
            {
                rb.isKinematic = true;
            } else
            {
                Invoke("Restart", restartDelay);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!rb.isKinematic && GetTimeInAir() >= minimumTimeInAir)
        {
            Invoke("Restart", restartDelay);
        }
    }

    private float GetTimeInAir()
    {
        return Time.time - startSwipeTime;
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
