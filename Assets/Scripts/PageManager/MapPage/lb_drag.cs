using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class lb_drag : MonoBehaviour
{
    private Vector3 Origin;
    private Vector3 Diference;
    private Vector3 velocity = Vector3.zero;
    Vector3 toadonew;

    private Vector3 screenPoint;
    private Vector3 offset;
    Vector3 curPosition = new Vector3(0, 0, -9);

    Camera cam;
    float left;
    float right;
    float top;
    float bottom;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    void OnEnable()
    {
        Start();
    }

    void Update()
    {
        if (Input.touchCount != 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Origin = MousePos();
            }
            if (Input.GetMouseButtonUp(0))
            {

            }
            //if (Input.GetMouseButton(0))
            //{
            //    Diference = MousePos() - Origin;
            //    //Debug.Log(Diference.magnitude);
            //    if (Diference.magnitude >= 10)
            //    {
            //        Diference = Diference.normalized;
            //        toadonew = this.transform.position + new Vector3(Diference.x, 0, Diference.y) / 1.8f;// * 1.5f
            //        Origin = MousePos();
            //    }
            //}
            if (this.transform.position != toadonew)
            {
                // this.transform.position = Vector3.SmoothDamp(this.transform.position, toadonew, ref velocity, 0.10f);//0.10
            }
        }

        this.transform.position = Vector3.SmoothDamp(this.transform.position, curPosition, ref velocity, 0.11f);//0.10}

        if (cam.fieldOfView < 50 && cam.fieldOfView >= 45)
        {
            left = 0.8f;
            right = -0.8f;
            top = -9.5f;
            bottom = -8.5f;
        }
        else if (cam.fieldOfView < 45 && cam.fieldOfView >= 35)
        {
            left = 1f;
            right = -1f;
            top = -9.9f;
            bottom = -8.3f;
        }
        else if (cam.fieldOfView < 35 && cam.fieldOfView >= 30)
        {
            left = 1.38f;
            right = -1.38f;
            top = -10.5f;
            bottom = -7.5f;
        }
        else if (cam.fieldOfView < 30 && cam.fieldOfView >= 25)
        {
            left = 1.4f;
            right = -1.4f;
            top = -10.6f;
            bottom = -7.5f;
        }
        else if (cam.fieldOfView > 50 && cam.fieldOfView <= 60)
        {
            left = 0.54f;
            right = -0.54f;
            top = -9.07f;
            bottom = -8.93f;
        }

        //limits
        if (this.transform.position.x > left)
        {
            this.transform.position = new Vector3(left, this.transform.position.y, this.transform.position.z);
        }
        if (this.transform.position.x < right)
        {
            this.transform.position = new Vector3(right, this.transform.position.y, this.transform.position.z);
        }

        if (this.transform.position.z < top)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, top);
        }
        if (this.transform.position.z > bottom)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, bottom);
        }

    }
    Vector3 MousePos()
    {
        return Input.mousePosition;
        return Camera.main.WorldToScreenPoint(Input.mousePosition);
    }
    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        //transform.position = curPosition;
    }
}