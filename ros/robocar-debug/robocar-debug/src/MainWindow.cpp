#include "MainWindow.h"
#include "ui_mainwindow.h"

#include <cstdio>


MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow),
    mRosSpinTimer(new QTimer(this))
{
    ui->setupUi(this);

    // force doRosSpin() call every ROS_SPIN_INTERVAL_MSEC
    connect(mRosSpinTimer, SIGNAL(timeout()), this, SLOT(doRosSpin()));
    mRosSpinTimer->setSingleShot(false);
    mRosSpinTimer->start(ROS_SPIN_INTERVAL_MSEC);

    refreshTopicsList();
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::doRosSpin()
{
    ros::spinOnce();
}

void MainWindow::on_listTopics_itemDoubleClicked(QListWidgetItem *item)
{
    std::string rosTopicName = item->text().toStdString();

    boost::shared_ptr<rosgui::Window> subscriber =
        rosgui::Window::createSubscriber(ui->mdiArea, mRosNode, rosTopicName);

    if (subscriber)
    {
        subscriber->show();
        mSubscribers.push_back(subscriber);
    }
}

void MainWindow::refreshTopicsList()
{
    ui->listTopics->model()->removeRows(0, ui->listTopics->model()->rowCount());

    // get ROS topics list from external command
    static char lineBuffer[4096];
    char* linePtr = lineBuffer;
    size_t bytesRead = sizeof(lineBuffer);
    FILE* availableTopics = popen("rostopic list", "r");

    // add all topics to the list
    while (getline(&linePtr, &bytesRead, availableTopics) >= 0)
    {
        for (size_t i = 1; i < bytesRead; ++i)
            if (lineBuffer[bytesRead - i] == '\n')
                lineBuffer[bytesRead - i] = '\0';

        ui->listTopics->insertItem(ui->listTopics->count(), QString(lineBuffer));
    }

    pclose(availableTopics);
}

void MainWindow::on_btnRefreshTopicList_clicked()
{
    ui->btnRefreshTopicList->setEnabled(false);

    refreshTopicsList();

    ui->btnRefreshTopicList->setEnabled(true);
    ui->statusBar->showMessage("Topics list refreshed!", 2000);
}
