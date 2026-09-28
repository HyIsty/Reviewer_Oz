#include <iostream>

int currentGold = 2000;

void MiningGold(int& gold) 
{
    if (gold > 1000) 
    {
        gold += 200;
        std::cout << "소지 골드가 1000이 넘어 추가로 골드를 획득합니다. 골드 + 200 현재 골드 : " << gold << "\n";
    }
    else 
    {
        gold += 100;
        std::cout << "골드를 획득합니다. 골드 + 100 현재 골드 : " << gold << "\n";
    }
}

void Swap_Value(int a, int b) {
    std::cout << "Before Call By Value : a - " << a << ", b - " << b << "\n";
    int temp = a;
    a = b;
    b = temp;
    std::cout << "After Call By Value : a - " << a << ", b - " << b << "\n";
}

void Swap_Ref(int& a, int& b) {
    std::cout << "Before Call By Reference : a - " << a << ", b - " << b << "\n";
    int temp = a;
    a = b;
    b = temp;
    std::cout << "After Call By Reference : a - " << a << ", b - " << b << "\n";
}

bool TrySpendGold(int& gold) {
    int spendGold = 1000;

    if (gold < spendGold) {
        std::cout << "골드가 부족하여 골드를 소모할 수 없습니다. 현재 골드 : " << gold << "\n";
        return false;
    }
    else {
        gold -= spendGold;
        std::cout << "골드를 " << spendGold << "만큼 소모하였습니다. 현재 골드 : " << gold << "\n";
        return true;
    }
}

int main()
{
    int choice;
    std::cout << "메뉴를 선택하세요 (1. 골드 획득, 2. Swap_Call By Value, 3. Swap_Call By Reference, 4. 골드 소모)" << "\n" << " : ";
    std::cin >> choice;

    switch (choice) {
    case 1:
        MiningGold(currentGold);
        break;
    case 2:
        std::cout << "숫자 두 개를 입력해주세요. : ";
        int a; int b;
        std::cin >> a >> b;
        std::cout << "\n";

        Swap_Value(a, b);
        std::cout << "AfterOut Call By Value : a - " << a << ", b - " << b << "\n";
        break;
    case 3:
        std::cout << "숫자 두 개를 입력해주세요. : ";
        int c, d;
        std::cin >> c >> d;
        std::cout << "\n";

        Swap_Ref(c, d);
        std::cout << "AfterOut Call By Reference : a - " << c << ", b - " << d << "\n";
        break;
    case 4:
        TrySpendGold(currentGold);
        break;
    default:
        std::cout << "잘못된 접근입니다.";
    }
}

