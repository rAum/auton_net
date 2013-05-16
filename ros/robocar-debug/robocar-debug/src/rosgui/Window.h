#ifndef ROSGUI_WINDOW_H
#define ROSGUI_WINDOW_H

#include "ThisCodeIsNotMine.h"
#   include <QMdiArea>
#   include <QMdiSubWindow>
#   include <QObject>

#   include "ros/subscriber.h"
#   include "ros/node_handle.h"
#include "ThisCodeIsMine.h"

#include <boost/shared_ptr.hpp>
#include <boost/enable_shared_from_this.hpp>
#include <string>


namespace rosgui
{
    /**
     * @brief Abstract base/factory for all windows that display ROS messages.
     *
     * Abstract base for all MDI windows that display messages supplied by ROS.
     * Serves as an abstract factory of said windows.
     */
    class Window:
        public QMdiSubWindow,
        public boost::enable_shared_from_this<rosgui::Window>
    {
        Q_OBJECT

    public:
        /**
         * @brief Factory function that creates a specific window.
         *
         * @returns Shared pointer to created window, or empty pointer if error happened.
         */
        static boost::shared_ptr<rosgui::Window> createSubscriber(
            QMdiArea* parent,
            ros::NodeHandle& rosNode,
            const std::string& rosTopic
        );

    protected:
        /**
         * @brief Constructor.
         */
        explicit Window(
            QMdiArea *parent = 0    /**< MDI area which should contain this window. */
        );

        /**
         * @brief Attempts to subscribe the window to a ROS topic.
         *
         * @returns true if function succeeded, false otherwise.
         */
        virtual bool listen(
            ros::NodeHandle& rosNode,       /**< ROS node handle. */
            const std::string& rosTopic     /**< ROS topic to subscribe to. */
        ) = 0;

        /**
         * @brief Close event handler - unsubscribes from ROS topic.
         */
        void closeEvent(
            QCloseEvent* e      /**< Event data. */
        );

        /** ROS subscriber object */
        ros::Subscriber mSubscriber;
    };
}

#endif // ROSGUI_WINDOW_H
