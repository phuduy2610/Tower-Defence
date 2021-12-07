
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed = 20f;

    [SerializeField]
    private float panBorderThickness = 10f;

    Vector2 panLimit;
    Vector2 startPos;


    // Start is called before the first frame update
    void Start()
    {
        startPos.x = transform.position.x;
        startPos.y = transform.position.y;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            transform.Translate(Vector3.up*cameraSpeed*Time.deltaTime);
        }

        if (Input.mousePosition.y <= panBorderThickness)
        {
            transform.Translate(Vector3.down*cameraSpeed*Time.deltaTime);
        }

        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            transform.Translate(Vector3.right*cameraSpeed*Time.deltaTime);
        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            transform.Translate(Vector3.left*cameraSpeed*Time.deltaTime);
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x,startPos.x,panLimit.x),Mathf.Clamp(transform.position.y,panLimit.y,startPos.y),-10);
    }

    public void SetLimits(Vector3 maxTile){
        Vector3 wp = Camera.main.ViewportToWorldPoint(new Vector3(1,0));
        //Khoảng cách có thể move là từ limit của camera tới chỗ tile xa nhất
        panLimit.x = maxTile.x - wp.x; 
        panLimit.y = maxTile.y - wp.y;

    }

}
