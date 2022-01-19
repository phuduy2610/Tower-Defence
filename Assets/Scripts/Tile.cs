using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum TilesType
{
    G, //Kiểu có thể đặt Máy bắn
    G1, //Ground về mặt visual nhưng không thể đặt máy
    P, //Enemy có thể di chuyển và đặt trap
    P3, // P không thể đặt trap
    E0, // Các E là để cho map đẹp hơn không thể tương tác
    P1, // Chỗ spawn portal
    P2, // Chỗ spawn tháp để thủ
    E1,
    E2,
    E3,
    E4,
    E5,
    E6,
    E7,
    E8,
    E9,
    E10,
    E11
}
public class Tile : MonoBehaviour
{

    public Point GridPosition { get; private set; }
    public TilesType type { get; private set; }
    public Vector3 WorldPos { get; private set; }
    private Color32 fullColor = new Color32(255, 74, 0, 225);
    private Color32 emptyColor = new Color32(0, 225, 56, 225);
    private SpriteRenderer spriteRenderer;

    public bool IsEmpty{get;set;} = true;

    public bool isHitbyRay { get; set; } = false;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {

    }

    public void Setup(Point gridPos, Vector3 worldPos, TilesType type)
    {
        this.GridPosition = gridPos;
        this.type = type;

        transform.position = worldPos;
        WorldPos = worldPos;
    }


    public void ColorTile(Color newColor)
    {
        spriteRenderer.color = newColor;
    }

    public void TurnColorGreen()
    {
        ColorTile(emptyColor);
    }

    public void TurnColorRed(){
        ColorTile(fullColor);
    }

    public void TurnColorWhite(){
        ColorTile(Color.white);
    }

}
