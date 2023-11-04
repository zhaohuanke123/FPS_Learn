using System;
using UnityEngine;
using UnityEngine.UI;

namespace Vanks.FPS
{
    public class HealthController : MonoBehaviour
    {
        private float _maxHp = 100;
        public float hp = 100;
        public Slider hpSlider;

        private void Start()
        {
            hp = _maxHp;
            if (hpSlider)
            {
                hpSlider.minValue = 0;
                hpSlider.maxValue = _maxHp;
                hpSlider.value = hp;
            }
        }

        public void Damage(float damage)
        {
            if (hp > 0)
            {
                hp -= damage;
                hpSlider.value = hp;
            }
            else
            {
            }
        }
    }
}