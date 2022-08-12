#ifndef SERVER_SAVE_SERVER_H
#define SERVER_SAVE_SERVER_H

#include <fstream>

#include "Server.h"


// TODO: implement
/** Saves all of a server's data to a file.
 */
void save_server(std::string file_name, Server const & server);


#endif