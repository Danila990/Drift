using UnityEngine;

namespace _Project.UnityServiceLocator
{
    public abstract class BootstrapInstaller : MonoBehaviour
    {
        public abstract void Install(IBuilder builder);
    }
}