using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TextRPG_SBH
{
    internal class GameSaveManager
    {
        private string folderPath;
        private DirectoryInfo directoryInfo;
        private string txtPath;

        private Character charSave;
        private Shop shopSave;

        private List<string> saveDataList = new List<string>();
        public List<string> SaveDataList {  get { return saveDataList; } }

        public GameSaveManager(Character _charSave, Shop _shopSave)
        {
            //객체 생성시, 현재 경로에 save폴더 찾기 경로 지정
            folderPath = Directory.GetCurrentDirectory() + "\\save";
            //경로에 폴더가 없으면 새로만들기
            directoryInfo = new DirectoryInfo(folderPath);
            if (directoryInfo.Exists == false)
            {
                directoryInfo.Create();
            }
            //저장한 텍스트파일 경로
            txtPath = folderPath + "\\savedata.txt";

            charSave = _charSave;
            shopSave = _shopSave;
        }

        public void SaveGame()
        {
            //저장할 리스트를 만들어줌
            ListMaker();
            //리스트의 모든 내용 텍스트에 저장
            File.WriteAllLines(txtPath, saveDataList);
            
        }

        public bool LoadGame()
        {
            //파일이 없으면 불러오기 실패
            if(!File.Exists(txtPath))
            {
                return false;
            }
            else
            {
                //리스트를 비워서 불러올 준비
                saveDataList.Clear();
                //줄 불러오기
                StreamReader reader = new StreamReader(txtPath);
                string readLine;
                while((readLine = reader.ReadLine()) != null)
                {
                    //각 줄을 리스트에 저장
                    saveDataList.Add(readLine);
                }
                //파일 사용 끝
                reader.Close();
                return true;
            }
        }

        //리스트에 저장할 텍스트를 생성하는 함수
        private void ListMaker()
        {
            //리스트를 비우고 저장할 내용 추가
            saveDataList.Clear();
            saveDataList.Add(charSave.UserName);
            saveDataList.Add(charSave.PlayerJobs.ToString());
            saveDataList.Add(charSave.Level.ToString());
            saveDataList.Add(charSave.HealthPoint.ToString());
            saveDataList.Add(charSave.Exp.ToString());
            saveDataList.Add(charSave.PlayerGold.ToString());

            for(int i = 0; i < charSave.inventory.Length; i++)
            {
                if (charSave.inventory[i] == null)
                {
                    saveDataList.Add((-1).ToString());
                }
                else
                {
                    saveDataList.Add(charSave.inventory[i].ItemId.ToString());
                }
            }

            for (int j = 0; j < charSave.inventory.Length; j++)
            {
                if (charSave.inventory[j] == null)
                {
                    saveDataList.Add((-1).ToString());
                }
                else
                {
                    if (charSave.inventory[j].IsEquipped == true)
                    {
                        saveDataList.Add(j.ToString());
                    }
                    else
                    {
                        saveDataList.Add((-1).ToString());
                    }
                }
            }

            for(int i = 0; i < shopSave.IsSold.Length; i++)
            {
                if (shopSave.IsSold[i] == true)
                {
                    saveDataList.Add("true");
                }
                else
                {
                    saveDataList.Add("false");
                }
            }
        }
        
    }
}
