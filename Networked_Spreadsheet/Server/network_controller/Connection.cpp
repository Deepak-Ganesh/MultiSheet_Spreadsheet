/** Contains function definitions for Connection.
 *
 *  1 April 2019
 *  Created by team FUBAR
 *  CS 3505
 *  Professor Peter Jensen
 *  Do not distribute
 */

#include <functional>
#include <iostream>

#include <nlohmann/json.hpp>

#include "Connection.h"
#include "Document.h"
#include "Server.h"
#include "buf_to_str.h"
#include <message_classes/ErrorMessage.h>
#include <message_classes/AdminUserListMessage.h>
#include <message_classes/AdminListMessage.h>

using asio::ip::tcp;
using namespace nlohmann;

std::shared_ptr<Connection> Connection::create(
  asio::ip::tcp::socket socket, Server & server)
{
  return std::make_shared<Connection>(std::move(socket), server);
}

Connection::Connection(asio::ip::tcp::socket socket, Server & server) :
  socket (std::move(socket)),
  server (server)
{

}

void Connection::start(size_t id)
{
  this->id = id;

  start_write_list();
}


void Connection::deliver(std::string const & message)
{
  // TODO: Remove this once we've tested the program enough.
  if (message.substr(message.size()-2) != "\n\n")
  {
    throw std::runtime_error(
      "Connection::deliver must be passed a \\n\\n delimited message");
  }

  shared_ptr<string> msg =
    make_shared<string>(message);
  weak_ptr<string> msg_weak = msg;


  auto self(shared_from_this());
  asio::async_write(
    socket,
    asio::buffer(
      msg->c_str(),
      msg->length()),
    [this, self, msg_weak](std::error_code ec, std::size_t length)
    {
      if (ec)
      {
		  if (document != nullptr)
		  {
			  document->leave(shared_from_this());
		  }
      }
    });
}



size_t Connection::get_id() const
{
  return id;
}



void Connection::start_write_list(std::string prependMessage)
{
  std::shared_ptr<Message> document_list =
    std::make_shared<Message>(prependMessage + server.get_documents().gen_list());

  // Weak ptr must be used so that the lambda doesn't self-reference (memory leak).
  // TODO: Make a helper method for this. E.g. pair<shared, weak_ptr> make_message(string).
  std::weak_ptr<Message> msg_weak = document_list;

  auto self(shared_from_this());
  asio::async_write(
    socket,
    asio::buffer(
      document_list->c_str(),
      document_list->length()),
    [this, self, msg_weak](std::error_code ec, std::size_t length)
    {
      if (!ec)
      {
        start_read_open();
      }
      else
      {
        handle_conn_error(ec);
      }
    });
}



void Connection::start_read_open()
{
  auto self(shared_from_this());
  asio::async_read_until(socket,
    buffer, "\n\n",
    [this, self](std::error_code ec, std::size_t bytes_transferred)
    {
      if (!ec)
      {

        std::string buffer_str {buf_to_str(buffer, bytes_transferred)};

        try
        {
          // TODO: Ignore message if this parsing fails.
          auto j = json::parse(buffer_str);

          // TODO: Verify correct message
          /*auto it_type = j.find("type");
          auto it_username = j.find("username");
          auto it_password = j.find("password");
          auto it_name = j.find("name");

          if (it_type == j.end())*/

          string username = j.at("username");
          string password = j.at("password");

          if (username == "admin" && password == "password")
          {
            start_write_admin_data();
            server.get_connections().assign_name(id, username);
            server.register_admin(shared_from_this());
            AdminListMessage adminMessage(server.get_documents().get_names());
            server.send_to_admin(adminMessage.get_json().dump());
            server.send_users();
          }

          else if (server.get_accounts().attempt_authentication(username, password)) // TODO: Make sure they convert to strings
          {
            server.get_connections().assign_name(id, username);
            document = &server.get_documents().get_or_create_document(j["name"]);
            document->join(shared_from_this());
            start_write_full_send();
			      server.send_users();
          }
          else
          {
            // TODO: Send "Error" message 1
            std::cout << "User authentication failed" << std::endl;
            start_write_list(ErrorMessage(1, "").toString());
          }
        }
        catch (std::exception const & e)
        {
          std::cout << "Failed to parse JSON message: " << e.what() << std::endl;
        }
      }
      else
      {
         handle_conn_error(ec);
      }
    });
}



void Connection::start_write_full_send()
{
  // Gets bound to the lambda, meaning its lifetime should last.
  // Must be a shared_ptr so it is not deleted after this method, and captured
  // in the lambda as a weak_ptr so it can be released.
  std::shared_ptr<Message> msg_shared =
    std::make_shared<Message>(document->gen_full_send());
  std::weak_ptr<Message> msg_weak (msg_shared);
  
  auto self(shared_from_this());
  asio::async_write(
    socket,
    asio::buffer(
      msg_shared->c_str(),
      msg_shared->length()),
    [this, self, msg_weak](std::error_code ec, std::size_t length)
    {
      if (!ec)
      {
        start_read_edits();
      }
      else
      {
        handle_conn_error(ec);
      }
    });
}


void Connection::start_read_edits()
{
  auto self(shared_from_this());
  asio::async_read_until(socket,
    buffer,
    "\n\n",
    [this, self](std::error_code ec, std::size_t bytes_transferred)
    {
      if (!ec)
      {
        std::string buffer_str {buf_to_str(buffer, bytes_transferred)};
        //std::cout << "Received " << buffer_str << std::endl;

        auto j = json::parse(buffer_str);
        
        // TODO: Read j message type, handle each appropriately.
        std::string type = j["type"];

        if (type == "edit")
        {
          document->edit(EditMessage(j), *this);
        }
        else if (type == "undo")
        {
          document->undo(*this);
        }
        else if (type == "revert")
        {
          document->revert(RevertMessage(j), *this);
        }

        // Repeat the main loop indefinitely.
        start_read_edits();
      }
      else
      {
        handle_conn_error(ec);
      }
    });
}




void Connection::start_write_error(int code)
{
  // TODO: Implement
}


void Connection::start_write_admin_data()
{
  //sends user list to admin
  server.send_users();

  //sends document list to admin
  AdminListMessage adminMessage(server.get_documents().get_names());
            server.send_to_admin(adminMessage.get_json().dump());

  json js;
  js["type"] = "Admin Update";
  js["documents"] = server.get_documents().serialize_names();
  js["users"] = server.get_accounts().serialize_names();
  js["connections"] = server.get_connections().serialize_names();

  std::shared_ptr<Message> msg_shared =
    std::make_shared<Message>(js.dump());
  std::weak_ptr<Message> msg_weak (msg_shared);
  
  auto self(shared_from_this());
  asio::async_write(
    socket,
    asio::buffer(
      msg_shared->c_str(),
      msg_shared->length()),
    [this, self, msg_weak](std::error_code ec, std::size_t length)
    {
      if (!ec)
      {
        start_read_admin_data();
      }
      else
      {
        handle_conn_error(ec);
      }
    });
}


void Connection::start_read_admin_data()
{
  auto self(shared_from_this());
  asio::async_read_until(socket,
    buffer,
    "\n\n",
    [this, self](std::error_code ec, std::size_t bytes_transferred)
    {
      if (!ec)
      {
        std::string buffer_str {buf_to_str(buffer, bytes_transferred)};

        auto j = json::parse(buffer_str);
        
        // TODO: Read j message type, handle each appropriately.
        std::string type = j["type"];

        if (type == "CRU") // Create User
        {
          server.accounts.add_account(j["username"], j["password"]);
        }
        else if (type == "DLU") // Delete User
        {
          server.accounts.delete_account(j["username"]);
        }
        else if (type == "CHP") // Change Password
        {
          server.accounts.change_password(j["username"], j["password"]);
        }
        else if (type == "RMS") // Remove Spreadsheet
        {
          server.documents.delete_document(j["name"]);
        }
        else if (type == "CRS") // Create Spreadsheet
        {
          server.documents.get_or_create_document(j["name"]);
        }
        else if (type == "SDS") // Shut Down Server
        {
          server.shut_down();

          exit(0);
        }

        // Repeat the main loop indefinitely.
        start_write_admin_data();
      }
      else
      {
        handle_conn_error(ec);
      }
    });
}



// TODO: Ensure no race conditions (since server::leave and document::leave could execute far apart).
void Connection::handle_conn_error(std::error_code ec)
{
    //std::cout << "Connection error code " << ec << std::endl;
    server.leave(shared_from_this());
    if (document != nullptr)
        document->leave(shared_from_this());
	  server.send_users();
}

Server & Connection::get_server()
{
  return this->server;
}

