using UnityEngine;

namespace RL
{
    public class PlayerMove : MonoBehaviour
    {
        private Archer acher;
     
        public float speed;
        public Vector3 moveInput;
        public  bool isMoving = false;
        Rigidbody rb;
        private void Start()
        {
            acher = GetComponent<Archer>();
            speed = acher.Adata.currentData.speed;
            rb = GetComponent<Rigidbody>();

        }
        public void FixedUpdate()
        {
            if (moveInput.sqrMagnitude > 0.1f)
            {
                rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);
                isMoving = true;
                transform.forward = moveInput;
            }
            else
            {
                isMoving = false;
            }
        }
        public void AddSpeed(float amount)
        {
            speed += amount;
        }
        public void Setinput(float h, float v)
        {
            moveInput = new Vector3(h, 0, v).normalized;
        }


    }
}