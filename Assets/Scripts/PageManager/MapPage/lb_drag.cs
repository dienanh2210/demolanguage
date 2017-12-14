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

        float widthRel = this.transform.localScale.y / (Screen.width) / 2; //relative width
        float heightRel = this.transform.localScale.x / (Screen.height) / 2; //relative height

        Vector3 viewPos = Camera.main.WorldToViewportPoint(this.transform.position);//
        viewPos.x = Mathf.Clamp(viewPos.x, widthRel + 0.1f, 0.9f + widthRel);// right -left
        viewPos.y = Mathf.Clamp(viewPos.y, heightRel + 0.31f, 0.68f + heightRel);//top - bottom
        this.transform.position = Camera.main.ViewportToWorldPoint(viewPos);

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