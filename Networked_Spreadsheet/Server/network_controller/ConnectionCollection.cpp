/** Contains function definitions for DocumentCollection.
 *
 *  1 April 2019
 *  Created by team FUBAR
 *  CS 3505
 *  Professor Peter Jensen
 *  Do not distribute
 */

#include <iostream>

#include "ConnectionCollection.h"

size_t ConnectionCollection::add_connection(Connection::Pointer ptr)
{
  lock_guard<mutex> lg {mut};
  connections.emplace(nextId, ptr);

  return nextId++;  // Post-increment
}


bool ConnectionCollection::remove_connection(size_t id)
{
  lock_guard<mutex> lg {mut};
  
  auto it = connections.find(id);
  
  if (it == connections.end())
    return false;  // Not found

  else
  {
    connections.erase(it);
	names.erase(id);
    return true;
  }
  
}


void ConnectionCollection::remove_all_connections()
{
  lock_guard<mutex> lg{ mut };

  connections = {};
}


bool ConnectionCollection::assign_name(size_t id, string const & name)
{
  lock_guard<mutex> lg {mut};

  auto it = connections.find(id);
  if (it == connections.end())
    return false;  // Not found

  else
  {
    name_list.push_back(name);
    names.emplace(id, name);
    return true;
  }
}


void ConnectionCollection::deliver(string const & message)
{
  lock_guard<mutex> lg {mut};

  for (auto & kvPair : connections)
  {
    kvPair.second->deliver(message);
  }
}


json ConnectionCollection::serialize_names()
{
  lock_guard<mutex> lg {mut};

  json js = json::array_t();

  for (auto & kvPair : names)
  {
    js.push_back(kvPair.second);
  }

  return js;
}

std::string ConnectionCollection::get_name(size_t ID)
{
  auto it = names.find(ID);
  if (it == names.end())
    return "-1";  // Not found
  return names[ID];
}

bool ConnectionCollection::contains_name(string s)
{
	for (auto &i : names)
		if (i.second == s)
			return true;
	return false;
}
