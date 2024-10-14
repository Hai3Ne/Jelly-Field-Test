using UnityEngine;
using UnityEngine.UI;

namespace Runtime.View
{
    public class GridSlot : MonoBehaviour
    {
        [SerializeField] private Image imgSlot;

        public void SetData(bool isLock)
        {
            imgSlot.enabled = !isLock;
        }
    }
}
