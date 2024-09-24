using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextRPG_SBH
{
    public enum ItemType
    {
        Weapon = 0,
        Armor = 1
    }

    public class Item
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ItemType Type { get; private set; }
        public float ItemAttack {  get; private set; }
        public float ItemDefence {  get; private set; }
        public int ItemId {  get; private set; }
        public int ItemPrice { get; private set; }
        public bool IsEquipped { get; private set; }

        public Item(int _id, string _itemName, string _itemDescription, ItemType _type, int _attck, int _defence, int price)
        {
            ItemId = _id;
            Name = _itemName;
            Description = _itemDescription;
            Type = _type;
            ItemAttack = _attck;
            ItemDefence = _defence;
            ItemPrice = price;
            IsEquipped = false;
        }

        //장비 장착, 장착해제를 당담
        public void EquipThis()
        {
            if(IsEquipped)
            {
                IsEquipped = false;
            }
            else
            {
                IsEquipped= true;
            }
        }

        //자신의 텍스트를 생성
        public string InvenItemText()
        {
            //장비 타입에 따른 스텟표기 변환
            string itemStat;
            if (Type == ItemType.Weapon)
            {
                itemStat = "공격력 +" + ItemAttack;
            }
            else
            {
                itemStat = "방어력 +" + ItemDefence;
            }

            //장착 여부에 따른 표기 변환 확인
            string itemName;
            if (IsEquipped == true)
            {
                itemName = "[E] " + Name;
            }
            else
            {
                itemName = Name;
            }

            itemName = itemName.PadRight(8, ' ');

            string itemText = itemName + " | " + itemStat + " | " + Description + "   ";
            return itemText;
        }
    }

    internal class ItemDatabase
    {
        private List<Item> items = new List<Item>();
        public List<Item> Items {  get { return items; } }

        public ItemDatabase()
        {
            items.Add(new Item(1, "시작의 검", "이 세상에 처음 만들어진 검", ItemType.Weapon, 1, 0, 50));
            items.Add(new Item(2, "조잡한 검", "이름없는 대장장이가 처음 만들어낸 검", ItemType.Weapon, 3, 0, 150));
            items.Add(new Item(3, "천 갑옷", "가볍고 평범한 갑옷", ItemType.Armor, 0, 2, 100));
            items.Add(new Item(4, "롱소드", "평범한 직검", ItemType.Weapon, 8, 0, 700));
            items.Add(new Item(5, "가죽 갑옷", "질긴 가죽으로 만들어진 갑옷", ItemType.Armor, 0, 5, 600));
            items.Add(new Item(6, "츠바이핸더", "높은 파괴력의 양손검", ItemType.Weapon, 20, 0, 2000));
            items.Add(new Item(7, "판금 갑옷", "금속판으로 만들어진 튼튼한 갑옷", ItemType.Armor, 0, 15, 1800));
        }
        
        //아이템 정보를 아이템에 부여된 아이디를 통해 가져옴
        public Item GetItemsByID(int _itemId)
        {
            Item returnItem = items.First();

            foreach(Item item in items)
            {
                if(item.ItemId == _itemId)
                {
                    returnItem = item;
                    break;
                }
            }

            return returnItem;
        }
    }
}
