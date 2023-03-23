using Interactable;
using Scriptables;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] private ChestDrop drop;
    [SerializeField] private Transform dropPosition;

    public void Interact() => Open();

    private void Open()
    {
        var inst = Instantiate(drop.drops[Random.Range(0, drop.drops.Length)], transform.position, Quaternion.identity);
        inst.transform.position = dropPosition.position;
        Destroy(gameObject);
    }
}