#ifndef UTILS_H
#define UTILS_H

#include <string>

namespace Utils
{
    /**
     * @brief Execute shell command and read output to string.
     *
     * @returns Executed command output or empty string if error happened.
     */
    const std::string readCommandOutput(
        const char* command     /**< Shell command to execute. */
    );
}

#endif // UTILS_H
