//
// Created by Bridger on 4/7/2019.
//

#include "buf_to_str.h"

#include <iostream>

std::string buf_to_str(asio::streambuf & buf, std::size_t length)
{
  //std::cout << "Buffer size " << buf.size() << " will consume " << length << std::endl;
  std::string result {
    asio::buffers_begin(buf.data()),
    asio::buffers_begin(buf.data()) + length - 2}; // -2 for the two newlines

  // Consume through the first delimiter.
  buf.consume(length);

  //std::cout << result << std::endl;

  return result;
}