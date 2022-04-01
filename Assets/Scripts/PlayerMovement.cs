using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JacDev.Fix
{
    public class PlayerMovement : MonoBehaviour
    {
        public static Transform player;
        public float moveSpeed = 3f;

        public Rigidbody2D rb;
        public Animator animator;

        Vector2 movement;

        public GameObject shiftingCol, standCol;


        [Header("無人機設定")]
        public KeyCode droneActiveKey = KeyCode.E;
        public KeyCode droneRecallKey = KeyCode.Q;
        public KeyCode hackActionKey = KeyCode.F;
        public GameObject dronePrefab = default;
        public GameObject currentDrone = default;
        public float droneMoveForce = 10f;
        public bool isUsingDrone = false;

        // Start is called before the first frame update
        void Start()
        {
            player = transform;
        }

        // Update is called once per frame
        void Update()
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (!isUsingDrone)
            {
                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Vertical", movement.y);
                animator.SetFloat("Speed", movement.sqrMagnitude);

                if (Input.GetKeyDown(hackActionKey))
                {
                    var hits = Physics2D.CapsuleCastAll(
                                            transform.position,
                                            Vector2.one * .15f,
                                            CapsuleDirection2D.Vertical,
                                            360f,
                                            Vector2.up);

                    if (hits.Length > 0)
                    {
                        foreach (var h in hits)
                        {
                            if (h.transform.GetComponent<HackingSpotBase>() != null)
                                h.transform.GetComponent<HackingSpotBase>().Hack(HackingSpotBase.HackSpotType.Player);
                        }
                    }
                }
            }
            else    // 後續新增無人機動畫，此處需重新寫過
            {
                animator.SetFloat("Horizontal", 0);
                animator.SetFloat("Vertical", 0);
                animator.SetFloat("Speed", 0);

                if (currentDrone != null)
                    currentDrone.transform.eulerAngles = Vector3.up * (currentDrone.GetComponent<Rigidbody2D>().velocity.x > 0 ? 0 : 180);

                if (Input.GetKeyDown(hackActionKey))
                {
                    var hits = Physics2D.CapsuleCastAll(
                                            currentDrone.transform.position,
                                            Vector2.one * .15f,
                                            CapsuleDirection2D.Vertical,
                                            360f,
                                            Vector2.up);

                    if (hits.Length > 0)
                    {
                        foreach (var h in hits)
                        {
                            if (h.transform.GetComponent<HackingSpotBase>() != null)
                                h.transform.GetComponent<HackingSpotBase>().Hack(HackingSpotBase.HackSpotType.Drone);
                        }
                    }
                }
            }


            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                animator.SetBool("Shifting", true);
                animator.SetTrigger("Shift");

                standCol.SetActive(false);
                shiftingCol.SetActive(true);
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                animator.SetBool("Shifting", false);

                standCol.SetActive(true);
                shiftingCol.SetActive(false);
            }

            if (Input.GetKeyDown(droneActiveKey))
            {
                isUsingDrone = !isUsingDrone;
                if (currentDrone == null)
                    currentDrone = Instantiate(dronePrefab, transform.position, Quaternion.identity);
                else if (!currentDrone.activeInHierarchy)
                    currentDrone.transform.position = transform.position;

                currentDrone.SetActive(true);

                FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().Follow = isUsingDrone ? currentDrone.transform : transform;
            }

            if (Input.GetKeyDown(droneRecallKey))
            {
                if (currentDrone == null)
                    return;

                isUsingDrone = false;
                currentDrone.SetActive(false);

                FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().Follow = isUsingDrone ? currentDrone.transform : transform;
            }
        }

        private void FixedUpdate()
        {
            if (currentDrone != null && isUsingDrone)
            {
                var dRb = currentDrone.GetComponent<Rigidbody2D>();
                // 不再使用，飛行無人機改為爬行無人機
                // dRb.AddForce(movement * droneMoveForce * Time.fixedDeltaTime);
                dRb.MovePosition(dRb.position + movement * moveSpeed * Time.fixedDeltaTime);
                return;
            }

            if (!isUsingDrone)
                rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }

    }

}
