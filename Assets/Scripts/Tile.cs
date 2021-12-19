using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum TilesType
{
    G, //Kiểu có thể đặt Máy bắn
    P, //Enemy có thể di chuyển và đặt trap
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
    private void Start()
    {
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

    private void OnMouseEnter()
    {
    }

    private void OnMouseOver()
    {

    }
}
