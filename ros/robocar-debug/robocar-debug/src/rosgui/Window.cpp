#include "rosgui/Window.h"

#include "ThisCodeIsNotMine.h"
#   include <QMessageBox>
#include "ThisCodeIsMine.h"

#include <boost/algorithm/string/trim.hpp>

#include "rosgui/ImageWindow.h"
#include "rosgui/StringWindow.h"

#include "Utils.h"


namespace rosgui
{
    boost::shared_ptr<Window> Window::createSubscriber(
        QMdiArea* parent,
        ros::NodeHandle& rosNode,
        const std::string& rosTopic
    ) {
        // extract message type
        char* cmdBuffer = new char[sizeof("rostopic type ") + rosTopic.size()];
        sprintf(cmdBuffer, "rostopic type %s", rosTopic.c_str());
        std::string dataType = Utils::readCommandOutput(cmdBuffer);

        boost::trim(dataType);

        boost::shared_ptr<rosgui::Window> ret;

        // create appropriate Window object
        if (dataType == "")
            qDebug("invalid topic: %s\n", rosTopic.c_str());
        else if (dataType == "std_msgs/String")
            ret.reset(new rosgui::StringWindow(parent));
        else if (dataType == "sensor_msgs/Image")
            ret.reset(new rosgui::ImageWindow(parent));

        if (!ret)
        {
            QString message = QString("Topic ") + rosTopic.c_str() + " publishes messages of unsupported type: " + dataType.c_str();
            QMessageBox(QMessageBox::Critical, "Error", message).exec();
        }
        else if (!ret->listen(rosNode, rosTopic))
            ret.reset();

        return ret;
    }


    Window::Window(
        QMdiArea* parent
    ):
        QMdiSubWindow(parent)
    {
    }

    void Window::closeEvent(
        QCloseEvent *e
    ) {
        (void)e;

        // unsubscribe
        mSubscriber = ros::Subscriber();

        // notify parent
        emit windowClosed(this);
    }
}
