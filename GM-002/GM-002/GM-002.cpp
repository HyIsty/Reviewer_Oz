#include <iostream>
#include <string>
using namespace std;

void PrintItem(string name, string type, int str, int price) {
    std::cout << "아이템 이름 : " << name << endl << "아이템 종류 : " << type << endl << "추가 공격력 : " << str << endl << "가격 : " << price << endl;
}

void MakeItem() {
    string name;
    string type;
    int str;
    int price;

    name = "롱소드";
    type = "무기";
    str = 5;
    price = 300;
    PrintItem(name, type, str, price);

    name = "도란의 반지";
    type = "악세서리(반지)";
    str = 6;
    price = 400;
    PrintItem(name, type, str, price);

    name = "도란의 방패";
    type = "방패";
    str = 2;
    price = 400;
    PrintItem(name, type, str, price);

    name = "워모그의 갑옷";
    type = "상의";
    str = 21;
    price = 3100;
    PrintItem(name, type, str, price);

    name = "라바돈의 죽음모자";
    type = "모자";
    str = 35;
    price = 3500;
    PrintItem(name, type, str, price);

    name = "생강꽃, 안경 그리고 전해진 편지";
    type = "악세서리(보조장비)";
    str = 10;
    price = 700;
    PrintItem(name, type, str, price);
}


void RandomInt(int& a, int& b, int& c) {
    a = rand() % (5 - 1 + 1) + 1;
    b = rand() % (20 - 6 + 1) + 6;
    c = rand() % (100 - 150 + 1) + 150;

    std::cout << "1~5 사이 랜덤 : " << a << ", 6~20 사이 랜덤 : " << b << ", 150~200 사이 랜덤 : " << c << endl;
}

void TakePrize() {
    int currentGold = 0;
    while (currentGold < 100 || currentGold > 200) {
        std::cout << "현재 가지고 있는 골드를 입력하세요. (범위 : 100 ~ 200) : ";
        std::cin >> currentGold;

        if (currentGold < 100 || currentGold > 200) {
            std::cout << "잘못된 접근입니다." << endl;
            continue;
        }

        int prize = rand() % (100 - 50 + 1) + 50;
        int duty = (currentGold + prize) / 10;

        currentGold += prize - duty;
        std::cout << "현상금으로 " << prize << "만큼 나왔지만, " << duty << "만큼 세금이 나와 최종적으로 " << prize - duty << "만큼 받으셨습니다. 현재 골드 : " << currentGold << endl;
    }
}

void RandomStat() {
    int str;
    int dex;
    int inte;
    int luk;
    int total;

    str = rand() % (10 - 1 + 1) + 1;
    dex = rand() % (10 - 1 + 1) + 1;
    inte = rand() % (10 - 1 + 1) + 1;
    luk = rand() % (10 - 1 + 1) + 1;
    total = str + dex + inte + luk;

    std::cout << "플레이어 님의 스탯입니다." << endl << "STR : " << str << endl << "DEX : " << dex << endl << "INT : "
        << inte << endl << "LUK : " << luk << endl << "총 능력치 : " << total << endl;
}

int main()
{
    int input = 1;
    while (input != 0) {
        std::cout << "메뉴를 선택하세요. (1. 아이템 만들기, 2. 랜덤 숫자 담기, 3. 현상금 받기, 4. 랜덤 능력치 생성, 0. 종료) " << endl << ": ";
        std::cin >> input;

        switch (input) {
        case 1:
            MakeItem();
            break;
        case 2:
            int a, b, c;
            RandomInt(a, b, c);
            break;
        case 3:
            TakePrize();
            break;
        case 4:
            RandomStat();
            break;
        case 0 :
            std::cout << "프로그램을 종료합니다." << endl;
            break;
        default:
            std::cout << "잘못된 접근입니다." << endl;
            break;
        }
    }
}