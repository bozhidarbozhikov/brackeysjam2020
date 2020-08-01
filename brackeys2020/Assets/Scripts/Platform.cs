using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Collider2D tilemapCollider;
    public Transform groundCheck;
    public float disableDuration;

    private bool playerOnPlatform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            Collider2D[] hit = Physics2D.OverlapCircleAll(groundCheck.position, 0.25f);

            foreach (Collider2D item in hit)
            {
                if (item.CompareTag("Platform"))
                {
                    StartCoroutine(DisableCollider());
                    return;
                }
            }
        }

        if (Input.GetButtonDown("Jump") && !playerOnPlatform)
        {
            StartCoroutine(DisableCollider());
        }
    }

    IEnumerator DisableCollider()
    {
        tilemapCollider.enabled = false;

        yield return new WaitForSeconds(disableDuration);

        tilemapCollider.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            playerOnPlatform = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            playerOnPlatform = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, 0.25f);
    }
}
