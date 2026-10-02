using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_012
{
    internal class Program
    {
        enum AgeRating { All, Teen, Adult}

        struct GameInfo
        {
            public string title;
            public AgeRating age;
            public int price;
            public float rating;
            /// <summary>
            /// 곧 DLC 출시 / 최적화 좋지 않음 / 버그 많음
            /// </summary>
            public (bool a, bool b, bool c) significant;

            public GameInfo(string title, AgeRating age, int price, float rating, bool a, bool b, bool c)
            {
                this.title = title;
                this.age = age;
                this.price = price;
                this.rating = rating;
                this.significant = (a, b, c);
            }
        }

        static GameInfo sts = new GameInfo("Slay the Spire", AgeRating.Teen, 27000, 8.0f,false, true, false);
        static GameInfo lc = new GameInfo("Limbus Company", AgeRating.Adult, 0, 8.5f, false, true, true);
        static GameInfo sz = new GameInfo("소녀전선", AgeRating.Teen, 0, 8.6f, false, false, true);
        static GameInfo snb = new GameInfo("산나비", AgeRating.Teen, 15500, 8.4f, true, false, false);
        static GameInfo mh = new GameInfo("몬스터헌터", AgeRating.Teen, 45800, 7.1f, true, true, false);

        static void Main(string[] args)
        {
            GameInfo[] games = new GameInfo[5];
            games[0] = sts;
            games[1] = lc;
            games[2] = sz;
            games[3] = snb;
            games[4] = mh;

            while (true) 
            {
                Console.WriteLine("게임 정보를 출력합니다.");
                Console.WriteLine("1. 모든 게임 정보 출력");
                Console.WriteLine("2. 특정 조건의 게임 정보 출력");
                Console.WriteLine("3. 종료");
                string input = Console.ReadLine();

                if(!int.TryParse(input, out _))
                {
                    Console.WriteLine("정해진 입력을 해주세요.");
                    continue;
                }
                else if(int.Parse(input) > 3 && int.Parse(input) < 1)
                {
                    Console.WriteLine("정해진 입력을 해주세요.");
                    continue;
                }

                switch(int.Parse(input))
                {
                    case 1:
                        Console.WriteLine("모든 게임의 정보를 출력합니다.");
                        foreach(GameInfo game in games)
                        {
                            Console.WriteLine("게임 타이틀 : " + game.title + " 이용 등급 : " + game.age);
                            Console.WriteLine("게임 가격 : " + game.price + "원 평점 : " + game.rating + "점");
                            Console.Write("특이사항 - ");
                            if (game.significant.a)
                                Console.Write("곧 DLC 출시 ");
                            if (game.significant.b)
                                Console.Write("최적화가 덜 됨 ");
                            if (game.significant.c)
                                Console.Write("버그가 많음 ");
                            Console.WriteLine();
                        }
                        break;
                    case 2:
                        while (true)
                        {
                            Console.WriteLine("특정 조건의 게임 정보를 출력합니다.");
                            Console.WriteLine("1. 평점 8.0 이상의 게임");
                            Console.WriteLine("2. Adult 등급의 게임");
                            Console.WriteLine("3. 최적화가 덜 된 게임");
                            string specialInput = Console.ReadLine();

                            if(!int.TryParse(specialInput, out _))
                            {
                                Console.WriteLine("정해진 입력을 해주세요.");
                                continue;
                            }

                            List<GameInfo> output = new List<GameInfo>();
                            switch (int.Parse(specialInput))
                            {
                                case 1:
                                    Console.WriteLine("평점 8.0 이상의 게임");
                                    foreach(GameInfo a in games)
                                    {
                                        if(a.rating > 8.0f)
                                            output.Add(a);
                                    }
                                    break;
                                case 2:
                                    Console.WriteLine("Adult 등급의 게임");
                                    foreach (GameInfo a in games)
                                    {
                                        if (a.age == AgeRating.Adult)
                                            output.Add(a);
                                    }
                                    break;
                                case 3:
                                    Console.WriteLine("최적화가 덜 된 게임");
                                    foreach (GameInfo a in games)
                                    {
                                        if (a.significant.b)
                                            output.Add(a);
                                    }
                                    break;
                                default:
                                    Console.WriteLine("정해진 입력을 해주세요.");
                                    continue;
                            }

                            foreach (GameInfo game in output)
                            {
                                Console.WriteLine("게임 타이틀 : " + game.title + " 이용 등급 : " + game.age);
                                Console.WriteLine("게임 가격 : " + game.price + "원 평점 : " + game.rating + "점");
                                Console.Write("특이사항 - ");
                                if (game.significant.a)
                                    Console.Write("곧 DLC 출시 ");
                                if (game.significant.b)
                                    Console.Write("최적화가 덜 됨 ");
                                if (game.significant.c)
                                    Console.Write("버그가 많음 ");
                                Console.WriteLine();
                            }

                            break;
                        }
                        break;
                    case 3:
                        Console.WriteLine("프로그램을 종료합니다.");
                        return;
                }
            }
        }
    }
}
