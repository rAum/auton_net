#include <iostream>
#include "CVSystem.hpp"
#include "Test_CVSystem.hpp"

int main(int argc, char**argv)
{
    CVSystem cvs;
    TestCVSystem test_cv(&cvs);

    /// TESTS
    test_cv.laneDetect("D:\\2.wmv");

    return 0;
}
