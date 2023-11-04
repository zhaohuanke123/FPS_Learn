using System;
using System.Collections;
using UnityEngine;

namespace Vanks.FPS
{
    public class PlayerController : MonoBehaviour
    {
        public float rotateSpeed = 180;
        [Range(1, 2)] public float rotateRadio = 1;
        public Transform playerTrans;
        public Transform eyeViewTrans;
        public float xLimit = 60;

        private float _xRangeOffset = 0;

        private CharacterController _playerCc;
        public float moveSpeed;
        public float gravity;
        public float verticalVelocity;
        public float maxHeight;

        public bool isGrounded = false;
        public Transform groundCheckPoint;
        public LayerMask groundLayer;
        public float checkRadius = 0.6f;

        private HoverbotAnimatorController _animatorController;

        public GameObject weapon;

        private void Start()
        {
            _playerCc = GetComponent<CharacterController>();
            _animatorController = GetComponent<HoverbotAnimatorController>();

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
        }

        private void FixedUpdate()
        {
            PlayerRotateControl();
            PlayerMovement();
            ColliderCheck();
        }

        private void ColliderCheck()
        {
            isGrounded = Physics.CheckSphere(groundCheckPoint.position, checkRadius, groundLayer);
        }

        private void PlayerRotateControl()
        {
            if (!playerTrans || !eyeViewTrans) return;

            // x的偏移量 控制 Player 水平旋转
            // 以世界坐标Y轴为旋转轴
            float offsetX = Input.GetAxis("Mouse X");
            // y的偏移量 控制EyeView 垂直旋转
            // 以X轴为旋转轴
            float offsetY = Input.GetAxis("Mouse Y");
            // Debug.Log($"offsetX: {offsetX}, offsetY: {offsetY}");

            playerTrans.Rotate(Vector3.up * (rotateSpeed * offsetX * rotateRadio * Time.deltaTime));

            _xRangeOffset -= offsetY * rotateSpeed * rotateRadio * Time.deltaTime;
            _xRangeOffset = Mathf.Clamp(_xRangeOffset, -xLimit, xLimit);

            var localEulerAngles = eyeViewTrans.localEulerAngles;
            Quaternion currentLocalRotate = Quaternion.Euler(
                new Vector3(_xRangeOffset, localEulerAngles.y, localEulerAngles.z)
            );
            eyeViewTrans.localRotation = currentLocalRotate;
        }

        private void PlayerMovement()
        {
            if (!_playerCc)
            {
                // Debug.Log("PlayerController: Player CharacterController is null");
                return;
            }

            Vector3 motionValue = Vector3.zero;

            float hInputValue = Input.GetAxis("Horizontal"); // 左右移动
            float vInputValue = Input.GetAxis("Vertical"); // 前后移动

            motionValue += transform.forward * (moveSpeed * vInputValue * moveSpeed * Time.fixedDeltaTime);
            motionValue += transform.right * (moveSpeed * hInputValue * moveSpeed * Time.fixedDeltaTime);
            // Debug.Log($"moveDirection: {moveDirection}");


            // h = 0.5 * gravity * t * t
            if (!isGrounded)
            {
                verticalVelocity += gravity * Time.fixedDeltaTime;
            }
            else if (isGrounded && verticalVelocity < 0)
            {
                verticalVelocity = 0;
            }

            if (isGrounded)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    verticalVelocity = Mathf.Sqrt(2 * maxHeight / (-gravity)) * (-gravity);
                }
            }

            motionValue += Vector3.up * (verticalVelocity * Time.fixedDeltaTime);

            _playerCc.Move(motionValue);

            if (_animatorController)
            {
                _animatorController.moveSpeed = moveSpeed * vInputValue;
                _animatorController.alerted = vInputValue != 0;
            }
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckPoint.position, checkRadius);
        }
    }
}