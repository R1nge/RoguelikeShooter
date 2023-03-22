using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "RarenessColors", menuName = "Rareness Colors")]
    public class RarenessColors : ScriptableObject
    {
        public Color[] colors;
    }
}