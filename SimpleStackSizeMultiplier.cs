using System;
using System.Collections.Generic;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("SimpleStackSizeMultiplier", "jlee2834", "1.0.0")]
    [Description("Doubles stack size for all items except guns, clothing, and attachments.")]

    class SimpleStackSizeMultiplier : CovalencePlugin
    {
        private readonly HashSet<ItemCategory> _excludedCategories = new HashSet<ItemCategory>
        {
            ItemCategory.Weapon,
            ItemCategory.Attire,
            ItemCategory.Misc // Attachments and mods
        };

        private Dictionary<string, int> _originalStacks;

        private void Init()
        {
            SaveOriginalStackSizes();
            ApplyStackMultipliers();
            Puts("SimpleStackSizeMultiplier loaded: Stack sizes doubled (excluding weapons, clothing, and attachments).");
        }

        private void Unload()
        {
            if (_originalStacks != null)
            {
                foreach (var itemDef in ItemManager.itemList)
                {
                    if (_originalStacks.TryGetValue(itemDef.shortname, out var originalStack))
                    {
                        itemDef.stackable = originalStack;
                    }
                }
                Puts("SimpleStackSizeMultiplier unloaded: Stack sizes reverted.");
            }
        }

        private void SaveOriginalStackSizes()
        {
            _originalStacks = new Dictionary<string, int>();
            foreach (var item in ItemManager.itemList)
            {
                _originalStacks[item.shortname] = item.stackable;
            }
        }

        private void ApplyStackMultipliers()
        {
            foreach (var item in ItemManager.itemList)
            {
                if (_excludedCategories.Contains(item.category))
                    continue;

                item.stackable = Mathf.Clamp(item.stackable * 2, 1, int.MaxValue);
            }
        }
    }
}
