namespace PlanetaryTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PlanetaryDeception;

    [TestClass]
    public class SaveManagerTests
    {
        [TestMethod]
        public void CreateSaveDataCapturesCharacterSettings()
        {
            var settings = CharacterSettings.Instance();
            settings.Name = "TestPlayer";
            settings.HairStyle = 3;
            settings.Outfit = 2;

            var data = SaveManager.CreateSaveData();

            Assert.AreEqual("TestPlayer", data.Name);
            Assert.AreEqual(3, data.HairStyle);
            Assert.AreEqual(2, data.Outfit);
        }

        [TestMethod]
        public void CreateSaveDataCapturesWallet()
        {
            var wallet = PlayerWallet.Instance();

            var data = SaveManager.CreateSaveData();

            Assert.AreEqual(wallet.GetAmount(), data.WalletAmount);
        }

        [TestMethod]
        public void CreateSaveDataCapturesInventory()
        {
            var inventory = PlayerInventory.Instance();
            var known = KnownItemsInventory.Instance();
            known.TransferItem(KnownItem.PlayerSecurityAccessCard, inventory);

            var data = SaveManager.CreateSaveData();

            Assert.IsTrue(data.InventoryItems.Contains(KnownItem.PlayerSecurityAccessCard));
        }

        [TestMethod]
        public void RoundTripSaveLoad()
        {
            var settings = CharacterSettings.Instance();
            settings.Name = "RoundTrip";
            settings.HairStyle = 5;

            var wallet = PlayerWallet.Instance();
            wallet.SetAmount(42);

            var saveData = SaveManager.CreateSaveData();
            var json = JsonHelper.ToJson(saveData);
            var loadedData = JsonHelper.FromJson<SaveData>(json);

            Assert.AreEqual("RoundTrip", loadedData.Name);
            Assert.AreEqual(5, loadedData.HairStyle);
            Assert.AreEqual(42, loadedData.WalletAmount);
        }

        [TestMethod]
        public void LoadSaveDataRestoresState()
        {
            var saveData = new SaveData
            {
                Name = "Loaded",
                HairStyle = 7,
                Accessory = 1,
                Outfit = 4,
                SkinColorR = 0.5f,
                SkinColorG = 0.6f,
                SkinColorB = 0.7f,
                HairColorR = 0.1f,
                HairColorG = 0.2f,
                HairColorB = 0.3f,
                AccessoryColorR = 0.8f,
                AccessoryColorG = 0.9f,
                AccessoryColorB = 1.0f,
                WalletAmount = 300,
                InventoryItems = new System.Collections.Generic.List<KnownItem>
                {
                    KnownItem.PlayerSecurityAccessCard,
                    KnownItem.VenrefInterrogated
                },
                Scenes = new System.Collections.Generic.List<SceneSettingsData>()
            };

            SaveManager.LoadSaveData(saveData);

            var settings = CharacterSettings.Instance();
            Assert.AreEqual("Loaded", settings.Name);
            Assert.AreEqual(7, settings.HairStyle);
            Assert.AreEqual(4, settings.Outfit);

            var wallet = PlayerWallet.Instance();
            Assert.AreEqual(300, wallet.GetAmount());

            var inventory = PlayerInventory.Instance();
            Assert.IsTrue(inventory.ContainsItem(KnownItem.PlayerSecurityAccessCard));
            Assert.IsTrue(inventory.ContainsItem(KnownItem.VenrefInterrogated));
        }
    }
}
