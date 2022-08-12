#include "SpreadsheetListMessage.h"

/**
* Default construtor for JSON
*/
SpreadsheetListMessage::SpreadsheetListMessage()
{
    //Don't know if this kind of JSON needs this, but better safe than sorry.
}

/**
* Copy Constructor. Takes in a json object of an SpreadsheetListMessage and returns an SpreadsheetListMessage.
* I didn't write this though, and so maybe it's slightly different than I think.
*/
SpreadsheetListMessage::SpreadsheetListMessage(json const & js)
{
    type = "list";

    spreadsheets = js["spreadsheets"].get<vector<string>>();
}

/**
* Constructor that takes in a cell name (to be edited), 
* the new value of the cell, and a list of dependencies for that cell.
*/
SpreadsheetListMessage::SpreadsheetListMessage(vector<string> _spreadsheets)
{
    type = "list";

    spreadsheets = _spreadsheets;
}

/**
* This names the JSON properties, sets them, and instantiates a JSON object with those fields.
* This method is private, as it will be called by toString, which both creates the JSON output and adds the needed
* "\n\n".
*/
json SpreadsheetListMessage::getJson()
{
    json j;
    j["type"]=type;
    j["spreadsheets"]= spreadsheets;
    
    return j;
}

/**
* This takes an SpreadsheetListMessage and serializes it into a JSON object, which is then dumped into a string and "\n\n" added.
*/
string SpreadsheetListMessage::toString()
{
    json jObject = this -> getJson();
    string out = jObject.dump();
    out += "\n\n";

    return out;
}

//Getters after this point.

string SpreadsheetListMessage::getType()
{
    return type;
}

vector<string> SpreadsheetListMessage::getSpreadsheets()
{
    return spreadsheets;
}