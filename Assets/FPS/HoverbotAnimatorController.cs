using System;
using UnityEngine;

namespace Vanks.FPS
{
    public class HoverbotAnimatorController : MonoBehaviour
    {
        public float moveSpeed;
        public bool alerted;
        public bool death;

        public Animator animator;

        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            SetParameter();
        }

        private void SetParameter()
        {
            if (!animator) return;
            animator.SetFloat("MoveSpeed", moveSpeed);
            animator.SetBool("Alerted", alerted);
            animator.SetBool("Death", death);
        }

        public void TriggerAttack()
        {
            if (!animator) return;
            animator.SetTrigger("Attack");
        }

        public void TriggerOnDamage()
        {
            if (!animator) return;
            animator.SetTrigger("OnDamage");
        }
    }
}