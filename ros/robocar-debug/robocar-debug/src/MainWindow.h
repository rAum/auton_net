#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include "ThisCodeIsNotMine.h"
#   include <QMainWindow>
#   include <QTimer>
#   include <QListWidgetItem>

#   include "ros/ros.h"
#include "ThisCodeIsMine.h"

#include <boost/shared_ptr.hpp>

#include "rosgui/Window.h"


namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT
    
public:
    /**
     * @brief Constructor.
     */
    explicit MainWindow(
        QWidget *parent = 0     /**< Window parent. */
    );

    /**
     * @brief Destructor.
     */
    ~MainWindow();
    
private slots:
    /**
     * @brief Slot - dispatches pending ROS messages.
     *
     * Connected to mRosSpinTimer timeout() signal.
     */
    void doRosSpin();

    /**
     * @brief Slot - removes given window from the active subscribers list.
     */
    void onSubscriberWindowClosed(
        rosgui::Window* closedWindow
    );

    /**
     * @brief Slot - opens a window and connects it to ROS topic.
     */
    void on_listTopics_itemDoubleClicked(
        QListWidgetItem *item       /**< Clicked item. */
    );

    /**
     * @brief Slot - refreshes ROS topics list.
     */
    void on_btnRefreshTopicList_clicked();

    /**
     * @brief Slot - arranges subscriber windows in rows.
     */
    void on_actionArrangeRows_triggered();

    /**
     * @brief Slot - arranges subscriber windows in columns.
     */
    void on_actionArrangeColumns_triggered();

    /**
     * @brief Slot - arranges subscriber windows in a grid.
     */
    void on_actionArrangeGrid_triggered();

private:
    /**
     * @brief Refreshes ROS topics list.
     */
    void refreshTopicsList();

    /**
     * @brief Arranges opened ROS windows into a grid.
     */
    void arrangeRosWindows(
        int rows,   /**< Number of grid rows. */
        int cols    /**< Number of grid cols. */
    );

    /** Window controls handle */
    Ui::MainWindow *ui;

    /** Timer that forces dispatch of ROS messages */
    QTimer* mRosSpinTimer;
    /** Main ROS node handle */
    ros::NodeHandle mRosNode;

    /** MDI windows that receive ROS messages */
    std::vector<boost::shared_ptr<rosgui::Window> > mSubscribers;

    /** Constants */
    enum {
        ROS_SPINS_PER_SEC = 50,                             /**< ROS spins per second */
        ROS_SPIN_INTERVAL_MSEC = 1000 / ROS_SPINS_PER_SEC   /**< Delay between consecutive doRosSpin() calls */
    };
};

#endif // MAINWINDOW_H
