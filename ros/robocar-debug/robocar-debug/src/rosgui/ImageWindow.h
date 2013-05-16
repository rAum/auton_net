#ifndef ROSGUI_IMAGEWINDOW_H
#define ROSGUI_IMAGEWINDOW_H

#include "ThisCodeIsNotMine.h"
#   include <QMdiArea>
#   include <QMdiSubWindow>
#   include <QObject>
#   include <QLabel>

#   include "ros/node_handle.h"
#   include "sensor_msgs/Image.h"
#include "ThisCodeIsMine.h"

#include "rosgui/Window.h"

namespace rosgui
{
    class ImageWindow:
        public Window
    {
        Q_OBJECT

    private:
        /**
         * @brief Constructor.
         */
        explicit ImageWindow(
            QMdiArea *parent = 0     /**< MDI area which should contain this window. */
        );

        /**
         * @brief Attempts to subscribe the window to a ROS topic.
         *
         * @returns true if function succeeded, false otherwise.
         */
        bool listen(
            ros::NodeHandle& rosNode,       /**< ROS node handle. */
            const std::string& rosTopic     /**< ROS topic to subscribe to. */
        );

        /**
         * @brief Handles incoming ROS messages.
         */
        void nextFrame(
            const sensor_msgs::Image::ConstPtr& msg     /**< Incoming image. */
        );

        /** Label for displaying received image. */
        QLabel* mImage;

        friend class rosgui::Window;
    };
}

#endif // ROSGUI_IMAGEWINDOW_H
