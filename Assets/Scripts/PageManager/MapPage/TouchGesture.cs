using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchGesture : MonoBehaviour
{
    public FingerGesture fg;
    public Transform player;
    Vector3 desiredPosition;

    private Vector3 targetPos;
    public float speed = 2.0f;

    public float smoothTime = 10;
    private Vector3 velocity = Vector3.zero;

    bool ta;

    // Use this for initialization
    void Start()
    {
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (fg.SwipeLeft)
            desiredPosition += Vector3.left;

        if (fg.SwipeRight)
            desiredPosition += Vector3.right;

        if (fg.SwipeUp)
            desiredPosition += Vector3.forward;

        if (fg.SwipeDown)
            desiredPosition += Vector3.back;

        //if (Input.GetMouseButtonDown(0))
        //{
            player.transform.position = Vector3.MoveTowards(player.transform.position, desiredPosition, 2 * Time.deltaTime);
        //}



        if (fg.Tap)
        {
            Debug.Log("Tap");
        }

    }

    void OnMouseDrag()
    {
        //float distance = transform.position.z - Camera.main.transform.position.z;
        //targetPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        //targetPos = Camera.main.ScreenToWorldPoint(targetPos);

        //Vector3 followXonly = new Vector3(targetPos.x, transform.position.y, targetPos.y);
        //transform.position = Vector3.SmoothDamp(transform.position, followXonly, ref velocity, smoothTime);
    }
}
