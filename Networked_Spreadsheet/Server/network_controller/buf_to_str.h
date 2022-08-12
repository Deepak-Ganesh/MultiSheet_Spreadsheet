//
// Created by Bridger on 4/7/2019.
//

#ifndef SERVER_BUF_TO_STR_H
#define SERVER_BUF_TO_STR_H

#include <string>
#include <asio.hpp>

/** Reads (and consumes) bytes from a streambuf to a string.
 *  Assumes a 2-byte delimiter (i.e. "\n\n") and removes from the final string.
 *
 * @param buf The buffer to read from.
 * @param length How many bytes (including 2-byte delimiter) to read.
 * @return
 */
std::string buf_to_str(asio::streambuf & buf, std::size_t length);


#endif //SERVER_BUF_TO_STR_H
