using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_020
{
    // 부모 클래스
    public class Building
    {
        public string buildName;
        public int baseProsperity;
        public int prosperity;

        public virtual int GetDailyProsperity(int x, int y, Building[,] grid)
        {
            prosperity = baseProsperity;
            return prosperity;
        }
    }

    public class House : Building
    {
        public House()
        {
            buildName = "H";
            baseProsperity = 10;
        }

        public override int GetDailyProsperity(int x, int y, Building[,] grid)
        {
            prosperity = baseProsperity;

            int[] xs = new int[8] { 1, -1, 0, 0, -1, -1, 1, 1 };
            int[] ys = new int[8] { 0, 0, 1, -1, -1, 1, -1, 1 };

            int houseCount = 0;
            int parkCount = 0;
            bool hasNearMarket = false;

            for (int i = 0; i < xs.Length; i++)
            {
                int nx = x + xs[i];
                int ny = y + ys[i];

                if (nx < 0 || ny < 0 || nx >= grid.GetLength(0) || ny >= grid.GetLength(1))
                    continue;

                if (grid[nx, ny] == null)
                    continue;

                // 상하좌우 검사
                if (i < 4)
                {
                    if (grid[nx, ny] is House)
                    {
                        houseCount++;
                    }
                    else if (grid[nx, ny] is Market)
                    {
                        hasNearMarket = true;
                    }
                }

                // 대각선을 포함한 8방향 검사
                if (grid[nx, ny] is Park)
                {
                    parkCount++;
                }
            }

            prosperity += houseCount * 5;
            prosperity += parkCount * 5;

            if (hasNearMarket)
            {
                prosperity += 10;
            }

            return prosperity;
        }
    }

    public class Market : Building
    {
        public Market()
        {
            buildName = "M";
            baseProsperity = 20;
        }

        public override int GetDailyProsperity(int x, int y, Building[,] grid)
        {
            prosperity = baseProsperity;

            bool foundMarket = false;

            // 같은 열에서 다른 시장 검색
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                if (i == x)
                    continue;

                if (grid[i, y] is Market)
                {
                    foundMarket = true;
                    break;
                }
            }

            // 같은 행에서 다른 시장 검색
            if (!foundMarket)
            {
                for (int i = 0; i < grid.GetLength(1); i++)
                {
                    if (i == y)
                        continue;

                    if (grid[x, i] is Market)
                    {
                        foundMarket = true;
                        break;
                    }
                }
            }

            // 물류 네트워크 보너스
            if (foundMarket)
            {
                prosperity = 35;
            }

            int[] xs = new int[8] { 1, -1, 0, 0, -1, -1, 1, 1 };
            int[] ys = new int[8] { 0, 0, 1, -1, -1, 1, -1, 1 };

            int houseCount = 0;
            int parkCount = 0;

            for (int i = 0; i < xs.Length; i++)
            {
                int nx = x + xs[i];
                int ny = y + ys[i];

                if (nx < 0 || ny < 0 || nx >= grid.GetLength(0) || ny >= grid.GetLength(1))
                    continue;

                if (grid[nx, ny] == null)
                    continue;

                // 상하좌우의 집 검사
                if (i < 4 && grid[nx, ny] is House)
                {
                    houseCount++;
                }

                // 대각선을 포함한 8방향의 공원 검사
                if (grid[nx, ny] is Park)
                {
                    parkCount++;
                }
            }

            prosperity += houseCount * 10;
            prosperity += parkCount * 5;

            return prosperity;
        }
    }

    public class Park : Building
    {
        public Park()
        {
            buildName = "P";
            baseProsperity = 0;
        }

        public override int GetDailyProsperity(int x, int y, Building[,] grid)
        {
            prosperity = 0;
            return prosperity;
        }
    }

    internal class Program
    {
        static Building[,] grid = new Building[5, 5];
        static Random random = new Random();

        static void Main(string[] args)
        {
            CreateVillage();

            Console.WriteLine("===== 판타지 마을 번영도 계산 =====");
            Console.WriteLine("[Space] 마을 번영도 계산");
            Console.WriteLine("[Esc] 프로그램 종료");

            while (true)
            {
                Console.WriteLine();
                Console.Write("행동을 선택하세요 : ");

                ConsoleKey input = Console.ReadKey(true).Key;
                Console.WriteLine();

                switch (input)
                {
                    case ConsoleKey.Spacebar:
                        {
                            Calculation();
                            break;
                        }

                    case ConsoleKey.Escape:
                        {
                            Console.WriteLine("프로그램을 종료합니다.");
                            return;
                        }

                    default:
                        {
                            Console.WriteLine("Space 또는 Esc를 입력해주세요.");
                            break;
                        }
                }
            }
        }

        // 마을 무작위 생성
        static void CreateVillage()
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    int randomBuilding = random.Next(0, 4);

                    if (randomBuilding == 0)
                        grid[i, j] = null;
                    else if (randomBuilding == 1)
                        grid[i, j] = new House();
                    else if (randomBuilding == 2)
                        grid[i, j] = new Market();
                    else if (randomBuilding == 3)
                        grid[i, j] = new Park();
                }
            }
        }

        // 마을 전체 번영도 계산
        static void Calculation()
        {
            int totalProsperity = 0;

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == null)
                        continue;

                    totalProsperity += grid[i, j].GetDailyProsperity(i, j, grid);
                }
            }

            Output(totalProsperity);
        }

        // 마을 배치와 번영도 출력
        static void Output(int totalProsperity)
        {
            string outer = "=== 건물 배치 그리드 ===";
            string prosperityOuter = "=== 건물별 번영도 ===";

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                outer += "\n";
                prosperityOuter += "\n";

                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == null)
                    {
                        outer += "[ ]";
                        prosperityOuter += "[ ]";
                    }
                    else
                    {
                        outer += $"[{grid[i, j].buildName}]";
                        prosperityOuter += $"[{grid[i, j].prosperity}]";
                    }
                }
            }

            Console.WriteLine(outer);
            Console.WriteLine(prosperityOuter);
            Console.WriteLine($"최종 번영도 : {totalProsperity}");
        }
    }
}