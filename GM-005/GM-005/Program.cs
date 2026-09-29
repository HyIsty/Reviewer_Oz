using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_005
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int input = 99;
            while(input != 0)
            {
                Console.WriteLine("메뉴를 선택해주세요. 1. 날짜 계산기, 2. 가위바위보, 0. 종료");
                input = int.Parse(Console.ReadLine());

                switch (input)
                {
                    case 1:
                        int dateInput;
                        Console.WriteLine("월의 일수를 구합니다. 1~12 중 하나를 입력해주세요. : ");
                        dateInput = int.Parse(Console.ReadLine());

                        DateCalculator(dateInput);
                        break;
                    case 2:
                        RSP();
                        break;
                    case 0:
                        Console.WriteLine("시스템을 종료합니다.");
                        return;
                    default:
                        Console.WriteLine("잘못된 접근입니다.");
                        break;
                }
            }
        }


        static void DateCalculator(int a)
        {
            switch (a)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    Console.WriteLine(a + "월은 31일까지 있습니다.");
                    break;

                case 2:
                    Console.WriteLine(a + "월은 28일까지 있습니다.");
                    break;

                case 4:
                case 6:
                case 9:
                case 11:
                    Console.WriteLine(a + "월은 30일까지 있습니다.");
                    break;

                default:
                    Console.WriteLine("잘못된 접근입니다.");
                    break;
            }
        }
        
        static void RSP()
        {
            int count = 0;
            int gold = 10000;
            do
            {
                Console.WriteLine("가위바위보를 시작합니다.현재 소지금 : " + gold + "\n베팅할 금액을 입력해주세요!(최소 베팅액 : 1000) : ");
                int bett = int.Parse(Console.ReadLine());
                if(bett > gold)
                {
                    Console.WriteLine("그만큼 베팅할 능력이 되지 않았습니다...");
                    continue;
                }
                if(bett < 1000)
                {
                    Console.WriteLine("최소 베팅 금액은 1000입니다!");
                    continue;
                }

                Console.WriteLine(bett + "만큼 베팅하셨습니다! \n가위, 바위, 보 중 하나를 입력해주세요! : ");
                string input = Console.ReadLine();
                if(input != "가위" && input != "바위" && input != "보")
                {
                    Console.WriteLine("가위, 바위, 보 중 하나를 입력해주세요.");
                    continue;
                }

                Random random = new Random();
                int rand = random.Next(1, 4);
                string npc = string.Empty;

                if (rand == 1)
                    npc = "가위";
                else if (rand == 2)
                    npc = "바위";
                else if (rand == 3)
                    npc = "보";

                Console.WriteLine("플레이어 " + input + " : " + npc + " 컴퓨터");

                int price = 0;
                switch(RSP_Calcul(input, rand))
                {
                    case -1:
                        price = bett * 7;
                        gold -= price;
                        Console.WriteLine("플레이어 패배... 판돈의 7배를 잃습니다. 잃은 돈 : " + price + ", 현재 골드 : " + gold);
                        break;
                    case 0:
                        price = bett * 5;
                        gold += price;
                        Console.WriteLine("비겼습니다! 판돈의 5배를 얻습니다. 얻은 돈 : " + price + ", 현재 골드 : " + gold);
                        break;
                    case 1:
                        price = bett * 3;
                        gold -= price;
                        Console.WriteLine("플레이어 승리! 판돈의 3배를 잃습니다. 잃은 돈 : " + price + ", 현재 골드 : " + gold);
                        break;
                }
                count++;
            } while (count < 5 || gold <= 0);
        }

        static int RSP_Calcul(string player, int npc)
        {
            switch (player)
            {
                case "가위":
                    switch (npc)
                    {
                        case 1:
                            return 0;
                        case 2: 
                            return -1;
                        case 3: 
                            return 1;
                        default:
                            return 99;
                    }
                case "바위":
                    switch (npc)
                    {
                        case 1:
                            return 1;
                        case 2:
                            return 0;
                        case 3:
                            return -1;
                        default:
                            return 99;
                    }
                case "보":
                    switch (npc)
                    {
                        case 1:
                            return -1;
                        case 2:
                            return 1;
                        case 3:
                            return 0;
                        default:
                            return 99;
                    }
                default:
                    return 99;
            }
        }
    }
}
