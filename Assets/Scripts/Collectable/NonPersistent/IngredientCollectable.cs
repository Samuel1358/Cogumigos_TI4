using UnityEngine;

public class IngredientCollectable : NonPersistentCollectable {
    [SerializeField] private IngredientTypes _ingredientType;
    protected override void OnCollect(PlayerInventory invetoryToStore) {
        invetoryToStore.CollectIngredient(_ingredientType);
        AudioManager.Instance.PlaySFX("Collectable");
    }
}
