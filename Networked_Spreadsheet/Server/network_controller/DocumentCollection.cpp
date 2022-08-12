/** Contains function definitions for DocumentCollection.
 *
 *  1 April 2019
 *  Created by team FUBAR
 *  CS 3505
 *  Professor Peter Jensen
 *  Do not distribute
 */

#include <iostream>

#include <nlohmann/json.hpp>

#include "DocumentCollection.h"

Document & DocumentCollection::get_or_create_document(std::string name)
{
  std::lock_guard<std::mutex> lg {mut};

  auto it = document_map.find(name);

  //if (it == document_map.end())
  //  document_map[name].set_name(name);

  document_map[name].set_name(name);
  return document_map[name];
}

/** Remove the document if it exists, returning true.
 * 
 * @param name The name of the spreadsheet.
 * 
 * @return True if the document previously existed.
 */
bool DocumentCollection::delete_document(std::string name)
{
  std::lock_guard<std::mutex> lg {mut};

  std::map<std::string, Document>::iterator it = document_map.find(name);

  if (it == document_map.end())
  {
    return false;
  }
  else 
  {
    document_map.erase(it);
    return true;
  }
}

std::string DocumentCollection::gen_list()
{
  std::lock_guard<std::mutex> lg (mut);

  nlohmann::json obj;
  obj["type"] = "list";

  std::vector<std::string> v;
  for (auto const & i : document_map)
  {
    v.push_back(i.first);
  }

  obj["spreadsheets"] = v;
  
  return obj.dump();
}


nlohmann::json DocumentCollection::serialize()
{
  std::lock_guard<std::mutex> lg (mut);

  nlohmann::json js = json::object();

  for (auto & it : document_map)
  {
    js[it.first] = it.second.serialize();
  }

  return js;
}


void DocumentCollection::deserialize(nlohmann::json & js)
{
  std::lock_guard<std::mutex> lg (mut);

  for (auto it = js.begin(); it != js.end(); ++it)
  {
    document_map[it.key()].deserialize(it.value());
  }
}


json DocumentCollection::serialize_names()
{
  std::lock_guard<std::mutex> lg (mut);
 
  json result = json::array_t();
  for (auto & i : document_map)
  {
    result.push_back(i.first);
  }

  return result;
}

std::vector<std::string> DocumentCollection::get_names()
{
  std::lock_guard<std::mutex> lg (mut);
  std::vector<std::string> names;

  for (auto & i : document_map)
  {
    names.push_back(i.first);
  }

  return names;
}


void DocumentCollection::remove_all_connections()
{
  for (auto & i : document_map)
  {
    i.second.remove_all_connections();
  }
}