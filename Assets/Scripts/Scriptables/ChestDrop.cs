using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "ChestDrop", menuName = "Chest Drop")]
    public class ChestDrop : ScriptableObject
    {
        public GameObject[] drops;
    }
}