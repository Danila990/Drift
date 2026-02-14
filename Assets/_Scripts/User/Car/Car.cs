using UnityEngine;

namespace _Project
{
    public class Car : MonoBehaviour
    {
        private Turret _turret;

        [field: SerializeField] public CarType carType { get; private set; }
        [field: SerializeField] public Transform turretPoint { get; private set; }
        [field: SerializeField] public Health healthCar { get; private set; }

        public void ChangeTurrety(Turret turret)
        {
            _turret = turret;
        }
    }
}