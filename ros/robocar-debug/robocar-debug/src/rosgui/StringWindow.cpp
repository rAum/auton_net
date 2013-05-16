#include "rosgui/StringWindow.h"

#include "ThisCodeIsNotMine.h"
#   include <QBoxLayout>
#include "ThisCodeIsMine.h"

#include <boost/function.hpp>

namespace rosgui
{
    StringWindow::StringWindow(
        QMdiArea *parent
    ):
        Window(parent),
        mMessages(nullptr)
    {
        mMessages = new QTextEdit(this);
        mMessages->setEnabled(false);
        mMessages->setMinimumSize(400, 300);

        layout()->addWidget(mMessages);
    }


    bool StringWindow::listen(
        ros::NodeHandle& rosNode,
        const std::string& rosTopic
    ) {
        qDebug("subscribing to '%s' (text)", rosTopic.c_str());
        setWindowTitle(rosTopic.c_str());

        mSubscriber = rosNode.subscribe(
            rosTopic,
            1000,
            &StringWindow::appendMessage,
            boost::dynamic_pointer_cast<StringWindow>(shared_from_this())
        );

        return !!mSubscriber;
    }

    void StringWindow::appendMessage(const std_msgs::String::ConstPtr& msg)
    {
        mMessages->append(("\n" + msg->data).c_str());

        // delete old messages, if there is too many
        int linesToDelete = mMessages->document()->lineCount() - MAX_MESSAGE_LINES;
        if (linesToDelete > 0)
        {
            QTextCursor cursor(mMessages->textCursor());
            cursor.movePosition(QTextCursor::Start, QTextCursor::MoveAnchor);

            for (int i = 0; i < linesToDelete; ++i)
                cursor.movePosition(QTextCursor::NextBlock, QTextCursor::KeepAnchor);

            cursor.removeSelectedText();
            mMessages->setTextCursor(cursor);
        }

        mMessages->scroll(0, mMessages->contentsRect().bottom());
    }
}
