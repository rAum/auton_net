/// This is a system using Computer Vision algorithms to learn about enviroment and
/// prepare world model viewd by camera.
/// \author: Piotr Gródek (grodekpiotr@gmail.com)

#ifndef __CV_SYSTEM_HPP__
#define __CV_SYSTEM_HPP__
#include "common.hpp"

#include <opencv2/opencv.hpp>
#include <opencv2/highgui/highgui.hpp>

class TestCVSystem;

/// Computer Vision system hides implementation details and provides
/// simple interface to obtain information about world from camera.
class CVSystem
{
public:
    friend class TestCVSystem; // for making testing more easy - break encapsulation.

    CVSystem();
    ~CVSystem();

    /// Initializes system by setting capture method, allocating memory etc
    /// \param source if == "" then cam is used, otherwise video or image (not yet implemented) file
    /// \return false if failed
    bool Init(const std::string& source);

    /// Grab next frame if needed and do preprocessing
    void Update();

    /// Detect lanes on road
    void DetectLaneMarks();

    /// If system is working.
    bool Active() const { return active; }

    /// Returns last error in human readable format
    std::string GetLastError() const { return lastError; }

private:
    //\FIXME: move this function somwhere else
    /// Returns matrix of remove perspective distortion transformation.
    /// TODO: Remove hardcoded values and use vanish point estimation instead.
    /// \return matrix 4x4
    void SetupPerspectiveMatrix();


    //\FIXME: move this function somwhere else
    /// Lane mark detection from Marcos Nieto which uses .
    /// Do not work inplace.
    /// \param input image in grayscale
    /// \param result output image where lighter color means higher probability of lane mark
    /// \param tau expected width of lane mark (in pixel space)
    void LaneMarkDetector(const cv::Mat& input, cv::Mat& result, int tau) const;


    //-----CAPTURE RELATED------
    cv::VideoCapture input; // capture video
    cv::Mat          frame; // input frame (may be preprocessed a bit)
    cv::Size     inputSize; // input frame size (in pixels)
    
    //-----PROCESSING FEATURES-----
    cv::Mat         processed;  // processed image
    std::vector<cv::Mat> inChannels; //splitted channels
    
    //-----LANE DETECTOR-----
    cv::Mat         perspTrans; // perspective transformation
    cv::Mat      invPerspTrans; // invert above ;)
    cv::Mat           lanesImg; // black&white image showing where line marks may be
    cv::Mat             lmdRes; // input image with drawed detected lanes
    std::vector<cv::Vec4i>  lm; // detected line marks 


    //----OTHER---
    std::string lastError; // text error
    bool           active; // does cam is active/movie is playing
};

#endif //__CV_SYSTEM_HPP__
