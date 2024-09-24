using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextRPG_SBH
{
    public enum Jobs
    {
        Warrior,
        Wizard,
        Theif,
        Archer
    }

    internal class Character
    {
        //플레이어 이름
        public string UserName { get; private set; }

        //플레이어 직업
        public Jobs PlayerJobs { get; private set; }
        public string KorJobName { get; private set; }

        //플레이어의 현재 레벨
        public int Level { get; private set; }

        //초기 공격력
        private float baseAttack;
        //현재 공격력
        public float AttackPoint { get; private set; }

        //초기 방어력
        private float baseDefence;
        //현재 방어력
        public float DefencePoint { get; private set; }

        //초기 체력, 최대체력
        private float baseHealth;
        private float maxHealth;
        public float MaxHealth
        {
            get { return maxHealth; }
        }
        
        //현재 체력
        public float HealthPoint { get; private set; }

        public bool IsAlive { get; private set; }

        //골드
        public int PlayerGold { get; private set; }
        //경험치
        public int Exp { get; private set; }

        //캐릭터 인벤토리(10칸)
        public Item[] inventory = new Item[10];
        //캐릭터 장비 칸
        public Item[] equipSlot = new Item[2];

        //생성자, 게임 시작시 이름과 직업을 고르고 그에 맞게 캐릭터 생성
        public Character()
        {
            UserName = "User";
            KorJobName = string.Empty;

            Level = 1;
            PlayerGold = 100;
            Exp = 0;
            IsAlive = true;
        }

        public void ExpUp(int expAmount)
        {
            Exp += expAmount;
            //내 레벨보다 경험치가 높아지면 레벨업
            if(Exp >= Level)
            {
                //레벨업에 경험치 소모
                Exp -= Level;
                LevelUp();
            }
        }

        public void LevelUp()
        {
            Level ++;

            baseAttack += 0.5f;
            baseDefence += 1f;
        }

        public void SetBaseStatus(string name, Jobs job)
        {
            UserName = name;

            switch (job)
            {
                case Jobs.Warrior:
                    baseAttack = 10;
                    baseDefence = 7;
                    baseHealth = 120;
                    KorJobName = "전사";
                    break;
                case Jobs.Wizard:
                    baseAttack = 17;
                    baseDefence = 4;
                    baseHealth = 80;
                    KorJobName = "마법사";
                    break;
                case Jobs.Theif:
                    baseAttack = 13;
                    baseDefence = 6;
                    baseHealth = 100;
                    KorJobName = "도적";
                    break;
                case Jobs.Archer:
                    baseAttack = 15;
                    baseDefence = 5;
                    baseHealth = 90;
                    KorJobName = "궁수";
                    break;
            }

            AttackPoint = baseAttack;
            DefencePoint = baseDefence;
            maxHealth = baseHealth;
            HealthPoint = baseHealth;
        }

        //인벤토리에 아이템 획득
        public void GetItem(Item item)
        {
            //아이템 인벤토리의 빈칸에 아이템 등록
            for(int i = 0; i < inventory.Length; i++)
            {
                //
                if (inventory[i] == null)
                {
                    inventory[i] = item;
                    break;
                }

                //만약, 빈칸이 없을경우(빈칸이 있으면 break를 통해 for문을 종료함)
                if(i == inventory.Length)
                {
                    Console.WriteLine("인벤토리가 가득차 아이템 획득에 실패했습니다.");
                }
            }
        }

        //인벤토리에서 아이템 삭제
        public void RemoveItem(int index)
        {
            //비어있는 슬롯 삭제 시도시 아무것도 하지 않음
            if (inventory[index] == null)
            {
                return;
            }
            //장착중인 아이템 삭제 시도시 아무것도 하지 않음
            if (inventory[index].IsEquipped)
            {
                return;
            }

            inventory[index] = null;
        }

        //아이템 장착 함수
        public void EquipItem(Item item)
        {
            //아이템 타입이 무기일경우
            if (item.Type == ItemType.Weapon)
            {
                //장비칸 배열 0번에 등록
                if (equipSlot[0] == null)
                {
                    equipSlot[0] = item;
                    AttackPoint += equipSlot[0].ItemAttack;
                }
                else
                {
                    //0번 자리가 차있을경우 아이템 교체
                    AttackPoint -= equipSlot[0].ItemAttack;
                    equipSlot[0] = item;
                    AttackPoint += equipSlot[0].ItemAttack;
                }
            }
            //아이템 타입이 방어구일경우
            else
            {
                //장비칸 배열 1번에 등록
                if (equipSlot[1] == null)
                {
                    equipSlot[1] = item;
                    DefencePoint += equipSlot[1].ItemDefence;
                }
                else
                {
                    //1번 자리가 차있을경우 아이템 교체
                    DefencePoint -= equipSlot[1].ItemDefence;
                    equipSlot[0] = item;
                    DefencePoint += equipSlot[1].ItemDefence;
                }
            }

            //호출되면, 장착혹은 해제
            item.EquipThis();
        }

        //아이템 장착 해제
        public void UnequipItem(ItemType type)
        {
            if(type == ItemType.Weapon)
            {
                AttackPoint -= equipSlot[0].ItemAttack;
                equipSlot[0].EquipThis();
                equipSlot[0] = null;
            }
            else
            {
                DefencePoint -= equipSlot[1].ItemDefence;
                equipSlot[0].EquipThis();
                equipSlot[1] = null;
            }
        }

        //인벤토리가 꽉찬 상태인지 여부 -> true면 꽉찬 상태
        public bool FullInven()
        {
            foreach(Item item in inventory)
            {
                if(item == null)
                {
                    return false;
                } 
            }

            return true;
        }

        //돈 사용
        public bool UseGold(int usedGold)
        {
            if (PlayerGold < usedGold)
            {
                return false;
            }
            else
            {
                PlayerGold -= usedGold;
                return true;
            }
        }
        
        //돈 획득
        public void GetGold(int gold)
        {
            PlayerGold += gold;
        }

        //체력 잃음, 사망판정
        public void LoseHealth(float health)
        {
            HealthPoint -= health;
            if(HealthPoint <= 0)
            {
                HealthPoint = 0;
                IsAlive = false;
            }
        }

        //체력 얻음, 최대치를 넘지 못함
        public void HealedHealth(float health)
        {
            float temp = HealthPoint + health;
            HealthPoint = (temp > maxHealth) ? maxHealth : temp;
        }

        public void LoadPlayerData(List<string> saveData)
        {
            ItemDatabase itemData = new ItemDatabase();
            string name = saveData[0];
            Jobs job;
            switch (saveData[1])
            {
                case "Warrior":
                    job = Jobs.Warrior;
                    break;
                case "Wizard":
                    job = Jobs.Wizard;
                    break;
                case "Theif":
                    job = Jobs.Theif;
                    break;
                case "Archer":
                    job = Jobs.Archer;
                    break;
                default:
                    job = Jobs.Warrior;
                    break;
            }
            //이름, 직업을 불러오면서 기본 스텟 세팅
            SetBaseStatus(name, job);
            //레벨 세팅과 동시에 스텟 지정
            Level = int.Parse(saveData[2]);
            baseAttack += (Level - 1) * 0.5f;
            baseDefence += (Level - 1) * 1f;
            HealthPoint = float.Parse(saveData[3]);
            Exp = int.Parse(saveData[4]);
            PlayerGold = int.Parse(saveData[5]);
            //인벤토리 불러오기
            for(int i = 0; i < inventory.Length; i++)
            {
                if (int.Parse(saveData[i + 6]) == -1)
                {
                    inventory[i] = null;
                }
                else
                {
                    //i+6 번부터 인벤토리 아이템 아이디 저장
                    inventory[i] = itemData.GetItemsByID(int.Parse(saveData[i + 6]));
                }
            }

            //아이템 장착
            for(int j = 0; j < inventory.Length; j++)
            {
                if (int.Parse(saveData[j + 16]) != -1)
                {
                    EquipItem(inventory[j]);
                }
            }
        }

    }
}
