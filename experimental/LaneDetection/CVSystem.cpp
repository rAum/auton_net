/// CVSystem implementation
/// \author: Piotr Gródek (grodekpiotr@gmail.com)
#include "CVSystem.hpp"


CVSystem::CVSystem()
{
    lastError = "No error.";
    active    = false;
}


CVSystem::~CVSystem()
{
}

void CVSystem::Update()
{
    if (input.grab() == false) {
        input.set(CV_CAP_PROP_POS_FRAMES, 0);
        //active = false; // temp - loop video
        return;
    }

    input.retrieve(frame);
    
    /// draw ROI lines of perspective transform
    cv::line(frame, cv::Point(180,120), cv::Point(180+460, 120), cv::Scalar(120,0,0), 1);
    cv::line(frame, cv::Point(376,50), cv::Point(376+140, 50), cv::Scalar(120,0,0), 1);
    cv::line(frame, cv::Point(180,120), cv::Point(376,50), cv::Scalar(120,0,0), 1, CV_AA);
    cv::line(frame, cv::Point(180+460, 120), cv::Point(376+140, 50), cv::Scalar(120,0,0), 1, CV_AA);

    cv::cvtColor(frame, frame,CV_BGR2GRAY);
    cv::split(frame, inChannels);
}

bool CVSystem::Init(const std::string& source)
{
    if (source == "") {
        input.open(-1);
    }
    else {
        input.open(source);
    }

    if (!input.isOpened()) {
        lastError = "Init: Error when tried to open capture stream.";
        return false;
    }


    input >> frame;

    cv::Mat::MSize ms = frame.size;
    inputSize.width   = ms[1]; // cols
    inputSize.height  = ms[0]; // rows

    lanesImg.create(inputSize, CV_8UC1);

    active = true;
    SetupPerspectiveMatrix();

    return true;
}


void CVSystem::SetupPerspectiveMatrix()
{
    cv::Point2f src[4], dst[4];

    /// approximate data for 2.wmv file.
    src[0].x = 376;       src[0].y = 50;  // up left
    src[1].x = 376 + 140; src[1].y = 50;  // up right
    src[2].x = 180;       src[2].y = 120; // down left
    src[3].x = 180 + 460; src[3].y = 120; // down right

    dst[0].x = 108;       dst[0].y = 0;   // up left
    dst[1].x = 540;       dst[1].y = 0;   // up right
    dst[2].x = 108;       dst[2].y = 400; // down left
    dst[3].x = 540;       dst[3].y = 400; // down right

    perspTrans = cv::getPerspectiveTransform(src, dst);
    invPerspTrans = cv::getPerspectiveTransform(dst, src);
}


void CVSystem::DetectLaneMarks()
{
    // remove perspective
    cv::warpPerspective(inChannels[0], processed, perspTrans, inputSize, cv::INTER_CUBIC);
    
    // apply lane detector filter   
    LaneMarkDetector(processed, lanesImg, 7);
    
    // threshold
    lanesImg = lanesImg > 50;

    // erode and dilate to remove noise nad make bigger lines
    cv::erode(lanesImg, lanesImg, cv::getStructuringElement(cv::MORPH_RECT, cv::Size(5,5))); // open
    cv::dilate(lanesImg, lanesImg, cv::getStructuringElement(cv::MORPH_RECT, cv::Size(3,3))); // close
    
    // remove detected lines for last frame
    lm.clear();

    /// using probabilistic Hough Line detector
    cv::HoughLinesP(lanesImg, 
                    lm,
                    10, // width of line (resolution) in pixels <- don't work?!
                    CV_PI, // angle resolution (in rad - 45 degree)
                    100, // how many votes need to pass
                    10, // minimum line length
                    10); // max distance of merging lines into one (in pixels)
    

    //////////////////////////////////////////////


    /// merge processed and nicely display result

    ///FIXME: remove lines below - it's ugly and stupid
    std::vector<cv::Mat> m;
    //or m.push_back(processed);
    m.push_back(frame);
    m.push_back(frame);
    m.push_back(frame);        
    cv::merge(m, lmdRes);
    // end of FIXME

    // Use perspective transform to correctly draw lines
    cv::vector<cv::Point2f> lpSrc, lpEnd;

    for (size_t i = 0; i < lm.size(); ++i) { // for each line detected
        cv::Vec4i line = lm[i];
        lpSrc.push_back(cv::Point2f(static_cast<float>(line[0]),static_cast<float>(line[1]))); // construct points
        lpSrc.push_back(cv::Point2f(static_cast<float>(line[2]),static_cast<float>(line[3])));
        cv::line(processed, lpSrc[2*i], lpSrc[2*i+1], cv::Scalar(53,53,200), 3); // draw it!
    }
    
    lpEnd.resize(lpSrc.size()); // resize to match lpSrc (otherwise opencv crashes)
    
    if (lpEnd.size() > 1) // cuz opencv is dumb...
        cv::perspectiveTransform(lpSrc, lpEnd, invPerspTrans); 

    for (size_t i = 0; i < lpEnd.size(); i += 2) { // for each point after transformation
        cv::line(lmdRes, lpEnd[i], lpEnd[i+1], cv::Scalar(53,53,200), 3); // draw it!
    }

    //draw perspective ~ROI lines
    cv::line(lmdRes, cv::Point(180,120), cv::Point(180+460, 120), cv::Scalar(120,0,0), 1);
    cv::line(lmdRes, cv::Point(376,50), cv::Point(376+140, 50), cv::Scalar(120,0,0), 1);
    cv::line(lmdRes, cv::Point(180,120), cv::Point(376,50), cv::Scalar(120,0,0), 1);
    cv::line(lmdRes, cv::Point(180+460, 120), cv::Point(376+140, 50), cv::Scalar(120,0,0), 1);

    // junk:
    //cv::Mat t(lmdRes);
    //lmdRes.copyTo(t);
    //cv::warpPerspective(t, lmdRes, invPerspTrans, inputSize);
}


// http://marcosnietoblog.wordpress.com/2011/12/27/lane-markings-detection-and-vanishing-point-detection-with-opencv/
// Author: Marcos Nieto
void CVSystem::LaneMarkDetector(const cv::Mat &srcGRAY, cv::Mat &dstGRAY, int tau) const
{
    dstGRAY.setTo(0);
	int aux = 0;
	for(int j = 0; j < srcGRAY.rows; ++j)
	{
		const unsigned char *ptRowSrc = srcGRAY.ptr<uchar>(j);
		unsigned char *ptRowDst = dstGRAY.ptr<uchar>(j);

		for(int i = tau; i < srcGRAY.cols - tau; ++i)
		{
			if (ptRowSrc[i] != 0)
			{
				aux = 2*ptRowSrc[i];
				aux += -ptRowSrc[i-tau];
				aux += -ptRowSrc[i+tau];
				aux += -abs((int)(ptRowSrc[i-tau] - ptRowSrc[i+tau]));

				aux = (aux<0)?(0):(aux);
				aux = (aux>255)?(255):(aux);

				ptRowDst[i] = (unsigned char)aux;
			}
		}
	}
}

