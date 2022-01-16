using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Hover : Singleton<Hover>
{
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }

    //Hàm để hiện lên icon khi chọn tháp
    private void FollowMouse()
    {
        if (spriteRenderer.enabled)
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = 10;
            transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }
    }

    //Bật sprite lên khi tháp được chọn
    public void Activate(Sprite sprite)
    {
        this.spriteRenderer.enabled = true;
        this.spriteRenderer.sprite = sprite;
    }

    //Tắt đi khi đặt xong
    public void DeActivate()
    {
        LevelManager.Instance.ClickedBtn = null;
        this.spriteRenderer.enabled = false;
    }
}
