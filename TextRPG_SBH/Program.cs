using System.ComponentModel.Design;

namespace TextRPG_SBH
{
    internal class Program
    {
        public static Character playerChar = new Character();
        public static ItemDatabase itemDb = new ItemDatabase();
        public static DungeonDatabase dungeonDb = new DungeonDatabase();
        public static Shop shop = new Shop(playerChar, itemDb);

        public static GameSaveManager saveManager = new GameSaveManager(playerChar, shop);
        
        static void Main()
        {
            string inputName;
            int inputSelectedJop;

            bool isCharCreated = false;

            Console.WriteLine("저장된 데이터를 검색합니다.");
            if (saveManager.LoadGame())
            {
                Console.WriteLine("저장된 데이터가 존재합니다.");
                Console.WriteLine("데이터를 불러오겠습니까?");
                Console.WriteLine();
                Console.WriteLine("1. 예\n2. 아니오 (2이외의 입력시 데이터를 불러옵니다.)");
                if (InputInt(Console.ReadLine()) == 2)
                {
                    Console.WriteLine("새로운 캐릭터를 생성합니다.");
                    Thread.Sleep(1000);
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("데이터를 불러옵니다.");
                    playerChar.LoadPlayerData(saveManager.SaveDataList);
                    Console.WriteLine("불러오기 성공");
                    Thread.Sleep(1000);
                    isCharCreated = true;
                }
            }
            else
            {
                Console.WriteLine("저장된 데이터가 없습니다. 새로운 캐릭터를 생성합니다.");
                Thread.Sleep(1000);
                Console.Clear();

            }

            while (!isCharCreated)
            {
                Console.WriteLine("당신의 이름은?");
                inputName = Console.ReadLine();

                if (inputName == null)
                {
                    inputName = "User";
                }

                Console.Clear();
                Console.WriteLine("직업을 고르세요");
                Console.WriteLine("1. 전사    2. 마법사    3. 도적    4. 궁수    5. 직업설명");
                inputSelectedJop = InputInt(Console.ReadLine());

                switch (inputSelectedJop - 1)
                {
                    case 0:
                        playerChar.SetBaseStatus(inputName, Jobs.Warrior);
                        isCharCreated = true;
                        break;
                    case 1:
                        playerChar.SetBaseStatus(inputName, Jobs.Wizard);
                        isCharCreated = true;
                        break;
                    case 2:
                        playerChar.SetBaseStatus(inputName, Jobs.Theif);
                        isCharCreated = true;
                        break;
                    case 3:
                        playerChar.SetBaseStatus(inputName, Jobs.Archer);
                        isCharCreated = true;
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine("전사\n초기 체력 : 120 \n초기 공격력 : 10\n초기 방어력 : 7\n매우 높은 생존력");
                        Console.WriteLine();
                        Console.WriteLine("마법사\n초기 체력 : 80 \n초기 공격력 : 17\n초기 방어력 : 4\n매우 낮은 생존력과 매우 높은 공격력");
                        Console.WriteLine();
                        Console.WriteLine("도적\n초기 체력 : 100 \n초기 공격력 : 13\n초기 방어력 : 6\n평균적인 능력치");
                        Console.WriteLine();
                        Console.WriteLine("궁수\n초기 체력 : 90 \n초기 공격력 : 15\n초기 방어력 : 5\n낮은 생존력과 높은 공격력");
                        Console.WriteLine();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }

                if(isCharCreated == true)
                {
                    int isRight = 0;
                    while (isRight != 1)
                    {
                        Console.Clear();
                        Console.WriteLine($"당신의 이름 : {inputName}");
                        Console.WriteLine($"당신의 직업 : {playerChar.KorJobName}");

                        Console.WriteLine("이 내용이 맞습니까?");
                        Console.WriteLine("1. 예    2. 아니오");
                        isRight = InputInt(Console.ReadLine());
                        Console.Clear();

                        if (isRight == 1)
                        {
                            break;
                        }
                        else if (isRight == 2)
                        {
                            isCharCreated = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("잘못된 입력입니다.");
                            Thread.Sleep(500);
                        }
                    }
                }
            }

            TextCreater("게임 시작   ");
            
            Console.Clear();
            int input;

            bool wantToEnd = false;

            //본 게임
            while (playerChar.IsAlive)
            {
                TextCreater("마을에 오신것을 환영합니다.\n어떤 행동을 할지 선택해주세요.\n\n");
                TextCreater("1.상태창\n2.인벤토리\n3.상점\n4.던전입장\n5.휴식하기\n6.저장\n7.게임 종료\n\n");

                TextCreater("행동 입력");
                input = InputInt(Console.ReadLine());

                switch (input)
                {
                    case 1:
                        {
                            ViewStatus();
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        }

                    case 2:
                        {
                            ViewInven();
                            Console.WriteLine("1. 아이템 장착 관리\n0. 나가기");
                            if (InputInt(Console.ReadLine()) == 1)
                            {
                                LineClear(4);

                                EquipOrder();
                            }
                            else
                            {
                                Console.Clear();
                            }
                            break;
                        }
                    case 3:
                        {
                            //상점 반복
                            while (true)
                            {
                                int resetPos = OpenShop();
                                Console.WriteLine();
                                Console.WriteLine("1.아이템 구매\n2.아이템 판매\n0.나가기");
                                input = InputInt(Console.ReadLine());
                                if (input == 1)
                                {
                                    ResetShop(resetPos);
                                }
                                else if (input == 2)
                                {
                                    SellItem(resetPos);
                                }
                                else
                                {
                                    Console.Clear();
                                    break;
                                }
                            }
                            break;
                        }
                    case 4:
                        {
                            //던전 선택 기능
                            DungeonSelecter();
                            
                            break;
                        }
                    case 5:
                        {
                            //휴식기능
                            InnSelecter();

                            break;
                        }
                    case 6:
                        {
                            //저장
                            Console.Clear();
                            TextCreater("저장하시겠습니까?");
                            Console.WriteLine();
                            TextCreater("1. 예\n0. 아니오");
                            input = InputInt(Console.ReadLine());
                            if(input == 1)
                            {
                                SaveGame();
                            }
                            else
                            {
                                Console.Clear();
                            }
                            break;
                        }
                    case 7:
                        {
                            Console.Clear();
                            //게임 종료
                            TextCreater("종료하시겠습니까?");
                            Console.WriteLine();
                            TextCreater("1. 예\n0. 아니오");
                            input = InputInt(Console.ReadLine());
                            {
                                Console.Clear();
                                if(input == 1)
                                {
                                    wantToEnd = true;
                                    TextCreater("저장하시겠습니까?");
                                    Console.WriteLine();
                                    TextCreater("1. 예\n0. 아니오");
                                    input = InputInt(Console.ReadLine());
                                    if (input == 1)
                                    {
                                        SaveGame();
                                    }
                                    else
                                    {
                                        TextCreater("저장하지 않고 게임을 종료합니다");
                                        Thread.Sleep(1000);
                                        Console.Clear();
                                    }
                                    break;
                                }
                                else
                                {
                                    Console.Clear();
                                }
                            }
                            break;
                        }
                    default:
                        {
                            Console.Clear();
                            TextCreater("잘못된 입력입니다.");
                            break;
                        }
                }

                if(wantToEnd == true)
                {
                    break;
                }
            }
        }

        //ReadLine 을 통해 int를 받을때 사용하는 함수
        //int 형이면 해당값 반환, 아니면 -1 반환
        private static int InputInt(string input)
        {
            int returnInt;

            if(int.TryParse(input, out returnInt))
            {
                return returnInt;
            }

            else
            {
                return -1;
            }
        }

        private static void TextCreater(string text, bool changeLine = true)
        {
            for(int i = 0; i < text.Length; i++)
            {
                Console.Write(text[i]);
                Thread.Sleep(10);
            }

            if (changeLine == true)
            {
                Console.WriteLine();
            }
        }

        //스텟을 보여주는 함수
        private static void ViewStatus()
        {
            Console.Clear();
            TextCreater("[상태창]\n\n");
            TextCreater($"Lv.{playerChar.Level} / 경험치 : {playerChar.Exp}");
            TextCreater($"{playerChar.UserName} ({playerChar.KorJobName})");
            TextCreater($"공격력 : {playerChar.AttackPoint}");
            TextCreater($"방어력 : {playerChar.DefencePoint}");
            TextCreater($"체력 : {playerChar.HealthPoint}/{playerChar.MaxHealth}");
            TextCreater($"Gold : {playerChar.PlayerGold}");
            Console.WriteLine();

            TextCreater("아무거나 입력하여 나가기");
        }

        //인벤토리를 보여주는 함수
        private static void ViewInven()
        {
            Console.Clear();
            TextCreater("[인벤토리]\n");
            TextCreater("보유 중인 아이템을 관리할 수 있습니다.\n");
            
            for(int i = 0; i < playerChar.inventory.Length; i++)
            {
                if (playerChar.inventory[i] == null)
                {
                    string itemText = "-" + (i + 1) + " ";
                    TextCreater(itemText);
                }
                else
                {
                    string itemText = "-" + (i + 1) + " " + playerChar.inventory[i].InvenItemText();
                    TextCreater(itemText);
                }
            }
        }

        //아이템 장착 요청
        private static void EquipOrder()
        {
            int input;
            while (true)
            {
                TextCreater("장착 혹은 장착 해제할 아이템 입력 (이외의 입력시 종료) : ", false);
                input = InputInt(Console.ReadLine());
                if (input >= 1 && input <= 10)
                {
                    //해당 위치의 인벤토리배열에 아이템이 있을때
                    if (playerChar.inventory[input - 1] != null)
                    {
                        //같은 슬롯에 장착아이템이 있으면 해제, 없으면 장착
                        if (playerChar.inventory[input - 1].IsEquipped == true)
                        {
                            playerChar.UnequipItem(playerChar.inventory[input - 1].Type);
                        }
                        else
                        {
                            playerChar.EquipItem(playerChar.inventory[input - 1]);
                        }

                        //마지막라인 위치 저장
                        int lastLine = Console.GetCursorPosition().Top;
                        //장착을 시도한 위치로 커서 이동, 아이템 설명에 [E] 여부 변경
                        Console.SetCursorPosition(0, input + 3);
                        Console.Write("-" + input + " " + playerChar.inventory[input - 1].InvenItemText());
                        
                        Console.SetCursorPosition(0, lastLine);

                        LineClear(2);
                    }
                    else
                    {
                        TextCreater("아이템이 없습니다.   ", false);
                        LineClear(2);
                    }
                }
                else
                {
                    Console.Clear();
                    break;
                }
            }
        }

        private static int OpenShop()
        {
            Console.Clear();
            TextCreater("[상점]");
            TextCreater("아이템을 사거나 팔 수 있습니다.");
            Console.WriteLine();
            //보유골드와, 아이템 정보를 수정하는 시작점
            int resetPosition = Console.GetCursorPosition().Top;

            string moneyText = "보유골드 : " + playerChar.PlayerGold + " G";
            TextCreater(moneyText);
            Console.WriteLine();

            TextCreater("판매 아이템 목록");
            for(int i = 0; i < shop.ShopItems.Length; i++)
            {
                TextCreater(shop.ShopItemText(i));
            }

            //시작점 반환
            return resetPosition;
        }

        //아이템 구매시 사용되는 함수
        private static void ResetShop(int resetPos)
        {
            while (true)
            {
                //상점 정보 초기화 시작지점을 받아온뒤, 그 아래의 텍스트를 모두 지움
                LineClear(Console.GetCursorPosition().Top - resetPos);
                Console.SetCursorPosition(0, resetPos);
                //골드 표기 새로고침
                string moneyText = "보유골드 : " + playerChar.PlayerGold + " G";
                Console.WriteLine(moneyText);
                Console.WriteLine();
                
                //판매 아이템 목록 새로고침
                Console.WriteLine("판매 아이템 목록");
                for (int i = 0; i < shop.ShopItems.Length; i++)
                {
                    //상점용 텍스트를 Shop 클래스에서 가져옴
                    Console.WriteLine((i + 1) + "." + shop.ShopItemText(i));
                }

                Console.WriteLine();

                TextCreater("구매할 아이템 번호 입력 (이외 입력시 종료)");
                int input = InputInt(Console.ReadLine());
                if (input <= 6 && input >= 1)
                {
                    if(shop.BuyItme(input - 1))
                    {
                        TextCreater("---구매 완료---");
                    }
                    else
                    {
                        TextCreater("돈이 부족합니다.");
                        Thread.Sleep(100);
                    }
                }
                else
                {
                    break;
                }
            }
        }

        //아이템 판매시 사용되는 함수
        private static void SellItem(int resetPos)
        {
            while(true)
            {
                //상점 정보 출력 초기화용
                LineClear(Console.GetCursorPosition().Top - resetPos);
                Console.SetCursorPosition(0, resetPos);
                //골드 새로고침
                string moneyText = "보유골드 : " + playerChar.PlayerGold + " G";
                Console.WriteLine(moneyText);
                Console.WriteLine();

                TextCreater("인벤토리 아이템 목록");
                //인벤토리 아이템 목록 출력, 없으면 번호만 출력함
                for(int i = 0; i < playerChar.inventory.Length; i++)
                {
                    string itemText;
                    if (playerChar.inventory[i] != null)
                    {
                        itemText = (i + 1) + ". " + playerChar.inventory[i].InvenItemText() + " | " + shop.SellItem(playerChar.inventory[i], true) + " G";
                    }
                    else
                    {
                        itemText = (i + 1) + ". ";
                    }
                    Console.WriteLine(itemText);
                }
                Console.WriteLine();
                Console.WriteLine("판매할 아이템의 번호를 입력해주세요.(이외 입력시 종료)");
                int input = InputInt(Console.ReadLine());
                if(input>= 1 && input <= 10)
                {
                    //선택한 인벤토리 슬롯이 비어있으면 판매 실패
                    if (playerChar.inventory[input - 1] != null)
                    {
                        //아이템이 장착중이면 판매되지 않음
                        if (playerChar.inventory[input - 1].IsEquipped == false)
                        {
                            playerChar.GetGold(shop.SellItem(playerChar.inventory[input - 1]));
                            playerChar.RemoveItem(input - 1);
                            TextCreater("판매 성공");
                            Thread.Sleep(200);
                        }
                        else
                        {
                            TextCreater("장착중인 아이템은 판매할 수 없습니다.");
                            Thread.Sleep(200);
                        }
                    }
                    else
                    {
                        TextCreater("선택한 슬롯이 비어있습니다.");
                    }
                }
                else
                {
                    break;
                }
            }
        }

        //던전 함수
        public static void DungeonSelecter()
        {
            Console.Clear();
            TextCreater("[던전 입구]");
            TextCreater("입장할 던전을 선택할 수 있습니다.");
            Console.WriteLine();

            int doungeonCount = dungeonDb.Dungeons.Count;
            for(int i = 0; i < dungeonDb.Dungeons.Count; i++)
            {
                string text = (i + 1) + ". " + dungeonDb.Dungeons[i].DungeonInfoText();
                TextCreater(text);
            }
            TextCreater("0. 나가기");
            Console.WriteLine();

            TextCreater("원하시는 행동을 입력해주세요.");
            int input = InputInt(Console.ReadLine());
            if(input >= 1 && input <= doungeonCount)
            {
                //던전 플레이
                PlayDungeon(input);
            }
            else
            {
                Console.Clear();
            }
        }

        public static void PlayDungeon(int input)
        {
            Console.Clear();
            int reward;
            int usedHp;
            bool isSucces = dungeonDb.Dungeons[input - 1].PlayDungeon(playerChar.DefencePoint, playerChar.AttackPoint, out reward, out usedHp);
            if(isSucces)
            {
                TextCreater("[공략 성공]");
                string outText = dungeonDb.Dungeons[input - 1].DungeonName +"을 클리어 하였습니다.";
                TextCreater(outText);
                Console.WriteLine();

                TextCreater("[탐험 결과]");
                string resultText = "체력 " + playerChar.HealthPoint + " -> " + (playerChar.HealthPoint - usedHp);
                TextCreater(resultText);
                resultText = "Gold " + playerChar.PlayerGold + " G -> " + (playerChar.PlayerGold + reward) + " G";
                TextCreater(resultText);

                //임시 : 공략 성공시 경험치 1 증가
                playerChar.ExpUp(1);
            }
            else
            {
                TextCreater("[공략 실패]");
                string outText = dungeonDb.Dungeons[input - 1].DungeonName + "공략에 실패하였습니다.";
                TextCreater(outText);
                Console.WriteLine();

                TextCreater("[탐험 결과]");
                string resultText = "체력 " + playerChar.HealthPoint + " -> " + (playerChar.HealthPoint - usedHp);
                TextCreater(resultText);
            }

            Console.ReadLine();

            playerChar.GetGold(reward);
            playerChar.LoseHealth(usedHp);
            if(playerChar.IsAlive)
            {
                DungeonSelecter();
            }
            else
            {
                GameOver();
            }
        }

        //휴식기능 함수
        private static void InnSelecter()
        {
            Console.Clear();
            TextCreater("[여관]");
            TextCreater("골드를 사용하여 체력을 회복할 수 있습니다.");
            Console.WriteLine();

            string text = "500G 를 사용하여 체력 50을 회복합니다." + "(소지금 : " + playerChar.PlayerGold + "G)";
            TextCreater(text);

            Console.WriteLine();
            TextCreater("1. 휴식하기\n0. 나가기");
            Console.WriteLine();

            TextCreater("행동을 입력해주세요");

            if (InputInt(Console.ReadLine()) == 1)
            {
                Console.Clear();
                TextCreater("[휴식하기]");
                float health = playerChar.HealthPoint;
                playerChar.HealedHealth(50);
                Console.Write("Z");
                Thread.Sleep(1000);
                Console.Write("Z");
                Thread.Sleep(1000);
                Console.Write("Z");
                Thread.Sleep(1000);

                Console.Clear();
                TextCreater("[회복 완료]");
                TextCreater("충분한 휴식으로 체력이 회복되었습니다.");
                Console.WriteLine();

                text = "체력 : " + health + " -> " + playerChar.HealthPoint;
                TextCreater(text);
                Console.ReadLine();
                Console.Clear();
            }
            else
            {
                Console.Clear();
            }

        }

        //게임오버 연출
        private static void GameOver()
        {
            Console.Clear();
            Console.Write("게");
            Thread.Sleep(100);
            Console.Write(" 임");
            Thread.Sleep(100);
            Console.Write(" 오");
            Thread.Sleep(100);
            Console.Write(" 버");

            Thread.Sleep(100);
            Console.WriteLine();
            TextCreater("당신은 사망하였습니다...");
        }

        //콘솔 라인 삭제 - 1이면 자신라인
        private static void LineClear(int line)
        {
            Console.WriteLine();
            for(int i = 0; i < line; i++)
            {
                Console.SetCursorPosition(0, Console.GetCursorPosition().Top - 1);
                Console.Write("\r                                                                                \r");
            }
        }

        private static void SaveGame()
        {
            saveManager.SaveGame();
            TextCreater("저장이 완료되었습니다");
            Thread.Sleep(1000);
            Console.Clear();
        }
    }
}
