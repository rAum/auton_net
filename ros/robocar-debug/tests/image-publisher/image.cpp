#include "ros/ros.h"
#include "sensor_msgs/Image.h"

#include <cstdio>
#include <sstream>
#include <cassert>

#pragma pack(1)
struct Bitmap
{
    struct FileHeader
    {
        char magic[2];
        uint32_t fileSize;
        uint16_t reserved[2];
        uint32_t dataOffset;
    };

    struct DibHeader
    {
        uint32_t headerSize;
        int32_t imgWidthPixels;
        int32_t imgHeightPixels;
        uint16_t numColorPlanes;
        uint16_t bitsPerPixel;
        uint32_t compressionMethod;
        uint32_t rawDataSize;
        int32_t horizontalResolution; // pixel per meter
        int32_t verticalResolution;   // pixel per meter
        uint32_t numColorsInPalette;
        uint32_t numImportantColors;
    };

    enum CompressionType
    {
        CompressionNone = 0,
        CompressionRLE8 = 1,
        CompressionRLE4 = 2,
        CompressionBitfields = 3,
        CompressionJpeg = 4,
        CompressionPng = 5,
        CompressionAlphaBitfields = 6
    };

    FileHeader fileHeader;
    DibHeader dibHeader; char* colorTable;
    char* pixelData;

    Bitmap(const char* filename = nullptr):
        colorTable(nullptr),
        pixelData(nullptr)
    {
        if (filename != nullptr)
            open(filename);
    }

    ~Bitmap()
    {
        delete[] colorTable;
        delete[] pixelData;
    }

    bool open(const char* filename)
    {
        FILE* f = fopen(filename, "r");
        if (f == NULL)
            return false;

        fread(&fileHeader, sizeof(FileHeader), 1, f);
        if (fileHeader.magic[0] != 'B' || fileHeader.magic[1] != 'M')
            return false;

        fread(&dibHeader, sizeof(DibHeader), 1, f);

        if (colorTable)
            delete[] colorTable;
        colorTable = new char[dibHeader.numColorsInPalette * 4];

        fread(colorTable, dibHeader.numColorsInPalette * 4, 1, f);

        if (pixelData)
            delete[] pixelData;
        pixelData = new char[dibHeader.rawDataSize];

        fseek(f, fileHeader.dataOffset, SEEK_SET);

        assert(dibHeader.bitsPerPixel % 8 == 0);
        assert(dibHeader.bitsPerPixel == 24); // fuck it, BGR only

        int32_t bytesPerPixel = dibHeader.bitsPerPixel / 8;
        for (int32_t row = dibHeader.imgHeightPixels - 1; row >= 0; --row)
        {
            char* dataPtr = pixelData + row * dibHeader.imgWidthPixels * bytesPerPixel;
            fread(dataPtr, dibHeader.imgWidthPixels * bytesPerPixel, 1, f);

            for (int32_t col = 0; col < dibHeader.imgWidthPixels; ++col)
            {
                // fuck it, BGR only
                char* pix = dataPtr + col * bytesPerPixel;
                char b = pix[0];
                pix[0] = pix[2];
                pix[2] = b;
            }
        }

        fclose(f);

        printf("loaded image:\n"
               "width = %d\n"
               "height = %d\n"
               "bpp = %u\n"
               , dibHeader.imgWidthPixels, dibHeader.imgHeightPixels, dibHeader.bitsPerPixel);

        return true;
    }

    const sensor_msgs::Image toSensorImage() const
    {
        assert(pixelData);

        sensor_msgs::Image ret;
        ret.width = dibHeader.imgWidthPixels;
        ret.height = dibHeader.imgHeightPixels;
        ret.data.resize(dibHeader.rawDataSize);
        memcpy(&ret.data[0], pixelData, ret.data.size());

        // ???
        ret.encoding = "";
        ret.step = 1;
        ret.is_bigendian = false;

        ret.header.frame_id = "";
        ret.header.seq = 0;
        ret.header.stamp = ros::Time(0.0);

        return ret;
    }

    void fillRow(int row, char r, char g, char b)
    {
        assert(dibHeader.bitsPerPixel % 8 == 0);

        int32_t bytesPerPixel = dibHeader.bitsPerPixel / 8;
        char* rowBase = pixelData + dibHeader.imgWidthPixels * bytesPerPixel * row;
        for (int32_t col = 0; col < dibHeader.imgWidthPixels; ++col)
        {
            char* pix = rowBase + col * bytesPerPixel;
            pix[0] = r;
            pix[1] = g;
            pix[2] = b;
        }
    }
};
#pragma pack()

int main(int argc, char **argv)
{
    ros::init(argc, argv, "image_generator");

    ros::NodeHandle n;
    ros::Publisher chatter_pub = n.advertise<sensor_msgs::Image>("image", 1000);
    ros::Rate loop_rate(10);

    Bitmap bmp;
    if (!bmp.open("colorful.bmp"))
        return 1;

    int colorCounter = 0;
    int rowCounter = 0;

    while (ros::ok())
    {
        char r = colorCounter == 0 ? 255 : 0;
        char g = colorCounter == 1 ? 255 : 0;
        char b = colorCounter == 2 ? 255 : 0;

        bmp.fillRow(rowCounter, r, g, b);

        if (++rowCounter >= bmp.dibHeader.imgHeightPixels)
        {
            rowCounter = 0;
            if (++colorCounter >= 3)
                colorCounter = 0;
        }

        sensor_msgs::Image msg = bmp.toSensorImage();

        chatter_pub.publish(msg);
        ros::spinOnce();
        loop_rate.sleep();
    }

    return 0;
}
