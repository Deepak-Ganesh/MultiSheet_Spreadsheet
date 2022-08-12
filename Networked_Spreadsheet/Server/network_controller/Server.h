/** Contains the class declaration of Server.
 *
 *  16 April 2019
 *  Created by team FUBAR
 *  CS 3505
 *  Professor Peter Jensen
 *  Do not distribute
 */

#ifndef SERVER_SERVER_H
#define SERVER_SERVER_H

#include <asio.hpp>
#include <thread>
#include <nlohmann/json.hpp>

#include "ConnectionCollection.h"
#include "DocumentCollection.h"
#include "UserAccountCollection.h"

/** Contains the state of a server, can accept clients.
 *
 *  Based off class chat_server in boost_asio/example/cpp11/chat/chat_server.cpp
 */
class Server
{
  public:
    /** Begins listening for clients immediately.
     * 
     * @param io_context The ASIO io_context to run on.
     * @param endpoint The host to connect to.
     */
    Server(
      asio::io_context& io_context,
      const asio::ip::tcp::endpoint& endpoint);

    /** Loads state from json then begins listening for clients.
     * 
     * @param io_context The ASIO io_context to run on.
     * @param endpoint The host to connect to.
     * @param js The json object to deserialize.
     */
    Server(
      asio::io_context& io_context,
      const asio::ip::tcp::endpoint& endpoint,
      nlohmann::json & js);

    ~Server();

    /** Returns a reference to the user accounts object. */
    UserAccountCollection & get_accounts();

    /** Returns a reference to the documents object. */
    DocumentCollection & get_documents();

    ConnectionCollection & get_connections();

    void save();

    void shut_down();

    /** Removes a socket from the server and document.
     *
     * @param connection The connection being removed.
     */
    void leave(Connection::Pointer connection);

    /** Returns a JSON string for the entire state of the server.
     * 
     * This is not for any messages, it's for saving the server.
     */
    nlohmann::json serialize();

    void register_admin(Connection::Pointer admin);
    void send_to_admin(std::string);
    void send_users();

  private:
	friend class Connection;

	void send_to_admin_unlocked(std::string str);

    /** Async method for accepting client connections.
     *  Continuously calls itself within a new thread.
     */
    void start_accept();

    /** Mutex for thread safety */
    std::mutex mut;
    
    /** Used for accepting clients. */
    asio::ip::tcp::acceptor acceptor;

    /** Contains all connections, not publicly available. */
    ConnectionCollection connections;

    /** Contains all documents, is publicy available. */
    DocumentCollection documents;

    /** Contains all account information, is publicly available. */
    UserAccountCollection accounts;

    //Pointer to admin connection
    Connection::Pointer admin;
};

#endif //SERVER_SERVER_H
