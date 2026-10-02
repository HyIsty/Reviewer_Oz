using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GM_011
{
    internal class Program
    {
        struct Registration
        {
            public int year;
            public int month;
            public int day;
            public bool isMale;

            public string frontNum;
            public string backNum;
        }

        static Registration reg = new Registration();
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("주민등록번호 형식 문자열 생성기.");
            Year();
            MonthDay();
            Gender();
            RandomBack();

            Console.WriteLine("입력하신 생년 월일 : " + reg.year + " " + reg.month + " " + reg.day);
            Console.WriteLine("생성된 주민등록번호 : " + reg.frontNum + "-" + reg.backNum);
        }

        static void Year()
        {
            while (true)
            {
                Console.WriteLine("태어난 년도를 입력해주세요. (숫자 두 개 혹은 네 개): ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out _))
                {
                    Console.WriteLine("숫자를 입력해주세요.");
                    continue;
                }
                int inputInt = int.Parse(input);
                if (!(inputInt >= 1000 && inputInt <= 9999) &&
                    !(inputInt >= 10 && inputInt <= 99))
                {
                    Console.WriteLine("숫자를 두 자리, 혹은 네 자리 입력해주세요.");
                    continue;
                }

                if (inputInt >= 1000 && inputInt <= 9999)
                {
                    reg.year = inputInt;
                    reg.frontNum += input[2];
                    reg.frontNum += input[3];
                }
                else
                {
                    /*
                    if (inputInt > 24)
                        reg.year = 1900 + inputInt;
                    else
                        reg.year = 2000 + inputInt;
                    */
                    reg.frontNum += inputInt.ToString();
                    while (true)
                    {
                        Console.WriteLine("연도를 선택해주세요.");
                        Console.WriteLine("1. 19" + inputInt);
                        Console.WriteLine("2. 20" + inputInt);
                        string a = Console.ReadLine();

                        if (!int.TryParse(a, out _))
                        {
                            Console.WriteLine("1, 2 중에 선택해주세요.");
                            continue;
                        }
                        else if (int.Parse(a) != 1 && int.Parse(a) != 2)
                        {
                            Console.WriteLine("1, 2 중에 선택해주세요.");
                            continue;
                        }
                        else if (int.Parse(a) == 1)
                            reg.year = 1900 + int.Parse(a);
                        else
                            reg.year = 2000 + int.Parse(a);

                        break;
                    }
                    break;
                }
            }
            
        }

        static void MonthDay()
        {
            while (true)
            {
                Console.WriteLine("태어난 달과 일을 입력해주세요. (/로 구분)");
                string input = Console.ReadLine();

                if (!input.Contains("/"))
                {
                    Console.WriteLine("/로 구분해주세요.");
                    continue;
                }
                string[] inputSplit = input.Split('/');
                if(inputSplit.Length != 2)
                {
                    Console.WriteLine("태어난 달과 일을 입력해야 합니다.");
                    continue;
                }
                int month = int.Parse(inputSplit[0]);
                int day = int.Parse(inputSplit[1]);
              
                switch (month)
                {
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                    case 12:
                        if(day > 31 && day < 1)
                        {
                            Console.WriteLine(month + "월은 31일까지 있습니다.");
                            continue;
                        }
                        break;
                    case 2:
                        if (day > 28 && day < 1)
                        {
                            Console.WriteLine(month + "월은 28일까지 있습니다.");
                            continue;
                        }
                        break;

                    case 4:
                    case 6:
                    case 9:
                    case 11:
                        if (day > 30 && day < 1)
                        {
                            Console.WriteLine(month + "월은 30일까지 있습니다.");
                            continue;
                        }
                        break;
                    default:
                        Console.WriteLine("1월에서 12월 사이로 입력해주세요.");
                        continue;
                }

                reg.month = month;
                reg.day = day;
                reg.frontNum += month.ToString("D2");
                reg.frontNum += day.ToString("D2");
                break;
            }
        }

        static void Gender()
        {
            while (true)
            {
                Console.WriteLine("성별을 입력해주세요. 남자면 1 혹은 3, 여자면 2 혹은 4를 입력해주세요.");
                string input = Console.ReadLine();
                if(!int.TryParse(input, out _))
                {
                    Console.WriteLine("숫자를 입력해주세요");
                    continue;
                }
                int inputInt = int.Parse(input);

                switch(inputInt)
                {
                    case 1:
                    case 3:
                        reg.isMale = true;
                        break;

                    case 2:
                    case 4:
                        reg.isMale = false;
                        break;

                    default:
                        reg.isMale = rnd.Next(0, 2) == 0;
                        break;
                }

                break;
            }
        }

        static void RandomBack()
        {
            int[] rndBack = new int[7];
            if (reg.isMale)
            {
                if (reg.year > 1999)
                    rndBack[0] = 3;
                else
                    rndBack[0] = 1;
            }
            else
            {
                if (reg.year > 1999)
                    rndBack[0] = 4;
                else
                    rndBack[0] = 2;
            }

            for (int i = 1; i < rndBack.Length; i++)
            {
                while (true)
                {
                    int temp = rnd.Next(1, 10);
                    if (temp == rndBack[i - 1])
                        continue;

                    rndBack[i] = temp;
                    break;
                }
            }

            string result = string.Empty;
            foreach (int i in rndBack)
                result += i;

            reg.backNum = result.ToString();
        }
    }
}
