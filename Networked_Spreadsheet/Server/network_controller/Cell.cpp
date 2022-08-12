#include "Cell.h"

Cell::Cell()
{

}


Cell::Cell(string value, vector<string> dependencies) :
    value (value), dependencies (dependencies)
{
    
}

Cell::Cell(json const & js) :
    Cell(js.at("value"), js.at("dependencies"))
{
    
}


string Cell::get_value() const
{
    return value;
}


vector<string> & Cell::getDependencies()
{
  return dependencies;
}


set<string> Cell::getDependencySet() const
{
  return {dependencies.begin(), dependencies.end()};
}


json Cell::serialize() const
{
  json js;
  serialize_into(js);
  return js;
}


void Cell::serialize_into(json & js) const
{
  js["value"] = value;
  js["dependencies"] = dependencies;
}
