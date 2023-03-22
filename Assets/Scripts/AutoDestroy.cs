using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] private float delay;

    private void Awake() => Destroy(gameObject, delay);
}