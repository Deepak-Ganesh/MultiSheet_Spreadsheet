/** Contains function definitions for Server.
 *
 *  16 April 2019
 *  Created by team FUBAR
 *  CS 3505
 *  Professor Peter Jensen
 *  Do not distribute
 */

#include "Server.h"

#include <iostream>
#include <fstream>
#include <nlohmann/json.hpp>
#include <message_classes/AdminUserListMessage.h>

using asio::ip::tcp;

/** Begins listening for clients immediately.
 * 
 * @param io_context The ASIO io_context to run on.
 * @param endpoint The host to connect to.
 */
Server::Server(
  asio::io_context& io_context,
  const tcp::endpoint& endpoint) : acceptor (io_context, endpoint)
{
  /*documents.get_or_create_document("test.sprd");
  documents.get_or_create_document("another_test.sprd");
  documents.get_or_create_document("1234567890897654312.sprd");
  documents.get_or_create_document("doc1");
  accounts.add_account("acc", "pass");
  std::ofstream of ("server.json");
  of << serialize();*/

  std::cout << "Server is running on port " << endpoint.port() << std::endl;
  start_accept();
}



/** Loads state from json then begins listening for clients.
 * 
 * @param io_context The ASIO io_context to run on.
 * @param endpoint The host to connect to.
 * @param js The json object to deserialize.
 */
Server::Server(
  asio::io_context& io_context,
  const tcp::endpoint& endpoint,
  nlohmann::json & js) : acceptor (io_context, endpoint)
{
  accounts.deserialize(js.at("accounts"));
  documents.deserialize(js.at("documents"));

  std::cout << "Server is running on port " << endpoint.port() << std::endl;
  start_accept();
}


Server::~Server()
{
  shut_down();
}



/** Returns a reference to the user accounts object. */
UserAccountCollection & Server::get_accounts()
{
  return accounts;
}



/** Returns a reference to the documents object. */
DocumentCollection & Server::get_documents()
{
  return documents;
}


ConnectionCollection & Server::get_connections()
{
  return connections;
}


void Server::save()
{
  std::ofstream of("server.json");
  of << serialize();
}


void Server::shut_down()
{
  documents.remove_all_connections();
  connections.remove_all_connections();

  std::cout << "Server is closing, saving state to server.json ..." << std::endl;
  save();
}


/** Removes a socket from the server and document.
 *
 * @param connection The connection being removed.
 */
void Server::leave(Connection::Pointer connection)
{
  connections.remove_connection(connection->get_id());
}



/** Returns a JSON string for the entire state of the server.
 * 
 * This is not for any messages, it's for saving the server.
 */
nlohmann::json Server::serialize()
{
  std::lock_guard<std::mutex> lg (mut);

  nlohmann::json js;

  js["accounts"] = accounts.serialize();
  js["documents"] = documents.serialize();

  return js;
}



/** Async method for accepting client connections.
 *  Continuously calls itself within a new thread.
 */
void Server::start_accept()
{
  // It's easiest to use lambdas since 'this' must be passed.
  acceptor.async_accept(
    [this](std::error_code ec, tcp::socket socket)
    {
      if (ec)
      {
        // Always use std::endl within async methods.
        std::cout << "Failed to accept client" << std::endl;
      }
      else
      {
        std::cout << "Accepted client" << std::endl;

        //std::make_shared<Connection>(std::move(socket), document)->start();
        auto conn = Connection::create(std::move(socket), *this); // TODO: Test this works
        auto id = connections.add_connection(conn);
        conn->start(id);
        send_users();
        
      }

      start_accept();
    });
}

void Server::send_users()
{
  std::lock_guard<std::mutex> lg (mut);
  map<std::string, bool> users;
  for(std::string user : get_accounts().get_names())
    {
      if (get_connections().contains_name(user))
        users.emplace(user, true);
      else
        {
          users.emplace(user, false);
        }
    }
  AdminUserListMessage adminMessage(users);
  send_to_admin_unlocked(adminMessage.get_json().dump());
}

void Server::register_admin(Connection::Pointer admin)
{
  std::lock_guard<std::mutex> lg (mut);
  this->admin = admin;
}

void Server::send_to_admin(std::string message)
{
  std::lock_guard<std::mutex> lg (mut);

  send_to_admin_unlocked(message);
}



void Server::send_to_admin_unlocked(std::string str)
{
  if (admin == NULL)
    return;

  //std::cout << "Sent to admin: " << str << std::endl;
  admin->deliver(str + "\n\n");
}


