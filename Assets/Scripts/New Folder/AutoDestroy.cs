using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float destroyTimer = 4f;

    private void Start() => Destroy(gameObject, destroyTimer);
}
