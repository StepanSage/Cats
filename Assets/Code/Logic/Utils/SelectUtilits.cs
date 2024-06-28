using DG.Tweening;
using UnityEngine;

public class SelectUtilits : MonoBehaviour
{
    protected const float VALUETIME = 0.5f;

    protected float OFFSET = 0.5f;
    protected Vector3 _normal;
    protected Vector3 _select;

    protected virtual void Start()
    {
        _normal = transform.localScale;
        _select = new Vector3(transform.localScale.x + OFFSET, transform.localScale.y + OFFSET, transform.localScale.z+ OFFSET);
    }

    public virtual void Select()
    {
        transform
            .DOScale(_select, VALUETIME);
    }

    public virtual void UNSelecet()
    {
        transform
            .DOScale(_normal, VALUETIME);
    }
}
