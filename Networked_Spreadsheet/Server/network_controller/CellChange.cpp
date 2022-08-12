#include "CellChange.h"

CellChange::CellChange()
{

}


CellChange::CellChange(string name, Cell previous_value, bool is_revert) :
  name (name), previous_value (previous_value), is_revert(is_revert)
{
    
}

CellChange::CellChange(json const & js) :
  CellChange(
    js.at("name"),
    Cell(js.at("previous_value")),
    js.at("is_revert").get<bool>())
{
    
}


string CellChange::get_name() const
{
  return name;
}


Cell CellChange::get_previous_value() const
{
  return previous_value;
}


bool CellChange::get_is_revert() const
{
  return is_revert;
}


json CellChange::serialize() const
{
  json js;
  
  js["name"] = name;
  js["previous_value"] = previous_value.serialize();
  js["is_revert"] = is_revert;

  return js;
}
