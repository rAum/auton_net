#include "Utils.h"

#include <cstdio>
#include <sstream>


namespace Utils
{
    const std::string readCommandOutput(const char* command)
    {
        FILE* file = popen(command, "r");
        if (file == nullptr)
            return "";

        std::stringstream ss;
        char buffer[65536];

        while (!feof(file))
        {
            size_t bytesRead = fread(buffer, 1, sizeof(buffer) - 1, file);
            buffer[bytesRead] = '\0';
            ss << buffer;
        }

        fclose(file);

        return ss.str();
    }
}
