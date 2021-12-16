using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public float radius;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<JacDev.Fix.playermovement>())
        {
            BombTriggered();
            gameObject.SetActive(false);
        }
    }

    public void BombTriggered()
    {
        var enemies = FindObjectsOfType<FieldOfView>();

        foreach (var e in enemies)
        {
            if (((Vector2)(e.transform.position - transform.position)).magnitude > radius)
                continue;

            e.warningValue = 1;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 1, .5f);
        Gizmos.DrawSphere(transform.position, radius);
    }
}
