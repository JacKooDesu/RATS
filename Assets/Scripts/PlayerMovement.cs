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
            }
            else    // 後續新增無人機動畫，此處需重新寫過
            {
                if (currentDrone != null)
                    currentDrone.transform.eulerAngles = Vector3.up * (movement.x > 0 ? 0 : 180);
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
                {
                    currentDrone = Instantiate(dronePrefab, transform.position, Quaternion.identity);
                }

                FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().Follow = isUsingDrone ? currentDrone.transform : transform;
            }
        }

        private void FixedUpdate()
        {
            if (currentDrone != null && isUsingDrone)
            {
                var dRb = currentDrone.GetComponent<Rigidbody2D>();
                dRb.AddForce(movement * droneMoveForce * Time.fixedDeltaTime);
                return;
            }

            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }

    }

}