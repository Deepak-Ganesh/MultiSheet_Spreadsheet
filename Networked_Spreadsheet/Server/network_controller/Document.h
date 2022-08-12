/** Contains the class declaration of Document.
 *
 *  1 April 2019
 *  Created by team FUBAR
 *  CS 3505
 *  Professor Peter Jensen
 *  Do not distribute
 */

#ifndef SERVER_DOCUMENT_H
#define SERVER_DOCUMENT_H

#include <set>
#include <vector>
#include <map>
#include <string>
#include <thread>
#include <nlohmann/json.hpp>

#include <message_classes/EditMessage.h>
#include <message_classes/UndoMessage.h>
#include <message_classes/RevertMessage.h>

#include "ConnectionCollection.h"
#include "Message.h"
#include "CellChange.h"
#include "DependencyGraph.h"

/** Contains a list of connections, can receive messages.
 *
 *  Based off class chat_room in boost_asio/example/cpp11/chat/chat_server.cpp
 */
class Document
{
  public:
    void set_name(std::string);

    void join(Connection::Pointer connection);

    void leave(Connection::Pointer connection);

    void edit(EditMessage edit_message, Connection & conn);
    void undo(Connection & conn);
    void revert(RevertMessage revert_message, Connection & conn);

    void remove_all_connections();

    std::string gen_full_send();

    /** Creates a json object from this object's state.
     * The json object has "cells" and "changes" fields.
     * cells: {name: [{value, dependents}, ...], ...}
     * 
     * @return The json object.
     */
    nlohmann::json serialize();

    /** Sets the internal data to the json-serialized data.
     * The json object must have "cells" and "changes" fields.
     * 
     * @param js The json object to deserialize.
     */
    void deserialize(nlohmann::json & js);


  private:

    std::string name;

    std::mutex mut;
    ConnectionCollection connections;

    DependencyGraph dependencyGraph;

    std::map<std::string, vector<Cell>> cell_map;


    /** A list of cells that have been changed, with the last element being the most
     * recently-changed cell.
     */
    std::vector<CellChange> change_log;


    /** Sends a message to all clients
     */
    void deliver(std::string const & str);

    static nlohmann::json serialize(CellChange const & cell);

    nlohmann::json serialize_changes();

    std::vector<CellChange> deserialize_changes(nlohmann::json & js);
};

#endif //SERVER_DOCUMENT_H
