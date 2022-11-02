using UnityEngine;
using System;
using System.Collections.Generic;

public class ItemEntriesHandler : Panel
{
    protected List<ItemEntry> entries;

    [SerializeField]
    private ItemEntry entryPrefab;

    [SerializeField]
    private RectTransform entriesParent;

    public event Action<ConfigurableItemData> ItemButtonClicked;

    protected virtual void Awake()
    {
        entries = new List<ItemEntry>();
    }

    ///  ?????????????  re-check stuff about dis
    protected void AddItemEntry(ItemEntry entry)
    {
        entries.Add(entry);
    }

    public void CreateEntries(List<ConfigurableItemData> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            var wheelsEntry = Instantiate(entryPrefab, entriesParent);
            wheelsEntry.Item = items[i];
            wheelsEntry.enabled = true;

            wheelsEntry.ItemButtonClicked += OnWheelsItemButtonClicked;
            AddItemEntry(wheelsEntry); 
        }
    }

    public void DestroyAllEntries()
    {
        for (int i = 0; i < entries.Count; i++)
        {
            ItemEntry entry = entries[i];
            entries[i] = null;
            Destroy(entry.gameObject);
        }
        entries.Clear();
    }

    private void OnWheelsItemButtonClicked(ConfigurableItemData item)
    {
        ItemButtonClicked?.Invoke(item);
    }

    protected void RemoveItemEntry(ConfigurableItemData item)
    {
        ItemEntry entryToDestroy = null;
        int indexToRemoveAt = -1;

        for (int i = 0; i < entries.Count; i++)
        {
            if (entries[i].Item == item)
            {
                indexToRemoveAt = i;
                entryToDestroy = entries[i];
                entries[i] = null;
                break;
            }
        }

        if (entryToDestroy)
        {
            Destroy(entryToDestroy.gameObject);
        }

        if (indexToRemoveAt != -1)
        {
            entries.RemoveAt(indexToRemoveAt);
        }
    }

    public void SelectEntry(ConfigurableItemData itemOnEntry)
    {
        for(int i = 0; i < entries.Count; i++)
        {
            if(entries[i].Item == itemOnEntry)
            {
                entries[i].Select();
            }
        }
    }
}
