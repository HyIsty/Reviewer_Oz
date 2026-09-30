using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_006
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] num = new int[3];

            Console.WriteLine("숫자 야구를 시작합니다.");

            num = GachaNum();

            int count = 1;
            int input;
            
            do
            {
                int baseStrike = 0;
                int baseBall = 0;
                int baseOut = 0;
                Console.WriteLine(count + "번째 시도입니다. 1. 숫자 입력, 2. 치트(힌트) 사용");
                input = int.Parse(Console.ReadLine());

                switch (input)
                {
                    case 1:
                        for (int i = 0; i < 3; i++)
                        {
                            Console.WriteLine(i+1 + "번째 숫자를 입력해주세요. : ");
                            input = int.Parse(Console.ReadLine());
                            if (input < 0 && input > 9)
                            {
                                Console.WriteLine("숫자는 0부터 9까지만 입력할 수 있습니다.");
                                break;
                            }
                            if (input == num[i])
                                baseStrike++;
                            else if (num.Contains(input))
                                baseBall++;
                            else
                                baseOut++;
                        }
                        Console.WriteLine("결과는 " + baseStrike + " Strike, " + baseBall + " Ball, " + baseOut + " Out 입니다.");
                        
                        if(baseStrike == 3)
                        {
                            Console.WriteLine("축하합니다! " + count + "번의 시도만에 승리하셨습니다!");
                            input = 0;
                            count = 0;
                            while(input != 1 && input != 2)
                            {
                                Console.WriteLine("다시 게임을 시작하시려면 1, 이대로 종료하시려면 2를 입력해주세요. : ");
                                input = int.Parse(Console.ReadLine());

                                if(input != 1 && input != 2)
                                {
                                    Console.WriteLine("1과 2 중에 선택해주세요.");
                                    continue;
                                }

                                switch(input)
                                {
                                    case 1:
                                        break;
                                    case 2:
                                        return;
                                }
                            }
                        }
                        count++;
                        break;
                    case 2:
                        Console.WriteLine("힌트-치트 | 컴퓨터가 가진 숫자 : " + num[0] + " " + num[1] + " " + num[2]);
                        break;
                    default:
                        Console.WriteLine("1과 2 중에 선택해주세요.");
                        break;
                }
            } while (true);


        }

        static int[] GachaNum()
        {
            Console.WriteLine("숫자 추첨 중...");
            Random rnd = new Random();
            int[] num = new int[3];
            num[0] = rnd.Next(0, 10);
            do
            {
                num[1] = rnd.Next(0, 10);
            } while (num[1] == num[0]);
            do
            {
                num[2] = rnd.Next(0, 10);
            } while (num[2] == num[1] || num[2] == num[1]);
            Console.WriteLine("숫자 추첨 완료.");
            return num;
        }
    }
}
