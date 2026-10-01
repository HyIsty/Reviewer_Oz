using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_009
{
    internal class Program
    {
        static int[] nums = new int[7];
        static int[] answers = new int[6];
        static int bonus;
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            int input = 99;
            Console.WriteLine("로또 추첨기입니다.");
            while (true)
            {
                Console.WriteLine("번호를 직접 입력하려면 1, 자동 입력은 2를 눌러주세요. (종료는 0번)");
                input = int.Parse(Console.ReadLine());

                switch (input)
                {
                    case 1:
                        Manual();
                        break;
                    case 2:
                        Auto();
                        break;
                    case 0:
                        Console.WriteLine("프로그램을 종료합니다.");
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
            }
        }

        static void Manual()
        {
            Console.WriteLine("수동 입력기를 선택하셨습니다.");

            while (true)
            {
                Console.WriteLine("1~45 사이의 당첨 번호 6개와 보너스 번호 1개를 입력해주세요. (공백으로 구분) : ");
                string[] input = Console.ReadLine().Split(' ');

#region exception
                bool except = false;
                for (int i = 0; i < input.Length; i++)
                {
                    if (!int.TryParse(input[i], out _))
                    {
                        Console.WriteLine("숫자를 입력해야 합니다.");
                        except = true;
                        break;
                    }
                    if (int.Parse(input[i]) > 45 || int.Parse(input[i]) < 1)
                    {
                        Console.WriteLine("1에서 45 사이의 번호여야 합니다.");
                        except = true;
                        break;
                    }
                    if(input.Length != 7)
                    {
                        Console.WriteLine("숫자를 총 7개 입력해야 합니다.");
                        except = true;
                        break;
                    }
                }
                if (input.Length != input.Distinct().Count())
                {
                    Console.WriteLine("중복된 숫자가 있습니다.");
                    except = true;
                }

                if (except)
                    continue;
                #endregion

                for(int i = 0; i < input.Length;i++)
                {
                    nums[i] = int.Parse(input[i]);
                }
                break;
            }

            Lottery();
        }

        static void Auto()
        {
            Console.WriteLine("자동 입력기를 선택하셨습니다.");

            for(int i = 0; i < nums.Length; i++)
            {
                while (true)
                {
                    int temp = rnd.Next(1, 46);
                    if (nums.Contains(temp))
                        continue;
                    nums[i] = temp;
                    break;
                }
            }

            Lottery();
        }

        static void Lottery()
        {
            Console.WriteLine("로또 추첨 중...");
            for (int i = 0; i < answers.Length; i++)
            {
                while (true)
                {
                    int temp = rnd.Next(1, 46);
                    if (answers.Contains(temp))
                        continue;
                    answers[i] = temp;
                    break;
                }
            }
            while (true)
            {
                int temp_bonus = rnd.Next(1, 46);
                if (answers.Contains(temp_bonus)) 
                    continue;
                bonus = temp_bonus;
                break;
            }


            Console.WriteLine("로또 추첨 완료.");
            Console.Write("로또 번호 : ");
            for (int i = 0; i < answers.Length; i++)
            {
                Console.Write(answers[i] + " ");
            }
            Console.WriteLine("보너스 번호 : " + bonus);

            Console.WriteLine("사용자의 번호 : ");
            for(int i = 0; i < nums.Length; i++)
            {
                Console.Write(nums[i] + " ");
            }
            Console.WriteLine();

            Result();
        }

        static void Result()
        {
            int count = 0;
            for(int i = 0; i < nums.Length;i++)
            {
                if (answers.Contains(nums[i]))
                    count++;
            }
            bool getBonus = nums[6] == bonus;

            if (count == 6)
                Console.WriteLine("로또 1등 당첨입니다!!");
            else if (count == 5 && getBonus)
                Console.WriteLine("로또 2등 당첨입니다!!");
            else if (count == 5)
                Console.WriteLine("로또 3등 당첨입니다!!");
            else if (count == 4)
                Console.WriteLine("로또 4등 당첨입니다!!");
            else if (count == 3)
                Console.WriteLine("로또 5등 당첨입니다!!");
            else
                Console.WriteLine("당첨되지 못했습니다...");
        }
    }
}
