/** Contains function definitions for Document.
 *
 *  1 April 2019
 *  Created by team FUBAR
 *  CS 3505
 *  Professor Peter Jensen
 *  Do not distribute
 */

#include <iostream>
#include <exception>

#include <nlohmann/json.hpp>

#include "Document.h"
#include "Server.h"

#include <message_classes/FullSendMessage.h>
#include <message_classes/ErrorMessage.h>
#include <message_classes/AdminEditOverviewMessage.h>


using namespace nlohmann;

void Document::set_name(std::string name)
{
  this->name = name;
}

void Document::join(Connection::Pointer connection)
{
  connections.add_connection(connection);
}

void Document::leave(Connection::Pointer connection)
{
  std::cout << "Connection left document" << std::endl;
  connections.remove_connection(connection->get_id());
}


void Document::edit(EditMessage edit_message, Connection & conn)
{
  lock_guard<mutex> lg (mut); // TODO: ensure no dead locks

  Cell cell (
    edit_message.getValue(),
    edit_message.getDependencies()
  );

  string name = edit_message.getCell();
  auto & edit_list = cell_map[edit_message.getCell()];

  Cell prevCell;
  string prevValue;
  set<string> prevDependencies;
  
  if (!edit_list.empty())
  {
    prevCell = edit_list.back();
    prevValue = edit_list.back().get_value();
    prevDependencies = edit_list.back().getDependencySet();
  }

  cell_map[name].push_back(cell);
  set<string> nameSet (cell.getDependencySet());
  dependencyGraph.replaceDependents(name, nameSet);

  try
  {
    dependencyGraph.GetCellsToRecalculate(name);

    change_log.push_back({name, prevCell, false});

    map<string, string> cellsToSend;
    cellsToSend.emplace(name, edit_message.getValue());
    deliver(FullSendMessage(cellsToSend).toString());

    string adminM = "edit: " + name + " = \"" + edit_message.getValue() + '"';
    AdminEditOverviewMessage adminMessage(this->name, 
      conn.get_server().get_connections().get_name(conn.get_id()), 
      adminM);
    conn.get_server().send_to_admin(adminMessage.get_json().dump());
  }
  catch (CircularException const & ex)
  {
    dependencyGraph.replaceDependents(name, prevDependencies);
    conn.deliver(ErrorMessage(2, name).toString());
  }
}

/**
The udno undos  the  last change to the entire spreadsheet
*/
void Document::undo(Connection & conn)
{
  lock_guard<mutex> lg (mut); // TODO: ensure no dead locks

  // Do nothing if the change log is empty.
  if (!change_log.empty())
  {
    CellChange change = change_log.back();

    auto name = change.get_name();
    vector<Cell> & edit_list = cell_map.at(name);
    //Cell edit = edit_list.back();

    Cell new_cell = change.get_previous_value();

    set<string> nameSet (new_cell.getDependencySet());
    try
    {
      dependencyGraph.replaceDependents(name, nameSet);

      //auto dependents = dependencyGraph.getDependents(name);
      dependencyGraph.GetCellsToRecalculate(name); // TODO: Remove? Should never throw

      // If the previous change was a revert, put the value back on the edit list.
      if (change.get_is_revert())
      {
        edit_list.push_back(change.get_previous_value());
      }
      // Otherwise remove the most recent edit.
      else
      {
        if (!edit_list.empty())
          edit_list.pop_back();
      }

      change_log.pop_back();
      //edit_list.pop_back();
      map<string, string> cellsToSend;
      cellsToSend.emplace(name, new_cell.get_value());
      deliver(FullSendMessage(cellsToSend).toString());

      string adminM = "undo: " + name + " = \"" + new_cell.get_value() + '"';
      AdminEditOverviewMessage adminMessage(this->name, 
        conn.get_server().get_connections().get_name(conn.get_id()), 
        adminM);
      conn.get_server().send_to_admin(adminMessage.get_json().dump());
    }
    catch(CircularException const & e)
    {
      throw std::runtime_error("Undo should never have a circular dependency");
    }
  }
}


void Document::revert(RevertMessage revert_message, Connection & conn)
{
  lock_guard<mutex> lg (mut); // TODO: ensure no dead locks

  string name = revert_message.getCell();

  auto edit_list_it = cell_map.find(name);

  // If it exists ... else do nothing
  if (edit_list_it != cell_map.end())
  {
    auto & edit_list = edit_list_it->second;
    auto size = edit_list.size();

    // Do nothing if empty.
    if (size > 0)
    {
      Cell prev_cell = edit_list.back();
      // Keep as an empty cell if the size is 1
      Cell new_cell;

      if (size > 1)
      {
        new_cell = edit_list[size - 2];
      }

      set<string> prevDependencies (edit_list.back().getDependencySet());
      set<string> nameSet (new_cell.getDependencySet());

      try
      {
        dependencyGraph.replaceDependents(name, nameSet);
        dependencyGraph.GetCellsToRecalculate(name);

        change_log.push_back({name, prev_cell, true});

        edit_list.pop_back();
        map<string, string> cellsToSend;
        cellsToSend.emplace(name, new_cell.get_value());
        deliver(FullSendMessage(cellsToSend).toString());

        string adminM = "revert: " + name + " = \"" + new_cell.get_value() + '"';
          AdminEditOverviewMessage adminMessage(this->name,
          conn.get_server().get_connections().get_name(conn.get_id()),
          adminM);
        conn.get_server().send_to_admin(adminMessage.get_json().dump());
      }
      catch(CircularException const & e)
      {
        // Rollback Is this necessary or does the graph rollback automatically? yes its necessary
        dependencyGraph.replaceDependents(name, prevDependencies);

        // Deliver error just to the client that sent the revert
        conn.deliver(ErrorMessage(2, name).toString());
      }

    }
  }
}

void Document::deliver(std::string const & message)
{
  //std::cout << "Sent " << message << std::endl;
  connections.deliver(message);
}


void Document::remove_all_connections()
{
  connections.remove_all_connections();
}


std::string Document::gen_full_send()
{
  lock_guard<mutex> lg (mut);

  json obj;

  obj["type"] = "full send";

  json cellMap;

  for (auto & i : cell_map)
  {
    // Map the name to the most recent value.
	if (!i.second.empty())
		cellMap[i.first] = i.second.back().get_value();
  }

  obj["spreadsheet"] = json::object_t();
  if (!cellMap.empty())
    obj["spreadsheet"] = cellMap;

  return obj.dump();
}


// TODO: Ensure this works with whatever change log implementation we decide on.
json Document::serialize()
{
  lock_guard<mutex> lg (mut);

  json js;

  json cellMap = json::object_t();

  for (auto & i : cell_map)
  {
    std::vector<json> jsVec;
    for (auto & j : i.second)
    {
      jsVec.push_back(j.serialize());
    }
    cellMap[i.first] = jsVec;
  }

  js["cells"] = cellMap;
  js["changes"] = serialize_changes();

  return js;
}


// TODO: Ensure this works with whatever change log implementation we decide on.
void Document::deserialize(nlohmann::json & js)
{
  lock_guard<mutex> lg (mut);
  
  auto jsMap = js.at("cells").get<std::map<std::string, json>>();

  for (auto & i : jsMap)
  {
    vector<Cell> cellVec;
    for (auto & j : i.second.get<json::array_t>())
    {
      cellVec.push_back(Cell(j));
    }

    cell_map[i.first] = cellVec;
  }

  change_log = deserialize_changes(js.at("changes"));
}


json Document::serialize_changes()
{
  json js = json::array();

  for (auto & i : change_log)
  {
    /*json temp;
    temp[i.get_name()] = i.get_previous_value().serialize();*/
    js.push_back(i.serialize());
  }

  return js;
}


std::vector<CellChange> Document::deserialize_changes(json & js)
{
  std::vector<CellChange> vec;
  for (auto & i : js.get<json::array_t>())
  {
    vec.push_back(CellChange(i));
  }

  return vec;
}