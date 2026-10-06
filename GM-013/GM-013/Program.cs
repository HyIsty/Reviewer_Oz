using System;

namespace GM_013
{
    internal class Program
    {
        static int playerHP = 100;
        static int playerAtk = 10;
        static int gold = 0;
        static int trainingCount = 0;
        static int trainingLimit = 3;

        static bool isBossDefeated = false;
        static int explorationProgress = 0;

        static void Main(string[] args)
        {
            Random random = new Random();

            Console.WriteLine("===== 용사의 하루 =====");
            Console.WriteLine("[Space] 아침 훈련");
            Console.WriteLine("[H] 황야 탐험");
            Console.WriteLine("[B] 보스전");
            Console.WriteLine("[R] 상태 확인");
            Console.WriteLine("[Esc] 종료");

            while (true)
            {
                Console.WriteLine();
                Console.Write("행동을 선택하세요 : ");

                ConsoleKey input = Console.ReadKey(true).Key;
                Console.WriteLine();
                #region Escape
                if (input == ConsoleKey.Escape)
                {
                    Console.WriteLine("프로그램을 종료합니다.");
                    break;
                }
                #endregion

                if (input == ConsoleKey.Spacebar)
                {
                    if (trainingCount >= trainingLimit)
                    {
                        Console.WriteLine("오늘은 더 이상 훈련할 수 없습니다...");
                        continue;
                    }

                    Console.WriteLine("--- 훈련 시작 ---");

                    for (int i = 1; i <= 10; i++)
                    {
                        Console.WriteLine($"칼을 휘두릅니다! ({i}회)");
                    }

                    Console.WriteLine("훈련을 하여 공격력이 5만큼 상승했습니다. " + $"{playerAtk} -> {playerAtk + 5}");

                    playerAtk += 5;
                    trainingCount++;

                    Console.WriteLine($"오늘의 훈련 횟수 : {trainingCount}/{trainingLimit}");
                }
                else if (input == ConsoleKey.H)
                {
                    if (playerHP <= 0)
                    {
                        Console.WriteLine("체력이 없어 탐험할 수 없습니다.");
                        continue;
                    }

                    explorationProgress = 0;
                    Console.WriteLine("--- 황야 탐험 시작 ---");

                    while (explorationProgress < 100)
                    {
                        explorationProgress += 20;

                        Console.WriteLine($"탐험 진행도 : {explorationProgress}%");

                        if (random.Next(0, 100) < 20)
                        {
                            playerHP -= 10;

                            if (playerHP < 0)
                                playerHP = 0;

                            Console.WriteLine("함정에 걸렸습니다! 체력이 10 감소했습니다. " + $"현재 HP : {playerHP}");

                            if (playerHP <= 0)
                            {
                                Console.WriteLine("탐험 실패!");
                                break;
                            }
                        }
                    }

                    if (explorationProgress >= 100 && playerHP > 0)
                    {
                        gold += 50;

                        Console.WriteLine("탐험을 성공적으로 마쳤습니다. " + $"+50G 현재 자금 : {gold}G");
                    }
                }
                else if (input == ConsoleKey.B)
                {
                    #region exception
                    if (isBossDefeated)
                    {
                        Console.WriteLine("이미 보스를 처치했습니다...");
                        continue;
                    }

                    if (playerHP <= 0)
                    {
                        Console.WriteLine("체력이 없어 보스와 싸울 수 없습니다.");
                        continue;
                    }
                    #endregion

                    int bossHP = 100;
                    Console.WriteLine("--- 보스전 시작 ---");

                    do
                    {
                        int previousBossHP = bossHP;
                        bossHP -= playerAtk;
                        if (bossHP < 0)
                            bossHP = 0;

                        Console.WriteLine($"보스에게 {playerAtk}만큼 피해를 주었습니다! " + $"보스 HP : {previousBossHP} -> {bossHP}");

                        if (bossHP <= 0)
                            break;

                        Console.WriteLine("보스가 반격합니다!");

                        int previousPlayerHP = playerHP;
                        playerHP -= 20;
                        if (playerHP < 0)
                            playerHP = 0;

                        Console.WriteLine("20의 피해를 받았습니다. " + $"현재 HP : {previousPlayerHP} -> {playerHP}");

                    } while (bossHP > 0 && playerHP > 0);

                    if (playerHP <= 0)
                    {
                        Console.WriteLine("보스에게 패배했습니다...");
                    }
                    else if (bossHP <= 0)
                    {
                        Console.WriteLine("보스를 격파했습니다!");
                        isBossDefeated = true;
                    }
                }
                else if (input == ConsoleKey.R)
                {
                    Console.WriteLine("--- 현재 상태 ---");
                    Console.WriteLine($"HP : {playerHP} | ATK : {playerAtk} | GOLD : {gold}G");

                    if (playerHP <= 20)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("휴식이 절실합니다...");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.WriteLine("Space, H, B, R 중 하나를 입력해주세요.");
                }
            }
        }
    }
}