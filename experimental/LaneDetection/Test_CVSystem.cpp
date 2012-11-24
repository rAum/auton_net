/// Implements tests for CVSystem.
/// \author: Piotr Gródek (grodekpiotr@gmail.com)
#include "Test_CVSystem.hpp"

TestCVSystem::TestCVSystem(CVSystem* _cvs) : cvs(_cvs)
{
}


bool TestCVSystem::laneDetect(const std::string& source)
{
    if (!cvs->Init(source)) {
        return false;
    }

    cv::namedWindow("Input");
    cv::namedWindow("Res");
    cv::namedWindow("Output");
    cv::namedWindow("Res 2");

    while(cvs->Active())
    {
        cvs->Update();
        cvs->DetectLaneMarks();

        cv::imshow("Input", cvs->frame);
        cv::imshow("Output", cvs->lanesImg);
        cv::imshow("Res", cvs->processed);
        cv::imshow("Res 2", cvs->lmdRes);
        if (cv::waitKey(25) >= 0)
            break;
    }

    return true;
}