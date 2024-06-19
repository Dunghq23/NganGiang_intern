#include <iostream>
#include <fstream>
#include <cstdlib>

using namespace std;

int main(int argc, char* argv[]) {
    // Kiểm tra xem có đúng hai đối số được cung cấp không
    if (argc != 3) {
        cout << "Usage: sum.exe <num1> <num2>" << endl;
        return 1;
    }

    // Chuyển đổi hai đối số thành số nguyên
    int num1 = atoi(argv[1]);
    int num2 = atoi(argv[2]);

    // Kiểm tra chuyển đổi thành công
    if (num1 == 0 && argv[1][0] != '0') {
        cout << "First argument must be a valid integer." << endl;
        return 1;
    }
    if (num2 == 0 && argv[2][0] != '0') {
        cout << "Second argument must be a valid integer." << endl;
        return 1;
    }

    // Tính tổng của hai số
    int sum_result = num1 + num2;

    // Ghi kết quả vào file "result.txt"
    ofstream outfile("result.txt");
    if (!outfile) {
        cout << "Failed to open result.txt" << endl;
        return 1;
    }
    outfile << sum_result;
    outfile.close();

    return 0;
}
