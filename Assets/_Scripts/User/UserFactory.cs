using System.Linq;
using UnityEngine;

namespace _Project
{
    [CreateAssetMenu(fileName = "UserFactory", menuName = "MySo/UserFactory")]
    public class UserFactory : ScriptableObject
    {
        [SerializeField] private Car[] _cars;
        [SerializeField] private Turret[] _turret;

        public Car CreateCar(PlayerConfig playerConfig)
        {
            Car carPrefab = _cars.FirstOrDefault(_ => _.carType == playerConfig.CarType);
            Turret turretPrefab = _turret.FirstOrDefault(_ => _.turretType == playerConfig.TurretType);

            carPrefab = Instantiate(carPrefab);
            turretPrefab = Instantiate(turretPrefab, carPrefab.turretPoint);
            carPrefab.ChangeTurrety(turretPrefab);
            return carPrefab;
        }
    }
}