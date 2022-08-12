/** Contains the class declaration of DocumentCollection.
 *
 *  1 April 2019
 *  Created by team FUBAR
 *  CS 3505
 *  Professor Peter Jensen
 *  Do not distribute
 */

#ifndef SERVER_CONNECTION_COLLECTION_H
#define SERVER_CONNECTION_COLLECTION_H

#include <map>
#include <thread>
#include <string>
#include <vector>
#include <nlohmann/json.hpp>

#include "Connection.h"

using namespace std;
using json = nlohmann::json;

/** Contains a map of connection shared pointers.
 */
class ConnectionCollection
{
  public:
    /** Adds a connection and assigns it an ID.
     * 
     * @return The connection's ID.
     */
    size_t add_connection(Connection::Pointer ptr);

    bool remove_connection(size_t id);

    void remove_all_connections();

    bool assign_name(size_t id, string const & name);

    void deliver(string const & message);

    json serialize_names();

    std::string get_name(size_t ID);

    bool contains_name(string);


  private:
    mutex mut;

    /** The ID of the next connection added. 0 is reserved for errors. */
    size_t nextId = 1;

    /** Maps connection ID to connection object. */
    map<size_t, Connection::Pointer> connections;

    /** Maps connection ID to user name. */
    map<size_t, string> names;

    vector<string> name_list;
    
};

#endif //SERVER_DOCUMENT_COLLECTION_H
