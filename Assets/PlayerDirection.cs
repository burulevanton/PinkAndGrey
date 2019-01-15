using UnityEngine;


public class PlayerDirection
{
    public int Horizontal { get; set; }
    public int Vertical { get; set; }

    public Vector2 Direction => new Vector2(Horizontal, Vertical);

    public Quaternion RotateAngleZ
    {
        get
        {
            if (Direction == Vector2.up)
                return Quaternion.Euler(0, 0, 180f);
            if (Direction == Vector2.down)
                return Quaternion.Euler(0, 0, 0f);
            if (Direction == Vector2.left)
                return Quaternion.Euler(0, 0, -90f);
            if (Direction == Vector2.right)
                return Quaternion.Euler(0, 0, 90f);
            return new Quaternion();
        }
    }

    public Vector3 GetOffsetAfterPortal(Vector3 portalPosition)
    {
        if (Direction == Vector2.up)
            return new Vector3(portalPosition.x, portalPosition.y + 1f, portalPosition.z);
        if (Direction == Vector2.down)
            return new Vector3(portalPosition.x, portalPosition.y - 1f, portalPosition.z);
        if (Direction == Vector2.left)
            return new Vector3(portalPosition.x - 1f, portalPosition.y, portalPosition.z);
        if (Direction == Vector2.right)
            return new Vector3(portalPosition.x + 1f, portalPosition.y, portalPosition.z);
        return new Vector3();
    }
}
