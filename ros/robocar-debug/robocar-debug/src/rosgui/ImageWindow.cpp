#include "rosgui/ImageWindow.h"

#include "ThisCodeIsNotMine.h"
#   include <QImage>
#   include <QPixmap>
#   include <QLayout>

#   include "sensor_msgs/image_encodings.h"
#include "ThisCodeIsMine.h"


namespace rosgui
{
    ImageWindow::ImageWindow(
        QMdiArea *parent
    ):
        Window(parent),
        mImage(nullptr)
    {
        mImage = new QLabel(this);
        mImage->setMinimumSize(400, 300);

        layout()->addWidget(mImage);
    }

    bool ImageWindow::listen(
        ros::NodeHandle &rosNode,
        const std::string &rosTopic
    ) {
        qDebug("subscribing to '%s' (image)", rosTopic.c_str());
        setWindowTitle(rosTopic.c_str());

        mSubscriber = rosNode.subscribe(
            rosTopic,
            1000,
            &ImageWindow::nextFrame,
            boost::dynamic_pointer_cast<ImageWindow>(shared_from_this())
        );

        return !!mSubscriber;
    }

    void ImageWindow::nextFrame(const sensor_msgs::Image::ConstPtr &msg)
    {
        // TODO TODO TODO
        // recognize image format, initialize QImage appropriately
        // headers to look at:
        // sensor_msgs/Image.h
        // sensor_msgs/image_encodings.h
        QImage img(&msg->data[0], msg->width, msg->height, QImage::Format_RGB888);

        mImage->setPixmap(QPixmap::fromImage(img).scaled(mImage->width(), mImage->height()));
    }
}
