namespace PlanetaryDeception
{
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    public static class SaveManager
    {
        private static string SavePath
        {
            get { return Path.Combine(Application.persistentDataPath, "savegame.json"); }
        }

        public static SaveData CreateSaveData()
        {
            var settings = CharacterSettings.Instance();
            var inventory = PlayerInventory.Instance();
            var wallet = PlayerWallet.Instance();

            var data = new SaveData
            {
                Name = settings.Name,
                HairStyle = settings.HairStyle,
                Accessory = settings.Accessory,
                Outfit = settings.Outfit,
                SkinColorR = settings.SkinColor.r,
                SkinColorG = settings.SkinColor.g,
                SkinColorB = settings.SkinColor.b,
                HairColorR = settings.HairColor.r,
                HairColorG = settings.HairColor.g,
                HairColorB = settings.HairColor.b,
                AccessoryColorR = settings.AccessoryColor.r,
                AccessoryColorG = settings.AccessoryColor.g,
                AccessoryColorB = settings.AccessoryColor.b,
                WalletAmount = wallet.GetAmount(),
                InventoryItems = inventory.GetItemIds(),
                Scenes = new List<SceneSettingsData>()
            };

            foreach (var kvp in settings.GetKnownScenes())
            {
                data.Scenes.Add(new SceneSettingsData
                {
                    SceneName = kvp.Value.SceneName,
                    PlayerPosIsSet = kvp.Value.PlayerPosIsSet,
                    PlayerPosX = kvp.Value.PlayerPosX,
                    PlayerPosY = kvp.Value.PlayerPosY,
                    PlayerIsFacingRight = kvp.Value.PlayerIsFacingRight
                });
            }

            return data;
        }

        public static void LoadSaveData(SaveData data)
        {
            var settings = CharacterSettings.Instance();
            settings.Name = data.Name;
            settings.HairStyle = data.HairStyle;
            settings.Accessory = data.Accessory;
            settings.Outfit = data.Outfit;
            settings.SkinColor = new Color(data.SkinColorR, data.SkinColorG, data.SkinColorB);
            settings.HairColor = new Color(data.HairColorR, data.HairColorG, data.HairColorB);
            settings.AccessoryColor = new Color(data.AccessoryColorR, data.AccessoryColorG, data.AccessoryColorB);

            var scenes = new Dictionary<string, SceneSettings>();
            foreach (var sceneData in data.Scenes)
            {
                scenes[sceneData.SceneName] = new SceneSettings
                {
                    SceneName = sceneData.SceneName,
                    PlayerPosIsSet = sceneData.PlayerPosIsSet,
                    PlayerPosX = sceneData.PlayerPosX,
                    PlayerPosY = sceneData.PlayerPosY,
                    PlayerIsFacingRight = sceneData.PlayerIsFacingRight
                };
            }
            settings.SetKnownScenes(scenes);

            var wallet = PlayerWallet.Instance();
            wallet.SetAmount(data.WalletAmount);

            var inventory = PlayerInventory.Instance();
            var known = KnownItemsInventory.Instance();
            inventory.Clear();
            foreach (var itemId in data.InventoryItems)
            {
                known.TransferItem(itemId, inventory);
            }
        }

        public static void Save()
        {
            var data = CreateSaveData();
            var json = JsonHelper.ToJson(data);
            File.WriteAllText(SavePath, json);
        }

        public static bool Load()
        {
            if (!File.Exists(SavePath))
            {
                return false;
            }

            var json = File.ReadAllText(SavePath);
            var data = JsonHelper.FromJson<SaveData>(json);
            LoadSaveData(data);
            return true;
        }

        public static bool SaveExists()
        {
            return File.Exists(SavePath);
        }
    }
}
