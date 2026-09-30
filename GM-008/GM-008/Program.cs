using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_008
{
    internal class Program
    {
        static Random rnd = new Random();
        static List<int> deck = new List<int>();
        static bool cheat = false;
        static int gold = 10000;

        static void Main(string[] args)
        {
            Console.WriteLine("월남뽕 게임을 시작합니다.");
            InitDeck();
            ShuffleDeck();
            while (gold > 0 && deck.Count >= 3)
            {
                PlayGame();
            }
        }

        static void InitDeck()
        {
            Console.WriteLine("덱을 초기화합니다.");
            gold = 10000;
            for(int i = 1; i < 53; i++)
                deck.Add(i);
        }

        static void ShuffleDeck()
        {
            Console.WriteLine("덱을 섞습니다.");
            int n = deck.Count;
            while(n > 1)
            {
                n--;
                int k = rnd.Next(n);
                int value = deck[k];
                deck[k] = deck[n];
                deck[n] = value;
            }
        }

        static string ConvertNumToString(int num)
        {
            string result = string.Empty;
            if (num <= 13)
                result += "♠";
            else if (num <= 26)
                result += "♦";
            else if (num <= 39)
                result += "♣";
            else
                result += "♥";

            if (num % 13 == 1)
                result += "A";
            else if (num % 13 == 11)
                result += "J";
            else if (num % 13 == 12)
                result += "Q";
            else if (num % 13 == 0)
                result += "K";
            else
                result += num % 13;

            return result;
        }

        static void PlayGame()
        {
            string outResult = cheat ? ConvertNumToString(deck[2]) : "?";
            Console.WriteLine("이번 턴의 패는 " + ConvertNumToString(deck[0]) + " " + outResult + " " + ConvertNumToString(deck[1]) + "입니다.");
            Console.WriteLine("베팅하시려면 베팅액을, 포기하시려면 fold를 입력해주세요. (최소 베팅금 : 1000), 현재 소지금 : " + gold);
            Console.WriteLine("혹은 치트. A. 패를 볼 수 있습니다. S. 다음 턴 카드를 미리 볼 수 있습니다. D. 현재 남아있는 카드를 볼 수 있습니다.");
            string input;
            input = Console.ReadLine();
            if(!int.TryParse(input, out _) && input != "fold" && input != "A" && input != "S" && input != "D")
            {
                Console.WriteLine("잘못된 입력입니다.");
                return;
            }
            if(int.TryParse(input, out _) && int.Parse(input) < 1000)
            {
                Console.WriteLine("최소 베팅액은 1000 입니다."); 
                return;
            }
            if(int.TryParse(input, out _) && int.Parse(input) > gold)
            {
                Console.WriteLine("베팅액이 현재 소지금보다 큽니다.");
                return;
            }

            if(input == "A")
            {
                string a = cheat ? "패 보기 치트 Off" : "패 보기 치트 On";
                cheat = !cheat;
                Console.WriteLine(a);
                return;
            }
            else if (input == "S")
            {
                Console.WriteLine("다음 턴 카드 보기 치트. 다음 턴에 " + ConvertNumToString(deck[4]) + " 카드가 등장합니다.");
                return;
            }
            else if(input == "D")
            {
                Console.WriteLine("현재 남아있는 카드 보기 치트.");
                for(int i = 0; i < deck.Count; i++)
                {
                    Console.Write(ConvertNumToString(deck[i]) + " ");
                    if (i % 11 == 10)
                        Console.WriteLine();
                }
                Console.WriteLine();
                return;
            }

            if (deck[0] % 13 == deck[1] % 13)
            {
                Console.Write("중복입니다. 금액을 차감합니다. 차감된 금액 : ");
                if (input == "fold")
                {
                    gold -= 1000;
                    Console.WriteLine("1000, 현재 소지금 : " + gold);
                }
                else
                {
                    gold -= int.Parse(input);
                    Console.WriteLine(int.Parse(input) + ", 현재 소지금 : " + gold);
                }
            }

            if(int.TryParse(input, out _))
            {
                Console.WriteLine(int.Parse(input) + "만큼 베팅하셨습니다.");
                gold -= int.Parse(input);
                Console.WriteLine("중간 패는 " + ConvertNumToString(deck[2]) + "입니다.");
                bool result = CalculateWin();

                switch (result)
                {
                    case true:
                        Console.WriteLine("중간 패가 앞 패와 뒷 패의 사이에 있습니다.");
                        Console.WriteLine("플레이어 승리!");
                        gold += int.Parse(input) * 2;
                        Console.WriteLine("베팅한 금액의 2배를 제공합니다. 제공된 금액 : " + int.Parse(input) * 2 + ", 현재 소지금 : " + gold);
                        break;
                    case false:
                        Console.WriteLine("중간 패가 앞 패와 뒷 패 사이에 있지 않습니다.");
                        Console.WriteLine("플레이어 패배.");
                        Console.WriteLine("베팅한 금액을 몰수합니다. 현지 소지금 : " + gold);
                        break;
                }
            }
            else
            {
                Console.WriteLine("폴드입니다. 다음 판을 진행합니다.");
            }
            if(gold <= 0)
            {
                Console.WriteLine("소지하신 돈이 매우 적습니다. 나가주세요.");
                return;
            }
            if(deck.Count < 3)
            {
                Console.WriteLine("덱을 모두 소진하였습니다. 게임을 종료합니다.");
                return;
            }

            Discard();
        }

        static bool CalculateWin()
        {
            int a = deck[0];
            int b = deck[1];
            int c = deck[2];

            if((c > a && c < b) || (c < a && c > b))
            {
                return true;
            }
            return false;
        }

        static void Discard()
        {
            Console.WriteLine("사용한 패를 제외합니다.");
            for(int i = 0; i < 3;  i++)
            {
                deck.RemoveAt(0);
            }
        }
    }
}
