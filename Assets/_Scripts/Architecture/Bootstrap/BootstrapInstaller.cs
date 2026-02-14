using UnityEngine;

namespace _Project.Bootstrap
{
    public abstract class BootstrapInstaller : MonoBehaviour
    {
        public abstract void Install(IBuilder builder);
    }
}