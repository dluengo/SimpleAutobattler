using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeightedItem<T> where T : class
{
    public T item;
    public float weight = 1f;

    public WeightedItem(T item, float weight)
    { 
        this.item = item;
        this.weight = weight;
    }
}

// This class represents a list of list where each item has an associated weight.
// It is useful when you want to randomly select an item from the list, but you want
// some list to be more likely to be selected than others based on their weights.
[Serializable]
public class WeightedList<T> : List<WeightedItem<T>> where T : class
{
    // --- Members ---
    private float totalWeight;


    // --- Methods ---
    // NOTE: This method should be called from Awake/OnEnable/Start if the list is
    // edited in the inspector. Otherwise the totalWeight is not properly set.
    public void UpdateWeights()
    {
        totalWeight = 0f;
        foreach (WeightedItem<T> element in this) {
            totalWeight += element.weight;
        }
    }

    public void Add(T item, float weight)
    {
        if (weight < 0f) {
            Debug.LogWarning("Weight cannot be negative. m_item not added.");
            return;
        }

        WeightedItem<T> newElement = new WeightedItem<T>(item, weight);
        base.Add(newElement);
        totalWeight += weight;
    }

    public T GetRandomItem()
    {
        if (totalWeight == 0f) {
            Debug.LogWarning("Total weight is zero. Cannot select an item.");
            return default;
        }

        if (this.Count == 0) {
            Debug.LogWarning("WeightedList is empty. Cannot select an item.");
            return default;
        }

        // The way we select a random item based on weights is by generating
        // a random number between 0 and the sum of all weights in the list.
        // Then we iterate through the list, keeping a cumulative sum of the
        // weights. When the cumulative sum exceeds the random number we
        // generated, we return the corresponding item.
        //
        // NOTE: Assuming UnityEngine.Random.Range() generates numbers uniformly.
        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;
        foreach (WeightedItem<T> element in this) {
            cumulativeWeight += element.weight;
            if (randomValue < cumulativeWeight) {
                return element.item;
            }
        }

        // Fallback in case of rounding errors
        return this[this.Count - 1].item;
    }
}
