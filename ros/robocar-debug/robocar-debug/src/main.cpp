#include "ThisCodeIsNotMine.h"
#   include <QApplication>

#   include "ros/ros.h"
#include "ThisCodeIsMine.h"

#include "MainWindow.h"

int main(int argc, char *argv[])
{
    ros::init(argc, argv, "robocar_debug");

    QApplication a(argc, argv);
    MainWindow w;
    w.show();
    
    return a.exec();
}
