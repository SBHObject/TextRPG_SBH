using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_SBH
{
    internal class Shop
    {
        private Item[] shopItems = new Item[6];
        public Item[] ShopItems { get { return shopItems; } }
        private bool[] isSold = new bool[6];
        public bool[] IsSold { get { return isSold; } }


        private ItemDatabase itemDb;

        private Character playerChar;

        public Shop(Character _playerChar, ItemDatabase db)
        {
            //캐릭터 생성 후, 객체를 메인에서 가져옴
            itemDb = db;
            playerChar = _playerChar;

            //아이템 데이터베이스에서 상점에 판매할 아이템 정보를 가져옴
            for (int i = 0; i < shopItems.Length; i++)
            {
                shopItems[i] = itemDb.GetItemsByID(i + 2);
                isSold[i] = false;
            }
        }

        //플레이어의 아이템 판매
        public int SellItem(Item soldItem , bool forText = false)
        {
            int itemPrice = (int)(soldItem.ItemPrice * 0.5f);
            //상점에 있는 아이템 판매시, 재입고
            for(int i = 0; i < shopItems.Length; i++)
            {
                if(shopItems[i].ItemId == soldItem.ItemId && forText == false)
                {
                    isSold[i] = false;
                }
            }

            return itemPrice;
        }

        //플레이어의 아이템 구매
        public bool BuyItme(int indexNum)
        {
            //인벤토리가 꽉차있으면 작동하지 않음
            if(playerChar.FullInven())
            {
                return false;
            }

            //돈이 충분하면 돈을 차감하고 아이템 추가
            if (playerChar.UseGold(shopItems[indexNum].ItemPrice))
            {
                playerChar.GetItem(shopItems[indexNum]);
                //상점 아이템 구매시, 매진 표기
                isSold[indexNum] = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        //상점 아이템 정보 출력
        public string ShopItemText(int indexNum)
        {
            string itemText;
            //팔린 아이템이면 매진 출력
            if (isSold[indexNum])
            {
                itemText = "----Sold Out----";
            }
            else
            {
                itemText = "- " + ShopItems[indexNum].InvenItemText() + " | " + ShopItems[indexNum].ItemPrice + " G";
            }

            return itemText;
        }

        public void LoadSoldInfo(List<string> saveData)
        {
            for(int i = 0; i < isSold.Length; i++)
            {
                isSold[i] = bool.Parse(saveData[i + 26]);
            }
        }
    }
}
