#ifndef ROSGUI_STRINGWINDOW_H
#define ROSGUI_STRINGWINDOW_H

#include "ThisCodeIsNotMine.h"
#   include <QMdiArea>
#   include <QMdiSubWindow>
#   include <QTextEdit>
#   include <QObject>

#   include "std_msgs/String.h"
#include "ThisCodeIsMine.h"

#include "rosgui/Window.h"

namespace rosgui
{
    class StringWindow:
        public Window
    {
        Q_OBJECT

    private:
        /**
         * @brief Constructor.
         */
        explicit StringWindow(
            QMdiArea *parent = 0    /**< MDI area which should contain this window. */
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
         * @brief Appends message to mMessages.
         */
        void appendMessage(
            const std_msgs::String::ConstPtr& msg       /**< Text to append. */
        );

        /** QTextEdit used for displaying messages. */
        QTextEdit* mMessages;

        /** Constants. */
        enum {
            MAX_MESSAGE_LINES = 1024    /**< Maximum number of lines that can be displayed simultaneously. */
        };

        friend class rosgui::Window;
    };
}

#endif // ROSGUI_STRINGWINDOW_H
