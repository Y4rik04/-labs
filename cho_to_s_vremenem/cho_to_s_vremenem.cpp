#include <iostream>
#include <iomanip>

using namespace std;

class Time24 {
public:
    int hour;
    int minute;
    int second;

    Time24() : hour(0), minute(0), second(0) {}
    Time24(int hour, int minute, int second) : hour(hour), minute(minute), second(second) {}
    Time24(int allSeconds);

    void print();
    Time24 operator++ (int);
    friend Time24 operator+ (Time24 left, Time24 right);
    Time24 operator- (Time24 right);
    bool   operator< (Time24 right);
    bool   operator> (Time24 right);
    operator int() const;
    operator float() const;

    int toSeconds();

};
class Time12;

int main() {
    Time24 t1(9, 45, 6);
    t1.print();
    Time24 t2(9, 47, 55);
    t2.print();

    t1++;
    t2++;

    t1.print();
    t2.print();

    Time24 t3 = t1 + t2;
    t3.print();

    Time24 t4(10000);
    t4.print();
    t4 = 25000;
    t4.print();

    t3 = t1 + static_cast<Time24>(1000);
    t3.print();

    int s = t1;
    cout << "All seconds: " << s << endl;
    float m = t1;
    cout << "All minutes: " << m;


    return 0;
}

void Time24::print() {
    cout << setfill('0') << setw(2) << hour << ':'
        << setfill('0') << setw(2) << minute << ':'
        << setfill('0') << setw(2) << second
        << endl;
}

Time24 Time24::operator++(int) {
    second++;
    if (second > 59) {
        second -= 60;
        minute++;
    }
    if (minute > 59) {
        minute -= 60;
        hour++;
    }
    return Time24(hour, minute, second);
}

Time24 Time24::operator-(Time24 right) {
    long allSeconds1 = hour * 3600 + minute * 60 + second;
    long allSeconds2 = right.hour * 3600 + right.minute * 60 + right.second;
    long result = allSeconds1 - allSeconds2;
    int h = result / 3600;
    result -= h * 3600;
    int m = result / 60;
    result -= m * 60;
    return Time24(h, m, result);
}

bool Time24::operator<(Time24 right) {
    long allSeconds1 = hour * 3600 + minute * 60 + second;
    long allSeconds2 = right.hour * 3600 + right.minute * 60 + right.second;
    return allSeconds1 < allSeconds2;
}

bool Time24::operator>(Time24 right) {
    long allSeconds1 = hour * 3600 + minute * 60 + second;
    long allSeconds2 = right.hour * 3600 + right.minute * 60 + right.second;
    return allSeconds1 > allSeconds2;
}

Time24::Time24(int allSeconds) {
    hour = allSeconds / 3600;
    allSeconds -= hour * 3600;
    minute = allSeconds / 60;
    allSeconds -= minute * 60;
    second = allSeconds;
}

Time24 operator+(Time24 left, Time24 right) {
    long allSeconds1 = left.hour * 3600 + left.minute * 60 + left.second;
    long allSeconds2 = right.hour * 3600 + right.minute * 60 + right.second;
    long result = allSeconds1 + allSeconds2;
    int h = result / 3600;
    result -= h * 3600;
    int m = result / 60;
    result -= m * 60;
    return Time24(h, m, result);
}

Time24::operator int() const {
    return hour * 3600 + minute * 60 + second;
}

Time24::operator float() const {
    return hour * 60 + minute + second / 60.0;
}

