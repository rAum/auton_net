#-------------------------------------------------
#
# Project created by QtCreator 2013-05-14T02:12:55
#
#-------------------------------------------------

QT       += core gui

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = robocar-debug
TEMPLATE = app

QMAKE_CXXFLAGS += -std=c++11

SOURCES += \
    src/MainWindow.cpp \
    src/main.cpp \
    src/rosgui/Window.cpp \
    src/rosgui/StringWindow.cpp \
    src/rosgui/ImageWindow.cpp \
    src/Utils.cpp

HEADERS  += \
    src/MainWindow.h \
    src/rosgui/Window.h \
    src/rosgui/StringWindow.h \
    src/rosgui/ImageWindow.h \
    src/Utils.h \
    src/ThisCodeIsNotMine.h \
    src/ThisCodeIsMine.h

FORMS    += mainwindow.ui

unix:!macx:!symbian: LIBS += -L/opt/ros/groovy/lib/ -lcpp_common -lroscpp -lrosconsole -lroscpp_serialization

INCLUDEPATH += $$PWD/src
