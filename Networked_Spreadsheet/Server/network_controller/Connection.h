/** Contains the class declaration of Connection.
 *
 *  1 April 2019
 *  Created by team FUBAR
 *  CS 3505
 *  Professor Peter Jensen
 *  Do not distribute
 */

#ifndef SERVER_SOCKET_H
#define SERVER_SOCKET_H

#include <asio.hpp>

#include "Message.h"

class Document;
class Server;

/** A connection between the server and a single client
 *
 *  Based off class chat_session in boost_asio/example/cpp11/chat/chat_server.cpp
 */
class Connection :
  public std::enable_shared_from_this<Connection> // Required for nomadic state?
{
  public:
    typedef std::shared_ptr<Connection> Pointer;

    static std::shared_ptr<Connection> create(
      asio::ip::tcp::socket socket, Server & server);

    Connection(asio::ip::tcp::socket socket, Server & server);

    /** Begins the handshake then main loop.
     */
    void start(size_t id);

    /** Sends message to the client (with newlines added). */
    void deliver(std::string const & message);

    size_t get_id() const;

    Server & get_server();


  private:
    /** First part of the handshake, sends the Spreadsheet List message.
     * 
     * @param prependMessage Optional extra message to send before the list.
     *                       This is only used when a user fails authentication.
     */
    void start_write_list(std::string prependMessage = "");

    /** Second part of the handshake, waits for the client to send Open message.
     */
    void start_read_open();

    /** Third part of the handshake, sends the Full Send message.
     */
    void start_write_full_send();

    /** Main event loop, continuously listens for edit/undo/revert messages.
     */
    void start_read_edits();

    void start_write_error(int code);

    void start_write_admin_data();

    void start_read_admin_data();

    void handle_conn_error(std::error_code ec);

    asio::ip::tcp::socket socket;
    Document * document = nullptr;
    Server & server;
    //Message read_msg;
    std::vector<Message> write_msgs;
    asio::streambuf buffer;
    size_t id;
};

#endif //SERVER_SOCKET_H
