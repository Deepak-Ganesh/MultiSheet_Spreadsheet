/** Contains function definitions for Message.
 *
 *  1 April 2019
 *  Created by team FUBAR
 *  CS 3505
 *  Professor Peter Jensen
 *  Do not distribute
 */

#include "Message.h"

Message::Message(std::string message_str) :
  str (message_str + "\n\n")
{

}


std::string const & Message::get() const
{
  return str;
}



char const * Message::c_str() const
{
  return str.c_str();
}



std::size_t Message::length() const
{
  return str.length();
}