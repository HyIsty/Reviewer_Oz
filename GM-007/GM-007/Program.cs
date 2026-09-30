using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_007
{
    internal class Program
    {
        static int playerHp = 100;
        static int monHp = 200;
        static string[] playerAttackName = { "일반 베기", "가로 베기", "세로 베기", "기 모으기", "무적 치트", "즉사 치트" };
        static int[] playerAttack = { 10, 50, 25, 0};
        static bool playerCharge = false;
        static bool monCharge = false;
        static string[] monPattern = { "일반 공격", "기 모으기" };
        static bool eternal = false;

        static void Main(string[] args)
        {
            Console.WriteLine("몬스터가 나타났다!");
            while(playerHp > 0 && monHp > 0)
            {
                Console.WriteLine("현재 플레이어 HP : " +  playerHp + " | " + monHp + " : 몬스터 HP");
                PlayerTurn();
                if(monHp <= 0)
                {
                    Console.WriteLine("플레이어의 승리!");
                    break;
                }

                MonsterTurn();
                if(playerHp <= 0)
                {
                    Console.WriteLine("플레이어의 패배..");
                    break;
                }

            }
        }

        static void PlayerTurn()
        {
            int input = 99;
            do
            {
                Console.Write("플레이어의 턴! ");
                for (int i = 0; i < playerAttackName.Length; i++)
                {
                    Console.Write(i + ". " + playerAttackName[i] + " ");
                }
                Console.WriteLine();
                input = int.Parse(Console.ReadLine());

                if (input > playerAttackName.Length || input < 0)
                {
                    Console.WriteLine("잘못된 행동이다.");
                    continue;
                }
            } while (input > playerAttackName.Length || input < 0);


            Console.Write("플레이어의 " + playerAttackName[input] + "!");
            if (input == 3)
            {
                Console.WriteLine("플레이어는 기를 모았다!");
                playerCharge = true;
            }
            else if(input == 4)
            {
                if (eternal)
                    Console.WriteLine("무적 치트 Off");
                else
                    Console.WriteLine("무적 치트 On");
                eternal = !eternal;
            }
            else if(input == 5)
            {
                Console.WriteLine("즉사 치트 On");
                monHp = 0;
            }
            else
            {
                int dmg = (int)(playerCharge ? playerAttack[input] * 1.5 : playerAttack[input]);
                monHp = monHp - dmg > 0 ? monHp - dmg : 0;
                Console.WriteLine("몬스터에게 " + dmg + "의 데미지를 입혔다! 현재 적의 HP : " + monHp);
                playerCharge = false;
            }
        }

        static void MonsterTurn()
        {
            Random rand = new Random();
            int pattern = monCharge ? 0 : rand.Next(0, 2);
            
            Console.WriteLine("몬스터의 " + monPattern[pattern] + "!");
            switch(pattern)
            {
                case 0:
                    int critical = rand.Next(0, 2);
                    int dmg = (int)(monCharge ? 10 * 1.5 : 10);
                    if(critical > 0)
                    {
                        dmg = dmg * 2;
                        Console.Write("크리티컬!");
                    }
                    if (!eternal)
                    {
                        playerHp = playerHp - dmg > 0 ? playerHp - dmg : 0;
                        Console.WriteLine(dmg + "의 데미지를 입었다. 현재 플레이어의 HP : " + playerHp);
                    }
                    else
                    {
                        Console.WriteLine(dmg + "의 데미지를 입었다. 현재 플레이어의 HP : ETERNAL");
                    }
                        monCharge = false;
                    break;
                case 1:
                    Console.WriteLine("몬스터는 기를 모았다!");
                    monCharge = true;
                    break;
            }
        }
    }
}
