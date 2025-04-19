using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, IResetable {

    public List<KeyTypes> Keys { get; private set; }
    public List<IngredientTypes> Ingredients { get; private set; }

    private List<KeyTypes> _keysHoldedAtCheckpoint;
    private List<IngredientTypes> _ingredientsHoldedAtCheckpoint;

    public void Initialize() {
        RespawnController.OnPlayerChangeCheckPoint += SaveResetState;
        RespawnController.Instance.TurnResetable(this);
    }

    private void OnDisable() {
        RespawnController.OnPlayerChangeCheckPoint -= SaveResetState;
        RespawnController.Instance.TurnNonResetable(this);
    }

    private void Awake() {
        Keys = new List<KeyTypes>();
        _keysHoldedAtCheckpoint = new List<KeyTypes>();

        Ingredients = new List<IngredientTypes>();
        _ingredientsHoldedAtCheckpoint = new List<IngredientTypes>();
    }

    public void CollectKey(KeyTypes keyCollected) {
        Keys.Add(keyCollected);
    }

    public bool VerifyKey(KeyTypes keyToVerify) {
        return Keys.Contains(keyToVerify);
    }

    public bool VerifyKeys(List<KeyTypes> keysToVerify) {
        int keyCount = 0;
        foreach (KeyTypes key in keysToVerify) {
            if (Keys.Contains(key)) {
                keyCount++;
            }
        }
        return keyCount == keysToVerify.Count && keyCount > 0;
    }

    public void CollectIngredient(IngredientTypes ingredientCollected) {
        Ingredients.Add(ingredientCollected);
    }

    public bool VerifyIngredient(IngredientTypes ingredientToVerify) {
        return Ingredients.Contains(ingredientToVerify);
    }

    public bool VerifyIngredients(List<IngredientTypes> ingredientsToVerify) {
        int ingredientCount = 0;
        foreach (IngredientTypes ingredient in ingredientsToVerify) {
            if (Ingredients.Contains(ingredient)) {
                ingredientCount++;
            }
        }
        return ingredientCount == ingredientsToVerify.Count && ingredientCount > 0;
    }

    public void SaveResetState(Checkpoint checkpoint) {
        _keysHoldedAtCheckpoint = Keys;
        _ingredientsHoldedAtCheckpoint = Ingredients;
    }

    public void ResetObject() {
        Keys = _keysHoldedAtCheckpoint;
        Ingredients = _ingredientsHoldedAtCheckpoint;
    }
}
