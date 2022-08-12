/** Contains the main function (program entry point).
 *
 *  1 April 2019
 *  Created by team FUBAR
 *  CS 3505
 *  Professor Peter Jensen
 *  Do not distribute
 */

#include <iostream>

#include "network_controller/NetworkController.h"

int main()
{
  NetworkController networkController;
  networkController.start(2112);  // TODO: Change to correct port

  return 0;
}