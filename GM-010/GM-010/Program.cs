using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_010
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("숫자를 입력해 프로그램을 실행하세요.");
                Console.WriteLine("1. 문자 거꾸로 뒤집기");
                Console.WriteLine("2. 있는 그대로를 출력하지만  짝수에 있는 문자만 거꾸로 출력");
                Console.WriteLine("3. 문자열을 입력받아 숫자만 출력");
                Console.WriteLine("4. 문자열과 문자 하나를 입력받고 문자열 안에 해당 문자가 몇개 있는지 출력");
                Console.WriteLine("5. 주민번호를 입력하고 -이 제거된 주민번호 출력");
                Console.WriteLine("0. 프로그램 종료");
                string input = Console.ReadLine();

                if(!int.TryParse(input, out _))
                {
                    Console.WriteLine("숫자를 입력해주세요");
                    continue;
                }

                switch(int.Parse(input))
                {
                    case 0:
                        Console.WriteLine("프로그램을 종료합니다.");
                        return;
                    case 1:
                        Reverse();
                        break;
                    case 2:
                        EvenReverse();
                        break;
                    case 3:
                        OnlyNum();
                        break;
                    case 4:
                        FindChar();
                        break;
                    case 5:
                        RegisterNumber();
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        continue;
                }
            }
        }

        static void Reverse()
        {
            Console.WriteLine("문자열을 입력해주세요 : ");
            string input = Console.ReadLine();
            string reverse = new string(input.Reverse().ToArray());

            Console.WriteLine("입력하신 문자열 : " + input + ", 뒤집은 문자열 : " + reverse);
        }

        static void EvenReverse()
        {
            Console.WriteLine("문자열을 입력해주세요 : ");
            string input = Console.ReadLine();
            string even = string.Empty;

            for(int i = 0; i < input.Length; i++)
            {
                if (i % 2 == 1)
                    even += input[i];
            }
            string reverseEven = new string(even.Reverse().ToArray());

            string result = string.Empty;
            for(int i = 0; i < input.Length; i++)
            {
                if (i % 2 == 1)
                {
                    result += reverseEven[i / 2];
                }
                else
                    result += input[i];
            }

            Console.WriteLine("입력하신 문자열 : " + input + ", 결과 문자열 : " + result);
        }

        static void OnlyNum()
        {
            Console.WriteLine("문자열을 입력해주세요 : ");
            string input = Console.ReadLine();
            string result = string.Empty;

            foreach(char i in input)
            {
                if (char.IsDigit(i)) 
                    result += i;
            }

            Console.WriteLine("입력하신 문자열 : " + input + ", 결과 문자열 : " + result);
        }

        static void FindChar()
        {
            Console.WriteLine("문자열을 입력해주세요 : ");
            string input = Console.ReadLine();
            string a;
            while (true)
            {
                Console.WriteLine("찾을 문자를 입력해주세요 : ");
                a = Console.ReadLine();

                if(a.Length != 1)
                {
                    Console.WriteLine("문자를 하나만 입력해주세요.");
                    continue;
                }
                break;
            }

            int count = 0;
            for(int i = 0;i < input.Length;i++)
            {
                if (input[i] == a[0])
                    count++;
            }

            Console.WriteLine("입력하신 문자열 : " + input + ", 문자 : " + a);
            Console.WriteLine("- 결과 : " + count);
        }

        static void RegisterNumber()
        {
            while (true)
            {
                Console.WriteLine("주민번호를 입력해주세요(xxxxxx - xxxxxxx) : ");
                string input = Console.ReadLine();

                if(input.Length != 14)
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    continue;
                }

                if (input[6] != '-')
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    continue;
                }

                string[] temps = input.Split('-');
                if(temps.Length != 2)
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    continue;
                }

                if (!int.TryParse(temps[0], out _) || !int.TryParse(temps[1], out _))
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    continue;
                }

                Console.WriteLine("입력하신 문자열 : " + input);
                Console.WriteLine("결과 : " + temps[0] + temps[1]);
            }
        }
    }
}
