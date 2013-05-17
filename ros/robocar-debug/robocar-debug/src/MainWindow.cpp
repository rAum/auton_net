#include "MainWindow.h"
#include "ui_mainwindow.h"

#include <cstdio>


MainWindow::MainWindow(
    QWidget *parent
):
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

void MainWindow::onSubscriberWindowClosed(
    rosgui::Window *closedWindow
) {
    for (auto it = mSubscribers.begin(); it != mSubscribers.end(); ++it)
        if (it->get() == closedWindow)
        {
            mSubscribers.erase(it);
            break;
        }
}

void MainWindow::on_listTopics_itemDoubleClicked(
    QListWidgetItem *item
) {
    std::string rosTopicName = item->text().toStdString();

    boost::shared_ptr<rosgui::Window> subscriber =
        rosgui::Window::createSubscriber(ui->mdiArea, mRosNode, rosTopicName);

    if (subscriber)
    {
        subscriber->show();
        mSubscribers.push_back(subscriber);

        connect(subscriber.get(), SIGNAL(windowClosed(rosgui::Window*)), this, SLOT(onSubscriberWindowClosed(rosgui::Window*)));
    }
}

void MainWindow::on_btnRefreshTopicList_clicked()
{
    ui->btnRefreshTopicList->setEnabled(false);

    refreshTopicsList();

    ui->btnRefreshTopicList->setEnabled(true);
    ui->statusBar->showMessage("Topics list refreshed!", 2000);
}

void MainWindow::on_actionArrangeRows_triggered()
{
    arrangeRosWindows(mSubscribers.size(), 1);
}

void MainWindow::on_actionArrangeColumns_triggered()
{
    arrangeRosWindows(1, mSubscribers.size());
}

void MainWindow::on_actionArrangeGrid_triggered()
{
    int cols = (int)ceil(sqrt((double)mSubscribers.size()));
    int rows = (int)ceil((double)mSubscribers.size() / (double)cols);

    arrangeRosWindows(rows, cols);
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

void MainWindow::arrangeRosWindows(
    int rows,
    int cols
) {
    qDebug("arranging: %d rows, %d cols\n", rows, cols);

    int wndWidth = ui->mdiArea->width() / cols;
    int wndHeight = ui->mdiArea->height() / rows;

    for (int row = 0; row < rows; ++row)
    {
        for (int col = 0; col < cols; ++col)
        {
            int idx = row * cols + col;
            if (idx >= (int)mSubscribers.size())
                return; // all windows arranged

            mSubscribers[idx]->resize(wndWidth, wndHeight);
            mSubscribers[idx]->move(col * wndWidth, row * wndHeight);
        }
    }
}
