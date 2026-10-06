using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_014
{
    internal class Program
    {
        static string[] inventory = new string[5];
        static string[] fieldItems = {"물약", "녹슨 검", "금화", "독사과", "방패"};
        static int fieldIndex = 0;

        static void Main(string[] args)
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                inventory[i] = "비어있음";
            }

            Console.WriteLine("===== 인벤토리 관리 시스템 =====");
            Console.WriteLine("[I] 인벤토리 확인");
            Console.WriteLine("[G] 필드 아이템 줍기");
            Console.WriteLine("[U] 1번 슬롯 아이템 사용");
            Console.WriteLine("[S] 독사과 버리기");
            Console.WriteLine("[Esc] 종료");

            while (true)
            {
                Console.WriteLine();
                Console.Write("행동을 선택하세요 : ");

                ConsoleKey input = Console.ReadKey(true).Key;
                Console.WriteLine();

                switch (input)
                {
                    case ConsoleKey.I:
                        Console.WriteLine("--- 인벤토리 상태 ---");

                        for (int i = 0; i < inventory.Length; i++)
                        {
                            Console.WriteLine($"[{i + 1}번 슬롯] : {inventory[i]}");
                        }
                        break;
                    case ConsoleKey.G:
                        if (fieldIndex >= fieldItems.Length)
                        {
                            Console.WriteLine("필드에 더 이상 아이템이 없습니다.");
                            break;
                        }

                        bool isItemAdded = false;

                        for (int i = 0; i < inventory.Length; i++)
                        {
                            if (inventory[i] == "비어있음")
                            {
                                inventory[i] = fieldItems[fieldIndex];
                                Console.WriteLine($"{i + 1}번 슬롯에 {fieldItems[fieldIndex]}(을)를 주웠습니다.");

                                fieldIndex++;
                                isItemAdded = true;
                                break;
                            }
                        }
                        if (!isItemAdded)
                        {
                            Console.WriteLine("가방이 가득 찼습니다!");
                        }
                        break;
                    case ConsoleKey.U:
                        if (inventory[0] != "비어있음")
                        {
                            Console.WriteLine($"{inventory[0]}(을)를 사용했습니다!");
                            inventory[0] = "비어있음";

                            for (int i = 1; i < inventory.Length; i++)
                            {
                                inventory[i - 1] = inventory[i];
                            }

                            inventory[inventory.Length - 1] = "비어있음";
                        }
                        else
                            Console.WriteLine("1번 슬롯이 비어있습니다...");
                        break;
                    case ConsoleKey.S:
                        bool isDiscarded = false;

                        for (int i = 0; i < inventory.Length; i++)
                        {
                            if (inventory[i] == "독사과")
                            {
                                inventory[i] = "비어있음";
                                isDiscarded = true;
                                Console.WriteLine($"{i + 1}번 슬롯에서 독사과를 버립니다.");
                            }
                        }

                        if (!isDiscarded)
                        {
                            Console.WriteLine("인벤토리에 독사과가 없습니다.");
                        }
                        break;
                    case ConsoleKey.Escape:
                        Console.WriteLine("프로그램을 종료합니다.");
                        return;
                    default:
                        Console.WriteLine("I, G, U, S 중 하나를 입력해주세요.");
                        break;
                }
            }
        }
    }
}