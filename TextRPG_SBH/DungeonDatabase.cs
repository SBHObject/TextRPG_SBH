using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_SBH
{
    internal class DungeonDatabase
    {
        private List<Dungeon> dungeons = new List<Dungeon>();
        public List<Dungeon> Dungeons { get { return dungeons; } }

        public DungeonDatabase()
        {
            dungeons.Add(new Dungeon("초급 던전", 5, 10, 1000));
            dungeons.Add(new Dungeon("중급 던전", 13, 25, 1700));
            dungeons.Add(new Dungeon("상급 던전", 21, 32, 2500));
        }
    }

    internal class Dungeon
    {
        public string DungeonName { get; private set; }
        public int RequireDef { get; private set; }
        public int AddtinalRewardAtk { get; private set; }
        private int rewardGold;

        public Dungeon(string name, int requiredDef, int requiredAttack, int _rewardGold)
        {
            DungeonName = name;
            RequireDef = requiredDef;
            AddtinalRewardAtk = requiredAttack;
            rewardGold = _rewardGold;
        }

        //던전 플레이 함수
        public bool PlayDungeon(float playerDef, float playerAtk, out int reward, out int usedHp)
        {
            //체력 소모
            Random rand = new Random();
            //20 ~ 35 사이의 체력 피해 지정
            int lostHp = rand.Next(20, 36);

            //방어력 계산, 성공 실패와 체력소모 기준
            float defCalc = RequireDef - playerDef;

            //최소 10의 체력 소모
            lostHp = (lostHp + RequireDef < 10) ? 10 : lostHp + RequireDef;
            //out에 입력
            usedHp = lostHp;

            //성공판정
            bool isSucces;
            if(defCalc > 0)
            {
                //0 ~ 9 까지 무작위 숫자 생성
                int randInt = rand.Next(0, 10);
                //생성된 숫자가 3보다 작으면 성공(40%)
                if(randInt <= 3)
                {
                    isSucces = true;
                }
                else
                {
                    isSucces= false;
                }
            }
            else
            {
                isSucces = true;
            }

            //보상 계산
            if(isSucces)
            {
                //추가보상을 받기위한 최소 공격력 계산
                int atkCalc = (playerAtk - AddtinalRewardAtk >= 0) ? (int)(playerAtk - AddtinalRewardAtk) : 0;
                //추가 보상 계산, 최소 공격력을 넘으면 넘은수치 ~ 넘은수치 * 2 % 만큼의 추가보상 획득
                int additinalReward = (int)(rewardGold * (1 + (rand.Next(atkCalc, atkCalc * 2 + 1) / 100f)));
                reward = (atkCalc <= 0) ? rewardGold : rewardGold * additinalReward;
                return true;
            }
            else
            {
                //보상을 주지 않음
                reward = 0;
                return false;
            }
        }

        public string DungeonInfoText()
        {
            string info;
            info = DungeonName.PadRight(8 , ' ');
            info = info + " | " + "방어력 " + RequireDef + " 이상 권장";

            return info;
        }
    }

}
