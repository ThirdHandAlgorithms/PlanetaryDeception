using System;

namespace PlanetaryDeception
{
    public class ItemException : Exception
    {
        public ItemException(string AMessage, int AItemID) : base(AMessage + " (" + AItemID + ")")
        {

        }
    };

    public enum ItemClassType {
        Trash = 0,
        Identification = 1,
        ProjectileWeapon = 2,
        ParticleWeapon = 3
    };

    public class ItemTag
    {
        public int ItemID { get; set; }
        public ItemClassType ItemClass { get; set; }
        public string ItemName { get; set; }

        public ItemTag(int AItemID, ItemClassType AItemClass, string AItemName)
        {
            ItemID = AItemID;
            ItemClass = AItemClass;
            ItemName = AItemName;
        }

        public ItemTag(ItemTag AItemTag)
        {
            ItemID = AItemTag.ItemID;
            ItemClass = AItemTag.ItemClass;
            ItemName = AItemTag.ItemName;
        }
    };
}
