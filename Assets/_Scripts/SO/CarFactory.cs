using System.Linq;
using UnityEngine;

namespace _Project
{
    [CreateAssetMenu(fileName = "CarFactory", menuName = "MySo/CarFactory")]
    public class CarFactory : ScriptableObject
    {
        [SerializeField] private Car[] _cars;
        [SerializeField] private Turret[] _turret;

        public Car CreateCar(CarConfig carConfig)
        {
            Car carPrefab = _cars.FirstOrDefault(_ => _.carType == carConfig.CarType);
            Turret turretPrefab = _turret.FirstOrDefault(_ => _.turretType == carConfig.TurretType);

            carPrefab = Instantiate(carPrefab);
            turretPrefab = Instantiate(turretPrefab, carPrefab.turretPoint);
            carPrefab.ChangeTurrety(turretPrefab);
            return carPrefab;
        }
    }
}