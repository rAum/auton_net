/// Implements tests for CVSystem.
/// \author: Piotr Gródek (grodekpiotr@gmail.com)
#ifndef __TEST_CV_SYSTEM_HPP__
#define __TEST_CV_SYSTEM_HPP__
#include "CVSystem.hpp"

class TestCVSystem
{
public:
    explicit TestCVSystem(CVSystem* _cvs);

    bool laneDetect(const std::string & source);

private:
    CVSystem* cvs;
};

#endif //__TEST_CV_SYSTEM_HPP__
