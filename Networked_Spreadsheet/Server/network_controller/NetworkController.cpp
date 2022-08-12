/** Contains function definitions for NetworkController.
 *
 *  1 April 2019
 *  Created by team FUBAR
 *  CS 3505
 *  Professor Peter Jensen
 *  Do not distribute
 */

#include <iostream>
#include <string>
#include <fstream>
#include <nlohmann/json.hpp>

#include "NetworkController.h"

using asio::ip::tcp;

NetworkController::NetworkController()
{

}



void NetworkController::start(unsigned short port)
{
  try
  {
    std::ifstream f ("server.json");

    asio::io_context io_ctx;

    if (f.good())
    {
      std::cout << "Loading server state from server.json" << std::endl;
      nlohmann::json js;
      js << f;

      Server server (io_ctx, tcp::endpoint(tcp::v4(), port), js);
      io_ctx.run();
    }
    else
    {
      std::cout << "Could not open server.json, creating new server" << std::endl;

      Server server (io_ctx, tcp::endpoint(tcp::v4(), port));
      io_ctx.run();
    }
    

  }
  catch (std::exception& e)
  {
    std::cerr << e.what() << std::endl;
  }
}
