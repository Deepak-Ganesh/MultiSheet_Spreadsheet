/** Contains the class declaration of NetworkController.
 *
 *  1 April 2019
 *  Created by team FUBAR
 *  CS 3505
 *  Professor Peter Jensen
 *  Do not distribute
 */

#ifndef SERVER_NETWORK_CONTROLLER_H
#define SERVER_NETWORK_CONTROLLER_H

#include <asio.hpp>

#include "Server.h"

/** Has a method for beginning the server.
 *
 */
class NetworkController
{
  public:
    NetworkController();

    void start(unsigned short port);
};



#endif //SERVER_NETWORK_CONTROLLER_H
