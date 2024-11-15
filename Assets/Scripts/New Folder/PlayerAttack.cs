using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage;
    public PhysicsCast physicsCast;
    public Animator swordAnimator;
    public float cooldown = 1f;

    private float cooldownTimer;

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer > 0f) return;

        if (Input.GetMouseButton(1))
        {
            swordAnimator.Play("Swing");
            cooldownTimer = cooldown;
        }
    }

    public void TryHitEnemy()
    {
        RaycastHit[] hits;
        int collisions = 0;
        if (physicsCast.TryCollide(out hits, out collisions))
        {
            for (int i = 0; i < collisions; i++)
            {
                Enemy enemy;
                if (hits[i].collider.TryGetComponent(out enemy)) enemy.TakeDamage(damage);
            }
        }
    }
}
