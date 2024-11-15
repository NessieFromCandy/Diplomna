using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCast : MonoBehaviour
{
    [SerializeField] protected type _type;
    [SerializeField] protected Transform _cachedTransform;
    [SerializeField] protected float _radius;
    [SerializeField] protected Vector3 _size;
    [SerializeField] protected Vector3 _direction;
    [SerializeField] protected float _maxDistance;
    [SerializeField] protected LayerMask _layerMask;
    [SerializeField] protected int _resultCount;
    [SerializeField] protected bool _DEBUG;

    protected RaycastHit[] _results;
    protected Color _debugColor = Color.grey;

    private void Awake()
    {
        _results = new RaycastHit[_resultCount];
    }

    public void Setup(Transform position, float radius, Vector3 direction, float maxDistance, LayerMask layerMask)
    {
        _cachedTransform = position;
        _radius = radius;
        _direction = direction;
        _maxDistance = maxDistance;
        _layerMask = layerMask;
    }

    public virtual bool TryCollide(out RaycastHit[] results, out int collisions)
    {
        collisions = 0;
        _results = new RaycastHit[_resultCount];

        if (_type == type.SPHERE)
        {
            collisions = Physics.SphereCastNonAlloc(_cachedTransform.position, _radius, _direction, _results, _maxDistance, _layerMask);
        }
        else if (_type == type.CUBE)
        {
            collisions = Physics.BoxCastNonAlloc(_cachedTransform.position, _size, _direction, _results, Quaternion.identity, _maxDistance, _layerMask);
        }

        bool hasCollisions = collisions > 0;
        _debugColor = hasCollisions ? Color.green : Color.red;
        results = _results;
        return hasCollisions;
    }

    public virtual bool TryCollide(out RaycastHit[] results)
    {
        int collisions = 0;
        return TryCollide(out results, out collisions);
    }

    public virtual bool TryCollide()
    {
        int collisions = 0;
        return TryCollide(out _results, out collisions);
    }

    private void OnValidate()
    {
        _resultCount = _resultCount < 1 ? 1 : _resultCount;
    }

    private void OnDrawGizmos()
    {
        if (!_DEBUG) return;

        Gizmos.color = _debugColor;
        if (_type == type.SPHERE)
        {
            Gizmos.DrawWireSphere(_cachedTransform.position, _radius);
        }
        else if (_type == type.CUBE)
        {
            Gizmos.matrix = Matrix4x4.TRS(_cachedTransform.position, _cachedTransform.rotation, _cachedTransform.lossyScale);
            Gizmos.DrawWireCube(Vector3.zero, _size * 2f);
            Gizmos.matrix = Matrix4x4.identity;
        }
    }

    protected enum type
    {
        SPHERE,
        CUBE,
    }
}
