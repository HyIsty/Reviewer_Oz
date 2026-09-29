#include <iostream>
using namespace std;

void WhileFirst(int a) {
	cout << "입력받은 정수 : " << a << endl;
	while (a >= -15) {
		a -= 5;
		cout << a << endl;
	}
}

void DoWhileFirst(int a, int b) {
	cout << "입력받은 정수 : " << a << ", " << b << endl;
	int n = a;
	int total = 0;
	int sum = 0;
	do {
		if (n % 3 == 0) {
			cout << n << " ";
			total++;
			sum += n;
		}
        n++;
	} while (n <= b);
	cout << endl << "총 갯수 : " << total << " / 합 출력 : " << sum << endl;
}

void ForFirst(int a) {
	int sum = 0;
	cout << "구구단 " << a << "단 시작." << endl;
	for (int i = 1; i < 10; i++) {
		cout << a << " * " << i << " = " << a * i << endl;
		sum += a * i;
	}
	cout << "구구단 " << a << "단 총합 : " << sum << endl;
}

int main()
{
    int input = 1;
    while (input != 0) {
        cout << "메뉴를 선택하세요. (1. While, 2. DoWhile, 3. First, 0. 종료) " << endl << ": ";
        cin >> input;
        int a, b;
        switch (input) {
        case 1:
            cout << "정수를 입력해주세요. : ";
            cin >> a;
            WhileFirst(a);
            break;
        case 2:
            cout << "정수를 두 개 입력해주세요.(공백으로 구분) : ";
            cin >> a >> b;
            DoWhileFirst(a, b);
            break;
        case 3:
            cout << "정수를 입력해주세요. : ";
            cin >> a;
            ForFirst(a);
            break;
        case 0:
            cout << "프로그램을 종료합니다." << endl;
            break;
        default:
            cout << "잘못된 접근입니다." << endl;
            break;
        }
    }
}
