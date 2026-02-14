using _Project.Bootstrap;
using UnityEngine;
using UnityEngine.UI;

namespace _Project
{
    public class SpeedView : MonoBehaviour
    {
        [SerializeField] private Text _speedText;

        [Inject] private CarEngine _car;


        private void Update()
        {
            _speedText.text = _car.Speed.ToString();
        }
    }
}
