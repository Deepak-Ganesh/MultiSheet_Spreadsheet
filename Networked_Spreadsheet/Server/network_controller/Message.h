/** Contains the class declaration of Message.
 *
 *  1 April 2019
 *  Created by team FUBAR
 *  CS 3505
 *  Professor Peter Jensen
 *  Do not distribute
 */

#ifndef SERVER_MESSAGE_H
#define SERVER_MESSAGE_H

#include <cstdio>
#include <cstdlib>
#include <string>

/** Stores the data of a socket message with double newline terminator.
 * 
 */
class Message
{
  public:
    /** Stores message_str with added "\n\n".
     */
    Message(std::string message_str);
    
    /** Returns the underlying string message.
     */
    std::string const & get() const;

    /** Returns a pointer to the underlying C string
     */
    char const * c_str() const;

    /** Returns the number of characters of the message.
     */
    std::size_t length() const;


  private:
    std::string str;
};

#endif //SERVER_MESSAGE_H
