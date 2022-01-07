using UnityEngine;

public class Pointer : PersistentSingleton<Pointer>
{
    [SerializeField]
    private Texture2D defaultPointer;

    [SerializeField]
    private Texture2D attackPointer;

    public enum PointerType { @default, attack}

    protected override void Awake()
    {
        base.Awake();
        Vector2 offset = new Vector2(defaultPointer.width * 0, defaultPointer.height * 0);
        Cursor.SetCursor(defaultPointer, offset, CursorMode.Auto);
    }

    public void switchPointer(PointerType pointerType) 
    {
        switch (pointerType)
        {
            case PointerType.@default:
                SetPointer(defaultPointer);
                break;
            case PointerType.attack:
                SetPointer(attackPointer);
                break;
            default:
                break;
        }
    }

    private void SetPointer(Texture2D tex)
    {
        var offset = new Vector2(tex.width / 2, tex.height / 2);
        Cursor.SetCursor(tex, offset, CursorMode.Auto);
    }
}
