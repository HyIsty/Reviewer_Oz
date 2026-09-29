#include <iostream>
using namespace std;

void Calcul_A(int a, int b) {
	cout << "입력받은 정수 : " << a << ", " << b << endl;
	cout << "사칙연산 시작" << endl;
	cout << a << " + " << b << " = " << a + b << endl;
	cout << a << " - " << b << " = " << a - b << endl;
	cout << a << " * " << b << " = " << a * b << endl;
	cout << a << " / " << b << " = " << a / b << endl;
}

void Calcul_B(int a, int b, int c) {
	cout << "입력받은 정수 : " << a << ", " << b << ", " << c << endl;
	cout << "(" << a << " + " << b << ") * (" << c << " + " << a << ") % " << a << " = " << (a + b) * (c + a) % a << endl;
	cout << "(" << c << " % " << b << ") + (" << a << " * 2) = " << (c % b) + (a * 2) << endl;
	cout << "(" << a << " + " << b << " + " << c << ") % (" << c << " + 1) = " << (a + b + c) % (c + 1) << endl;
}

void Calcul_Remain(int a, int b) {
	cout << "입력받은 정수 : " << a << ", " << b << endl;
	int result = a / b;
	int remain = a % b;
	cout << a << "와 " << b << "의 몫은 " << result << ", 나머지는 " << remain << "입니다." << endl << endl;

	cout << "복구 수식 : a = (" << b << " * " << result << ") + " << remain << endl;
	cout << "원본 a = " << a << ", 복구된 a = " << (b * result) + remain << endl;
}

void RhombusStar() {
	for (int i = 0; i < 9; i++) {
		int n = (i % ((9 / 2) + 1)) + (i / ((9 / 2) + 1) * (3 - 2 * (i % ((9 / 2) + 1))));
		cout << string(4 - n, ' ') << string(2 * n + 1, '*') << endl;

	}
}

int main()
{
    int input = 1;
    while (input != 0) {
        cout << "메뉴를 선택하세요. (1. 사칙연산, 2. 수식, 3. 나누기/몫, 4. 마름모, 0. 종료) " << endl << ": ";
        cin >> input;
        int a, b, c;
        switch (input) {
        case 1:
            cout << "정수를 두 개 입력해주세요.(공백으로 구분) : ";
            cin >> a >> b;
            Calcul_A(a, b);
            break;
        case 2:
            cout << "정수를 세 개 입력해주세요.(공백으로 구분) : ";
            cin >> a >> b >> c;
            Calcul_B(a, b, c);
            break;
        case 3:
            cout << "정수를 두 개 입력해주세요.(공백으로 구분) : ";
            cin >> a >> b;
            Calcul_Remain(a, b);
            break;
        case 4:
            RhombusStar();
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