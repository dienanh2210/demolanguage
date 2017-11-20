using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class lb_drag : MonoBehaviour
{

    private Vector3 Origin;
    private Vector3 Diference;
    public GameObject[] point;
    float xxx, yyy;
    private Vector3 velocity = Vector3.zero;
    bool chayxa;
    Vector3 toadonew;
    public GameObject[] img_thanh;

    void Start()
    {

        toadonew = this.transform.position;
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
                //Debug.Log(Origin);
                Origin = MousePos();
            }
            if (Input.GetMouseButtonUp(0))
            {
                //Debug.Log(MousePos());

                //  Debug.Log(Origin);
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
                this.transform.position = Vector3.SmoothDamp(this.transform.position, toadonew, ref velocity, 0.10f);//0.10

            }
        }


    }
    Vector3 MousePos()
    {
        return Input.mousePosition;
        return Camera.main.WorldToScreenPoint(Input.mousePosition);
    }
    void OnMouseDrag()
    {
        if (Input.GetMouseButton(0))
        {
            Diference = MousePos() - Origin;
            //Debug.Log(Diference.magnitude);
            if (Diference.magnitude >= 10)
            {
                Diference = Diference.normalized;

                toadonew = this.transform.position + new Vector3(Diference.x, 0, Diference.y) / 2.5f;// * 1.5f
                Origin = MousePos();
            }

        }
    }
}